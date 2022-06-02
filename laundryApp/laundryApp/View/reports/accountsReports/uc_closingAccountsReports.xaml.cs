using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
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
using System.IO;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;

namespace laundryApp.View.reports.accountsReports
{
    /// <summary>
    /// Interaction logic for uc_closingAccountsReports.xaml
    /// </summary>
    public partial class uc_closingAccountsReports : UserControl
    {
        private static uc_closingAccountsReports _instance;
        public static uc_closingAccountsReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_closingAccountsReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_closingAccountsReports()
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

        IEnumerable<POSOpenCloseModel> closings;
        IEnumerable<POSOpenCloseModel> closingTemp = null;
        Statistics statisticsModel = new Statistics();
        string searchText = "";
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

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

                txt_search.Text = "";

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), btn_closing.Tag.ToString());

                chk_closingBranches.IsChecked = true;

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
        void fillBranches()
        {
            var iulist = closings.GroupBy(g => g.branchId).Select(g => new { branchId = g.FirstOrDefault().branchId, branchName = g.FirstOrDefault().branchName }).ToList();
            cb_closingBranches.SelectedValuePath = "branchId";
            cb_closingBranches.DisplayMemberPath = "branchName";
            cb_closingBranches.ItemsSource = iulist;
        }
        private void translate()
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_closingStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_closingEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_closingBranches, AppSettings.resourcemanager.GetString("trBranchHint"));

            chk_closingBranches.Content = AppSettings.resourcemanager.GetString("trAll");

            tt_closing.Content = AppSettings.resourcemanager.GetString("trCash");

            col_Num.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_pos.Header = AppSettings.resourcemanager.GetString("trPOS");
            col_openDate.Header = AppSettings.resourcemanager.GetString("trOpenDate");
            col_openCash.Header = AppSettings.resourcemanager.GetString("trOpenCash");
            col_closeDate.Header = AppSettings.resourcemanager.GetString("trCloseDate");
            col_closeCash.Header = AppSettings.resourcemanager.GetString("trCloseCash");
            col_operation.Header = AppSettings.resourcemanager.GetString("trOperations");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }
        async Task Search()
        {
            if (closings is null)
                await RefreshClosingList();

            searchText = txt_search.Text.ToLower();

            closingTemp = closings.Where(t =>
            (t.transNum.ToLower().Contains(searchText)
            || t.posName.ToLower().Contains(searchText)
            )
            &&
            //closing start date
            (dp_closingStartDate.SelectedDate != null ? t.updateDate >= dp_closingStartDate.SelectedDate : true)
            &&
            //closing end date
            (dp_closingEndDate.SelectedDate != null ? t.updateDate <= dp_closingEndDate.SelectedDate : true)
            &&
            //branchID
            (cb_closingBranches.SelectedIndex != -1 ? t.branchId == Convert.ToInt32(cb_closingBranches.SelectedValue) : true)
            );

            RefreshClosingView();
            fillBranches();
            fillColumnChart();
            fillPieChart();
        }

        private void RefreshClosingView()
        {
            dgClosing.ItemsSource = closingTemp;
            txt_count.Text = closingTemp.Count().ToString();

        }


        async Task<IEnumerable<POSOpenCloseModel>> RefreshClosingList()
        {
            closings = await statisticsModel.GetPosCashOpenClose(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
            return closings;
        }

        #endregion

        #region charts

        private void fillPieChart()
        {
            List<string> titles = new List<string>();
            IEnumerable<int> x = null;
            IEnumerable<decimal> cashes = null;

            titles.Clear();

            var cashTemp = closingTemp.GroupBy(m => m.posId).Select(
                g => new
                {
                    posId = g.Key,
                    branchName = g.FirstOrDefault().branchName,
                    branchId = g.FirstOrDefault().branchId,
                    cash = g.LastOrDefault().cash
                });
            titles.AddRange(cashTemp.Select(jj => jj.branchName));

            var result = cashTemp.GroupBy(m => m.branchId)
                        .Select(
                            g => new
                            {
                                branchId = g.Key,
                                cash = g.Sum(s => s.cash),
                            });
            cashes = result.Select(m => decimal.Parse(HelpClass.DecTostring(m.cash.Value)));

            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < cashes.Count(); i++)
            {
                List<decimal> final = new List<decimal>();
                List<string> lable = new List<string>();
                final.Add(cashes.ToList().Skip(i).FirstOrDefault());
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

        private void fillRowChart()
        {

        }

        private void fillColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            List<decimal> cashes = new List<decimal>();

            var result = closingTemp.GroupBy(s => s.posId).Select(s => new
            {
                posId = s.Key,
                cash = s.LastOrDefault().cash
            });

            var tempName = closingTemp.GroupBy(s => s.posName + "/" + s.branchName).Select(s => new
            {
                posName = s.Key
            });
            names.AddRange(tempName.Select(nn => nn.posName));

            cashes.AddRange(result.Select(nn => decimal.Parse(HelpClass.DecTostring(nn.cash.Value))));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> cS = new List<decimal>();

            List<string> titles = new List<string>()
            {
               AppSettings.resourcemanager.GetString("trCloseCash")
            };
            int x = 6;
            if (names.Count() <= 6) x = names.Count();

            for (int i = 0; i < x; i++)
            {
                cS.Add(cashes.ToList().Skip(i).FirstOrDefault());
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            if (names.Count() > 6)
            {
                decimal balanceSum = 0;
                for (int i = 6; i < names.Count(); i++)
                    balanceSum = balanceSum + cashes.ToList().Skip(i).FirstOrDefault();

                if (balanceSum != 0)
                    cS.Add(balanceSum);

                axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
            }

            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cS.AsChartValues(),
                Title = titles[0],
                DataLabels = true,
            });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        #endregion

        #region events
         
        private async void Cb_closingBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void Dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {//select date
            callSearch(sender);
        }

        private void Txt_search_SelectionChanged(object sender, RoutedEventArgs e)
        {//search
            callSearch(sender);
        }

        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                txt_search.Text = "";
                searchText = "";
                await Search();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Chk_closingBranches_Checked(object sender, RoutedEventArgs e)
        {//select all branches
            try
            {

                HelpClass.StartAwait(grid_main);

                cb_closingBranches.SelectedIndex = -1;
                cb_closingBranches.IsEnabled = false;
                await Search();


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private async void Chk_closingBranches_Unchecked(object sender, RoutedEventArgs e)
        {//unselect all branches
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_closingBranches.IsEnabled = true;

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

        #region reports

        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            string firstTitle = "closing";//trDailyClosing
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\StatisticReport\Accounts\Closing\Ar\ArClosing.rdlc";

            }
            else
            {
                //english
                addpath = @"\Reports\StatisticReport\Accounts\Closing\En\EnClosing.rdlc";
            }

            secondTitle = "cash";
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("trAccounting") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //  getpuritemcount
            //paramarr.Add(new ReportParameter("totalBalance", tb_total.Text));

            clsReports.ClosingStsReport(closingTemp, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            //pdf
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
                List<ItemTransferInvoice> query = new List<ItemTransferInvoice>();

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
                List<ItemTransferInvoice> query = new List<ItemTransferInvoice>();

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

        #region dataGrid events


        private async void moveRowinDatagrid(object sender, RoutedEventArgs e)
        {//move
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        POSOpenCloseModel row = (POSOpenCloseModel)dgClosing.SelectedItems[0];

                        Statistics statisticsModel = new Statistics();
                        IEnumerable<OpenClosOperatinModel> cashesQuery;
                        cashesQuery = await statisticsModel.GetTransBetweenOpenClose(row.openCashTransId.Value, row.cashTransId);
                        cashesQuery = cashesQuery.Where(c => c.transType != "c" && c.transType != "o");
                        if (cashesQuery.Count() == 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_transBetweenOpenClose w = new wd_transBetweenOpenClose();
                            w.openCashTransID = row.openCashTransId.Value;
                            w.closeCashTransID = row.cashTransId;
                    // w.ShowInTaskbar = false;
                            w.ShowDialog();
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
        private void BuildOperationReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            string firstTitle = "closing";//trDailyClosing
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\StatisticReport\Accounts\Closing\Ar\ArClosOp.rdlc";

            }
            else
            {
                //english
                addpath = @"\Reports\StatisticReport\Accounts\Closing\En\EnClosOp.rdlc";
            }

            secondTitle = "operations";// trOperations
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("trAccounting") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //  getpuritemcount
            //paramarr.Add(new ReportParameter("totalBalance", tb_total.Text));
            //  OpenClosOperatinModel
            clsReports.ClosingOpStsReport(opquery, rep, reppath, paramarr, openclosrow);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        private async void printRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        POSOpenCloseModel row = (POSOpenCloseModel)dgClosing.SelectedItems[0];
                        cashTransID = row.cashTransId;
                        openCashTransID = row.openCashTransId.Value;
                        await getopquery(row);
                        if (opquery.Count() == 0)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {
                            BuildOperationReport();

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

        private async void pdfRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        POSOpenCloseModel row = (POSOpenCloseModel)dgClosing.SelectedItems[0];
                        cashTransID = row.cashTransId;
                        openCashTransID = row.openCashTransId.Value;
                        await getopquery(row);
                        if (opquery.Count() == 0)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            BuildOperationReport();

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

        private async void previewRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        POSOpenCloseModel row = (POSOpenCloseModel)dgClosing.SelectedItems[0];
                        cashTransID = row.cashTransId;
                        openCashTransID = row.openCashTransId.Value;
                        await getopquery(row);
                        if (opquery.Count() == 0)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {
                            string pdfpath = "";

                            pdfpath = @"\Thumb\report\temp.pdf";
                            pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                            BuildOperationReport();
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

        private async void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
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

        private void allowPrintRowinDatagrid(object sender, RoutedEventArgs e)
        {

        }

        private async void excelRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        POSOpenCloseModel row = (POSOpenCloseModel)dgClosing.SelectedItems[0];
                        cashTransID = row.cashTransId;
                        openCashTransID = row.openCashTransId.Value;
                        await getopquery(row);
                        if (opquery.Count() == 0)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoChange"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                        {
                            BuildOperationReport();

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
        #endregion
    }
}
