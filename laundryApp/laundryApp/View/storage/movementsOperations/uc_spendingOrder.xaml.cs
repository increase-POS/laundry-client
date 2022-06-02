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

namespace laundryApp.View.storage.movementsOperations
{
    /// <summary>
    /// Interaction logic for uc_spendingOrder.xaml
    /// </summary>
    public partial class uc_spendingOrder : UserControl
    {
        string savePermission = "spendingOrder_save";
        string reportsPermission = "spendingOrder_reports";
        private static uc_spendingOrder _instance;
        public static uc_spendingOrder Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_spendingOrder();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_spendingOrder()
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
        
        public Invoice invoice = new Invoice();
        Invoice generatedInvoice = new Invoice();
        List<ItemTransfer> invoiceItems;
        List<ItemTransfer> mainInvoiceItems;
        List<ItemUnit> itemUnits;
        public List<Control> controls;

        static private int _SequenceNum = 0;
        static private int _Count = 0;
        static private int _invoiceId;
        static public string _InvoiceType = "srd"; //spending request waiting
        #region for barcode

        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        bool _IsFocused = false;
        #endregion
        #region notification
        private static DispatcherTimer timer;
        int _OrdersCount = 0;
        int _DraftsCount = 0;
        public static bool archived = false;
        public static bool isFromReport = false;
        #endregion
        Item item = new Item();
       // IEnumerable<Item> items;
        public static List<string> requiredControlList;
        private void translate()
        {
           

            ////////////////////////////////----Order----/////////////////////////////////
            dg_billDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("trNum");
            dg_billDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_billDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_billDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trQuantity");

            txt_orders.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_pdf.Text = AppSettings.resourcemanager.GetString("trPdf");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_count.Text = AppSettings.resourcemanager.GetString("trCount");

            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trSpendingOrder");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));

            tt_error_previous.Content = AppSettings.resourcemanager.GetString("trPrevious");
            tt_error_next.Content = AppSettings.resourcemanager.GetString("trNext");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            txt_drafts.Text = AppSettings.resourcemanager.GetString("trDrafts");
            txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
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
                refreshDraftNotification();
                refreshOrdersNotification();
            }
            catch (Exception ex)
            {
            }
        }
        private async void refreshDraftNotification()
        {
            try
            {
                string invoiceType = "srd";
                int draftsCount = await FillCombo.invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, 2);
                if (invoice.invType == "srd")
                    draftsCount--;

                HelpClass.refreshNotification(md_draftsCount, ref _DraftsCount, draftsCount);
            }
            catch { }
        }
        private async void refreshOrdersNotification()
        {
            try
            {
                string invoiceType = "srw";
                int ordersCount = await invoice.GetCountBranchInvoices(invoiceType, 0, MainWindow.branchLogin.branchId);
                if (invoice.invType == "srw")
                    ordersCount--;

                HelpClass.refreshNotification(md_ordersCount, ref _OrdersCount, ordersCount);
            }
            catch { }
        }

        #endregion
        #region navigation buttons
        private void navigateBtnActivate()
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
            int codeindex = barcode.IndexOf("-");
            string prefix = "";
            if (codeindex >= 0)
                prefix = barcode.Substring(0, codeindex);
            prefix = prefix.ToLower();
            barcode = barcode.ToLower();
            switch (prefix)
            {
                case "sr":// this barcode for invoice               
                    Btn_newDraft_Click(null, null);
                    invoice = await FillCombo.invoice.GetInvoicesByNum(barcode);
                    _InvoiceType = invoice.invType;
                    if (invoice.invType == "srw")
                    {
                        await fillInvoiceInputs(invoice);
                    }
                    break;

                default: // if barcode for item
                         // get item matches barcode
                    if (FillCombo.itemUnitList != null && _InvoiceType == "srw")
                    {
                        // ItemUnit unit1 = barcodesList.ToList().Find(c => c.barcode == barcode.Trim());
                        ItemUnit unit1 = FillCombo.itemUnitList.ToList().Find(c => c.barcode == barcode.Trim() && FillCombo.purchaseTypes.Contains(c.type));

                        // get item matches the barcode
                        if (unit1 != null)
                        {
                            int itemId = (int)unit1.itemId;
                            if (unit1.itemId != 0)
                            {
                                int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == unit1.itemUnitId && p.OrderId == 0).FirstOrDefault());

                                if (index == -1)//item doesn't exist in bill
                                {
                                    // get item units
                                    //itemUnits = await FillCombo.itemUnit.GetItemUnits(itemId);
                                    itemUnits = FillCombo.itemUnitList.Where(c => c.itemId == itemId).ToList();
                                    //get item from list
                                    item = FillCombo.purchaseItems.ToList().Find(i => i.itemId == itemId);

                                    int count = 1;
                                    addRowToBill(item.name, item.itemId, unit1.mainUnit, unit1.itemUnitId, count);
                                }
                                else // item exist prevoiusly in list
                                {
                                    billDetails[index].Count++;
                                    billDetails[index].Total = billDetails[index].Count * billDetails[index].Price;
                                }
                                refrishBillDetails();
                            }
                        }
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorItemNotFoundToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    break;
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
        #region save - delete
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
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(savePermission, FillCombo.groupObjects, "one"))
                    {
                    bool valid = validateItemUnits();
                    if (valid)
                    {
                        wd_transItemsLocation w;
                        Window.GetWindow(this).Opacity = 0.2;
                        w = new wd_transItemsLocation();
                        List<ItemTransfer> orderList = new List<ItemTransfer>();
                        List<int> ordersIds = new List<int>();
                        foreach (BillDetailsPurchase d in billDetails)
                        {
                            if (d.Count == 0)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorQuantIsZeroToolTip"), animation: ToasterAnimation.FadeIn);
                                HelpClass.EndAwait(grid_main);
                                return;
                            }
                            else
                            {
                                orderList.Add(new ItemTransfer()
                                {
                                    itemName = d.Product,
                                    itemId = d.itemId,
                                    unitName = d.Unit,
                                    itemUnitId = d.itemUnitId,
                                    quantity = d.Count,
                                    invoiceId = d.OrderId,
                                });
                               // ordersIds.Add(d.OrderId);
                            }
                        }
                        w.orderList = orderList;
                        if (w.ShowDialog() == true)
                        {
                            if (w.selectedItemsLocations != null)
                            {
                                List<ItemLocation> itemsLocations = w.selectedItemsLocations;
                                List<ItemLocation> readyItemsLoc = new List<ItemLocation>();

                                for (int i = 0; i < itemsLocations.Count; i++)
                                {
                                    if (itemsLocations[i].isSelected == true)
                                        readyItemsLoc.Add(itemsLocations[i]);
                                }                             
                                #region invoice 
                                //invoice.invType = "sr";
                                //invoice.updateUserId = MainWindow.userLogin.userId;
                                #endregion
                                //int res = await invoice.saveInvoice(invoice);
                                int res = await addInvoice("sr");
                                if(res >0)
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                    #region notification Object
                                    Notification not = new Notification()
                                    {
                                        title = "trSpendingOrderAlertTilte",
                                        ncontent = "trSpendingOrderApproveAlertContent",
                                        msgType = "alert",
                                        createDate = DateTime.Now,
                                        updateDate = DateTime.Now,
                                        createUserId = MainWindow.userLogin.userId,
                                        updateUserId = MainWindow.userLogin.userId,
                                    };
                                    await not.save(not, MainWindow.branchLogin.branchId, "storageAlerts_spendingOrderApprove", MainWindow.userLogin.fullName);
                                    #endregion

                                    await FillCombo.itemLocation.transferToKitchen(readyItemsLoc, orderList, MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                                    setNotifications();
                                    clear();
                                }
                                else
                                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                            }
                        }
                        Window.GetWindow(this).Opacity = 1;

                        
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
        private async Task<int> addInvoice(string invType)
        {
            if (invType == "sr" && (invoice.invoiceId == 0 || invoice.invType == "srd"))
            {
                invoice.invNumber = await invoice.generateInvNumber("sr", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
            }
            else if (invType == "srd" && invoice.invoiceId == 0)
                invoice.invNumber = await invoice.generateInvNumber("srd", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
           
            invoice.invType = invType;

            invoice.paid = 0;
            invoice.deserved = invoice.totalNet;
            invoice.branchId = MainWindow.branchLogin.branchId;
            invoice.branchCreatorId = MainWindow.branchLogin.branchId;
            invoice.posId = MainWindow.posLogin.posId;
            invoice.createUserId = MainWindow.userLogin.userId;
            invoice.updateUserId = MainWindow.userLogin.userId;
            //// invoice items
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
            // save invoice in DB
            int invoiceId = await FillCombo.invoice.saveInvoiceWithItems(invoice, invoiceItems);
            invoice.invoiceId = invoiceId;
            return invoiceId;

        }
        private async void Btn_deleteInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (invoice.invoiceId != 0)
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
                        invoice.invType = "src";
                        invoice.updateUserId = MainWindow.userLogin.userId;
                        int res = await FillCombo.invoice.saveInvoice(invoice);
                        if (res > 0)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                            clear();
                            setNotifications();
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
        //invoiceItems
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        Branch branchModel = new Branch();
        public async Task BuildReport()
        {

            if (invoice.invoiceId > 0)
            {
                List<ReportParameter> paramarr = new List<ReportParameter>();
                Invoice prInvoice = invoice;
                Invoice invoiceModel = invoice;
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
                    foreach (var item in paramarr.Where(x => x.Name == "Title").ToList())
                    {
                        StringCollection myCol = new StringCollection();
                        myCol.Add(AppSettings.resourcemanagerreport.GetString("trSpendingOrder"));
                        item.Values = myCol;


                    }
                 //   paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trSpendingOrder")));
                    rep.SetParameters(paramarr);
                    rep.Refresh();
                }
                else
                {


                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSaveInvoiceToPreview"), animation: ToasterAnimation.FadeIn);



                }
            }
        }
        //private void BuildReport()
        //{
        //    List<ReportParameter> paramarr = new List<ReportParameter>();

        //    string addpath;
        //    bool isArabic = ReportCls.checkLang();
        //    if (isArabic)
        //    {
        //        addpath = @"\Reports\Storage\movementsOperations\Ar\ArSpendingOrder.rdlc";
        //    }
        //    else
        //    {
        //        addpath = @"\Reports\Storage\movementsOperations\En\EnSpendingOrder.rdlc";
        //    }
        //    string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

        //    //D:\myproj\posproject5\laundryApp\laundryApp\View\storage\movementsOperations\uc_SpendingOrder.xaml.cs

        //    clsReports.SpendingOrder(invoiceItems, rep, reppath, paramarr);
        //    clsReports.setReportLanguage(paramarr);
        //    clsReports.Header(paramarr);

        //    rep.SetParameters(paramarr);

        //    rep.Refresh();
        //}
        private async void PrintRep()
        {
           await  BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
            });
        }
        private async  void PdfRep()
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

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    if (invoiceItems != null)
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
        private void Btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {

                    if (invoiceItems != null)
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
    
       

        private async  void Btn_preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    #region
                    if (invoiceItems != null)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        /////////////////////
                        string pdfpath = "";
                        pdfpath = @"\Thumb\report\temp.pdf";
                        pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                     await   BuildReport();
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
        #region btn
        private async void Btn_orders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                await saveDraft();
                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();

                w.invoiceType = "srw";
                w.title = AppSettings.resourcemanager.GetString("trOrders");
                w.branchId = MainWindow.branchLogin.branchId;
                w.icon = "waitingOrders";
                w.page = "spendingOrder";

                if (w.ShowDialog() == true)
                {
                    if (w.invoice != null)
                    {
                        invoice = w.invoice;
                        _InvoiceType = invoice.invType;
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
        private async void Btn_items_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                Window.GetWindow(this).Opacity = 0.2;
                wd_purchaseItems w = new wd_purchaseItems();
                w.ShowDialog();
                if (w.isActive)
                {
                    // ChangeItemIdEvent(w.selectedItem);
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
        private async void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                await saveDraft();
                    
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task saveDraft()
        {
            if (billDetails.Count != 0 && _InvoiceType=="srd")
            {
                #region Accept
                MainWindow.mainWindow.Opacity = 0.2;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                w.contentText = AppSettings.resourcemanager.GetString("trSaveInvoiceNotification");
                // w.ShowInTaskbar = false;
                w.ShowDialog();
                MainWindow.mainWindow.Opacity = 1;
                #endregion
                if (w.isOk)
                {
                    int res = await addInvoice("srd");
                    if (res > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                        clear();
                        //refreshDraftNotification();
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }
                clear();
                setNotifications();
            }
        }
        private async void Btn_draft_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                HelpClass.StartAwait(grid_main);

                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();
                string invoiceType = "srd";
                int duration = 2;
                w.invoiceType = invoiceType;
                w.userId = MainWindow.userLogin.userId;
                w.duration = duration; // view drafts which updated during 2 last days 
                w.title = AppSettings.resourcemanager.GetString("trDrafts");
                w.icon = "draftOrders";
                w.page = "spendingOrder";

                if (w.ShowDialog() == true)
                {
                    if (w.invoice != null)
                    {
                        invoice = w.invoice;
                        _InvoiceType = invoice.invType;
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
                TextBlock tb = new TextBlock();
                TextBox t = e.EditingElement as TextBox;  // Assumes columns are all TextBoxes

                var columnName = e.Column.Header.ToString();

                BillDetailsPurchase row = e.Row.Item as BillDetailsPurchase;
                int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == row.itemUnitId).FirstOrDefault());

                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds < 100)
                {
                    if (columnName == AppSettings.resourcemanager.GetString("trQuantity"))
                        t.Text = billDetails[index].Count.ToString();
                }
                else
                {
                    int oldCount = 0;
                    long newCount = 0;
                    //"tb_amont"
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

                    oldCount = row.Count;

                    //validateAvailableAmount(row, newCount, index, tb );
                    int availableAmount = await getAvailableAmount(row.itemId, row.itemUnitId, MainWindow.branchLogin.branchId, row.ID);
                    if (availableAmount < newCount )
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableToolTip"), animation: ToasterAnimation.FadeIn); 
                        newCount = availableAmount;
                        tb = dg_billDetails.Columns[4].GetCellContent(dg_billDetails.Items[index]) as TextBlock;
                        tb.Text = newCount.ToString();
                        row.Count = (int)newCount;
                    }                     

                    // update item in billdetails           
                    billDetails[index].Count = (int)newCount;
                }
            }
            catch (Exception ex)
            {
                //
                //    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
         
        }
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
        private void Cbm_unitItemDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var cmb = sender as ComboBox;

                if (dg_billDetails.SelectedIndex != -1 && cmb != null)
                    billDetails[dg_billDetails.SelectedIndex].itemUnitId = (int)cmb.SelectedValue;
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
                if (billDetails.Count == 1)
                {
                    var cmb = sender as ComboBox;
                    cmb.SelectedValue = (int)billDetails[0].itemUnitId;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
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
                    // create new row in bill details data grid
                    addRowToBill(item.name, itemId, defaultPurUnit.mainUnit, defaultPurUnit.itemUnitId, 1);

                    //_Count++;
                    //refrishBillDetails();
                }
                else
                {
                    addRowToBill(item.name, itemId, null, 0, 1);
                }
                _Count++;
            }
        }
        private void clear()
        {
            _Count = 0;
            _SequenceNum = 0;
            _InvoiceType = "srd";
            isFromReport = false;
            archived = false;
            invoice = new Invoice();
            generatedInvoice = new Invoice();
            tb_barcode.Clear();
            billDetails.Clear();
            txt_invNumber.Text = "";
            refrishBillDetails();

            btn_deleteInvoice.Visibility = Visibility.Collapsed;
            btn_next.Visibility = Visibility.Collapsed;
            btn_previous.Visibility = Visibility.Collapsed;

            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        public async Task fillInvoiceInputs(Invoice invoice)
        {
            _Count = invoice.itemsCount;
            tb_count.Text = _Count.ToString();
            txt_invNumber.Text = invoice.invNumber.ToString();
            // build invoice details grid
            await buildInvoiceDetails();

            inputEditable();
        }
        private async Task buildInvoiceDetails()
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
            if (_InvoiceType == "srw") // spending order waiting
            {
                btn_items.IsEnabled = true;
                btn_deleteInvoice.Visibility = Visibility.Visible;
            }
            else
            {
                btn_items.IsEnabled = false;
                btn_deleteInvoice.Visibility = Visibility.Collapsed;

            }

            btn_deleteInvoice.Visibility = Visibility.Visible;
            if (!isFromReport)
            {
                btn_next.Visibility = Visibility.Visible;
                btn_previous.Visibility = Visibility.Visible;
            }
        }
        #endregion
        #region available amount - 
        private async Task<int> getAvailableAmount(int itemId, int itemUnitId, int branchId, int ID)
        {
            var itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == itemId).ToList();
            int availableAmount = await FillCombo.itemLocation.getAmountInBranch(itemUnitId, branchId);
            var smallUnits = await FillCombo.itemUnit.getSmallItemUnits(itemId, itemUnitId);
            foreach (ItemUnit u in itemUnits)
            {
                var isInBill = billDetails.ToList().Find(x => x.itemUnitId == (int)u.itemUnitId && x.ID != ID); // unit exist in invoice
                if (isInBill != null)
                {
                    var isSmall = smallUnits.Find(x => x.itemUnitId == (int)u.itemUnitId);
                    int unitValue = 0;

                    int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == u.itemUnitId).FirstOrDefault());
                    int quantity = billDetails[index].Count;
                    if (itemUnitId == u.itemUnitId)
                    { }
                    else if (isSmall != null) // from-unit is bigger than to-unit
                    {
                        unitValue = await FillCombo.itemUnit.largeToSmallUnitQuan(itemUnitId, (int)u.itemUnitId);
                        quantity = quantity / unitValue;
                    }
                    else
                    {
                        unitValue = await FillCombo.itemUnit.smallToLargeUnit(itemUnitId, (int)u.itemUnitId);

                        if (unitValue != 0)
                        {
                            quantity = quantity * unitValue;
                        }
                    }
                    availableAmount -= quantity;
                }
            }
            return availableAmount;
        }

        #endregion

       
    }
}
