using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using laundryApp.Classes;
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
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.IO;
using netoaster;
using System.Threading;
using laundryApp.View.sales;
using laundryApp.View.windows;
using System.Resources;
using System.Reflection;

namespace laundryApp.View.sales
{
    /// <summary>
    /// Interaction logic for uc_dailySalesStatistic.xaml
    /// </summary>
    public partial class uc_dailySalesStatistic : UserControl
    {
        private static uc_dailySalesStatistic _instance;
        public static uc_dailySalesStatistic Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_dailySalesStatistic();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_dailySalesStatistic()
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
        private Statistics statisticModel = new Statistics();

        IEnumerable<ItemTransferInvoice> itemTrasferInvoices;
        IEnumerable<ItemTransferInvoice> itemTrasferInvoicesQuery;
        string searchText = "";
        // report
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
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                fillServices();

                Btn_Invoice_Click(btn_invoice, null);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region methods
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
            tt_invoice.Content = AppSettings.resourcemanager.GetString("trInvoices");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_invoiceDate, AppSettings.resourcemanager.GetString("trDate"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_sevices, AppSettings.resourcemanager.GetString("typesOfService") + "...");

            chk_allServices.Content = AppSettings.resourcemanager.GetString("trAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_No.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_type.Header = AppSettings.resourcemanager.GetString("trType");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_pos.Header = AppSettings.resourcemanager.GetString("trPOS");
            col_discount.Header = AppSettings.resourcemanager.GetString("trDiscount");
            col_tax.Header = AppSettings.resourcemanager.GetString("trTax");
            col_totalNet.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_processType.Header = AppSettings.resourcemanager.GetString("trPaymentMethods");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }

        async Task<IEnumerable<ItemTransferInvoice>> RefreshItemTransferInvoiceList()
        {
            itemTrasferInvoices = await statisticModel.GetUserdailyinvoice((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);
            return itemTrasferInvoices;
        }

        async Task Search()
        {
            try
            {
                if (itemTrasferInvoices is null)
                    await RefreshItemTransferInvoiceList();

                searchText = txt_search.Text.ToLower();
                itemTrasferInvoicesQuery = itemTrasferInvoices
                    .Where(s =>
                (
                s.invNumber.ToLower().Contains(searchText)
                ||
                (s.invBarcode != null ? s.invBarcode.ToLower().Contains(searchText) : false)
                ||
                s.branchCreatorName.ToString().ToLower().Contains(searchText)
                ||
                s.posName.ToString().ToLower().Contains(searchText)
                ||
                s.invType.ToString().ToLower().Contains(searchText)
                )
                &&
                //service
                (cb_sevices.SelectedIndex != -1 ? s.invType == cb_sevices.SelectedValue.ToString() : true)
                &&
                //date
                (dp_invoiceDate.SelectedDate != null ? s.updateDate.Value.Date.ToShortDateString() == dp_invoiceDate.SelectedDate.Value.Date.ToShortDateString() : true)
                );
            
            }
            catch { }

            RefreshIemTrasferInvoicesView();
        }

        void RefreshIemTrasferInvoicesView()
        {
            //hide tax column if region tax equals to 0
            if (!AppSettings.invoiceTax_bool.Value)
                col_tax.Visibility = Visibility.Hidden;
            else
                col_tax.Visibility = Visibility.Visible;

            dgInvoice.ItemsSource = itemTrasferInvoicesQuery;
            txt_count.Text = itemTrasferInvoicesQuery.Count().ToString();

            fillColumnChart();
            fillPieChart();
            fillRowChart();
        }

        #endregion

        #region events
        private async void Btn_Invoice_Click(object sender, RoutedEventArgs e)
        {//invoice tab
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                txt_search.Text = "";

                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_invoice);
                path_invoice.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                dp_invoiceDate.SelectedDate = DateTime.Now;
                chk_allServices.IsChecked = true;
                rowToHide.Height = rowToShow.Height;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {//select date
            try
            {
                HelpClass.StartAwait(grid_main);

                await RefreshItemTransferInvoiceList();
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
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = "";
                txt_search.Text = "";
                chk_allServices.IsChecked = true;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Chk_allServices_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_sevices.SelectedIndex = -1;
                cb_sevices.IsEnabled = false;

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
        #endregion

        #region charts
        private void fillPieChart()
        {
            List<string> titles = new List<string>();
            List<int> resultList = new List<int>();
            titles.Clear();

            var result = itemTrasferInvoicesQuery.Where(s => s.invType == "s" || s.invType == "ts" || s.invType == "ss")
                .GroupBy(s => new { s.invType })
                .Select(s => new
                {
                   count = s.Count(),
                    type = s.FirstOrDefault().invType,
                });
            resultList = result.Select(m => m.count).ToList();
            titles = result.Select(m => m.type).ToList();
            for (int t = 0; t < titles.Count; t++)
            {
                string s = "";
                switch (titles[t])
                {
                    case "s": s = AppSettings.resourcemanager.GetString("trDiningHallType"); break;
                    case "ts": s = AppSettings.resourcemanager.GetString("trTakeAway"); break;
                    case "ss": s = AppSettings.resourcemanager.GetString("trSelfService"); break;
                }
                titles[t] = s;
            }

            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < resultList.Count(); i++)
            {
                List<int> final = new List<int>();
                List<string> lable = new List<string>();

                final.Add(resultList.Skip(i).FirstOrDefault());
                lable = titles;
                piechartData.Add(
                  new PieSeries
                  {
                      Values = final.AsChartValues(),
                      Title = lable.Skip(i).FirstOrDefault(),
                      DataLabels = true,
                  }
              );

            }
            chart1.Series = piechartData;
        }

        private void fillColumnChart()
        {
            axcolumn.Labels = new List<string>();

            var resultList = itemTrasferInvoicesQuery.GroupBy(x => x.invType).Select(x => new 
            {
                invType  = x.FirstOrDefault().invType,
                sTotal   = x.Where(g => g.invType == "s"  ).Count(),
                tsTotal  = x.Where(g => g.invType == "ts" ).Count(),
                ssTotal  = x.Where(g => g.invType == "ss" ).Count(),
                sdTotal  = x.Where(g => g.invType == "sd" ).Count(),
                tsdTotal = x.Where(g => g.invType == "tsd").Count(),
                ssdTotal = x.Where(g => g.invType == "ssd").Count(),
            }
            ).ToList();

            List<string> names = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trDiningHallType"),
                AppSettings.resourcemanager.GetString("trTakeAway"),
                AppSettings.resourcemanager.GetString("trSelfService")
            };

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> invoice = new List<int>();
            List<int> draft   = new List<int>();

            invoice.Add(resultList.Sum(i => i.sTotal));
            invoice.Add(resultList.Sum(i => i.tsTotal));
            invoice.Add(resultList.Sum(i => i.ssTotal));

            draft.Add(resultList.Sum(i => i.sdTotal));
            draft.Add(resultList.Sum(i => i.tsdTotal));
            draft.Add(resultList.Sum(i => i.ssdTotal));

            axcolumn.Labels.Add(names[0]);
            axcolumn.Labels.Add(names[1]);
            axcolumn.Labels.Add(names[2]);

            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = invoice.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("trInvoice")
            });
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = draft.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("trDraft")
            });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillRowChart()
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
           
            List<decimal> s = new List<decimal>();
            List<decimal> ts = new List<decimal>();
            List<decimal> ss = new List<decimal>();

            var result = itemTrasferInvoicesQuery.GroupBy(i => i.invType).Select(i => new
            {
                invType    = i.Key,
                totalP     = i.Where(x => x.invType == "s" ).Sum(x => x.totalNet),
                totalPb    = i.Where(x => x.invType == "ts").Sum(x => x.totalNet),
                resultTemp = i.Where(x => x.invType == "ss").Sum(x => x.totalNet)
            }
         );

            s.Add(result.Sum(x => (decimal)x.totalP));
            ts.Add(result.Sum(x => (decimal)x.totalPb));
            ss.Add(result.Sum(x => (decimal)x.resultTemp));

            SeriesCollection rowChartData = new SeriesCollection();
           

            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trDiningHallType"),
                AppSettings.resourcemanager.GetString("trTakeAway"),
                AppSettings.resourcemanager.GetString("trSelfService")
            };
          
            rowChartData.Add(
            new LineSeries
            {
                Values = s.AsChartValues(),
                Title = titles[0]
            });
            rowChartData.Add(
             new LineSeries
             {
                 Values = ts.AsChartValues(),
                 Title = titles[1]
             });
            rowChartData.Add(
            new LineSeries
            {
                Values = ss.AsChartValues(),
                Title = titles[2]
            });

            DataContext = this;
            rowChart.Series = rowChartData;
        }

        #endregion
        #region report
        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            string firstTitle = "dailySalesStatistic";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\StatisticReport\Sale\Daily\Ar\ArDailySale.rdlc";




                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            }
            else
            {
                addpath = @"\Reports\StatisticReport\Sale\Daily\En\EnDailySale.rdlc";
                //if (selectedTab == 0)
                //{
                //    secondTitle = "invoice";
                //}
                //else if (selectedTab == 1)
                //{
                //    secondTitle = "order";
                //}
                //else
                //{
                //    //  selectedTab == 2
                //    secondTitle = "quotation";

                //}
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            Title = AppSettings.resourcemanagerreport.GetString("SalesReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            clsReports.SaledailyReport(itemTrasferInvoicesQuery.ToList(), rep, reppath, paramarr);
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

        #region datagrid btns
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
        #region reports
        //public void BuildReport()
        //{
        //    List<ReportParameter> paramarr = new List<ReportParameter>();

        //    string addpath;
        //    bool isArabic = ReportCls.checkLang();
        //    if (isArabic)
        //    {
        //        addpath = @"\Reports\Sale\Ar\dailySale.rdlc";
        //    }
        //    else
        //        addpath = @"\Reports\Sale\En\dailySale.rdlc";
        //    string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

        //    ReportCls.checkLang();

        //    clsReports.SaledailyReport(itemTrasferInvoicesQuery, rep, reppath, paramarr);
        //    clsReports.setReportLanguage(paramarr);
        //    clsReports.Header(paramarr);

        //    rep.SetParameters(paramarr);

        //    rep.Refresh();
        //}
        //public void pdfdaily()
        //{

        //    BuildReport();

        //    this.Dispatcher.Invoke(() =>
        //    {
        //        saveFileDialog.Filter = "PDF|*.pdf;";

        //        if (saveFileDialog.ShowDialog() == true)
        //        {
        //            string filepath = saveFileDialog.FileName;
        //            LocalReportExtensions.ExportToPDF(rep, filepath);
        //        }
        //    });
        //}

        //private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);
        //        //if (MainWindow.groupObject.HasPermissionAction(basicsPermission, MainWindow.groupObjects, "report") || HelpClass.isAdminPermision())
        //        //{
        //        /////////////////////////////////////
        //        Thread t1 = new Thread(() =>
        //        {
        //            pdfdaily();
        //        });
        //        t1.Start();
        //        //////////////////////////////////////
        //        //}
        //        //else
        //        //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

        //            HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {

        //            HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        //public void printDaily()
        //{
        //    BuildReport();

        //    this.Dispatcher.Invoke(() =>
        //    {
        //        LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
        //    });
        //}
        //private void Btn_print_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);

        //        //if (MainWindow.groupObject.HasPermissionAction(basicsPermission, MainWindow.groupObjects, "report") || HelpClass.isAdminPermision())
        //        //{
        //        /////////////////////////////////////
        //        Thread t1 = new Thread(() =>
        //        {
        //            printDaily();
        //        });
        //        t1.Start();
        //        //////////////////////////////////////

        //        //}
        //        //else
        //        //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        //public void ExcelDaily()
        //{

        //    BuildReport();

        //    this.Dispatcher.Invoke(() =>
        //    {
        //        saveFileDialog.Filter = "EXCEL|*.xls;";
        //        if (saveFileDialog.ShowDialog() == true)
        //        {
        //            string filepath = saveFileDialog.FileName;
        //            LocalReportExtensions.ExportToExcel(rep, filepath);
        //        }
        //    });
        //}
        //private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);

        //        //if (MainWindow.groupObject.HasPermissionAction(basicsPermission, MainWindow.groupObjects, "report") || HelpClass.isAdminPermision())
        //        //{
        //        //Thread t1 = new Thread(() =>
        //        //{
        //        ExcelDaily();

        //        //});
        //        //t1.Start();
        //        //}
        //        //else
        //        //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        //private void Btn_preview_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);

        //        //if (MainWindow.groupObject.HasPermissionAction(basicsPermission, MainWindow.groupObjects, "report") || HelpClass.isAdminPermision())
        //        //{
        //        #region
        //        Window.GetWindow(this).Opacity = 0.2;
        //        /////////////////////
        //        string pdfpath = "";
        //        pdfpath = @"\Thumb\report\temp.pdf";
        //        pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
        //        BuildReport();
        //        LocalReportExtensions.ExportToPDF(rep, pdfpath);
        //        ///////////////////
        //        wd_previewPdf w = new wd_previewPdf();
        //        w.pdfPath = pdfpath;
        //        if (!string.IsNullOrEmpty(w.pdfPath))
        //        {
        //            w.ShowDialog();
        //            w.wb_pdfWebViewer.Dispose();
        //        }
        //        Window.GetWindow(this).Opacity = 1;
        //        #endregion
        //        //}
        //        //else
        //        //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}



        #endregion


    }
}
