using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using laundryApp.Classes;
using laundryApp.View.purchase;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace laundryApp.View.reports.purchaseReports
{
    /// <summary>
    /// Interaction logic for uc_invoicePurchaseReports.xaml
    /// </summary>
    public partial class uc_invoicePurchaseReports : UserControl
    {
        private static uc_invoicePurchaseReports _instance;
        public static uc_invoicePurchaseReports Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_invoicePurchaseReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_invoicePurchaseReports()
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

   

        private int selectedTab = 0;

        Statistics statisticModel = new Statistics();

        List<ItemTransferInvoice> Invoices;

        //for combo boxes
        /*************************/
        Branch selectedBranch;
        Pos selectedPos;
        Agent selectedVendor;
        User selectedUser;

        List<Branch> comboBranches;
        List<Pos> comboPoss;
        List<Agent> comboVendors;
        List<User> comboUsers;

        ObservableCollection<Branch> comboBrachTemp = new ObservableCollection<Branch>();
        ObservableCollection<Pos> comboPosTemp = new ObservableCollection<Pos>();
        ObservableCollection<Agent> comboVendorTemp = new ObservableCollection<Agent>();
        ObservableCollection<User> comboUserTemp = new ObservableCollection<User>();

        ObservableCollection<Branch> dynamicComboBranches;
        ObservableCollection<Pos> dynamicComboPoss;
        ObservableCollection<Agent> dynamicComboVendors;
        ObservableCollection<User> dynamicComboUsers;

        IEnumerable<ItemTransferInvoice> reportQuery;
        Branch branchModel = new Branch();
        Pos posModel = new Pos();
        Agent agentModel = new Agent();
        User userModel = new User();


        /*************************/

        ObservableCollection<int> selectedBranchId = new ObservableCollection<int>();
        ObservableCollection<int> selectedPosId = new ObservableCollection<int>();
        ObservableCollection<int> selectedVendorsId = new ObservableCollection<int>();
        ObservableCollection<int> selectedUserId = new ObservableCollection<int>();


        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                Invoices = await statisticModel.GetPuritemcount((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);

                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                comboBranches = await branchModel.GetAllWithoutMain("b");
                comboPoss = await posModel.Get();
                comboVendors = await agentModel.Get("v");
                comboUsers = await userModel.Get();

                dynamicComboBranches = new ObservableCollection<Branch>(comboBranches);
                dynamicComboPoss = new ObservableCollection<Pos>(comboPoss);
                dynamicComboVendors = new ObservableCollection<Agent>(comboVendors);
                dynamicComboUsers = new ObservableCollection<User>(comboUsers);

                chk_invoice.IsChecked = true;

                btn_branch_Click(btn_branch, null);

                
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
            tt_vendors.Content = AppSettings.resourcemanager.GetString("trVendors");
            tt_users.Content = AppSettings.resourcemanager.GetString("trUsers");

            chk_invoice.Content = AppSettings.resourcemanager.GetString("tr_Invoice");
            chk_return.Content = AppSettings.resourcemanager.GetString("trReturned");
            chk_drafs.Content = AppSettings.resourcemanager.GetString("trDraft");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trBranchHint"));

            chk_allBranches.Content = AppSettings.resourcemanager.GetString("trAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_endDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dt_endTime, AppSettings.resourcemanager.GetString("trEndTime") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dt_startTime, AppSettings.resourcemanager.GetString("trStartTime") + "...");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_No.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_type.Header = AppSettings.resourcemanager.GetString("trType");
            col_Date.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_pos.Header = AppSettings.resourcemanager.GetString("trPOS");
            col_vendor.Header = AppSettings.resourcemanager.GetString("trVendor");
            col_agentCompany.Header = AppSettings.resourcemanager.GetString("trCompany");
            col_user.Header = AppSettings.resourcemanager.GetString("trUser");
            col_item.Header = AppSettings.resourcemanager.GetString("trItem");
            col_count.Header = AppSettings.resourcemanager.GetString("trQTR");
            col_discount.Header = AppSettings.resourcemanager.GetString("trDiscount");
            col_itQuantity.Header = AppSettings.resourcemanager.GetString("trQTR");
            col_price.Header = AppSettings.resourcemanager.GetString("trPrice");
            col_tax.Header = AppSettings.resourcemanager.GetString("trTax");
            col_subTotal.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_totalNet.Header = AppSettings.resourcemanager.GetString("trTotal");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }

        private void fillComboBranches()
        {
            cb_branches.SelectedValuePath = "branchId";
            cb_branches.DisplayMemberPath = "name";
            cb_branches.ItemsSource = dynamicComboBranches;
        }

        private void fillComboPos()
        {
            cb_branches.SelectedValuePath = "posId";
            cb_branches.DisplayMemberPath = "name";
            cb_branches.ItemsSource = dynamicComboPoss;
        }

        private void fillComboVendors()
        {
            cb_branches.SelectedValuePath = "agentId";
            cb_branches.DisplayMemberPath = "name";
            cb_branches.ItemsSource = dynamicComboVendors;
        }

        private void fillComboUsers()
        {
            cb_branches.SelectedValuePath = "userId";
            cb_branches.DisplayMemberPath = "username";
            cb_branches.ItemsSource = dynamicComboUsers;
        }

        private static void fillDates(DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime)
        {
            if (startDate.SelectedDate != null && startTime.SelectedTime != null)
            {
                string x = startDate.SelectedDate.Value.Date.ToShortDateString();
                string y = startTime.SelectedTime.Value.ToShortTimeString();
                string resultStartTime = x + " " + y;
                startTime.SelectedTime = DateTime.Parse(resultStartTime);
                startDate.SelectedDate = DateTime.Parse(resultStartTime);
            }
            if (endDate.SelectedDate != null && endTime.SelectedTime != null)
            {
                string x = endDate.SelectedDate.Value.Date.ToShortDateString();
                string y = endTime.SelectedTime.Value.ToShortTimeString();
                string resultEndTime = x + " " + y;
                endTime.SelectedTime = DateTime.Parse(resultEndTime);
                endDate.SelectedDate = DateTime.Parse(resultEndTime);
            }
        }

        private IEnumerable<ItemTransferInvoice> fillList(IEnumerable<ItemTransferInvoice> Invoices, CheckBox chkInvoice, CheckBox chkReturn, CheckBox chkDraft
           , DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime)
        {
            fillDates(startDate, endDate, startTime, endTime);
            var result = Invoices.Where(x => (

               ((chkDraft.IsChecked == true ? (x.invType == "pd" || x.invType == "pbd") : false)
                || (chkReturn.IsChecked == true ? (x.invType == "pb") : false)
                || (chkInvoice.IsChecked == true ? ((x.invType == "p") || (x.invType == "pw")) : false))
                && (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
                && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
                && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
                && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)));
            invLst = result;
            return result;
        }
        IEnumerable<ItemTransferInvoice> invLst;
       
        public List<ItemTransferInvoice> filltoprint()
        {
            List<ItemTransferInvoice> xx = new List<ItemTransferInvoice>();

            if (selectedTab == 0)
            {
                xx = fillPdfList(cb_branches, selectedBranchId);
            }
            else if (selectedTab == 1)
            {
                xx = fillPdfList(cb_branches, selectedPosId);
            }
            else if (selectedTab == 2)
            {
                xx = fillPdfList(cb_branches, selectedVendorsId);
            }
            else if (selectedTab == 3)
            {
                xx = fillPdfList(cb_branches, selectedUserId);
            }

            return xx;
        }

        private void fillPieChart(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            List<string> titles = new List<string>();
            IEnumerable<int> x = null;
            titles.Clear();

            var temp = invLst;
            if (selectedTab == 0)
            {
                temp = temp.Where(j => (selectedBranchId.Count != 0 ? stackedButton.Contains((int)j.branchCreatorId) : true));
                var titleTemp = temp.GroupBy(m => m.branchCreatorName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.branchCreatorId).Select(s => new { branchCreatorId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }
            else if (selectedTab == 1)
            {
                temp = temp.Where(j => (selectedPosId.Count != 0 ? stackedButton.Contains((int)j.posId) : true));
                var titleTemp = temp.GroupBy(m => new { m.posName, m.posId });
                titles.AddRange(titleTemp.Select(jj => jj.Key.posName));
                var result = temp.GroupBy(s => s.posId).Select(s => new { posId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }
            else if (selectedTab == 2)
            {
                temp = temp.Where(j => (selectedVendorsId.Count != 0 ? stackedButton.Contains((int)j.agentId) : true));
                var titleTemp = temp.GroupBy(m => m.agentName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.agentId).Select(s => new { agentId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }
            else if (selectedTab == 3)
            {
                temp = temp.Where(j => (selectedUserId.Count != 0 ? stackedButton.Contains((int)j.updateUserId) : true));
                var titleTemp = temp.GroupBy(m => m.cUserAccName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.createUserId).Select(s => new { userId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }

            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < x.Count(); i++)
            {
                List<int> final = new List<int>();
                List<string> lable = new List<string>();
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
            chart1.Series = piechartData;
        }


        private void fillColumnChart(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<int> x = null;
            IEnumerable<int> y = null;
            IEnumerable<int> z = null;

            var temp = invLst;
            if (selectedTab == 0)
            {
                temp = temp.Where(j => (selectedBranchId.Count != 0 ? stackedButton.Contains((int)j.branchCreatorId) : true));
                var result = temp.GroupBy(s => s.branchCreatorId).Select(s => new
                {
                    branchCreatorId = s.Key,
                    countP = s.Where(m => m.invType == "p" || m.invType == "pw").Count(),
                    countPb = s.Where(m => m.invType == "pb").Count(),
                    countD = s.Where(m => m.invType == "pd" || m.invType == "pbd").Count()
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
                temp = temp.Where(j => (selectedPosId.Count != 0 ? stackedButton.Contains((int)j.posId) : true));
                var result = temp.GroupBy(s => s.posId).Select(s => new
                {
                    posId = s.Key,
                    countP = s.Where(m => m.invType == "p" || m.invType == "pw").Count(),
                    countPb = s.Where(m => m.invType == "pb").Count(),
                    countD = s.Where(m => m.invType == "pd" || m.invType == "pbd").Count()
                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.posName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            else if (selectedTab == 2)
            {
                temp = temp.Where(j => (selectedVendorsId.Count != 0 ? stackedButton.Contains((int)j.agentId) : true));
                var result = temp.GroupBy(s => s.agentId).Select(s => new
                {
                    agentId = s.Key,
                    countP = s.Where(m => m.invType == "p" || m.invType == "pw").Count(),
                    countPb = s.Where(m => m.invType == "pb").Count(),
                    countD = s.Where(m => m.invType == "pd" || m.invType == "pbd").Count()

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
                temp = temp.Where(j => (selectedUserId.Count != 0 ? stackedButton.Contains((int)j.updateUserId) : true));
                var result = temp.GroupBy(s => s.createUserId).Select(s => new
                {
                    createUserId = s.Key,
                    countP = s.Where(m => m.invType == "p" || m.invType == "pw").Count(),
                    countPb = s.Where(m => m.invType == "pb").Count(),
                    countD = s.Where(m => m.invType == "pd" || m.invType == "pbd").Count()

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

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> cP = new List<int>();
            List<int> cPb = new List<int>();
            List<int> cD = new List<int>();
            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trPurchases"),
                AppSettings.resourcemanager.GetString("trReturned"),
                AppSettings.resourcemanager.GetString("trDraft")
            };
            for (int i = 0; i < x.Count(); i++)
            {
                cP.Add(x.ToList().Skip(i).FirstOrDefault());
                cPb.Add(y.ToList().Skip(i).FirstOrDefault());
                cD.Add(z.ToList().Skip(i).FirstOrDefault());
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
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
        private List<ItemTransferInvoice> fillPdfList(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            //var temp = fillRowChartList(Invoices, chk_invoice, chk_return, chk_drafs, dp_startDate, dp_endDate, dt_startTime, dt_endTime);
            var temp = fillList(Invoices, chk_invoice, chk_return, chk_drafs, dp_startDate, dp_endDate, dt_startTime, dt_endTime);
            if (selectedTab == 0)
            {
                temp = temp.Where(j => (selectedBranchId.Count != 0 ? stackedButton.Contains((int)j.branchCreatorId) : true));
            }
            else if (selectedTab == 1)
            {
                temp = temp.Where(j => (selectedPosId.Count != 0 ? stackedButton.Contains((int)j.posId) : true));
            }
            else if (selectedTab == 2)
            {
                temp = temp.Where(j => (selectedVendorsId.Count != 0 ? stackedButton.Contains((int)j.agentId) : true));
            }
            else if (selectedTab == 3)
            {
                temp = temp.Where(j => (selectedUserId.Count != 0 ? stackedButton.Contains((int)j.updateUserId) : true));
            }
            list = temp.ToList();

            return list;
        }

        private void fillRowChart(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<decimal> pTemp = null;
            IEnumerable<decimal> pbTemp = null;
            IEnumerable<decimal> resultTemp = null;

            var temp = invLst;
            if (selectedTab == 0)
            {
                temp = temp.Where(j => (selectedBranchId.Count != 0 ? stackedButton.Contains((int)j.branchCreatorId) : true));
                var result = temp.GroupBy(s => s.branchCreatorId).Select(s => new
                {
                    branchCreatorId = s.Key,
                    totalP = s.Where(x => x.invType == "p" || x.invType == "pw").Sum(x => x.totalNet),
                    totalPb = s.Where(x => x.invType == "pb").Sum(x => x.totalNet)
                }
             );
                var resultTotal = result.Select(x => new { x.branchCreatorId, total = x.totalP - x.totalPb }).ToList();
                pTemp = result.Select(x => (decimal)x.totalP);
                pbTemp = result.Select(x => (decimal)x.totalPb);
                resultTemp = result.Select(x => (decimal)x.totalP);
                var tempName = temp.GroupBy(s => s.branchCreatorName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            if (selectedTab == 1)
            {
                temp = temp.Where(j => (selectedPosId.Count != 0 ? stackedButton.Contains((int)j.posId) : true));
                var result = temp.GroupBy(s => s.posId).Select(s => new
                {
                    posId = s.Key,
                    totalP = s.Where(x => x.invType == "p" || x.invType == "pw").Sum(x => x.totalNet),
                    totalPb = s.Where(x => x.invType == "pb").Sum(x => x.totalNet)
                }
             );
                var resultTotal = result.Select(x => new { x.posId, total = x.totalP - x.totalPb }).ToList();
                pTemp = result.Select(x => (decimal)x.totalP);
                pbTemp = result.Select(x => (decimal)x.totalPb);
                resultTemp = result.Select(x => (decimal)x.totalP);
                var tempName = temp.GroupBy(s => s.posName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            if (selectedTab == 2)
            {
                temp = temp.Where(j => (selectedVendorsId.Count != 0 ? stackedButton.Contains((int)j.agentId) : true));
                var result = temp.GroupBy(s => s.agentId).Select(s => new
                {
                    agentId = s.Key,
                    totalP = s.Where(x => x.invType == "p" || x.invType == "pw").Sum(x => x.totalNet),
                    totalPb = s.Where(x => x.invType == "pb").Sum(x => x.totalNet)
                }
             );
                var resultTotal = result.Select(x => new { x.agentId, total = x.totalP - x.totalPb }).ToList();
                pTemp = result.Select(x => (decimal)x.totalP);
                pbTemp = result.Select(x => (decimal)x.totalPb);
                resultTemp = result.Select(x => (decimal)x.totalP);
                var tempName = temp.GroupBy(s => s.agentName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            if (selectedTab == 3)
            {
                temp = temp.Where(j => (selectedUserId.Count != 0 ? stackedButton.Contains((int)j.updateUserId) : true));
                var result = temp.GroupBy(s => s.createUserId).Select(s => new
                {
                    createUserId = s.Key,
                    totalP = s.Where(x => x.invType == "p" || x.invType == "pw").Sum(x => x.totalNet),
                    totalPb = s.Where(x => x.invType == "pb").Sum(x => x.totalNet)
                }
             );
                var resultTotal = result.Select(x => new { x.createUserId, total = x.totalP - x.totalPb }).ToList();
                pTemp = result.Select(x => (decimal)x.totalP);
                pbTemp = result.Select(x => (decimal)x.totalPb);
                resultTemp = result.Select(x => (decimal)x.totalP);
                var tempName = temp.GroupBy(s => s.uUserAccName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }

            SeriesCollection rowChartData = new SeriesCollection();
            List<decimal> purchase = new List<decimal>();
            List<decimal> returns = new List<decimal>();
            List<decimal> sub = new List<decimal>();
            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trTotalPurchases"),
                AppSettings.resourcemanager.GetString("trTotalReturn"),
                AppSettings.resourcemanager.GetString("trNetPurchases")
            };
            for (int i = 0; i < pbTemp.Count(); i++)
            {
                purchase.Add(pTemp.ToList().Skip(i).FirstOrDefault());
                returns.Add(pbTemp.ToList().Skip(i).FirstOrDefault());
                sub.Add(resultTemp.ToList().Skip(i).FirstOrDefault());
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            rowChartData.Add(
          new LineSeries
          {
              Values = purchase.AsChartValues(),
              Title = titles[0]
          }); ;
            rowChartData.Add(
         new LineSeries
         {
             Values = returns.AsChartValues(),
             Title = titles[1]
         });
            rowChartData.Add(
        new LineSeries
        {
            Values = sub.AsChartValues(),
            Title = titles[2]

        });
            DataContext = this;
            rowChart.Series = rowChartData;
        }

        IEnumerable<ItemTransferInvoice> itemTransfers = null;

        public void fillEvent()
        {
            reportQuery = filltoprint();

            //hide tax column if all values equal 0
            if (!AppSettings.invoiceTax_bool.Value)
                col_tax.Visibility = Visibility.Hidden;
            else
                col_tax.Visibility = Visibility.Visible;


            dgInvoice.ItemsSource = reportQuery;

            txt_count.Text = dgInvoice.Items.Count.ToString();

            ObservableCollection<int> selected = new ObservableCollection<int>();
            if (selectedTab == 0)
                selected = selectedBranchId;
            if (selectedTab == 1)
                selected = selectedPosId;
            if (selectedTab == 2)
                selected = selectedVendorsId;
            if (selectedTab == 3)
                selected = selectedUserId;

            fillPieChart(cb_branches, selected);
            fillColumnChart(cb_branches, selected);
            fillRowChart(cb_branches, selected);
        }


        #region Functions
        private void hideSatacks()
        {
            stk_tagsBranches.Visibility = Visibility.Collapsed;
            stk_tagsItems.Visibility = Visibility.Collapsed;
            stk_tagsPos.Visibility = Visibility.Collapsed;
            stk_tagsUsers.Visibility = Visibility.Collapsed;
            stk_tagsVendors.Visibility = Visibility.Collapsed;
        }

        public void paint()
        {
            //bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);
            bdr_branch.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_pos.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_vendors.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_users.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_items.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;

            path_branch.Fill = Brushes.White;
            path_pos.Fill = Brushes.White;
            path_vendors.Fill = Brushes.White;
            path_users.Fill = Brushes.White;
            path_items.Fill = Brushes.White;

            grid_branch.Visibility = Visibility.Visible;

            col_item.Visibility = Visibility.Collapsed;
            col_branch.Visibility = Visibility.Collapsed;
            col_pos.Visibility = Visibility.Collapsed;
            col_user.Visibility = Visibility.Collapsed;
            col_vendor.Visibility = Visibility.Collapsed;
        }

        #endregion


        private void hideAllColumn()
        {
            col_branch.Visibility = Visibility.Hidden;
            col_pos.Visibility = Visibility.Hidden;
            col_vendor.Visibility = Visibility.Hidden;
            col_agentCompany.Visibility = Visibility.Hidden;
            col_user.Visibility = Visibility.Hidden;
            col_item.Visibility = Visibility.Hidden;
            col_discount.Visibility = Visibility.Hidden;
            col_count.Visibility = Visibility.Hidden;
            col_itQuantity.Visibility = Visibility.Hidden;
            col_price.Visibility = Visibility.Hidden;
            col_totalNet.Visibility = Visibility.Hidden;
            col_tax.Visibility = Visibility.Hidden;
            col_subTotal.Visibility = Visibility.Hidden;
        }
        private void btn_branch_Click(object sender, RoutedEventArgs e)
        {//branches
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trBranchHint"));

                txt_search.Text = "";
                selectedTab = 0;

                hideSatacks();
                stk_tagsBranches.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_branch);
                path_branch.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hideAllColumn();
                col_branch.Visibility = Visibility.Visible;
                col_count.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;

                fillComboBranches();
                fillEvent();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void btn_pos_Click(object sender, RoutedEventArgs e)
        {//pos
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trPosHint"));

                txt_search.Text = "";
                selectedTab = 1;

                hideSatacks();
                stk_tagsPos.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_pos);
                path_pos.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hideAllColumn();
                col_branch.Visibility = Visibility.Visible;
                col_count.Visibility = Visibility.Visible;
                col_pos.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;

                fillComboPos();
                fillEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void btn_vendors_Click(object sender, RoutedEventArgs e)
        {//vendors
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trVendorHint"));

                txt_search.Text = "";
                selectedTab = 2;

                hideSatacks();
                stk_tagsVendors.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_vendors);
                path_vendors.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hideAllColumn();
                col_branch.Visibility = Visibility.Visible;
                col_vendor.Visibility = Visibility.Visible;
                col_agentCompany.Visibility = Visibility.Visible;
                col_discount.Visibility = Visibility.Visible;
                col_count.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;
                col_tax.Visibility = Visibility.Visible;

                fillComboVendors();
                fillEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void btn_users_Click(object sender, RoutedEventArgs e)
        {//users
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trUserHint"));

                txt_search.Text = "";
                selectedTab = 3;

                hideSatacks();
                stk_tagsUsers.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_users);
                path_users.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                hideAllColumn();
                col_branch.Visibility = Visibility.Visible;
                col_pos.Visibility = Visibility.Visible;
                col_user.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;
                col_discount.Visibility = Visibility.Visible;

                fillComboUsers();
                fillEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                
                HelpClass.StartAwait(grid_main);

                cb_branches.SelectedItem = null;
                chk_drafs.IsChecked = false;
                chk_return.IsChecked = false;
                chk_invoice.IsChecked = true;
                dp_endDate.SelectedDate = null;
                dp_startDate.SelectedDate = null;
                dt_startTime.SelectedTime = null;
                dt_endTime.SelectedTime = null;
                chk_allBranches.IsChecked = false;
                cb_branches.IsEnabled = true;

                if (selectedTab == 0)
                {
                    for (int i = 0; i < comboBrachTemp.Count; i++)
                        dynamicComboBranches.Add(comboBrachTemp.Skip(i).FirstOrDefault());

                    comboBrachTemp.Clear();
                    stk_tagsBranches.Children.Clear();
                    selectedBranchId.Clear();
                }

                else if (selectedTab == 1)
                {
                    for (int i = 0; i < comboPosTemp.Count; i++)
                        dynamicComboPoss.Add(comboPosTemp.Skip(i).FirstOrDefault());

                    comboPosTemp.Clear();
                    stk_tagsPos.Children.Clear();
                    selectedPosId.Clear();
                }

                else if (selectedTab == 2)
                {
                    for (int i = 0; i < comboVendorTemp.Count; i++)
                        dynamicComboVendors.Add(comboVendorTemp.Skip(i).FirstOrDefault());
                    comboVendorTemp.Clear();
                    stk_tagsVendors.Children.Clear();
                    selectedVendorsId.Clear();
                }
                else if (selectedTab == 3)
                {
                    for (int i = 0; i < comboUserTemp.Count; i++)
                        dynamicComboUsers.Add(comboUserTemp.Skip(i).FirstOrDefault());
                    comboUserTemp.Clear();
                    stk_tagsUsers.Children.Clear();
                    selectedUserId.Clear();
                }

                fillEvent();

                txt_search.Text = "";

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void selectionChangedCall(object sender)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);

                fillEvent();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void SelectionChanged(object sender, RoutedEventArgs e)
        {
            selectionChangedCall(sender);
        }

        #region settings
        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                List<string> Headers = new List<string>();
                List<string> Headers1 = new List<string>();
                foreach (var item in dgInvoice.Columns)
                {
                    Headers.Add(item.Header.ToString());
                }

                winControlPanel win = new winControlPanel(Headers);

                if (win.ShowDialog() == false)
                {
                    Headers1.Clear();
                    Headers1.AddRange(win.newHeaderResult);
                }
                for (int i = 0; i < Headers1.Count; i++)
                {
                    if (dgInvoice.Columns[i].Header.ToString() == Headers1[i])
                    {
                        dgInvoice.Columns[i].Visibility = Visibility.Visible;
                    }
                    else
                        dgInvoice.Columns[i].Visibility = Visibility.Hidden;
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


        private void Chip_OnDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                var currentChip = (Chip)sender;
                stk_tagsBranches.Children.Remove(currentChip);
                var m = comboBrachTemp.Where(j => j.branchId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                dynamicComboBranches.Add(m.FirstOrDefault());
                selectedBranchId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                if (selectedBranchId.Count == 0)
                {
                    cb_branches.SelectedItem = null;
                }
                fillEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chip_OnDeletePosClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                var currentChip = (Chip)sender;
                stk_tagsPos.Children.Remove(currentChip);
                var m = comboPosTemp.Where(j => j.posId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                dynamicComboPoss.Add(m.FirstOrDefault());
                selectedPosId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                if (selectedPosId.Count == 0)
                {
                    cb_branches.SelectedItem = null;
                }
                fillEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chip_OnDeleteVendorClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                var currentChip = (Chip)sender;
                stk_tagsVendors.Children.Remove(currentChip);
                var m = comboVendorTemp.Where(j => j.agentId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                dynamicComboVendors.Add(m.FirstOrDefault());
                selectedVendorsId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                if (selectedVendorsId.Count == 0)
                {
                    cb_branches.SelectedItem = null;
                }
                fillEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chip_OnDeleteUserClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                var currentChip = (Chip)sender;
                stk_tagsUsers.Children.Remove(currentChip);
                var m = comboUserTemp.Where(j => j.userId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                dynamicComboUsers.Add(m.FirstOrDefault());
                selectedUserId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                if (selectedUserId.Count == 0)
                {
                    cb_branches.SelectedItem = null;
                }
                fillEvent();


                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }



        private void cb_branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (cb_branches.SelectedItem != null)
                {

                    if (selectedTab == 0)
                    {
                        if (stk_tagsBranches.Children.Count < 5)
                        {
                            selectedBranch = cb_branches.SelectedItem as Branch;
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
                    else if (selectedTab == 1)
                    {
                        if (stk_tagsPos.Children.Count < 5)
                        {
                            selectedPos = cb_branches.SelectedItem as Pos;
                            var b = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedPos.name,
                                Name = "btn" + selectedPos.posId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            b.DeleteClick += Chip_OnDeletePosClick;
                            stk_tagsPos.Children.Add(b);
                            comboPosTemp.Add(selectedPos);
                            selectedPosId.Add(selectedPos.posId);
                            dynamicComboPoss.Remove(selectedPos);
                        }
                    }
                    else if (selectedTab == 2)
                    {
                        if (stk_tagsVendors.Children.Count < 5)
                        {
                            selectedVendor = cb_branches.SelectedItem as Agent;
                            var b = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedVendor.name,
                                Name = "btn" + selectedVendor.agentId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            b.DeleteClick += Chip_OnDeleteVendorClick;
                            stk_tagsVendors.Children.Add(b);
                            comboVendorTemp.Add(selectedVendor);
                            selectedVendorsId.Add(selectedVendor.agentId);
                            dynamicComboVendors.Remove(selectedVendor);
                        }
                    }
                    else if (selectedTab == 3)
                    {
                        if (stk_tagsUsers.Children.Count < 5)
                        {
                            selectedUser = cb_branches.SelectedItem as User;
                            var b = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedUser.username,
                                Name = "btn" + selectedUser.userId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            b.DeleteClick += Chip_OnDeleteUserClick;
                            stk_tagsUsers.Children.Add(b);
                            comboUserTemp.Add(selectedUser);
                            selectedUserId.Add(selectedUser.userId);
                            dynamicComboUsers.Remove(selectedUser);
                        }
                    }
                    fillEvent();

                }

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        bool isClickedAllBranches = false;
        private void chk_allBranches_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (!isClickedAllBranches)
                {
                    isClickedAllBranches = true;
                    cb_branches.SelectedItem = null;
                    cb_branches.IsEnabled = false;
                    if (selectedTab == 0)
                    {
                        for (int i = 0; i < comboBrachTemp.Count; i++)
                        {
                            dynamicComboBranches.Add(comboBrachTemp.Skip(i).FirstOrDefault());
                        }
                        comboBrachTemp.Clear();
                        stk_tagsBranches.Children.Clear();
                        selectedBranchId.Clear();
                    }
                    else if (selectedTab == 1)
                    {
                        for (int i = 0; i < comboPosTemp.Count; i++)
                        {
                            dynamicComboPoss.Add(comboPosTemp.Skip(i).FirstOrDefault());
                        }
                        comboPosTemp.Clear();
                        stk_tagsPos.Children.Clear();
                        selectedPosId.Clear();
                    }
                    else if (selectedTab == 2)
                    {
                        for (int i = 0; i < comboVendorTemp.Count; i++)
                        {
                            dynamicComboVendors.Add(comboVendorTemp.Skip(i).FirstOrDefault());
                        }
                        comboVendorTemp.Clear();
                        stk_tagsVendors.Children.Clear();
                        selectedVendorsId.Clear();
                    }
                    else if (selectedTab == 3)
                    {
                        for (int i = 0; i < comboUserTemp.Count; i++)
                        {
                            dynamicComboUsers.Add(comboUserTemp.Skip(i).FirstOrDefault());
                        }
                        comboUserTemp.Clear();
                        stk_tagsUsers.Children.Clear();
                        selectedUserId.Clear();
                    }
                }
                else
                {
                    cb_branches.IsEnabled = true;
                    isClickedAllBranches = false;
                }
                fillEvent();

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        Invoice invoice;
        private async void DgInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                invoice = new Invoice();
                if (dgInvoice.SelectedIndex != -1)
                {
                    ItemTransferInvoice item = dgInvoice.SelectedItem as ItemTransferInvoice;
                    if (item.invoiceId > 0)
                    {
                        invoice = await invoice.GetByInvoiceId(item.invoiceId);
                        MainWindow.mainWindow.Btn_purchase_Click(MainWindow.mainWindow.btn_purchase, null);
                        //uc_purchases.Instance.UserControl_Loaded(null, null);
                        //uc_purchases.Instance.btn_payInvoice_Click(uc_purchases.Instance.btn_payInvoice, null);

                        MainWindow.mainWindow.initializationMainTrack("payInvoice");

                        MainWindow.mainWindow.grid_main.Children.Clear();
                        MainWindow.mainWindow.grid_main.Children.Add(uc_payInvoice.Instance);

                        //uc_payInvoice.Instance.UserControl_Loaded(uc_payInvoice.Instance, null);
                        uc_payInvoice._InvoiceType = invoice.invType;
                        uc_payInvoice.Instance.invoice = invoice;
                        uc_payInvoice.isFromReport = true;
                        if (item.archived == 0)
                            uc_payInvoice.archived = false;
                        else
                            uc_payInvoice.archived = true;
                        await uc_payInvoice.Instance.fillInvoiceInputs(invoice);
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
        #region report
        //prin & pdf
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
      //  List<ItemTransferInvoice> query = new List<ItemTransferInvoice>();
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
                    addpath = @"\Reports\StatisticReport\Purchase\Ar\ArInvoiceBranch.rdlc";
                    secondTitle = "branch";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\Ar\ArInvoicePos.rdlc";
                    secondTitle = "pos";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 2)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\Ar\ArInvoiceVendor.rdlc";
                    //    paramarr.Add(new ReportParameter("isTax", MainWindow.tax.ToString()));
                    secondTitle = "vendors";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 3)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\Ar\ArInvoiceUser.rdlc";
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
                    addpath = @"\Reports\StatisticReport\Purchase\En\EnInvoiceBranch.rdlc";
                    secondTitle = "branch";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\En\EnInvoicePos.rdlc";
                    secondTitle = "pos";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 2)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\En\EnInvoiceVendor.rdlc";
                    // paramarr.Add(new ReportParameter("isTax", MainWindow.tax.ToString()));
                    secondTitle = "vendors";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 3)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\En\EnInvoiceUser.rdlc";
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
            Title = AppSettings.resourcemanagerreport.GetString("trPurchasesReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            clsReports.PurInvStsReport(reportQuery, rep, reppath, paramarr);
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




        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (selectedTab == 0)
                {
                    itemTransfers = invLst.Where(j => (selectedBranchId.Count != 0 ? selectedBranchId.Contains((int)j.branchCreatorId) : true));

                    reportQuery = itemTransfers.Where(s => (s.branchCreatorName.ToLower().Contains(txt_search.Text) || s.invNumber.ToLower().Contains(txt_search.Text)));
                }
                else if (selectedTab == 1)
                {
                    itemTransfers = invLst.Where(j => (selectedPosId.Count != 0 ? selectedPosId.Contains((int)j.posId) : true));

                    reportQuery = itemTransfers.Where(s => (s.branchCreatorName.ToLower().Contains(txt_search.Text) || s.posName.ToLower().Contains(txt_search.Text) ||
                                                            s.invNumber.ToLower().Contains(txt_search.Text)));
                }
                else if (selectedTab == 2)
                {
                    itemTransfers = invLst.Where(j => (selectedVendorsId.Count != 0 ? selectedVendorsId.Contains((int)j.agentId) : true));

                    reportQuery = itemTransfers
                        .Where(s => (s.branchCreatorName.ToLower().Contains(txt_search.Text) || s.agentName.ToLower().Contains(txt_search.Text) ||
                                     s.agentCompany.ToLower().Contains(txt_search.Text) || s.invNumber.ToLower().Contains(txt_search.Text)));
                }
                else if (selectedTab == 3)
                {
                    itemTransfers = invLst.Where(j => (selectedUserId.Count != 0 ? selectedUserId.Contains((int)j.updateUserId) : true));

                    reportQuery = itemTransfers.Where(s => (s.branchCreatorName.ToLower().Contains(txt_search.Text) || s.posName.ToLower().Contains(txt_search.Text) ||
                                                            s.uUserAccName.ToLower().Contains(txt_search.Text) || s.invNumber.ToLower().Contains(txt_search.Text)));
                }

                dgInvoice.ItemsSource = reportQuery;

                txt_count.Text = dgInvoice.Items.Count.ToString();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

   

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChangedCall(sender);
        }
    }
}
