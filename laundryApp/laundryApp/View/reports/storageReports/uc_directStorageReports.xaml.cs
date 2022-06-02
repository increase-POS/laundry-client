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
using static laundryApp.Classes.Statistics;
using LiveCharts.Helpers;
using LiveCharts;
using LiveCharts.Wpf;
using System.Resources;
using System.Reflection;
using Microsoft.Win32;
using Microsoft.Reporting.WinForms;
using System.Threading;
using System.IO;
using laundryApp.View.windows;
using laundryApp.View.storage;
using laundryApp.View.storage.storageOperations;

namespace laundryApp.View.reports.storageReports
{
    /// <summary>
    /// Interaction logic for uc_directStorageReports.xaml
    /// </summary>
    public partial class uc_directStorageReports : UserControl
    {
        private static uc_directStorageReports _instance;
        public static uc_directStorageReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_directStorageReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_directStorageReports()
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

        Statistics statisticModel = new Statistics();
        List<ItemTransferInvoice> itemsTransfer;
        List<ExternalitemCombo> comboDirectItems;
        List<ExternalUnitCombo> comboDirectUnits;
        IEnumerable<ItemTransferInvoice> temp = null;

        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        string searchText = "";

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

                itemsTransfer = await statisticModel.GetDirectInMov((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);

                comboDirectItems = statisticModel.getExternalItemCombo(itemsTransfer);
                comboDirectUnits = statisticModel.getExternalUnitCombo(itemsTransfer);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), btn_item.Tag.ToString());

                txt_search.Text = "";

                await FillCombo.fillComboBranchesAllWithoutMain(cb_directItemsBranches);

                fillEvents();


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        #region charts
        private void fillDirectRowChart()
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            List<int> pTemp = new List<int>();

            var result = temp.GroupBy(x => new { x.itemUnitId }).Select(x => new ItemTransferInvoice
            {
                itemUnitId = x.FirstOrDefault().itemUnitId,
                itemName = x.FirstOrDefault().itemName + "-" + x.FirstOrDefault().unitName,
                itemsCount = x.Sum(g => (int)(g.quantity))
            });

            for (int i = 0; i < result.Count(); i++)
            {
                pTemp.Add(result.ToList().Skip(i).FirstOrDefault().itemsCount.Value);
            }

            names.AddRange(result.Select(nn => nn.itemName));

            SeriesCollection rowChartData = new SeriesCollection();
            for (int i = 0; i < pTemp.Count(); i++)
            {
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            rowChartData.Add(
              new LineSeries
              {
                  Values = pTemp.AsChartValues(),
                  Title = AppSettings.resourcemanager.GetString("trItemUnit")

              });

            rowChart.Series = rowChartData;
            DataContext = this;
        }

        private void fillDirectColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();

            var res = temp.GroupBy(x => new { x.branchId }).Select(x => new ItemTransferInvoice
            {
                branchId = x.FirstOrDefault().branchId,
                branchName = x.FirstOrDefault().branchName,
                itemsCount = x.Sum(g => (int)(g.quantity))
            });

            names.AddRange(res.Select(nn => nn.branchName));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> cP = new List<int>();

            int xCount = 6;
            if (names.Count() <= 6) xCount = names.Count();

            for (int i = 0; i < xCount; i++)
            {
                cP.Add(res.ToList().Skip(i).FirstOrDefault().itemsCount.Value);
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            if (names.Count() > 6)
            {
                int b = 0;
                for (int i = 6; i < names.Count(); i++)
                {
                    b = b + res.ToList().Skip(i).FirstOrDefault().itemsCount.Value;
                }
                if (!(b == 0))
                {
                    cP.Add(b);
                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cP.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("trBranch")
            }); ;

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillDirectPieChart()
        {
            List<string> titles = new List<string>();
            List<string> titles1 = new List<string>();
            List<decimal> x = new List<decimal>();
            titles.Clear();
            titles1.Clear();

            var result = temp
                .GroupBy(s => new { s.itemId, s.unitId })
                .Select(s => new ItemTransferInvoice
                {
                    itemId = s.FirstOrDefault().itemId,
                    unitId = s.FirstOrDefault().unitId,
                    quantity = s.Sum(g => g.quantity),
                    itemName = s.FirstOrDefault().itemName,
                    unitName = s.FirstOrDefault().unitName,
                }).OrderByDescending(s => s.quantity);
            x = result.Select(m => (decimal)m.quantity).ToList();
            titles = result.Select(m => m.itemName).ToList();
            titles1 = result.Select(m => m.unitName).ToList();
            int count = x.Count();
            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < count; i++)
            {
                List<decimal> final = new List<decimal>();
                List<string> lable = new List<string>();
                if (i < 5)
                {
                    final.Add(x.Max());
                    lable.Add(titles.Skip(i).FirstOrDefault() + titles1.Skip(i).FirstOrDefault());
                    piechartData.Add(
                      new PieSeries
                      {
                          Values = final.AsChartValues(),
                          Title = lable.FirstOrDefault(),
                          DataLabels = true,
                      }
                  );
                    x.Remove(x.Max());
                }
                else
                {
                    final.Add(x.Sum());
                    piechartData.Add(
                      new PieSeries
                      {
                          Values = final.AsChartValues(),
                          Title = AppSettings.resourcemanager.GetString("trOthers"),
                          DataLabels = true,
                      }
                  ); ;
                    break;
                }

            }
            chart1.Series = piechartData;
        }

        #endregion

        #region methods

        private void translate()
        {
            tt_item.Content = AppSettings.resourcemanager.GetString("trItems");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_directItemsBranches, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_directItems, AppSettings.resourcemanager.GetString("trItemHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_directUnits, AppSettings.resourcemanager.GetString("trUnitHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_directStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_directEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));

            chk_directAllBranches.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_directAllItems.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_directAllUnits.Content = AppSettings.resourcemanager.GetString("trAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_Num.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_date.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_item.Header = AppSettings.resourcemanager.GetString("trItem");
            col_unit.Header = AppSettings.resourcemanager.GetString("trUnit");
            col_quantity.Header = AppSettings.resourcemanager.GetString("trQTR");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }
        private IEnumerable<ItemTransferInvoice> fillList()
        {
            var result = itemsTransfer.Where(x =>
                             (cb_directItemsBranches.SelectedItem != null ? (x.branchId == Convert.ToInt32(cb_directItemsBranches.SelectedValue)) : true)
                          && (cb_directItems.SelectedItem != null ? (x.itemId == Convert.ToInt32(cb_directItems.SelectedValue)) : true)
                          && (cb_directUnits.SelectedItem != null ? (x.unitId == Convert.ToInt32(cb_directUnits.SelectedValue)) : true)//cb_directUnits
                          && (dp_directStartDate.SelectedDate != null ? (x.invDate >= dp_directStartDate.SelectedDate) : true)
                          && (dp_directEndDate.SelectedDate != null ? (x.invDate <= dp_directEndDate.SelectedDate) : true)
                          );
            return result;

        }

        private void fillEvents()
        {
            temp = fillList();

            dgDirect.ItemsSource = temp;
            txt_count.Text = temp.Count().ToString();

            fillDirectColumnChart();
            fillDirectPieChart();
            //fillDirectRowChart();
        }

        private void fillComboDirectItems()
        {
            var temp = cb_directItemsBranches.SelectedItem as Branch;
            cb_directItems.SelectedValuePath = "ItemId";
            cb_directItems.DisplayMemberPath = "ItemName";
            if (temp == null)
            {
                cb_directItems.ItemsSource = comboDirectItems
                    .GroupBy(x => x.ItemId)
                    .Select(g => new ExternalitemCombo
                    {
                        ItemId = g.FirstOrDefault().ItemId,
                        ItemName = g.FirstOrDefault().ItemName,
                        BranchId = g.FirstOrDefault().BranchId
                    }).ToList();
            }
            else
            {
                cb_directItems.ItemsSource = comboDirectItems
                    .Where(x => x.BranchId == temp.branchId)
                    .GroupBy(x => x.ItemId)
                    .Select(g => new ExternalitemCombo
                    {
                        ItemId = g.FirstOrDefault().ItemId,
                        ItemName = g.FirstOrDefault().ItemName,
                        BranchId = g.FirstOrDefault().BranchId
                    }).ToList();
            }
        }

        private void fillComboDirectUnits()
        {
            var temp = cb_directItems.SelectedItem as ExternalitemCombo;
            var temp1 = cb_directItemsBranches.SelectedItem as Branch;

            cb_directUnits.SelectedValuePath = "UnitId";
            cb_directUnits.DisplayMemberPath = "UnitName";
            if (temp == null && temp1 == null)
            {
                cb_directUnits.ItemsSource = comboDirectUnits
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
            else if (temp != null && temp1 == null)
            {
                cb_directUnits.ItemsSource = comboDirectUnits
                     .Where(x => x.ItemId == temp.ItemId)
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
            else if (temp == null && temp1 != null)
            {
                cb_directUnits.ItemsSource = comboDirectUnits
                     .Where(x => x.BranchId == temp1.branchId)
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
            else
            {
                cb_directUnits.ItemsSource = comboDirectUnits
                    .Where(x => x.ItemId == temp.ItemId && x.BranchId == temp1.branchId)
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
        }
        #endregion

        #region events

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {

                HelpClass.StartAwait(grid_main);

                txt_search.Text = "";
                searchText = "";
                
                fillEvents();

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
            try
            {

                HelpClass.StartAwait(grid_main);

                temp = temp
                              .Where(s => (s.invNumber.ToString().Contains(txt_search.Text) ||
                                           s.branchName.Contains(txt_search.Text) ||
                                           s.itemName.Contains(txt_search.Text) ||
                                           s.unitName.Contains(txt_search.Text) ||
                                           s.quantity.ToString().Contains(txt_search.Text)
                                     ));

                dgDirect.ItemsSource = temp;
                txt_count.Text = temp.Count().ToString();

                fillDirectColumnChart();
                fillDirectPieChart();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_directItemsBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);

                cb_directItems.IsEnabled = true;
                chk_directAllItems.IsEnabled = true;
                chk_directAllItems.IsChecked = true;

                fillComboDirectItems();

                fillEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_directItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);

                cb_directUnits.IsEnabled = true;
                chk_directAllUnits.IsEnabled = true;

                fillComboDirectUnits();

                fillEvents();

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_directUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                fillEvents();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Chk_directAllBranches_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_directItemsBranches.IsEnabled = false;
                cb_directItemsBranches.SelectedItem = null;
                cb_directItems.IsEnabled = true;
                chk_directAllItems.IsEnabled = true;

                fillComboDirectItems();

                fillEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_directAllBranches_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_directItemsBranches.IsEnabled = true;
                cb_directItems.IsEnabled = false;
                cb_directItems.SelectedItem = null;
                chk_directAllItems.IsEnabled = false;
                chk_directAllItems.IsChecked = false;

                fillEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_directAllItems_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_directItems.IsEnabled = false;
                cb_directItems.SelectedItem = null;
                cb_directUnits.IsEnabled = true;
                chk_directAllUnits.IsEnabled = true;

                fillComboDirectUnits();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_directAllItems_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_directItems.IsEnabled = true;
                cb_directUnits.IsEnabled = false;
                cb_directUnits.SelectedItem = null;
                chk_directAllUnits.IsEnabled = false;
                chk_directAllUnits.IsChecked = false;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_directAllUnits_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_directUnits.IsEnabled = false;
                cb_directUnits.SelectedItem = null;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_directAllUnits_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_directUnits.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                fillEvents();

                
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

        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            string firstTitle = "DirectEntry";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\StatisticReport\Storage\Ar\ArDirectItem.rdlc";

            }
            else
            {
                addpath = @"\Reports\StatisticReport\Storage\En\EnDirectItem.rdlc";

            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            secondTitle = "items";
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            Title = AppSettings.resourcemanagerreport.GetString("trStorageReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            clsReports.itemTransferInvoiceDirect(temp, rep, reppath, paramarr);

            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        #endregion

        Invoice invoice;
        private async void DgDirect_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);
                invoice = new Invoice();
                if (dgDirect.SelectedIndex != -1)
                {
                    ItemTransferInvoice item = dgDirect.SelectedItem as ItemTransferInvoice;
                    if (item.invoiceId > 0)
                    {
                        invoice = await invoice.GetByInvoiceId(item.invoiceId);
                        MainWindow.mainWindow.Btn_storage_Click(MainWindow.mainWindow.btn_storage, null);

                        //View.uc_storage.Instance.UserControl_Loaded(null, null);
                        //View.uc_storage.Instance.Btn_receiptOfPurchaseInvoice_Click(View.uc_storage.Instance.btn_reciptOfInvoice, null);

                        MainWindow.mainWindow.grid_main.Children.Clear();
                        MainWindow.mainWindow.grid_main.Children.Add(uc_storageInvoice.Instance);

                        MainWindow.mainWindow.initializationMainTrack("storageMovements");
                        //uc_storageInvoice.Instance.UserControl_Loaded(null, null);
                        uc_storageInvoice._InvoiceType = invoice.invType;
                        uc_storageInvoice.Instance.invoice = invoice;
                        uc_storageInvoice.isFromReport = true;
                        await uc_storageInvoice.Instance.fillInvoiceInputs(invoice);
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

        
    }
}
