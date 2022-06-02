using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace laundryApp.View.kitchen
{
    /// <summary>
    /// Interaction logic for uc_consumptionRawMaterials.xaml
    /// </summary>
    public partial class uc_consumptionRawMaterials : UserControl
    {
        string consumptionPermission = "consumptionRawMaterials_consumption";
        string reportPermission = "consumptionRawMaterials_report";
        public static uc_consumptionRawMaterials _instance;
        public static uc_consumptionRawMaterials Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_consumptionRawMaterials();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_consumptionRawMaterials()
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
        ObservableCollection<BillDetailsPurchase> billDetails = new ObservableCollection<BillDetailsPurchase>();
        public static bool archived = false;
        public static bool isFromReport = false;
        List<ItemUnit> itemUnits;
        public static Invoice invoice = new Invoice();   
        List<ItemTransfer> invoiceItems;
        public List<Control> controls;
        static public string _InvType = "fbcd"; //draft import

        static private int _SequenceNum = 0;
        static private int _Count = 0;
        static public int _invoiceId;
        #region for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        #endregion
        #region notifications
        int _InvCount = 0;
        bool _IsFocused = false;
        private static DispatcherTimer timer;
        #endregion
        Item item = new Item();
        public static List<string> requiredControlList;
        private void translate()
        {
            ////////////////////////////////----Order----/////////////////////////////////
            dg_billDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_billDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_billDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_billDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trQuantity");

            txt_itemsStorage.Text = AppSettings.resourcemanager.GetString("trItemsStorage");
            txt_orders.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_pdf.Text = AppSettings.resourcemanager.GetString("trPdf");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_count.Text = AppSettings.resourcemanager.GetString("trCount");
            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trConsumptionRawMaterials");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));

            tt_error_previous.Content = AppSettings.resourcemanager.GetString("trPrevious");
            tt_error_next.Content = AppSettings.resourcemanager.GetString("trNext");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                MainWindow.mainWindow.KeyDown -= HandleKeyPress;
                timer.Stop();
                Instance = null;
                GC.Collect();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
            }
        }
        public async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "branch", };

                MainWindow.mainWindow.KeyDown += HandleKeyPress;

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                setNotifications();
                setTimer();

                await FillCombo.RefreshPurchaseItems();
                //List all the UIElement in the VisualTree
                controls = new List<Control>();
                FindControl(this.grid_main, controls);

                HelpClass.EndAwait(grid_main);
                tb_barcode.Focus();
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public void FindControl(DependencyObject root, List<Control> controls)
        {
            controls.Clear();
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var control = current as Control;
                if (control != null && control.IsTabStop)
                {
                    controls.Add(control);
                }
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    if (child != null)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }

        #region loading
        async void loading_RefrishItems()
        {
            try
            {
                if(FillCombo.purchaseItems == null)
                    await FillCombo.RefreshPurchaseItems();
            }
            catch (Exception)
            { }
        }
        #endregion
        #region timer to refresh notifications
        private void setTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(30);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sendert, EventArgs et)
        {
            try
            {
                setNotifications();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region notifications
        private void setNotifications()
        {
            try
            {
                refreshInvNotification();
            }
            catch (Exception ex)
            {
                //HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void refreshInvNotification()
        {
            try
            {
                string invoiceType = "fbc";
                int duration = 1;
                int draftCount = await invoice.GetCountBranchInvoices(invoiceType, 0,MainWindow.branchLogin.branchId, duration);
                if (invoice.invType == "fbc")
                    draftCount--;

                HelpClass.refreshNotification(md_invCount, ref _InvCount, draftCount);
            }
            catch { }
        }

        #endregion
        #region navigation buttons
        public void navigateBtnActivate()
        {
            int index = FillCombo.invoices.IndexOf(FillCombo.invoices.Where(x => x.invoiceId == _invoiceId).FirstOrDefault());
            if (index == FillCombo.invoices.Count - 1)
                btn_next.IsEnabled = false;
            else
                btn_next.IsEnabled = true;

            if (index == 0)
                btn_previous.IsEnabled = false;
            else
                btn_previous.IsEnabled = true;
        }
        private async void Btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                int index = FillCombo.invoices.IndexOf(FillCombo.invoices.Where(x => x.invoiceId == _invoiceId).FirstOrDefault());
                index++;
                clear();
                invoice = FillCombo.invoices[index];
                _InvType = invoice.invType;
                _invoiceId = invoice.invoiceId;
                navigateBtnActivate();
                await fillInvoiceInputs(invoice);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_previous_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                int index = FillCombo.invoices.IndexOf(FillCombo.invoices.Where(x => x.invoiceId == _invoiceId).FirstOrDefault());
                index--;
                clear();
                invoice = FillCombo.invoices[index];
                _InvType = invoice.invType;
                _invoiceId = invoice.invoiceId;
                navigateBtnActivate();
                await fillInvoiceInputs(invoice);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region barcode
        // read item from barcode
        private async void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_IsFocused)
                {
                    Control c = CheckActiveControl();
                    if (c == null)
                        tb_barcode.Focus();
                    _IsFocused = true;
                }

                HelpClass.StartAwait(grid_main);
                if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                {
                    switch (e.Key)
                    {
                        case Key.P:
                            //handle P key
                            Btn_printInvoice_Click(null, null);
                            break;
                        case Key.S:
                            //handle S key
                            Btn_save_Click(btn_save, null);
                            break;
                        case Key.I:
                            //handle S key
                            Btn_items_Click(null, null);
                            break;
                    }
                }
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 50)
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
                else if (e.Key >= Key.A && e.Key <= Key.Z)
                    digit = e.Key.ToString();
                else if (e.Key == Key.OemMinus)
                {
                    digit = "-";
                }
                _BarcodeStr += digit;
                _lastKeystroke = DateTime.Now;
                // process barcode

                if (e.Key.ToString() == "Return" && _BarcodeStr != "")
                {
                    await dealWithBarcode(_BarcodeStr);
                    e.Handled = true;
                }
                _BarcodeStr = "";

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public Control CheckActiveControl()
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Control c = controls[i];
                if (c.IsFocused)
                {
                    return c;
                }
            }
            return null;
        }
        private async Task dealWithBarcode(string barcode)
        {
            tb_barcode.Text = barcode;
            // get item matches barcode
            if (FillCombo.itemUnitList != null)
            {
                ItemUnit unit1 = FillCombo.itemUnitList.Find(c => c.barcode == tb_barcode.Text.Trim());

                // get item matches the barcode
                if (unit1 != null)
                {
                    int itemId = (int)unit1.itemId;
                    if (unit1.itemId != 0)
                    {
                        int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == unit1.itemUnitId).FirstOrDefault());

                        if (index == -1)//item doesn't exist in bill
                        {
                            // get item units
                            itemUnits = await FillCombo.itemUnit.GetItemUnits(itemId);
                            //get item from list
                            item = FillCombo.purchaseItems.ToList().Find(i => i.itemId == itemId);

                            int count = 1;
                            _Count++;

                            addRowToBill(item.name, item.itemId, unit1.mainUnit, unit1.itemUnitId, count);
                        }
                        else // item exist prevoiusly in list
                        {
                            billDetails[index].Count++;
                            _Count++;
                        }
                        refrishBillDetails();
                    }
                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorItemNotFoundToolTip"), animation: ToasterAnimation.FadeIn);
                }
            }
            tb_barcode.Clear();
        }
        private async void Tb_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (e.Key == Key.Return)
                {
                    string barcode = "";
                    if (_BarcodeStr.Length < 13)
                    {
                        barcode = tb_barcode.Text;
                        await dealWithBarcode(barcode);
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
        #region save
        private bool validateItemUnits()
        {
            bool valid = true;
            if (billDetails.Count == 0)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAddInvoiceWithoutItems"), animation: ToasterAnimation.FadeIn);
                return false;
            }
            for (int i = 0; i < billDetails.Count; i++)
            {
                if (billDetails[i].itemUnitId == 0)
                {
                    valid = false;
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemWithNoUnit"), animation: ToasterAnimation.FadeIn);

                    return valid;
                }
            }
            return valid;
        }
        private async Task<Boolean> checkItemsAmounts()
        {
            Boolean available = true;
            for (int i = 0; i < billDetails.Count; i++)
            {
                int availableAmountInBranch = await FillCombo.itemLocation.getAmountInBranch(billDetails[i].itemUnitId, MainWindow.branchLogin.branchId,1);
                int amountInBill = await getAmountInBill(billDetails[i].itemId, billDetails[i].itemUnitId, billDetails[i].ID);
                int availableAmount = availableAmountInBranch - amountInBill;

                if (availableAmount < billDetails[i].Count )
                {
                    available = false;
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableFromToolTip") + " " + billDetails[i].Product, animation: ToasterAnimation.FadeIn);
                    return available;
                }
            }
            return available;
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(consumptionPermission, FillCombo.groupObjects, "one"))
                {
                    bool valid = validateItemUnits();
                    bool validateAmount = await checkItemsAmounts();
                    if (valid && validateAmount)
                    {
                        await addInvoice("fbc");
                        clear();
                        setNotifications();

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
        async Task addInvoice(string invType)
        {
            #region invoice
            invoice.invNumber = await invoice.generateInvNumber("fbc", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
            invoice.branchCreatorId = MainWindow.branchLogin.branchId;
            invoice.posId = MainWindow.posLogin.posId;
            invoice.invType = invType;
            invoice.branchId = MainWindow.branchLogin.branchId;
            invoice.createUserId = MainWindow.userLogin.userId;
            invoice.updateUserId = MainWindow.userLogin.userId;
            #endregion
            #region invoice items
            invoiceItems = new List<ItemTransfer>();
            ItemTransfer itemT;
            for (int i = 0; i < billDetails.Count; i++)
            {
                itemT = new ItemTransfer();

                itemT.invoiceId = 0;
                itemT.quantity = billDetails[i].Count;
                itemT.price = billDetails[i].Price;
                itemT.itemUnitId = billDetails[i].itemUnitId;
                itemT.createUserId = MainWindow.userLogin.userId;

                invoiceItems.Add(itemT);
            }
            #endregion
            int invoiceId = await FillCombo.invoice.saveInvoiceWithItems(invoice,invoiceItems);
            if (invoiceId > 0)
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                // decrease amount from kitchen
                await FillCombo.itemLocation.decreaseAmountsInKitchen(invoiceItems, MainWindow.branchLogin.branchId, MainWindow.userLogin.userId); // update item quantity in DB
            }
            else
                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        }
        #endregion
        #region events


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {

                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
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
        private void space_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {

                var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                    e.Handled = false;

                else
                    e.Handled = true;
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
        Invoice prInvoice = new Invoice();
        List<ReportParameter> paramarr = new List<ReportParameter>();
        Branch branchModel = new Branch();

        Invoice invoiceModel = new Invoice();


        public async Task BuildReport()
        {
            if (invoice.invoiceId > 0)
            {

                prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);
                paramarr = new List<ReportParameter>();

                string reppath = reportclass.SpendingRequestRdlcpath();
                if (prInvoice.invoiceId > 0)
                {
                    invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                    if (prInvoice.agentId != null)
                    {
                        Agent agentinv = new Agent();
                        //  agentinv = vendors.Where(X => X.agentId == prInvoice.agentId).FirstOrDefault();
                        agentinv = await agentinv.getAgentById((int)prInvoice.agentId);
                        prInvoice.agentCode = agentinv.code;
                        //new lines
                        prInvoice.agentName = agentinv.name;
                        prInvoice.agentCompany = agentinv.company;
                    }
                    else
                    {

                        prInvoice.agentCode = "-";
                        //new lines
                        prInvoice.agentName = "-";
                        prInvoice.agentCompany = "-";
                    }
                    User employ = new User();
                    employ = await employ.getUserById((int)prInvoice.updateUserId);
                    prInvoice.uuserName = employ.name;
                    prInvoice.uuserLast = employ.lastname;


                    Branch branch = new Branch();
                    //branch = await branchModel.getBranchById((int)prInvoice.branchCreatorId);
                    //if (branch.branchId > 0)
                    //{
                    //    prInvoice.branchCreatorName = branch.name;
                    //}
                    //branch reciver
                    if (prInvoice.branchId != null)
                    {
                        if (prInvoice.branchId > 0)
                        {
                            branch = await branchModel.getBranchById((int)prInvoice.branchId);
                            prInvoice.branchName = branch.name;
                        }
                        else
                        {
                            prInvoice.branchName = "-";
                        }

                    }
                    else
                    {
                        prInvoice.branchName = "-";
                    }
                    // end branch reciever


                    ReportCls.checkLang();
                    foreach (var i in invoiceItems)
                    {
                        i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                        i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));
                    }
                    clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                    clsReports.setReportLanguage(paramarr);
                    clsReports.Header(paramarr);
                    paramarr = reportclass.fillSpendingRequest(prInvoice, paramarr);


                    //if (prInvoice.invType == "p" || prInvoice.invType == "pw" || prInvoice.invType == "pbd" || prInvoice.invType == "pb" || prInvoice.invType == "pd" || prInvoice.invType == "isd" || prInvoice.invType == "is" || prInvoice.invType == "pbw")
                    //{
                    //    CashTransfer cachModel = new CashTransfer();
                    //    List<PayedInvclass> payedList = new List<PayedInvclass>();
                    //    payedList = await cachModel.GetPayedByInvId(prInvoice.invoiceId);
                    //    decimal sump = payedList.Sum(x => x.cash) ;
                    //    decimal deservd = (decimal)prInvoice.totalNet - sump;
                    //    //convertter
                    //    foreach (var p in payedList)
                    //    {
                    //        p.cash = decimal.Parse(reportclass.DecTostring(p.cash));
                    //    }
                    //    paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

                    //    paramarr.Add(new ReportParameter("sumP", reportclass.DecTostring(sump)));
                    //    paramarr.Add(new ReportParameter("deserved", reportclass.DecTostring(deservd)));
                    //    rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", payedList));


                    //}
                    //  multiplePaytable(paramarr);

                    rep.SetParameters(paramarr);
                    rep.Refresh();
                }
                else
                {


                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSaveInvoiceToPreview"), animation: ToasterAnimation.FadeIn);



                }
            }
        }
        private async void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
                if (invoice.invoiceId > 0)
                {
                    HelpClass.StartAwait(grid_main);

                    if (FillCombo.groupObject.HasPermissionAction(reportPermission, FillCombo.groupObjects, "one"))
                    {
                        #region
                        if (invoiceItems != null)
                        {
                            Window.GetWindow(this).Opacity = 0.2;
                            /////////////////////
                            string pdfpath = "";
                            pdfpath = @"\Thumb\report\temp.pdf";
                            pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                            await BuildReport();
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
                        }
                        //////////////////////////////////////
                        #endregion
                    }
                    else
                        Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);

                }
                else
                {
                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSaveInvoiceToPreview"), animation: ToasterAnimation.FadeIn);

                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportPermission, FillCombo.groupObjects, "one"))
                {
                    /////////////////////////////////////
                    ///  
                    if (invoiceItems != null)
                    {
                        Thread t1 = new Thread(() =>
                        {
                            printExport();
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
        private async void printExport()
        {
            await BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
            });
        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportPermission, FillCombo.groupObjects, "one"))
                {
                    if (invoiceItems != null)
                    {
                        Thread t1 = new Thread(() =>
                        {
                            pdfExport();
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
        private async void pdfExport()
        {

            await BuildReport();

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
        #endregion
        #region btn
        private void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clear();
                setNotifications();
            }
            catch { }
        }
        private async void Btn_orders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();

                w.condition = "order";
                w.invoiceType = "fbc";
                w.duration = 1;
                w.page = "consumption";
                w.icon = "invoices";
                w.title = AppSettings.resourcemanager.GetString("trInvoices");
                w.branchId = MainWindow.branchLogin.branchId;

                if (w.ShowDialog() == true)
                {
                    if (w.invoice != null)
                    {
                        invoice = w.invoice;
                        _InvType = invoice.invType;
                        _invoiceId = invoice.invoiceId;
                        isFromReport = false;
                        archived = false;
                        setNotifications();
                        await fillInvoiceInputs(invoice);                      
                        navigateBtnActivate();
                    }
                }
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_itemsStorage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                Window.GetWindow(this).Opacity = 0.2;
                wd_itemsStorage w = new wd_itemsStorage();
                    // w.ShowInTaskbar = false;
                w.ShowDialog();

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_items_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                Window.GetWindow(this).Opacity = 0.2;
                wd_purchaseItems w = new wd_purchaseItems();
                w.CardType = "consumption";
                // w.ShowInTaskbar = false;
                w.ShowDialog();
                if (w.isActive)
                {
                    for (int i = 0; i < w.selectedItems.Count; i++)
                    {
                        int itemId = w.selectedItems[i];
                        await ChangeItemIdEvent(itemId);
                    }
                    refrishBillDetails();
                }

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
              
        #endregion
        #region DataGrid
        void deleteRowFromInvoiceItems(object sender, RoutedEventArgs e)
        {
            try
            {

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        BillDetailsPurchase row = (BillDetailsPurchase)dg_billDetails.SelectedItems[0];
                        int index = dg_billDetails.SelectedIndex;
                        // calculate new sum
                        _Count -= row.Count;

                        // remove item from bill
                        billDetails.RemoveAt(index);

                        ObservableCollection<BillDetailsPurchase> data = (ObservableCollection<BillDetailsPurchase>)dg_billDetails.ItemsSource;
                        data.Remove(row);

                        tb_count.Text = _Count.ToString();
                    }
                _SequenceNum = 0;
                for (int i = 0; i < billDetails.Count; i++)
                {
                    _SequenceNum++;
                    billDetails[i].ID = _SequenceNum;
                }
                refrishBillDetails();

            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dg_billDetails_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                TextBox t = e.EditingElement as TextBox;
                var columnName = e.Column.Header.ToString();

                BillDetailsPurchase row = e.Row.Item as BillDetailsPurchase;
                int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == row.itemUnitId && p.OrderId == row.OrderId).FirstOrDefault());

                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds < 100)
                {
                    t.Text = billDetails[index].Count.ToString();
                }
                else
                {

                    int oldCount = row.Count;
                    int newCount = 0;
                    if (columnName == AppSettings.resourcemanager.GetString("trQuantity"))
                    {
                        newCount = int.Parse(t.Text);
                        if (newCount < 0)
                        {
                            newCount = 0;
                            t.Text = "0";
                        }

                    }
                    else
                        newCount = row.Count;
                    //"tb_amont"
                    if (columnName == AppSettings.resourcemanager.GetString("trQuantity"))
                    {
                        //availableAmount = await getAvailableAmount(row.itemId, row.itemUnitId, MainWindow.branchLogin.branchId, row.ID);

                        #region caculate available amount for this bill
                        int availableAmountInBranch = await FillCombo.itemLocation.getAmountInBranch(row.itemUnitId, MainWindow.branchLogin.branchId,1);
                        int amountInBill = await getAmountInBill(row.itemId, row.itemUnitId, row.ID);
                        int availableAmount = availableAmountInBranch - amountInBill;
                        #endregion

                        if (availableAmount < newCount)
                        {

                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableToolTip"), animation: ToasterAnimation.FadeIn);
                            newCount =  availableAmount;
                            t.Text = newCount.ToString();
                            row.Count = (int)newCount;
                        }
                        else
                        {
                            if (!t.Text.Equals(""))
                                newCount = int.Parse(t.Text);
                            else
                                newCount = 0;
                            if (newCount < 0)
                            {
                                newCount = 0;
                                t.Text = "0";
                            }
                        }
                    }
                    else
                        newCount = row.Count;



                    _Count -= oldCount;
                    _Count += newCount;

                    //  refresh count text box
                    tb_count.Text = _Count.ToString();

                    // update item in billdetails           
                    billDetails[index].Count = (int)newCount;
                    if (invoiceItems != null)
                        invoiceItems[index].quantity = (int)newCount;
                }

                refrishDataGridItems();

            }
            catch (Exception ex)
            {
                //
                //    HelpClass.EndAwait(grid_main);
               // HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region calculate quantity
        private async Task<int> getAmountInBill(int itemId, int itemUnitId, int ID)
        {
            int quantity = 0;
            var itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == itemId).ToList();

            var smallUnits = await FillCombo.itemUnit.getSmallItemUnits(itemId, itemUnitId);
            foreach (ItemUnit u in itemUnits)
            {
                var isInBill = billDetails.ToList().Find(x => x.itemUnitId == (int)u.itemUnitId && x.ID != ID); // unit exist in invoice
                if (isInBill != null)
                {
                    var isSmall = smallUnits.Find(x => x.itemUnitId == (int)u.itemUnitId);
                    int unitValue = 0;

                    int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == u.itemUnitId).FirstOrDefault());
                    int count = billDetails[index].Count;
                    if (itemUnitId == u.itemUnitId)
                    {
                        quantity += count;
                    }
                    else if (isSmall != null) // from-unit is bigger than to-unit
                    {
                        unitValue = await FillCombo.itemUnit.largeToSmallUnitQuan(itemUnitId, (int)u.itemUnitId);
                        quantity += count / unitValue;
                    }
                    else
                    {
                        unitValue = await FillCombo.itemUnit.smallToLargeUnit(itemUnitId, (int)u.itemUnitId);

                        if (unitValue != 0)
                        {
                            quantity += count * unitValue;
                        }
                    }

                }
            }
            return quantity;
        }
        #endregion
        //private async Task<int> getAvailableAmount(int itemId, int itemUnitId, int branchId, int ID)
        //{
        //    var itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == itemId).ToList();
        //    int availableAmount = await FillCombo.itemLocation.getAmountInBranch(itemUnitId, branchId,1);
        //    var smallUnits = await FillCombo.itemUnit.getSmallItemUnits(itemId, itemUnitId);
        //    foreach (ItemUnit u in itemUnits)
        //    {
        //        var isInBill = billDetails.ToList().Find(x => x.itemUnitId == (int)u.itemUnitId && x.ID != ID); // unit exist in invoice
        //        if (isInBill != null)
        //        {
        //            var isSmall = smallUnits.Find(x => x.itemUnitId == (int)u.itemUnitId);
        //            int unitValue = 0;

        //            int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == u.itemUnitId).FirstOrDefault());
        //            int quantity = billDetails[index].Count;
        //            if (itemUnitId == u.itemUnitId)
        //            { }
        //            else if (isSmall != null) // from-unit is bigger than to-unit
        //            {
        //                unitValue = await FillCombo.itemUnit.largeToSmallUnitQuan(itemUnitId, (int)u.itemUnitId);
        //                quantity = quantity / unitValue;
        //            }
        //            else
        //            {
        //                unitValue = await FillCombo.itemUnit.smallToLargeUnit(itemUnitId, (int)u.itemUnitId);

        //                if (unitValue != 0)
        //                {
        //                    quantity = quantity * unitValue;
        //                }
        //            }
        //            availableAmount -= quantity;
        //        }
        //    }
        //    return availableAmount;
        //}
        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);


                //billDetails
                int count = 0;
                foreach (var item in billDetails)
                {
                    if (dg_billDetails.Items.Count != 0)
                    {
                        if (dg_billDetails.Items.Count > 1)
                        {
                            DataGridCell cell = null;
                            try
                            {
                                cell = DataGridHelper.GetCell(dg_billDetails, count, 3);
                            }
                            catch
                            { }
                            if (cell != null)
                            {
                                var cp = (ContentPresenter)cell.Content;
                                var combo = (ComboBox)cp.ContentTemplate.FindName("cbm_unitItemDetails", cp);
                                //var combo = (combo)cell.Content;
                                combo.SelectedValue = (int)item.itemUnitId;
                            }
                        }
                    }
                    count++;
                }


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Cbm_unitItemDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var cmb = sender as ComboBox;
                if (dg_billDetails.SelectedIndex != -1 && cmb.SelectedValue != null)
                {
                    int _datagridSelectedIndex = dg_billDetails.SelectedIndex;
                    TextBlock t = col_amount.GetCellContent(dg_billDetails.Items[_datagridSelectedIndex]) as TextBlock;

                    if (dg_billDetails.SelectedIndex != -1 && cmb != null)
                    {
                        billDetails[dg_billDetails.SelectedIndex].itemUnitId = (int)cmb.SelectedValue;
                        if (_InvType == "fbc")
                            cmb.IsEnabled = false;
                        else
                            cmb.IsEnabled = true;


                        #region check amount & calc new count
                        int oldCount = billDetails[dg_billDetails.SelectedIndex].Count;
                        int newCount = 0;

                        newCount = int.Parse(t.Text);
                        if (newCount < 0)
                        {
                            newCount = 0;
                            t.Text = "0";
                        }
                        //availableAmount = await getAvailableAmount(row.itemId, row.itemUnitId, MainWindow.branchLogin.branchId, row.ID);

                        #region caculate available amount for this bill
                        int availableAmountInBranch = await FillCombo.itemLocation.getAmountInBranch(billDetails[dg_billDetails.SelectedIndex].itemUnitId, MainWindow.branchLogin.branchId, 1);
                        int amountInBill = await getAmountInBill(billDetails[dg_billDetails.SelectedIndex].itemId, billDetails[dg_billDetails.SelectedIndex].itemUnitId, billDetails[dg_billDetails.SelectedIndex].ID);
                        int availableAmount = availableAmountInBranch - amountInBill;
                        #endregion

                        if (availableAmount < newCount)
                        {

                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableToolTip"), animation: ToasterAnimation.FadeIn);
                            newCount = availableAmount;
                            t.Text = newCount.ToString();
                            billDetails[dg_billDetails.SelectedIndex].Count = (int)newCount;
                        }
                        else
                        {
                            if (!t.Text.Equals(""))
                                newCount = int.Parse(t.Text);
                            else
                                newCount = 0;
                            if (newCount < 0)
                            {
                                newCount = 0;
                                t.Text = "0";
                            }
                        }

                        _Count -= oldCount;
                        _Count += newCount;

                        //  refresh count text box
                        tb_count.Text = _Count.ToString();

                        // update item in billdetails           
                        billDetails[dg_billDetails.SelectedIndex].Count = (int)newCount;
                        if (invoiceItems != null)
                            invoiceItems[dg_billDetails.SelectedIndex].quantity = (int)newCount;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cbm_unitItemDetails_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {

                //billDetails
                //if (billDetails.Count == 1)
                //{
                    var cmb = sender as ComboBox;
                    cmb.SelectedValue = (int)billDetails[0].itemUnitId;

                    if (_InvType == "fbc")
                        cmb.IsEnabled = false;
                    else
                        cmb.IsEnabled = true;
                //}

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Dg_billDetails_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            int column = dg_billDetails.CurrentCell.Column.DisplayIndex;
            if (dg_billDetails.SelectedIndex != -1 && column == 3)
                if (_InvType == "fbc")
                    e.Cancel = true;
        }
        private void Dg_billDetails_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            _IsFocused = true;
        }
        #endregion
        #region billdetails
        private void addRowToBill(string itemName, int itemId, string unitName, int itemUnitId, int count)
        {
            // increase sequence for each read
            _SequenceNum++;

            billDetails.Add(new BillDetailsPurchase()
            {
                ID = _SequenceNum,
                Product = item.name,
                itemId = item.itemId,
                Unit = unitName,
                itemUnitId = itemUnitId,
                Count = count,
            });

        }
        bool firstTimeForDatagrid = true;
        async void refrishBillDetails()
        {
            dg_billDetails.ItemsSource = null;
            dg_billDetails.ItemsSource = billDetails;
            if (firstTimeForDatagrid)
            {
                HelpClass.StartAwait(grid_main);
                await Task.Delay(1000);
                dg_billDetails.Items.Refresh();
                if(dg_billDetails.Items.Count>0)
                firstTimeForDatagrid = false;
                HelpClass.EndAwait(grid_main);
            }
            DataGrid_CollectionChanged(dg_billDetails, null);
            tb_count.Text = _Count.ToString();

        }
        void refrishDataGridItems()
        {
            dg_billDetails.ItemsSource = null;
            dg_billDetails.ItemsSource = billDetails;
            dg_billDetails.Items.Refresh();
            DataGrid_CollectionChanged(dg_billDetails, null);

        }
        public async Task ChangeItemIdEvent(int itemId)
        {
            item = FillCombo.purchaseItems.ToList().Find(c => c.itemId == itemId);

            if (item != null)
            {
                this.DataContext = item;

                // get item units
                itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == item.itemId).ToList();
                // search for default unit for purchase
                var defaultPurUnit = itemUnits.ToList().Find(c => c.defaultPurchase == 1);
                if (defaultPurUnit != null)
                {
                    int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == defaultPurUnit.itemUnitId ).FirstOrDefault());
                    // create new row in bill details data grid
                    if (index == -1)
                    {
                        addRowToBill(item.name, itemId, defaultPurUnit.unitName, defaultPurUnit.itemUnitId, 1);
                        _Count++;
                    }
                    else // item exist prevoiusly in list
                    {
                        billDetails[index].Count++;
                        billDetails[index].Total = billDetails[index].Count * billDetails[index].Price;
                    }
                }
                else
                {
                    addRowToBill(item.name, itemId, null, 0, 1);
                    _Count++;
                }
              
            }
        }
        private void clear()
        {
            _Count = 0;
            _SequenceNum = 0;
            _InvType = "fbcd";
            isFromReport = false;
            archived = false;
            invoice = new Invoice();
            tb_barcode.Clear();
            billDetails.Clear();
            txt_invNumber.Text = "";
            refrishBillDetails();
            inputEditable();
            btn_next.Visibility = Visibility.Collapsed;
            btn_previous.Visibility = Visibility.Collapsed;

            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        public async Task fillInvoiceInputs(Invoice invoice)
        {
            _Count = invoice.itemsCount;
            tb_count.Text = _Count.ToString();
            txt_invNumber.Text = invoice.invNumber;
            // build invoice details grid
            await buildInvoiceDetails();

            inputEditable();
        }
        public async Task buildInvoiceDetails()
        {
            //get invoice items
            invoiceItems = await invoice.GetInvoicesItems(invoice.invoiceId);
            // build invoice details grid
            _SequenceNum = 0;
            billDetails.Clear();
            foreach (ItemTransfer itemT in invoiceItems)
            {
                _SequenceNum++;
                decimal total = (decimal)(itemT.price * itemT.quantity);
                int orderId = 0;
                if (itemT.invoiceId != null)
                    orderId = (int)itemT.invoiceId;
                billDetails.Add(new BillDetailsPurchase()
                {
                    ID = _SequenceNum,
                    Product = itemT.itemName,
                    itemId = (int)itemT.itemId,
                    Unit = itemT.itemUnitId.ToString(),
                    itemUnitId = (int)itemT.itemUnitId,
                    Count = (int)itemT.quantity,
                    Price = (decimal)itemT.price,
                    Total = total,
                    OrderId = orderId,
                });
            }
            tb_barcode.Focus();

            refrishBillDetails();
        }
        private void inputEditable()
        {

            if (_InvType == "fbcd" ) // 
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete hidden
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                tb_barcode.IsEnabled = true;
                btn_save.IsEnabled = true;
                btn_items.IsEnabled = true;
            }
            else if (_InvType == "fbc") // food Beverages consumption
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Collapsed; //make delete hidden
                dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                tb_barcode.IsEnabled = false;
                btn_save.IsEnabled = false;
                btn_items.IsEnabled = false;
            }

            if (!isFromReport)
            {
                btn_next.Visibility = Visibility.Visible;
                btn_previous.Visibility = Visibility.Visible;
            }
        }
        #endregion

        
    }
}
