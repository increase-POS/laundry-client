using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace laundryApp.View.storage.stocktakingOperations
{
    /// <summary>
    /// Interaction logic for uc_stocktaking.xaml
    /// </summary>
    public partial class uc_stocktaking : UserControl
     {
         List<ItemLocation> itemsLocations;
        InventoryItemLocation invItemModel = new InventoryItemLocation();
        List<InventoryItemLocation> invItemsLocations = new List<InventoryItemLocation>();

        Inventory inventory = new Inventory();
        private static DispatcherTimer timer;
        int _DocCount = 0;
        bool firstTimeForDatagrid = true;

        string _InventoryType = "d";
        string createInventoryPermission = "stocktaking_create";
        string archivingPermission = "stocktaking_archiving";
        string reportsPermission = "stocktaking_reports";

        private static int _ShortageAmount = 0;
        private static int _DestroyAmount = 0;

        private static uc_stocktaking _instance;
        public static uc_stocktaking Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_stocktaking();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public uc_stocktaking()
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
        private async void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (_InventoryType.Equals("d") && invItemsLocations.Count > 0)
                {
                    #region Accept
                    MainWindow.mainWindow.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trSaveStockTakingNotification");
                    w.ShowDialog();
                    MainWindow.mainWindow.Opacity = 1;
                    #endregion
                    if (w.isOk)
                        await addInventory("d"); // d:draft        
                }
                clearInventory();
                timer.Stop();

                Instance = null;
                GC.Collect();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                //HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                MainWindow.mainWindow.Closing += ParentWin_Closing;

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                setTimer();

                translate();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void ParentWin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //isClose = true;
            //UserControl_Unloaded(this, null);
        }
        private void translate()
        {

           
            ////////////////////////////////----Inventory----/////////////////////////////////
            dg_items.Columns[0].Header = AppSettings.resourcemanager.GetString("trNum");
            dg_items.Columns[1].Header = AppSettings.resourcemanager.GetString("trSectionLocation");
            dg_items.Columns[2].Header = AppSettings.resourcemanager.GetString("trItemUnit");
            dg_items.Columns[3].Header = AppSettings.resourcemanager.GetString("trRealAmount");
            dg_items.Columns[4].Header = AppSettings.resourcemanager.GetString("trInventoryAmount");
            dg_items.Columns[5].Header = AppSettings.resourcemanager.GetString("trDestoryCount");

            txt_inventoryDetails.Text = AppSettings.resourcemanager.GetString("trStocktakingDetails");
            txt_titleDataGrid.Text = AppSettings.resourcemanager.GetString("trStocktakingItems");
            btn_archive.Content = AppSettings.resourcemanager.GetString("trSave");

            //bdr_archive.Background = Application.Current.Resources["MainColorBlue"] as SolidColorBrush;
            txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            txt_drafts.Text = AppSettings.resourcemanager.GetString("trDraft");
            txt_inventory.Text = AppSettings.resourcemanager.GetString("trInventory");
            txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_invoiceImages.Text = AppSettings.resourcemanager.GetString("trImages");
            txt_shortageTitle.Text = AppSettings.resourcemanager.GetString("trShortages");
            txt_destroyTitle.Text = AppSettings.resourcemanager.GetString("trDestructives");
        }
        #region refresh notifications
        private void setTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private async void timer_Tick(object sendert, EventArgs et)
        {
            try
            {

                if (inventory.inventoryId != 0)
                {
                    await refreshDocCount(inventory.inventoryId);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        
        private async Task refreshDocCount(int inventoryId)
        {        
            try
            {
                DocImage doc = new DocImage();
                int docCount = await doc.GetDocCount("Inventory", inventoryId);

                HelpClass.refreshNotification(md_docImage, ref _DocCount, docCount);
            }
            catch { }
        }
        #endregion
        private async Task fillItemLocations()
        {
            int sequence = 0;
            _DestroyAmount = 0;
            _ShortageAmount = 0;
            invItemsLocations.Clear();
            inventory = new Inventory();
            inventory = await inventory.getByBranch("d", MainWindow.branchLogin.branchId);
            if (inventory.inventoryId == 0)// there is no draft in branch
            {
                itemsLocations = await FillCombo.itemLocation.getAll(MainWindow.branchLogin.branchId);

                foreach (ItemLocation il in itemsLocations)
                {
                    sequence++;
                    InventoryItemLocation iil = new InventoryItemLocation();
                    iil.sequence = sequence;
                    iil.itemName = il.itemName;
                    iil.section = il.section;
                    iil.location = il.location;
                    iil.unitName = il.unitName;
                    iil.quantity = (int)il.quantity;
                    iil.itemLocationId = il.itemsLocId;
                    iil.isDestroyed = false;
                    iil.isFalls = false;
                    iil.amountDestroyed = 0;
                    iil.amount = 0;
                    iil.createUserId = MainWindow.userLogin.userId;

                    invItemsLocations.Add(iil);

                    //calculate _ShortageCount 
                    _ShortageAmount += (int)il.quantity;
                }
                tb_shortage.Text = _ShortageAmount.ToString();
                await inputEditable();
                dg_items.ItemsSource = invItemsLocations.ToList();
                if (firstTimeForDatagrid)
                {
                    await Task.Delay(1000);
                    dg_items.Items.Refresh();
                if(dg_items.Items.Count>0)
                        firstTimeForDatagrid = false;
                }
            }
            else
            {
                Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trDraftExist"), animation: ToasterAnimation.FadeIn);

            }
        }
        private async Task fillInventoryDetails()
        {
            int sequence = 0;
            _ShortageAmount = 0;
            _DestroyAmount = 0;
            invItemsLocations.Clear();

            if (_InventoryType == "d")
                inventory = await inventory.getByBranch("d", MainWindow.branchLogin.branchId);
            if (inventory.inventoryId == 0)// there is no draft in branch
            {
                itemsLocations = await FillCombo.itemLocation.getAll(MainWindow.branchLogin.branchId);
                foreach (ItemLocation il in itemsLocations)
                {
                    sequence++;
                    InventoryItemLocation iil = new InventoryItemLocation();
                    iil.sequence = sequence;
                    iil.itemName = il.itemName;
                    iil.section = il.section;
                    iil.location = il.location;
                    iil.unitName = il.unitName;
                    iil.quantity = (int)il.quantity;
                    iil.itemLocationId = il.itemsLocId;
                    iil.isDestroyed = false;
                    iil.isFalls = false;
                    iil.amountDestroyed = 0;
                    iil.amount = 0;
                    iil.createUserId = MainWindow.userLogin.userId;

                    invItemsLocations.Add(iil);

                    //calculate _ShortageCount 
                    _ShortageAmount += (int)il.quantity;
                }
            }
            else
            {
                txt_inventoryNum.Text = inventory.num;
                txt_inventoryDate.Text = inventory.createDate.ToString();
                invItemsLocations = await invItemModel.GetAll(inventory.inventoryId);
                foreach (InventoryItemLocation il in invItemsLocations)
                {
                    _ShortageAmount += (int)(il.quantity - il.amount);
                    _DestroyAmount += (int)il.amountDestroyed;
                }
            }

            tb_shortage.Text = _ShortageAmount.ToString();
            tb_destroy.Text = _DestroyAmount.ToString();
            await inputEditable();
            dg_items.ItemsSource = invItemsLocations.ToList();
            if (firstTimeForDatagrid)
            {
                await Task.Delay(1000);
                dg_items.Items.Refresh();
                if(dg_items.Items.Count>0)
                    firstTimeForDatagrid = false;
            }
        }
        private async Task inputEditable()
        {
            if (_InventoryType == "d") // draft
            {
                dg_items.Columns[4].IsReadOnly = false;
                dg_items.Columns[5].IsReadOnly = false;
                btn_archive.IsEnabled = true;
                btn_deleteInventory.Visibility = Visibility.Collapsed;
                btn_archive.Content = AppSettings.resourcemanager.GetString("trSave");
               //bdr_archive.Background = Application.Current.Resources["MainColorBlue"] as SolidColorBrush;
            }
            else if (_InventoryType == "n") // normal saved
            {
                dg_items.Columns[4].IsReadOnly = true;
                dg_items.Columns[5].IsReadOnly = true;
                btn_archive.Content = AppSettings.resourcemanager.GetString("trArchive");
               // bdr_archive.Background = Application.Current.Resources["mediumRed"] as SolidColorBrush;
                if (HelpClass.isAdminPermision())
                    btn_deleteInventory.Visibility = Visibility.Visible;
                bool shortageManipulated = await inventory.shortageIsManipulated(inventory.inventoryId);
                if (shortageManipulated)
                    btn_archive.IsEnabled = true;
                else
                    btn_archive.IsEnabled = false;

            }
        }
        private void Dg_items_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                TextBox t = new TextBox();
                InventoryItemLocation row = e.Row.Item as InventoryItemLocation;
                var index = e.Row.GetIndex();
                if (dg_items.SelectedIndex != -1 && index < invItemsLocations.Count)
                {
                    var columnName = e.Column.Header.ToString();
                    t = e.EditingElement as TextBox;
                    int oldCount;
                    int newCount;
                    if (t != null && columnName == AppSettings.resourcemanager.GetString("trDestoryCount"))
                    {
                        oldCount = (int)row.amountDestroyed;
                        newCount = int.Parse(t.Text);
                        if (newCount > invItemsLocations[index].quantity)
                        {

                            t.Text = "0";
                            newCount = 0;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorDistroyMoreQuanToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                        _DestroyAmount -= oldCount;
                        _DestroyAmount += newCount;
                        tb_destroy.Text = _DestroyAmount.ToString();
                    }
                    if (t != null && columnName == AppSettings.resourcemanager.GetString("trInventoryAmount"))
                    {
                        oldCount = (int)row.amount;
                        newCount = int.Parse(t.Text);
                        if (newCount > invItemsLocations[index].quantity)
                        {
                            t.Text = "0";
                            newCount = 0;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorShortMoreQuanToolTip"), animation: ToasterAnimation.FadeIn);
                        }

                        _ShortageAmount -= (int)(invItemsLocations[index].quantity - oldCount);
                        _ShortageAmount += (int)invItemsLocations[index].quantity - newCount;
                        tb_shortage.Text = _ShortageAmount.ToString();
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
        private async void clearInventory()
        {
            _InventoryType = "d";
            inventory = new Inventory();
            txt_inventoryDate.Text = "";
            txt_inventoryNum.Text = "";
            md_docImage.Badge = "";
            _DestroyAmount = 0;
            _ShortageAmount = 0;
            invItemsLocations.Clear();
            txt_titleDataGrid.Text = AppSettings.resourcemanager.GetString("trInventoryDraft");
            dg_items.ItemsSource = null;
            btn_deleteInventory.Visibility = Visibility.Collapsed;
            await inputEditable();
        }
        private async Task addInventory(string invType)
        {
            if (inventory.inventoryId == 0)
            {
                inventory.branchId = MainWindow.branchLogin.branchId;
                inventory.posId = MainWindow.posLogin.posId;
                inventory.createUserId = MainWindow.userLogin.userId;
                inventory.isActive = 1;
                if(invType == "d")
                    inventory.num = await inventory.generateInvNumber("ind", MainWindow.branchLogin.branchId);
            }
            if (invType == "n")
                inventory.num = await inventory.generateInvNumber("in", MainWindow.branchLogin.branchId);
            inventory.inventoryType = invType;
            inventory.updateUserId = MainWindow.userLogin.userId;

            int inventoryId = await inventory.save(inventory);

            if (inventoryId != 0)
            {
                // add inventory details
                int res = await invItemModel.save(invItemsLocations, inventoryId);
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                clearInventory();
            }
            else
                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        }
        private async void Btn_newInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (!_InventoryType.Equals("n") && invItemsLocations.Count > 0)
                {
                    await addInventory("d"); // d:draft
                    clearInventory();
                }
                else
                    await fillItemLocations();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_draft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                inventory = await inventory.getByBranch("d", MainWindow.branchLogin.branchId);
                if (inventory.inventoryId == 0)
                {
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoDraft"), animation: ToasterAnimation.FadeIn);
                }
                else
                {
                    if (_InventoryType == "d" && invItemsLocations.Count > 0)
                    { }
                    else
                    {
                        txt_titleDataGrid.Text = AppSettings.resourcemanager.GetString("trInventoryDraft");
                        _InventoryType = "d";
                        await refreshDocCount(inventory.inventoryId);
                        await fillInventoryDetails();
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
        private async void Btn_Inventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (_InventoryType.Equals("d") && invItemsLocations.Count > 0)
                {
                    #region Accept
                    MainWindow.mainWindow.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trSaveStockTakingNotification");
                    w.ShowDialog();
                    MainWindow.mainWindow.Opacity = 1;
                    #endregion
                    if (w.isOk)
                    {
                        await addInventory("d"); // d:draft
                    }
                    else
                        clearInventory();
                }
                inventory = await inventory.getByBranch("n", MainWindow.branchLogin.branchId);
                if (inventory.inventoryId == 0)
                {
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoInventory"), animation: ToasterAnimation.FadeIn);

                }
                else
                {
                    if (_InventoryType == "n" && invItemsLocations.Count > 0)
                    { }
                    else
                    {
                        txt_titleDataGrid.Text = AppSettings.resourcemanager.GetString("trStocktaking");
                        _InventoryType = "n";
                        await refreshDocCount(inventory.inventoryId);
                        await fillInventoryDetails();
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
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(createInventoryPermission, FillCombo.groupObjects, "one"))
                {
                    var inv = await inventory.getByBranch("n", MainWindow.branchLogin.branchId);
                    if (inv.inventoryId == 0)
                        await addInventory("n"); // n:normal
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trWarningOneInventory"), animation: ToasterAnimation.FadeIn);
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
        private async void Btn_archive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(archivingPermission, FillCombo.groupObjects, "one") || FillCombo.groupObject.HasPermissionAction(createInventoryPermission, FillCombo.groupObjects, "one"))
                {
                    if (invItemsLocations.Count > 0)
                    {
                        if (_InventoryType == "n")
                            await addInventory("a"); // a:archived
                        else if (_InventoryType == "d")
                        {
                            var inv = await inventory.getByBranch("n", MainWindow.branchLogin.branchId);
                            if (inv.inventoryId == 0)
                                await addInventory("n"); // n:normal
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trWarningOneInventory"), animation: ToasterAnimation.FadeIn);
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


        #region report

        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Storage\stocktackingOperations\Ar\ArStocktaking.rdlc";
            }
            else
            {
                addpath = @"\Reports\Storage\stocktackingOperations\En\EnStocktaking.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            //D:\myproj\posproject5\laundryApp\laundryApp\View\storage\stocktakingOperations\uc_Stocktaking.xaml.cs
            ReportCls.checkLang();

            clsReports.Stocktaking(invItemsLocations, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);
            paramarr.Add(new ReportParameter("InventoryNum", inventory.num));
            paramarr.Add(new ReportParameter("InventoryDate",inventory.createDate.ToString()));

            /*
             * txt_inventoryNum  trInventoryNum
txt_inventoryDate trInventoryDate
   txt_inventoryNum.Text = inventory.num;
                txt_inventoryDate.Text = inventory.createDate.ToString();
             * */
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


        private void Btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                   
                    if (invItemsLocations != null)
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

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    if (invItemsLocations != null)
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

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    #region
                    if (invItemsLocations != null)
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
        //private void BuildReport()
        //{
        //    List<ReportParameter> paramarr = new List<ReportParameter>();

        //    string addpath;
        //    bool isArabic = ReportCls.checkLang();
        //    if (isArabic)
        //    {
        //        addpath = @"\Reports\Store\Ar\ArInventory.rdlc";//////////??????????
        //    }
        //    else
        //        addpath = @"\Reports\Store\En\Inventory.rdlc";/////////???????????
        //    string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

        //    ReportCls.checkLang();

        //    clsReports.inventoryReport(invItemsLocations, rep, reppath, paramarr);////////////?????
        //    clsReports.setReportLanguage(paramarr);
        //    clsReports.Header(paramarr);

        //    rep.SetParameters(paramarr);

        //    rep.Refresh();
        //}
        private async void Btn_invoiceImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (inventory != null && inventory.inventoryId != 0)
                {
                    Window.GetWindow(this).Opacity = 0.2;

                    wd_uploadImage w = new wd_uploadImage();

                    w.tableName = "Inventory";
                    w.tableId = inventory.inventoryId;
                    w.docNum = inventory.num;
                    w.ShowDialog();
                    await refreshDocCount(inventory.inventoryId);
                    Window.GetWindow(this).Opacity = 1;
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trChooseInvoiceToolTip"), animation: ToasterAnimation.FadeIn);
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_deleteInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                
                    HelpClass.StartAwait(grid_main);
                if (inventory.inventoryId != 0)
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                    w.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                    if (w.isOk)
                    {
                        int res = await inventory.delete(inventory.inventoryId, MainWindow.userLogin.userId, false);
                        if (res > 0)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                            clearInventory();
                        }
                        else
                            Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
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

        
    }
}
