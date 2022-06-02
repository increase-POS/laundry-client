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
    /// Interaction logic for uc_spendingRequest.xaml
    /// </summary>
    public partial class uc_spendingRequest : UserControl
    {
        string createPermission = "spendingRequest_create";
        string reportsPermission = "spendingRequest_report";
        string returnPermission = "spendingRequest_return";
        private static uc_spendingRequest _instance;
        public static uc_spendingRequest Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_spendingRequest();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_spendingRequest()
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
        public Invoice invoice = new Invoice();
        List<ItemTransfer> mainInvoiceItems;
        List<ItemTransfer> invoiceItems;
        
        static public string _InvoiceType = "srd"; //draft spending request

        static private int _SequenceNum = 0;
        static private int _Count = 0;
        static public int _invoiceId;
        #region barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        bool _IsFocused = false;
        public List<Control> controls;
        #endregion
        #region notification parameter
        private static DispatcherTimer timer;
        int _DraftCount = 0;
        int _InvCount = 0;
        #endregion
        Item item = new Item();
        IEnumerable<Item> items;
        public static List<string> requiredControlList;
        private void translate()
        {
            ////////////////////////////////----Order----/////////////////////////////////
            dg_billDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_billDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_billDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_billDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trQuantity");

            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trSpendingRequest");

            tt_error_previous.Content = AppSettings.resourcemanager.GetString("trPrevious");
            tt_error_next.Content = AppSettings.resourcemanager.GetString("trNext");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcode"));

            txt_count.Text = AppSettings.resourcemanager.GetString("trCount");
            txt_orders.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_drafts.Text = AppSettings.resourcemanager.GetString("trDrafts");
            txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            txt_returnInvoice.Text = AppSettings.resourcemanager.GetString("trReturn");
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                MainWindow.mainWindow.KeyDown -= HandleKeyPress;
                timer.Stop();
                if (billDetails.Count > 0 && _InvoiceType == "srd" )
                {
                    #region Accept
                    MainWindow.mainWindow.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trSaveOrderNotification");
                    // w.ShowInTaskbar = false;
                    w.ShowDialog(); 
                    MainWindow.mainWindow.Opacity = 1;
                    #endregion
                    if (w.isOk)
                        Btn_newDraft_Click(null, null);
                    else
                        clear();
                }
                else
                    clear();
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
                requiredControlList = new List<string> {  };

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
        private void setNotifications()
        {
            try
            {
                refreshDraftNotification();
                refreshOrdersNotification();
            }
            catch (Exception ex)
            {
                //HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void refreshDraftNotification()
        {
            try
            {
                string invoiceType = "srd , srbd";
                int duration = 2;
                int draftCount = await invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, duration);
                if (invoice.invType == "srd" || invoice.invType == "srbd" )
                    draftCount--;
                HelpClass.refreshNotification(md_draftsCount, ref _DraftCount, draftCount);              
            }
            catch { }
        }
        private async void refreshOrdersNotification()
        {
            try
            {
                string invoiceType = "sr ,srw ,src ";
                int duration = 1;
                int invCount = await invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, duration);
                if (invoice != null && (invoice.invType == "sr" || invoice.invType == "srw" || invoice.invType == "src") && !isFromReport)
                    invCount--;

                HelpClass.refreshNotification(md_invoices, ref _InvCount, invCount);
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
                _InvoiceType = invoice.invType;
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
                _InvoiceType = invoice.invType;
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
                    if (_InvoiceType.Equals("sr") || _InvoiceType.Equals("srw") || _InvoiceType.Equals("src"))
                    {
                        await fillInvoiceInputs(invoice);
                    }
                    break;

                default: // if barcode for item
                         // get item matches barcode
                    if (FillCombo.itemUnitList != null && _InvoiceType == "srd")
                    {
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
        #region save
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one"))
                {                  
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        bool valid = validateItemUnits();
                        if (valid)
                        {
                            #region spending request
                            if ( _InvoiceType == "srd")
                            {
                                await addInvoice("srw");
                                clear();
                                setNotifications();
                                #region notification Object
                                Notification not = new Notification()
                                {
                                    title = "trSpendingOrderAlertTilte",
                                    ncontent = "trSpendingOrderAlertContent",
                                    msgType = "alert",
                                    createDate = DateTime.Now,
                                    updateDate = DateTime.Now,
                                    createUserId = MainWindow.userLogin.userId,
                                    updateUserId = MainWindow.userLogin.userId,
                                };

                                await not.save(not, MainWindow.branchLogin.branchId, "kitchenAlerts_spendingOrderRequest", MainWindow.userLogin.fullName);
                                #endregion
                            }
                            #endregion
                            #region bounce spending request
                            else if (_InvoiceType == "srbd")
                            {
                                await addInvoice("srb");
                                
                                // decrease amounts from kitchen
                                await FillCombo.itemLocation.returnSpendingOrder(invoiceItems,MainWindow.branchLogin.branchId,MainWindow.userLogin.userId);
                                clear();
                                setNotifications();
                            }
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
        async Task saveDraft()
        {
            bool valid = validateItemUnits();
            await addInvoice(_InvoiceType);
            clear();
        }
        private async Task addInvoice(string invType)
        {
            if (invoice.invType == "sr"  && (invType == "srb" || invType == "srbd")) // spending request will be bounce   or  bounce draft , save another invoice in db
            {
                invoice.invoiceMainId = invoice.invoiceId;
                invoice.invoiceId = 0;
                if(invType == "srb")
                    invoice.invNumber = await invoice.generateInvNumber("srb", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                else
                    invoice.invNumber = await invoice.generateInvNumber("srbd", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);

                invoice.branchCreatorId = MainWindow.branchLogin.branchId;
                invoice.posId = MainWindow.posLogin.posId;
            }
            if (invType == "srw")
            {
                invoice.invNumber = await invoice.generateInvNumber("sr", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
            }
            else if (invType == "srd" && invoice.invoiceId == 0)
                invoice.invNumber = await invoice.generateInvNumber("srd", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
            else if(invoice.invType == "srbd" && invType == "srb")
                invoice.invNumber = await invoice.generateInvNumber("srb", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);

            invoice.invType = invType;

            invoice.paid = 0;
            invoice.deserved = invoice.totalNet;
            invoice.branchId = MainWindow.branchLogin.branchId;
            invoice.branchCreatorId = MainWindow.branchLogin.branchId;
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
                itemT.invoiceId = billDetails[i].OrderId;

                invoiceItems.Add(itemT);
            }
            // save invoice in DB
            int invoiceId = await FillCombo.invoice.saveInvoiceWithItems(invoice,invoiceItems);
            invoice.invoiceId = invoiceId;
            if (invoiceId != 0)
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
            }
            else
                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);


        }
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

        Invoice   invoiceModel=new Invoice();


        public async Task BuildReport()
        {
          if(invoice.invoiceId>0){
               
            prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);
            paramarr = new List<ReportParameter>();

            string reppath = reportclass.SpendingRequestRdlcpath( );
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

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
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
         await   BuildReport();

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

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
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

        await    BuildReport();

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
        private async void Btn_returnInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(returnPermission, FillCombo.groupObjects, "one"))
                {
                    if (_InvoiceType == "sr")
                    {
                        _InvoiceType = "srbd"; // spending request bounce draft
                        isFromReport = true;
                        archived = false;
                        await fillInvoiceInputs(invoice);
                        mainInvoiceItems = invoiceItems;
                        txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trReturnedInvoice");
                        btn_save.Content = AppSettings.resourcemanager.GetString("trReturn");
                        setNotifications();
                    }
                    else
                    {
                        //await saveBeforeExit();
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_returnInvoice w = new wd_returnInvoice();
                        w.page = "spendingOrder";
                        w.userId = MainWindow.userLogin.userId;
                        w.invoiceType = "sr";
                        if (w.ShowDialog() == true)
                        {
                            _InvoiceType = "srbd";
                            invoice = w.invoice;
                            isFromReport = true;
                            archived = false;
                            await fillInvoiceInputs(invoice);
                            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trReturnedInvoice");
                            btn_save.Content = AppSettings.resourcemanager.GetString("trReturn");
                            setNotifications();
                        }
                        Window.GetWindow(this).Opacity = 1;
                    }
                    mainInvoiceItems = invoiceItems;
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
        private async void Btn_orders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();

                w.title = AppSettings.resourcemanager.GetString("trOrders");
                w.userId = MainWindow.userLogin.userId;
                w.invoiceType = "sr ,srw,src";
                w.duration = 2;
                w.icon = "spendingRequest";
                w.page = "spendingRequest";
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
        private async void Btn_ordersWait_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(exportPermission, FillCombo.groupObjects, "one") )
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_invoice w = new wd_invoice();

                    w.invoiceType = "exw";
                    w.title = AppSettings.resourcemanager.GetString("trOrders");
                    w.branchId = MainWindow.branchLogin.branchId;

                    if (w.ShowDialog() == true)
                    {
                        if (w.invoice != null)
                        {
                            invoice = w.invoice;
                            _ProcessType = invoice.invType;
                            _invoiceId = invoice.invoiceId;
                            setNotifications();
                            await fillOrderInputs(invoice);
                            invoices = await invoice.getBranchInvoices(w.invoiceType, 0, MainWindow.branchLogin.branchId);
                            navigateBtnActivate();
                        }
                    }
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
            */
        }
        private async void Btn_items_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //items
                Window.GetWindow(this).Opacity = 0.2;
                wd_purchaseItems w = new wd_purchaseItems();

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
        private async void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (billDetails.Count > 0 && ( _InvoiceType == "srd" || _InvoiceType == "srbd"))
                {
                    #region Accept
                    MainWindow.mainWindow.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trSaveOrderNotification");
                    w.ShowDialog();
                    MainWindow.mainWindow.Opacity = 1;
                    #endregion
                    if (w.isOk)
                    {
                        await saveDraft();
                        clear();
                    }
                    else
                        clear();
                }
                else
                    clear();

                setNotifications();

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

                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();
                string invoiceType = "srd , srbd";
                int duration = 2;
                w.invoiceType = invoiceType;
                w.userId = MainWindow.userLogin.userId;
                w.duration = duration; // view drafts which updated during 2 last days 
                w.title = AppSettings.resourcemanager.GetString("trDrafts");
                w.icon = "drafts";
                w.page = "spendingRequest";

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
        private void Dg_billDetails_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                TextBox t = e.EditingElement as TextBox;  // Assumes columns are all TextBoxes
                var columnName = e.Column.Header.ToString();

                BillDetailsPurchase row = e.Row.Item as BillDetailsPurchase;
                int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == row.itemUnitId && p.OrderId == row.OrderId).FirstOrDefault());

                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds < 100)
                {
                    if (columnName == AppSettings.resourcemanager.GetString("trQuantity"))
                        t.Text = billDetails[index].Count.ToString();
                }
                else
                {
                    int oldCount  = row.Count;
                    int newCount = 0;
                    if (!t.Text.Equals(""))
                        newCount = int.Parse(t.Text);
                    else
                        newCount = 0;
                   
                    //"tb_amont"
                    if (columnName == AppSettings.resourcemanager.GetString("trQuantity"))
                    {
                        if (_InvoiceType == "srbd")
                        {
                            ItemTransfer item = mainInvoiceItems.ToList().Find(i => i.itemUnitId == row.itemUnitId);
                            if (newCount > item.quantity)
                            {
                                // return old value 
                                t.Text = item.quantity.ToString();

                                newCount = (int)item.quantity;
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                            }
                        }
                        else
                        {
                            newCount = row.Count;
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
                }

            }
            catch (Exception ex)
            {
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


                                if (_InvoiceType == "sr" )
                                    combo.IsEnabled = false;
                                else
                                    combo.IsEnabled = true;
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
        private void Dg_billDetails_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            int column = dg_billDetails.CurrentCell.Column.DisplayIndex;
            if (_InvoiceType == "sr" && column == 3)
                e.Cancel = true;
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

                    if (_InvoiceType == "sr")
                        cmb.IsEnabled = false;
                    else
                        cmb.IsEnabled = true;
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
        //void refrishDataGridItems()
        //{
        //    dg_billDetails.ItemsSource = null;
        //    dg_billDetails.ItemsSource = billDetails;
        //    dg_billDetails.Items.Refresh();
        //    DataGrid_CollectionChanged(dg_billDetails, null);

        //}
        public async Task ChangeItemIdEvent(int itemId)
        {
             item = FillCombo.purchaseItems.ToList().Find(c => c.itemId == itemId);

            if (item != null)
            {
                this.DataContext = item;

                // get item units
                if (FillCombo.itemUnitList is null)
                   await FillCombo.RefreshItemUnit();
                itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == item.itemId).ToList();
                // search for default unit for purchase
                var defaultPurUnit = itemUnits.ToList().Find(c => c.defaultPurchase == 1);
                if (defaultPurUnit != null)
                {
                    int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == defaultPurUnit.itemUnitId && p.OrderId == 0).FirstOrDefault());
                    if (index == -1)//item doesn't exist in bill
                    {
                        // create new row in bill details data grid
                        addRowToBill(item.name, itemId, defaultPurUnit.unitName, defaultPurUnit.itemUnitId, 1);
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
            tb_barcode.Clear();
            billDetails.Clear();
            txt_invNumber.Text = "";
            refrishBillDetails();
            inputEditable();
            btn_next.Visibility = Visibility.Collapsed;
            btn_previous.Visibility = Visibility.Collapsed;
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trSpendingRequest");


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
          
            if (_InvoiceType == "srd" ) 
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete hidden
                dg_billDetails.Columns[3].IsReadOnly = false; // unit 
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                tb_barcode.IsEnabled = true;
                btn_save.IsEnabled = true;
                btn_items.IsEnabled = true;
            }
            if (_InvoiceType == "srbd") 
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete hidden
                dg_billDetails.Columns[3].IsReadOnly = true; // unit 
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                tb_barcode.IsEnabled = true;
                btn_save.IsEnabled = true;
                btn_items.IsEnabled = false;
            }
            else if (_InvoiceType == "srw" || _InvoiceType == "sr" || _InvoiceType == "src")
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Collapsed; //make delete hidden
                dg_billDetails.Columns[3].IsReadOnly = true; // unit 
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
