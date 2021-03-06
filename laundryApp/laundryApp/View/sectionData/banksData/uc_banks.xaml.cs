using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;
using System.IO;
using Microsoft.Reporting.WinForms;

namespace laundryApp.View.sectionData.banksData
{
    /// <summary>
    /// Interaction logic for uc_banks.xaml
    /// </summary>
    public partial class uc_banks : UserControl
    {
        public uc_banks()
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
        private static uc_banks _instance;
        public static uc_banks Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_banks();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string basicsPermission = "banks_basics";
        Bank bank = new Bank();
        IEnumerable<Bank> banksQuery;
        IEnumerable<Bank> banks;
        byte tgl_bankState;
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
                requiredControlList = new List<string> { "name", "accNumber", "mobile", "phone" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                await FillCombo.fillCountries(cb_areaMobile);
                await FillCombo.fillCountries(cb_areaPhone);
                Keyboard.Focus(tb_name);
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

            //txt_title.Text = AppSettings.resourcemanager.GetString("trBank");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trBankNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_accNumber, AppSettings.resourcemanager.GetString("trAccNumberHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_area, AppSettings.resourcemanager.GetString("trAreaHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_mobile, AppSettings.resourcemanager.GetString("trMobileHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_phone, AppSettings.resourcemanager.GetString("trPhoneHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_address, AppSettings.resourcemanager.GetString("trAdressHint"));
            txt_contentInformatin.Text = AppSettings.resourcemanager.GetString("trAnotherInfomation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            dg_bank.Columns[0].Header = AppSettings.resourcemanager.GetString("trBankName");
            dg_bank.Columns[1].Header = AppSettings.resourcemanager.GetString("trAccNumber");
            dg_bank.Columns[2].Header = AppSettings.resourcemanager.GetString("trAddress");
            dg_bank.Columns[3].Header = AppSettings.resourcemanager.GetString("trMobile");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
            txt_contactInformation.Text = AppSettings.resourcemanager.GetString("trContactInformation");

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                bank = new Bank();
                if (HelpClass.validate(requiredControlList, this))
                {

                    bool isBankExist = await chkDuplicateBank();
                    if (isBankExist)
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopBankExist"), animation: ToasterAnimation.FadeIn);
                        #region Tooltip_name
                        p_error_name.Visibility = Visibility.Visible;
                        ToolTip toolTip_name = new ToolTip();
                        toolTip_name.Content = AppSettings.resourcemanager.GetString("trPopBankExist");
                        toolTip_name.Style = Application.Current.Resources["ToolTipError"] as Style;
                        p_error_name.ToolTip = toolTip_name;
                        #endregion
                        #region Tooltip_accNumber
                        p_error_accNumber.Visibility = Visibility.Visible;
                        ToolTip toolTip_accNumber = new ToolTip();
                        toolTip_accNumber.Content = AppSettings.resourcemanager.GetString("trPopBankExist");
                        toolTip_accNumber.Style = Application.Current.Resources["ToolTipError"] as Style;
                        p_error_accNumber.ToolTip = toolTip_accNumber;
                        #endregion
                    }
                    else
                    {

                        bank.name = tb_name.Text;
                        bank.accNumber = tb_accNumber.Text;
                        bank.address = tb_address.Text;
                        bank.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                        if (!tb_phone.Text.Equals(""))
                            bank.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                        bank.createUserId = MainWindow.userLogin.userId;
                        bank.updateUserId = MainWindow.userLogin.userId;
                        bank.notes = tb_notes.Text;
                        bank.isActive = 1;

                        int s = await bank.save(bank);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);



                            Clear();
                            await RefreshCustomersList();
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
                    if (bank.bankId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this) )
                {
                    bool isBankExist = await chkDuplicateBank();
                    if (isBankExist)
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopBankExist"), animation: ToasterAnimation.FadeIn);
                        #region Tooltip_name
                        p_error_name.Visibility = Visibility.Visible;
                        ToolTip toolTip_name = new ToolTip();
                        toolTip_name.Content = AppSettings.resourcemanager.GetString("trPopBankExist");
                        toolTip_name.Style = Application.Current.Resources["ToolTipError"] as Style;
                        p_error_name.ToolTip = toolTip_name;
                        #endregion
                        #region Tooltip_accNumber
                        p_error_accNumber.Visibility = Visibility.Visible;
                        ToolTip toolTip_accNumber = new ToolTip();
                        toolTip_accNumber.Content = AppSettings.resourcemanager.GetString("trPopBankExist");
                        toolTip_accNumber.Style = Application.Current.Resources["ToolTipError"] as Style;
                        p_error_accNumber.ToolTip = toolTip_accNumber;
                        #endregion
                    }
                    else
                    {
                        bank.name = tb_name.Text;
                        bank.accNumber = tb_accNumber.Text;
                        bank.address = tb_address.Text;
                        bank.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                        if (!tb_phone.Text.Equals(""))
                            bank.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                        bank.updateUserId = MainWindow.userLogin.userId;
                        bank.notes = tb_notes.Text;

                        int s = await bank.save(bank);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                            await RefreshCustomersList();
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
        {
            try
            {//delete
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                if (bank.bankId != 0)
                {
                    if ((!bank.canDelete) && (bank.isActive == 0))
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
                        if (bank.canDelete)
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                        if (!bank.canDelete)
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion
                        if (w.isOk)
                        {
                            string popupContent = "";
                            if (bank.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                            if ((!bank.canDelete) && (bank.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                            int s = await bank.delete(bank.bankId, MainWindow.userLogin.userId, bank.canDelete);
                            if (s < 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                    bank.bankId = 0;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await RefreshCustomersList();
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
            bank.isActive = 1;
            int s = await bank.save(bank);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshCustomersList();
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
                if (banks is null)
                    await RefreshCustomersList();
                tgl_bankState = 1;
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
                if (banks is null)
                    await RefreshCustomersList();
                tgl_bankState = 0;
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
        private void Dg_bank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_bank.SelectedIndex != -1)
                {
                    bank = dg_bank.SelectedItem as Bank;
                    this.DataContext = bank;
                    if (bank != null)
                    {
                        #region delete
                        if (bank.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (bank.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                        HelpClass.getMobile(bank.mobile, cb_areaMobile, tb_mobile);
                        HelpClass.getPhone(bank.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
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

                searchText = "";
                tb_search.Text = "";
                await RefreshCustomersList();
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
            if (banks is null)
                await RefreshCustomersList();
            searchText = tb_search.Text.ToLower();
            banksQuery = banks.Where(s => (s.name.ToLower().Contains(searchText) ||
            s.accNumber.ToLower().Contains(searchText) ) && s.isActive == tgl_bankState);
            RefreshCustomersView();
        }
        async Task<IEnumerable<Bank>> RefreshCustomersList()
        {
            banks = await bank.Get();
            return banks;
        }
        void RefreshCustomersView()
        {
            dg_bank.ItemsSource = banksQuery;
            txt_count.Text = banksQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Bank();
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            #region mobile-Phone-fax-email
            brd_areaPhoneLocal.Visibility = Visibility.Collapsed;
            cb_areaMobile.SelectedIndex = -1;
            cb_areaPhone.SelectedIndex = -1;
            cb_areaPhoneLocal.SelectedIndex = -1;
            tb_mobile.Clear();
            tb_phone.Clear();
          
            #endregion
           


            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {


                //only  digits
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

        #region Phone
        int? countryid;
        private async void Cb_areaPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaPhone.SelectedValue != null)
                {
                    if (cb_areaPhone.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaPhone.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaPhoneLocal, (int)countryid, brd_areaPhoneLocal);
                    }
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion
       
        private async Task<bool> chkDuplicateBank()
        {
            bool b = false;


            List<Bank> banks = await bank.Get();
            Bank bank1 = new Bank();

            for (int i = 0; i < banks.Count(); i++)
            {
                bank1 = banks[i];
                if ((bank1.name.Equals(tb_name.Text.Trim())) &&
                    (bank1.accNumber.Equals(tb_accNumber.Text.Trim())) &&
                    (bank1.bankId != bank.bankId))
                { b = true; break; }
            }

            return b;
        }

        #region report
        //report  parameters
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        // end report parameters
        public void BuildReport()
        {

            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\SectionData\banksData\Ar\ArBank.rdlc";
            }
            else
            {
                addpath = @"\Reports\SectionData\banksData\En\EnBank.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            clsReports.BanksReport(banksQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {

            //pdf
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
                {
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
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
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;

                    string pdfpath = "";
                    //
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
                {

                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
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
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
                {
                    #region
                    //Thread t1 = new Thread(() =>
                    //{
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


                    //});
                    //t1.Start();
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
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvc win = new win_lvc(banksQuery, 5);
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
