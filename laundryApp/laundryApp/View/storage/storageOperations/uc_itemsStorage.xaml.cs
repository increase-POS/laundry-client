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
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.IO;
using System.Windows.Threading;
using System.Threading;
 

namespace laundryApp.View.storage.storageOperations
{
    /// <summary>
    /// Interaction logic for uc_itemsStorage.xaml
    /// </summary>
    public partial class uc_itemsStorage : UserControl
    {
        public uc_itemsStorage()
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
        private static uc_itemsStorage _instance;
        public static uc_itemsStorage Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_itemsStorage();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string transferPermission = "itemsStorage_transfer";
        string reportsPermission = "itemsStorage_reports";
        ItemLocation itemLocation = new ItemLocation();
        IEnumerable<ItemLocation> itemLocationsQuery;
        IEnumerable<ItemLocation> itemLocations;
        byte tgl_branchState;
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
                requiredControlList = new List<string> { "itemName", "quantity", "sectionId", "locationId" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                await FillCombo.FillComboSections(cb_sectionId);

                btn_transfer.IsEnabled = false;
                //btn_locked.IsEnabled = false;

                Keyboard.Focus(tb_quantity);
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
            ////////////////////////////////----invoice----/////////////////////////////////
            dg_itemsStorage.Columns[0].Header = AppSettings.resourcemanager.GetString("trItemUnit");
            dg_itemsStorage.Columns[1].Header = AppSettings.resourcemanager.GetString("trSectionLocation");
            dg_itemsStorage.Columns[2].Header = AppSettings.resourcemanager.GetString("trQTR");
            dg_itemsStorage.Columns[3].Header = AppSettings.resourcemanager.GetString("trStartDate");
            dg_itemsStorage.Columns[4].Header = AppSettings.resourcemanager.GetString("trEndDate");
            dg_itemsStorage.Columns[5].Header = AppSettings.resourcemanager.GetString("trNote");
            //dg_itemsStorage.Columns[6].Header = AppSettings.resourcemanager.GetString("trOrderNum");

            txt_Location.Text = AppSettings.resourcemanager.GetString("trLocationt");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_itemName, AppSettings.resourcemanager.GetString("trItemNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_quantity, AppSettings.resourcemanager.GetString("trQuantityHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_sectionId, AppSettings.resourcemanager.GetString("trSectionHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_locationId, AppSettings.resourcemanager.GetString("trLocationHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            chk_stored.Content = AppSettings.resourcemanager.GetString("trStored");
            chk_freezone.Content = AppSettings.resourcemanager.GetString("trFreeZone");
            //chk_locked.Content = AppSettings.resourcemanager.GetString("trReserved");
            btn_transfer.Content = AppSettings.resourcemanager.GetString("trTransfer");
           // btn_locked.Content = AppSettings.resourcemanager.GetString("trUnlock");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        //private async void Btn_locked_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {                
        //        HelpClass.StartAwait(grid_main);

        //        if (FillCombo.groupObject.HasPermissionAction(transferPermission, FillCombo.groupObjects, "one") )
        //        {
        //            if (dg_itemsStorage.SelectedIndex != -1)
        //            {
        //                if (tb_quantity.Text != "" && int.Parse(tb_quantity.Text) == 0)
        //                    HelpClass.SetValidate(p_error_quantity, AppSettings.resourcemanager.GetString("trErrorQuantIsZeroToolTip"));
        //                else if (HelpClass.validate(requiredControlList, this))
        //                {
        //                    int quantity = int.Parse(tb_quantity.Text);
        //                    ItemLocation newLocation = new ItemLocation();
        //                    newLocation.itemsLocId = itemLocation.itemsLocId;
        //                    newLocation.itemUnitId = itemLocation.itemUnitId;
        //                    newLocation.locationId = itemLocation.locationId;
        //                    newLocation.quantity = quantity;
        //                    newLocation.startDate = dp_startDate.SelectedDate;
        //                    newLocation.endDate = dp_endDate.SelectedDate;
        //                    newLocation.notes = tb_notes.Text;
        //                    newLocation.updateUserId = MainWindow.userLogin.userId;
        //                    newLocation.createUserId = MainWindow.userLogin.userId;
        //                    int res = await itemLocation.unlockItem(newLocation, MainWindow.branchLogin.branchId);
        //                    if (res > 0)
        //                    {
        //                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

        //                    }
        //                    else 
        //                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

        //                    await RefreshItemLocationsList();
        //                    await Search();
        //                    Clear();
        //                }
        //            }
        //        }
        //        else
        //            Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
        //            HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //            HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private async void Btn_transfer_Click(object sender, RoutedEventArgs e)
        {
            // transfer
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(transferPermission, FillCombo.groupObjects, "one") )
                {
                    if (dg_itemsStorage.SelectedIndex != -1)
                    {
                        if (tb_quantity.Text != "" && int.Parse(tb_quantity.Text) == 0)
                            HelpClass.SetValidate(p_error_quantity, "trErrorQuantIsZeroToolTip");
                        else if (HelpClass.validate(requiredControlList, this))
                        {
                            int oldLocationId = (int)itemLocation.locationId;
                            int newLocationId = (int)cb_locationId.SelectedValue;
                            if (oldLocationId != newLocationId)
                            {
                                int quantity = int.Parse(tb_quantity.Text);
                                ItemLocation newLocation = new ItemLocation();
                                newLocation.itemUnitId = itemLocation.itemUnitId;
                                newLocation.invoiceId = itemLocation.invoiceId;
                                newLocation.locationId = newLocationId;
                                newLocation.quantity = quantity;
                                newLocation.startDate = dp_startDate.SelectedDate;
                                newLocation.endDate = dp_endDate.SelectedDate;
                                newLocation.notes = tb_notes.Text;
                                newLocation.updateUserId = MainWindow.userLogin.userId;
                                newLocation.createUserId = MainWindow.userLogin.userId;

                                int res = await itemLocation.trasnferItem(itemLocation.itemsLocId, newLocation);
                                if (res > 0)
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                }
                                else 
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                                await RefreshItemLocationsList();
                                await Search();
                                Clear();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTranseToSameLocation"), animation: ToasterAnimation.FadeIn);
                           
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
        #endregion
        #region events
        int _int = 0;
        private async void Cb_sectionId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_sectionId.SelectedValue != null)
                {
                    if (int.TryParse(cb_sectionId.SelectedValue.ToString(), out _int))
                    {
                        await FillCombo.FillComboLocationsBySection(cb_locationId, (int)cb_sectionId.SelectedValue);
                        cb_locationId.SelectedValue = itemLocation.locationId;
                    }
                }
                else
                {
                    await FillCombo.FillComboLocationsBySection(cb_locationId, -1);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void search_Checking(object sender, RoutedEventArgs e)
        {
            try
            {      
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_stored")
                    {
                        chk_freezone.IsChecked = false;
                        //chk_locked.IsChecked = false;
                        btn_transfer.Visibility = Visibility.Visible;
                        //btn_locked.Visibility = Visibility.Collapsed;
                        //dg_itemsStorage.Columns[6].Visibility = Visibility.Collapsed; //make order num column unvisible
                        dg_itemsStorage.Columns[3].Visibility = Visibility.Visible;
                        dg_itemsStorage.Columns[4].Visibility = Visibility.Visible;
                    }
                    else if (cb.Name == "chk_freezone")
                    {
                        chk_stored.IsChecked = false;
                        //chk_locked.IsChecked = false;
                        btn_transfer.Visibility = Visibility.Visible;
                       // btn_locked.Visibility = Visibility.Collapsed;
                        //dg_itemsStorage.Columns[6].Visibility = Visibility.Collapsed; //make order num column unvisible
                        dg_itemsStorage.Columns[3].Visibility = Visibility.Visible;
                        dg_itemsStorage.Columns[4].Visibility = Visibility.Visible;
                    }
                    //else
                    //{
                    //    chk_stored.IsChecked = false;
                    //    chk_freezone.IsChecked = false;
                    //    btn_locked.Visibility = Visibility.Visible;
                    //    btn_transfer.Visibility = Visibility.Collapsed;
                    //    dg_itemsStorage.Columns[6].Visibility = Visibility.Visible; //make order num column visible
                    //    dg_itemsStorage.Columns[3].Visibility = Visibility.Collapsed;
                    //    dg_itemsStorage.Columns[4].Visibility = Visibility.Collapsed;
                    //}
                }
                HelpClass.StartAwait(grid_main);
                await RefreshItemLocationsList();
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
        private void chk_uncheck(object sender, RoutedEventArgs e)
        {
            try
            {               
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_stored")
                        chk_stored.IsChecked = true;
                    else if (cb.Name == "chk_freezone")
                        chk_freezone.IsChecked = true;
                    //else
                    //    chk_locked.IsChecked = true;
                }                
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
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
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Dg_itemsStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_itemsStorage.SelectedIndex != -1)
                {
                    itemLocation = dg_itemsStorage.SelectedItem as ItemLocation;
                    this.DataContext = itemLocation;
                    if (itemLocation != null)
                    {
                        if (itemLocation.itemType.Equals("PurchaseExpire"))
                        {
                            gd_date.Visibility = Visibility.Visible;
                            requiredControlList = new List<string> { "itemName", "quantity", "sectionId", "locationId", "startDate" , "endDate" };
                        }
                        else
                        {
                            gd_date.Visibility = Visibility.Collapsed;
                            requiredControlList = new List<string> { "itemName", "quantity", "sectionId", "locationId" };
                        }
                        btn_transfer.IsEnabled = true;
                        //btn_locked.IsEnabled = true;

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
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshItemLocationsList();
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
            if (itemLocations is null)
                await RefreshItemLocationsList();
            searchText = tb_search.Text.ToLower();
            if (itemLocations != null)
            {
                itemLocationsQuery = itemLocations.Where(s => (s.itemName.ToLower().Contains(searchText) ||
                s.unitName.ToLower().Contains(searchText) ||
                s.section.ToLower().Contains(searchText) ||
                s.location.ToLower().Contains(searchText)));
            }
            RefreshItemsView();
        }
        void RefreshItemsView()
        {
            dg_itemsStorage.ItemsSource = itemLocationsQuery;
            txt_count.Text = itemLocationsQuery.Count().ToString();
        }
        private async Task refreshStoredItemsLocations()
        {
            itemLocations = await itemLocation.get(MainWindow.branchLogin.branchId);
        }
        private async Task refreshFreeZoneItemsLocations()
        {
            itemLocations = await itemLocation.GetFreeZoneItems(MainWindow.branchLogin.branchId);
        }
        //private async Task refreshLockedItems()
        //{
        //    itemLocations = await itemLocation.GetLockedItems(MainWindow.branchLogin.branchId);
        //}
        async Task<IEnumerable<ItemLocation>> RefreshItemLocationsList()
        {
            if (chk_stored.IsChecked == true)
                await refreshStoredItemsLocations();
            else if (chk_freezone.IsChecked == true)
                await refreshFreeZoneItemsLocations();
            //else if (chk_locked.IsChecked == true)
            //    await refreshLockedItems();
            
            return itemLocations;
        }

        #endregion
        #region validate - Clear - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new ItemLocation();
            dg_itemsStorage.SelectedIndex = -1;
            // last 
            HelpClass.clearValidate(requiredControlList, this);
            btn_transfer.IsEnabled = false;
            //btn_locked.IsEnabled = false;
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

        #region report

        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string subtitle = "";
            string  title = "";
            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Storage\storageOperations\Ar\ArItemsStorage.rdlc";

            }
            else
            {
                addpath = @"\Reports\Storage\storageOperations\En\EnItemsStorage.rdlc";
            }
            if ((bool)chk_freezone.IsChecked)
            {
                subtitle = AppSettings.resourcemanagerreport.GetString("trFreeZone");
            }
            else  
            {
                     subtitle = AppSettings.resourcemanagerreport.GetString("trStored");
            }
            title = AppSettings.resourcemanagerreport.GetString("trLocations") + "/" + subtitle;
            paramarr.Add(new ReportParameter("trTitle", title)); 

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            //D:\myproj\posproject5\laundryApp\laundryApp\View\storage\storageOperations\uc_ItemsStorage.xaml.cs
            ReportCls.checkLang();

            clsReports.ItemsStorage(itemLocationsQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        private void PrintRep()
        {
            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
            });
        }
        private void PdfRep()
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

        /*
 
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvcCatalog win = new win_lvcCatalog(itemsQuery, 3);
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

        */
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    if (itemLocationsQuery != null)
                    {
                        Thread t1 = new Thread(() =>
                        {
                            PdfRep();
                        });
                        t1.Start();
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {

                    if (itemLocationsQuery != null)
                    {
                        Thread t1 = new Thread(() =>
                        {
                            PrintRep();
                        });
                        t1.Start();
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

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    #region
                    //Thread t1 = new Thread(() =>
                    //{
                    if (itemLocationsQuery != null)
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

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    #region
                    if (itemLocationsQuery != null)
                    {
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
                            w.ShowDialog();
                            w.wb_pdfWebViewer.Dispose();
                        }
                        Window.GetWindow(this).Opacity = 1;
                    }
                    //////////////////////////////////////
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
