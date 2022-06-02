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
    /// Interaction logic for uc_storageMovements.xaml
    /// </summary>
    public partial class uc_storageMovements : UserControl
    {
        string importPermission = "storageMovements_import";
        string exportPermission = "storageMovements_export";
        string reportsPermission = "storageMovements_reports";
        string packagePermission = "storageMovements_package";
        string unitConversionPermission = "storageMovements_unitConversion";
        string initializeShortagePermission = "storageMovements_initializeShortage";
        private static uc_storageMovements _instance;
        public static uc_storageMovements Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_storageMovements();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_storageMovements()
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
        static public string _ProcessType = "imd"; //draft import

        static private int _SequenceNum = 0;
        static private int _Count = 0;
        static private int _invoiceId;
        #region for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        static private string _SelectedProcess = "";
        static private int _SelectedBranch = -1;
        bool _IsFocused = false;
        public static List<string> requiredControlList;
        #endregion
        #region for notifications
        private static DispatcherTimer timer;
        public static bool archived = false;
        public static bool isFromReport = false;
        #endregion
        Item item = new Item();

        private void translate()
        {
           

            ////////////////////////////////----Order----/////////////////////////////////
            dg_billDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_billDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_billDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_billDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trQuantity");

            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementImport");

            //txt_shortageInvoice.Text = AppSettings.resourcemanager.GetString("trLack");





            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            txt_drafts.Text = AppSettings.resourcemanager.GetString("trDrafts");
            txt_orders.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_ordersWait.Text = AppSettings.resourcemanager.GetString("trOrdersWait");
            txt_unitConversion.Text = AppSettings.resourcemanager.GetString("trUnitConversion");
            txt_processType.Text = AppSettings.resourcemanager.GetString("trProcessType");
            txt_store.Text = AppSettings.resourcemanager.GetString("trStore/Branch");
            txt_count.Text = AppSettings.resourcemanager.GetString("trCount");
            txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_pdf.Text = AppSettings.resourcemanager.GetString("trPdf");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");


            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_processType, AppSettings.resourcemanager.GetString("trProcessTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branch, AppSettings.resourcemanager.GetString("trStore/BranchHint"));

            tt_error_previous.Content = AppSettings.resourcemanager.GetString("trPrevious");
            tt_error_next.Content = AppSettings.resourcemanager.GetString("trNext");



        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                MainWindow.mainWindow.KeyDown -= HandleKeyPress;
                timer.Stop();
                if (billDetails.Count > 0 && (_ProcessType == "imd" || _ProcessType == "exd"))
                {
                    #region Accept
                    MainWindow.mainWindow.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = "Do you want save sale invoice in drafts?";
                    w.ShowDialog();
                    MainWindow.mainWindow.Opacity = 1;
                    #endregion
                    if (w.isOk)
                        Btn_newDraft_Click(null, null);
                    else
                        clearProcess();
                }
                else
                    clearProcess();
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
                requiredControlList = new List<string> { "branch" };

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
                FillCombo.FillMovementsProcessType(cb_processType);
                //await FillCombo.fillBranchesWithoutCurrent(cb_branch);
                // await RefrishItems();
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_RefrishItems", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillBranchesWithoutCurrent", value = false });

                loading_RefrishItems();
                loading_fillBranchesWithoutCurrent();
                do
                {
                    isDone = true;
                    foreach (var item in loadingList)
                    {
                        if (item.value == false)
                        {
                            isDone = false;
                            break;
                        }
                    }
                    if (!isDone)
                    {
                        await Task.Delay(0500);
                    }
                }
                while (!isDone);
                #endregion
                //List all the UIElement in the VisualTree
                controls = new List<Control>();
                FindControl(this.grid_main, controls);
                #region Permision


                if (FillCombo.groupObject.HasPermissionAction(exportPermission, FillCombo.groupObjects, "one"))
                    md_orderWaitCount.Visibility = Visibility.Visible;
                else
                    md_orderWaitCount.Visibility = Visibility.Collapsed;
                
                /*
                if (FillCombo.groupObject.HasPermissionAction(initializeShortagePermission, FillCombo.groupObjects, "one"))
                {
                    btn_shortageInvoice.Visibility = Visibility.Visible;
                    bdr_shortageInvoice.Visibility = Visibility.Visible;
                    md_shortage.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_shortageInvoice.Visibility = Visibility.Collapsed;
                    bdr_shortageInvoice.Visibility = Visibility.Collapsed;
                    md_shortage.Visibility = Visibility.Collapsed;
                }
                */

                //if (FillCombo.groupObject.HasPermissionAction(packagePermission, FillCombo.groupObjects, "one"))
                //    btn_package.Visibility = Visibility.Visible;
                //else
                //    btn_package.Visibility = Visibility.Collapsed;

                if (FillCombo.groupObject.HasPermissionAction(unitConversionPermission, FillCombo.groupObjects, "one"))
                    btn_unitConversion.Visibility = Visibility.Visible;
                else
                    btn_unitConversion.Visibility = Visibility.Collapsed;

                //if (!FillCombo.groupObject.HasPermissionAction(packagePermission, FillCombo.groupObjects, "one")
                //    && !FillCombo.groupObject.HasPermissionAction(unitConversionPermission, FillCombo.groupObjects, "one"))
                //    bdr_package_converter.Visibility = Visibility.Collapsed;


                #endregion


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
        List<keyValueBool> loadingList;
        async void loading_RefrishItems()
        {
            try
            {
                await FillCombo.RefreshPurchaseItems();
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefrishItems"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_fillBranchesWithoutCurrent()
        {
            try
            {
                await FillCombo.fillBranchesNoCurrentDefault(cb_branch, "s");
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillBranchesWithoutCurrent"))
                {
                    item.value = true;
                    break;
                }
            }
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
                refreshDraftNotification();
                refreshOrderWaitNotification();
                //refreshLackNotification();
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
                string invoiceType = "imd ,exd";
                int duration = 2;
                int draftCount = await invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, duration);
                if ((invoice.invType == "imd" || invoice.invType == "exd"))
                    draftCount--;

                int previouseCount = 0;
                if (md_draftsCount.Badge != null && md_draftsCount.Badge.ToString() != "") previouseCount = int.Parse(md_draftsCount.Badge.ToString());

                if (draftCount != previouseCount)
                {
                    if (draftCount > 9)
                    {
                        draftCount = 9;
                        md_draftsCount.Badge = "+" + draftCount.ToString();
                    }
                    else if (draftCount == 0) md_draftsCount.Badge = "";
                    else
                        md_draftsCount.Badge = draftCount.ToString();
                }
            }
            catch { }
        }
        private async void refreshOrderWaitNotification()
        {
            try
            {
                string invoiceType = "exw";

                int waitedOrdersCount = await invoice.GetCountBranchInvoices(invoiceType, 0, MainWindow.branchLogin.branchId);
                if (invoice.invType == "exw")
                    waitedOrdersCount--;

                int previouseCount = 0;
                if (md_orderWaitCount.Badge != null && md_orderWaitCount.Badge.ToString() != "") previouseCount = int.Parse(md_orderWaitCount.Badge.ToString());

                if (waitedOrdersCount != previouseCount)
                {
                    if (waitedOrdersCount > 9)
                    {
                        waitedOrdersCount = 9;
                        md_orderWaitCount.Badge = "+" + waitedOrdersCount.ToString();
                    }
                    else if (waitedOrdersCount == 0) md_orderWaitCount.Badge = "";
                    else
                        md_orderWaitCount.Badge = waitedOrdersCount.ToString();
                }
            }
            catch (Exception ex)
            {
                //HelpClass.ExceptionMessage(ex, this);
            }
        }
        /*
        private async Task refreshLackNotification()
        {
            try
            {
                string isThereLack = await invoice.isThereLack(MainWindow.branchLogin.branchId);
                if (isThereLack == "yes")
                    md_shortage.Badge = "!";
                else
                    md_shortage.Badge = "";
            }
            catch { }
        }
        */
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
        private async Task navigateInvoice(int index)
        {
            try
            {
                clearProcess();
                invoice = FillCombo.invoices[index];
                _ProcessType = invoice.invType;
                _invoiceId = invoice.invoiceId;
                navigateBtnActivate();
                await fillOrderInputs(invoice);

                #region set title according to invoice type
                if (_ProcessType == "im" || _ProcessType == "imw" || _ProcessType == "imd")
                {
                    txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementImport");
                }
                else if (_ProcessType == "ex" || _ProcessType == "exw" || _ProcessType == "exd")
                {
                    txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementExport");
                }
                #endregion
            }
            catch (Exception ex)
            {
            }
        }
        private async void Btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                int index = FillCombo.invoices.IndexOf(FillCombo.invoices.Where(x => x.invoiceId == _invoiceId).FirstOrDefault());
                index++;

                await navigateInvoice(index);

                //clearProcess();
                //invoice = FillCombo.invoices[index];
                //_ProcessType = invoice.invType;
                //_invoiceId = invoice.invoiceId;
                //navigateBtnActivate();
                //await fillOrderInputs(invoice);
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
                await navigateInvoice(index);

                //clearProcess();
                //invoice = FillCombo.invoices[index];
                //_ProcessType = invoice.invType;
                //_invoiceId = invoice.invoiceId;
                //navigateBtnActivate();
                //await fillOrderInputs(invoice);
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
                            itemUnits = FillCombo.itemUnitList.Where(c => c.itemId == itemId).ToList();
                            //get item from list
                            item = FillCombo.purchaseItems.Find(i => i.itemId == itemId);

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
        private async Task save()
        {
            int invoiceId;
            invoiceItems = new List<ItemTransfer>();
            ItemTransfer itemT;
            for (int i = 0; i < billDetails.Count; i++)
            {
                itemT = new ItemTransfer();

                itemT.quantity = billDetails[i].Count;
                itemT.price = billDetails[i].Price;
                itemT.itemUnitId = billDetails[i].itemUnitId;
                itemT.createUserId = MainWindow.userLogin.userId;
                itemT.invoiceId = billDetails[i].OrderId;
                invoiceItems.Add(itemT);
            }
            switch (_ProcessType)
            {
                case "imd":// add or edit import order then add export order
                           // import order
                    invoice.invType = "im";
                    invoice.branchId = MainWindow.branchLogin.branchId;
                    invoice.posId = MainWindow.posLogin.posId;
                    invoice.createUserId = MainWindow.userLogin.userId;
                    invoice.updateUserId = MainWindow.userLogin.userId;
                    if (invoice.invNumber == null)
                        invoice.invNumber = await invoice.generateInvNumber("im", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                    // save invoice in DB
                    invoiceId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);
                    if (invoiceId != 0)
                    {
                        #region notification Object
                        Notification not = new Notification()
                        {
                            title = "trExportAlertTilte",
                            ncontent = "trExportAlertContent",
                            msgType = "alert",
                            createUserId = MainWindow.userLogin.userId,
                            updateUserId = MainWindow.userLogin.userId,
                        };
                        await not.save(not, (int)cb_branch.SelectedValue, "storageAlerts_ImpExp", MainWindow.branchLogin.name);
                        #endregion
                        // expot order
                        if (invoice.invoiceId == 0) // create new export order
                        {
                            invoice = new Invoice();
                            invoice.invType = "exw";
                            invoice.invoiceMainId = invoiceId;
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            invoice.invNumber = await invoice.generateInvNumber("ex", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                            invoice.createUserId = MainWindow.userLogin.userId;
                        }
                        else // edit exit export order
                        {
                            invoice = await invoice.getgeneratedInvoice(invoiceId);
                            invoice.invType = "exw";
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            invoice.updateUserId = MainWindow.userLogin.userId;
                        }
                        int exportId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);

                        // add order details                      
                        //await invoice.saveInvoiceItems(invoiceItems, invoiceId);
                        //await invoice.saveInvoiceItems(invoiceItems, exportId);

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    break;
                case "exd":// add or edit export order then add import order
                           // import order
                    invoice.invType = "ex";
                    invoice.branchId = MainWindow.branchLogin.branchId;
                    invoice.posId = MainWindow.posLogin.posId;
                    invoice.createUserId = MainWindow.userLogin.userId;
                    invoice.updateUserId = MainWindow.userLogin.userId;
                    if (invoice.invNumber == null)
                        invoice.invNumber = await invoice.generateInvNumber("ex", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                    // save invoice in DB
                    invoiceId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);

                    if (invoiceId != 0)
                    {
                        // import order
                        if (invoice.invoiceId == 0) // create new export order
                        {
                            invoice = new Invoice();
                            invoice.invType = "im";
                            invoice.invoiceMainId = invoiceId;
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            invoice.invNumber = await invoice.generateInvNumber("im", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                            invoice.createUserId = MainWindow.userLogin.userId;
                        }
                        else // edit exit export order
                        {
                            invoice = await invoice.getgeneratedInvoice(invoiceId);
                            invoice.invType = "im";
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            invoice.updateUserId = MainWindow.userLogin.userId;
                        }
                        int importId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);

                        // add order details
                        // await invoice.saveInvoiceItems(invoiceItems, invoiceId);
                        //await invoice.saveInvoiceItems(invoiceItems, importId);

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                    break;
                case "exw":
                    invoice.invType = "ex";
                    invoice.updateUserId = MainWindow.userLogin.userId;
                    // save invoice in DB
                    invoiceId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);
                    if (invoiceId != 0)
                    {
                        // await invoice.saveInvoiceItems(invoiceItems, invoiceId);
                        await invoice.saveInvoiceItems(invoiceItems, invoice.invoiceMainId.Value);
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                    break;
            }
            clearProcess();
        }
        private async Task<bool> validateOrder()
        {
            bool valid = true;
            if (billDetails.Count == 0)
                valid = false;

            valid = await checkItemsAmounts();
            if (billDetails.Count == 0)
                clearProcess();
            else
                HelpClass.validate(requiredControlList, this);
            return valid;
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (
                    (
                        (_ProcessType == "im" || _ProcessType == "imd")
                    &&
                            (FillCombo.groupObject.HasPermissionAction(importPermission, FillCombo.groupObjects, "one"))
                    )
                ||
                    (
                        (_ProcessType == "ex" || _ProcessType == "exd" || _ProcessType == "exw")
                    &&
                        (FillCombo.groupObject.HasPermissionAction(exportPermission, FillCombo.groupObjects, "one"))
                    )
                   )
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                                wd_transItemsLocation w;
                                switch (_ProcessType)
                                {
                                    case "exw":
                                    case "exd":
                                        //bool valid = true;
                                        //if(_ProcessType == "exw")
                                        //    valid = await validateOrder();
                                        //if (valid)
                                        Window.GetWindow(this).Opacity = 0.2;
                                        w = new wd_transItemsLocation();
                                        List<ItemTransfer> orderList = new List<ItemTransfer>();
                                        List<int> ordersIds = new List<int>();
                                        foreach (BillDetailsPurchase d in billDetails)
                                        {
                                            if (d.Count == 0)
                                            {
                                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorQuantIsZeroToolTip"), animation: ToasterAnimation.FadeIn);

                                                Window.GetWindow(this).Opacity = 1;
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
                                                ordersIds.Add(d.OrderId);
                                            }
                                        }
                                        w.orderList = orderList;
                                        w.ShowDialog();
                                        if (w.DialogResult == true)
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
                                                #region notification Object
                                                Notification not = new Notification()
                                                {
                                                    title = "trExceedMaxLimitAlertTilte",
                                                    ncontent = "trExceedMaxLimitAlertContent",
                                                    msgType = "alert",
                                                    createDate = DateTime.Now,
                                                    updateDate = DateTime.Now,
                                                    createUserId = MainWindow.userLogin.userId,
                                                    updateUserId = MainWindow.userLogin.userId,
                                                };
                                                #endregion
                                                await FillCombo.itemLocation.recieptOrder(readyItemsLoc, orderList, (int)cb_branch.SelectedValue, MainWindow.userLogin.userId, "storageAlerts_minMaxItem", not);
                                                await save();
                                            }
                                        }
                                        Window.GetWindow(this).Opacity = 1;
                                        break;
                                    default:
                                        await save();
                                        break;
                                }
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
        private async Task saveDraft()
        {
            int invoiceId;
            invoiceItems = new List<ItemTransfer>();
            ItemTransfer itemT;
            for (int i = 0; i < billDetails.Count; i++)
            {
                itemT = new ItemTransfer();

                itemT.quantity = billDetails[i].Count;
                itemT.price = billDetails[i].Price;
                itemT.itemUnitId = billDetails[i].itemUnitId;
                itemT.createUserId = MainWindow.userLogin.userId;
                itemT.invoiceId = billDetails[i].OrderId;

                invoiceItems.Add(itemT);
            }
            switch (_ProcessType)
            {
                case "imd":// add or edit import order then add export order
                           // import order
                    invoice.invType = _ProcessType;
                    invoice.branchId = MainWindow.branchLogin.branchId;
                    invoice.createUserId = MainWindow.userLogin.userId;
                    invoice.updateUserId = MainWindow.userLogin.userId;
                    if (invoice.invNumber == null)
                        invoice.invNumber = await invoice.generateInvNumber("im", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                    // save invoice in DB
                    invoiceId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);
                    if (invoiceId != 0)
                    {
                        // expot order
                        if (invoice.invoiceId == 0) // create new export order
                        {
                            invoice = new Invoice();
                            invoice.invType = "exi";
                            invoice.invoiceMainId = invoiceId;
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            else
                                invoice.branchId = MainWindow.branchLogin.branchId;
                            invoice.invNumber = await invoice.generateInvNumber("ex", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                            invoice.createUserId = MainWindow.userLogin.userId;
                        }
                        else // edit exit export order
                        {
                            invoice = await invoice.getgeneratedInvoice(invoiceId);
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            else
                                invoice.branchId = MainWindow.branchLogin.branchId;
                            invoice.updateUserId = MainWindow.userLogin.userId;
                        }
                        int exportId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);

                        // add order details                      
                        //await invoice.saveInvoiceItems(invoiceItems, invoiceId);
                        //await invoice.saveInvoiceItems(invoiceItems, exportId);

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    break;
                case "exd":// add or edit export order then add import order
                           // import order
                    invoice.invType = _ProcessType;
                    invoice.branchId = MainWindow.branchLogin.branchId;
                    invoice.createUserId = MainWindow.userLogin.userId;
                    invoice.updateUserId = MainWindow.userLogin.userId;
                    if (invoice.invNumber == null)
                        invoice.invNumber = await invoice.generateInvNumber("ex", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                    // save invoice in DB
                    invoiceId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);

                    if (invoiceId != 0)
                    {
                        // import order
                        if (invoice.invoiceId == 0) // create new export order
                        {
                            invoice = new Invoice();
                            invoice.invType = "imi";
                            invoice.invoiceMainId = invoiceId;
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            invoice.invNumber = await invoice.generateInvNumber("im", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                            invoice.createUserId = MainWindow.userLogin.userId;
                        }
                        else // edit exit export order
                        {
                            invoice = await invoice.getgeneratedInvoice(invoiceId);
                            if (cb_branch.SelectedIndex != -1)
                                invoice.branchId = (int)cb_branch.SelectedValue;
                            invoice.updateUserId = MainWindow.userLogin.userId;
                        }
                        int importId = await invoice.saveInvoiceWithItems(invoice, invoiceItems);

                        // add order details
                        //await invoice.saveInvoiceItems(invoiceItems, invoiceId);
                        //await invoice.saveInvoiceItems(invoiceItems, importId);

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    break;
            }
        }
        private async Task<int> getAvailableAmount(int itemId, int itemUnitId, int branchId, int ID)
        {
            // var itemUnits = await itemUnitModel.GetItemUnits(itemId);
            if (FillCombo.itemUnitList == null)
                await FillCombo.RefreshItemUnit();
            var itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == item.itemId).ToList();
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
                        unitValue = await FillCombo.itemUnit.largeToSmallUnitQuan(itemUnitId, u.itemUnitId);
                        quantity = quantity / unitValue;
                    }
                    else
                    {
                        unitValue = await FillCombo.itemUnit.smallToLargeUnit(itemUnitId, u.itemUnitId);

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
        private async Task<Boolean> checkItemsAmounts()
        {
            Boolean available = true;
            for (int i = 0; i < billDetails.Count; i++)
            {
                int availableAmount = await FillCombo.itemLocation.getAmountInBranch(billDetails[i].itemUnitId, MainWindow.branchLogin.branchId);
                if (availableAmount < billDetails[i].Count)
                {
                    available = false;
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableFromToolTip") + " " + billDetails[i].Product, animation: ToasterAnimation.FadeIn);
                    return available;
                }
            }
            return available;
        }
        private void ereaseQuantity()
        {
            foreach (BillDetailsPurchase b in billDetails)
            {
                b.Count = 0;
            }
            refrishDataGridItems();
        }
        #endregion
        #region events
        private void Cb_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Cb_processType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 100 && cb_processType.SelectedIndex != -1)
                {
                    _SelectedProcess = (string)cb_processType.SelectedValue;
                    if (invoice.invoiceId == 0)
                        _ProcessType = cb_processType.SelectedValue + "d";
                    if (cb_processType.SelectedValue.ToString() == "im")
                    {
                        txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementImport");

                        btn_save.Content = AppSettings.resourcemanager.GetString("trImport");
                    }
                    else if (cb_processType.SelectedValue.ToString() == "ex")
                    {
                        txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementExport");
                        btn_save.Content = AppSettings.resourcemanager.GetString("trExport");
                        ereaseQuantity();
                    }
                }
                else
                {
                    cb_processType.SelectedValue = _SelectedProcess;
                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_branch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 100 && cb_branch.SelectedIndex != -1)
                {
                    _SelectedBranch = (int)cb_branch.SelectedValue;
                }
                else
                {
                    cb_branch.SelectedValue = _SelectedBranch;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
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
        Branch branchModel = new Branch();
        public async Task BuildReport()
        {
            Invoice prInvoice = invoice;
            List<ReportParameter> paramarr = new List<ReportParameter>();

            // string reppath = reportclass.GetDirectEntryRdlcpath(prInvoice);
            if (prInvoice.invoiceId > 0)
            {
                string addpath;
                bool isArabic = ReportCls.checkLang();
                if (isArabic)
                {//ItemsExport
                    addpath = @"\Reports\Storage\movementsOperations\Ar\ArMovement.rdlc";
                }
                else
                {
                    addpath = @"\Reports\Storage\movementsOperations\En\EnMovement.rdlc";
                }
                string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

                // invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                //if (prInvoice.agentId != null)
                //{
                //    Agent agentinv = new Agent();
                //    //  agentinv = vendors.Where(X => X.agentId == prInvoice.agentId).FirstOrDefault();
                //    agentinv = await agentinv.getAgentById((int)prInvoice.agentId);
                //    prInvoice.agentCode = agentinv.code;
                //    //new lines
                //    prInvoice.agentName = agentinv.name;
                //    prInvoice.agentCompany = agentinv.company;
                //}
                //else
                //{

                //    prInvoice.agentCode = "-";
                //    //new lines
                //    prInvoice.agentName = "-";
                //    prInvoice.agentCompany = "-";
                //}
                User employ = new User();
                employ = await employ.getUserById((int)prInvoice.updateUserId);
                prInvoice.uuserName = employ.name;
                prInvoice.uuserLast = employ.lastname;


                Branch branchfrom = new Branch();
                Branch branchto = new Branch();
                //

                //
                //if (prInvoice.invoiceMainId == null)
                //{
                //    branch = await branchModel.getBranchById((int)prInvoice.branchCreatorId);
                //    prInvoice.branchCreatorName = branch.name;
                //}
                //branch creator

                if (prInvoice.invoiceMainId == null)
                {
                    if (prInvoice.branchId > 0)
                    {
                        //FROM
                        branchfrom = await branchModel.getBranchById((int)prInvoice.branchId);
                        prInvoice.branchCreatorName = branchfrom.name;
                        //TO
                        Invoice secondinv = new Invoice();
                        secondinv = await invoice.getgeneratedInvoice(prInvoice.invoiceId);
                        if (secondinv.branchId != null)
                        {
                            branchto = await branchModel.getBranchById((int)secondinv.branchId);
                            prInvoice.branchName = branchto.name;
                        }
                        else
                        {
                            prInvoice.branchName = "-";
                        }

                    }
                    else
                    {

                    }

                }
                else
                {// NOT THE CREATOR OF ORDER
                    if (prInvoice.branchId > 0)
                    {
                        //TO
                        branchto = await branchModel.getBranchById((int)prInvoice.branchId);
                        prInvoice.branchName = branchto.name;
                        //FROM
                        Invoice secondinv = new Invoice();
                        secondinv = await invoice.GetByInvoiceId((int)prInvoice.invoiceMainId); ;
                        if (secondinv.branchId != null)
                        {
                            branchfrom = await branchModel.getBranchById((int)secondinv.branchId);
                            prInvoice.branchCreatorName = branchfrom.name;
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
                }
                // end branch  

                paramarr.Add(new ReportParameter("trFromBranchType", clsReports.BranchStoreConverter(branchfrom.type)));
                paramarr.Add(new ReportParameter("trToBranchType", clsReports.BranchStoreConverter(branchto.type)));

                foreach (var i in invoiceItems)
                {
                    i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                    i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));
                }
                clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                clsReports.setReportLanguage(paramarr);
                clsReports.Header(paramarr);
                paramarr = reportclass.fillMovment(prInvoice, paramarr);

                if (_ProcessType == "im" || _ProcessType == "imw" || _ProcessType == "imd")
                {

                    paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trInternalMovementImport")));

                }
                else if (_ProcessType == "ex" || _ProcessType == "exw" || _ProcessType == "exd")
                {

                    paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trInternalMovementExport")));

                }

                

                rep.SetParameters(paramarr);
                rep.Refresh();

            }
        }
        //private void BuildReport()
        //{
        //    List<ReportParameter> paramarr = new List<ReportParameter>();

        //    string addpath;
        //    bool isArabic = ReportCls.checkLang();
        //    //D:\myproj\posproject5\laundryApp\laundryApp\Reports\Storage\movementsOperations\En\EnStorageMovements.rdlc
        //    if (isArabic)
        //    {//ItemsExport
        //        addpath = @"\Reports\Storage\movementsOperations\Ar\ArStorageMovements.rdlc";
        //    }
        //    else
        //    {
        //        addpath = @"\Reports\Storage\movementsOperations\En\EnStorageMovements.rdlc";
        //    }

        //    string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

        //    ReportCls.checkLang();

        //    clsReports.ItemsExportReport(invoiceItems, rep, reppath, paramarr);
        //    clsReports.setReportLanguage(paramarr);
        //    clsReports.Header(paramarr);

        //    rep.SetParameters(paramarr);

        //    rep.Refresh();
        //}
        private async void printExport()
        {
          await  BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
            });
        }
        private async void pdfExport()
        {

           await  BuildReport();

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
        private async void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
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

                        await BuildReport();
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
      
        #endregion
        #region shortage
        private async void Btn_shortageInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (invoice.invoiceId != 0)
                    clearProcess();
                cb_processType.SelectedIndex = 0;
                cb_processType.IsEnabled = false;
                await buildShortageInvoiceDetails();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task buildShortageInvoiceDetails()
        {
            //get invoice items
            invoiceItems = await invoice.getShortageItems(MainWindow.branchLogin.branchId);
            mainInvoiceItems = invoiceItems;
            // build invoice details grid
            _SequenceNum = 0;
            billDetails.Clear();
            foreach (ItemTransfer itemT in invoiceItems)
            {
                _SequenceNum++;
                decimal total = (decimal)(itemT.price * itemT.quantity);
                billDetails.Add(new BillDetailsPurchase()
                {
                    ID = _SequenceNum,
                    Product = itemT.itemName,
                    itemId = (int)itemT.itemId,
                    Unit = itemT.itemUnitId.ToString(),
                    itemUnitId = (int)itemT.itemUnitId,
                    Count = (int)itemT.quantity,
                    OrderId = (int)itemT.invoiceId,
                    Price = decimal.Parse(HelpClass.DecTostring((decimal)itemT.price)),
                    Total = total,
                });
            }
            tb_barcode.Focus();

            refrishBillDetails();
        }

        #endregion
        #region btn
        private async void Btn_orders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();

                if ((
                        FillCombo.groupObject.HasPermissionAction(importPermission, FillCombo.groupObjects, "one")

                        ) &&
                        (
                        FillCombo.groupObject.HasPermissionAction(exportPermission, FillCombo.groupObjects, "one")

                        ))
                    w.invoiceType = "im ,ex";
                else if (FillCombo.groupObject.HasPermissionAction(importPermission, FillCombo.groupObjects, "one"))
                    w.invoiceType = "im";
                else if (FillCombo.groupObject.HasPermissionAction(exportPermission, FillCombo.groupObjects, "one"))
                    w.invoiceType = "ex";


                w.title = AppSettings.resourcemanager.GetString("trOrders");
                w.branchId = MainWindow.branchLogin.branchId;
                w.icon = "orders";
                w.page = "storageMov";
                if (w.ShowDialog() == true)
                {
                    if (w.invoice != null)
                    {
                        invoice = w.invoice;
                        _ProcessType = invoice.invType;
                        _invoiceId = invoice.invoiceId;
                        isFromReport = false;
                        archived = false;
                        setNotifications();
                        await fillOrderInputs(invoice);
                        if (_ProcessType == "im")// set title to bill
                        {
                            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementImport");

                        }
                        else if (_ProcessType == "ex")
                        {
                            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementExport");

                        }
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
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(exportPermission, FillCombo.groupObjects, "one"))
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_invoice w = new wd_invoice();

                    w.invoiceType = "exw";
                    w.title = AppSettings.resourcemanager.GetString("trOrders");
                    w.branchId = MainWindow.branchLogin.branchId;
                    w.icon = "ordersWait";
                    w.page = "storageMov";
                    if (w.ShowDialog() == true)
                    {
                        if (w.invoice != null)
                        {
                            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementExport");

                            invoice = w.invoice;
                            _ProcessType = invoice.invType;
                            _invoiceId = invoice.invoiceId;
                            setNotifications();
                            await fillOrderInputs(invoice);
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
        }
        private void Btn_package_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(packagePermission, FillCombo.groupObjects, "one") )
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_generatePackage w = new wd_generatePackage();

                    if (w.ShowDialog() == true)
                    {

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
        private void Btn_unitConversion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(unitConversionPermission, FillCombo.groupObjects, "one"))
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_unitConversion w = new wd_unitConversion();
                    if (w.ShowDialog() == true)
                    {

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
        }
        private async void Btn_items_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //items
                Window.GetWindow(this).Opacity = 0.2;
                wd_purchaseItems w = new wd_purchaseItems();

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

                if (billDetails.Count != 0 && (_ProcessType == "imd" || _ProcessType == "exd"))
                {
                    #region Accept
                    MainWindow.mainWindow.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trSaveInvoiceNotification");
                    w.ShowDialog();
                    MainWindow.mainWindow.Opacity = 1;
                    #endregion
                    if (w.isOk)
                    {
                        await saveDraft();
                    }                 
                }

                clearProcess();
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
                string invoiceType = "imd ,exd";
                int duration = 2;
                w.invoiceType = invoiceType;
                w.userId = MainWindow.userLogin.userId;
                w.duration = duration; // view drafts which updated during 2 last days 
                w.icon = "drafts";
                w.page = "storageMov";
                w.title = AppSettings.resourcemanager.GetString("trDrafts");

                if (w.ShowDialog() == true)
                {
                    if (w.invoice != null)
                    {
                        invoice = w.invoice;
                        _ProcessType = invoice.invType;
                        _invoiceId = invoice.invoiceId;
                        isFromReport = false;
                        archived = false;
                        setNotifications();
                        await fillOrderInputs(invoice);
                        //if (_ProcessType == "imd")// set title to bill
                        //{
                        //    //  mainInvoiceItems = invoiceItems;

                        //}
                        //else if (_ProcessType == "exd")
                        //{
                        //    //   mainInvoiceItems = await invoiceModel.GetInvoicesItems(invoice.invoiceMainId.Value);

                        //}
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
                //
                //    HelpClass.StartAwait(grid_main);

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
                    int availableAmount = 0;

                    int oldCount = 0;
                    if (!t.Text.Equals(""))
                        oldCount = int.Parse(t.Text);
                    else
                        oldCount = 0;
                    int newCount = 0;
                    //"tb_amont"
                    if (columnName == AppSettings.resourcemanager.GetString("trQuantity"))
                    {
                        if (_ProcessType == "exd")
                        {
                            availableAmount = await getAvailableAmount(row.itemId, row.itemUnitId, MainWindow.branchLogin.branchId, row.ID);
                            if (availableAmount < oldCount)
                            {

                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableToolTip"), animation: ToasterAnimation.FadeIn);
                                newCount = newCount + availableAmount;
                                t.Text = availableAmount.ToString();
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
                        {
                            if (!t.Text.Equals(""))
                                newCount = int.Parse(t.Text);
                            else
                                newCount = 0;
                        }
                    }
                    else
                        newCount = row.Count;

                    if (row.OrderId != 0)
                    {
                        ItemTransfer item = mainInvoiceItems.ToList().Find(i => i.itemUnitId == row.itemUnitId && i.invoiceId == row.OrderId);
                        if (newCount > item.quantity)
                        {
                            // return old value 
                            t.Text = item.quantity.ToString();

                            newCount = (int)item.quantity;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }


                    _Count -= oldCount;
                    _Count += newCount;

                    //  refresh count text box
                    tb_count.Text = _Count.ToString();

                    // update item in billdetails           
                    billDetails[index].Count = (int)newCount;
                    if (invoiceItems != null)
                        invoiceItems[index].quantity = (int)newCount;
                }
                //
                //    HelpClass.EndAwait(grid_main);
                //refrishDataGridItems();

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
                                #region disable & enable unit comboBox
                                if (_ProcessType == "ex" || _ProcessType == "im" || _ProcessType == "exw" || _ProcessType == "imw")
                                    combo.IsEnabled = false;
                                else
                                    combo.IsEnabled = true;
                                #endregion
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

                    #region disable & enable unit comboBox
                    if (_ProcessType == "ex" || _ProcessType == "im" || _ProcessType == "exw" || _ProcessType == "imw")
                        cmb.IsEnabled = false;
                    else
                        cmb.IsEnabled = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Dg_billDetails_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            int column = dg_billDetails.CurrentCell.Column.DisplayIndex;
            if ((_ProcessType == "ex" || _ProcessType == "im" || _ProcessType == "exw" || _ProcessType == "imw") 
                && column == 3)
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
            item = FillCombo.purchaseItems.Find(c => c.itemId == itemId);

            if (item != null)
            {
                this.DataContext = item;

                // get item units
                itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == item.itemId).ToList();
                // search for default unit for purchase
                var defaultPurUnit = itemUnits.Find(c => c.defaultPurchase == 1);
                if (defaultPurUnit != null)
                {
                    // create new row in bill details data grid
                    addRowToBill(item.name, itemId, defaultPurUnit.unitName, defaultPurUnit.itemUnitId, 1);

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
        private void clearProcess()
        {
            _Count = 0;
            _SequenceNum = 0;
            _SelectedBranch = -1;
            _SelectedProcess = "imd";
            _ProcessType = "imd";
            isFromReport = false;
            archived = false;
            invoice = new Invoice();
            generatedInvoice = new Invoice();
            tb_barcode.Clear();
            cb_branch.SelectedIndex = -1;
            billDetails.Clear();

            txt_titleDataGridInvoice.Text = AppSettings.resourcemanager.GetString("trInternalMovementImport");

            refrishBillDetails();
            inputEditable();
            btn_next.Visibility = Visibility.Collapsed;
            btn_previous.Visibility = Visibility.Collapsed;

            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        public async Task fillOrderInputs(Invoice invoice)
        {
            if (invoice.invoiceMainId == null)
                generatedInvoice = await invoice.getgeneratedInvoice(invoice.invoiceId);
            else
                generatedInvoice = await invoice.GetByInvoiceId((int)invoice.invoiceMainId);
            _Count = invoice.itemsCount;
            tb_count.Text = _Count.ToString();

            cb_branch.SelectedValue = generatedInvoice.branchId;
            switch (_ProcessType)
            {
                case "imd":
                case "im":
                case "imw":
                    cb_processType.SelectedIndex = 0;
                    cb_processType.SelectedValue = "im";
                    break;
                case "exd":
                case "ex":
                case "exw":
                    cb_processType.SelectedIndex = 1;
                    cb_processType.SelectedValue = "ex";
                    break;
            }

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
            if (invoice.invoiceId == 0)
                cb_processType.IsEnabled = true;
            else
                cb_processType.IsEnabled = false;

            if (_ProcessType == "imd" || _ProcessType == "exd") // return invoice
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete hidden
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                cb_processType.IsEnabled = true;
                cb_branch.IsEnabled = true;
                tb_barcode.IsEnabled = true;
                btn_save.IsEnabled = true;
                btn_items.IsEnabled = true;
            }
            else if (_ProcessType == "im" || _ProcessType == "ex")
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Collapsed; //make delete hidden
                dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                cb_branch.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                btn_save.IsEnabled = false;
                btn_items.IsEnabled = false;
            }
            else if (_ProcessType == "imw")
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Collapsed; //make delete hidden
                dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                cb_branch.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                btn_save.IsEnabled = true;
                btn_items.IsEnabled = false;
            }
            else if (_ProcessType == "exw")
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete hidden
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                cb_branch.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                btn_save.IsEnabled = true;
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
