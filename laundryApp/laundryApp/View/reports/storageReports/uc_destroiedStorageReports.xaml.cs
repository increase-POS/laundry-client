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
using static laundryApp.Classes.Statistics;
using System.Globalization;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.Resources;
using System.Reflection;

namespace laundryApp.View.reports.storageReports
{
    /// <summary>
    /// Interaction logic for uc_destroiedStorageReports.xaml
    /// </summary>
    public partial class uc_destroiedStorageReports : UserControl
    {
        private static uc_destroiedStorageReports _instance;
        public static uc_destroiedStorageReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_destroiedStorageReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_destroiedStorageReports()
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

        List<ItemTransferInvoice> Destroied;

        Statistics statisticModel = new Statistics();

        List<DestroiedCombo> comboDestroiedItemmsUnits;

        IEnumerable<ItemTransferInvoice> temp = null;


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

                Destroied = await statisticModel.GetDesItems((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);

                
                await FillCombo.fillComboBranchesAllWithoutMain(cb_destroiedBranch);

                Btn_destroied_Click(btn_destroied, null);

                
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
            tt_destroied.Content = AppSettings.resourcemanager.GetString("trDestructives");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_destroiedBranch, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_destroiedItemsUnits, AppSettings.resourcemanager.GetString("trItemUnitHint"));

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_destroiedStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_destroiedEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));

            chk_destroiedAllBranches.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_destroiedAllItemsUnits.Content = AppSettings.resourcemanager.GetString("trAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_use.Header = AppSettings.resourcemanager.GetString("trUser");
            col_destroiedNumber.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_destroiedDate.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_destroiedItemsUnits.Header = AppSettings.resourcemanager.GetString("trItemUnit");
            col_destroiedReason.Header = AppSettings.resourcemanager.GetString("trReason");
            col_destroiedAmount.Header = AppSettings.resourcemanager.GetString("trQTR");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }

        public void paint()
        {
            grid_detroied.Visibility = Visibility.Visible;

            bdr_destroied.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;

            path_destroied.Fill = Brushes.White;
        }


        private void fillDestroidEvents()
        {
            temp = fillListDestroied(cb_destroiedBranch, cb_destroiedItemsUnits, dp_destroiedStartDate, dp_destroiedEndDate);
            dgStock.ItemsSource = temp;
            txt_count.Text = temp.Count().ToString();

            fillDestroyColumnChart();
            fillDestroyRowChart();
            fillDestroyPieChart();
        }

        private void fillComboItemsUnits()
        {
            var temp = cb_destroiedBranch.SelectedItem as Branch;
            cb_destroiedItemsUnits.SelectedValuePath = "ItemsUnitsId";
            cb_destroiedItemsUnits.DisplayMemberPath = "ItemsUnits";
            if (temp == null)
            {
                cb_destroiedItemsUnits.ItemsSource = comboDestroiedItemmsUnits
                    .GroupBy(x => x.ItemsUnitsId)
                    .Select(g => new DestroiedCombo
                    {
                        ItemsUnits = g.FirstOrDefault().ItemsUnits,
                        BranchId = g.FirstOrDefault().BranchId,
                        ItemsUnitsId = g.FirstOrDefault().ItemsUnitsId
                    }).ToList();
            }
            else
            {
                cb_destroiedItemsUnits.ItemsSource = comboDestroiedItemmsUnits
                   .Where(x => x.BranchId == temp.branchId)
                    .GroupBy(x => x.ItemsUnitsId)
                    .Select(g => new DestroiedCombo
                    {
                        ItemsUnits = g.FirstOrDefault().ItemsUnits,
                        BranchId = g.FirstOrDefault().BranchId,
                        ItemsUnitsId = g.FirstOrDefault().ItemsUnitsId
                    }).ToList();
            }
        }

        IEnumerable<ItemTransferInvoice> lst;
        private IEnumerable<ItemTransferInvoice> fillListDestroied(ComboBox branch, ComboBox cb, DatePicker startDate, DatePicker endDate)
        {
            var selectedBranch = branch.SelectedItem as Branch;
            var selectedType = cb.SelectedItem as DestroiedCombo;
            var result = Destroied.Where(x => (

                         (branch.SelectedItem != null ? (x.branchId == selectedBranch.branchId) : true)
                        && (cb.SelectedItem != null ? (x.itemUnitId == selectedType.ItemsUnitsId) : true)
                        && (startDate.SelectedDate != null ? (x.invDate >= startDate.SelectedDate) : true)
                        && (endDate.SelectedDate != null ? (x.invDate <= endDate.SelectedDate) : true)
          ));

            lst = result;
            return result;
        }

        private void Cb_destroiedBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                fillComboItemsUnits();
                fillDestroidEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_destroiedAllBranches_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_destroiedBranch.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_destroiedAllBranches_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_destroiedBranch.SelectedItem = null;
                cb_destroiedBranch.IsEnabled = false;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void fillEventsCall(object sender)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                fillDestroidEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Chk_destroiedAllItemsUnits_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_destroiedItemsUnits.SelectedItem = null;
                cb_destroiedItemsUnits.IsEnabled = false;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_destroiedAllItemsUnits_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_destroiedItemsUnits.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void fillDestroyRowChart()
        {
            List<long> cP = new List<long>();

            MyAxis.Labels = new List<string>();

            List<string> names = new List<string>();

            //var temp = fillListDestroied(cb_destroiedBranch, cb_destroiedItemsUnits, dp_destroiedStartDate, dp_destroiedEndDate);

            //var result = temp.GroupBy(s => new { s.itemUnitId }).Select(s => new ItemTransferInvoice
            var result = lst.GroupBy(s => new { s.itemUnitId }).Select(s => new ItemTransferInvoice
            {
                branchId = s.FirstOrDefault().branchId,
                branchName = s.FirstOrDefault().branchName,
                quantity = s.Sum(x => x.quantity),
                ItemUnits = s.FirstOrDefault().ItemUnits,
                itemUnitId = s.FirstOrDefault().itemUnitId,
                itemName = s.FirstOrDefault().itemName,
                unitName = s.FirstOrDefault().unitName
            });
            var tempName = result.GroupBy(s => new { s.itemUnitId }).Select(s => new
            {
                itemName = s.FirstOrDefault().itemName + s.FirstOrDefault().unitName,
            });
            names.AddRange(tempName.Select(nn => nn.itemName));
            for (int i = 0; i < result.Count(); i++)
            {
                cP.Add(long.Parse(result.ToList().Skip(i).FirstOrDefault().quantity.ToString()));
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            SeriesCollection rowChartData = new SeriesCollection();

            rowChartData.Add(
             new LineSeries
             {
                 Values = cP.AsChartValues(),

                 DataLabels = true,
             });
            DataContext = this;
            rowChart.Series = rowChartData;

        }

        private void fillDestroyColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();

            //var temp = fillListDestroied(cb_destroiedBranch, cb_destroiedItemsUnits, dp_destroiedStartDate, dp_destroiedEndDate);

            //var result = temp.GroupBy(s => new { s.branchId }).Select(s => new ItemTransferInvoice
            var result = lst.GroupBy(s => new { s.branchId }).Select(s => new ItemTransferInvoice
            {
                branchId = s.FirstOrDefault().branchId,
                branchName = s.FirstOrDefault().branchName,
                quantity = s.Sum(x => x.quantity),
            });

            var tempName = result.GroupBy(s => new { s.branchId }).Select(s => new
            {
                itemName = s.FirstOrDefault().branchName,
            });
            names.AddRange(tempName.Select(nn => nn.itemName));

            SeriesCollection columnChartData = new SeriesCollection();
            List<long> cPa = new List<long>();

            int xCount = 6;
            if (result.Count() <= 6) xCount = result.Count();

            for (int i = 0; i < xCount; i++)
            {
                cPa.Add(long.Parse(result.ToList().Skip(i).FirstOrDefault().quantity.ToString()));
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            if (result.Count() > 6)
            {
                long c = 0;
                for (int i = 6; i < result.Count(); i++)
                {
                    c = c + long.Parse(result.ToList().Skip(i).FirstOrDefault().quantity.ToString());
                }
                if (c != 0)
                {
                    cPa.Add(c);
                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cPa.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("trAmount")
            });


            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillDestroyPieChart()
        {
            List<string> titles = new List<string>();
            List<long> cP = new List<long>();

            titles.Clear();
            //var temp = fillListDestroied(cb_destroiedBranch, cb_destroiedItemsUnits, dp_destroiedStartDate, dp_destroiedEndDate);

            //var result = temp.GroupBy(s => new { s.itemUnitId }).Select(s => new ItemTransferInvoice
            var result = lst.GroupBy(s => new { s.itemUnitId }).Select(s => new ItemTransferInvoice
            {
                branchId = s.FirstOrDefault().branchId,
                branchName = s.FirstOrDefault().branchName,
                quantity = s.Sum(x => x.quantity),
                ItemUnits = s.FirstOrDefault().ItemUnits,
                itemUnitId = s.FirstOrDefault().itemUnitId,
                itemName = s.FirstOrDefault().itemName,
                unitName = s.FirstOrDefault().unitName
            });
            var tempName = result.GroupBy(s => new { s.itemUnitId }).Select(s => new
            {
                itemName = s.FirstOrDefault().itemName + s.FirstOrDefault().unitName,
            });
            titles.AddRange(tempName.Select(nn => nn.itemName));
            cP = result.Select(m => (long)m.quantity).ToList();
            int count = cP.Count();
            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < count; i++)
            {
                List<decimal> final = new List<decimal>();
                List<string> lable = new List<string>();
                if (i < 5)
                {
                    final.Add(cP.Max());
                    lable.Add(titles.Skip(i).FirstOrDefault());
                    piechartData.Add(
                      new PieSeries
                      {
                          Values = final.AsChartValues(),
                          Title = lable.FirstOrDefault(),
                          DataLabels = true,
                      }
                  );
                    cP.Remove(cP.Max());
                }
                else
                {
                    final.Add(cP.Sum());
                    piechartData.Add(
                      new PieSeries
                      {
                          Values = final.AsChartValues(),
                          Title = AppSettings.resourcemanager.GetString("trOthers"),
                          DataLabels = true,
                      }
                  );
                    break;
                }

            }
            chart1.Series = piechartData;
        }

        private void Btn_destroied_Click(object sender, RoutedEventArgs e)
        {//destroid
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                comboDestroiedItemmsUnits = statisticModel.getDestroiedCombo(Destroied);
                txt_search.Text = "";

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_destroied);
                path_destroied.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                chk_destroiedAllBranches.IsChecked = true;
                chk_destroiedAllItemsUnits.IsChecked = true;

                fillDestroidEvents();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            //string firstTitle = "destroied";
            //string secondTitle = "";
            //string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\StatisticReport\Storage\Ar\ArDestructive.rdlc";

            }
            else
            {
                addpath = @"\Reports\StatisticReport\Storage\En\EnDestructive.rdlc";
            }

            // secondTitle = "destroied";
            //  subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("trStorageReport") + " / " + AppSettings.resourcemanagerreport.GetString("trDestructives");
            paramarr.Add(new ReportParameter("trTitle", Title));

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            clsReports.itemTransferInvoiceDestroied(temp, rep, reppath, paramarr);
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

        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillEventsCall(sender);
        }

        private void Dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fillEventsCall(sender);
        }

        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                dgStock.ItemsSource = lst
                    .Where(obj => (
                obj.branchName.ToLower().Contains(txt_search.Text)
                ||
                obj.invNumber.ToLower().Contains(txt_search.Text)
                //||
                //obj.userdestroy.ToLower().Contains(txt_search.Text) 
                //obj.cUserAccName != null ? obj.cUserAccName.ToLower().Contains(txt_search.Text) : false
                //||
                //obj.causeDestroy != null ? obj.causeDestroy.ToLower().Contains(txt_search.Text) : true
                ||
                obj.quantity.ToString().ToLower().Contains(txt_search.Text)
                ));

                txt_count.Text = dgStock.Items.Count.ToString();

                
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

                txt_search.Text = "";

                cb_destroiedBranch.SelectedItem = null;
                cb_destroiedItemsUnits.SelectedItem = null;

                chk_destroiedAllBranches.IsChecked = true;
                chk_destroiedAllItemsUnits.IsChecked = true;

                dp_destroiedStartDate.SelectedDate = null;
                dp_destroiedEndDate.SelectedDate = null;

                fillListDestroied(cb_destroiedBranch, cb_destroiedItemsUnits, dp_destroiedStartDate, dp_destroiedEndDate);

                
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
