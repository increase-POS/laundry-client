using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.Globalization;
using System.Net.Mail;
using System.Windows.Threading;
using System.Threading;


namespace laundryApp.View.purchase
{
    /// <summary>
    /// Interaction logic for uc_purchaseOrder.xaml
    /// </summary>
    public partial class uc_purchaseOrder : UserControl
    {
        private static uc_purchaseOrder _instance;
        public static uc_purchaseOrder Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_purchaseOrder();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_purchaseOrder()
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


        string createPermission = "purchaseOrder_create";
        string reportsPermission = "purchaseOrder_reports";
        string sendEmailPermission = "purchaseOrder_sendEmail";
        string initializeShortagePermission = "purchaseOrder_initializeShortage";

        ObservableCollection<BillDetailsPurchase> billDetails = new ObservableCollection<BillDetailsPurchase>();

        Item item = new Item();
        IEnumerable<Item> items;
        List<ItemUnit> itemUnits;

         public Invoice invoice = new Invoice();
        List<ItemTransfer> invoiceItems;
        List<ItemTransfer> mainInvoiceItems;
        public List<Control> controls;
        #region for notifications
        private static DispatcherTimer timer;
        public static bool isFromReport = false;
        public static bool archived = false;
        int _DraftCount = 0;
        int _OrdersCount = 0;
        int _DocCount = 0;
        #endregion
        #region //to handle barcode characters
        static private int _SelectedVendor = -1;
        bool _IsFocused = false;
        // for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        static private object _Sender;
        #endregion      
        //for bill details
        static private int _SequenceNum = 0;
        static private int _invoiceId;
        static private decimal _Sum = 0;
        static public string _InvoiceType = "pod"; // purchase order draft
        static private decimal _Count = 0;
      
        private void translate()
        {
            ////////////////////////////////----invoice----/////////////////////////////////
            dg_billDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_billDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_billDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_billDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trQTR");

            //txt_shortageInvoice.Text = AppSettings.resourcemanager.GetString("trLack");
            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaceOrder");
            txt_vendor.Text = AppSettings.resourcemanager.GetString("trVendor");
            tb_count.Text = AppSettings.resourcemanager.GetString("trCount:");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendor, AppSettings.resourcemanager.GetString("trVendorHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_drafts.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            txt_purchaseOrder.Text = AppSettings.resourcemanager.GetString("trReady");
            txt_emailMessage.Text = AppSettings.resourcemanager.GetString("trSendEmail");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_pdf.Text = AppSettings.resourcemanager.GetString("trPdfBtn");
            txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_invoiceImages.Text = AppSettings.resourcemanager.GetString("trImages");

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

                saveBeforeExit();
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
        public static List<string> requiredControlList;

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
        async void loading_fillVendorCombo()
        {
            try
            {
                await FillCombo.FillComboVendors(cb_vendor);
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillVendorCombo"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        #endregion
        public async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "vendor" };

                // for pagination
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
                setTimer();

                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_RefrishItems", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillVendorCombo", value = false });

                loading_RefrishItems();
                loading_fillVendorCombo();
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

                refreshNotification();
                //refreshLackNotification();
                //List all the UIElement in the VisualTree
                controls = new List<Control>();
                FindControl(this.grid_main, controls);


                #region Permision


                //if (FillCombo.groupObject.HasPermissionAction(sendEmailPermission, FillCombo.groupObjects, "one"))
                //{
                //    btn_emailMessage.Visibility = Visibility.Visible;
                //    bdr_emailMessage.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    btn_emailMessage.Visibility = Visibility.Collapsed;
                //    bdr_emailMessage.Visibility = Visibility.Collapsed;
                //}

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
                #endregion
                #region print - pdf - send email
                btn_printInvoice.Visibility = Visibility.Collapsed;
                btn_pdf.Visibility = Visibility.Collapsed;
                sp_Approved.Visibility = Visibility.Collapsed;
                btn_emailMessage.Visibility = Visibility.Collapsed;
                bdr_emailMessage.Visibility = Visibility.Collapsed;
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
                if (invoice.invoiceId != 0)
                {
                    refreshOrdersNotification();
                    refreshDocCount(invoice.invoiceId);

                    //refreshLackNotification();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region notification
        private void refreshNotification()
        {
            try
            {
                refreshDraftNotification();
                refreshOrdersNotification();
            }
            catch { }
        }
        private async void refreshDraftNotification()
        {
            try
            {
                string invoiceType = "pod,pos";
                int duration = 1;
                int draftCount = await invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, duration);
                if (invoice != null && (invoice.invType == "pod" || invoice.invType == "pos") && !isFromReport)
                    draftCount--;

                HelpClass.refreshNotification(md_draft, ref _DraftCount, draftCount);               
            }
            catch { }
        }
        private async void refreshOrdersNotification()
        {
            try
            {
                string invoiceType = "po";
                int duration = 1;
                int orderCount = await invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, duration);
                if (invoice != null && invoice.invType == "po" && !isFromReport)
                    orderCount--;

                HelpClass.refreshNotification(md_order, ref _OrdersCount, orderCount);               
            }
            catch { }
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
        private async void refreshDocCount(int invoiceId)
        {
            try
            {
                DocImage doc = new DocImage();
                int docCount = await doc.GetDocCount("Invoices", invoiceId);

                HelpClass.refreshNotification(md_docImage, ref _DocCount, docCount);
            }
            catch { }
        }
        #endregion
        
       
        #region validation
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


        private void input_LostFocus(object sender, RoutedEventArgs e)
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
        #region save invoice

        private async Task<int> addInvoice(string invType)
        {
            if ((invType == "po" || invType == "pos") && (invoice.invType == "pod" || invoice.invoiceId == 0))
                invoice.invNumber = await invoice.generateInvNumber("po", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
            else if (invType == "pod" && invoice.invoiceId == 0)
                invoice.invNumber = await invoice.generateInvNumber("pod", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);

            invoice.branchCreatorId = MainWindow.branchLogin.branchId;
            invoice.branchId = MainWindow.branchLogin.branchId;
            invoice.posId = MainWindow.posLogin.posId;

            invoice.invType = invType;
            invoice.total = 0;
            invoice.totalNet = 0;

            if (cb_vendor.SelectedIndex != -1)
                invoice.agentId = (int)cb_vendor.SelectedValue;

            invoice.notes = tb_notes.Text;
            invoice.taxtype = 2;
            invoice.createUserId = MainWindow.userLogin.userId;
            invoice.updateUserId = MainWindow.userLogin.userId;
            byte isApproved = 0;
            if (tgl_ActiveOffer.IsChecked == true)
                isApproved = 1;
            else
                isApproved = 0;
            invoice.isApproved = isApproved;
            // save invoice in DB
            int invoiceId = await FillCombo.invoice.saveInvoice(invoice);
            invoice.invoiceId = invoiceId;
            if (invoiceId > 0)
            {
                // add invoice details
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
                await FillCombo.invoice.saveInvoiceItems(invoiceItems, invoiceId);

                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
            }
            else
                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

            return invoiceId;
        }
        private bool validateInvoiceValues()
        {
            bool valid = true;

            if (billDetails.Count == 0)
            {
                valid = false;
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAddInvoiceWithoutItems"), animation: ToasterAnimation.FadeIn);
            }
            if (valid)
                valid = validateItemUnits();
            return valid;
        }
        private void Tgl_ActiveOffer_Checked(object sender, RoutedEventArgs e)
        {
            if (tgl_ActiveOffer.IsFocused)
            {
                #region Accept
                if (cb_vendor.SelectedIndex != -1)
                {
                    MainWindow.mainWindow.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trApproveOrderNotification");

                    // w.ShowInTaskbar = false;
                    w.ShowDialog();
                    if (!w.isOk)
                    {
                        tgl_ActiveOffer.IsChecked = false;
                    }
                    else
                    {
                        _InvoiceType = "po";
                        btn_save.Content = AppSettings.resourcemanager.GetString("trSubmit");
                    }
                    MainWindow.mainWindow.Opacity = 1;

                }
                #endregion
                else
                {
                    tgl_ActiveOffer.IsChecked = false;
                    //exp_vendor.IsExpanded = true;
                    requiredControlList = new List<string> { "vendor" };
                    HelpClass.validate(requiredControlList, this);
                }
                inputEditable();

                if (tgl_ActiveOffer.IsChecked == true)
                {
                    dg_billDetails.Columns[3].IsReadOnly = true; //make unit read only
                    dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                }
                else
                {
                    dg_billDetails.Columns[3].IsReadOnly = false; //make unit read only
                    dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                }
                refrishDataGridItems();
            }
        }
        private void Tgl_ActiveOffer_Unchecked(object sender, RoutedEventArgs e)
        {
            if (tgl_ActiveOffer.IsFocused)
            {
                _InvoiceType = invoice.invType;
                btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
                if (tgl_ActiveOffer.IsChecked == true)
                {
                    dg_billDetails.Columns[3].IsReadOnly = true; //make unit read only
                    dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                }
                else
                {
                    dg_billDetails.Columns[3].IsReadOnly = false; //make unit read only
                    dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                }
                inputEditable();
                refrishDataGridItems();
            }
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {

                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    //check mandatory inputs
                    bool valid = validateInvoiceValues();
                    if (valid)
                    {
                        if (HelpClass.validate(requiredControlList, this))
                        {
                            if (tgl_ActiveOffer.IsChecked == true)
                                _InvoiceType = "po";
                            else
                                _InvoiceType = "pos";
                            await addInvoice(_InvoiceType); // po: purchase order

                        if (_InvoiceType == "po")
                        {
                            //_InvoiceType = "pod";
                            clearInvoice();

                        }
                        else
                            inputEditable();

                        refreshNotification();

                    }
                    else
                        {
                            
                            HelpClass.validateEmptyCombo(cb_vendor, p_error_vendor);
                           
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
        private bool validateItemUnits()
        {
            bool valid = true;
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
        private async void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
               
                if (billDetails.Count > 0 && _InvoiceType == "pod")
                {
                    bool valid = validateItemUnits();
                    if (valid)
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
                            await addInvoice(_InvoiceType);
                        
                    }
                }

                clearInvoice();
                refreshNotification();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void clearInvoice()
        {
            _Sum = 0;
            _Count = 0;
            txt_invNumber.Text = "";
            _SequenceNum = 0;
            _SelectedVendor = -1;
            _InvoiceType = "pod"; // purchase order draft
            invoice = new Invoice();
            tb_barcode.Clear();
            cb_vendor.SelectedIndex = -1;
            cb_vendor.SelectedItem = "";
            tb_notes.Clear();
            billDetails.Clear();
            tb_total.Text = "";
            btn_updateVendor.IsEnabled = false;
            md_docImage.Badge = "";
            isFromReport = false;
            archived = false;
            tgl_ActiveOffer.IsChecked = false;
            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseOrder");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            inputEditable();

            refrishBillDetails();
            
            btn_next.Visibility = Visibility.Collapsed;
            btn_previous.Visibility = Visibility.Collapsed;

            requiredControlList = new List<string> { "vendor" };
            HelpClass.clearValidate(requiredControlList, this);
        }
        private async Task<bool> saveBeforeExit()
        {
            int invioceId = 0;
            bool succssess = false;
            if (billDetails.Count > 0 && _InvoiceType == "pod")
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
                {
                    bool valid = validateItemUnits();
                    if (billDetails.Count > 0 && valid)
                    {
                        invioceId = await addInvoice(_InvoiceType);
                        succssess = true;
                    }
                    
                }
                else
                {
                    clearInvoice();
                    refreshDraftNotification();
                    succssess = true;
                }
            }
            else
            {
                clearInvoice();
                refreshDraftNotification();
                succssess = true;
            }
            return succssess;
        }

        #endregion

        #region billdetails
        public async Task ChangeItemIdEvent(int itemId)
        {
            item = FillCombo.purchaseItems.ToList().Find(c => c.itemId == itemId);

            if (item != null)
            {
                //this.DataContext = item;

                // get item units
                itemUnits = FillCombo.itemUnitList.Where(a => a.itemId == item.itemId).ToList();
                // search for default unit for purchase
                var defaultPurUnit = itemUnits.ToList().Find(c => c.defaultPurchase == 1);
                if (defaultPurUnit != null)
                {
                    int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == defaultPurUnit.itemUnitId && p.OrderId == 0).FirstOrDefault());
                    if (index == -1)//item doesn't exist in bill
                    {
                        // create new row in bill details data grid
                        addRowToBill(item.name, itemId, defaultPurUnit.unitName, defaultPurUnit.itemUnitId, 1, 0, 0);
                    }
                    else // item exist prevoiusly in list
                    {
                        billDetails[index].Count++;
                        billDetails[index].Total = billDetails[index].Count * billDetails[index].Price;

                        _Count += billDetails[index].Count;
                        _Sum += billDetails[index].Price;
                    }
                }
                else
                {
                    addRowToBill(item.name, itemId, null, 0, 1, 0, 0);
                }
            }
            tb_total.Text = _Count.ToString();
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
            tb_total.Text = _Count.ToString();
        }
        void refrishDataGridItems()
        {
            dg_billDetails.ItemsSource = null;
            dg_billDetails.ItemsSource = billDetails;
            dg_billDetails.Items.Refresh();
            DataGrid_CollectionChanged(dg_billDetails, null);
        }
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
                            //handle D key
                            btn_printInvoice_Click(null, null);
                            break;
                        case Key.S:
                            //handle X key
                            Btn_save_Click(btn_save, null);
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
                    if (_Sender != null)
                    {
                       TextBox tb = _Sender as TextBox;
                       if (tb != null)
                        {
                            if ( tb.Name == "tb_notes" || tb.Name == "tb_barcode")// remove barcode from text box
                            {
                                string tbString = tb.Text;
                                string newStr = "";
                                int startIndex = tbString.IndexOf(_BarcodeStr);
                                if (startIndex != -1)
                                    newStr = tbString.Remove(startIndex, _BarcodeStr.Length);

                                tb.Text = newStr;
                            }
                        }
                    }
                    tb_barcode.Text = _BarcodeStr;
                    _BarcodeStr = "";
                    _IsFocused = false;
                    e.Handled = true;
                }
                _Sender = null;
                tb_barcode.Clear();
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
                case "po":// this barcode for invoice               
                    Btn_newDraft_Click(null, null);
                    invoice = await FillCombo.invoice.GetInvoicesByNum(barcode);
                    _InvoiceType = invoice.invType;
                    if (_InvoiceType.Equals("po") || _InvoiceType.Equals("pod"))
                    {
                        // set title to bill
                        if (_InvoiceType == "pod")
                        {
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaceOrderDraft");
                            brd_total.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA926"));
                        }
                        else if (_InvoiceType == "po")
                        {
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trOrders");
                            brd_total.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFA926"));
                        }

                        await fillInvoiceInputs(invoice);
                    }
                    break;

                default: // if barcode for item
                         // get item matches barcode
                    if (FillCombo.itemUnitList != null)
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
                                    itemUnits = FillCombo.itemUnitList.ToList().Where(c => c.itemId == itemId).ToList();

                                    //get item from list
                                    item = FillCombo.purchaseItems.ToList().Find(i => i.itemId == itemId);

                                    int count = 1;
                                    decimal price = 0; 
                                    decimal total = 0;
                                    addRowToBill(item.name, item.itemId, unit1.mainUnit, unit1.itemUnitId, count, price, total);
                                }
                                else // item exist prevoiusly in list
                                {
                                    billDetails[index].Count++;
                                    billDetails[index].Total = billDetails[index].Count * billDetails[index].Price;
                                    _Sum += billDetails[index].Price;
                                }
                                refreshTotalValue();
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
        private void addRowToBill(string itemName, int itemId, string unitName, int itemUnitId, int count, decimal price, decimal total)
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
                Count = 1,
                Price = price,
                Total = total,
            });
            _Count++;
            _Sum += total;
        }
        public async Task fillInvoiceInputs(Invoice invoice)
        {
            txt_invNumber.Text = invoice.invNumber.ToString();
            cb_vendor.SelectedValue = invoice.agentId;
            tb_notes.Text = invoice.notes;
            if (invoice.isApproved == 1)
                tgl_ActiveOffer.IsChecked = true;
            else
                tgl_ActiveOffer.IsChecked = false;
            await buildInvoiceDetails();
            inputEditable();
        }
        private async Task buildInvoiceDetails()
        {
            //get invoice items
            invoiceItems = await FillCombo.invoice.GetInvoicesItems(invoice.invoiceId);
            // build invoice details grid
            _SequenceNum = 0;
            billDetails.Clear();
            foreach (ItemTransfer itemT in invoiceItems)
            {
                _Count += (int)itemT.quantity;
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
            tb_total.Text = _Count.ToString();

            tb_barcode.Focus();

            refrishBillDetails();
        }
        private void inputEditable()
        {
            if (_InvoiceType == "pod") // purchase order draft
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete column visible
                dg_billDetails.Columns[3].IsReadOnly = false; //make unit read only
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                cb_vendor.IsEnabled = true;
                tb_notes.IsEnabled = true;
                tb_barcode.IsEnabled = true;
                btn_clear.IsEnabled = true;
                btn_items.IsEnabled = true;
                tgl_ActiveOffer.Visibility = Visibility.Collapsed;
                tgl_ActiveOffer.IsEnabled = false;
                btn_save.IsEnabled = true;
            }
            else if (_InvoiceType == "pos") // purchase order saved
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete column visible
                dg_billDetails.Columns[3].IsReadOnly = false; //make unit read only
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                cb_vendor.IsEnabled = true;
                tb_notes.IsEnabled = true;
                tb_barcode.IsEnabled = true;
                btn_clear.IsEnabled = true;
                btn_items.IsEnabled = true;
                tgl_ActiveOffer.Visibility = Visibility.Visible;
                tgl_ActiveOffer.IsEnabled = true;
                btn_save.IsEnabled = true;
            }
            else if (_InvoiceType == "po") // purchase order
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Collapsed; //make delete column unvisible
                dg_billDetails.Columns[3].IsReadOnly = true; //make unit read only
                dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                cb_vendor.IsEnabled = false;
                tb_notes.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                btn_clear.IsEnabled = false;
                btn_items.IsEnabled = false;
                tgl_ActiveOffer.Visibility = Visibility.Visible;
                tgl_ActiveOffer.IsEnabled = true;
                btn_save.IsEnabled = true;
            }

            if (archived) //come from reports
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Collapsed; //make delete column unvisible
                dg_billDetails.Columns[3].IsReadOnly = true; //make unit read only
                dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                cb_vendor.IsEnabled = false;
                tb_notes.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                tgl_ActiveOffer.IsEnabled = false;
                btn_items.IsEnabled = false;
                btn_clear.IsEnabled = false;
                btn_save.IsEnabled = false;
            }
            if (_InvoiceType.Equals("po") || _InvoiceType.Equals("pos"))
            {
                #region print - pdf - send email
                btn_printInvoice.Visibility = Visibility.Visible;
                btn_pdf.Visibility = Visibility.Visible;
                sp_Approved.Visibility = Visibility.Visible;
                if (FillCombo.groupObject.HasPermissionAction(sendEmailPermission, FillCombo.groupObjects, "one"))
                {
                    btn_emailMessage.Visibility = Visibility.Visible;
                    bdr_emailMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_emailMessage.Visibility = Visibility.Collapsed;
                    bdr_emailMessage.Visibility = Visibility.Collapsed;
                }
                #endregion
            }
            else
            {
                #region print - pdf - send email
                btn_printInvoice.Visibility = Visibility.Collapsed;
                btn_pdf.Visibility = Visibility.Collapsed;
                sp_Approved.Visibility = Visibility.Collapsed;
                btn_emailMessage.Visibility = Visibility.Collapsed;
                bdr_emailMessage.Visibility = Visibility.Collapsed;
                #endregion
            }
            if (!isFromReport)
            {
                btn_next.Visibility = Visibility.Visible;
                btn_previous.Visibility = Visibility.Visible;
            }
        }
        private void refreshTotalValue()
        {
            decimal total = _Sum;
            decimal taxValue = 0;
            decimal taxInputVal = 0;
            if (total != 0)
                taxValue = HelpClass.calcPercentage(total, taxInputVal);
        }

        #endregion billdetails

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
                await navigateInvoice(index);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void clearNavigation()
        {
            _Sum = 0;
            _Count = 0;
            txt_invNumber.Text = "";
            _SequenceNum = 0;
            _SelectedVendor = -1;
            invoice = new Invoice();
            tb_barcode.Clear();
            cb_vendor.SelectedIndex = -1;
            cb_vendor.SelectedItem = "";
            tb_notes.Clear();
            billDetails.Clear();
            tb_total.Text = "";
            btn_updateVendor.IsEnabled = false;
            md_docImage.Badge = "";
            isFromReport = false;
            archived = false;
            tgl_ActiveOffer.IsChecked = false;

            refrishBillDetails();
            btn_next.Visibility = Visibility.Collapsed;
            btn_previous.Visibility = Visibility.Collapsed;
        }
        private async Task navigateInvoice(int index)
        {
            try
            {
                clearNavigation();
                invoice = FillCombo.invoices[index];
                _invoiceId = invoice.invoiceId;
                _InvoiceType = invoice.invType;
                if (invoice.invType == "pod")
                    txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaceOrderDraft");
                else if (invoice.invType == "pos")
                    txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaceOrderSaved");
                navigateBtnActivate();
                await fillInvoiceInputs(invoice);
            }
            catch { }
        }
        private async void Btn_previous_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                int index = FillCombo.invoices.IndexOf(FillCombo.invoices.Where(x => x.invoiceId == _invoiceId).FirstOrDefault());
                index--;
                await navigateInvoice(index);


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
        private void Cbm_unitItemDetails_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                //billDetails
                var cmb = sender as ComboBox;
                cmb.SelectedValue = (int)billDetails[0].itemUnitId;

                if (tgl_ActiveOffer.IsChecked == true)
                    cmb.IsEnabled = false;
                else
                    cmb.IsEnabled = true;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cbm_unitItemDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cmb = sender as ComboBox;

                if (dg_billDetails.SelectedIndex != -1 && cmb.SelectedValue != null)
                {
                    billDetails[dg_billDetails.SelectedIndex].itemUnitId = (int)cmb.SelectedValue;
                    if (tgl_ActiveOffer.IsChecked == true)
                        cmb.IsEnabled = false;
                    else
                        cmb.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
                //return;
            }
        }
        private void DataGrid_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
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
                                combo.SelectedValue = (int)item.itemUnitId;

                                if (tgl_ActiveOffer.IsChecked == true)
                                    combo.IsEnabled = false;
                                else
                                    combo.IsEnabled = true;
                            }
                        }
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Dg_billDetails_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (dg_billDetails.SelectedIndex != -1)
                if (tgl_ActiveOffer.IsChecked == true)
                    e.Cancel = true;
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
                    if (columnName == AppSettings.resourcemanager.GetString("trQTR"))
                        t.Text = billDetails[index].Count.ToString();
                }
                else
                {
                    int oldCount = 0;
                    long newCount = 0;

                    //"tb_amont"
                    if (columnName == AppSettings.resourcemanager.GetString("trQTR"))
                        newCount = int.Parse(t.Text);
                    else
                        newCount = row.Count;
                    if (newCount < 0)
                    {
                        newCount = 0;
                        t.Text = "0";
                    }

                    oldCount = row.Count;

                    if (_InvoiceType == "pbd" || _InvoiceType == "pbw")
                    {
                        ItemTransfer item = mainInvoiceItems.ToList().Find(i => i.itemUnitId == row.itemUnitId && i.invoiceId == row.OrderId);
                        if (newCount > item.quantity)
                        {
                            // return old value 
                            t.Text = item.quantity.ToString();

                            newCount = (long)item.quantity;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }


                    _Count -= oldCount;
                    _Count += newCount;
                    tb_total.Text = _Count.ToString();


                    // update item in billdetails           
                    billDetails[index].Count = (int)newCount;

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
                        _Sum -= row.Total;

                        // remove item from bill
                        billDetails.RemoveAt(index);

                        ObservableCollection<BillDetailsPurchase> data = (ObservableCollection<BillDetailsPurchase>)dg_billDetails.ItemsSource;
                        data.Remove(row);

                        // calculate new total
                        refreshTotalValue();
                    }
                _SequenceNum = 0;
                _Sum = 0;
                for (int i = 0; i < billDetails.Count; i++)
                {
                    _SequenceNum++;
                    _Sum += billDetails[i].Total;
                    billDetails[i].ID = _SequenceNum;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        #region events
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                var Sender = sender as Expander;

                foreach (var control in FindControls.FindVisualChildren<Expander>(this))
                {

                    var expander = control as Expander;
                    if (expander.Tag != null && Sender.Tag != null)
                        if (expander.Tag.ToString() != Sender.Tag.ToString())
                            expander.IsExpanded = false;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_vendor_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                cb_vendor.ItemsSource = FillCombo.vendorsList.Where(x => x.name.Contains(cb_vendor.Text));
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_vendor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 100 && cb_vendor.SelectedIndex != -1)
                {
                    _SelectedVendor = (int)cb_vendor.SelectedValue;
                }
                else
                {
                    cb_vendor.SelectedValue = _SelectedVendor;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Tb_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _Sender = sender;
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
        #endregion
        #region shortageInvoice
        private async void Btn_shortageInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (invoice.invoiceId != 0)
                    clearInvoice();
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
        #region report
        // for report
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
       int prInvoiceId ;
        Invoice invoiceModel = new Invoice();
     Branch   branchModel = new Branch();
        //print
        public async Task<string> SavePurOrderpdf()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string pdfpath = "";

            //
            Invoice prInvoice = new Invoice();
            if (invoice.invoiceId > 0)
            {
                prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);
                pdfpath = @"\Thumb\report\File.pdf";
                pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
                string reppath = reportclass.GetpayInvoiceRdlcpath(prInvoice);
                invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                if (prInvoice.agentId != null)
                {
                    Agent agentinv = new Agent();
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

                invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                //
                Branch branch = new Branch();
                branch = await branchModel.getBranchById((int)prInvoice.branchCreatorId);
                //branch creator
                if (branch.branchId > 0)
                {
                    prInvoice.branchCreatorName = branch.name;
                }

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
                //
                User employ = new User();
                employ = await employ.getUserById((int)prInvoice.updateUserId);
                prInvoice.uuserName = employ.name;
                prInvoice.uuserLast = employ.lastname;

                ReportCls.checkLang();

                clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                clsReports.setReportLanguage(paramarr);
                clsReports.Header(paramarr);
                paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                rep.SetParameters(paramarr);
                rep.Refresh();

                LocalReportExtensions.ExportToPDF(rep, pdfpath);

            }

            return pdfpath;
        }
        //print
        private async void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    Thread t1 = new Thread(() =>
                    {
                        pdfPurInvoice();
                    });
                    t1.Start();
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


        private async void btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    Thread t1 = new Thread(() =>
                    {
                        printPurInvoice();
                    });
                    t1.Start();
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

        public async void printPurInvoice()
        {
            Invoice prInvoice = new Invoice();
            if (invoice.invoiceId > 0)
            {
                prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);

                if (prInvoice.invType == "pd" || prInvoice.invType == "sd" || prInvoice.invType == "qd"
                                   || prInvoice.invType == "sbd" || prInvoice.invType == "pbd"
                                   || prInvoice.invType == "ord" || prInvoice.invType == "imd" || prInvoice.invType == "exd")
                {
                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPrintDraftInvoice"), animation: ToasterAnimation.FadeIn);
                }
                else
                {
                    ReportCls rr = new ReportCls();

                    List<ReportParameter> paramarr = new List<ReportParameter>();

                    string reppath = reportclass.GetpayInvoiceRdlcpath(prInvoice);
                    if (prInvoice.invoiceId > 0)
                    {
                        invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                        Agent agentinv = new Agent();
                        agentinv =await agentinv.getAgentById( (int)prInvoice.agentId) ;

                        User employ = new User();
                        employ = await employ.getUserById((int)prInvoice.updateUserId);
                        prInvoice.uuserName = employ.name;
                        prInvoice.uuserLast = employ.lastname;

                        prInvoice.agentCode = agentinv.code;
                        //new lines
                        prInvoice.agentName = agentinv.name;
                        prInvoice.agentCompany = agentinv.company;

                        invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                        //
                        Branch branch = new Branch();
                        branch = await branchModel.getBranchById((int)prInvoice.branchCreatorId);
                        //branch creator
                        if (branch.branchId > 0)
                        {
                            prInvoice.branchCreatorName = branch.name;
                        }

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
                        //

                        ReportCls.checkLang();

                        clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                        clsReports.setReportLanguage(paramarr);
                        clsReports.Header(paramarr);
                        paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                        rep.SetParameters(paramarr);
                        rep.Refresh();

                        this.Dispatcher.Invoke(() =>
                        {

                            LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.pur_copy_count));
                        });
                    }
                }
            }
        }
        public async void pdfPurInvoice()
        {
            Invoice prInvoice = new Invoice();
            if (invoice.invoiceId > 0)
            {
                prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);

                if (prInvoice.invType == "pd" || prInvoice.invType == "sd" || prInvoice.invType == "qd"
                   || prInvoice.invType == "sbd" || prInvoice.invType == "pbd"
                   || prInvoice.invType == "ord" || prInvoice.invType == "imd" || prInvoice.invType == "exd")
                {
                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPrintDraftInvoice"), animation: ToasterAnimation.FadeIn);
                }
                else
                {

                    List<ReportParameter> paramarr = new List<ReportParameter>();


                    string reppath = reportclass.GetpayInvoiceRdlcpath(invoice);
                    if (prInvoice.invoiceId > 0)
                    {
                        invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                        Agent agentinv = new Agent();
                        agentinv = await agentinv.getAgentById((int)prInvoice.agentId);  

                        prInvoice.agentCode = agentinv.code;
                        //new lines
                        prInvoice.agentName = agentinv.name;
                        prInvoice.agentCompany = agentinv.company;

                        User employ = new User();
                        employ = await employ.getUserById((int)prInvoice.updateUserId);
                        prInvoice.uuserName = employ.name;
                        prInvoice.uuserLast = employ.lastname;


                        //
                        Branch branch = new Branch();
                        branch = await branchModel.getBranchById((int)prInvoice.branchCreatorId);
                        //branch creator
                        if (branch.branchId > 0)
                        {
                            prInvoice.branchCreatorName = branch.name;
                        }

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
                        //

                        ReportCls.checkLang();

                        clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                        clsReports.setReportLanguage(paramarr);
                        clsReports.Header(paramarr);
                        paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                        rep.SetParameters(paramarr);
                        rep.Refresh();
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

                }
            }
        }
        private async void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    #region
                    Invoice prInvoice = new Invoice();
                    if (invoice.invoiceId > 0)
                    {
                        prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);
                        Window.GetWindow(this).Opacity = 0.2;

                        List<ReportParameter> paramarr = new List<ReportParameter>();
                        string pdfpath;

                        pdfpath = @"\Thumb\report\temp.pdf";
                        pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
                        string reppath = reportclass.GetpayInvoiceRdlcpath(invoice);
                        if (prInvoice.invoiceId > 0)
                        {
                            invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                            if (prInvoice.agentId != null)
                            {
                                Agent agentinv = new Agent();
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

                            invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                            //
                            Branch branch = new Branch();
                            branch = await branchModel.getBranchById((int)prInvoice.branchCreatorId);
                            //branch creator
                            if (branch.branchId > 0)
                            {
                                prInvoice.branchCreatorName = branch.name;
                            }

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
                            //
                            User employ = new User();
                            employ = await employ.getUserById((int)prInvoice.updateUserId);
                            prInvoice.uuserName = employ.name;
                            prInvoice.uuserLast = employ.lastname;

                            ReportCls.checkLang();

                            clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                            clsReports.setReportLanguage(paramarr);
                            clsReports.Header(paramarr);
                            paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                            rep.SetParameters(paramarr);
                            rep.Refresh();

                            LocalReportExtensions.ExportToPDF(rep, pdfpath);
                        }

                        wd_previewPdf w = new wd_previewPdf();
                        w.pdfPath = pdfpath;
                        if (!string.IsNullOrEmpty(w.pdfPath))
                        {
                    // w.ShowInTaskbar = false;
                            w.ShowDialog();
                            w.wb_pdfWebViewer.Dispose();
                        }
                        else
                            Toaster.ShowError(Window.GetWindow(this), message: "", animation: ToasterAnimation.FadeIn);
                        Window.GetWindow(this).Opacity = 1;
                    }
                    else
                    {
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSaveInvoiceToPreview"), animation: ToasterAnimation.FadeIn);
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

        private async void Btn_emailMessage_Click(object sender, RoutedEventArgs e)
        {//email
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    /////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        sendPurEmail();
                    });
                    t1.Start();
                    /////////////////////////////
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
        public async void sendPurEmail()
        {
            Invoice prInvoice = new Invoice();
            prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);
            SysEmails email = new SysEmails();
            EmailClass mailtosend = new EmailClass();
            email = await email.GetByBranchIdandSide((int)MainWindow.branchLogin.branchId, "purchase");
            Agent toAgent = new Agent();
            toAgent = await toAgent.getAgentById((int)prInvoice.agentId) ;
            
            //  int? itemcount = invoiceItems.Count();
            if (email.emailId == 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoEmailForThisDept"), animation: ToasterAnimation.FadeIn);
            else
            {
                if (prInvoice.invoiceId == 0)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trThereIsNoOrderToSen"), animation: ToasterAnimation.FadeIn);
                else
                {
                    if (invoiceItems == null || invoiceItems.Count() == 0)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trThereIsNoItemsToSend"), animation: ToasterAnimation.FadeIn);
                    else
                    {
                        if (toAgent.email.Trim() == "")
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheVendorHasNoEmail"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            SetValues setvmodel = new SetValues();

                            string pdfpath = await SavePurOrderpdf();
                            mailtosend.AddAttachTolist(pdfpath);
                            List<SetValues> setvlist = new List<SetValues>();
                            setvlist = await setvmodel.GetBySetName("pur_order_email_temp");

                            mailtosend = mailtosend.fillOrderTempData(prInvoice, invoiceItems, email, toAgent, setvlist);
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                                string msg = "";
                                msg = mailtosend.Sendmail();//tempdelite
                                if (msg == "Failure sending mail.")
                                {
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoConnection"), animation: ToasterAnimation.FadeIn);
                                }
                                else if (msg == "mailsent")
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailSent"), animation: ToasterAnimation.FadeIn);
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailNotSent"), animation: ToasterAnimation.FadeIn);
                            }));

                        }
                    }
                }
            }
        }

        #endregion

        #region btn
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
                    refreshTotalValue();
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
        private async void Btn_draft_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                bool res = await saveBeforeExit();
                while (!res) { }
                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();

                // purchase drafts and purchase bounce drafts
                string invoiceType = "pod, pos";
                int duration = 1;
                w.invoiceType = invoiceType;
                w.page = "purchaseOrders";
                w.icon = "drafts";
                w.userId = MainWindow.userLogin.userId;
                w.duration = duration; // view drafts which created during 2 last days 
                w.title = AppSettings.resourcemanager.GetString("trDrafts");

                if (w.ShowDialog() == true)
                {
                    if (w.invoice != null)
                    {
                        invoice = w.invoice;
                        _InvoiceType = invoice.invType;
                        _invoiceId = invoice.invoiceId;
                        isFromReport = false;
                        archived = false;
                        // notifications
                        refreshNotification();
                        refreshDocCount(invoice.invoiceId);
                        await fillInvoiceInputs(invoice);

                        mainInvoiceItems = invoiceItems;
                        if (_InvoiceType == "pod")
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaceOrderDraft");
                        else
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaceOrderSaved");

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
       
        private async void Btn_invoiceImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    if (invoice != null && invoice.invoiceId != 0)
                    {
                        Window.GetWindow(this).Opacity = 0.2;

                        wd_uploadImage w = new wd_uploadImage();

                        w.tableName = "invoices";
                        w.tableId = invoice.invoiceId;
                        w.docNum = invoice.invNumber;
                    // w.ShowInTaskbar = false;
                        w.ShowDialog();
                        refreshDocCount(invoice.invoiceId);
                        Window.GetWindow(this).Opacity = 1;
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trChooseInvoiceToolTip"), animation: ToasterAnimation.FadeIn);



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
        private async void Btn_purchaseOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                saveBeforeExit();
                wd_invoice w = new wd_invoice();
                string invoiceType = "po";
                int duration = 1;
                w.invoiceType = invoiceType;
                w.icon = "invoices";
                w.page = "purchaseOrders";
                w.userId = MainWindow.userLogin.userId;
                w.branchCreatorId = MainWindow.branchLogin.branchId;
                w.duration = duration; // view purchase orders which updated during  last one day 
                w.title = AppSettings.resourcemanager.GetString("trOrders");

                if (w.ShowDialog() == true)
                {
                    if (w.invoice != null)
                    {
                        invoice = w.invoice;
                        _InvoiceType = invoice.invType;
                        _invoiceId = invoice.invoiceId;
                        isFromReport = false;
                        archived = false;
                        // notifications
                        refreshNotification();
                        refreshDocCount(invoice.invoiceId);

                        await fillInvoiceInputs(invoice);

                        mainInvoiceItems = invoiceItems;
                        txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaceOrder");
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
        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

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
                        Count = (int)itemT.quantity,
                        Price = (decimal)itemT.price,
                        Total = total,
                    });
                }
                tb_barcode.Focus();

                refrishBillDetails();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_addVendor_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_updateVendor w = new wd_updateVendor();
                // pass agent id to update windows
                w.agent.agentId = 0;
                w.type = "v";
                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                if (w.isOk == true)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    await FillCombo.RefreshVendors();
                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
            */
        }
        private async void Btn_updateVendor_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {

                HelpClass.StartAwait(grid_main);

                if (cb_vendor.SelectedIndex != -1)
                {

                    Window.GetWindow(this).Opacity = 0.2;
                    wd_updateVendor w = new wd_updateVendor();
                    // pass agent id to update windows
                    w.agent.agentId = (int)cb_vendor.SelectedValue;
                    // w.ShowInTaskbar = false;
                    w.ShowDialog();
                    await FillCombo.RefreshVendors();

                    Window.GetWindow(this).Opacity = 1;
                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
            */
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                clearInvoice();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion
        #region refrish     
        #endregion
        private void moveControlToBarcode(object sender, KeyEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                DatePicker dt = sender as DatePicker;
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds < 100)
                {
                    tb_barcode.Focus();
                    HandleKeyPress(sender, e);
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
