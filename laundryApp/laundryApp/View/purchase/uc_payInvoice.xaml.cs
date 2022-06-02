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
using System.Windows.Threading;
using System.Threading;
using System.Windows.Resources;
using laundryApp.View.windows;

namespace laundryApp.View.purchase
{
    /// <summary>
    /// Interaction logic for uc_payInvoice.xaml
    /// </summary>
    public partial class uc_payInvoice : UserControl
    {
        string invoicePermission = "payInvoice_invoice";
        string returnPermission = "payInvoice_return";
        string paymentsPermission = "payInvoice_payments";
        string sendEmailPermission = "payInvoice_sendEmail";
        string openOrderPermission = "payInvoice_openOrder";
        string initializeShortagePermission = "payInvoice_initializeShortage";
        string printCountPermission = "payInvoice_printCount";

        private static uc_payInvoice _instance;
        public static uc_payInvoice Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_payInvoice();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_payInvoice()
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
        public List<Control> controls;
        public static bool isFromReport = false;
        public static bool archived = false;
        Item item = new Item();
        List<ItemUnit> itemUnits;
        public Invoice invoice = new Invoice();
        List<ItemTransfer> invoiceItems;
        List<ItemTransfer> mainInvoiceItems;
        CashTransfer cashTransfer = new CashTransfer();
        #region //to handle barcode characters
        static private int _SelectedBranch = -1;
        static private int _SelectedVendor = -1;
        static private int _SelectedDiscountType = -1;
        static private string _SelectedPaymentType = "cash";
        static private int _SelectedCard = -1;
        // for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        static private object _Sender;
        bool _IsFocused = false;
        #endregion
        #region for notifications
        private static DispatcherTimer timer;
        int _DraftCount = 0;
        int _InvCount = 0;
        int _DocCount = 0;
        int _OrderCount = 0;
        int _PaymentCount = 0;

        #endregion
        //for bill details
        static private int _SequenceNum = 0;
        static private int _invoiceId;
        static private decimal _Sum = 0;
        static public string _InvoiceType = "pd"; // purchase draft
       
        public static List<string> requiredControlList = new List<string>();
        #region loading
        List<keyValueBool> loadingList;
        async void loading_RefrishItems()
        {
            try
            {
                if (FillCombo.purchaseItems == null)
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
                await FillCombo.fillBranchesWithoutCurrent(cb_branch, "bs");
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
        async void loading_fillCardCombo()
        {
            try
            {
                if (FillCombo.cardsList is null)
                    await FillCombo.RefreshCards();
                InitializeCardsPic(FillCombo.cardsList);
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillCardCombo"))
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
                MainWindow.mainWindow.KeyDown += HandleKeyPress;
                HelpClass.StartAwait(grid_main);

                
                tb_moneyIcon.Text = AppSettings.Currency;
                tb_moneyIconTotal.Text = AppSettings.Currency;
                dp_desrvedDate.SelectedDateChanged += this.dp_SelectedDateChanged;
                dp_invoiceDate.SelectedDateChanged += this.dp_SelectedDateChanged;

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                controls = new List<Control>();
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_RefrishItems", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillBranchesWithoutCurrent", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillCardCombo", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillVendorCombo", value = false });


                loading_RefrishItems();
                loading_fillBranchesWithoutCurrent();
                loading_fillCardCombo();
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
                if (AppSettings.invoiceTax_bool == false)
                    sp_tax.Visibility = Visibility.Collapsed;
                else
                {
                    sp_tax.Visibility = Visibility.Visible;
                    tb_taxValue.Text = HelpClass.PercentageDecTostring(AppSettings.invoiceTax_decimal);
                }

                setTimer();
                FillCombo.FillDiscountType(cb_typeDiscount);
                FillCombo.FillDefaultPayType_cashBalanceCardMultiple(cb_paymentProcessType);
                await FillCombo.FillComboVendors(cb_vendor);
                setNotifications();
                #region Style Date
                HelpClass.defaultDatePickerStyle(dp_desrvedDate);
                HelpClass.defaultDatePickerStyle(dp_invoiceDate);
                #endregion

                 
                //Walk through the VisualTree
                FindControl(this.grid_main, controls);

                #region Permision

                if (FillCombo.groupObject.HasPermissionAction(openOrderPermission, FillCombo.groupObjects, "one"))
                    md_orders.Visibility = Visibility.Visible;
                else
                    md_orders.Visibility = Visibility.Collapsed;

                if (FillCombo.groupObject.HasPermissionAction(returnPermission, FillCombo.groupObjects, "one"))
                {
                    brd_returnInvoice.Visibility = Visibility.Visible;
                }
                else
                {
                    brd_returnInvoice.Visibility = Visibility.Collapsed;
                }

                //if (FillCombo.groupObject.HasPermissionAction(paymentsPermission, FillCombo.groupObjects, "one"))
                //{
                //    md_payments.Visibility = Visibility.Visible;
                //    bdr_payments.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    md_payments.Visibility = Visibility.Collapsed;
                //    bdr_payments.Visibility = Visibility.Collapsed;
                //}

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

                //if (FillCombo.groupObject.HasPermissionAction(printCountPermission, FillCombo.groupObjects, "one"))
                //{
                //    btn_printCount.Visibility = Visibility.Visible;
                //    bdr_printCount.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    btn_printCount.Visibility = Visibility.Collapsed;
                //    bdr_printCount.Visibility = Visibility.Collapsed;
                //}

                #endregion
                #region print - pdf - send email
                btn_printInvoice.Visibility = Visibility.Collapsed;
                btn_pdf.Visibility = Visibility.Collapsed;
                btn_printCount.Visibility = Visibility.Collapsed;
                btn_emailMessage.Visibility = Visibility.Collapsed;
                bdr_emailMessage.Visibility = Visibility.Collapsed;
                #endregion


                //txt_payment.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                HelpClass.EndAwait(grid_main);
                tb_barcode.Focus();
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {               
                HelpClass.StartAwait(grid_main);

                MainWindow.mainWindow.KeyDown -= HandleKeyPress;
                saveBeforeExit();
                timer.Stop();
 
                HelpClass.EndAwait(grid_main);

                Instance = null;
                GC.Collect();
            }
            catch 
            {               
                HelpClass.EndAwait(grid_main);
            }
        }
        private void translate()
        {
            ////////////////////////////////----invoice----/////////////////////////////////
            dg_billDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_billDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_billDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_billDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trQTR");
            dg_billDetails.Columns[5].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_billDetails.Columns[6].Header = AppSettings.resourcemanager.GetString("trTotal");

            txt_discount.Text = AppSettings.resourcemanager.GetString("trDiscount");
            txt_sum.Text = AppSettings.resourcemanager.GetString("trSum");
            txt_total.Text = AppSettings.resourcemanager.GetString("trTotal");
            txt_tax.Text = AppSettings.resourcemanager.GetString("trTax");
            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseBill");
            txt_store.Text = AppSettings.resourcemanager.GetString("trStore/Branch");
            txt_vendor.Text = AppSettings.resourcemanager.GetString("trVendor");
            txt_vendorIvoiceDetails.Text = AppSettings.resourcemanager.GetString("trVendorDetails");

            //txt_shortageInvoice.Text = AppSettings.resourcemanager.GetString("trLack");
            txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_invoiceImages.Text = AppSettings.resourcemanager.GetString("trImages");
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_drafts.Text = AppSettings.resourcemanager.GetString("trDrafts");
            txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            txt_payments.Text = AppSettings.resourcemanager.GetString("trPayments");
            txt_returnInvoice.Text = AppSettings.resourcemanager.GetString("trReturn");
            txt_invoices.Text = AppSettings.resourcemanager.GetString("trInvoices");
            txt_purchaseOrder.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_payment.Text = AppSettings.resourcemanager.GetString("trPayment1");
            txt_totalDescount.Text = AppSettings.resourcemanager.GetString("trDiscount");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_paymentProcessType, AppSettings.resourcemanager.GetString("trPaymentProcessType"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));
           // MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branch, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_discount, AppSettings.resourcemanager.GetString("trDiscountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_typeDiscount, AppSettings.resourcemanager.GetString("trDiscountTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branch, AppSettings.resourcemanager.GetString("trStore/BranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendor, AppSettings.resourcemanager.GetString("trVendorHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_desrvedDate, AppSettings.resourcemanager.GetString("trDeservedDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_invoiceNumber, AppSettings.resourcemanager.GetString("trInvoiceNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_invoiceDate, AppSettings.resourcemanager.GetString("trInvoiceDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            tt_error_previous.Content = AppSettings.resourcemanager.GetString("trPrevious");
            tt_error_next.Content = AppSettings.resourcemanager.GetString("trNext");

            btn_save.Content = AppSettings.resourcemanager.GetString("trBuy");
        }
        #region timer to refresh notifications
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
                await refreshOrdersNotification();
                //refreshLackNotification();
                if (invoice.invoiceId != 0)
                {
                    refreshDocCount(invoice.invoiceId);
                    if (_InvoiceType == "pw" || _InvoiceType == "p" || _InvoiceType == "pb" || _InvoiceType == "pbw")
                        refreshPaymentsNotification(invoice.invoiceId);
                }
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
                refreshInvNotification();
                refreshOrdersNotification();
                //refreshLackNotification();
            }
            catch { }

        }
        private async Task refreshDraftNotification()
        {
            try
            {
                string invoiceType = "pd ,pbd";
                int duration = 1;
                int draftCount = await invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, duration);
                if (invoice != null && (invoice.invType == "pd" || invoice.invType == "pbd") && invoice.invoiceId != 0 && !isFromReport)
                    draftCount--;

                HelpClass.refreshNotification(md_draft, ref _DraftCount, draftCount);                
            }
            catch { }
        }
        private async Task refreshInvNotification()
        {
            try
            {
                string invoiceType = "p ,pw ,pb ,pbw";
                int duration = 1;
                int invCount = await invoice.GetCountByCreator(invoiceType, MainWindow.userLogin.userId, duration);
                if (invoice != null && (invoice.invType == "p" || invoice.invType == "pb" || invoice.invType == "pbw" || invoice.invType == "pw") && !isFromReport)
                    invCount--;

                HelpClass.refreshNotification(md_invoices, ref _InvCount, invCount);                
            }
            catch { }
        }
        private async Task refreshOrdersNotification()
        {
            try
            {
                string invoiceType = "po";
                int ordersCount = await invoice.GetCountUnHandeledOrders(invoiceType, 0, 0);
                if (invoice != null && _InvoiceType == "po" && invoice != null && invoice.invoiceId != 0)
                    ordersCount--;

                HelpClass.refreshNotification(md_orders, ref _OrderCount, ordersCount);              
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
        private async void refreshPaymentsNotification(int invoiceId)
        {
            try
            {
                int paymentsCount = await cashTransfer.GetCashCount(invoice.invoiceId);
                if (paymentsCount == 0)
                {
                    bdr_payments.Visibility = Visibility.Collapsed;
                    md_payments.Visibility = Visibility.Collapsed;
                }
                else if (FillCombo.groupObject.HasPermissionAction(paymentsPermission, FillCombo.groupObjects, "one"))
                {
                    bdr_payments.Visibility = Visibility.Visible;
                    md_payments.Visibility = Visibility.Visible;

                    HelpClass.refreshNotification(md_payments, ref _PaymentCount, paymentsCount);
                }
            }
            catch { }
        }
        #endregion
        #region refrish 
        #endregion
        #region save
        private async Task addInvoice(string invType, string invCode)
        {
            #region invoice object
            if ((invoice.invType == "p" || invoice.invType == "pw") && (invType == "pbw" || invType == "pbd")) // invoice is purchase and will be bounce purchase  or purchase bounce draft , save another invoice in db
            {
                invoice.invoiceMainId = invoice.invoiceId;
                invoice.invoiceId = 0;
                invoice.invNumber = await invoice.generateInvNumber("pb", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
                invoice.branchCreatorId = MainWindow.branchLogin.branchId;
                invoice.posId = MainWindow.posLogin.posId;
            }
            else if (invoice.invType == "po")
            {
                invoice.invNumber = await invoice.generateInvNumber("pi", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
            }
            else if (invType == "pd" && invoice.invoiceId == 0)
                invoice.invNumber = await invoice.generateInvNumber("pd", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);

            if (invoice.branchCreatorId == 0 || invoice.branchCreatorId == null)
            {
                invoice.branchCreatorId = MainWindow.branchLogin.branchId;
                invoice.posId = MainWindow.posLogin.posId;
            }

            //if (invoice.invType != "pw" || invoice.invoiceId == 0)
            //{
            invoice.invType = invType;
            if (!tb_discount.Text.Equals(""))
                invoice.discountValue = decimal.Parse(tb_discount.Text);

            if (cb_typeDiscount.SelectedIndex != -1)
                invoice.discountType = cb_typeDiscount.SelectedValue.ToString();

            invoice.total = _Sum;
            invoice.totalNet = decimal.Parse(tb_total.Text);
            invoice.paid = 0;
            invoice.deserved = invoice.totalNet;
            if (cb_vendor.SelectedIndex != -1 && cb_vendor.SelectedIndex != 0)
                invoice.agentId = (int)cb_vendor.SelectedValue;

            if (cb_branch.SelectedIndex != -1 && cb_branch.SelectedIndex != 0)
                invoice.branchId = (int)cb_branch.SelectedValue;
            else
                invoice.branchId = MainWindow.branchLogin.branchId;

            invoice.deservedDate = dp_desrvedDate.SelectedDate;
            invoice.vendorInvNum = tb_invoiceNumber.Text;
            invoice.vendorInvDate = dp_invoiceDate.SelectedDate;
            invoice.notes = tb_notes.Text;
            invoice.taxtype = 2;
            if (tb_taxValue.Text != "")
                invoice.tax = decimal.Parse(tb_taxValue.Text);
            else
                invoice.tax = 0;

            invoice.createUserId = MainWindow.userLogin.userId;
            invoice.updateUserId = MainWindow.userLogin.userId;
            if (invType == "pw" || invType == "p")
                invoice.invNumber = await invoice.generateInvNumber("pi", MainWindow.branchLogin.code, MainWindow.branchLogin.branchId);
            #endregion
            #region invoice items
            invoiceItems = new List<ItemTransfer>();
            ItemTransfer itemT;
            for (int i = 0; i < billDetails.Count; i++)
            {
                itemT = new ItemTransfer();

                //itemT.invoiceId = invoiceId;
                itemT.quantity = billDetails[i].Count;
                itemT.price = billDetails[i].Price;
                itemT.itemUnitId = billDetails[i].itemUnitId;
                itemT.createUserId = MainWindow.userLogin.userId;
                itemT.invoiceId = billDetails[i].OrderId;

                invoiceItems.Add(itemT);
            }
            #endregion
            // save invoice in DB
            int invoiceId = await FillCombo.invoice.saveInvoiceWithItems(invoice, invoiceItems);
            prInvoiceId = invoiceId;
            invoice.invoiceId = invoiceId;
            if (invoiceId != 0)
            {            
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                if (invType == "p")
                    FillCombo.invoice.saveAvgPurchasePrice(invoiceItems);
            }
            else
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);


        }
        private bool validateInvoiceValues()
        {
            if (decimal.Parse(tb_total.Text) == 0)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorTotalIsZeroToolTip"), animation: ToasterAnimation.FadeIn);
                return false;
            }
            if (cb_paymentProcessType.SelectedValue.ToString() == "cash" && MainWindow.posLogin.balance < decimal.Parse(tb_total.Text))
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);
                return false;
            }

            return true;
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                HelpClass.StartAwait(grid_main);
               
                if (FillCombo.groupObject.HasPermissionAction(invoicePermission, FillCombo.groupObjects, "one") )
                {
                    if (MainWindow.posLogin.boxState == "o") // box is open
                    {
                        //check mandatory inputs
                        if (HelpClass.validate(requiredControlList, this))
                        {
                            bool validate = validateInvoiceValues();
                            bool valid = validateItemUnits();
                            TextBox tb = (TextBox)dp_desrvedDate.Template.FindName("PART_TextBox", dp_desrvedDate);
                            if (valid && validate)
                            {
                                bool multipleValid = true;
                                List<CashTransfer> listPayments = new List<CashTransfer>();

                                if (_InvoiceType == "pbd") //pbd means purchase bounse draft
                                {
                                    #region notification Object
                                    Notification not = new Notification()
                                    {
                                        title = "trPurchaseReturnInvoiceAlertTilte",
                                        ncontent = "trPurchaseReturnInvoiceAlertContent",
                                        msgType = "alert",
                                        createUserId = MainWindow.userLogin.userId,
                                        updateUserId = MainWindow.userLogin.userId,
                                    };
                                    await not.save(not, (int)invoice.branchCreatorId, "storageAlerts_ctreatePurchaseReturnInvoice", MainWindow.branchLogin.name);
                                    #endregion
                                    await addInvoice("pbw", "pb"); // pbw means waiting purchase bounce
                                    clearInvoice();
                                    _InvoiceType = "pd";
                                }

                                else//pw  waiting purchase invoice
                                {
                                    if (cb_paymentProcessType.SelectedValue.ToString() == "multiple")
                                    {
                                        Window.GetWindow(this).Opacity = 0.2;
                                        wd_multiplePayment w = new wd_multiplePayment();
                                        if (cb_vendor.SelectedIndex > 0)
                                            w.hasCredit = true;
                                        else
                                            w.hasCredit = false;
                                        w.isPurchase = true;
                                        w.invoice.invType = _InvoiceType;
                                        w.invoice.totalNet = decimal.Parse(tb_total.Text);
                                        w.cards = FillCombo.cardsList;
                                        // w.ShowInTaskbar = false;
                                        w.ShowDialog();
                                        Window.GetWindow(this).Opacity = 1;
                                        multipleValid = w.isOk;
                                        listPayments = w.listPayments;
                                    }
                                    #region save
                                    if (multipleValid)
                                    {
                                        if (cb_paymentProcessType.SelectedValue.ToString() == "cash" && MainWindow.posLogin.balance < invoice.totalNet)
                                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);

                                        else
                                        {
                                            if (cb_branch.SelectedIndex == -1 || cb_branch.SelectedIndex == 0) // reciept invoice directly
                                            {
                                                await addInvoice("p", "pi");
                                                #region notification Object
                                                Notification not = new Notification()
                                                {
                                                    title = "trExceedMaxLimitAlertTilte",
                                                    ncontent = "trExceedMaxLimitAlertContent",
                                                    msgType = "alert",
                                                    createUserId = MainWindow.userLogin.userId,
                                                    updateUserId = MainWindow.userLogin.userId,
                                                };
                                                #endregion
                                                await FillCombo.itemLocation.recieptInvoice(invoiceItems, MainWindow.branchLogin.branchId, MainWindow.userLogin.userId, "storageAlerts_minMaxItem", not); // increase item quantity in DB
                                            }
                                            else
                                            {
                                                await addInvoice("pw", "pi");
                                                #region notification Object
                                                if ((int)cb_branch.SelectedIndex != -1 && (int)cb_branch.SelectedIndex != 0)
                                                {
                                                    Notification not = new Notification()
                                                    {
                                                        title = "trPurchaseInvoiceAlertTilte",
                                                        ncontent = "trPurchaseInvoiceAlertContent",
                                                        msgType = "alert",
                                                        createUserId = MainWindow.userLogin.userId,
                                                        updateUserId = MainWindow.userLogin.userId,
                                                    };
                                                    await not.save(not, (int)cb_branch.SelectedValue, "storageAlerts_ctreatePurchaseInvoice", MainWindow.branchLogin.name);
                                                }
                                                #endregion
                                            }

                                            #region save payments
                                            await invoice.recordPosCashTransfer(invoice, "pi");
                                            if (cb_paymentProcessType.SelectedValue.ToString() == "multiple")
                                            {
                                                foreach (var item in listPayments)
                                                {
                                                    // if is card
                                                    tb_processNum.Text = item.docNum;

                                                    item.transType = "p"; //pull
                                                    item.posId = MainWindow.posLogin.posId;
                                                    item.agentId = invoice.agentId;
                                                    item.invId = invoice.invoiceId;
                                                    item.transNum = await cashTransfer.generateCashNumber("pv");
                                                    item.side = "v"; // vendor
                                                    item.createUserId = MainWindow.userLogin.userId;
                                                    await saveConfiguredCashTrans(item);
                                                }
                                            }
                                            else
                                            {
                                                CashTransfer cashTrasnfer = new CashTransfer();
                                                cashTrasnfer.transType = "p"; //pull
                                                cashTrasnfer.posId = MainWindow.posLogin.posId;
                                                cashTrasnfer.agentId = invoice.agentId;
                                                cashTrasnfer.invId = invoice.invoiceId;
                                                cashTrasnfer.transNum = await cashTrasnfer.generateCashNumber("pv");
                                                cashTrasnfer.cash = invoice.totalNet;
                                                cashTrasnfer.side = "v"; // vendor
                                                cashTrasnfer.processType = cb_paymentProcessType.SelectedValue.ToString();
                                                cashTrasnfer.createUserId = MainWindow.userLogin.userId;
                                                await saveCashTransfer(cashTrasnfer);
                                            }


                                            #endregion
                                            clearInvoice();
                                            _InvoiceType = "pd";
                                        }

                                        #region print
                                        prInvoice = await FillCombo.invoice.GetByInvoiceId(prInvoiceId);
                                        ///////////////////////////////////////

                                        if (prInvoice.invType == "pw" || prInvoice.invType == "p")
                                        {
                                            Thread t = new Thread(() =>
                                            {
                                                if (AppSettings.print_on_save_pur == "1")
                                                {
                                                    printPurInvoice();
                                                }
                                                if (AppSettings.email_on_save_pur == "1")
                                                {
                                                    sendPurEmail();
                                                }
                                            });
                                            t.Start();

                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                refreshDraftNotification();
                                refreshInvNotification();

                                
                                await MainWindow.refreshBalance();
                            }
                        }
                        else
                        {
                            // validate card
                            if (p_error_card.Visibility == Visibility.Visible)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectCreditCard"), animation: ToasterAnimation.FadeIn);
                            }
                            HelpClass.validateEmptyCombo(cb_vendor, p_error_vendor);
                            if (p_error_vendor.Visibility == Visibility.Visible || p_error_invoiceNumber.Visibility == Visibility.Visible || p_error_desrvedDate.Visibility == Visibility.Visible)
                            {
                                exp_vendor.IsExpanded = true;
                            }
                        }

                    }
                    else //box is closed
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBoxIsClosed"), animation: ToasterAnimation.FadeIn);
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
        private async Task saveConfiguredCashTrans(CashTransfer cashTransfer)
        {
            switch (cashTransfer.processType)
            {
                case "cash":// cash: update pos balance  
                    if (MainWindow.posLogin.balance > 0)
                    {
                        if (MainWindow.posLogin.balance >= cashTransfer.cash)
                        {
                            MainWindow.posLogin.balance -= cashTransfer.cash;
                            invoice.paid = cashTransfer.cash;
                            invoice.deserved -= cashTransfer.cash;
                        }
                        else
                        {
                            invoice.paid = MainWindow.posLogin.balance;
                            cashTransfer.cash = MainWindow.posLogin.balance;
                            invoice.deserved -= MainWindow.posLogin.balance;
                            MainWindow.posLogin.balance = 0;
                        }
                        await MainWindow.posLogin.save(MainWindow.posLogin);
                        await cashTransfer.Save(cashTransfer); //add cash transfer  
                        await invoice.saveInvoice(invoice);
                    }
                    break;
                case "balance":// balance: update customer balance
                    await invoice.recordConfiguredAgentCash(invoice, "pi", cashTransfer);
                    //invoice.paid += cashTransfer.cash;
                    //invoice.deserved -= cashTransfer.cash;
                    await invoice.saveInvoice(invoice);
                    break;
                case "card": // card 
                    cashTransfer.docNum = tb_processNum.Text;
                    //cashTransfer.cardId = _SelectedCard;
                    await cashTransfer.Save(cashTransfer); //add cash transfer 
                    invoice.paid += cashTransfer.cash;
                    invoice.deserved -= cashTransfer.cash;
                    await invoice.saveInvoice(invoice);
                    break;
            }
        }
        private async Task saveCashTransfer(CashTransfer cashTransfer)
        {
            switch (cb_paymentProcessType.SelectedValue)
            {
                case "cash":// cash: update pos balance
                    if (MainWindow.posLogin.balance > 0)
                    {
                        if (MainWindow.posLogin.balance >= invoice.totalNet)
                        {
                            MainWindow.posLogin.balance -= invoice.totalNet;
                            invoice.paid = cashTransfer.cash = invoice.totalNet;
                            invoice.deserved = 0;
                        }
                        else
                        {
                            invoice.paid = cashTransfer.cash = MainWindow.posLogin.balance;
                            invoice.deserved -= MainWindow.posLogin.balance;
                            MainWindow.posLogin.balance = 0;
                        }
                        await MainWindow.posLogin.save(MainWindow.posLogin);
                        await cashTransfer.Save(cashTransfer); //add cash transfer  
                        await invoice.saveInvoice(invoice);
                    }
                    break;
                case "balance":// balance: update vendor balance                
                    await invoice.recordCashTransfer(invoice, "pi");
                    break;
                case "card":
                    cashTransfer.cash = invoice.totalNet;
                    cashTransfer.cardId = _SelectedCard;
                    cashTransfer.docNum = tb_processNum.Text;
                    await cashTransfer.Save(cashTransfer); //add cash transfer  
                    invoice.paid = invoice.totalNet;
                    invoice.deserved = 0;
                    await invoice.saveInvoice(invoice);
                    break;
            }
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
        private async Task saveBeforeExit()
        {
            if (billDetails.Count > 0 && _InvoiceType == "pd")
            {
                #region Accept
                MainWindow.mainWindow.Opacity = 0.2;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trSaveInvoiceNotification");
                //w.contentText = "Do you want save pay invoice in drafts?";
                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                MainWindow.mainWindow.Opacity = 1;
                #endregion
                if (w.isOk)
                {
                    await addInvoice(_InvoiceType, "pi");
                    clearInvoice();
                    _InvoiceType = "pd";
                }
            }
            clearInvoice();

        }
        private async Task saveBeforeTransfer()
        {
            if (billDetails.Count > 0 && _InvoiceType == "pd")
            {
                #region Accept
                MainWindow.mainWindow.Opacity = 0.2;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trSaveInvoiceNotification");
                //w.contentText = "Do you want save pay invoice in drafts?";
                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                MainWindow.mainWindow.Opacity = 1;
                #endregion
                if (w.isOk)
                {
                    await addInvoice(_InvoiceType, "pi");
                    clearInvoice();
                    _InvoiceType = "pd";
                    setNotifications();
                }
            }
            else
                clearInvoice();

        }
        #endregion
        #region events
        private void Tb_barcode_TextChanged(object sender, TextChangedEventArgs e)
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
        private void Cb_vendor_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                cb_vendor.ItemsSource =FillCombo.vendorsList.Where(x => x.name.Contains(cb_vendor.Text));

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
                if (cb_vendor.IsFocused || _InvoiceType == "pd")
                {

                    TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                    if (elapsed.TotalMilliseconds > 100 && cb_vendor.SelectedIndex != -1)
                    {
                        _SelectedVendor = (int)cb_vendor.SelectedValue;
                        var v = FillCombo.vendorsList.Where(x => x.agentId == _SelectedVendor).FirstOrDefault();
                        if (v.payType != null)
                        {
                            cb_paymentProcessType.SelectedValue = v.payType;
                            Animations.shakingControl(cb_paymentProcessType);
                            Animations.shakingControl(txt_payment);
                        }
                        //else
                        //    cb_paymentProcessType.SelectedIndex = 0;

                    }
                    else
                    {
                        cb_vendor.SelectedValue = _SelectedVendor;
                    }

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
        private void Cb_typeDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 100 && cb_typeDiscount.SelectedIndex != -1)
                {
                    _SelectedDiscountType = (int)cb_typeDiscount.SelectedValue;
                    refreshTotalValue();
                }
                else
                {
                    cb_typeDiscount.SelectedValue = _SelectedDiscountType;
                }


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void tb_discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var txb = sender as TextBox;
                if ((sender as TextBox).Name == "tb_discount")
                    HelpClass.InputJustNumber(ref txb);
                if ((sender as TextBox).Name == "tb_taxValue")
                    HelpClass.InputJustNumber(ref txb);
                _Sender = sender;
                refreshTotalValue();
                e.Handled = true;


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Dp_date_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                _Sender = sender;
                moveControlToBarcode(sender, e);
                
               HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
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
        private void Cb_paymentProcessType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 100 && cb_paymentProcessType.SelectedIndex != -1)
                {
                    _SelectedPaymentType = cb_paymentProcessType.SelectedValue.ToString();
                }
                else
                {
                    cb_paymentProcessType.SelectedValue = _SelectedPaymentType;
                }
                
                switch (cb_paymentProcessType.SelectedIndex)
                {
                    case 0://cash
                        gd_card.Visibility = Visibility.Collapsed;
                        p_error_card.Visibility = Visibility.Collapsed;
                        tb_processNum.Clear();
                        _SelectedCard = -1;
                        txt_card.Text = "";
                        brd_processNum.Visibility = Visibility.Collapsed;
                        dp_desrvedDate.IsEnabled = false;
                        // update validate list
                        requiredControlList = new List<string> { };
                        break;
                    case 1:// balance
                        gd_card.Visibility = Visibility.Collapsed;
                        p_error_card.Visibility = Visibility.Collapsed;
                        dp_desrvedDate.IsEnabled = true;
                        tb_processNum.Clear();
                        _SelectedCard = -1;
                        // update validate list
                        requiredControlList = new List<string> {"vendor", "invoiceNumber",   "desrvedDate" };
                        break;
                    case 2://card
                        dp_desrvedDate.IsEnabled = false;
                        gd_card.Visibility = Visibility.Visible;
                        // update validate list
                        //requiredControlList = new List<string> {"card", "processNum" };
                        requiredControlList = new List<string> {"card"};
                        break;
                    case 3://multiple
                        gd_card.Visibility = Visibility.Collapsed;
                        p_error_card.Visibility = Visibility.Collapsed;
                        tb_processNum.Clear();
                        _SelectedCard = -1;
                        txt_card.Text = "";
                        brd_processNum.Visibility = Visibility.Collapsed;
                        dp_desrvedDate.IsEnabled = true;
                        // update validate list
                        requiredControlList = new List<string> { };
                        break;

                }
                HelpClass.validate(requiredControlList, this);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void PreventSpaces(object sender, KeyEventArgs e)
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
        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime desrveDate;
                if (dp_desrvedDate.SelectedDate != null )
                {
                    desrveDate = (DateTime)dp_desrvedDate.SelectedDate.Value.Date;
                    if (desrveDate.Date < DateTime.Now.Date && dp_desrvedDate.IsFocused)
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorInvDateAfterDeserveToolTip"), animation: ToasterAnimation.FadeIn);
                        dp_desrvedDate.Text = "";
                    }
                }

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Tb_EnglishDigit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only english and digits
            Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
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
        private void clearNavigation()
        {
            _Sum = 0;

            txt_invNumber.Text = "";
            _SequenceNum = 0;
            _SelectedBranch = -1;
            _SelectedVendor = -1;
            invoice = new Invoice();
            tb_barcode.Clear();
            cb_branch.SelectedIndex = -1;
            cb_vendor.SelectedIndex = -1;
            cb_vendor.SelectedItem = "";
            cb_typeDiscount.SelectedIndex = 0;
            dp_desrvedDate.Text = "";
            txt_vendorIvoiceDetails.Text = "";
            tb_invoiceNumber.Clear();
            dp_invoiceDate.Text = "";
            tb_notes.Clear();
            tb_discount.Clear();
            tb_taxValue.Clear();
            billDetails.Clear();
            tb_processNum.Clear();
            cb_paymentProcessType.SelectedIndex = 0;
            cb_paymentProcessType.IsEnabled = true;
            gd_card.Visibility = Visibility.Collapsed;
            tb_total.Text = "0";
            tb_sum.Text = "0";
            if (AppSettings.tax != 0)
                tb_taxValue.Text = HelpClass.PercentageDecTostring(AppSettings.invoiceTax_decimal);
            else
                tb_taxValue.Text = "0";

            isFromReport = false;
            archived = false;
            md_docImage.Badge = "";
            md_payments.Badge = "";

            TextBox tbStartDate = (TextBox)dp_desrvedDate.Template.FindName("PART_TextBox", dp_desrvedDate);
            refrishBillDetails();
        }
        private async Task navigateInvoice(int index)
        {
            try
            {
                clearNavigation();
                invoice = FillCombo.invoices[index];
                _InvoiceType = invoice.invType;
                _invoiceId = invoice.invoiceId;
                navigateBtnActivate();
                await fillInvoiceInputs(invoice);

                #region title text
                if (_InvoiceType == "pw" || _InvoiceType == "p")
                    txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseInvoice");
                else if (_InvoiceType == "pb" || _InvoiceType == "pbw")
                    txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trReturnedInvoice");
                else if (_InvoiceType == "pd")
                    txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trDraftPurchaseBill");
                else if (_InvoiceType == "pbd")
                    txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trDraftBounceBill");

                #endregion
                if (_InvoiceType == "pw" || _InvoiceType == "p" || _InvoiceType == "pb" || _InvoiceType == "pbw")
                    refreshPaymentsNotification(invoice.invoiceId);
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
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
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
                await saveBeforeTransfer();
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
            dg_billDetails.Columns[5].IsReadOnly = false; //make price read only
            dg_billDetails.Columns[4].IsReadOnly = false; //make quantity read only
            dg_billDetails.Columns[3].IsReadOnly = false; //make quantity read only
        }
        #endregion
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
        #region  DataGrid
        private void Cbm_unitItemDetails_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                //billDetails
                var cmb = sender as ComboBox;
                cmb.SelectedValue = (int)billDetails[0].itemUnitId;

                if (_InvoiceType == "p" || _InvoiceType == "pw"
                        || _InvoiceType == "pb" || _InvoiceType == "pbw")
                    cmb.IsEnabled = false;
                else
                    cmb.IsEnabled = true;
            }
            catch (Exception ex)
            {
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
                    billDetails[dg_billDetails.SelectedIndex].itemUnitId = (int)cmb.SelectedValue;
                    TextBlock tb;

                    int _datagridSelectedIndex = dg_billDetails.SelectedIndex;
                    int itemUnitId = (int)cmb.SelectedValue;
                    int itemId = billDetails[_datagridSelectedIndex].itemId;
                    billDetails[_datagridSelectedIndex].itemUnitId = (int)cmb.SelectedValue;
                    #region Dina

                    dynamic unit;
                    unit = FillCombo.itemUnitList.Where(x => x.itemUnitId == (int)cmb.SelectedValue);
                   
                    int oldCount = 0;
                    long newCount = 0;
                    decimal oldPrice = 0;
                    decimal itemTax = 0;

                    decimal newPrice = 0;
                    oldCount = billDetails[_datagridSelectedIndex].Count;
                    oldPrice = billDetails[_datagridSelectedIndex].Price;
                    newCount = oldCount;
                    #region if return invoice
                    if (_InvoiceType == "pbd")
                    {
                        tb = dg_billDetails.Columns[4].GetCellContent(dg_billDetails.Items[_datagridSelectedIndex]) as TextBlock;

                        var itemUnitsIds = FillCombo.itemUnitList.Where(x => x.itemId == itemId).Select(x => x.itemUnitId).ToList();

                        #region caculate available amount in this bil
                        int availableAmountInBranch = await FillCombo.itemLocation.getAmountInBranch(itemUnitId, MainWindow.branchLogin.branchId);
                        int amountInBill = await getAmountInBill(itemId, itemUnitId, billDetails[_datagridSelectedIndex].ID);
                        int availableAmount = availableAmountInBranch - amountInBill;
                        #endregion
                        #region calculate amount in purchase invoice
                        var items = mainInvoiceItems.ToList().Where(i => itemUnitsIds.Contains((int)i.itemUnitId));
                        int purchasedAmount = 0;
                        foreach (ItemTransfer it in items)
                        {
                            if (itemUnitId == (int)it.itemUnitId)
                                purchasedAmount += (int)it.quantity;
                            else
                                purchasedAmount += await FillCombo.itemUnit.fromUnitToUnitQuantity((int)it.quantity, itemId, (int)it.itemUnitId, itemUnitId);
                        }
                        #endregion
                        if (newCount > (purchasedAmount - amountInBill) || newCount > availableAmount)
                        {
                            tb.Text = (purchasedAmount - amountInBill) > availableAmount ? availableAmount.ToString() : (purchasedAmount - amountInBill).ToString();

                            newCount = (purchasedAmount - amountInBill) > availableAmount ? availableAmount : (purchasedAmount - amountInBill);
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    #endregion


                    // newPrice = unit.cost;
                    newPrice = oldPrice;

                    tb = dg_billDetails.Columns[5].GetCellContent(dg_billDetails.Items[_datagridSelectedIndex]) as TextBlock;
                    tb.Text = newPrice.ToString();

                    // old total for changed item
                    decimal total = oldPrice * oldCount;
                    _Sum -= total;


                    // new total for changed item
                    total = newCount * newPrice;
                    _Sum += total;

                    #region items tax
                    if (item.taxes != null)
                        itemTax = (decimal)item.taxes;
                    #endregion

                    refreshTotalValue();

                    // update item in billdetails           
                    billDetails[_datagridSelectedIndex].Count = (int)newCount;
                    billDetails[_datagridSelectedIndex].Price = newPrice;
                    billDetails[_datagridSelectedIndex].Total = total;
                    refrishBillDetails();
                    #endregion
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

                                if (_InvoiceType == "p" || _InvoiceType == "pw" || _InvoiceType == "pb" || _InvoiceType == "pbw")
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
            int column = dg_billDetails.CurrentCell.Column.DisplayIndex;
            if (_InvoiceType == "p" || _InvoiceType == "pw"
                || _InvoiceType == "pb" || _InvoiceType == "pbw"
                || (_InvoiceType == "pbd" && column == 3))
                e.Cancel = true;
        }
        private async void Dg_billDetails_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
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
                    else if (columnName == AppSettings.resourcemanager.GetString("trPrice"))
                        t.Text = HelpClass.DecTostring(billDetails[index].Price);

                }
                else
                {
                    int oldCount = 0;
                    long newCount = 0;
                    decimal oldPrice = 0;
                    decimal newPrice = 0;

                    //"tb_amont"
                    if (columnName == AppSettings.resourcemanager.GetString("trQTR"))
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
                    else
                        newCount = row.Count;

                    oldCount = row.Count;
                    oldPrice = row.Price;
                    //if (_InvoiceType == "pbd" || _InvoiceType == "pbw")
                    //{
                    //    ItemTransfer item = mainInvoiceItems.ToList().Find(i => i.itemUnitId == row.itemUnitId);
                    //    if (newCount > item.quantity)
                    //    {
                    //        // return old value 
                    //        t.Text = item.quantity.ToString();

                    //        newCount = (long)item.quantity;
                    //        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                    //    }
                    //}
                    #region if return invoice
                    if (_InvoiceType == "pbd" || _InvoiceType == "pbw")
                    {
                        var selectedItemUnitId = row.itemUnitId;

                        var itemUnitsIds = FillCombo.itemUnitList.Where(x => x.itemId == row.itemId).Select(x => x.itemUnitId).ToList();

                        #region caculate available amount in this bil
                        int availableAmountInBranch = await FillCombo.itemLocation.getAmountInBranch(row.itemUnitId, MainWindow.branchLogin.branchId);
                        int amountInBill = await getAmountInBill(row.itemId, row.itemUnitId, row.ID);
                        int availableAmount = availableAmountInBranch - amountInBill;
                        #endregion
                        #region calculate amount in purchase invoice
                        var items = mainInvoiceItems.ToList().Where(i => itemUnitsIds.Contains((int)i.itemUnitId));
                        int purchasedAmount = 0;
                        foreach (ItemTransfer it in items)
                        {
                            if (selectedItemUnitId == (int)it.itemUnitId)
                                purchasedAmount += (int)it.quantity;
                            else
                                purchasedAmount += await FillCombo.itemUnit.fromUnitToUnitQuantity((int)it.quantity, row.itemId, (int)it.itemUnitId, selectedItemUnitId);
                        }
                        #endregion
                        if (newCount > (purchasedAmount - amountInBill) || newCount > availableAmount)
                        {
                            // return old value 
                            t.Text = (purchasedAmount - amountInBill) > availableAmount ? availableAmount.ToString() : (purchasedAmount - amountInBill).ToString();

                            newCount = (purchasedAmount - amountInBill) > availableAmount ? availableAmount : (purchasedAmount - amountInBill);
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    #endregion
                    if (columnName == AppSettings.resourcemanager.GetString("trPrice") && !t.Text.Equals(""))
                        newPrice = decimal.Parse(t.Text);
                    else
                        newPrice = row.Price;

                   

                    // old total for changed item
                    decimal total = oldPrice * oldCount;
                    _Sum -= total;

                    // new total for changed item
                    total = newCount * newPrice;
                    _Sum += total;

                    //refresh total cell
                    TextBlock tb = dg_billDetails.Columns[6].GetCellContent(dg_billDetails.Items[index]) as TextBlock;
                    tb.Text = HelpClass.DecTostring(total);

                    //  refresh sum and total text box
                    refreshTotalValue();

                    // update item in billdetails           
                    billDetails[index].Count = (int)newCount;
                    billDetails[index].Price = newPrice;
                    billDetails[index].Total = total;
                }
                refrishBillDetails();
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
        #region billdetails
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
                    int index = billDetails.IndexOf(billDetails.Where(p => p.itemUnitId == defaultPurUnit.itemUnitId ).FirstOrDefault());
                    if (index == -1)//item doesn't exist in bill
                    {
                        // create new row in bill details data grid
                        addRowToBill(item.name, itemId, defaultPurUnit.unitName, defaultPurUnit.itemUnitId, 1, 0, 0);
                    }
                    else // item exist prevoiusly in list
                    {
                        billDetails[index].Count++;
                        billDetails[index].Total = billDetails[index].Count * billDetails[index].Price;

                        _Sum += billDetails[index].Price;
                    }
                }
                else
                {
                    addRowToBill(item.name, itemId, null, 0, 1, 0, 0);
                }

            }
        }
        bool firstTimeForDatagrid = true;
        async void refrishBillDetails()
        {

            dg_billDetails.ItemsSource = null;
            dg_billDetails.ItemsSource = billDetails;
            if (firstTimeForDatagrid)
            {
                HelpClass.StartAwait(grid_main);
                dg_billDetails.IsEnabled = false;
                await Task.Delay(1000);
                dg_billDetails.Items.Refresh();
                if(dg_billDetails.Items.Count>0)
                firstTimeForDatagrid = false;
                dg_billDetails.IsEnabled = true;
                HelpClass.EndAwait(grid_main);
            }
            DataGrid_CollectionChanged(dg_billDetails, null);
            if (_Sum != 0)
                tb_sum.Text = HelpClass.DecTostring(_Sum);
            else
                tb_sum.Text = "0";

        }
        //void refrishDataGridItems()
        //{
        //    dg_billDetails.ItemsSource = null;
        //    dg_billDetails.ItemsSource = billDetails;
        //    dg_billDetails.Items.Refresh();
        //    DataGrid_CollectionChanged(dg_billDetails, null);

        //}
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
                    
                    if (_Sender != null)
                    {
                        DatePicker dt = _Sender as DatePicker;
                        TextBox tb = _Sender as TextBox;
                        if (dt != null)
                        {
                            if (dt.Name == "dp_desrvedDate" || dt.Name == "dp_invoiceDate")
                            {
                                string br = "";
                                for(int i = 0; i< _BarcodeStr.Length; i++)
                                {
                                    br += _BarcodeStr[i];
                                    i++;
                                }
                                _BarcodeStr = br;
                            }
                        }
                        else if (tb != null)
                        {
                            if (tb.Name == "tb_invoiceNumber" || tb.Name == "tb_notes" || tb.Name == "tb_discount")// remove barcode from text box
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
                    await dealWithBarcode(_BarcodeStr);
                    tb_barcode.Text = _BarcodeStr;
                    _BarcodeStr = "";
                    _IsFocused = false;
                    e.Handled = true;
                    cb_branch.SelectedValue = _SelectedBranch;
                }
                _Sender = null;
                tb_barcode.Clear();
                if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
                {
                    switch (e.Key)
                    {
                        case Key.P:
                            //handle P key
                            btn_printInvoice_Click(null, null);
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
                case "pi":// this barcode for invoice               
                    Btn_newDraft_Click(null, null);
                    invoice = await FillCombo.invoice.GetInvoicesByNum(barcode);
                    _InvoiceType = invoice.invType;
                    if (_InvoiceType.Equals("pd") || _InvoiceType.Equals("p") || _InvoiceType.Equals("pbd") || _InvoiceType.Equals("pb"))
                    {
                        // set title to bill
                        if (_InvoiceType == "pd")
                        {
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trDraftPurchaseBill");
                            txt_payInvoice.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        }
                        else if (_InvoiceType == "p")
                        {
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseBill");
                            txt_payInvoice.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        }
                        else if (_InvoiceType == "pbd")
                        {
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trDraftBounceBill");
                            txt_payInvoice.Foreground = Application.Current.Resources["MainColorRed"] as SolidColorBrush;
                        }
                        else if (_InvoiceType == "pb")
                        {
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trReturnedInvoice");
                            txt_payInvoice.Foreground = Application.Current.Resources["MainColorRed"] as SolidColorBrush;
                        }

                        await fillInvoiceInputs(invoice);
                    }
                    break;

                default: // if barcode for item
                         // get item matches barcode
                    if (FillCombo.itemUnitList != null)
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
                                    decimal price = 0; //?????
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
            _Sum += total;
        }
        private void refreshTotalValue()
        {
            #region discount
            decimal discountValue = 0;
            if (tb_discount.Text != "." && !tb_discount.Text.Equals(""))
                discountValue = decimal.Parse(tb_discount.Text);

            if (cb_typeDiscount.SelectedIndex != -1 && int.Parse(cb_typeDiscount.SelectedValue.ToString()) == 2) // discount type is rate
            {
                discountValue = HelpClass.calcPercentage(_Sum, discountValue);
            }

            if (discountValue != 0)
                tb_totalDescount.Text = HelpClass.PercentageDecTostring(discountValue);
            else
                tb_totalDescount.Text = "0";
            #endregion
            decimal total = _Sum - discountValue;

            #region tax
            decimal taxValue = 0;
            decimal taxInputVal = 0;
            if (!tb_taxValue.Text.Equals(""))
                taxInputVal = decimal.Parse(tb_taxValue.Text);
            if (total != 0)
                taxValue = HelpClass.calcPercentage(total, taxInputVal);
            #endregion
            total = total + taxValue;

            if (_Sum != 0)
                tb_sum.Text = HelpClass.DecTostring(_Sum);
            else
                tb_sum.Text = "0";

           

            if (total < 0)
                total = 0;
            if (total != 0)
                tb_total.Text = HelpClass.DecTostring(total);
            else tb_total.Text = "0";

        }
        public async Task fillInvoiceInputs(Invoice invoice)
        {
            #region payment process
            if (_InvoiceType == "p" || _InvoiceType == "pw" || _InvoiceType == "pbw" || _InvoiceType == "pb")
            {
                //payments
                var cashTransfers = await cashTransfer.GetListByInvId(invoice.invoiceId);
                if (cashTransfers.Count == 0)
                {
                    cb_paymentProcessType.SelectedValue = "balance";

                }
                else if (cashTransfers.Count == 1)
                {
                    cb_paymentProcessType.SelectedValue = cashTransfers[0].processType;
                    try
                    {
                        _SelectedCard = (int)cashTransfers[0].cardId;
                        tb_processNum.Text = cashTransfers[0].docNum;

                        if ((int)cashTransfers[0].cardId != 0)
                        {
                            var card = FillCombo.cardsList.Where(x => x.cardId == (int)cashTransfers[0].cardId).FirstOrDefault();
                            txt_card.Text = card.name;
                            if (card.hasProcessNum)
                            {
                                brd_processNum.Visibility = Visibility.Visible;
                                tb_processNum.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                brd_processNum.Visibility = Visibility.Collapsed;
                                tb_processNum.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                    catch { }
                }
                else
                    cb_paymentProcessType.SelectedValue = "multiple";

            }
            #endregion
            #region tax
            if (_InvoiceType == "p" || _InvoiceType == "pw" || _InvoiceType == "pd")
            {
                if (invoice.tax != null)
                    tb_taxValue.Text = HelpClass.PercentageDecTostring(invoice.tax);
                else
                    tb_taxValue.Text = "0";
            }
            else if (_InvoiceType == "pbw" || _InvoiceType == "pb" || _InvoiceType == "pbd")
            {
                tb_taxValue.Text = "0";
            }
            #endregion
            _Sum = (decimal)invoice.total;
            txt_invNumber.Text = invoice.invNumber.ToString();
            cb_branch.SelectedValue = invoice.branchId;
            cb_vendor.SelectedValue = invoice.agentId;
            dp_desrvedDate.Text = invoice.deservedDate.ToString();
            tb_invoiceNumber.Text = invoice.vendorInvNum;
            dp_invoiceDate.Text = invoice.vendorInvDate.ToString();
            if (invoice.totalNet != 0)
                tb_total.Text = HelpClass.DecTostring(invoice.totalNet);
            else tb_total.Text = "0";

            if ((invoice.tax != 0) && (invoice.tax != null))
                tb_taxValue.Text = HelpClass.PercentageDecTostring(invoice.tax);
            else
                tb_taxValue.Text = "0";
            tb_notes.Text = invoice.notes;

            if (invoice.total != 0)
                tb_sum.Text = HelpClass.DecTostring(invoice.total);
            else tb_sum.Text = "0";

            if ((invoice.discountValue != 0) && (invoice.discountValue != null))
                tb_discount.Text = HelpClass.PercentageDecTostring(invoice.discountValue);
            else
                tb_discount.Text = "0";
            if (invoice.discountType == "1")
                cb_typeDiscount.SelectedIndex = 1;
            else if (invoice.discountType == "2")
                cb_typeDiscount.SelectedIndex = 2;
            else
                cb_typeDiscount.SelectedIndex = 0;

            // build invoice details grid
            await buildInvoiceDetails();
            refreshTotalValue();
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
                    Price = decimal.Parse(HelpClass.DecTostring((decimal)itemT.price)),
                    Total = total,
                    OrderId = orderId,
                });
            }

            tb_barcode.Focus();

            refrishBillDetails();
        }
        private void inputEditable()
        {
            if (_InvoiceType == "pbw") // purchase invoice
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete column visible
                dg_billDetails.Columns[5].IsReadOnly = false; //make price read only
                dg_billDetails.Columns[3].IsReadOnly = false; //make unit read only
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                cb_vendor.IsEnabled = false;
                dp_desrvedDate.IsEnabled = false;
                dp_invoiceDate.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                cb_branch.IsEnabled = false;
                tb_discount.IsEnabled = false;
                cb_typeDiscount.IsEnabled = false;
                cb_paymentProcessType.IsEnabled = false;
                tb_processNum.IsEnabled = false;
                dkp_cards.IsEnabled = false;
                btn_save.IsEnabled = true;
                tb_invoiceNumber.IsEnabled = false;
                tb_taxValue.IsEnabled = false;

            }
            else if (_InvoiceType == "pbd") // return invoice
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete column visible
                dg_billDetails.Columns[5].IsReadOnly = false; //make price read only
                dg_billDetails.Columns[3].IsReadOnly = true; //make unit read only
                dg_billDetails.Columns[4].IsReadOnly = false; //make count read only
                cb_vendor.IsEnabled = false;
                dp_desrvedDate.IsEnabled = false;
                dp_invoiceDate.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                cb_branch.IsEnabled = true;
                tb_discount.IsEnabled = false;
                cb_typeDiscount.IsEnabled = false;
                btn_save.IsEnabled = true;
                tb_invoiceNumber.IsEnabled = false;
                tb_taxValue.IsEnabled = false;
                btn_items.IsEnabled = false;
                cb_paymentProcessType.IsEnabled = true;
                tb_processNum.IsEnabled = true;
                dkp_cards.IsEnabled = true;
                btn_clear.IsEnabled = true;
                btn_updateVendor.IsEnabled = true;
                btn_addVendor.IsEnabled = true;
            }
            else if (_InvoiceType == "pd") // purchase draft 
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete column visible
                dg_billDetails.Columns[5].IsReadOnly = false;
                dg_billDetails.Columns[3].IsReadOnly = false;
                dg_billDetails.Columns[4].IsReadOnly = false;
                cb_vendor.IsEnabled = true;
                dp_desrvedDate.IsEnabled = true;
                dp_invoiceDate.IsEnabled = true;
                tb_barcode.IsEnabled = true;
                cb_branch.IsEnabled = true;
                tb_discount.IsEnabled = true;
                cb_typeDiscount.IsEnabled = true;
                btn_save.IsEnabled = true;
                tb_invoiceNumber.IsEnabled = true;
                tb_taxValue.IsEnabled = true;
                btn_items.IsEnabled = true;
                cb_paymentProcessType.IsEnabled = true;
                dkp_cards.IsEnabled = true;
                tb_processNum.IsEnabled = true;
                btn_clear.IsEnabled = true;
                btn_updateVendor.IsEnabled = true;
                btn_addVendor.IsEnabled = true;
            }
            else if (_InvoiceType == "po") //  purchase order
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Visible; //make delete column visible
                dg_billDetails.Columns[5].IsReadOnly = false;
                dg_billDetails.Columns[3].IsReadOnly = false;
                dg_billDetails.Columns[4].IsReadOnly = false;
                cb_vendor.IsEnabled = false;
                dp_desrvedDate.IsEnabled = true;
                dp_invoiceDate.IsEnabled = true;
                tb_barcode.IsEnabled = true;
                cb_branch.IsEnabled = true;
                tb_discount.IsEnabled = true;
                cb_typeDiscount.IsEnabled = true;
                btn_save.IsEnabled = true;
                tb_invoiceNumber.IsEnabled = true;
                tb_taxValue.IsEnabled = true;
                btn_items.IsEnabled = true;
                cb_paymentProcessType.IsEnabled = true;
                dkp_cards.IsEnabled = true;
                tb_processNum.IsEnabled = true;
                btn_clear.IsEnabled = false;
                btn_updateVendor.IsEnabled = false;
                btn_addVendor.IsEnabled = false;
            }
            else if (_InvoiceType == "pw" || _InvoiceType == "p" || _InvoiceType == "pb" || archived)
            {
                dg_billDetails.Columns[0].Visibility = Visibility.Collapsed; //make delete column unvisible
                dg_billDetails.Columns[5].IsReadOnly = true; //make price read only
                dg_billDetails.Columns[3].IsReadOnly = true; //make unit read only
                dg_billDetails.Columns[4].IsReadOnly = true; //make count read only
                cb_vendor.IsEnabled = false;
                dp_desrvedDate.IsEnabled = false;
                dp_invoiceDate.IsEnabled = false;
                tb_barcode.IsEnabled = false;
                cb_branch.IsEnabled = false;
                tb_discount.IsEnabled = false;
                cb_typeDiscount.IsEnabled = false;
                btn_save.IsEnabled = false;
                tb_invoiceNumber.IsEnabled = false;
                tb_taxValue.IsEnabled = false;
                btn_items.IsEnabled = false;
                cb_paymentProcessType.IsEnabled = false;
                dkp_cards.IsEnabled = false;
                tb_processNum.IsEnabled = false;
                btn_clear.IsEnabled = false;
                btn_updateVendor.IsEnabled = false;
                btn_addVendor.IsEnabled = false;
            }

            if (_InvoiceType.Equals("pb") || _InvoiceType.Equals("p"))
            {
                #region print - pdf - send email
                btn_printInvoice.Visibility = Visibility.Visible;
                btn_pdf.Visibility = Visibility.Visible;
                if (FillCombo.groupObject.HasPermissionAction(printCountPermission, FillCombo.groupObjects, "one"))
                {
                    btn_printCount.Visibility = Visibility.Visible;
                    bdr_printCount.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_printCount.Visibility = Visibility.Collapsed;
                    bdr_printCount.Visibility = Visibility.Collapsed;
                }
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
                btn_printCount.Visibility = Visibility.Collapsed;
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
        private void clearInvoice()
        {
            _Sum = 0;
            txt_invNumber.Text = "";
            _SequenceNum = 0;
            _SelectedBranch = -1;
            _SelectedVendor = -1;
            invoice = new Invoice();
            tb_barcode.Clear();
            cb_branch.SelectedIndex = -1;
            cb_vendor.SelectedIndex = -1;
            cb_vendor.SelectedItem = "";
            cb_typeDiscount.SelectedIndex = 0;
            dp_desrvedDate.Text = "";
            txt_vendorIvoiceDetails.Text = "";
            tb_invoiceNumber.Clear();
            dp_invoiceDate.Text = "";
            tb_notes.Clear();
            tb_discount.Clear();
            tb_taxValue.Clear();
            billDetails.Clear();
            tb_processNum.Clear();
            cb_paymentProcessType.SelectedIndex = 0;
            cb_paymentProcessType.IsEnabled = true;
            gd_card.Visibility = Visibility.Collapsed;
            tb_total.Text = "0";
            tb_sum.Text = "0";
            if (AppSettings.invoiceTax_decimal != 0)
                tb_taxValue.Text = HelpClass.PercentageDecTostring(AppSettings.invoiceTax_decimal);
            else
                tb_taxValue.Text = "0";

            isFromReport = false;
            archived = false;
            md_docImage.Badge = "";
            md_payments.Badge = "";

            TextBox tbStartDate = (TextBox)dp_desrvedDate.Template.FindName("PART_TextBox", dp_desrvedDate);
            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseBill");
            btn_save.Content = AppSettings.resourcemanager.GetString("trBuy");

            refrishBillDetails();
            _InvoiceType = "pd";
            inputEditable();
            btn_next.Visibility = Visibility.Collapsed;
            btn_previous.Visibility = Visibility.Collapsed;


            foreach (var el in cardEllipseList)
            {
                el.Stroke = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            }


            // last 
            requiredControlList = new List<string> { };
            HelpClass.clearValidate(requiredControlList, this);
        }
        private async void clearVendor()
        {
            await FillCombo.FillComboVendors(cb_vendor);
            cb_vendor.SelectedIndex = -1;
            cb_vendor.Text = "";
            dp_desrvedDate.SelectedDate = null;
            dp_desrvedDate.Text = "";
            tb_invoiceNumber.Text = "";
            dp_invoiceDate.SelectedDate = null;
            dp_invoiceDate.Text = "";
            tb_notes.Text = "";
            invoice.agentId = 0;

        }
        #endregion
        #region btn
        private async void Btn_newDraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (billDetails.Count > 0 && (_InvoiceType == "pd" || _InvoiceType == "pbd"))
                {
                    bool valid = validateItemUnits();
                    if (billDetails.Count > 0 && valid)
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
                            await addInvoice(_InvoiceType, "pi");
                            clearInvoice();
                            _InvoiceType = "pd";
                            //refreshDraftNotification();
                        }
                        else
                        {
                            clearInvoice();
                            _InvoiceType = "pd";
                        }
                    }
                }
                else
                    clearInvoice();

                setNotifications();

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
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearVendor();
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_payments_Click(object sender, RoutedEventArgs e)
        {//payments
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (invoice != null && invoice.invoiceId != 0)
                {
                    if (FillCombo.groupObject.HasPermissionAction(paymentsPermission, FillCombo.groupObjects, "one"))
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_cashTransfer w = new wd_cashTransfer();

                        w.invId = invoice.invoiceId;
                        w.invPaid = invoice.paid;
                        w.invTotal = invoice.totalNet;

                        w.title = AppSettings.resourcemanager.GetString("trPayments");
                    // w.ShowInTaskbar = false;
                        w.ShowDialog();

                        Window.GetWindow(this).Opacity = 1;
                    }
                    else
                        Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                }
                
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
                string invoiceType = "pd ,pbd";
                int duration = 1;
                w.invoiceType = invoiceType;
                w.icon = "drafts";
                w.page = "purchases";
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
                        await fillInvoiceInputs(invoice);
                        setNotifications();
                        refreshDocCount(invoice.invoiceId);
                        navigateBtnActivate();
                        md_payments.Badge = "";
                        if (_InvoiceType == "pd")// set title to bill
                        {
                            mainInvoiceItems = invoiceItems;
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trDraftPurchaseBill");
                            txt_payInvoice.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        }
                        if (_InvoiceType == "pbd")
                        {
                            mainInvoiceItems = await FillCombo.invoice.GetInvoicesItems(invoice.invoiceMainId.Value);
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trDraftBounceBill");
                            txt_payInvoice.Foreground = Application.Current.Resources["MainColorRed"] as SolidColorBrush;
                        }
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
        private async void Btn_invoices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                await saveBeforeTransfer();
                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();

                // purchase invoices
                string invoiceType = "p , pw , pb, pbw";
                int duration = 1;
                w.icon = "invoices";
                w.page = "purchases";
                w.invoiceType = invoiceType;
                w.userId = MainWindow.userLogin.userId;
                w.duration = duration; // view drafts which created during 1 last days 

                w.title = AppSettings.resourcemanager.GetString("trInvoices");

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
                        refreshPaymentsNotification(_invoiceId);
                        refreshDocCount(invoice.invoiceId);
                        // set title to bill
                        if (invoice.invType == "p" || invoice.invType == "pw")
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseInvoice");
                        else
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trReturnedInvoice");

                        await fillInvoiceInputs(invoice);
                        navigateBtnActivate();
                    }
                }

                HelpClass.clearValidate(requiredControlList, this);
                Window.GetWindow(this).Opacity = 1;
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
                await saveBeforeTransfer();
                Window.GetWindow(this).Opacity = 0.2;
                wd_invoice w = new wd_invoice();

                // purchase orders
                string invoiceType = "po";
                w.invoiceType = invoiceType;
                w.icon = "orders";
                w.page = "purchases";
                w.condition = "orders";
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
                        md_payments.Badge = "";
                        setNotifications();
                        refreshDocCount(invoice.invoiceId);

                        // set title to bill
                        txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseOrder");
                        txt_payInvoice.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
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
        private void Btn_invoiceImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

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
        private async void Btn_returnInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(returnPermission, FillCombo.groupObjects, "one"))
                {
                    if (_InvoiceType == "p")
                    {
                        _InvoiceType = "pbd";
                        isFromReport = true;
                        archived = false;
                        await fillInvoiceInputs(invoice);
                        txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trReturnedInvoice");
                        txt_payInvoice.Foreground = Application.Current.Resources["MainColorRed"] as SolidColorBrush;
                        btn_save.Content = AppSettings.resourcemanager.GetString("trReturn");
                        refreshInvNotification();
                    }
                    else
                    {
                        await saveBeforeExit();
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_returnInvoice w = new wd_returnInvoice();
                        w.page = "purchase";
                        w.userId = MainWindow.userLogin.userId;
                        w.invoiceType = "p";
                        if (w.ShowDialog() == true)
                        {
                            _InvoiceType = "pbd";
                            invoice = w.invoice;
                            isFromReport = true;
                            archived = false;
                            await fillInvoiceInputs(invoice);
                            txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trReturnedInvoice");
                            txt_payInvoice.Foreground = Application.Current.Resources["MainColorRed"] as SolidColorBrush;
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
        private async void Btn_addVendor_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_updateVendor w = new wd_updateVendor();
                //// pass agent id to update windows
                w.agent.agentId = 0;
                w.type = "v";
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                if (w.isOk == true)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    await FillCombo.RefreshVendors();
                    await FillCombo.FillComboVendors(cb_vendor);
                }

                HelpClass.EndAwait(grid_main);

            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private async void Btn_updateVendor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cb_vendor.SelectedIndex != -1)
                {
                    HelpClass.StartAwait(grid_main);
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_updateVendor w = new wd_updateVendor();
                    //// pass agent id to update windows
                    w.agent.agentId = (int)cb_vendor.SelectedValue;
                    w.type = "v";
                    w.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    if (w.isOk == true)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        await FillCombo.RefreshVendors();
                        //await FillCombo.FillComboVendors(cb_vendor);
                    }

                    HelpClass.EndAwait(grid_main);

                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        #endregion
        #region report

        // for report
        Invoice invoiceModel = new Invoice();
        Branch branchModel = new Branch();
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        int prInvoiceId;
        Invoice prInvoice = new Invoice();
        List<PayedInvclass> mailpayedList = new List<PayedInvclass>();
        //print
        private async void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(paymentsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    ////////////////////////////
                   // Thread t1 = new Thread(() =>
                  //  {
                        pdfPurInvoice();
                    //});
                    //t1.Start();
                    ////////////////////////////
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

        public async void multiplePaytable(List<ReportParameter> paramarr)
        {
            if ((prInvoice.invType == "p" || prInvoice.invType == "pw" || prInvoice.invType == "pbd" || prInvoice.invType == "pb"))
            {
                CashTransfer cachModel = new CashTransfer();
                List<PayedInvclass> payedList = new List<PayedInvclass>();
                payedList = await cachModel.GetPayedByInvId(prInvoice.invoiceId);
                mailpayedList = payedList;
                decimal sump = payedList.Sum(x => x.cash);
                decimal deservd = (decimal)prInvoice.totalNet - sump;
                paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

                paramarr.Add(new ReportParameter("sumP", reportclass.DecTostring(sump)));
                paramarr.Add(new ReportParameter("deserved", reportclass.DecTostring(deservd)));
                rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", payedList));


            }
        }

        public async void pdfPurInvoice()
        {
            try
            {
                if (invoice.invoiceId > 0)
                {
                    prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);

                    if (int.Parse(AppSettings.Allow_print_inv_count) <= prInvoice.printedcount)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);

                        });
                    }
                    else
                    {

                        if (prInvoice.invType == "pd" || prInvoice.invType == "sd" || prInvoice.invType == "qd"
                  || prInvoice.invType == "sbd" || prInvoice.invType == "pbd"
                  || prInvoice.invType == "ord" || prInvoice.invType == "imd" || prInvoice.invType == "exd")
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPrintDraftInvoice"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {

                            List<ReportParameter> paramarr = new List<ReportParameter>();

                            string reppath = reportclass.GetpayInvoiceRdlcpath(prInvoice);
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
                                foreach (var i in invoiceItems)
                                {
                                    i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                                    i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));
                                }
                                clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                                clsReports.setReportLanguage(paramarr);
                                clsReports.Header(paramarr);
                                paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                                multiplePaytable(paramarr);

                                rep.SetParameters(paramarr);
                                rep.Refresh();

                                saveFileDialog.Filter = "PDF|*.pdf;";
                                bool? savdialog = false;
                                this.Dispatcher.Invoke(() =>
                                {
                                    savdialog = saveFileDialog.ShowDialog();

                                });


                                if (savdialog == true)
                                {

                                    string filepath = saveFileDialog.FileName;

                                    //copy count
                                    if (prInvoice.invType == "s" || prInvoice.invType == "sb" || prInvoice.invType == "p" || prInvoice.invType == "pb")
                                    {

                                        paramarr.Add(new ReportParameter("isOrginal", false.ToString()));



                                        rep.SetParameters(paramarr);

                                        rep.Refresh();

                                        if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                                        {

                                            this.Dispatcher.Invoke(() =>
                                            {
                                                LocalReportExtensions.ExportToPDF(rep, filepath);

                                            });


                                            int res = 0;

                                            res = await invoiceModel.updateprintstat(prInvoice.invoiceId, 1, false, true);



                                            prInvoice.printedcount = prInvoice.printedcount + 1;

                                            prInvoice.isOrginal = false;


                                        }
                                        else
                                        {
                                            this.Dispatcher.Invoke(() =>
                                            {
                                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);

                                            });

                                        }


                                    }
                                    else
                                    {

                                        this.Dispatcher.Invoke(() =>
                                        {

                                            LocalReportExtensions.ExportToPDF(rep, filepath);


                                        });

                                    }
                                    // end copy count

                                    /*
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        saveFileDialog.Filter = "PDF|*.pdf;";

                                        if (saveFileDialog.ShowDialog() == true)
                                        {

                                            string filepath = saveFileDialog.FileName;
                                            LocalReportExtensions.ExportToPDF(rep, filepath);
                                        }
                                    });


                                    */
                                }
                            }

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                });
            }
        }
        public async void previewPurInvoice()
        {
            try
            {



                if (invoice.invoiceId > 0)
                {

                    prInvoice = await invoiceModel.GetByInvoiceId(invoice.invoiceId);

                    if (int.Parse(AppSettings.Allow_print_inv_count) <= prInvoice.printedcount)
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);

                    }
                    else
                    {
                        Window.GetWindow(this).Opacity = 0.2;

                        List<ReportParameter> paramarr = new List<ReportParameter>();
                        string pdfpath;

                        //
                        pdfpath = @"\Thumb\report\temp.pdf";
                        pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
                        string reppath = reportclass.GetpayInvoiceRdlcpath(prInvoice);
                        if (prInvoice.invoiceId > 0)
                        {
                            invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                            if (prInvoice.agentId != null)
                            {
                                Agent agentinv = new Agent();
                                agentinv = await agentinv.getAgentById((int)prInvoice.agentId);
                                // agentinv = vendors.Where(X => X.agentId == prInvoice.agentId).FirstOrDefault();

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
                            foreach (var i in invoiceItems)
                            {
                                i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                                i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));
                                //r.deserveDate = Convert.ToDateTime(HelpClass.DateToString(r.deserveDate));
                            }
                            clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                            clsReports.setReportLanguage(paramarr);
                            clsReports.Header(paramarr);
                            paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                            if ((prInvoice.invType == "p" || prInvoice.invType == "pw" || prInvoice.invType == "pbd" || prInvoice.invType == "pb" || prInvoice.invType == "pd"))
                            {
                                CashTransfer cachModel = new CashTransfer();
                                List<PayedInvclass> payedList = new List<PayedInvclass>();
                                payedList = await cachModel.GetPayedByInvId(prInvoice.invoiceId);
                                decimal sump = payedList.Sum(x => x.cash) ;
                                decimal deservd = (decimal)prInvoice.totalNet - sump;
                                paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

                                paramarr.Add(new ReportParameter("sumP", reportclass.DecTostring(sump)));
                                paramarr.Add(new ReportParameter("deserved", reportclass.DecTostring(deservd)));
                                rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", payedList));


                            }

                            if (prInvoice.invType == "pd" || prInvoice.invType == "sd" || prInvoice.invType == "qd"
          || prInvoice.invType == "sbd" || prInvoice.invType == "pbd"
          || prInvoice.invType == "ord" || prInvoice.invType == "imd" || prInvoice.invType == "exd")
                            {
                                paramarr.Add(new ReportParameter("isOrginal", true.ToString()));
                            }
                            else
                            {
                                paramarr.Add(new ReportParameter("isOrginal", false.ToString()));
                            }
                            rep.SetParameters(paramarr);
                            rep.Refresh();
                            //copy count
                            if (prInvoice.invType == "s" || prInvoice.invType == "sb" || prInvoice.invType == "p" || prInvoice.invType == "pw" || prInvoice.invType == "pw")
                            {

                                // paramarr.Add(new ReportParameter("isOrginal", prInvoice.isOrginal.ToString()));

                                // update paramarr->isOrginal
                                foreach (var item in paramarr.Where(x => x.Name == "isOrginal").ToList())
                                {
                                    StringCollection myCol = new StringCollection();
                                    myCol.Add(prInvoice.isOrginal.ToString());
                                    item.Values = myCol;


                                }
                                //end update


                                rep.SetParameters(paramarr);

                                rep.Refresh();

                                if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                                {

                                    LocalReportExtensions.ExportToPDF(rep, pdfpath);

                                    int res = 0;

                                    res = await invoiceModel.updateprintstat(prInvoice.invoiceId, 1, false, true);



                                    prInvoice.printedcount = prInvoice.printedcount + 1;

                                    prInvoice.isOrginal = false;


                                }
                                else
                                {
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);
                                }


                            }
                            else
                            {

                                LocalReportExtensions.ExportToPDF(rep, pdfpath);

                            }
                            // end copy count

                            //   LocalReportExtensions.ExportToPDF(rep, pdfpath);




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
                            Toaster.ShowWarning(Window.GetWindow(this), message: "", animation: ToasterAnimation.FadeIn);

                        Window.GetWindow(this).Opacity = 1;

                    }
                    //
                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSaveInvoiceToPreview"), animation: ToasterAnimation.FadeIn);

                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                });
            }

        }
        public async void printPurInvoice()
        {
            try
            {


                if (prInvoiceId > 0)
                {
                    prInvoice = new Invoice();
                    prInvoice = await invoiceModel.GetByInvoiceId(prInvoiceId);
                    //
                    if (prInvoice.invType == "pd" || prInvoice.invType == "sd" || prInvoice.invType == "qd"
                                 || prInvoice.invType == "sbd" || prInvoice.invType == "pbd"
                                 || prInvoice.invType == "ord" || prInvoice.invType == "imd" || prInvoice.invType == "exd")
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPrintDraftInvoice"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        List<ReportParameter> paramarr = new List<ReportParameter>();

                        string reppath = reportclass.GetpayInvoiceRdlcpath(prInvoice);
                        if (prInvoice.invoiceId > 0)
                        {
                            invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);


                            // agentinv = vendors.Where(X => X.agentId == prInvoice.agentId).FirstOrDefault();

                            User employ = new User();
                            employ = await employ.getUserById((int)prInvoice.updateUserId);
                            prInvoice.uuserName = employ.name;
                            prInvoice.uuserLast = employ.lastname;

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
                            foreach (var i in invoiceItems)
                            {
                                i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                                i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));
                            }
                            clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                            clsReports.setReportLanguage(paramarr);
                            clsReports.Header(paramarr);
                            paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                            //
                            //   multiplePaytable(paramarr);
                            if ((prInvoice.invType == "p" || prInvoice.invType == "pw" || prInvoice.invType == "pbd" || prInvoice.invType == "pb"))
                            {
                                CashTransfer cachModel = new CashTransfer();
                                List<PayedInvclass> payedList = new List<PayedInvclass>();
                                payedList = await cachModel.GetPayedByInvId(prInvoice.invoiceId);
                                mailpayedList = payedList;
                                decimal sump = payedList.Sum(x => x.cash);
                                decimal deservd = (decimal)prInvoice.totalNet - sump;
                                paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

                                paramarr.Add(new ReportParameter("sumP", reportclass.DecTostring(sump)));
                                paramarr.Add(new ReportParameter("deserved", reportclass.DecTostring(deservd)));
                                rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", payedList));


                            }
                            //
                            rep.SetParameters(paramarr);
                            rep.Refresh();


                            //copy count
                            if (prInvoice.invType == "s" || prInvoice.invType == "sb" || prInvoice.invType == "p" || prInvoice.invType == "pb" || prInvoice.invType == "pw")
                            {

                                paramarr.Add(new ReportParameter("isOrginal", prInvoice.isOrginal.ToString()));

                                for (int i = 1; i <= short.Parse(AppSettings.pur_copy_count); i++)
                                {
                                    if (i > 1)
                                    {
                                        // update paramarr->isOrginal
                                        foreach (var item in paramarr.Where(x => x.Name == "isOrginal").ToList())
                                        {
                                            StringCollection myCol = new StringCollection();
                                            myCol.Add(prInvoice.isOrginal.ToString());
                                            item.Values = myCol;


                                        }
                                        //end update

                                    }
                                    rep.SetParameters(paramarr);

                                    rep.Refresh();

                                    if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                                    {

                                        this.Dispatcher.Invoke(() =>
                                        {


                                            LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.sale_printer_name, 1);



                                        });


                                        int res = 0;
                                        res = await invoiceModel.updateprintstat(prInvoice.invoiceId, 1, false, true);
                                        prInvoice.printedcount = prInvoice.printedcount + 1;

                                        prInvoice.isOrginal = false;


                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);
                                        });

                                    }

                                }
                            }
                            else
                            {

                                this.Dispatcher.Invoke(() =>
                                {
                                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
                                });

                            }
                            // end copy count

                            /*
                            this.Dispatcher.Invoke(() =>
                            {
                                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, MainWindow.rep_printer_name, 1);
                            });
                            */


                        }
                    }

                    //
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                });

            }
        }


        public async void sendPurEmail()
        {
            try
            {
                //
                if (prInvoiceId > 0)
                {
                    prInvoice = new Invoice();
                    prInvoice = await invoiceModel.GetByInvoiceId(prInvoiceId);
                    decimal? discountval = 0;
                    string discounttype = "";
                    discountval = prInvoice.discountValue;
                    discounttype = prInvoice.discountType;

                    if (prInvoice.invType == "pd" || prInvoice.invType == "sd" || prInvoice.invType == "qd"
                    || prInvoice.invType == "sbd" || prInvoice.invType == "pbd"
                    || prInvoice.invType == "ord" || prInvoice.invType == "imd" || prInvoice.invType == "exd")
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trCanNotSendDraftInvoice"), animation: ToasterAnimation.FadeIn);
                        }));
                    }
                    else
                    {
                        if (prInvoice.invType == "pw" || prInvoice.invType == "p")
                        {
                            invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                            SysEmails email = new SysEmails();
                            EmailClass mailtosend = new EmailClass();
                            email = await email.GetByBranchIdandSide((int)MainWindow.branchLogin.branchId, "purchase");
                            if (email == null)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoEmailForThisDept"), animation: ToasterAnimation.FadeIn);
                                }));
                            }
                            else
                            {
                                Agent toAgent = new Agent();

                                if (prInvoice.agentId != null)
                                {

                                    //  agentinv = vendors.Where(X => X.agentId == prInvoice.agentId).FirstOrDefault();
                                    toAgent = await toAgent.getAgentById((int)prInvoice.agentId);
                                    prInvoice.agentCode = toAgent.code;
                                    //new lines
                                    prInvoice.agentName = toAgent.name;
                                    prInvoice.agentCompany = toAgent.company;
                                }
                                else
                                {

                                    prInvoice.agentCode = "-";
                                    //new lines
                                    prInvoice.agentName = "-";
                                    prInvoice.agentCompany = "-";
                                }
                                if (toAgent == null || toAgent.agentId == 0)
                                {
                                    //edit warning message to customer
                                    Dispatcher.Invoke(new Action(() =>
                                    {
                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheVendorHasNoEmail"), animation: ToasterAnimation.FadeIn);
                                    }));
                                }
                                else
                                {
                                    //  int? itemcount = invoiceItems.Count();
                                    if (email.emailId == 0)
                                    {
                                        Dispatcher.Invoke(new Action(() =>
                                        {
                                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoEmailForThisDept"), animation: ToasterAnimation.FadeIn);
                                        }));
                                    }
                                    else
                                    {
                                        if (prInvoice.invoiceId == 0)
                                        {
                                            Dispatcher.Invoke(new Action(() =>
                                            {
                                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trThereIsNoOrderToSen"), animation: ToasterAnimation.FadeIn);
                                            }));
                                        }
                                        else
                                        {
                                            if (invoiceItems == null || invoiceItems.Count() == 0)
                                            {
                                                Dispatcher.Invoke(new Action(() =>
                                                {
                                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trThereIsNoItemsToSend"), animation: ToasterAnimation.FadeIn);
                                                }));
                                            }
                                            else
                                            {

                                                if (toAgent.email.Trim() == "" || toAgent.email.Trim() == null)
                                                {
                                                    Dispatcher.Invoke(new Action(() =>
                                                    {
                                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheVendorHasNoEmail"), animation: ToasterAnimation.FadeIn);
                                                    }));
                                                }

                                                else
                                                {
                                                    SetValues setvmodel = new SetValues();


                                                    List<SetValues> setvlist = new List<SetValues>();
                                                    if (prInvoice.invType == "pw" || prInvoice.invType == "p")
                                                    {
                                                        setvlist = await setvmodel.GetBySetName("pur_email_temp");
                                                    }
                                                    else if (prInvoice.invType == "or" || prInvoice.invType == "ors")
                                                    {
                                                        setvlist = await setvmodel.GetBySetName("sale_order_email_temp");
                                                    }
                                                    foreach (var i in invoiceItems)
                                                    {
                                                        i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                                                        i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));
                                                    }
                                                    string pdfpath = await SavePurpdf();
                                                    prInvoice.discountValue =  discountval.Value;
                                                    prInvoice.discountType = discounttype;
                                                    mailtosend = mailtosend.fillSaleTempData(prInvoice, invoiceItems, mailpayedList, email, toAgent, setvlist);

                                                    //SavePurpdf();
                                                    //string pdfpath = emailpdfpath;
                                                    mailtosend.AddAttachTolist(pdfpath);

                                                    string msg = "";
                                                    this.Dispatcher.Invoke(new Action(() =>
                                                    {
                                                        msg = mailtosend.Sendmail();// temp comment
                                                        if (msg == "Failure sending mail.")
                                                        {
                                                            // msg = "No Internet connection";

                                                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoConnection"), animation: ToasterAnimation.FadeIn);
                                                        }
                                                        else if (msg == "mailsent")
                                                        {
                                                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailSent"), animation: ToasterAnimation.FadeIn);

                                                        }
                                                        else
                                                        {
                                                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailNotSent"), animation: ToasterAnimation.FadeIn);

                                                        }
                                                    }));

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("only purshase invoice");
                        }
                    }
                    //
                }
                else
                {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trThereIsNoItemsToSend"), animation: ToasterAnimation.FadeIn);
                    }));
                }


                //
            }
            catch
            {
                this.Dispatcher.Invoke(() =>
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                });
            }
        }
        //public async void SavePurpdf()

        public async Task<string> SavePurpdf()
        {
            string pdfpath;
            pdfpath = @"\Thumb\report\File.pdf";
            try
            {


                if (prInvoiceId > 0)
                {
                    prInvoice = new Invoice();
                    prInvoice = await invoiceModel.GetByInvoiceId(prInvoiceId);

                    List<ReportParameter> paramarr = new List<ReportParameter>();

                    //

                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
                    string reppath = reportclass.GetpayInvoiceRdlcpath(prInvoice);
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
                        foreach (var i in invoiceItems)
                        {
                            i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                            i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));
                        }
                        clsReports.purchaseInvoiceReport(invoiceItems, rep, reppath);
                        clsReports.setReportLanguage(paramarr);
                        clsReports.Header(paramarr);
                        paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);


                        if ((prInvoice.invType == "p" || prInvoice.invType == "pw" || prInvoice.invType == "pbd" || prInvoice.invType == "pb"))
                        {
                            CashTransfer cachModel = new CashTransfer();
                            List<PayedInvclass> payedList = new List<PayedInvclass>();
                            payedList = await cachModel.GetPayedByInvId(prInvoice.invoiceId);
                            mailpayedList = payedList;
                            decimal sump = payedList.Sum(x => x.cash);
                            decimal deservd = (decimal)prInvoice.totalNet - sump;
                            paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

                            paramarr.Add(new ReportParameter("sumP", reportclass.DecTostring(sump)));
                            paramarr.Add(new ReportParameter("deserved", reportclass.DecTostring(deservd)));
                            rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", payedList));


                        }
                        rep.SetParameters(paramarr);
                        rep.Refresh();
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            LocalReportExtensions.ExportToPDF(rep, pdfpath);
                        }));
                    }
                }
                return pdfpath;
            }
            catch
            {
                this.Dispatcher.Invoke(() =>
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                });
                return pdfpath;
            }
        }
        private void btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(paymentsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    prInvoiceId = invoice.invoiceId;

                    //////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        printPurInvoice();

                    });
                    t1.Start();
                    ////////////////////////////
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
        //
        private async void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(paymentsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    /////////////////////
                    previewPurInvoice();
                    /////////////////////
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
        private void Btn_emailMessage_Click(object sender, RoutedEventArgs e)
        {//email
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(paymentsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    prInvoiceId = invoice.invoiceId;
                    ///////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        sendPurEmail();
                    });
                    t1.Start();
                    ////////////////////////////////////
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
        private async void Btn_printCount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                int result = 0;

                if (invoice.invoiceId > 0)
                {
                    result = await invoiceModel.updateprintstat(invoice.invoiceId, -1, true, true);


                    if (result > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }

                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trChooseInvoiceToolTip"), animation: ToasterAnimation.FadeIn);
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
        #region  InitializeCardsPic
        ImageBrush brush = new ImageBrush();
        List<Ellipse> cardEllipseList = new List<Ellipse>();

        void InitializeCardsPic(IEnumerable<Card> cards)
        {
            #region cardImageLoad
            dkp_cards.Children.Clear();
            int userCount = 0;
            foreach (var item in cards)
            {
                #region Button
                Button button = new Button();
                button.DataContext = item;
                button.Tag = item.cardId;
                button.Padding = new Thickness(0, 0, 0, 0);
                button.Margin = new Thickness(2.5, 5, 2.5, 5);
                button.Background = null;
                button.BorderBrush = null;
                button.Height = 35;
                button.Width = 35;
                button.Click += card_Click;
                #region grid
                Grid grid = new Grid();
                #region 
                Ellipse ellipse = new Ellipse();
                ellipse.StrokeThickness = 1;
                ellipse.Stroke = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                ellipse.Height = 35;
                ellipse.Width = 35;
                ellipse.FlowDirection = FlowDirection.LeftToRight;
                ellipse.ToolTip = item.name;
                ellipse.Tag = item.cardId;
                userImageLoad(ellipse, item.image);
                Grid.SetColumn(ellipse, userCount);
                grid.Children.Add(ellipse);
                cardEllipseList.Add(ellipse);
                #endregion
                #endregion
                button.Content = grid;
                #endregion
                dkp_cards.Children.Add(button);

            }
            #endregion
        }
        void card_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            _SelectedCard = int.Parse(button.Tag.ToString());
            Card card = button.DataContext as Card;
            txt_card.Text = card.name;
            if (card.hasProcessNum)
            {
                brd_processNum.Visibility = Visibility.Visible;
                tb_processNum.Visibility = Visibility.Visible;
                requiredControlList = new List<string> { "card", "processNum" };
            }
            else
            {
                brd_processNum.Visibility = Visibility.Collapsed;
                tb_processNum.Visibility = Visibility.Collapsed;
                requiredControlList = new List<string> { "card" };
            }
            //set border color
            foreach (var el in cardEllipseList)
            {
                if ((int)el.Tag == (int)button.Tag)
                    el.Stroke = Application.Current.Resources["MainColor"] as SolidColorBrush;
                else
                    el.Stroke = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            }

        }
        async void userImageLoad(Ellipse ellipse, string image)
        {
            try
            {
                if (!string.IsNullOrEmpty(image))
                {
                    clearImg(ellipse);

                    byte[] imageBuffer = await FillCombo.card.downloadImage(image); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    ellipse.Fill = new ImageBrush(bitmapImage);
                }
                else
                {
                    clearImg(ellipse);
                }
            }
            catch
            {
                clearImg(ellipse);
            }
        }
        private void clearImg(Ellipse ellipse)
        {
            Uri resourceUri = new Uri("pic/no-image-icon-90x90.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            brush.ImageSource = temp;
            ellipse.Fill = brush;
        }
        #endregion
    }
}
