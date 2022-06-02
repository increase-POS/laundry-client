using netoaster;
using laundryApp.Classes;
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
using System.Windows.Navigation;
using Microsoft.Win32;
using laundryApp.View.windows;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace laundryApp.View.kitchen
{
    /// <summary>
    /// Interaction logic for uc_menuSettings.xaml
    /// </summary>
    public partial class uc_menuSettings : UserControl
    {
        public uc_menuSettings()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private static uc_menuSettings _instance;
        public static uc_menuSettings Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_menuSettings();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        string updatePermission = "menuSettings_update";
        byte tgl_itemState;
        string searchText = "";
        int categoryId = 0;
        int tagId = 0;
        List<string> weekDays = new List<string>() { "sat","sun", "mon","tues","wed","thur","fri" };
        List<string> selectedDays = new List<string>();
        public static List<string> requiredControlList;
        //List<Unit> units;
        Unit unit = new Unit();
        #region for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        #endregion
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.KeyDown -= HandleKeyPress;
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                MainWindow.mainWindow.KeyDown += HandleKeyPress;
                // for pagination onTop Always
                btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                catigoriesAndItemsView.ucmenuSettings = this;

                catalogMenuList = new List<string> { "allMenu", "appetizers", "beverages", "fastFood", "mainCourses", "desserts" };
                categoryBtns = new List<Button> { btn_appetizers, btn_beverages, btn_fastFood, btn_mainCourses, btn_desserts };
                requiredControlList = new List<string> { "preparingTime" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                Clear();
                await RefreshItemsList();
                await refrishCategories();
                //enable categories buttons
                HelpClass.activateCategoriesButtons(items, FillCombo.categoriesList, categoryBtns);
                await Search();
                
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

            //txt_title.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_name.Text = AppSettings.resourcemanager.GetString("itemName");
            txt_daysOfWeek.Text = AppSettings.resourcemanager.GetString("trDaysOfWeek");
            txt_minute.Text = AppSettings.resourcemanager.GetString("trMinute");
            txt_activeItem.Text = AppSettings.resourcemanager.GetString("trActive");
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
            txt_allMenu.Text = AppSettings.resourcemanager.GetString("trAll");
            txt_appetizers.Text = AppSettings.resourcemanager.GetString("trAppetizers");
            txt_beverages.Text = AppSettings.resourcemanager.GetString("trBeverages");
            txt_fastFood.Text = AppSettings.resourcemanager.GetString("trFastFood");
            txt_mainCourses.Text = AppSettings.resourcemanager.GetString("trMainCourses");
            txt_desserts.Text = AppSettings.resourcemanager.GetString("trDesserts");


            chb_all.Content = AppSettings.resourcemanager.GetString("trAll");
            chb_sat.Content = AppSettings.resourcemanager.GetString("trSaturday");
            chb_sun.Content = AppSettings.resourcemanager.GetString("trSunday");
            chb_mon.Content = AppSettings.resourcemanager.GetString("trMonday");
            chb_tues.Content = AppSettings.resourcemanager.GetString("trTuesday");
            chb_wed.Content = AppSettings.resourcemanager.GetString("trWednsday");
            chb_thur.Content = AppSettings.resourcemanager.GetString("trThursday");
            chb_fri.Content = AppSettings.resourcemanager.GetString("trFriday");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            btn_refresh.ToolTip = AppSettings.resourcemanager.GetString("trRefresh");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_preparingTime, AppSettings.resourcemanager.GetString("trPreparingTimeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "one"))
                {
                    //save
                    #region validate controls
                    if (tgl_isActiveItem.IsChecked == true)
                        requiredControlList = new List<string>() { "preparingTime" };
                    else
                        requiredControlList = new List<string>();
                    #endregion

                    #region save
                    if(item.itemUnitId != null)
                    {
                        if (HelpClass.validate(requiredControlList, this))
                    {
                        //item.itemUnitId
                        var menuSet = new MenuSetting();

                        if (tgl_isActiveItem.IsChecked == true)
                            menuSet.isActive = 1;
                        else
                            menuSet.isActive = 0;
                        if (menuSet.isActive == 1 && selectedDays.Count == 0)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectOnDayAtLeast"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            if (item.menuSettingId == null)
                                menuSet.menuSettingId = 0;
                            else
                                menuSet.menuSettingId = item.menuSettingId;
                            menuSet.itemUnitId = item.itemUnitId;
                            menuSet.preparingTime = decimal.Parse(tb_preparingTime.Text);
                            menuSet.branchId = MainWindow.branchLogin.branchId;


                            menuSet.createUserId = MainWindow.userLogin.userId;
                            menuSet.updateUserId = MainWindow.userLogin.userId;
                            foreach (string str in selectedDays)
                            {
                                switch (str)
                                {
                                    case "sat":
                                        menuSet.sat = true;
                                        break;
                                    case "sun":
                                        menuSet.sun = true;
                                        break;
                                    case "mon":
                                        menuSet.mon = true;
                                        break;
                                    case "tues":
                                        menuSet.tues = true;
                                        break;
                                    case "wed":
                                        menuSet.wed = true;
                                        break;
                                    case "thur":
                                        menuSet.thur = true;
                                        break;
                                    case "fri":
                                        menuSet.fri = true;
                                        break;
                                }
                            }
                            int res = await menuSetting.saveItemMenuSetting(menuSet);
                            if (res > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await RefreshItemsList();
                                await Search();
                                Clear();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);
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
                if (items is null)
                    await RefreshItemsList();
                tgl_itemState = 1;
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
                if (items is null)
                    await RefreshItemsList();
                tgl_itemState = 0;
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
        private async void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 150)
                {
                    _BarcodeStr = "";
                }

                string digit = "";
                // record keystroke & timestamp 
                if (e.Key >= Key.D0 && e.Key <= Key.D9)
                {
                    //digit pressed!
                    digit = e.Key.ToString().Substring(1);
                    // = "1" when D1 is pressed
                }
                else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    digit = e.Key.ToString().Substring(6); // = "1" when NumPad1 is pressed

                }
                _BarcodeStr += digit;
                _lastKeystroke = DateTime.Now;

                // process barcode 
                if (e.Key.ToString() == "Return" && _BarcodeStr != "")
                {
                    // get item matches barcode
                    if (FillCombo.itemUnitList != null)
                    {
                        var ob = FillCombo.itemUnitList.ToList().Find(c => c.barcode == _BarcodeStr && FillCombo.purchaseTypes.Contains(c.type));
                        if (ob != null)
                        {
                            int itemId = (int)ob.itemId;
                            ChangeItemIdEvent(itemId);
                        }
                        else
                        {
                            Clear();
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorItemNotFoundToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    _BarcodeStr = "";
                }
            }
            catch (Exception ex)
            {
                _BarcodeStr = "";
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        //private async void Dg_item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);
        //        //selection
        //        if (dg_item.SelectedIndex != -1)
        //        {
        //            item = dg_item.SelectedItem as Item;
        //            this.DataContext = item;
        //            if (item != null)
        //            {
        //                await getImg();
        //                #region delete
        //                if (item.canDelete)
        //                    btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
        //                else
        //                {
        //                    if (item.isActive == 0)
        //                        btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
        //                    else
        //                        btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
        //                }
        //                #endregion
        //                HelpClass.getMobile(item.mobile, cb_areaMobile, tb_mobile);
        //                HelpClass.getPhone(item.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
        //                HelpClass.getPhone(item.fax, cb_areaFax, cb_areaFaxLocal, tb_fax);
        //            }
        //        }
        //        HelpClass.clearValidate(requiredControlList, this);
        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshItemsList();
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
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = tb_search.Text;
                if (items == null)
                    await RefreshItemsList();
                itemsQuery = items.Where(x => (x.code.ToLower().Contains(searchText) ||
                    x.name.ToLower().Contains(searchText) ||
                    x.details.ToLower().Contains(searchText)
                     || x.code.ToLower().Contains(searchText)
                    ) && x.isActive == tgl_itemState).ToList();

                #region search for category
                if (categoryId > 0)
                    itemsQuery = itemsQuery.Where(x => x.categoryId == categoryId).ToList();
                #endregion

                #region search for tag
                if (tagId > 0)
                    itemsQuery = itemsQuery.Where(x => x.tagId == tagId).ToList();
                #endregion
                pageIndex = 1;
                #region


                if (btns is null)
                    btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - checkAllCheckBox . . . . 
        void checkAllCheckBox()
        {
            chb_all.IsChecked =
            chb_sat.IsChecked =
                        chb_sun.IsChecked =
                        chb_mon.IsChecked =
                        chb_tues.IsChecked =
                        chb_wed.IsChecked =
                        chb_thur.IsChecked =
                        chb_fri.IsChecked = true;

            wp_daysWeek.IsEnabled = false;

            selectedDays = new List<string>();
            selectedDays.AddRange(weekDays);
        }
        void unCheckAllCheckBox()
        {
            chb_sat.IsChecked =
                        chb_sun.IsChecked =
                        chb_mon.IsChecked =
                        chb_tues.IsChecked =
                        chb_wed.IsChecked =
                        chb_thur.IsChecked =
                        chb_fri.IsChecked = false;
            wp_daysWeek.IsEnabled = true;
        }
        void Clear()
        {
            item = new MenuSetting();

            this.DataContext = item;
            tb_preparingTime.Text = "";
            #region selected days
            checkAllCheckBox();
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
        private void ValidateEmpty_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        #region report
        /*
        // report
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Sale\Ar\PackageReport.rdlc";
            }
            else
                addpath = @"\Reports\Sale\En\PackageReport.rdlc";
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            clsReports.packageReport(itemsQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        public void pdfpackage()
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
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {

                
                    SectionData.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    /////////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        pdfpackage();
                    });
                    t1.Start();
                    //////////////////////////////////////
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        public void printpackage()
        {
            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, MainWindow.rep_printer_name, short.Parse(MainWindow.rep_print_count));
            });
        }
        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    /////////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        printpackage();
                    });
                    t1.Start();
                    //////////////////////////////////////

                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvcCatalog win = new win_lvcCatalog(itemsQuery, 3);
                    // // w.ShowInTaskbar = false;
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    /////////////////////
                    string pdfpath = "";
                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
                    BuildReport();
                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    ///////////////////
                    wd_previewPdf w = new wd_previewPdf();
                    w.pdfPath = pdfpath;
                    if (!string.IsNullOrEmpty(w.pdfPath))
                    {
                    // w.ShowInTaskbar = false;
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();
                    }
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        public void ExcelPackage()
        {

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
        }
        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    Thread t1 = new Thread(() =>
                    {
                        ExcelPackage();

                    });
                    t1.Start();
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        */
        #endregion
        #region  Cards
        CatigoriesAndItemsView catigoriesAndItemsView = new CatigoriesAndItemsView();
        #region Refrish Y
        MenuSetting item = new MenuSetting();
        MenuSetting menuSetting = new MenuSetting();
        List<MenuSetting> items;
        List<MenuSetting> itemsQuery;
        async Task RefreshItemsList()
        {
            items = await menuSetting.Get(MainWindow.branchLogin.branchId);
        }
        async Task refrishCategories()
        {
            if (FillCombo.categoriesList == null)
                await FillCombo.RefreshCategory();
        }
        void RefrishItemsCard(IEnumerable<MenuSetting> _items)
        {
            grid_itemContainerCard.Children.Clear();
            catigoriesAndItemsView.gridCatigorieItems = grid_itemContainerCard;
            string jsonItem = JsonConvert.SerializeObject(_items);
            List<Item> it = JsonConvert.DeserializeObject<List<Item>>(jsonItem, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            catigoriesAndItemsView.FN_refrishCatalogItem(it.ToList() ,3,5, "purchase");
        }
        #endregion
        #region Get Id By Click  Y

        public async void ChangeItemIdEvent(int itemId)
        {
            try
            {
                Clear();
                item = items.Where(x => x.itemId == itemId).FirstOrDefault();
                this.DataContext = item;
                tb_preparingTime.Text = HelpClass.PercentageDecTostring(item.preparingTime);
                #region selected days
                selectedDays = new List<string>();
                if (item.menuSettingId != null)
                {
                    foreach (string day in weekDays)
                    {
                        var propertyInfo = item.GetType().GetProperty(day);
                        bool value = (bool)propertyInfo.GetValue(item);
                        CheckBox cb = ((CheckBox)FindName("chb_" + day));
                        if (value == true)
                            selectedDays.Add(day);

                    }
                    if (selectedDays.Count == weekDays.Count)
                        checkAllCheckBox();
                    else
                    {
                        chb_all.IsChecked = false;
                        unCheckAllCheckBox();
                    }

                    // check selected days
                    if (chb_all.IsChecked == false)
                    {
                        foreach (string day in selectedDays)
                        {
                            CheckBox cb = ((CheckBox)FindName("chb_" + day));
                            cb.IsChecked = true;

                        }
                    }
                }
                else
                    checkAllCheckBox();

                #endregion
                HelpClass.clearValidate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        #region Pagination Y
        Pagination pagination = new Pagination();
        Button[] btns;
        public int pageIndex = 1;

        private void Tb_pageNumberSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                itemsQuery = items.ToList();

                if (tb_pageNumberSearch.Text.Equals(""))
                {
                    pageIndex = 1;
                }
                else if (((itemsQuery.Count() - 1) / 9) + 1 < int.Parse(tb_pageNumberSearch.Text))
                {
                    pageIndex = ((itemsQuery.Count() - 1) / 9) + 1;
                }
                else
                {
                    pageIndex = int.Parse(tb_pageNumberSearch.Text);
                }

                #region

                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_firstPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                pageIndex = 1;
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_prevPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_prevPage.Content.ToString());
                #region
                itemsQuery = items.ToList();

                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_activePage_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_activePage.Content.ToString());
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_nextPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_nextPage.Content.ToString());
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_lastPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                itemsQuery = items.ToList();
                pageIndex = ((itemsQuery.Count() - 1) / 9) + 1;
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        #endregion

        #endregion

        #region catalogMenu
        public static List<string> catalogMenuList;
        public static List<Button> categoryBtns;
        private async void catalogMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tagId = 0;
                string senderTag = (sender as Button).Tag.ToString();
                if (senderTag != "allMenu")
                    categoryId = FillCombo.categoriesList.Where(x => x.categoryCode == senderTag).FirstOrDefault().categoryId;
                else
                    categoryId = -1;
                #region refresh colors
                foreach (var control in catalogMenuList)
                {
                    Border border = FindControls.FindVisualChildren<Border>(this).Where(x => x.Tag != null && x.Name == "bdr_" + control)
                        .FirstOrDefault();
                    if (border.Tag.ToString() == senderTag)
                        border.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    else
                        border.Background = Application.Current.Resources["White"] as SolidColorBrush;
                }
                foreach (var control in catalogMenuList)
                {

                    Path path = FindControls.FindVisualChildren<Path>(this).Where(x => x.Tag != null && x.Name == "path_" + control)
                        .FirstOrDefault();
                    if (path.Tag.ToString() == senderTag)
                        path.Fill = Application.Current.Resources["White"] as SolidColorBrush;
                    else
                        path.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
                }
                foreach (var control in catalogMenuList)
                {
                    TextBlock textBlock = FindControls.FindVisualChildren<TextBlock>(this).Where(x => x.Tag != null && x.Name == "txt_" + control)
                        .FirstOrDefault();
                    if (textBlock.Tag.ToString() == senderTag)
                        textBlock.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    else
                        textBlock.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                }
                #endregion
                refreshCatalogTags(senderTag);
                await Search();
            }
            catch { }
        }
        public static List<Tag> tagsList;
        async void refreshCatalogTags(string tag)
        {
            //tagsList = new List<string> { "Orient", "Western", "Eastern" };
            tagsList = await FillCombo.tag.Get(categoryId);
            if (tagsList.Count > 1)
            {
                Tag allTag = new Tag();
                allTag.tagName = AppSettings.resourcemanager.GetString("trAll");
                allTag.tagId = 0;
                tagsList.Add(allTag);
            }
            sp_menuTags.Children.Clear();
            foreach (var item in tagsList)
            {
                #region  
                Button button = new Button();
                button.Content = item.tagName;
                button.Tag = "catalogTags-" + item.tagName;
                button.FontSize = 10;
                button.Height = 25;
                button.Padding = new Thickness(5, 0, 5, 0);
                MaterialDesignThemes.Wpf.ButtonAssist.SetCornerRadius(button, (new CornerRadius(7)));
                button.Margin = new Thickness(5, 0, 5, 0);
                if (item.tagName == AppSettings.resourcemanager.GetString("trAll"))
                {
                    button.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    button.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                }
                else
                {
                    button.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    button.Background = Application.Current.Resources["White"] as SolidColorBrush;                  
                }
                button.BorderBrush = Application.Current.Resources["MainColor"] as SolidColorBrush;
                button.Click += buttonCatalogTags_Click;


                sp_menuTags.Children.Add(button);
                /////////////////////////////////

                #endregion
            }
        }
       async void buttonCatalogTags_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string senderTag = (sender as Button).Tag.ToString();
                #region refresh colors
                foreach (var control in tagsList)
                {
                    Button button = FindControls.FindVisualChildren<Button>(this).Where(x => x.Tag != null && x.Tag.ToString() == "catalogTags-" + control.tagName)
                        .FirstOrDefault();
                    if (button.Tag.ToString() == senderTag)
                    {
                        button.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                        button.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        tagId = control.tagId;
                    }
                    else
                    {
                        button.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        button.Background = Application.Current.Resources["White"] as SolidColorBrush;
                    }
                }
                #endregion
                await Search();
            }
            catch { }
        }
        #endregion

        private void Tgl_isActiveItem_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tgl_isActiveItem.IsChecked.Value)
                    sp_daysWeek.IsEnabled = true;
                else
                    sp_daysWeek.IsEnabled = false;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Tgl_isActiveItem_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tgl_isActiveItem.IsChecked.Value)
                    sp_daysWeek.IsEnabled = true;
                else
                    sp_daysWeek.IsEnabled = false;

            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Chb_all_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsFocused)
                {
                    if (checkBox.Name == "chb_all")
                        checkAllCheckBox();                      
                    else if (checkBox.Name == "chb_sat")
                        selectedDays.Add("sat");
                    else if (checkBox.Name == "chb_sun")
                        selectedDays.Add("sun");
                    else if (checkBox.Name == "chb_mon")
                        selectedDays.Add("mon");
                    else if (checkBox.Name == "chb_tues")
                        selectedDays.Add("tues");
                    else if (checkBox.Name == "chb_wed")
                        selectedDays.Add("wed");
                    else if (checkBox.Name == "chb_thur")
                        selectedDays.Add("thur");
                    else if (checkBox.Name == "chb_fri")
                        selectedDays.Add("fri");
                }
               
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Chb_all_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.IsFocused)
                {
                    if (checkBox.Name == "chb_all")
                    {
                        unCheckAllCheckBox();
                        selectedDays = new List<string>();
                    }
                    else if (checkBox.Name == "chb_sat")
                        selectedDays.Remove("sat");
                    else if (checkBox.Name == "chb_sun")
                        selectedDays.Remove("sun");
                    else if (checkBox.Name == "chb_mon")
                        selectedDays.Remove("mon");
                    else if (checkBox.Name == "chb_tues")
                        selectedDays.Remove("tues");
                    else if (checkBox.Name == "chb_wed")
                        selectedDays.Remove("wed");
                    else if (checkBox.Name == "chb_thur")
                        selectedDays.Remove("thur");
                    else if (checkBox.Name == "chb_fri")
                        selectedDays.Remove("fri");
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
