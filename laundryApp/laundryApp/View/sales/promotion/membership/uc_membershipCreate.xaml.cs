using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace laundryApp.View.sales.promotion.membership
{
    /// <summary>
    /// Interaction logic for uc_membershipCreate.xaml
    /// </summary>
    public partial class uc_membershipCreate : UserControl
    {
        public uc_membershipCreate()
        {
            try
            {
                InitializeComponent();
                if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1440)
                {
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;
                }
                else if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1360)
                {
                    txt_add_Icon.Visibility = Visibility.Collapsed;
                    txt_update_Icon.Visibility = Visibility.Collapsed;
                    txt_delete_Icon.Visibility = Visibility.Collapsed;
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;

                }
                else
                {
                    txt_deleteButton.Visibility = Visibility.Collapsed;
                    txt_addButton.Visibility = Visibility.Collapsed;
                    txt_updateButton.Visibility = Visibility.Collapsed;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private static uc_membershipCreate _instance;
        public static uc_membershipCreate Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_membershipCreate();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string basicsPermission = "membershipCreate_basics";
        string subscriptionFeesPermission = "membershipCreate_subscriptionFees";
        Memberships membership = new Memberships();
        IEnumerable<Memberships> membershipsQuery;
        IEnumerable<Memberships> memberships;
        //byte tgl_agentState;
        string _subscriptionType = "";
        string searchText = "";
        byte tgl_membershipState = 1;
        public static List<string> requiredControlList;

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                
                requiredControlList = new List<string> { "code" , "name" , "subscriptionType" };

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                #endregion

                FillCombo.fillSubscriptionType(cb_subscriptionType);

                Keyboard.Focus(tb_code);
                
                await Search();
                
                Clear();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translate()
        {
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            //txt_title.Text = AppSettings.resourcemanager.GetString("trMembership");
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_contentInformatin.Text = AppSettings.resourcemanager.GetString("trDetails");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_code, AppSettings.resourcemanager.GetString("trCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_price, AppSettings.resourcemanager.GetString("trPriceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_subscriptionType, AppSettings.resourcemanager.GetString("trSubscriptionType")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            dg_membership.Columns[0].Header = AppSettings.resourcemanager.GetString("trCode");
            dg_membership.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_membership.Columns[2].Header = AppSettings.resourcemanager.GetString("trSubscriptionType");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            btn_subscriptionFees.Content = AppSettings.resourcemanager.GetString("trSubscriptionFees");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }

        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    //chk code 
                    bool isCodeExist = false;
                    if(memberships.Any(m => m.code == tb_code.Text))
                        isCodeExist = true;
                    if (isCodeExist)
                        HelpClass.SetValidate(p_error_code, "trErrorDuplicateCode");
                    
                    else
                    {
                        membership = new Memberships();
                        if (HelpClass.validate(requiredControlList, this) )
                        {
                            membership.code = tb_code.Text;
                            membership.name = tb_name.Text;
                            membership.subscriptionType = _subscriptionType;
                            membership.createUserId = MainWindow.userLogin.userId;
                            membership.updateUserId = MainWindow.userLogin.userId;
                            membership.notes = tb_notes.Text;
                            membership.isActive = 1;
                            try
                            { membership.subscriptionFee = decimal.Parse(tb_price.Text); }
                            catch { membership.subscriptionFee = 0; }
                            membership.isFreeDelivery = false;
                            membership.deliveryDiscountPercent = 0;

                            int s = await membership.SaveMemberAndSub(membership);
                            if (s <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                Clear();
                                await RefreshMembershipsList();
                                await Search();
                            }
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (membership.membershipId > 0)
                    {
                        //chk code 
                        bool isCodeExist = false;
                        if (memberships.Any(m => m.code == tb_code.Text && m.membershipId != membership.membershipId))
                            isCodeExist = true;
                        if (isCodeExist)
                            HelpClass.SetValidate(p_error_code, "trErrorDuplicateCode");
                        else
                        {
                            if (HelpClass.validate(requiredControlList, this))
                            {
                                membership.code = tb_code.Text;
                                membership.name = tb_name.Text;
                                membership.subscriptionType = _subscriptionType;
                                membership.updateUserId = MainWindow.userLogin.userId;
                                membership.notes = tb_notes.Text;
                                membership.isActive = 1;
                                try
                                { membership.subscriptionFee = decimal.Parse(tb_price.Text); }
                                catch { membership.subscriptionFee = 0; }
                                membership.isFreeDelivery = false;
                                membership.deliveryDiscountPercent = 0;

                                int s = await membership.SaveMemberAndSub(membership);
                                if (s <= 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                                    await RefreshMembershipsList();
                                    await Search();
                                }
                            }

                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (membership.membershipId != 0)
                    {
                        if ((!membership.canDelete) && (membership.isActive == 0))
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxActivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                                await activate();
                        }
                        else
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            if (membership.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!membership.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (membership.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!membership.canDelete) && (membership.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await membership.delete(membership.membershipId, MainWindow.userLogin.userId, membership.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshMembershipsList();
                                    await Search();
                                    Clear();
                                }
                            }
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async Task activate()
        {//activate

            membership.isActive = 1;
            int s = await membership.save(membership);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshMembershipsList();
                await Search();
            }

        }
        #endregion

        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        
        private async void Tgl_isActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (memberships is null)
                    await RefreshMembershipsList();

                tgl_membershipState = 1;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Tgl_isActive_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (memberships is null)
                    await RefreshMembershipsList();

                tgl_membershipState = 0;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dg_membership_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//selection
            try
            {
                HelpClass.StartAwait(grid_main);
                
                if (dg_membership.SelectedIndex != -1)
                {
                    membership = dg_membership.SelectedItem as Memberships;
                    this.DataContext = membership;
                    if (membership != null)
                    {
                        #region delete
                        if (membership.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (membership.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                tb_search.Text = "";
                searchText = "";
                await RefreshMembershipsList();
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        #region Refresh & Search

        private string getSubscriptionType(string s)
        {
            string value = "";
           
            if(s.Equals("f"))          value = AppSettings.resourcemanager.GetString("trFree");
            else if (s.Equals("m"))    value = AppSettings.resourcemanager.GetString("trMonthly");
            else if (s.Equals("y"))    value = AppSettings.resourcemanager.GetString("trYearly");
            else if (s.Equals("o"))    value = AppSettings.resourcemanager.GetString("trOnce");

            return value;
        }
        async Task Search()
        {
            if (memberships is null)
                await RefreshMembershipsList();

            searchText = tb_search.Text.ToLower();
            membershipsQuery = memberships
                .Where(s => (
            s.code.ToLower().Contains(searchText) ||
            s.name.ToLower().Contains(searchText) 
            ||
            getSubscriptionType(s.subscriptionType).Contains(searchText)
            )
            && s.isActive == tgl_membershipState
            )
            ;

            RefreshMembershipsView();
        }
        async Task<IEnumerable<Memberships>> RefreshMembershipsList()
        {
            memberships = await membership.GetAll();
            return memberships;
        }
        void RefreshMembershipsView()
        {
            dg_membership.ItemsSource = membershipsQuery;
            txt_count.Text = membershipsQuery.Count().ToString();
        }
        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Memberships();
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only  digits
            try
            {
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
                else if (textBox.Tag.ToString() == "decimal")
                {
                    input = e.Text;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only english and digits
            try
            {
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        #region reports
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string firstTitle = "promotion";
            string secondTitle = "memberships";
            string subTitle = "";
            string Title = "";

            string addpath = "";
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {

                addpath = @"\Reports\Sale\Ar\ArMembership.rdlc";
                //   secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            }
            else
            {
                //english

                addpath = @"\Reports\Sale\En\EnMembership.rdlc";
                //  secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);


            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //  getpuritemcount
            Title = AppSettings.resourcemanagerreport.GetString("trSales") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));


            clsReports.membershipReport(membershipsQuery, rep, reppath, paramarr);

            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    /////////////////////////////////////
                    //Thread t1 = new Thread(() =>
                    //{
                        pdf();
                    //});
                    //t1.Start();
                    //////////////////////////////////////
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    /////////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        print();
                    });
                    t1.Start();
                    //////////////////////////////////////

                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvcSales win = new win_lvcSales(membershipsQuery, 4);
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();
                    this.Dispatcher.Invoke(() =>
                    {
                        saveFileDialog.Filter = "EXCEL|*.xls;";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filepath = saveFileDialog.FileName;
                            LocalReportExtensions.ExportToExcel(rep, filepath);
                        }
                    });


                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    string pdfpath = "";



                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    BuildReport();

                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    wd_previewPdf w = new wd_previewPdf();
                    w.pdfPath = pdfpath;
                    if (!string.IsNullOrEmpty(w.pdfPath))
                    {
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();
                    }
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public async void pdf()
        {
            BuildReport();
            this.Dispatcher.Invoke(() =>
            {
                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
            });
        }

        public async void print()
        {
            BuildReport();
            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
            });
        }

      
        #endregion

        #region barcode
        /*List<ItemUnit> barcodesList;
        static private int _InternalCounter = 0;
        private Boolean checkBarcodeValidity(string barcode)
        {
            if (FillCombo.itemUnitList != null)
            {
                var exist = FillCombo.itemUnitList.Where(x => x.barcode == barcode && x.itemId != item.itemId).FirstOrDefault();
                if (exist != null)
                    return false;
                else
                    return true;
            }
            return true;
        }
        */
        private void Tb_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            try
            {
                TextBox tb = (TextBox)sender;
                string barCode = tb_barcode.Text;
                if (e.Key == Key.Return && barCode.Length == 13)
                {
                    if (isBarcodeCorrect(barCode) == false)
                    {
                        item.barcode = "";
                        this.DataContext = item;

                    }
                    else
                        drawBarcode(barCode);
                }
                else if (barCode.Length == 13 || barCode.Length == 12)
                    drawBarcode(barCode);
                else
                    drawBarcode("");
            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this);
            }
            */
        }


        /*
        private bool isBarcodeCorrect(string barCode)
        {
            char checkDigit;
            char[] barcodeData;

            char cd = barCode[0];
            barCode = barCode.Substring(1);
            barcodeData = barCode.ToCharArray();
            checkDigit = Mod10CheckDigit(barcodeData);

            if (checkDigit != cd)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorBarcodeToolTip"), animation: ToasterAnimation.FadeIn);
                return false;
            }
            else
                return true;
        }
        public static char Mod10CheckDigit(char[] data)
        {
            // Start the checksum calculation from the right most position.
            int factor = 3;
            int weight = 0;
            int length = data.Length;

            for (int i = 0; i <= length - 1; i++)
            {
                weight += (data[i] - '0') * factor;
                factor = (factor == 3) ? 1 : 3;
            }

            return (char)(((10 - (weight % 10)) % 10) + '0');

        }
        private void drawBarcode(string barcodeStr)
        {
            try
            {
                // configur check sum metrics
                BarcodeSymbology s = BarcodeSymbology.CodeEan13;

                BarcodeDraw drawObject = BarcodeDrawFactory.GetSymbology(s);

                BarcodeMetrics barcodeMetrics = drawObject.GetDefaultMetrics(60);
                barcodeMetrics.Scale = 2;

                if (barcodeStr != "")
                {
                    if (barcodeStr.Length == 13)
                        barcodeStr = barcodeStr.Substring(1);//remove check sum from barcode string
                    var barcodeImage = drawObject.Draw(barcodeStr, barcodeMetrics);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();

                        img_barcode.Source = ImageProcess.ByteToImage(imageBytes);
                    }
                }
                else
                    img_barcode.Source = null;
            }
            catch { img_barcode.Source = null; }

        }
        private async void generateBarcode()
        {
            string barcodeString = "";
            barcodeString = generateRandomBarcode();
            if (FillCombo.itemUnitList != null)
            {
                if (!checkBarcodeValidity(barcodeString))
                    barcodeString = generateRandomBarcode();
            }
            item.barcode = barcodeString;
            this.DataContext = item;
            //tb_barcode.Text = barcodeString;
            HelpClass.validateEmpty("trErrorEmptyBarcodeToolTip", p_error_barcode);
            drawBarcode(tb_barcode.Text);
        }
        static public string generateRandomBarcode()
        {
            var now = DateTime.Now;

            var days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
            var seconds = (int)(now - DateTime.Today).TotalSeconds;

            var counter = _InternalCounter++ % 100;
            string randomBarcode = days.ToString("00000") + seconds.ToString("00000") + counter.ToString("00");
            char[] barcodeData = randomBarcode.ToCharArray();
            char checkDigit = Mod10CheckDigit(barcodeData);
            return checkDigit + randomBarcode;

        }
        */
        #endregion

        private void Btn_subscriptionFees_Click(object sender, RoutedEventArgs e)
        {// subscriptionFee
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(subscriptionFeesPermission, FillCombo.groupObjects, "one"))
                {
                    Window.GetWindow(this).Opacity = 0.2;

                    wd_subscriptionFees w = new wd_subscriptionFees();

                    w.memberShipID = membership.membershipId;
                    w.memberShipType = membership.subscriptionType;
                    w.ShowDialog();
                    //if (w.isActive)
                    //{
                    //}

                    Window.GetWindow(this).Opacity = 1;
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_subscriptionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select subscription
            try
            {
                switch(cb_subscriptionType.SelectedValue)
                {
                    case "f":
                        _subscriptionType = "f";
                        bdr_price.Visibility = Visibility.Collapsed;
                        btn_subscriptionFees.IsEnabled = false;
                        requiredControlList = new List<string> { "code", "name", "subscriptionType" };
                        break;
                    case "m":
                        _subscriptionType = "m";
                        bdr_price.Visibility = Visibility.Collapsed;
                        btn_subscriptionFees.IsEnabled = true;
                        requiredControlList = new List<string> { "code", "name", "subscriptionType" };
                        break;
                    case "y":
                        _subscriptionType = "y";
                        bdr_price.Visibility = Visibility.Collapsed;
                        btn_subscriptionFees.IsEnabled = true;
                        requiredControlList = new List<string> { "code", "name", "subscriptionType" };
                        break;
                    case "o":
                        _subscriptionType = "o";
                        bdr_price.Visibility = Visibility.Visible;
                        btn_subscriptionFees.IsEnabled = false;
                        requiredControlList = new List<string> { "code", "name", "subscriptionType", "price" };
                        break;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

      
    }
}
