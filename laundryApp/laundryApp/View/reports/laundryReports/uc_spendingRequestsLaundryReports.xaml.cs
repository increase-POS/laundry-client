using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using laundryApp.Classes;
using laundryApp.View.kitchen;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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

namespace laundryApp.View.reports.kitchenReports
{
    /// <summary>
    /// Interaction logic for uc_spendingRequestsLaundryReports.xaml
    /// </summary>
    public partial class uc_spendingRequestsLaundryReports : UserControl
    {
        IEnumerable<ItemTransferInvoice> spendingRequests;
        Statistics statisticsModel = new Statistics();
        IEnumerable<ItemTransferInvoice> spendingRequestsQuery;

        string searchText = "";
        int selectedTab = 0;
        //report
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private static uc_spendingRequestsLaundryReports _instance;
        public static uc_spendingRequestsLaundryReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_spendingRequestsLaundryReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_spendingRequestsLaundryReports()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                chk_normal.IsChecked = true;

                Btn_invoice_Click(btn_invoice, null);

                //HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), btn_invoice.Tag.ToString());

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        private async void Btn_invoice_Click(object sender, RoutedEventArgs e)
        {//invoices
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                hideAllColumns();
                dkp_chks.Visibility = Visibility.Visible;
                row_chks.Height = row_cmb.Height;
                selectedTab = 0;
                spendingRequests = null;

                col_invNum.Visibility = Visibility.Visible;
                col_count.Visibility = Visibility.Visible;
                col_invDate.Visibility = Visibility.Visible;
                col_branch.Visibility = Visibility.Visible;
                col_details.Visibility = Visibility.Visible;

                txt_search.Text = "";

                path_item.Fill = Brushes.White;
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_invoice);
                path_invoice.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                chk_allBranches.IsChecked = true;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_item_Click(object sender, RoutedEventArgs e)
        {//items
            try
            {
                HelpClass.StartAwait(grid_main);
                hideAllColumns();
                dkp_chks.Visibility = Visibility.Collapsed;
                row_chks.Height = new GridLength(0);
                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                selectedTab = 1;
                spendingRequests = null;

                chk_allBranches.IsChecked = true;

                col_itemName.Visibility = Visibility.Visible;
                col_unitName.Visibility = Visibility.Visible;
                col_quantity.Visibility = Visibility.Visible;
                col_branch.Visibility = Visibility.Visible;

                txt_search.Text = "";
                path_invoice.Fill = Brushes.White;
                bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_item);
                path_item.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region methods
        private void hideAllColumns()
        {
            for (int i = 0; i < dg_request.Columns.Count; i++)
                dg_request.Columns[i].Visibility = Visibility.Hidden;
        }

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
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_endDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            chk_allBranches.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_normal.Content = AppSettings.resourcemanager.GetString("trNormal");
            chk_return.Content = AppSettings.resourcemanager.GetString("trReturn");

            tt_invoice.Content = AppSettings.resourcemanager.GetString("trInvoices");
            tt_item.Content = AppSettings.resourcemanager.GetString("trItems");
            //items
            col_invNum.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_itemName.Header = AppSettings.resourcemanager.GetString("trItem");
            col_unitName.Header = AppSettings.resourcemanager.GetString("trUnit");
            col_quantity.Header = AppSettings.resourcemanager.GetString("trQTR");
            //invoice
            col_invDate.Header = AppSettings.resourcemanager.GetString("trDate");
            col_count.Header = AppSettings.resourcemanager.GetString("trCount");
            col_invDate.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_details.Header = AppSettings.resourcemanager.GetString("trDetails");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }
        async Task<IEnumerable<ItemTransferInvoice>> RefreshSpendingRequestsList()
        {
            if (selectedTab == 0)
                spendingRequests = await statisticsModel.GetSpendingRequest(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
            else if (selectedTab == 1)
                spendingRequests = await statisticsModel.GetSpendingItems(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);

            return spendingRequests;
        }

        IEnumerable<ItemTransferInvoice> requestsTemp = null;

        async Task Search()
        {
            try
            {
                if(spendingRequests == null)
                    await RefreshSpendingRequestsList();

                searchText = txt_search.Text.ToLower();

                requestsTemp = spendingRequests.Where(p =>
                (dp_startDate.SelectedDate != null ? p.updateDate >= dp_startDate.SelectedDate : true)
                &&
                //end date
                (dp_endDate.SelectedDate != null ? p.updateDate <= dp_endDate.SelectedDate : true)
                );

                if (selectedTab == 0) await SearchInvoice();
                else if (selectedTab == 1) await SearchItem();

                RefreshSpendingRequestsView();
                fillBranches();
                fillColumnChart();
                fillPieChart();
                fillRowChart();
            }
            catch { }
        }

        async Task SearchInvoice()
        {
            spendingRequestsQuery = requestsTemp
            .Where(s =>
            (
            s.invNumber.ToLower().Contains(searchText)
            ||
            s.branchName.ToString().ToLower().Contains(searchText)
            )
            &&
            //branchID
            (
                cb_branches.SelectedIndex != -1 ? s.branchId == Convert.ToInt32(cb_branches.SelectedValue) : true
            )
            &&
            (
            //normal
            (
                chk_normal.IsChecked == true ?  s.invType == "sr" : false
            )
            ||
            //return
            (
                chk_return.IsChecked == true ? s.invType == "srb" : false
            )
            )
            );

        }

        async Task SearchItem()
        {
            var quantitiesNormal = requestsTemp.GroupBy(s => s.ITitemUnitId).Select(inv => new {
                ITquantity = inv.Where(m => m.invType == "sr").Sum(p => p.ITquantity.Value),
            }).ToList();
            var quantitiesReturn = requestsTemp.GroupBy(s => s.ITitemUnitId).Select(inv => new {
                ITquantity = inv.Where(m => m.invType == "srb").Sum(p => p.ITquantity.Value),
            }).ToList();

            requestsTemp = requestsTemp.GroupBy(s => s.ITitemUnitId).SelectMany(inv => inv.Take(1)).ToList();

            spendingRequestsQuery = requestsTemp
            .Where(s =>
            (
            s.invNumber.ToLower().Contains(searchText)
            ||
            s.ITitemName.ToLower().Contains(searchText)
            ||
            s.ITunitName.ToLower().Contains(searchText)
            ||
            s.ITquantity.ToString().ToLower().Contains(searchText)
            ||
            s.branchName.ToString().ToLower().Contains(searchText)
            )
            &&
            //branchID
            (
                cb_branches.SelectedIndex != -1 ? s.branchId == Convert.ToInt32(cb_branches.SelectedValue) : true
            )
            );

            int i = 0;
            foreach (var x in spendingRequestsQuery)
            {
                x.ITquantity = quantitiesNormal[i].ITquantity - quantitiesReturn[i].ITquantity;
                i++;
            }

            spendingRequestsQuery = spendingRequestsQuery.Where(s => s.ITquantity != 0).ToList();
        }


        void RefreshSpendingRequestsView()
        {
            dg_request.ItemsSource = spendingRequestsQuery;
            txt_count.Text = spendingRequestsQuery.Count().ToString();
        }

        private void fillBranches()
        {
            cb_branches.SelectedValuePath = "branchId";
            cb_branches.DisplayMemberPath = "branchName";
            cb_branches.ItemsSource = spendingRequests.GroupBy(g => g.branchId).Select(i => new { i.FirstOrDefault().branchName, i.FirstOrDefault().branchId });
        }

        #endregion

        #region events
        private async void detailsRowinDatagrid(object sender, RoutedEventArgs e)
        {//details
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        ItemTransferInvoice row = (ItemTransferInvoice)dg_request.SelectedItems[0];
                        if(row.invoiceId > 0)
                        {
                            Invoice invoice = new Invoice();
                            invoice = await invoice.GetByInvoiceId(row.invoiceId);

                            MainWindow.mainWindow.Btn_kitchen_Click(MainWindow.mainWindow.btn_kitchen,null);
                            MainWindow.mainWindow.initializationMainTrack("spendingRequests");
                            MainWindow.mainWindow.grid_main.Children.Clear();
                            MainWindow.mainWindow.grid_main.Children.Add(uc_spendingRequest.Instance);
                            uc_spendingRequest.Instance.invoice = invoice;
                            uc_spendingRequest._InvoiceType = invoice.invType;
                            uc_spendingRequest._invoiceId = invoice.invoiceId;
                            uc_spendingRequest.isFromReport = true;
                            uc_spendingRequest.archived = false;
                            //setNotifications();
                            await uc_spendingRequest.Instance.fillInvoiceInputs(invoice);
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

        private void Chk_Checked(object sender, RoutedEventArgs e)
        {
            callSearch(sender);
        }

        private void RefreshView_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            callSearch(sender);
        }

        private void cb_branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            callSearch(sender);
        }

        private void Chk_allBranches_Checked(object sender, RoutedEventArgs e)
        {//select all branches
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_branches.SelectedIndex = -1;
                cb_branches.IsEnabled = false;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private async void Chk_allBranches_Unchecked(object sender, RoutedEventArgs e)
        {//unselect all branches
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

        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            callSearch(sender);
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            searchText = "";
            txt_search.Text = "";
            chk_allBranches.IsChecked = true;
            dp_startDate.SelectedDate = null;
            dp_endDate.SelectedDate = null;
            callSearch(sender);
        }
        #endregion

        #region charts
        private void fillColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<int> requestNormalLst = null;
            IEnumerable<int> requestReturnLst = null;

            var temp = spendingRequestsQuery;

            int count = 0;
            
            var tempName = temp.GroupBy(s => s.branchId).Select(s => new
            {
                branchName = s.FirstOrDefault().branchName 
            });
            count = tempName.Count();
            names.AddRange(tempName.Select(nn => nn.branchName));

            //invoice
            if (selectedTab == 0)
            {
                var tempRequestsLst = temp.GroupBy(s => s.branchId).Select(s => new
                {
                    reqNormal = s.Where(m => m.invType == "sr").Count(),
                    reqReturn = s.Where(m => m.invType == "srb").Count(),
                });

                requestNormalLst = tempRequestsLst.Select(m => m.reqNormal);
                requestReturnLst = tempRequestsLst.Select(m => m.reqReturn);

                List<string> lable = new List<string>();
                SeriesCollection columnChartData = new SeriesCollection();

                List<decimal> cNormal = new List<decimal>();
                List<decimal> cReturn = new List<decimal>();

                List<string> titles = new List<string>()
                {
                   AppSettings.resourcemanager.GetString("trNormal") ,
                   AppSettings.resourcemanager.GetString("trReturn")
                };
                int x = 6;
                if (count <= 6) x = count;
                for (int i = 0; i < x; i++)
                {
                    cNormal.Add(requestNormalLst.ToList().Skip(i).FirstOrDefault());
                    cReturn.Add(requestReturnLst.ToList().Skip(i).FirstOrDefault());

                    axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
                }

                if (count > 6)
                {
                    decimal reqNormalSum = 0, reqReturnSum = 0;
                    for (int i = 6; i < count; i++)
                    {
                        reqNormalSum = reqNormalSum + requestNormalLst.ToList().Skip(i).FirstOrDefault();
                        reqReturnSum = reqReturnSum + requestReturnLst.ToList().Skip(i).FirstOrDefault();
                    }
                    if (!((reqNormalSum == 0) && (reqReturnSum == 0)))
                    {
                        cNormal.Add(reqNormalSum);
                        cReturn.Add(reqReturnSum);

                        axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                    }
                }
                columnChartData.Add(
                    new StackedColumnSeries
                    {
                        Values = cNormal.AsChartValues(),
                        Title = titles[0],
                        DataLabels = true,
                    });
                columnChartData.Add(
                   new StackedColumnSeries
                   {
                       Values = cReturn.AsChartValues(),
                       Title = titles[1],
                       DataLabels = true,
                   });
                DataContext = this;
                cartesianChart.Series = columnChartData;
            }
            //items
            else if (selectedTab == 1)
            {
                var tempRequestsLst = temp.GroupBy(s => s.branchId).Select(s => new
                {
                    reqNormal = s.Count()
                });

                requestNormalLst = tempRequestsLst.Select(m => m.reqNormal);

                List<string> lable = new List<string>();
                SeriesCollection columnChartData = new SeriesCollection();

                List<decimal> cNormal = new List<decimal>();

                List<string> titles = new List<string>()
                {
                   AppSettings.resourcemanager.GetString("trItems") ,
                };
                int x = 6;
                if (count <= 6) x = count;
                for (int i = 0; i < x; i++)
                {
                    cNormal.Add(requestNormalLst.ToList().Skip(i).FirstOrDefault());

                    axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
                }

                if (count > 6)
                {
                    decimal reqNormalSum = 0;
                    for (int i = 6; i < count; i++)
                    {
                        reqNormalSum = reqNormalSum + requestNormalLst.ToList().Skip(i).FirstOrDefault();
                    }
                    if (reqNormalSum != 0)
                    {
                        cNormal.Add(reqNormalSum);

                        axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                    }
                }
                columnChartData.Add(
                    new StackedColumnSeries
                    {
                        Values = cNormal.AsChartValues(),
                        Title = titles[0],
                        DataLabels = true,
                    });
                
                DataContext = this;
                cartesianChart.Series = columnChartData;
            }
        }

        private void fillPieChart()
        {
            List<string> titles = new List<string>();
            IEnumerable<int> x = null;
            titles.Clear();

            var temp = spendingRequestsQuery;

            if (selectedTab == 0)
            {
                var titleTemp = temp.GroupBy(m => m.branchName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.branchName).Select(s => new { branchName = s.Key, sum = s.Sum(g => (int)g.count) });
                x = result.Select(m => m.sum);

                SeriesCollection piechartData = new SeriesCollection();

                int xCount = 6;
                if (x.Count() <= 6) xCount = x.Count();
                for (int i = 0; i < xCount; i++)
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

                if (x.Count() > 6)
                {
                    int xSum = 0;
                    for (int i = 6; i < x.Count(); i++)
                    {
                        xSum = xSum + x.ToList().Skip(i).FirstOrDefault();
                    }

                    if (xSum > 0)
                    {
                        List<int> final = new List<int>();
                        List<string> lable = new List<string>();
                        final.Add(xSum);
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
            else if (selectedTab == 1)
            {
                var titleTemp = temp.GroupBy(m => m.ITitemUnitName1);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.ITitemUnitName1).Select(s => new { ITitemUnitName1 = s.Key, sum = s.Sum(g => (int)g.ITquantity) });
                x = result.Select(m => m.sum);

                SeriesCollection piechartData = new SeriesCollection();

                int xCount = 6;
                if (x.Count() <= 6) xCount = x.Count();
                for (int i = 0; i < xCount; i++)
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

                if (x.Count() > 6)
                {
                    int xSum = 0;
                    for (int i = 6; i < x.Count(); i++)
                    {
                        xSum = xSum + x.ToList().Skip(i).FirstOrDefault();
                    }

                    if (xSum > 0)
                    {
                        List<int> final = new List<int>();
                        List<string> lable = new List<string>();
                        final.Add(xSum);
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
        }

        private void fillRowChart()
        {
            int endYear = DateTime.Now.Year;
            int startYear = endYear - 1;
            int startMonth = DateTime.Now.Month;
            int endMonth = startMonth;
            if (dp_startDate.SelectedDate != null && dp_endDate.SelectedDate != null)
            {
                startYear = dp_startDate.SelectedDate.Value.Year;
                endYear = dp_endDate.SelectedDate.Value.Year;
                startMonth = dp_startDate.SelectedDate.Value.Month;
                endMonth = dp_endDate.SelectedDate.Value.Month;
            }


            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();

            SeriesCollection rowChartData = new SeriesCollection();

            var tempName = spendingRequestsQuery.GroupBy(s => new { s.ITitemUnitId }).Select(s => new
            {
                Name = s.FirstOrDefault().updateDate,
            });
            names.AddRange(tempName.Select(nn => nn.Name.ToString()));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> orderLst = new List<int>();

            if (endYear - startYear <= 1)
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = startMonth; month <= 12; month++)
                    {
                        var firstOfThisMonth = new DateTime(year, month, 1);
                        var firstOfNextMonth = firstOfThisMonth.AddMonths(1);
                        if (selectedTab == 0)
                        {
                            var drawQuantity = spendingRequestsQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Select(s => s.ITitemUnitName1).Count();
                            orderLst.Add(drawQuantity);
                        }
                        else if (selectedTab == 1)
                        {
                            var drawQuantity = spendingRequestsQuery.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Count();
                            orderLst.Add(drawQuantity);
                        }
                        MyAxis.Labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + "/" + year);

                        if (year == endYear && month == endMonth)
                        {
                            break;
                        }
                        if (month == 12)
                        {
                            startMonth = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var firstOfThisYear = new DateTime(year, 1, 1);
                    var firstOfNextMYear = firstOfThisYear.AddYears(1);
                    if (selectedTab == 0)
                    {
                        var drawQuantity = spendingRequestsQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Select(s => s.ITitemUnitName1).Count();
                        orderLst.Add(drawQuantity);
                    }
                    else if (selectedTab == 1)
                    {
                        var drawQuantity = spendingRequestsQuery.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear).Count();
                        orderLst.Add(drawQuantity);
                    }
                    MyAxis.Labels.Add(year.ToString());
                }
            }
            string _title = "";
            if (selectedTab == 0)
                _title = AppSettings.resourcemanager.GetString("trSpendingRequest") + "/" + AppSettings.resourcemanager.GetString("trCount");
            else if(selectedTab == 1)
                _title = AppSettings.resourcemanager.GetString("trItem") + "/" + AppSettings.resourcemanager.GetString("trCount");

            rowChartData.Add(
          new LineSeries
          {
              Values = orderLst.AsChartValues(),
              Title = _title
          });

            DataContext = this;
            rowChart.Series = rowChartData;
        }

        #endregion

        

        #region reports
        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            string firstTitle = "SpendingRequests";//trSpendingRequests
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Kitchen\Ar\ArSpendingInvoice.rdlc";
                    secondTitle = "invoice";
                }
                else
                {
                    addpath = @"\Reports\StatisticReport\Kitchen\Ar\ArSpendingItem.rdlc";
                    secondTitle = "items";
                }

                //Reports\StatisticReport\Sale\Daily\Ar
            }
            else
            {
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Kitchen\En\EnSpendingInvoice.rdlc";
                    secondTitle = "invoice";
                }
                else
                {
                    addpath = @"\Reports\StatisticReport\Kitchen\En\EnSpendingItem.rdlc";
                    secondTitle = "items";
                }
            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
           
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            Title = AppSettings.resourcemanagerreport.GetString("trKitchen") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            clsReports.spendingRequestReport(spendingRequestsQuery, rep, reppath, paramarr);//PreparingOrders

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
                LocalReportExtensions.PrintToPrinter(rep);
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

    }
}
