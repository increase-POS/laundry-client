using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
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

namespace laundryApp.View.reports.accountsReports
{
    /// <summary>
    /// Interaction logic for uc_taxAccountsReports.xaml
    /// </summary>
    public partial class uc_taxAccountsReports : UserControl
    {
        private static uc_taxAccountsReports _instance;
        public static uc_taxAccountsReports Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_taxAccountsReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_taxAccountsReports()
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

        IEnumerable<ItemTransferInvoiceTax> taxes;
        IEnumerable<ItemTransferInvoiceTax> taxTemp = null;
        IEnumerable<ItemTransferInvoiceTax> taxTab;
        Statistics statisticsModel = new Statistics();
        string searchText = "";
        int selectedTab = 0;


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

                txt_search.Text = "";

                if (!AppSettings.invoiceTax_bool.Value)
                {
                    bdr_invoice.Visibility = Visibility.Collapsed;
                    bdr_item.Margin = new Thickness(10, 5, 0, -1);
                    bdr_item.Visibility = Visibility.Visible;
                    Btn_item_Click(btn_item, null);
                }
                else if (!AppSettings.itemsTax_bool.Value)
                {
                    bdr_invoice.Visibility = Visibility.Visible;
                    bdr_item.Visibility = Visibility.Collapsed;
                    bdr_item.Margin = new Thickness(0, 5, 0, -1);
                    Btn_invoice_Click(btn_invoice, null);
                }
                else
                {
                    bdr_invoice.Visibility = Visibility.Visible;
                    bdr_item.Visibility = Visibility.Visible;
                    bdr_item.Margin = new Thickness(0, 5, 0, -1);
                    Btn_invoice_Click(btn_invoice, null);
                }

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), btn_invoice.Tag.ToString());

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region methods
        void fillBranches()
        {
            var iulist = taxes.GroupBy(g => g.branchId).Select(g => new { branchId = g.FirstOrDefault().branchId, branchName = g.FirstOrDefault().branchName }).ToList();
            cb_branches.SelectedValuePath = "branchId";
            cb_branches.DisplayMemberPath = "branchName";
            cb_branches.ItemsSource = iulist;
        }

        async Task Search()
        {
            if (taxes is null)
                await RefreshTaxList();

            searchText = txt_search.Text.ToLower();

            if (selectedTab == 0)
                taxTab = taxes.GroupBy(t => t.invoiceId).SelectMany(inv => inv.Take(1)).ToList();
            else
                taxTab = taxes;

            taxTemp = taxTab.Where(t =>
            //start date
            (dp_startDate.SelectedDate != null ? t.invDate >= dp_startDate.SelectedDate : true)
            &&
            //end date
            (dp_endDate.SelectedDate != null ? t.invDate <= dp_endDate.SelectedDate : true)
            &&
            //branchID
            (cb_branches.SelectedIndex != -1 ? t.branchId == Convert.ToInt32(cb_branches.SelectedValue) : true)
            );

            RefreshTaxView();
            fillBranches();
            fillColumnChart();
            fillRowChart();
        }

        private void RefreshTaxView()
        {
            dgTax.ItemsSource = taxTemp;
            txt_count.Text = taxTemp.Count().ToString();

            decimal total = 0;

            if (selectedTab == 0)
                total = taxTemp.Select(b => b.invTaxVal.Value).Sum();
            else
                total = taxTemp.Select(b => b.itemUnitTaxwithQTY.Value).Sum();

            tb_total.Text = HelpClass.DecTostring(total);
        }


        async Task<IEnumerable<ItemTransferInvoiceTax>> RefreshTaxList()
        {
            taxes = await statisticsModel.GetInvItemTax(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
            return taxes;
        }
        private void translate()
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_endDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            chk_allBranches.Content = AppSettings.resourcemanager.GetString("trAll");

            tt_invoice.Content = AppSettings.resourcemanager.GetString("trInvoices");
            tt_item.Content = AppSettings.resourcemanager.GetString("trItems");
            ////////////////////////////////grid//////////////////////////////////////
            col_invNum.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_Date.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            ////invoice
            col_invQuantity.Header = AppSettings.resourcemanager.GetString("trQTR");
            col_invTotal.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_taxOnInvoice.Header = AppSettings.resourcemanager.GetString("trTaxValue");
            col_invTaxPercent.Header = AppSettings.resourcemanager.GetString("trTaxPercentage");
            col_totalNet.Header = AppSettings.resourcemanager.GetString("trTotalInvoice");
            ////item
            col_itemunitName.Header = AppSettings.resourcemanager.GetString("trItemUnit");
            col_taxOnItems.Header = AppSettings.resourcemanager.GetString("trOnItem");
            col_price.Header = AppSettings.resourcemanager.GetString("trPrice");
            col_itemsQuantity.Header = AppSettings.resourcemanager.GetString("trQTR");
            col_taxOnItems.Header = AppSettings.resourcemanager.GetString("trTaxValue");
            col_itemTaxPercent.Header = AppSettings.resourcemanager.GetString("trTaxPercentage");
            col_itemsTotal.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_totalNetItem.Header = AppSettings.resourcemanager.GetString("trTotalInvoice");
            //////////////////////////////////////////////////////////////////////////
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            txt_total.Text = AppSettings.resourcemanager.GetString("trTotalTax");
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
        private void hideAllColumns()
        {
            for (int i = 3; i < dgTax.Columns.Count; i++)
                dgTax.Columns[i].Visibility = Visibility.Hidden;
        }

        #endregion
         
        private async void Btn_invoice_Click(object sender, RoutedEventArgs e)
        {//invoice
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                hideAllColumns();
                selectedTab = 0;
                tb_total.Text = "";

                col_invQuantity.Visibility = Visibility.Visible;
                col_invTotal.Visibility = Visibility.Visible;
                col_taxOnInvoice.Visibility = Visibility.Visible;
                col_invTaxPercent.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Visible;

                txt_search.Text = "";

                path_item.Fill = Brushes.White;
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_invoice);
                path_invoice.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                chk_allBranches.IsChecked = true;
                Chk_allBranches_Checked(chk_allBranches, null);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

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
                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                selectedTab = 1;
                tb_total.Text = "";

                chk_allBranches.IsChecked = true;
                Chk_allBranches_Checked(chk_allBranches, null);

                col_itemunitName.Visibility = Visibility.Visible;
                col_taxOnItems.Visibility = Visibility.Visible;
                col_totalNet.Visibility = Visibility.Collapsed;
                col_price.Visibility = Visibility.Visible;
                col_itemsQuantity.Visibility = Visibility.Visible;
                col_taxOnItems.Visibility = Visibility.Visible;
                col_itemTaxPercent.Visibility = Visibility.Visible;
                col_itemsTotal.Visibility = Visibility.Visible;
                col_totalNetItem.Visibility = Visibility.Visible;

                txt_search.Text = "";
                path_invoice.Fill = Brushes.White;
                bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_item);
                path_item.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private async void cb_branches_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private async void Chk_allBranches_Checked(object sender, RoutedEventArgs e)
        {//select all branches
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_branches.SelectedIndex = -1;
                cb_branches.IsEnabled = false;
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

        private void RefreshView_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {//change selection
            callSearch(sender);
        }

        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            callSearch(sender);
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            callSearch(sender);
        }


        #region charts

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

            var tempName = taxTemp.GroupBy(s => new { s.branchId }).Select(s => new
            {
                Name = s.FirstOrDefault().invDate,
            });
            names.AddRange(tempName.Select(nn => nn.Name.ToString()));
            string title = "";
            if (selectedTab == 0)
                title = AppSettings.resourcemanager.GetString("trTax") + " / " + AppSettings.resourcemanager.GetString("trInvoice");
            else if (selectedTab == 1)
                title = AppSettings.resourcemanager.GetString("trTax") + " / " + AppSettings.resourcemanager.GetString("trItems");

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> taxLst = new List<decimal>();

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
                            var drawTax = taxTemp.ToList().Where(c => c.invDate > firstOfThisMonth && c.invDate <= firstOfNextMonth).Select(c => c.invTaxVal.Value).Sum();

                            taxLst.Add(drawTax);
                        }
                        if (selectedTab == 1)
                        {
                            var drawTax = taxTemp.ToList().Where(c => c.invDate > firstOfThisMonth && c.invDate <= firstOfNextMonth).Select(c => c.itemUnitTaxwithQTY.Value).Sum();

                            taxLst.Add(drawTax);
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
                        var drawTax = taxTemp.ToList().Where(c => c.invDate > firstOfThisYear && c.invDate <= firstOfNextMYear).Select(c => c.invTaxVal.Value).Sum();
                        taxLst.Add(drawTax);
                    }
                    if (selectedTab == 1)
                    {
                        var drawTax = taxTemp.ToList().Where(c => c.invDate > firstOfThisYear && c.invDate <= firstOfNextMYear).Select(c => c.itemUnitTaxwithQTY.Value).Sum();
                        taxLst.Add(drawTax);
                    }
                    MyAxis.Labels.Add(year.ToString());
                }
            }
            rowChartData.Add(
          new LineSeries
          {
              Values = taxLst.AsChartValues(),
              Title = title
          }); ;

            DataContext = this;
            rowChart.Series = rowChartData;
        }

        private void fillColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            List<ItemTransferInvoiceTax> resultList = new List<ItemTransferInvoiceTax>();
            string title = "";

            #region group data by selected tab
            if (selectedTab == 0)
            {
                title = AppSettings.resourcemanager.GetString("trTax") + " / " + AppSettings.resourcemanager.GetString("trInvoice");
            }
            else if (selectedTab == 1)
            {
                title = AppSettings.resourcemanager.GetString("trTax") + " / " + AppSettings.resourcemanager.GetString("trItems");
            }
            #endregion

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> tax = new List<decimal>();

            if ((chk_allBranches.IsChecked == false) && (cb_branches.SelectedIndex != -1))
            {
                if (selectedTab == 0)
                    tax.Add(taxTemp.Select(b => b.invTaxVal.Value).Sum());
                if (selectedTab == 1)
                    tax.Add(taxTemp.Select(b => b.itemUnitTaxwithQTY.Value).Sum());

                names.AddRange(taxTemp.Where(nn => nn.branchId == (int)cb_branches.SelectedValue).Select(nn => nn.branchName));
                axcolumn.Labels.Add(names.ToList().Skip(0).FirstOrDefault());

                columnChartData.Add(
                  new StackedColumnSeries
                  {
                      Values = tax.AsChartValues(),
                      DataLabels = true,
                      Title = title
                  });

            }
            else
            {
                int count = 0;
                if (selectedTab == 0)
                {
                    var temp = taxTemp.GroupBy(t => t.branchId).Select(t => new
                    {
                        invTaxVal = t.Sum(p => decimal.Parse(HelpClass.DecTostring(p.invTaxVal))),
                        branchName = t.FirstOrDefault().branchName
                    });
                    names.AddRange(temp.Select(nn => nn.branchName));
                    tax.AddRange(temp.Select(nn => nn.invTaxVal));
                    count = names.Count();
                }
                if (selectedTab == 1)
                {
                    var temp = taxTemp.GroupBy(t => t.branchId).Select(t => new
                    {
                        itemUnitTaxwithQTY = t.Sum(p => decimal.Parse(HelpClass.DecTostring(p.itemUnitTaxwithQTY))),
                        branchName = t.FirstOrDefault().branchName
                    });
                    names.AddRange(temp.Select(nn => nn.branchName));
                    tax.AddRange(temp.Select(nn => nn.itemUnitTaxwithQTY));
                    count = names.Count();
                }

                List<decimal> cS = new List<decimal>();

                List<string> titles = new List<string>()
                {
                   title
                };
                int x = 6;
                if (count <= 6) x = count;
                for (int i = 0; i < x; i++)
                {
                    cS.Add(tax.ToList().Skip(i).FirstOrDefault());
                    axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
                }

                if (count > 6)
                {
                    decimal taxSum = 0;
                    for (int i = 6; i < count; i++)
                    {
                        taxSum = taxSum + tax.ToList().Skip(i).FirstOrDefault();
                    }
                    if (!((taxSum == 0)))
                    {
                        cS.Add(taxSum);

                        axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                    }
                }
                columnChartData.Add(
                new StackedColumnSeries
                {
                    Values = cS.AsChartValues(),
                    Title = titles[0],
                    DataLabels = true,
                });
            }
            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        #endregion

        #region reports
        //prin & pdf
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string addpath;
            string firstTitle = "tax";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (selectedTab == 0)
                {
                    //invoice
                    addpath = @"\Reports\StatisticReport\Accounts\Tax\Ar\ArTaxInvoice.rdlc";
                    secondTitle = "invoice";
                }
                else
                {
                    //items
                    addpath = @"\Reports\StatisticReport\Accounts\Tax\Ar\ArTaxItem.rdlc";
                    secondTitle = "items";
                }
            }
            else
            {
                if (selectedTab == 0)
                {
                    //invoice
                    addpath = @"\Reports\StatisticReport\Accounts\Tax\En\EnTaxInvoice.rdlc";
                    secondTitle = "invoice";
                }
                else
                {
                    //items
                    addpath = @"\Reports\StatisticReport\Accounts\Tax\En\EnTaxItem.rdlc";
                    secondTitle = "items";
                }
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            ReportCls.checkLang();
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("trAccounting") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            clsReports.AccTaxReport(taxTemp, rep, reppath, paramarr);
            paramarr.Add(new ReportParameter("totalSum", tb_total.Text));
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);
            rep.SetParameters(paramarr);
            rep.Refresh();
        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            //pdf
                try
                {
                    HelpClass.StartAwait(grid_main);

                    /////////////////////////////////////
                    BuildReport();
                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
                    //////////////////////////////////////

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

                /////////////////////////////////////

                BuildReport();

                this.Dispatcher.Invoke(() =>
                {
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
                });

                //////////////////////////////////////

                HelpClass.EndAwait(grid_main);
                }
                catch (Exception ex)
                {

                    HelpClass.EndAwait(grid_main);
                    HelpClass.ExceptionMessage(ex, this);
                }
            }

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

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
                /////////////////////
                string pdfpath = "";
                pdfpath = @"\Thumb\report\temp.pdf";
                pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
                BuildReport();
                LocalReportExtensions.ExportToPDF(rep, pdfpath);
                ///////////////////
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

    }

       
    }
