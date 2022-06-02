using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
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

namespace laundryApp.View.sales.promotion
{
    /// <summary>
    /// Interaction logic for uc_invoicesClasses.xaml
    /// </summary>
    public partial class uc_invoicesClasses : UserControl
    {
        public uc_invoicesClasses()
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
        private static uc_invoicesClasses _instance;
        public static uc_invoicesClasses Instance
        {
            get
            {
                //if (_instance == null)
                if (_instance is null)
                    _instance = new uc_invoicesClasses();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string basicsPermission = "invoicesClasses_basics";
        InvoicesClass invoicesClass = new InvoicesClass();
        IEnumerable<InvoicesClass> invoicesClassesQuery;
        IEnumerable<InvoicesClass> invoicesClasses;
        byte tgl_invoicesClassState;
        string searchText = "";
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

                requiredControlList = new List<string> { "name", "minInvoiceValue", "maxInvoiceValue", "discountValue", "discountType" };

                FillCombo.fillDiscountType(cb_discountType);


                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                await RefreshInvoicesClassesList();
                await Search();

                Keyboard.Focus(tb_name);

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

            //txt_title.Text = AppSettings.resourcemanager.GetString("invoicesClasses");
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));

            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_membershipId, AppSettings.resourcemanager.GetString("trMembership")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_discountType, AppSettings.resourcemanager.GetString("trTypeDiscountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_discountValue, AppSettings.resourcemanager.GetString("trDiscountValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_minInvoiceValue, AppSettings.resourcemanager.GetString("trMinimumInvoiceValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_maxInvoiceValue, AppSettings.resourcemanager.GetString("trMaximumInvoiceValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");

            dg_invoicesClass.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            string str = AppSettings.resourcemanager.GetString("trMinimumInvoiceValueHint");
            int index = str.IndexOf("...");
            if (index != -1)
            {
                str = str.Remove(index);
            }
            dg_invoicesClass.Columns[1].Header = str;
            string str1 = AppSettings.resourcemanager.GetString("trMinimumInvoiceValueHint");
            dg_invoicesClass.Columns[2].Header = str1.Replace("..." , "");
            //dg_invoicesClass.Columns[3].Header = AppSettings.resourcemanager.GetString("trQuantity");
            //dg_invoicesClass.Columns[4].Header = AppSettings.resourcemanager.GetString("trRemainQuantity");
            //dg_invoicesClass.Columns[5].Header = AppSettings.resourcemanager.GetString("trvalidity");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");


        }

        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add"))
                {
                    HelpClass.StartAwait(grid_main);
                    invoicesClass = new InvoicesClass();
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        #region validate
                       

                        bool isMaxInvoiceValueSmaller = false;
                        try
                        {
                            if (!decimal.Parse(tb_maxInvoiceValue.Text).Equals(0))
                                if (decimal.Parse(tb_maxInvoiceValue.Text) < decimal.Parse(tb_minInvoiceValue.Text)) isMaxInvoiceValueSmaller = true;
                        }
                        catch { }
                        if ( isMaxInvoiceValueSmaller)
                        {
                                HelpClass.SetValidate(p_error_minInvoiceValue, "trErrorMaxInvoiceSmallerToolTip");
                                HelpClass.SetValidate(p_error_maxInvoiceValue, "trErrorMaxInvoiceSmallerToolTip");
                        }
                        #endregion
                        else
                        {
                            #region add
                            invoicesClass = new InvoicesClass();

                            invoicesClass.name = tb_name.Text;
                            invoicesClass.notes = tb_notes.Text;
                            invoicesClass.discountType = Convert.ToByte(cb_discountType.SelectedValue);
                            invoicesClass.discountValue = decimal.Parse(tb_discountValue.Text);

                            if (!tb_minInvoiceValue.Text.Equals(""))
                                invoicesClass.minInvoiceValue = decimal.Parse(tb_minInvoiceValue.Text);
                            else
                                invoicesClass.minInvoiceValue = 0;
                            if (!tb_maxInvoiceValue.Text.Equals(""))
                                invoicesClass.maxInvoiceValue = decimal.Parse(tb_maxInvoiceValue.Text);
                            else
                                invoicesClass.maxInvoiceValue = 0;
                            invoicesClass.isActive = 1;
                            invoicesClass.createUserId = MainWindow.userLogin.userId;
                            invoicesClass.updateUserId= MainWindow.userLogin.userId;

                            int s = await invoicesClass.save(invoicesClass);

                            if (s > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                Clear();

                                await RefreshInvoicesClassesList();
                                await Search();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            #endregion
                        }

                    }
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

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                {
                    HelpClass.StartAwait(grid_main);
                    if (invoicesClass.invClassId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this))
                        {
                           


                          

                            bool isMaxInvoiceValueSmaller = false;
                            try
                            {
                                if (!decimal.Parse(tb_maxInvoiceValue.Text).Equals(0))
                                    if (decimal.Parse(tb_maxInvoiceValue.Text) < decimal.Parse(tb_minInvoiceValue.Text)) isMaxInvoiceValueSmaller = true;
                            }
                            catch { }
                            if (isMaxInvoiceValueSmaller)
                            {
                                HelpClass.SetValidate(p_error_minInvoiceValue, "trErrorMaxInvoiceSmallerToolTip");
                                HelpClass.SetValidate(p_error_maxInvoiceValue, "trErrorMaxInvoiceSmallerToolTip");
                            }
                            else
                            {
                                #region update
                                invoicesClass.name = tb_name.Text;
                                invoicesClass.notes = tb_notes.Text;
                                invoicesClass.discountType = Convert.ToByte(cb_discountType.SelectedValue);
                                invoicesClass.discountValue = decimal.Parse(tb_discountValue.Text);

                               
                                if (!tb_minInvoiceValue.Text.Equals(""))
                                    invoicesClass.minInvoiceValue = decimal.Parse(tb_minInvoiceValue.Text);
                                else
                                    invoicesClass.minInvoiceValue = 0;
                                if (!tb_maxInvoiceValue.Text.Equals(""))
                                    invoicesClass.maxInvoiceValue = decimal.Parse(tb_maxInvoiceValue.Text);
                                else
                                    invoicesClass.maxInvoiceValue = 0;
                               
                                invoicesClass.updateUserId = MainWindow.userLogin.userId;

                                int s = await invoicesClass.save(invoicesClass);

                                if (s > 0)
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                                    await Search();
                                }
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                            }
                            #endregion
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
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete"))
                {
                    HelpClass.StartAwait(grid_main);
                    if (invoicesClass.invClassId != 0)
                    {
                        if ((!invoicesClass.canDelete) && (invoicesClass.isActive == 0))
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
                            if (invoicesClass.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!invoicesClass.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (invoicesClass.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!invoicesClass.canDelete) && (invoicesClass.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await invoicesClass.delete(invoicesClass.invClassId, MainWindow.userLogin.userId, invoicesClass.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    invoicesClass.invClassId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshInvoicesClassesList();
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
            invoicesClass.isActive = 1;
            int s = await invoicesClass.save(invoicesClass);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshInvoicesClassesList();
                await Search();
            }
        }

        #endregion

        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await Search();
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
                if (invoicesClasses is null)
                    await RefreshInvoicesClassesList();

                tgl_invoicesClassState = 1;

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

                if (invoicesClasses is null)
                    await RefreshInvoicesClassesList();

                tgl_invoicesClassState = 0;

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
        private async void Dg_invoicesClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_invoicesClass.SelectedIndex != -1)
                {
                    invoicesClass = dg_invoicesClass.SelectedItem as InvoicesClass;
                    this.DataContext = invoicesClass;
                    if (invoicesClass != null)
                    {

                       

                        #region delete
                        if (invoicesClass.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (invoicesClass.isActive == 0)
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
                await RefreshInvoicesClassesList();
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

        async Task Search()
        {
            //search
            if (invoicesClasses is null)
                await RefreshInvoicesClassesList();

            searchText = tb_search.Text.ToLower();
            invoicesClassesQuery = invoicesClasses;
            invoicesClassesQuery = invoicesClasses.Where(s =>
            (
             
            s.name.ToLower().Contains(searchText)
            ||
            s.minInvoiceValue.ToString().ToLower().Contains(searchText)
            ||
            s.maxInvoiceValue.ToString().ToLower().Contains(searchText)
            )
            && s.isActive == tgl_invoicesClassState
            );
            RefreshCustomersView();
        }
        async Task<IEnumerable<InvoicesClass>> RefreshInvoicesClassesList()
        {
            invoicesClasses = await invoicesClass.GetAll();
            return invoicesClasses;
        }
        void RefreshCustomersView()
        {
            dg_invoicesClass.ItemsSource = invoicesClassesQuery;
            txt_count.Text = invoicesClassesQuery.Count().ToString();
        }

        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new InvoicesClass();
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tb_discountValue.IsEnabled = true;
            cb_discountType.IsEnabled = true;

            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
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
                    decimal _decimal;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only english and digits
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
                string name = sender.GetType().Name;
                var txb = sender as TextBox;
               

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
            string secondTitle = "invClasses";
            string subTitle = "";
            string Title = "";

            string addpath = "";
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {

                addpath = @"\Reports\Sale\Ar\ArInvClass.rdlc";
                //   secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            }
            else
            {
                //english

                addpath = @"\Reports\Sale\En\EnInvClass.rdlc";
                //  secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);


            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //  getpuritemcount
            Title = AppSettings.resourcemanagerreport.GetString("trSales") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));


            clsReports.InvClassReport(invoicesClassesQuery, rep, reppath, paramarr);

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

        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvcSales win = new win_lvcSales(invoicesClassesQuery, 5);
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
        #endregion



    }
}
