using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace laundryApp.View.accounts
{
    /// <summary>
    /// Interaction logic for uc_ordersAccounting.xaml
    /// </summary>
    public partial class uc_ordersAccounting : UserControl
    {
        public uc_ordersAccounting()
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
        private static uc_ordersAccounting _instance;
        public static uc_ordersAccounting Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                    _instance = new uc_ordersAccounting();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string createPermission = "ordersAccounting_create";
        string reportsPermission = "ordersAccounting_reports";
        string BranchesPermission = "ordersAccounting_allBranches";
        CashTransfer cashModel = new CashTransfer();
        Invoice invoiceModel = new Invoice();
        Branch branchModel = new Branch();
        CashTransfer cashtrans = new CashTransfer();
        Invoice invoice = new Invoice();

        //Bonds bondModel = new Bonds();
        Card cardModel = new Card();
        Agent agentModel = new Agent();
        User userModel = new User();
        Pos posModel = new Pos();
        IEnumerable<Agent> agents;
        IEnumerable<Agent> customers;
        IEnumerable<User> users;
        IEnumerable<Card> cards;
        //IEnumerable<CashTransfer> cashesQuery;
        //IEnumerable<CashTransfer> cashesQueryExcel;
        IEnumerable<Invoice> invoiceQuery;
        IEnumerable<Invoice> invoiceQueryExcel;
        //IEnumerable<CashTransfer> cashes;
        IEnumerable<Invoice> invoices;
        IEnumerable<Branch> branches;
        int agentId, userId;
        string searchText = "";

        static private int _SelectedCard = -1;

        public static List<string> requiredControlList;
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private void Btn_confirm_Click(object sender, RoutedEventArgs e)
        {//confirm

        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "cash", "paymentProcessType" };

                #region fill branch 
                try
                {
                    await FillCombo.fillBranchesWithAll(cb_branch);
                    cb_branch.SelectedValue = MainWindow.branchLogin.branchId;
                    if (FillCombo.groupObject.HasPermissionAction(BranchesPermission, FillCombo.groupObjects, "one"))
                        cb_branch.IsEnabled = true;
                    else
                        cb_branch.IsEnabled = false;
                }
                catch { }
                #endregion

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                #endregion

                dp_searchEndDate.SelectedDate = DateTime.Now;
                dp_searchStartDate.SelectedDate = DateTime.Now;

                dp_searchStartDate.SelectedDateChanged += this.dp_SelectedStartDateChanged;
                dp_searchEndDate.SelectedDateChanged += this.dp_SelectedEndDateChanged;

                btn_save.IsEnabled = false;

                #region fill agent combo
                //List<Agent> agents = new List<Agent>();
                //List<Agent> customers = new List<Agent>();
                //try
                //{
                //    customers = await agentModel.GetAgentsActive("c");
                //    agents = await agentModel.GetAgentsActive("v");
                //    agents.AddRange(customers);

                //    cb_customer.ItemsSource = customers;
                //    cb_customer.DisplayMemberPath = "name";
                //    cb_customer.SelectedValuePath = "agentId";
                //    cb_customer.SelectedIndex = -1;
                //}
                //catch { }
                await FillCombo.FillComboCustomers(cb_customer);
            #endregion

                #region fill salesman combo
                //try
                //{
                //    users = await userModel.GetUsersActive();
                //    cb_salesMan.ItemsSource = users;
                //    cb_salesMan.DisplayMemberPath = "username";
                //    cb_salesMan.SelectedValuePath = "userId";
                //    cb_salesMan.SelectedIndex = -1;
                //}
                //catch { }
                await FillCombo.FillComboUsers(cb_salesMan);
                #endregion

                await RefreshInvoiceList();
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

        private async void dp_SelectedEndDateChanged(object sender, SelectionChangedEventArgs e)
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
        private async void dp_SelectedStartDateChanged(object sender, SelectionChangedEventArgs e)
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
        private void translate()
        {
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            //txt_title.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trTransaferDetails");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branch, AppSettings.resourcemanager.GetString("trBranchHint"));
            chk_delivered.Content = AppSettings.resourcemanager.GetString("trDelivered");
            chk_inDelivery.Content = AppSettings.resourcemanager.GetString("trInDelivery");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_searchStartDate, AppSettings.resourcemanager.GetString("trStartDate") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_searchEndDate, AppSettings.resourcemanager.GetString("trEndDate") + "...");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_invoiceNum, AppSettings.resourcemanager.GetString("trInvoiceNumberHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_cash, AppSettings.resourcemanager.GetString("trCashHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_cashDelivered, AppSettings.resourcemanager.GetString("trCashHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_salesMan, AppSettings.resourcemanager.GetString("trSalesManHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_customer, AppSettings.resourcemanager.GetString("trCustomerHint"));

            dg_orderAccounts.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_orderAccounts.Columns[1].Header = AppSettings.resourcemanager.GetString("trSalesMan");
            dg_orderAccounts.Columns[2].Header = AppSettings.resourcemanager.GetString("trCustomer");
            dg_orderAccounts.Columns[3].Header = AppSettings.resourcemanager.GetString("trDate");
            dg_orderAccounts.Columns[4].Header = AppSettings.resourcemanager.GetString("trCashTooltip");
            dg_orderAccounts.Columns[5].Header = AppSettings.resourcemanager.GetString("trState");
            dg_orderAccounts.Columns[6].Header = AppSettings.resourcemanager.GetString("paid");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }
        private void Dg_orderAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//selection
            try
            {
                HelpClass.StartAwait(grid_main);

                if (dg_orderAccounts.SelectedIndex != -1)
            {
                invoice = dg_orderAccounts.SelectedItem as Invoice;
                this.DataContext = cashtrans;

                if (invoice != null)
                {
                    tb_invoiceNum.Text = invoice.invNumber;

                    agentId = invoice.agentId.Value;

                    userId = invoice.shipUserId.Value;

                    tb_cash.Text = HelpClass.DecTostring(invoice.deserved);
                    
                    tb_cashDelivered.Text = HelpClass.DecTostring(invoice.paid);
                    
                    if (invoice.status == "Done")
                    {
                        btn_save.IsEnabled = false;
                        tb_notes.IsEnabled = false;
                    }
                    else
                    {
                        btn_save.IsEnabled = true;
                        tb_notes.IsEnabled = true;
                    }
                }
                else
                {
                    btn_save.IsEnabled = false;
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

        async Task Search()
        {
            try
            {
                if (invoices is null)
                    await RefreshInvoiceList();

                if (chb_all.IsChecked == false)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        searchText = tb_search.Text.ToLower();
                        if (chk_delivered.IsChecked == true)
                            invoiceQuery = invoices.Where(s => (s.invNumber.ToLower().Contains(searchText)
                            || s.branchCreatorName.ToLower().Contains(searchText)
                            || s.shipUserName.ToLower().Contains(searchText)
                            || s.agentName.ToLower().Contains(searchText)
                            || s.totalNet.ToString().ToLower().Contains(searchText)
                            || s.status.ToLower().Contains(searchText)
                            )
                            && s.updateDate.Value.Date >= dp_searchStartDate.SelectedDate.Value.Date
                            && s.updateDate.Value.Date <= dp_searchEndDate.SelectedDate.Value.Date
                            && s.status == "Done"
                            );
                        else if (chk_inDelivery.IsChecked == true)
                            invoiceQuery = invoices.Where(s => (s.invNumber.ToLower().Contains(searchText)
                           || s.branchCreatorName.ToLower().Contains(searchText)
                           || s.shipUserName.ToLower().Contains(searchText)
                           || s.agentName.ToLower().Contains(searchText)
                           || s.totalNet.ToString().ToLower().Contains(searchText)
                           || s.status.ToLower().Contains(searchText)
                           )
                           && s.updateDate.Value.Date >= dp_searchStartDate.SelectedDate.Value.Date
                           && s.updateDate.Value.Date <= dp_searchEndDate.SelectedDate.Value.Date
                           && s.status == "InTheWay"
                           );

                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        searchText = tb_search.Text.ToLower();
                        if (chk_delivered.IsChecked == true)
                            invoiceQuery = invoices.Where(s => (s.invNumber.ToLower().Contains(searchText)
                            || s.branchCreatorName.ToLower().Contains(searchText)
                            || s.shipUserName.ToLower().Contains(searchText)
                            || s.agentName.ToLower().Contains(searchText)
                            || s.totalNet.ToString().ToLower().Contains(searchText)
                            || s.status.ToLower().Contains(searchText)
                            )
                             && s.status == "Done"
                            );
                        else if (chk_inDelivery.IsChecked == true)
                            invoiceQuery = invoices.Where(s => (s.invNumber.ToLower().Contains(searchText)
                            || s.branchCreatorName.ToLower().Contains(searchText)
                            || s.shipUserName.ToLower().Contains(searchText)
                            || s.agentName.ToLower().Contains(searchText)
                            || s.totalNet.ToString().ToLower().Contains(searchText)
                            || s.status.ToLower().Contains(searchText)
                            )
                             && s.status == "InTheWay"
                            );

                    });
                }

                invoiceQueryExcel = invoiceQuery.ToList();
                RefreshInvoiceView();
            }
            catch { }

        }
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
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
      
        private async Task calcBalance(decimal ammount)
        {
            int s = 0;
            //increase pos balance
            Pos pos = await posModel.getById(MainWindow.posLogin.posId);
            pos.balance += ammount;

            s = await pos.save(pos);
        }
        private void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update

        }
        private void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete

        }
        
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {//clear
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

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
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
        void FN_ExportToExcel()
        {
            var QueryExcel = invoiceQuery.AsEnumerable().Select(x => new
            {
                InvoiceNumber = x.invNumber,
                SalesMan = x.shipUserName,
                Customer = x.agentName,
                Cash = x.totalNet,
                Status = x.status
            });
            var DTForExcel = QueryExcel.ToDataTable();
            DTForExcel.Columns[0].Caption = AppSettings.resourcemanager.GetString("trInvoiceNumber");
            DTForExcel.Columns[1].Caption = AppSettings.resourcemanager.GetString("trSalesMan");
            DTForExcel.Columns[2].Caption = AppSettings.resourcemanager.GetString("trCustomer");
            DTForExcel.Columns[3].Caption = AppSettings.resourcemanager.GetString("trCashTooltip");
            DTForExcel.Columns[4].Caption = AppSettings.resourcemanager.GetString("trState");

            ExportToExcel.Export(DTForExcel);

        }
        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {//image
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                {
                    if (cashtrans != null || cashtrans.cashTransId != 0)
                    {
                        wd_uploadImage w = new wd_uploadImage();

                        w.tableName = "invoices";
                        w.tableId = invoice.invoiceId;
                        w.docNum = invoice.invNumber;
                    // w.ShowInTaskbar = false;
                        w.ShowDialog();
                    }
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = "";
                tb_search.Text = "";
                await RefreshInvoiceList();
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task<IEnumerable<Invoice>> RefreshInvoiceList()
        {
            invoices = await invoiceModel.getOrdersForPay(Convert.ToInt32(cb_branch.SelectedValue));
            return invoices;

        }
        void RefreshInvoiceView()
        {
            dg_orderAccounts.ItemsSource = invoiceQuery;
            txt_count.Text = invoiceQuery.Count().ToString();
        }
        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
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
        private void validateEmpty(string name, object sender)
        {
        
        }
        private void PreventSpaces(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
        private void Tb_docNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only int
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Tb_cash_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only decimal
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void Tb_validateEmptyTextChange(object sender, TextChangedEventArgs e)
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
        
        //private async Task fillCustomers()
        //{
        //    agents = await agentModel.GetAgentsActive("c");
        //    cb_customer.ItemsSource = agents;
        //    cb_customer.DisplayMemberPath = "name";
        //    cb_customer.SelectedValuePath = "agentId";
        //    cb_salesMan.SelectedIndex = -1;
        //}
        //private async Task fillUsers()
        //{
        //    users = await userModel.GetUsersActive();

        //    cb_salesMan.ItemsSource = users;
        //    cb_salesMan.DisplayMemberPath = "username";
        //    cb_salesMan.SelectedValuePath = "userId";
        //    cb_salesMan.SelectedIndex = -1;
        //}
        //private void Btn_printInvoice_Click(object sender, RoutedEventArgs e)
        //{
        //    if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
        //    {

        //    }
        //    else
        //        Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
        //}
        //private void Btn_invoices_Click(object sender, RoutedEventArgs e)
        //{

        //}
        private void Cb_salesMan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select salesman
            try
            {
                HelpClass.StartAwait(grid_main);

                invoiceQuery = invoiceQuery.Where(u => u.shipUserId == Convert.ToInt32(cb_salesMan.SelectedValue));
                invoiceQueryExcel = invoiceQuery;
                RefreshInvoiceView();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select agent
            try
            {
                HelpClass.StartAwait(grid_main);

                invoiceQuery = invoiceQuery.Where(c => c.agentId == Convert.ToInt32(cb_customer.SelectedValue));
                invoiceQueryExcel = invoiceQuery;
                RefreshInvoiceView();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        //private void Cb_state_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{//select state
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);

        //        invoiceQuery = invoiceQuery.Where(s => s.status == cb_state.SelectedValue.ToString());
        //        invoiceQueryExcel = invoiceQuery;
        //        RefreshInvoiceView();

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private async void Cb_branch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select branch
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
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//pay
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (MainWindow.groupObject.HasPermissionAction(createPermission, MainWindow.groupObjects, "one"))
                {
                    if (MainWindow.posLogin.boxState == "o")
                    {
                        bool multipleValid = true;
                        List<CashTransfer> listPayments = new List<CashTransfer>();
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_multiplePayment w = new wd_multiplePayment();
                        w.isPurchase = false;

                        Agent customer = FillCombo.customersList.ToList().Find(b => b.agentId == invoice.agentId && b.isLimited == true);
                        if (customer != null)
                        {
                            decimal remain = 0;
                            //if (customer.maxDeserve != 0)
                            //    remain = getCusAvailableBlnc(customer);
                            w.hasCredit = true;
                            w.maxCredit = remain;
                        }
                        else
                        {
                            w.hasCredit = false;
                            w.maxCredit = 0;
                        }

                        w.invoice.invType = invoice.invType;
                        w.invoice.totalNet = invoice.totalNet;
                        w.cards = FillCombo.cardsList;
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        multipleValid = w.isOk;
                        listPayments = w.listPayments;

                        if (multipleValid)
                        {
                            #region Save

                            foreach (var item in listPayments)
                            {
                                await saveConfiguredCashTrans(item);
                                // yasin code
                                if (item.processType != "balance")
                                {
                                    invoice.paid += item.cash;
                                    invoice.deserved -= item.cash;
                                }
                            }

                            int s = await invoice.saveInvoice(invoice);
                            
                            if (!s.Equals(0))
                            {
                                List<OrderPreparing> orderPpList = await orderModel.GetOrdersByInvoiceId(s);
                                OrderPreparing orderPp = orderPpList.FirstOrDefault(); 
                                await saveOrderStatus(orderPp.orderPreparingId, "Done");

                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                Clear();

                                await RefreshInvoiceList();
                                Tb_search_TextChanged(null, null);
                                await MainWindow.refreshBalance();
                                btn_save.IsEnabled = false;
                            }
                            else
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            }
                            #endregion
                        }
                    }
                    else //box is closed
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBoxIsClosed"), animation: ToasterAnimation.FadeIn);
                    }
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        OrderPreparing orderModel = new OrderPreparing();
        private async Task saveOrderStatus(int orderPreparingId, string status)
        {
            //invoiceStatus st = new invoiceStatus();
            //st.status = status;
            //st.invoiceId = invoiceId;
            //st.createUserId = MainWindow.userLogin.userId;
            //st.isActive = 1;
            //await invoice.saveOrderStatus(st);

          

            orderPreparingStatus ops = new orderPreparingStatus();
            ops.orderPreparingId = orderPreparingId;
            //ops.status = "Done";
            ops.status = status;
            ops.createUserId = MainWindow.userLogin.userId;
            ops.updateUserId = MainWindow.userLogin.userId;
            ops.notes = tb_notes.Text;
            ops.isActive = 1;
            await orderModel.updateOrderStatus(ops);

        }

        private async Task saveConfiguredCashTrans(CashTransfer cashTransfer)
        {
            switch (cashTransfer.processType)
            {
                case "cash":// cash: update pos balance   
                    MainWindow.posLogin.balance += invoice.totalNet;
                    await MainWindow.posLogin.save(MainWindow.posLogin);
                    cashTransfer.transType = "d"; //deposit
                    cashTransfer.posId = MainWindow.posLogin.posId;
                    cashTransfer.agentId = invoice.agentId;
                    cashTransfer.invId = invoice.invoiceId;
                    cashTransfer.transNum = await cashTransfer.generateCashNumber("dc");
                    cashTransfer.side = "c"; // customer                    
                    cashTransfer.createUserId = MainWindow.userLogin.userId;
                    await cashTransfer.Save(cashTransfer); //add cash transfer   
                    break;
                case "balance":// balance: update customer balance
                    await invoice.recordConfiguredAgentCash(invoice, "si", cashTransfer);
                    break;
                case "card": // card
                    cashTransfer.transType = "d"; //deposit
                    cashTransfer.posId = MainWindow.posLogin.posId;
                    cashTransfer.agentId = invoice.agentId;
                    cashTransfer.invId = invoice.invoiceId;
                    cashTransfer.transNum = await cashTransfer.generateCashNumber("dc");
                    cashTransfer.side = "c"; // customer
                    cashTransfer.createUserId = MainWindow.userLogin.userId;
                    await cashTransfer.Save(cashTransfer); //add cash transfer  
                    break;
            }
        }
        /*
        private decimal getCusAvailableBlnc(Agent customer)
        {
            decimal remain = 0;

            decimal customerBalance = customer.balance;

            if (customer.balanceType == 0)
                remain = invoice.totalNet - (decimal)customerBalance;
            else
                remain = (decimal)customer.balance + invoice.totalNet;
            return remain;
        }
        */

        #region report

        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
     
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string firstTitle = "orders";
            string secondTitle = "";
            string state = "";
            string Title = "";


            if (chk_delivered.IsChecked == true)
            {
                secondTitle = "delivered";
                state = "d";

            }
            else if (chk_inDelivery.IsChecked == true)
            {
                secondTitle = "indelivery";
                state = "i";

            }


            string addpath;
            bool isArabic = ReportCls.checkLang();

            if (isArabic)
            {
                addpath = @"\Reports\Account\report\Ar\ArOrderAcc.rdlc";
            }
            else
            {
                addpath = @"\Reports\Account\report\En\EnOrderAcc.rdlc";
            }


            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //Title = MainWindow.resourcemanagerreport.GetString("trOrders") + " / " + secondTitle;
            Title = clsReports.ReportTabTitle(firstTitle, secondTitle);
            paramarr.Add(new ReportParameter("trTitle", Title));
            paramarr.Add(new ReportParameter("state", state));
            //clsReports.orderReport(invoiceQuery, rep, reppath);
            clsReports.orderReport(invoiceQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);
            rep.Refresh();

        }
        private void Btn_preview1_Click_1(object sender, RoutedEventArgs e)
        {
            //preview
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                /////////////////////
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    string pdfpath = "";


                    //
                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    BuildReport();

                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
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
                /////////////////////
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                /////////////////////
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    win_IvcAccount win = new win_IvcAccount(invoiceQuery ,2 );
                    // // w.ShowInTaskbar = false;
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                /////////////////////
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_print_Click_1(object sender, RoutedEventArgs e)
        {
            //print
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
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

        private void Btn_exportToExcel_Click_1(object sender, RoutedEventArgs e)
        {
            //excel
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
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
        private void Btn_pdf1_Click(object sender, RoutedEventArgs e)
        {
            //pdf
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                /////////////////////
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                /////////////////////
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        #endregion
        //
        private async void Chb_all_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                dp_searchStartDate.IsEnabled =
            dp_searchEndDate.IsEnabled = false;
                Btn_refresh_Click(btn_refresh, null);
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
                dp_searchStartDate.IsEnabled =
                dp_searchEndDate.IsEnabled = true;

                Btn_refresh_Click(btn_refresh, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Invoice();

            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        { //only  digits
            try
            {
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
        { //only english and digits
            try
            {
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

        private async void search_Checking(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_delivered")
                    {
                        chk_inDelivery.IsChecked = false;
                        dg_orderAccounts.Columns[4].Visibility = Visibility.Collapsed;
                        dg_orderAccounts.Columns[6].Visibility = Visibility.Visible;
                        bdr_cash.Visibility = Visibility.Collapsed;
                        bdr_cashDelivered.Visibility = Visibility.Visible;
                    }
                    else if (cb.Name == "chk_inDelivery")
                    {
                        chk_delivered.IsChecked = false;
                        dg_orderAccounts.Columns[4].Visibility = Visibility.Visible;
                        dg_orderAccounts.Columns[6].Visibility = Visibility.Collapsed;
                        bdr_cash.Visibility = Visibility.Visible;
                        bdr_cashDelivered.Visibility = Visibility.Collapsed;
                    }
                }
                HelpClass.StartAwait(grid_main);
                Clear();
                await Search();

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
                    if (cb.Name == "chk_delivered")
                        chk_delivered.IsChecked = true;
                    else if (cb.Name == "chk_inDelivery")
                        chk_inDelivery.IsChecked = true;
                }
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
    }
}
