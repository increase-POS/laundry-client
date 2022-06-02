using LiveCharts;
using LiveCharts.Wpf;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Data.SqlClient;
using LiveCharts.Helpers;
using laundryApp.View.windows;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.Threading;
using System.Resources;
using System.Reflection;
using laundryApp.View.sales;
using netoaster;

namespace laundryApp.View.reports.salesReports
{
    /// <summary>
    /// Interaction logic for uc_invoiceSalesReports.xaml
    /// </summary>
    public partial class uc_invoiceSalesReports : UserControl
    {
        private static uc_invoiceSalesReports _instance;
        public static uc_invoiceSalesReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_invoiceSalesReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_invoiceSalesReports()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        //prin & pdf
     

        private int selectedTab = 0;

        Statistics statisticModel = new Statistics();

        List<ItemTransferInvoice> Invoices;
        List<ItemTransferInvoice> InvoicesQuery;

        string searchText = "";

        //for combo boxes
        /*************************/
        Branch selectedBranch;
        Pos selectedPos;
        Agent selectedVendor;
        User selectedUser;

        List<Branch> comboBrachTemp = new List<Branch>();
        List<Pos> comboPosTemp = new List<Pos>();
        List<Agent> comboVendorTemp = new List<Agent>();
        List<User> comboUserTemp = new List<User>();

        List<Branch> dynamicComboBranches = new List<Branch>();
        List<Pos> dynamicComboPoss = new List<Pos>();
        List<Agent> dynamicComboVendors = new List<Agent>();
        List<User> dynamicComboUsers = new List<User>();

        Branch branchModel = new Branch();
        Pos posModel = new Pos();
        Agent agentModel = new Agent();
        User userModel = new User();

        List<int> selectedBranchId = new List<int>();
        List<int> selectedPosId = new List<int>();
        List<int> selectedVendorsId = new List<int>();
        List<int> selectedUserId = new List<int>();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                fillServices();

                chk_allBranches.IsChecked = true;
                chk_allServices.IsChecked = true;

                btn_branch_Click(btn_branch , null);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region methods
        private async void callSearch(object sender)
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
            tt_branch.Content = AppSettings.resourcemanager.GetString("trBranches");
            tt_pos.Content = AppSettings.resourcemanager.GetString("trPOSs");
            tt_vendors.Content = AppSettings.resourcemanager.GetString("trCustomers");
            tt_users.Content = AppSettings.resourcemanager.GetString("trUsers");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_sevices, AppSettings.resourcemanager.GetString("typesOfService") +"...");

            chk_allBranches.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allServices.Content = AppSettings.resourcemanager.GetString("trAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_endDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dt_endTime, AppSettings.resourcemanager.GetString("trEndTime") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dt_startTime, AppSettings.resourcemanager.GetString("trStartTime") + "...");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_No.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_type.Header = AppSettings.resourcemanager.GetString("trType");
            col_date.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_pos.Header = AppSettings.resourcemanager.GetString("trPOS");
            col_vendor.Header = AppSettings.resourcemanager.GetString("trCustomer");
            col_agentCompany.Header = AppSettings.resourcemanager.GetString("trCompany");
            col_user.Header = AppSettings.resourcemanager.GetString("trUser");
            col_count.Header = AppSettings.resourcemanager.GetString("trQTR");
            col_discount.Header = AppSettings.resourcemanager.GetString("trDiscount");
            col_tax.Header = AppSettings.resourcemanager.GetString("trTax");
            col_totalNet.Header = AppSettings.resourcemanager.GetString("trTotal");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }

        private void fillServices()
        {
            var typelist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trDiningHallType")       , Value = "s" },
                new { Text = AppSettings.resourcemanager.GetString("trTakeAway")             , Value = "ts" },
                new { Text = AppSettings.resourcemanager.GetString("trSelfService")          , Value = "ss" },
                 };
            cb_sevices.SelectedValuePath = "Value";
            cb_sevices.DisplayMemberPath = "Text";
            cb_sevices.ItemsSource = typelist;
            cb_sevices.SelectedIndex = -1;
        }
        private void fillComboBranches()
        {
            cb_branches.SelectedValuePath = "branchCreatorId";
            cb_branches.DisplayMemberPath = "branchCreatorName";
            var lst = Invoices.GroupBy(i => i.branchCreatorId).Select(i => new { i.FirstOrDefault().branchCreatorName, i.FirstOrDefault().branchCreatorId });
            cb_branches.ItemsSource = lst;
        }

        private void fillComboPos()
        {
            cb_branches.SelectedValuePath = "posId";
            cb_branches.DisplayMemberPath = "posName";
            var lst = Invoices.GroupBy(i => i.posId).Select(i => new { i.FirstOrDefault().posName, i.FirstOrDefault().posId });
            cb_branches.ItemsSource = lst;
        }

        private void fillComboVendors()
        {
            cb_branches.SelectedValuePath = "agentId";
            cb_branches.DisplayMemberPath = "agentName";
            var lst = Invoices.GroupBy(i => i.agentId).Select(i => new { i.FirstOrDefault().agentName, i.FirstOrDefault().agentId });
            cb_branches.ItemsSource = lst;
        }

        private void fillComboUsers()
        {
            cb_branches.SelectedValuePath = "updateUserId";
            cb_branches.DisplayMemberPath = "uUserAccName";
            var lst = Invoices.GroupBy(i => i.updateUserId).Select(i => new { i.FirstOrDefault().updateUserId, i.FirstOrDefault().uUserAccName });
            cb_branches.ItemsSource = lst;
        }

        private void hideSatacks()
        {
            stk_tagsBranches.Visibility = Visibility.Collapsed;
            stk_tagsItems.Visibility = Visibility.Collapsed;
            stk_tagsPos.Visibility = Visibility.Collapsed;
            stk_tagsUsers.Visibility = Visibility.Collapsed;
            stk_tagsVendors.Visibility = Visibility.Collapsed;
            stk_tagsCoupons.Visibility = Visibility.Collapsed;
            stk_tagsOffers.Visibility = Visibility.Collapsed;
        }

        public void paint()
        {
            //bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);

            bdr_branch.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_pos.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_vendors.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_users.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;

            path_branch.Fill = Brushes.White;
            path_pos.Fill = Brushes.White;
            path_vendors.Fill = Brushes.White;
            path_users.Fill = Brushes.White;
        }

        async Task Search()
        {

            if (Invoices is null)
                await RefreshInvoicesList();

            searchText = txt_search.Text.ToLower();
            InvoicesQuery = Invoices
               .Where(s =>
            (
            s.invNumber.ToLower().Contains(searchText)
            ||
            (s.invBarcode        != null ? s.barcode.ToLower().Contains(searchText) : false)
            ||
            (s.branchCreatorName != null ? s.branchCreatorName.ToLower().Contains(searchText) : false)
            ||
            (s.posName      != null ? s.posName.ToLower().Contains(searchText)      : false)
            ||
            (s.agentName    != null ? s.agentName.ToLower().Contains(searchText)    : false)
            ||
            (s.agentCompany != null ? s.agentCompany.ToLower().Contains(searchText) : false)
            ||
            (s.uUserAccName != null ? s.uUserAccName.ToLower().Contains(searchText) : false)
            )
            &&
            //service
            (cb_sevices.SelectedIndex != -1     ? s.invType == cb_sevices.SelectedValue.ToString() : true)
            &&
            //start date
            (dp_startDate.SelectedDate != null  ? s.invDate >= dp_startDate.SelectedDate : true)
            &&
            //end date
            (dp_endDate.SelectedDate != null    ? s.invDate <= dp_endDate.SelectedDate : true)
            && 
            //start time
            (dt_startTime.SelectedTime != null  ? s.invDate >= dt_startTime.SelectedTime : true)
            && 
            //end time
            (dt_endTime.SelectedTime != null    ? s.invDate <= dt_endTime.SelectedTime : true)
            ).ToList();
            //branch
            if (selectedTab == 0)
                InvoicesQuery = InvoicesQuery.Where(j => (selectedBranchId.Count != 0 ? selectedBranchId.Contains((int)j.branchCreatorId) : true)).ToList();
            //pos
            if (selectedTab == 1)
                InvoicesQuery = InvoicesQuery.Where(j => (selectedPosId.Count != 0 ? selectedPosId.Contains((int)j.posId) : true)).ToList();
            //agent
            if (selectedTab == 2)
                InvoicesQuery = InvoicesQuery.Where(j => (selectedVendorsId.Count != 0 ? selectedVendorsId.Contains((int)j.agentId) : true)).ToList();
            //user
            if (selectedTab == 3)
                InvoicesQuery = InvoicesQuery.Where(j => (selectedUserId.Count != 0 ? selectedUserId.Contains((int)j.updateUserId) : true)).ToList();

            RefreshInvoicesView();
        }

        void RefreshInvoicesView()
        {
            dgInvoice.ItemsSource = InvoicesQuery;
            txt_count.Text = InvoicesQuery.Count().ToString();

            //hide tax column if region tax equals to 0
            if (!AppSettings.invoiceTax_bool.Value)
                col_tax.Visibility = Visibility.Hidden;
            else
                col_tax.Visibility = Visibility.Visible;

            List<int> selected = new List<int>();
            if (selectedTab == 0)
                selected = selectedBranchId;
            if (selectedTab == 1)
                selected = selectedPosId;
            if (selectedTab == 2)
                selected = selectedVendorsId;
            if (selectedTab == 3)
                selected = selectedUserId;

            fillColumnChart(selected);
            fillPieChart(selected);
            fillRowChart(selected);
        }

        async Task<IEnumerable<ItemTransferInvoice>> RefreshInvoicesList()
        {
            Invoices = await statisticModel.GetSaleitemcount((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);
            return Invoices;
        }
       
        #endregion
      
        #region Events

        #region tabControl
        private void hidAllColumns()
        {
            col_type.Visibility = Visibility.Hidden;
            col_branch.Visibility = Visibility.Hidden;
            col_pos.Visibility = Visibility.Hidden;
            col_vendor.Visibility = Visibility.Hidden;
            col_agentCompany.Visibility = Visibility.Hidden;
            col_user.Visibility = Visibility.Hidden;
            col_discount.Visibility = Visibility.Hidden;
            col_count.Visibility = Visibility.Hidden;
            col_totalNet.Visibility = Visibility.Hidden;
            col_tax.Visibility = Visibility.Hidden;
        }
        private async void btn_branch_Click(object sender, RoutedEventArgs e)
        {//branches
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trBranchHint"));

                txt_search.Text = "";
                hideSatacks();
                stk_tagsBranches.Visibility = Visibility.Visible;
                selectedTab = 0;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_branch);
                path_branch.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hidAllColumns();
                //show columns
                col_branch.Visibility = Visibility.Visible;
                col_count.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;
                col_discount.Visibility = Visibility.Visible;
                col_tax.Visibility = Visibility.Visible;
                col_type.Visibility = Visibility.Visible;

                await Search();
                fillComboBranches();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void btn_pos_Click(object sender, RoutedEventArgs e)
        {//pos
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trPosHint"));

                txt_search.Text = "";
                hideSatacks();
                stk_tagsPos.Visibility = Visibility.Visible;
                selectedTab = 1;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_pos);
                path_pos.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hidAllColumns();
                //show columns
                col_branch.Visibility = Visibility.Visible;
                col_count.Visibility = Visibility.Visible;
                col_pos.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;
                col_discount.Visibility = Visibility.Visible;
                col_tax.Visibility = Visibility.Visible;
                col_type.Visibility = Visibility.Visible;

                await Search();
                fillComboPos();
               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void btn_vendors_Click(object sender, RoutedEventArgs e)
        {//vendor
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trCustomerHint"));

                txt_search.Text = "";
                hideSatacks();
                stk_tagsVendors.Visibility = Visibility.Visible;
                selectedTab = 2;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_vendors);
                path_vendors.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hidAllColumns();
                //show columns
                col_branch.Visibility = Visibility.Visible;
                col_vendor.Visibility = Visibility.Visible;
                col_agentCompany.Visibility = Visibility.Visible;
                col_discount.Visibility = Visibility.Visible;
                col_count.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;
                col_tax.Visibility = Visibility.Visible;
                col_type.Visibility = Visibility.Visible;

                await Search();
                fillComboVendors();

               HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void btn_users_Click(object sender, RoutedEventArgs e)
        {//users
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trUserHint"));

                txt_search.Text = "";
                hideSatacks();
                stk_tagsUsers.Visibility = Visibility.Visible;
                selectedTab = 3;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_users);
                path_users.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hidAllColumns();
                //show columns
                col_branch.Visibility = Visibility.Visible;
                col_discount.Visibility = Visibility.Visible;
                col_pos.Visibility = Visibility.Visible;
                col_user.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;
                col_tax.Visibility = Visibility.Visible;
                col_type.Visibility = Visibility.Visible;

                await Search();
                fillComboUsers();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                chk_allBranches.IsChecked = true;
                chk_allServices.IsChecked = true;
                dp_endDate.SelectedDate = null;
                dp_startDate.SelectedDate = null;
                dt_startTime.SelectedTime = null;
                dt_endTime.SelectedTime = null;

                txt_search.Text = "";
               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            callSearch(sender);
        }

        private void dt_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            callSearch(sender);
        }

        private async void Chk_allBranches_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_branches.SelectedIndex = -1;
                cb_branches.IsEnabled = false;

                hideSatacks();
                //branch stack
                selectedBranch = null;
                selectedBranchId.Clear();
                comboBrachTemp.Clear();
                dynamicComboBranches.Clear();
                stk_tagsBranches.Children.Clear();
                //pos stack
                selectedPos = null;
                selectedPosId.Clear();
                comboPosTemp.Clear();
                dynamicComboPoss.Clear();
                stk_tagsPos.Children.Clear();
                //vendor stack
                selectedVendor = null;
                selectedVendorsId.Clear();
                comboVendorTemp.Clear();
                dynamicComboVendors.Clear();
                stk_tagsVendors.Children.Clear();
                //user stack
                selectedUser = null;
                selectedUserId.Clear();
                comboUserTemp.Clear();
                dynamicComboUsers.Clear();
                stk_tagsUsers.Children.Clear();

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Chk_allBranches_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_branches.IsEnabled = true;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void cb_branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (cb_branches.SelectedItem != null)
                {
                    if (selectedTab == 0)
                    {
                        stk_tagsBranches.Visibility = Visibility.Visible;
                        if (stk_tagsBranches.Children.Count < 5)
                        {
                            int bId = (int)cb_branches.SelectedValue;

                            selectedBranch = await branchModel.getBranchById(bId);
                            var b = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedBranch.name,
                                Name = "btn" + selectedBranch.branchId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            b.DeleteClick += Chip_OnDeleteClick;
                            stk_tagsBranches.Children.Add(b);
                            comboBrachTemp.Add(selectedBranch);
                            selectedBranchId.Add(selectedBranch.branchId);
                            dynamicComboBranches.Remove(selectedBranch);
                        }
                    }
                    if (selectedTab == 1)
                    {
                        stk_tagsPos.Visibility = Visibility.Visible;
                        if (stk_tagsPos.Children.Count < 5)
                        {
                            int pId = (int)cb_branches.SelectedValue;

                            selectedPos = await posModel.getById(pId);
                            var p = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedPos.name,
                                Name = "btn" + selectedPos.posId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            p.DeleteClick += Chip_OnDeleteClick;
                            stk_tagsPos.Children.Add(p);
                            comboPosTemp.Add(selectedPos);
                            selectedPosId.Add(selectedPos.posId);
                            dynamicComboPoss.Remove(selectedPos);
                        }
                    }
                    if (selectedTab == 2)
                    {
                        stk_tagsVendors.Visibility = Visibility.Visible;
                        if (stk_tagsVendors.Children.Count < 5)
                        {
                            int aId = (int)cb_branches.SelectedValue;

                            selectedVendor = await agentModel.getAgentById(aId);
                            try
                            {
                                var a = new MaterialDesignThemes.Wpf.Chip()
                                {
                                    Content = selectedVendor.name,
                                    Name = "btn" + selectedVendor.agentId.ToString(),
                                    IsDeletable = true,
                                    Margin = new Thickness(5, 0, 5, 0)
                                };
                                a.DeleteClick += Chip_OnDeleteClick;
                                stk_tagsVendors.Children.Add(a);
                                comboVendorTemp.Add(selectedVendor);
                                selectedVendorsId.Add(selectedVendor.agentId);
                                dynamicComboVendors.Remove(selectedVendor);
                            }
                            catch { }
                        }
                    }
                    if (selectedTab == 3)
                    {
                        stk_tagsUsers.Visibility = Visibility.Visible;
                        if (stk_tagsUsers.Children.Count < 5)
                        {
                            int uId = (int)cb_branches.SelectedValue;

                            selectedUser = await userModel.getUserById(uId);
                            var u = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedUser.name,
                                Name = "btn" + selectedUser.userId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            u.DeleteClick += Chip_OnDeleteClick;
                            stk_tagsUsers.Children.Add(u);
                            comboUserTemp.Add(selectedUser);
                            selectedUserId.Add(selectedUser.userId);
                            dynamicComboUsers.Remove(selectedUser);
                        }
                    }

                    await Search();

                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Chk_allServices_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_sevices.SelectedIndex = -1;
                cb_sevices.IsEnabled = false;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Chk_allServices_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_sevices.IsEnabled = true;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_sevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            callSearch(sender);
        }

        IEnumerable<ItemTransferInvoice> itemTransfers = null;

        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            callSearch(sender);
        }
        private async void Chip_OnDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                var currentChip = (Chip)sender;
                if (selectedTab == 0)
                {
                    stk_tagsBranches.Children.Remove(currentChip);
                    var m = comboBrachTemp.Where(j => j.branchId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                    dynamicComboBranches.Add(m.FirstOrDefault());
                    selectedBranchId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                    if (selectedBranchId.Count == 0)
                    {
                        cb_branches.SelectedItem = null;
                    }
                }
                else if (selectedTab == 1)
                {
                    stk_tagsPos.Children.Remove(currentChip);
                    var m = comboPosTemp.Where(j => j.posId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                    dynamicComboPoss.Add(m.FirstOrDefault());
                    selectedPosId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                    if (selectedPosId.Count == 0)
                    {
                        cb_branches.SelectedItem = null;
                    }
                }
                else if (selectedTab == 2)
                {
                    stk_tagsVendors.Children.Remove(currentChip);
                    var m = comboVendorTemp.Where(j => j.agentId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                    dynamicComboVendors.Add(m.FirstOrDefault());
                    selectedVendorsId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                    if (selectedVendorsId.Count == 0)
                    {
                        cb_branches.SelectedItem = null;
                    }
                }
                else if (selectedTab == 3)
                {
                    stk_tagsUsers.Children.Remove(currentChip);
                    var m = comboUserTemp.Where(j => j.userId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                    dynamicComboUsers.Add(m.FirstOrDefault());
                    selectedUserId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                    if (selectedUserId.Count == 0)
                    {
                        cb_branches.SelectedItem = null;
                    }
                }
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

        #region charts
        private void fillPieChart(List<int> stackedButton)
        {
            List<string> titles = new List<string>();
            IEnumerable<int> x = null;

            var temp = InvoicesQuery;

            if (selectedTab == 0)
            {
                temp = temp.Where(j => (selectedBranchId.Count != 0 ? stackedButton.Contains((int)j.branchCreatorId) : true)).ToList();
                var titleTemp = temp.GroupBy(m => m.branchCreatorName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.branchCreatorId).Select(s => new { branchCreatorId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }
            else if (selectedTab == 1)
            {
                temp = temp.Where(j => (selectedPosId.Count != 0 ? stackedButton.Contains((int)j.posId) : true)).ToList();
                var titleTemp = temp.GroupBy(m => new { m.posName, m.posId });
                titles.AddRange(titleTemp.Select(jj => jj.Key.posName));
                var result = temp.GroupBy(s => s.posId).Select(s => new { posId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }
            else if (selectedTab == 2)
            {
                temp = temp.Where(j => (selectedVendorsId.Count != 0 ? stackedButton.Contains((int)j.agentId) : true)).ToList();
                var titleTemp = temp.GroupBy(m => m.agentName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.agentId).Select(s => new { agentId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }
            else if (selectedTab == 3)
            {
                temp = temp.Where(j => (selectedUserId.Count != 0 ? stackedButton.Contains((int)j.updateUserId) : true)).ToList();
                var titleTemp = temp.GroupBy(m => m.cUserAccName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.updateUserId).Select(s => new { updateUserId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }

            SeriesCollection piechartData = new SeriesCollection();
            int xCount = 6;
            if (x.Count() <= 6) xCount = x.Count();
            for (int i = 0; i < xCount; i++)
            {
                List<int> final = new List<int>();

                final.Add(x.ToList().Skip(i).FirstOrDefault());
                piechartData.Add(
                  new PieSeries
                  {
                      Values = final.AsChartValues(),
                      Title = titles.Skip(i).FirstOrDefault(),
                      DataLabels = true,
                  }
              );
            }
            if (x.Count() > 6)
            {
                int finalSum = 0;
                for (int i = 6; i < x.Count(); i++)
                {
                    finalSum = finalSum + x.ToList().Skip(i).FirstOrDefault();
                }
                if (finalSum != 0)
                {
                    List<int> final = new List<int>();

                    final.Add(finalSum);
                    piechartData.Add(
                      new PieSeries
                      {
                          Values = final.AsChartValues(),
                          Title = AppSettings.resourcemanager.GetString("trOthers"),
                          DataLabels = true,
                      }
                  );
                }
            }
            chart1.Series = piechartData;
        }

        private void fillColumnChart(List<int> stackedButton)
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<int> x = null;
            IEnumerable<int> y = null;
            IEnumerable<int> z = null;
           
            var temp = InvoicesQuery;
            if (selectedTab == 0)
            {
                temp = temp.Where(j => (selectedBranchId.Count != 0 ? stackedButton.Contains((int)j.branchCreatorId) : true)).ToList();
                var result = temp.GroupBy(s => s.branchCreatorId).Select(s => new
                {
                    branchCreatorId = s.Key,
                    countP  = s.Where(m => m.invType == "s").Count(),
                    countPb = s.Where(m => m.invType == "ts").Count(),
                    countD  = s.Where(m => m.invType == "ss").Count()
                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.branchCreatorName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            else if (selectedTab == 1)
            {
                temp = temp.Where(j => (selectedPosId.Count != 0 ? stackedButton.Contains((int)j.posId) : true)).ToList();
                var result = temp.GroupBy(s => s.posId).Select(s => new
                {
                    posId = s.Key,
                    countP  = s.Where(m => m.invType == "s").Count(),
                    countPb = s.Where(m => m.invType == "ts").Count(),
                    countD  = s.Where(m => m.invType == "ss").Count()
                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.posName).Select(s => new
                {
                    uUserName = s.Key + "/" + s.FirstOrDefault().branchCreatorName
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            else if (selectedTab == 2)
            {
                temp = temp.Where(j => (selectedVendorsId.Count != 0 ? stackedButton.Contains((int)j.agentId) : true)).ToList();
                var result = temp.GroupBy(s => s.agentId).Select(s => new
                {
                    agentId = s.Key,
                    countP  = s.Where(m => m.invType == "s").Count(),
                    countPb = s.Where(m => m.invType == "ts").Count(),
                    countD  = s.Where(m => m.invType == "ss").Count()

                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.agentName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            else if (selectedTab == 3)
            {
                temp = temp.Where(j => (selectedUserId.Count != 0 ? stackedButton.Contains((int)j.updateUserId) : true)).ToList();
                var result = temp.GroupBy(s => s.updateUserId).Select(s => new
                {
                    updateUserId = s.Key,
                    countP  = s.Where(m => m.invType == "s").Count(),
                    countPb = s.Where(m => m.invType == "ts").Count(),
                    countD  = s.Where(m => m.invType == "ss").Count()

                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.uUserAccName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }

            SeriesCollection columnChartData = new SeriesCollection();
            List<int> cP = new List<int>();
            List<int> cPb = new List<int>();
            List<int> cD = new List<int>();
            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trDiningHallType"),
                AppSettings.resourcemanager.GetString("trTakeAway"),
                AppSettings.resourcemanager.GetString("trSelfService")
            };
          
            int xCount = 6;
            if (x.Count() <= 6) xCount = x.Count();

            for (int i = 0; i < xCount; i++)
            {
                cP.Add(x.ToList().Skip(i).FirstOrDefault());
                cPb.Add(y.ToList().Skip(i).FirstOrDefault());
                cD.Add(z.ToList().Skip(i).FirstOrDefault());
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            if (x.Count() > 6)
            {
                int cPSum = 0, cPbSum = 0, cDSum = 0;
                for (int i = 6; i < x.Count(); i++)
                {
                    cPbSum = cPSum + x.ToList().Skip(i).FirstOrDefault();
                    cPbSum = cPbSum + y.ToList().Skip(i).FirstOrDefault();
                    cDSum = cDSum + z.ToList().Skip(i).FirstOrDefault();
                }
                if (!((cPbSum == 0) && (cPbSum == 0) && (cDSum == 0)))
                {
                    cP.Add(cPSum);
                    cPb.Add(cPbSum);
                    cD.Add(cDSum);
                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            //3 فوق بعض
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cP.AsChartValues(),
                Title = titles[0],
                DataLabels = true,
            });
            columnChartData.Add(
           new StackedColumnSeries
           {
               Values = cPb.AsChartValues(),
               Title = titles[1],
               DataLabels = true,
           });
            columnChartData.Add(
           new StackedColumnSeries
           {
               Values = cD.AsChartValues(),
               Title = titles[2],
               DataLabels = true,
           });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillRowChart(List<int> stackedButton)
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<decimal> sTemp = null;
            IEnumerable<decimal> tsTemp = null;
            IEnumerable<decimal> ssTemp = null;

            var temp = InvoicesQuery;
            if (selectedTab == 0)
            {
                temp = temp.Where(j => (selectedBranchId.Count != 0 ? stackedButton.Contains((int)j.branchCreatorId) : true)).ToList();
                var result = temp.GroupBy(s => s.branchCreatorId).Select(s => new
                {
                    branchCreatorId = s.Key,
                    totals = s.Where(x => x.invType == "s").Sum(x => x.totalNet),
                    totalts = s.Where(x => x.invType == "ts").Sum(x => x.totalNet),
                    totalss = s.Where(x => x.invType == "ss").Sum(x => x.totalNet)
                }

             );
                sTemp = result.Select(x => (decimal)x.totals);
                tsTemp = result.Select(x => (decimal)x.totalts);
                ssTemp = result.Select(x => (decimal)x.totalss);
                var tempName = temp.GroupBy(s => s.branchCreatorName).Select(s => new
                {
                    name = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.name));
            }
            if (selectedTab == 1)
            {
                temp = temp.Where(j => (selectedPosId.Count != 0 ? stackedButton.Contains((int)j.posId) : true)).ToList();
                var result = temp.GroupBy(s => s.posId).Select(s => new
                {
                    posId = s.Key,
                    totals = s.Where(x => x.invType == "s").Sum(x => x.totalNet),
                    totalts = s.Where(x => x.invType == "ts").Sum(x => x.totalNet),
                    totalss = s.Where(x => x.invType == "ss").Sum(x => x.totalNet)
                }
             );
                sTemp = result.Select(x => (decimal)x.totals);
                tsTemp = result.Select(x => (decimal)x.totalts);
                ssTemp = result.Select(x => (decimal)x.totalss);
                var tempName = temp.GroupBy(s => s.posName).Select(s => new
                {
                    name = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.name));
            }
            if (selectedTab == 2)
            {
                temp = temp.Where(j => (selectedVendorsId.Count != 0 ? stackedButton.Contains((int)j.agentId) : true)).ToList();
                var result = temp.GroupBy(s => s.agentId).Select(s => new
                {
                    agentId = s.Key,
                    totals = s.Where(x => x.invType == "s").Sum(x => x.totalNet),
                    totalts = s.Where(x => x.invType == "ts").Sum(x => x.totalNet),
                    totalss = s.Where(x => x.invType == "ss").Sum(x => x.totalNet)
                }
             );
                sTemp = result.Select(x => (decimal)x.totals);
                tsTemp = result.Select(x => (decimal)x.totalts);
                ssTemp = result.Select(x => (decimal)x.totalss);
                var tempName = temp.GroupBy(s => s.agentName).Select(s => new
                {
                    name = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.name));
            }
            if (selectedTab == 3)
            {
                temp = temp.Where(j => (selectedUserId.Count != 0 ? stackedButton.Contains((int)j.updateUserId) : true)).ToList();
                var result = temp.GroupBy(s => s.updateUserId).Select(s => new
                {
                    updateUserId = s.Key,
                    totals = s.Where(x => x.invType == "s").Sum(x => x.totalNet),
                    totalts = s.Where(x => x.invType == "ts").Sum(x => x.totalNet),
                    totalss = s.Where(x => x.invType == "ss").Sum(x => x.totalNet)
                }
             );
                sTemp = result.Select(x => (decimal)x.totals);
                tsTemp = result.Select(x => (decimal)x.totalts);
                ssTemp = result.Select(x => (decimal)x.totalss);
                var tempName = temp.GroupBy(s => s.uUserAccName).Select(s => new
                {
                    name = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.name));
            }

            SeriesCollection rowChartData = new SeriesCollection();
            List<decimal> dinningHall = new List<decimal>();
            List<decimal> takeAway = new List<decimal>();
            List<decimal> selfService = new List<decimal>();
            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trDiningHallType"),
                AppSettings.resourcemanager.GetString("trTakeAway"),
                AppSettings.resourcemanager.GetString("trSelfService")
            };
          
            int xCount = 0;
            if (sTemp.Count() <= 6) xCount = sTemp.Count();
            for (int i = 0; i < xCount; i++)
            {
                dinningHall.Add(sTemp.ToList().Skip(i).FirstOrDefault());
                takeAway.Add(tsTemp.ToList().Skip(i).FirstOrDefault());
                selfService.Add(ssTemp.ToList().Skip(i).FirstOrDefault());
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            if (sTemp.Count() > 6)
            {
                decimal dinningSum = 0, takeSum = 0, selfSum = 0;
                for (int i = 6; i < sTemp.Count(); i++)
                {
                    dinningSum = dinningSum + sTemp.ToList().Skip(i).FirstOrDefault();
                    takeSum = takeSum + tsTemp.ToList().Skip(i).FirstOrDefault();
                    selfSum = selfSum + ssTemp.ToList().Skip(i).FirstOrDefault();
                }
                if (!((dinningSum == 0) && (takeSum == 0) && (selfSum == 0)))
                {
                    dinningHall.Add(dinningSum);
                    takeAway.Add(takeSum);
                    selfService.Add(selfSum);
                    MyAxis.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }

            rowChartData.Add(
          new LineSeries
          {
              Values = dinningHall.AsChartValues(),
              Title = titles[0]
          });
            rowChartData.Add(
         new LineSeries
         {
             Values = takeAway.AsChartValues(),
             Title = titles[1]
         });
            rowChartData.Add(
        new LineSeries
        {
            Values = selfService.AsChartValues(),
            Title = titles[2]

        });
            DataContext = this;
            rowChart.Series = rowChartData;
        }

        #endregion

        #region report
        //prin & pdf
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
   
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string firstTitle = "invoice";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";
            string addpath = "";
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\Ar\ArBranch.rdlc";
                    secondTitle = "branch";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\Ar\ArPos.rdlc";
                    secondTitle = "pos";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 2)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\Ar\ArCustomer.rdlc";
                    //    paramarr.Add(new ReportParameter("isTax", MainWindow.tax.ToString()));
                    secondTitle = "customers";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 3)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\Ar\ArUser.rdlc";
                    secondTitle = "users";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                //else
                //{
                //    addpath = @"\Reports\StatisticReport\Purchase\Invoice\Ar\ArPurItemSts.rdlc";
                //    secondTitle = "items";
                //    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                //}
            }
            else
            {
                //english
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\En\EnBranch.rdlc";
                    secondTitle = "branch";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\En\EnPos.rdlc";
                    secondTitle = "pos";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 2)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\En\EnCustomer.rdlc";
                    // paramarr.Add(new ReportParameter("isTax", MainWindow.tax.ToString()));
                    secondTitle = "vendors";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 3)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Invoice\En\EnUser.rdlc";
                    secondTitle = "users";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                //else
                //{
                //    addpath = @"\Reports\StatisticReport\Purchase\Invoice\En\EnPurItemSts.rdlc";
                //    secondTitle = "items";
                //    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                //}

            }


            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            Title = AppSettings.resourcemanagerreport.GetString("SalesReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            clsReports.SaleInvoiceSTS(InvoicesQuery.ToList(), rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {

                HelpClass.StartAwait(grid_main);

                #region
                BuildReport();

                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
                #endregion


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {

                HelpClass.StartAwait(grid_main);

                #region
                BuildReport();

                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));

                #endregion


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

                #region
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
                #endregion


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {

                HelpClass.StartAwait(grid_main);

                #region
                Window.GetWindow(this).Opacity = 0.2;
                string pdfpath = "";
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


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        #region datagrid print buttons
        private async void pdfRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        ItemTransferInvoice row = (ItemTransferInvoice)dgInvoice.SelectedItems[0];
                        clsReports classreport = new clsReports();
                        resultmessage resmsg = new resultmessage();
                        resmsg = await classreport.pdfSaleInvoice(row.invoiceId, "pdf");
                        if (resmsg.result != "")
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(resmsg.result), animation: ToasterAnimation.FadeIn);

                            });
                        }
                        //cashTransID = row.cashTransId;
                        //openCashTransID = row.openCashTransId.Value;
                        //await getopquery(row);
                        //if (opquery.Count() == 0)
                        //{
                        //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);
                        //}
                        //else
                        //{
                        //    BuildOperationReport();

                        //    saveFileDialog.Filter = "PDF|*.pdf;";

                        //    if (saveFileDialog.ShowDialog() == true)
                        //    {
                        //        string filepath = saveFileDialog.FileName;
                        //        LocalReportExtensions.ExportToPDF(rep, filepath);
                        //    }
                        //}
                    }


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void printRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        ItemTransferInvoice row = (ItemTransferInvoice)dgInvoice.SelectedItems[0];
                        clsReports classreport = new clsReports();
                        resultmessage resmsg = new resultmessage();
                        resmsg = await classreport.pdfSaleInvoice(row.invoiceId, "print");
                        if (resmsg.result != "")
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(resmsg.result), animation: ToasterAnimation.FadeIn);

                            });
                        }
                        //POSOpenCloseModel row = (POSOpenCloseModel)dgClosing.SelectedItems[0];
                        //cashTransID = row.cashTransId;
                        //openCashTransID = row.openCashTransId.Value;
                        //await getopquery(row);
                        //if (opquery.Count() == 0)
                        //{
                        //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        //}
                        //else
                        //{
                        //    BuildOperationReport();

                        //    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
                        //}
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void previewRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        ItemTransferInvoice row = (ItemTransferInvoice)dgInvoice.SelectedItems[0];
                        clsReports classreport = new clsReports();
                        resultmessage resmsg = new resultmessage();
                        resmsg = await classreport.pdfSaleInvoice(row.invoiceId, "prev");
                        if (resmsg.result != "")
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(resmsg.result), animation: ToasterAnimation.FadeIn);

                            });
                        }
                        else
                        {
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_previewPdf w = new wd_previewPdf();
                            w.pdfPath = resmsg.pdfpath;
                            if (!string.IsNullOrEmpty(w.pdfPath))
                            {
                                w.ShowDialog();

                                w.wb_pdfWebViewer.Dispose();

                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: "", animation: ToasterAnimation.FadeIn);
                            Window.GetWindow(this).Opacity = 1;
                        }
                        #region
                        //POSOpenCloseModel row = (POSOpenCloseModel)dgClosing.SelectedItems[0];
                        //cashTransID = row.cashTransId;
                        //openCashTransID = row.openCashTransId.Value;
                        //await getopquery(row);
                        //if (opquery.Count() == 0)
                        //{
                        //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        //}
                        //else
                        //{
                        //    string pdfpath = "";

                        //    pdfpath = @"\Thumb\report\temp.pdf";
                        //    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                        //    BuildOperationReport();
                        //    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                        //    wd_previewPdf w = new wd_previewPdf();
                        //    w.pdfPath = pdfpath;
                        //    if (!string.IsNullOrEmpty(w.pdfPath))
                        //    {
                        //        // w.ShowInTaskbar = false;
                        //        w.ShowDialog();
                        //        w.wb_pdfWebViewer.Dispose();
                        //    }
                        //    Window.GetWindow(this).Opacity = 1;
                        //}
                        #endregion
                    }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void allowPrintRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                int result = 0;
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        ItemTransferInvoice row = (ItemTransferInvoice)dgInvoice.SelectedItems[0];
                        clsReports classreport = new clsReports();

                        Invoice invoiceModel = new Invoice();
                        if (row.invoiceId > 0)
                        {
                            result = await invoiceModel.updateprintstat(row.invoiceId, -1, true, true);


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
                    }
           
                if (sender != null)
                    HelpClass.EndAwait(grid_main);

             
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion


    }
}
