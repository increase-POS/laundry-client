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
using System.Threading;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.IO;

namespace laundryApp.View.storage.stocktakingOperations
{
    /// <summary>
    /// Interaction logic for uc_itemsShortage.xaml
    /// </summary>
    public partial class uc_itemsShortage : UserControl
    {
        public uc_itemsShortage()
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
        private static uc_itemsShortage _instance;
        public static uc_itemsShortage Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_itemsShortage();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        string savePermission = "itemsShortage_save";
        string reportsPermission = "itemsShortage_reports";

        InventoryItemLocation invItemLocModel = new InventoryItemLocation();
        InventoryItemLocation invItemLoc = new InventoryItemLocation();
        IEnumerable<InventoryItemLocation> inventoriesItems;
        IEnumerable<InventoryItemLocation> invItemLocsQuery;
        byte tgl_invItemLocState;
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
                requiredControlList = new List<string> { "amount", "fallCause" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                Keyboard.Focus(tb_amount);
                await refreshShortageDetails();
                await FillCombo.FillComboUsers(cb_user);
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
        #region loading
        private async Task refreshShortageDetails()
        {
            inventoriesItems = await invItemLocModel.GetShortageItem(MainWindow.branchLogin.branchId);
            dg_invItemLoc.ItemsSource = inventoriesItems;
        }
        #endregion
        private void translate()
        {

            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );
            ////////////////////////////////----Grid----/////////////////////////////////
            dg_invItemLoc.Columns[0].Header = AppSettings.resourcemanager.GetString("trInventoryNum");
            dg_invItemLoc.Columns[1].Header = AppSettings.resourcemanager.GetString("trDate");
            dg_invItemLoc.Columns[2].Header = AppSettings.resourcemanager.GetString("trSectionLocation");
            dg_invItemLoc.Columns[3].Header = AppSettings.resourcemanager.GetString("trItemUnit");
            dg_invItemLoc.Columns[4].Header = AppSettings.resourcemanager.GetString("trAmount");

            txt_title.Text = AppSettings.resourcemanager.GetString("trItemShortage");
            txt_shortage.Text = AppSettings.resourcemanager.GetString("trShortage");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_itemName, AppSettings.resourcemanager.GetString("trItemHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_amount, AppSettings.resourcemanager.GetString("trAmountShortageedHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_user, AppSettings.resourcemanager.GetString("trUserHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_fallCause, AppSettings.resourcemanager.GetString("trReasonOfShortageHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            btn_shortage.Content = AppSettings.resourcemanager.GetString("trShortage");
            btn_refresh.ToolTip = AppSettings.resourcemanager.GetString("trRefresh");

        }
        #region Add  - Search - Tgl - Clear - DG_SelectionChanged - refresh 

        private async void Btn_shortage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(savePermission, FillCombo.groupObjects, "one")  )
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        int itemUnitId = 0;
                        int itemId = 0;
                        int invoiceId = 0;
                        string serialNum = "";

                        itemUnitId = invItemLoc.itemUnitId;
                        itemId = invItemLoc.itemId;
                        invItemLoc.notes = tb_notes.Text;
                        invItemLoc.fallCause = tb_fallCause.Text;
                        #region add invoice

                        decimal price = (decimal)invItemLoc.avgPurchasePrice;
                        decimal total = price * int.Parse(tb_amount.Text);
                        FillCombo.invoice = new Invoice();
                        FillCombo.invoice.invNumber = await FillCombo.invoice.generateInvNumber("sh",MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                        FillCombo.invoice.branchCreatorId = MainWindow.branchLogin.branchId;
                        FillCombo.invoice.posId = MainWindow.posLogin.posId;
                        FillCombo.invoice.createUserId = MainWindow.userLogin.userId;
                        FillCombo.invoice.invType = "sh"; // shortage
                        FillCombo.invoice.total = total;
                        FillCombo.invoice.totalNet = total;
                        FillCombo.invoice.paid = 0;
                        FillCombo.invoice.deserved = FillCombo.invoice.totalNet;
                        FillCombo.invoice.notes = tb_notes.Text;

                        if (cb_user.SelectedIndex != -1 && cb_user.SelectedIndex != 0)
                            FillCombo.invoice.userId = (int)cb_user.SelectedValue;
                        #endregion

                        List<ItemTransfer> orderList = new List<ItemTransfer>();


                        orderList.Add(new ItemTransfer()
                        {
                            itemName = invItemLoc.itemName,
                            itemId = invItemLoc.itemId,
                            unitName = invItemLoc.unitName,
                            itemUnitId = invItemLoc.itemUnitId,
                            quantity = (long)invItemLoc.amount,
                            itemSerial = serialNum,
                            price = price,
                            invoiceId = 0,
                            inventoryItemLocId = invItemLoc.id,
                            createUserId = MainWindow.userLogin.userId,
                        });
                        invoiceId = await FillCombo.invoice.saveInvoiceWithItems(FillCombo.invoice,orderList);
                        if (invoiceId != 0)
                        {
                            FillCombo.invoice.invoiceId = invoiceId;
                            //await invoiceModel.saveInvoiceItems(orderList, invoiceId);
                            await invItemLoc.fallItem(invItemLoc);
                            if (cb_user.SelectedIndex != -1 && cb_user.SelectedIndex != 0)
                                await invItemLoc.ShortageRecordCash(FillCombo.invoice, (int)cb_user.SelectedValue);
                            await refreshShortageDetails();
                            Search();
                            Clear();
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                            Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                          
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
                if (inventoriesItems is null)
                    await refreshShortageDetails();
                tgl_invItemLocState = 1;
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
                if (inventoriesItems is null)
                    await refreshShortageDetails();
                tgl_invItemLocState = 0;
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
        private void Dg_invItemLoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_invItemLoc.SelectedIndex != -1)
                {
                    invItemLoc = dg_invItemLoc.SelectedItem as InventoryItemLocation;
                    this.DataContext = invItemLoc;
                    if (invItemLoc != null)
                    {
                        btn_shortage.IsEnabled = true;
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
                await refreshShortageDetails();
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
            if (inventoriesItems is null)
                await refreshShortageDetails();
            searchText = tb_search.Text.ToLower();
            invItemLocsQuery = inventoriesItems.Where(s => s.itemName.ToLower().Contains(searchText));
            RefreshInventoryItemLocationsView();
        }
        void RefreshInventoryItemLocationsView()
        {
            dg_invItemLoc.ItemsSource = invItemLocsQuery;
            txt_count.Text = invItemLocsQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new InventoryItemLocation();
            btn_shortage.IsEnabled = false;
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
        #region report
        
        // report
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
                addpath = @"\Reports\Storage\stocktackingOperations\Ar\ArItemsShortage.rdlc";
            }
            else
            {
                addpath = @"\Reports\Storage\stocktackingOperations\En\EnItemsShortage.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            clsReports.ItemsShortage(invItemLocsQuery, rep, reppath, paramarr);
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



        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    if (invItemLocsQuery != null)
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
                    if (invItemLocsQuery != null)
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    /////////////////////////////////////
                    ///  
                    if (invItemLocsQuery != null)
                    {
                        Thread t1 = new Thread(() =>
                        {
                            PrintRep();
                        });
                        t1.Start();
                    }
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
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "report") || HelpClass.isAdminPermision())
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
        //private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        //{//pie
        //    try
        //    {
        //        
        //            SectionData.StartAwait(grid_main);

        //        if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
        //        {
        //            #region
        //            Window.GetWindow(this).Opacity = 0.2;
        //            win_lvcCatalog win = new win_lvcCatalog(itemsQuery, 3);
        //            win.ShowDialog();
        //            Window.GetWindow(this).Opacity = 1;
        //            #endregion
        //        }
        //        else
        //            Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
        //        
        //            SectionData.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //            SectionData.EndAwait(grid_main);
        //        SectionData.ExceptionMessage(ex, this);
        //    }
        //}
       
         
        #endregion

        private void Btn_clearSerial_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {

                _serialCount = 0;
                lst_serials.Items.Clear();
            }
            catch (Exception ex)
            {
                SectionData.ExceptionMessage(ex, this);
            }
            */
        }
        private void Tb_serialNum_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            try
            {
                
                    SectionData.StartAwait(grid_main);


                if (e.Key == Key.Return && !tb_amount.Text.Equals(""))
                {
                    string s = tb_serialNum.Text;
                    int itemCount = int.Parse(tb_amount.Text);

                    if (!s.Equals(""))
                    {
                        int found = lst_serials.Items.IndexOf(s);

                        if (_serialCount == itemCount)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trWarningItemCountIs:") + " " + itemCount, animation: ToasterAnimation.FadeIn);
                        }
                        else if (found == -1)
                        {
                            lst_serials.Items.Add(tb_serialNum.Text);
                            _serialCount++;
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trWarningSerialExists"), animation: ToasterAnimation.FadeIn);
                    }
                    tb_serialNum.Clear();
                }
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
            */
        }

    }
}
