using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using laundryApp.Classes;
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

namespace laundryApp.View.reports.salesReports
{
    /// <summary>
    /// Interaction logic for uc_membershipSalesReports.xaml
    /// </summary>
    public partial class uc_membershipSalesReports : UserControl
    {

        IEnumerable<SalesMembership> memberships;
        Statistics statisticsModel = new Statistics();
        IEnumerable<SalesMembership> membershipsQuery;

        //prin & pdf
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        string searchText = "";

        private static uc_membershipSalesReports _instance;
        public static uc_membershipSalesReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_membershipSalesReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public uc_membershipSalesReports()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                #region translate
                if (AppSettings.lang.Equals("en"))
                grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                chk_allBranches.IsChecked = true;

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), btn_membership.Tag.ToString());

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

        #region methods
        private void translate()
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_endDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branches, AppSettings.resourcemanager.GetString("trBranch") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_membership, AppSettings.resourcemanager.GetString("membership") + "...");

            chk_allBranches.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allMemberships.Content = AppSettings.resourcemanager.GetString("trAll");

            tt_membership.Content = AppSettings.resourcemanager.GetString("membership");

            col_invNum.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_customerName.Header = AppSettings.resourcemanager.GetString("trCustomer");
            col_membership.Header = AppSettings.resourcemanager.GetString("membership");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_details.Header = AppSettings.resourcemanager.GetString("trDetails");
            col_discount.Header = AppSettings.resourcemanager.GetString("trDiscount");
            col_print.Header = AppSettings.resourcemanager.GetString("trPrint");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }

        async Task Search()
        {

            if (memberships is null)
                await RefreshMembershipsList();

            searchText = txt_search.Text.ToLower();
            membershipsQuery = memberships
                .Where(s =>
            (
             s.invNumber.ToLower().Contains(searchText)
            ||
            (s.invBarcode != null ? s.invBarcode.ToLower().Contains(searchText) : false)
            ||
            s.branchName.ToLower().Contains(searchText)
            ||
            s.membershipsName.ToLower().Contains(searchText)
            ||
            s.agentName.ToLower().Contains(searchText)
            )
            &&
            //branch
            (cb_branches.SelectedIndex != -1 ? s.branchId == Convert.ToInt32(cb_branches.SelectedValue) : true)
            &&
            //membership
            (cb_membership.SelectedIndex != -1 ? s.membershipId == Convert.ToInt32(cb_membership.SelectedValue) : true)
            &&
            //start date
            (dp_startDate.SelectedDate != null ? s.invDate >= dp_startDate.SelectedDate : true)
            &&
            //end date
            (dp_endDate.SelectedDate != null ? s.invDate <= dp_endDate.SelectedDate : true)
            );

            RefreshMembershipsView();

            fillColumnChart();
            fillPieChart();
            fillRowChart();

        }

        void RefreshMembershipsView()
        {
            dg_memberships.ItemsSource = membershipsQuery;
            txt_count.Text = membershipsQuery.Count().ToString();
        }

        async Task<IEnumerable<SalesMembership>> RefreshMembershipsList()
        {
            memberships = await statisticsModel.GetSaleMembership(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
            fillBranches();
            fillMemberships();
            return memberships;
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
        private void fillBranches()
        {
            cb_branches.SelectedValuePath = "branchId";
            cb_branches.DisplayMemberPath = "branchName";
            cb_branches.ItemsSource = memberships.Select(i => new { i.branchName, i.branchId }).Distinct();
        }
        private void fillMemberships()
        {
            cb_membership.SelectedValuePath = "membershipId";
            cb_membership.DisplayMemberPath = "membershipsName";
            cb_membership.ItemsSource = memberships.Select(i => new { i.membershipsName, i.membershipId }).Distinct();
        }
        #endregion

        #region events
        private void RefreshView_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            callSearch(sender);
        }

        private async void cb_branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select branch
            callSearch(sender);
        }

        private void Chk_allBranches_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_branches.SelectedIndex = -1;
                cb_branches.IsEnabled = false;

                chk_allMemberships.IsChecked = true;

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

        private void Cb_membership_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            callSearch(sender);
        }

        private async void Chk_allMemberships_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_membership.SelectedIndex = -1;
                cb_membership.IsEnabled = false;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Chk_allMemberships_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_membership.IsEnabled = true;

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
        {
            callSearch(sender);
        }

        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = "";
                txt_search.Text = "";
                await RefreshMembershipsList();
                dp_startDate.SelectedDate = null;
                dp_endDate.SelectedDate = null;
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

        #region print datagrid btns

        int cashTransID = 0, openCashTransID = 0;
        IEnumerable<OpenClosOperatinModel> opquery;
        POSOpenCloseModel openclosrow = new POSOpenCloseModel();
        public async Task<IEnumerable<OpenClosOperatinModel>> getopquery(POSOpenCloseModel ocrow)
        {

            Statistics statisticsModel = new Statistics();

            opquery = await statisticsModel.GetTransBetweenOpenClose((int)ocrow.openCashTransId, ocrow.cashTransId);
            opquery = opquery.Where(c => c.transType != "c" && c.transType != "o");

            openclosrow = ocrow;
            return opquery;
        }
        private void BuildDetailReport(SalesMembership salesMembership)
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            string firstTitle = "membership";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {

                addpath = @"\Reports\StatisticReport\Sale\Membership\Ar\ArDiscountDetails.rdlc";
            }
            else
            {
                addpath = @"\Reports\StatisticReport\Sale\Membership\En\EnDiscountDetails.rdlc";
            }

            secondTitle = "discounts";// trOperations
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("SalesReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            /*
                 public List<CouponInvoice> CouponInvoiceList = new List<CouponInvoice>();
           public List<ItemTransfer> itemsTransferList = new List<ItemTransfer>();
           public List<InvoicesClass> invoiceClassDiscountList = new List<InvoicesClass>();
             * */
            clsReports.membershiptDiscountReport(salesMembership, rep, reppath, paramarr, openclosrow);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        private void pdfRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesMembership row = (SalesMembership)dg_memberships.SelectedItems[0];
                       
                        if (row == null)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            BuildDetailReport(row);

                            saveFileDialog.Filter = "PDF|*.pdf;";

                            if (saveFileDialog.ShowDialog() == true)
                            {
                                string filepath = saveFileDialog.FileName;
                                LocalReportExtensions.ExportToPDF(rep, filepath);
                            }
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

        private void printRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesMembership row = (SalesMembership)dg_memberships.SelectedItems[0];

                        if (row == null)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {
                            string pdfpath = "";

                            pdfpath = @"\Thumb\report\temp.pdf";
                            pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                            BuildDetailReport(row);

                            LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
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

        private void excelRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesMembership row = (SalesMembership)dg_memberships.SelectedItems[0];

                        if (row == null)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {

                            BuildDetailReport(row);

                            this.Dispatcher.Invoke(() =>
                            {
                                saveFileDialog.Filter = "EXCEL|*.xls;";
                                if (saveFileDialog.ShowDialog() == true)
                                {
                                    string filepath = saveFileDialog.FileName;
                                    LocalReportExtensions.ExportToExcel(rep, filepath);
                                }
                            });
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

        private void previewRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesMembership row = (SalesMembership)dg_memberships.SelectedItems[0];
                       
                        if (row == null)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {
                            string pdfpath = "";

                            pdfpath = @"\Thumb\report\temp.pdf";
                            pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                            BuildDetailReport(row);
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

        #region membership datagrid btns
        
        private void couponsRowinDatagrid(object sender, RoutedEventArgs e)
        {//coupons
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesMembership row = (SalesMembership)dg_memberships.SelectedItems[0];

                        if (row.CouponInvoiceList.Count > 0)
                        {
                            Window.GetWindow(this).Opacity = 0.2;

                            wd_membershipListForReports w = new wd_membershipListForReports();

                            w.membershipType = "c";
                            w.CouponInvoiceList = row.CouponInvoiceList;
                            w.ShowDialog();

                            Window.GetWindow(this).Opacity = 1;
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("noCoupons"), animation: ToasterAnimation.FadeIn);
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void offersRowinDatagrid(object sender, RoutedEventArgs e)
        {//offers
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesMembership row = (SalesMembership)dg_memberships.SelectedItems[0];

                        if (row.itemsTransferList.Count > 0)
                        {
                            Window.GetWindow(this).Opacity = 0.2;

                            wd_membershipListForReports w = new wd_membershipListForReports();

                            w.membershipType = "o";
                            w.itemsTransferList = row.itemsTransferList;
                            w.ShowDialog();

                            Window.GetWindow(this).Opacity = 1;
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("noOffers"), animation: ToasterAnimation.FadeIn);
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void invoicesClassesRowinDatagrid(object sender, RoutedEventArgs e)
        {//invoices
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        SalesMembership row = (SalesMembership)dg_memberships.SelectedItems[0];

                        if (row.invoiceClassDiscountList.Count > 0)
                        {
                            Window.GetWindow(this).Opacity = 0.2;

                            wd_membershipListForReports w = new wd_membershipListForReports();

                            w.membershipType = "i";
                            w.invoiceClassDiscountList = row.invoiceClassDiscountList;
                            w.ShowDialog();

                            Window.GetWindow(this).Opacity = 1;
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("noInvoicesClasses"), animation: ToasterAnimation.FadeIn);
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

        #region charts
        private void fillColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();

            var tempName = membershipsQuery.GroupBy(s => s.branchName).Select(s => new
            {
                name = s.Key
            });
            names.AddRange(tempName.Select(nn => nn.name));

            var tempMembership = membershipsQuery.GroupBy(s => s.membershipsName).Select(s => new
            {
                name = s.Key
            });
            foreach (var m in tempMembership)
            {
                List<int> membershipCount = new List<int>();

                #region 
                var result = membershipsQuery.GroupBy(s => s.branchId).Select(s => new
                {
                    branchId = s.Key,
                    quantity = s.Where(n => n.membershipsName == m.name).Count()
                });

                membershipCount.AddRange(result.Select(nn => nn.quantity));

                List<string> lable = new List<string>();
                List<int> cS = new List<int>();

                List<string> titles = new List<string>();
               
                titles.Add(m.name);

                int x = 6;
                if (names.Count() <= 6) x = names.Count();

                for (int i = 0; i < x; i++)
                {
                    cS.Add(membershipCount.ToList().Skip(i).FirstOrDefault());
                    axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
                }

                if (names.Count() > 6)
                {
                    int membershipSum = 0;
                    for (int i = 6; i < names.Count(); i++)
                        membershipSum = membershipSum + membershipCount.ToList().Skip(i).FirstOrDefault();

                    if (membershipSum != 0)
                        cS.Add(membershipSum);

                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }

                columnChartData.Add(
                new StackedColumnSeries
                {
                    Values = cS.AsChartValues(),
                    Title = titles[0],
                    DataLabels = true,
                });
                #endregion
            }
            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillPieChart()
        {
            List<string> titles = new List<string>();
            IEnumerable<int> x = null;
            titles.Clear();

            var temp = membershipsQuery;

            var titleTemp = temp.GroupBy(m => m.membershipsName);
            titles.AddRange(titleTemp.Select(jj => jj.Key));
            var result = temp.GroupBy(s => s.membershipId).Select(s => new { membershipId = s.Key, quantity = s.Count() });
            x = result.Select(m => m.quantity);

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
            List<CashTransferSts> resultList = new List<CashTransferSts>();

            SeriesCollection rowChartData = new SeriesCollection();

            var tempName = membershipsQuery.GroupBy(s => new { s.invNumber }).Select(s => new
            {
                Name = s.FirstOrDefault().invDate,
            });
            names.AddRange(tempName.Select(nn => nn.Name.ToString()));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> membershipLst = new List<decimal>();

            if (endYear - startYear <= 1)
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = startMonth; month <= 12; month++)
                    {
                        var firstOfThisMonth = new DateTime(year, month, 1);
                        var firstOfNextMonth = firstOfThisMonth.AddMonths(1);
                        var drawQuantity = membershipsQuery.ToList().Where(c => c.invDate > firstOfThisMonth && c.invDate <= firstOfNextMonth).Select(s => s.totalDiscount).Sum();
                        membershipLst.Add(drawQuantity);
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
                    var drawQuantity = membershipsQuery.ToList().Where(c => c.invDate > firstOfThisYear && c.invDate <= firstOfNextMYear).Select(s => s.totalDiscount).Sum();

                    membershipLst.Add(drawQuantity);

                    MyAxis.Labels.Add(year.ToString());
                }
            }
            rowChartData.Add(
          new LineSeries
          {
              Values = membershipLst.AsChartValues(),
              Title = AppSettings.resourcemanager.GetString("trDiscount")
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
            string firstTitle = "membership";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                 
                    addpath = @"\Reports\StatisticReport\Sale\Membership\Ar\ArMembership.rdlc";                 
            }
            else
            {
                    addpath = @"\Reports\StatisticReport\Sale\Membership\En\EnMembership.rdlc";   
            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            Title = AppSettings.resourcemanagerreport.GetString("SalesReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            clsReports.membershipsReport(membershipsQuery, rep, reppath, paramarr);//PreparingOrders

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

        private void allowPrintRowinDatagrid(object sender, RoutedEventArgs e)
        {

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
