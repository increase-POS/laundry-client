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
using System.Windows.Shapes;
using static laundryApp.Classes.Statistics;

namespace laundryApp.View.reports.accountsReports
{
    /// <summary>
    /// Interaction logic for uc_posAccountsReports.xaml
    /// </summary>
    public partial class uc_posAccountsReports : UserControl
    {
        private static uc_posAccountsReports _instance;
        public static uc_posAccountsReports Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_posAccountsReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_posAccountsReports()
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
        List<CashTransferSts> list;
        List<CashTransfer> listCash;
        List<branchFromCombo> fromBranches = new List<branchFromCombo>();
        List<branchToCombo> toBranches = new List<branchToCombo>();
        List<posFromCombo> fromPos;
        List<posToCombo> toPos;
        IEnumerable<AccountantCombo> accCombo;

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

                listCash = await statisticModel.GetBytypeAndSideForPos("all", "p");

                accCombo = listCash.GroupBy(g => g.updateUserAcc).Select(g => new AccountantCombo { Accountant = g.FirstOrDefault().updateUserAcc }).ToList();

                fillAccCombo();

                Btn_payments_Click(btn_payments, null);

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
            tt_payments.Content = AppSettings.resourcemanager.GetString("trDeposit");
            tt_pulls.Content = AppSettings.resourcemanager.GetString("trReceive");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_formBranch, AppSettings.resourcemanager.GetString("trFromBranch") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toBranch, AppSettings.resourcemanager.GetString("trToBranch") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_formPos, AppSettings.resourcemanager.GetString("trDepositor") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toPos, AppSettings.resourcemanager.GetString("trRecepient") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Accountant, AppSettings.resourcemanager.GetString("trAccoutant") + "...");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_StartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_EndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            chk_allFromBranch.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allToBranch.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allFromPos.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allToPos.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allAccountant.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_twoWay.Content = AppSettings.resourcemanager.GetString("trTwoWays");
          
            col_tansNum.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_creatorBranch.Header = AppSettings.resourcemanager.GetString("trCreator");
            col_fromBranch.Header = AppSettings.resourcemanager.GetString("trFromBranch");
            col_fromPos.Header = AppSettings.resourcemanager.GetString("trDepositor");
            col_toBranch.Header = AppSettings.resourcemanager.GetString("trToBranch");
            col_toPos.Header = AppSettings.resourcemanager.GetString("trRecepient");
            col_updateUserAcc.Header = AppSettings.resourcemanager.GetString("trAccoutant");
            col_updateDate.Header = AppSettings.resourcemanager.GetString("trDate");
            col_status.Header = AppSettings.resourcemanager.GetString("trStatus");
            col_cash.Header = AppSettings.resourcemanager.GetString("trAmount");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }

        List<CashTransfer> posLst;
        private List<CashTransfer> fillList()
        {
            var result = listCash
          .Where(s =>
          //start date
          (dp_StartDate.SelectedDate != null ? s.updateDate >= dp_StartDate.SelectedDate : true)
          &&
          //end date
          (dp_EndDate.SelectedDate != null ? s.updateDate <= dp_EndDate.SelectedDate : true)
          &&
          //fromBranch
          (cb_formBranch.SelectedIndex != -1 ? s.branchId == Convert.ToInt32(cb_formBranch.SelectedValue) : true)
          &&
          //toBranch
          (cb_toBranch.SelectedIndex != -1 ? s.branch2Id == Convert.ToInt32(cb_toBranch.SelectedValue) : true)
          &&
          //accountant
          (cb_Accountant.SelectedIndex != -1 ? s.updateUserAcc == cb_Accountant.SelectedValue.ToString() : true)
          &&
          //fromPos
          (cb_formPos.SelectedIndex != -1 ? s.posId == Convert.ToInt32(cb_formPos.SelectedValue) : true)
          &&
          //toPos
          (cb_toPos.SelectedIndex != -1 ? s.pos2Id == Convert.ToInt32(cb_toPos.SelectedValue) : true)
          // &&
          // //twoWay
          // (
          // chk_twoWay.IsChecked == true ?
          //     //fromPos
          //     (cb_formPos.SelectedIndex != -1 ? s.fromposId == Convert.ToInt32(cb_formPos.SelectedValue) || s.toposId == Convert.ToInt32(cb_formPos.SelectedValue) : true)
          //     &&
          //     //toPos
          //     (cb_toPos.SelectedIndex != -1 ? s.toposId == Convert.ToInt32(cb_toPos.SelectedValue) || s.fromposId == Convert.ToInt32(cb_toPos.SelectedValue) : true)
          //:
          //     //fromPos
          //     (cb_formPos.SelectedIndex != -1 ? s.fromposId == Convert.ToInt32(cb_formPos.SelectedValue) : true)
          //     &&
          //     //toPos
          //     (cb_toPos.SelectedIndex != -1 ? s.toposId == Convert.ToInt32(cb_toPos.SelectedValue) : true)
          // )
          && s.transType == _transtype
          );
            posLst = result.ToList();
            return result.ToList();
        }

        private void fillComboBranches()
        {
            cb_formBranch.SelectedValuePath = "BranchFromId";
            cb_formBranch.DisplayMemberPath = "BranchFromName";
            cb_formBranch.ItemsSource = fromBranches;

            cb_toBranch.SelectedValuePath = "BranchToId";
            cb_toBranch.DisplayMemberPath = "BranchToName";
            cb_toBranch.ItemsSource = toBranches;
        }
        //private void fillComboFromBranch(ComboBox cb)
        //{
        //    cb.SelectedValuePath = "BranchFromId";
        //    cb.DisplayMemberPath = "BranchFromName";
        //    cb.ItemsSource = fromBranches;
        //}

        //private void fillComboToBranch(ComboBox cb)
        //{
        //    cb.SelectedValuePath = "BranchToId";
        //    cb.DisplayMemberPath = "BranchToName";
        //    cb.ItemsSource = toBranches;
        //}
        private void fillComboFromPos()
        {
            cb_formPos.SelectedValuePath = "PosFromId";
            cb_formPos.DisplayMemberPath = "PosFromName";
            cb_formPos.ItemsSource = fromPos;
            if (cb_formBranch.SelectedItem != null)
            {
                var temp = cb_formBranch.SelectedItem as branchFromCombo;
                cb_formPos.ItemsSource = fromPos.Where(x => x.BranchId == temp.BranchFromId);
            }
        }
        private void fillComboToPos()
        {
            cb_toPos.SelectedValuePath = "PosToId";
            cb_toPos.DisplayMemberPath = "PosToName";
            cb_toPos.ItemsSource = toPos;
            if (cb_toBranch.SelectedItem != null)
            {
                var temp = cb_toBranch.SelectedItem as branchToCombo;
                cb_toPos.ItemsSource = toPos.Where(x => x.BranchId == temp.BranchToId);
            }
        }

        private void fillAccCombo()
        {
            cb_Accountant.SelectedValuePath = "Accountant";
            cb_Accountant.DisplayMemberPath = "Accountant";
            cb_Accountant.ItemsSource = accCombo;
        }

        IEnumerable<CashTransfer> temp = null;
        private void fillEvents()
        {
            temp = fillList();
            dgPayments.ItemsSource = temp;
            txt_count.Text = temp.Count().ToString();
            fillColumnChart();
            //fillPieChart();
            fillRowChart();
        }

        private void Cb_formBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                fillComboFromPos();
                fillEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allFromBranch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_formBranch.IsEnabled = false;
                cb_formBranch.SelectedItem = null;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allFromBranch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_formBranch.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allToBranch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_toBranch.IsEnabled = false;
                cb_toBranch.SelectedItem = null;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allToBranch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_toBranch.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_formPos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (cb_formPos.SelectedItem != null)
                    chk_twoWay.IsEnabled = true;

                fillEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allFromPos_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_formPos.IsEnabled = false;
                cb_formPos.SelectedItem = null;

                try
                {
                    if (cb_toPos.SelectedItem == null)
                        chk_twoWay.IsEnabled = false;
                }
                catch { }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allFromPos_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_formPos.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allToPos_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_toPos.IsEnabled = false;
                cb_toPos.SelectedItem = null;
                if (cb_formPos.SelectedItem == null)
                    chk_twoWay.IsEnabled = false;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allToPos_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_toPos.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_toBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                fillComboToPos();

                fillEvents();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_toPos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (cb_toPos.SelectedItem != null)
                    chk_twoWay.IsEnabled = true;

                fillEvents();

                
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
            changeSelection(sender);
        }

        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeSelection(sender);
        }

        private void Chk_allAccountant_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_Accountant.IsEnabled = false;
                cb_Accountant.SelectedItem = null;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allAccountant_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_Accountant.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void changeSelection(object sender)
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

        private void Chk_Checked(object sender, RoutedEventArgs e)
        {
            changeSelection(sender);
        }

        private void fillPieChart()
        {
            List<string> titles = new List<string>();
            List<int> resultList = new List<int>();
            titles.Clear();

            var temp = posLst;
            var result = temp
                .GroupBy(s => new { s.transType })
                .Select(s => new CashTransferSts
                {
                    processTypeCount = s.Count(),
                    processType = s.FirstOrDefault().transType,
                });
            resultList = result.Select(m => m.processTypeCount).ToList();
            titles = result.Select(m => m.processType).ToList();
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
            List<string> names = new List<string>();

            var temp = posLst;

            var res = temp.GroupBy(x => new { x.posId }).Select(x => new CashTransfer
            {
                transType = _transtype,
                posId = x.FirstOrDefault().posId,
                posName = x.FirstOrDefault().posName + "/" + x.FirstOrDefault().branchName,
                cash = x.Sum(g => g.cash)
            });

            List<CashTransfer> result = new List<CashTransfer>();

            result.AddRange(res.ToList());

            var finalResult = result.GroupBy(x => new { x.posId }).Select(x => new CashTransferSts
            {
                transType = x.FirstOrDefault().transType,
                posId = x.FirstOrDefault().posId,
                posName = x.FirstOrDefault().posName,
                depositSum = x.Where(g => g.transType == _transtype).Sum(g => (decimal)g.cash),
            });
            var tempName = finalResult.Select(s => new
            {
                itemName = s.posName,
            });
            names.AddRange(tempName.Select(nn => nn.itemName));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> cP = new List<decimal>();

            int xCount = 6;

            if (names.Count() <= 6)
                xCount = names.Count();

            for (int i = 0; i < xCount; i++)
            {
                cP.Add(finalResult.ToList().Skip(i).FirstOrDefault().depositSum);
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            if (names.Count() > 6)
            {
                decimal depositSum = 0;
                for (int i = 6; i < names.Count(); i++)
                {
                    depositSum = depositSum + finalResult.ToList().Skip(i).FirstOrDefault().depositSum;
                }
                if (!(depositSum == 0))
                {
                    cP.Add(depositSum);

                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }

            string title = "";
            if (_transtype == "d") title = AppSettings.resourcemanager.GetString("trDeposit");
            else if (_transtype == "p") title = AppSettings.resourcemanager.GetString("trPull");

            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cP.AsChartValues(),
                DataLabels = true,
                Title = title
            });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillRowChart()
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            List<CashTransfer> resultList = new List<CashTransfer>();

            SeriesCollection rowChartData = new SeriesCollection();

            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> deposit = new List<decimal>();

            var temp = posLst;

            var res = temp.GroupBy(x => new { x.branchId }).Select(x => new CashTransfer
            {
                transType = _transtype,
                branchId = x.FirstOrDefault().branchId,
                branchName = x.FirstOrDefault().branchName,
                cash = x.Sum(g => g.cash)
            });

            List<CashTransfer> result = new List<CashTransfer>();

            result.AddRange(res.ToList());

            var finalResult = result.GroupBy(x => new { x.branchId }).Select(x => new CashTransferSts
            {
                transType = x.FirstOrDefault().transType,
                branchId = x.FirstOrDefault().branchId,
                branchName = x.FirstOrDefault().branchName,
                depositSum = x.Where(g => g.transType == _transtype).Sum(g => (decimal)g.cash),
            });
            var tempName = finalResult.Select(s => new
            {
                itemName = s.branchName,
            });
            names.AddRange(tempName.Select(nn => nn.itemName));

            int xCount = 6;

            if (names.Count() <= 6)
                xCount = names.Count();

            for (int i = 0; i < xCount; i++)
            {
                deposit.Add(finalResult.ToList().Skip(i).FirstOrDefault().depositSum);
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            if (names.Count() > 6)
            {
                decimal depositSum = 0;
                for (int i = 6; i < names.Count(); i++)
                {
                    depositSum = depositSum + finalResult.ToList().Skip(i).FirstOrDefault().depositSum;
                }
                if (!(depositSum == 0))
                {
                    deposit.Add(depositSum);

                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            for (int i = 0; i < deposit.Count(); i++)
            {
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            string title = "";
            if (_transtype == "d") title = AppSettings.resourcemanager.GetString("trDeposit");
            else if (_transtype == "p") title = AppSettings.resourcemanager.GetString("trPull");

            rowChartData.Add(
          new LineSeries
          {
              Values = deposit.AsChartValues(),
              Title = title
          });

            DataContext = this;
            rowChart.Series = rowChartData;
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_formBranch.SelectedItem = null;
                cb_toBranch.SelectedItem = null;
                cb_formPos.SelectedItem = null;
                cb_toPos.SelectedItem = null;
                cb_Accountant.SelectedItem = null;
                chk_allFromBranch.IsChecked = true;
                chk_allToBranch.IsChecked = true;
                chk_allFromPos.IsChecked = true;
                chk_allToPos.IsChecked = true;
                chk_allAccountant.IsChecked = true;
                chk_twoWay.IsChecked = false;
                dp_StartDate.SelectedDate = null;
                dp_EndDate.SelectedDate = null;
                txt_search.Text = "";

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

                //dgPayments.ItemsSource = fillList()
                dgPayments.ItemsSource = posLst
                                            .Where(obj => (
                                            obj.transNum.Contains(txt_search.Text) ||
                                            obj.branchName.Contains(txt_search.Text) ||
                                            obj.branch2Name.Contains(txt_search.Text) ||
                                            obj.posName.Contains(txt_search.Text) ||
                                            obj.pos2Name.Contains(txt_search.Text) ||
                                            obj.updateUserAcc.Contains(txt_search.Text)
                                            ));

                txt_count.Text = dgPayments.Items.Count.ToString();

                
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
            string firstTitle = "transfers";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\StatisticReport\Accounts\Pos\Ar\ArPosAccReport.rdlc";

            }
            else
            {
                addpath = @"\Reports\StatisticReport\Accounts\Pos\En\PosAccReport.rdlc";
            }
            secondTitle = "pos";


            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("trAccounting") + " / " + subTitle;
            if (_transtype == "d")
            {
                Title = Title + "/" + AppSettings.resourcemanagerreport.GetString("trDeposits");

                paramarr.Add(new ReportParameter("trPos1Header", AppSettings.resourcemanagerreport.GetString("trDepositor")));
                paramarr.Add(new ReportParameter("trPos2Header", AppSettings.resourcemanagerreport.GetString("trRecepient")));

                paramarr.Add(new ReportParameter("trBranch1Header", AppSettings.resourcemanagerreport.GetString("trFromBranch")));
                paramarr.Add(new ReportParameter("trBranch2Header", AppSettings.resourcemanagerreport.GetString("trToBranch")));
            }
            else if (_transtype == "p")
            {

                Title = Title + "/" + AppSettings.resourcemanagerreport.GetString("trReceives");
                paramarr.Add(new ReportParameter("trPos1Header", AppSettings.resourcemanagerreport.GetString("trRecepient")));
                paramarr.Add(new ReportParameter("trPos2Header", AppSettings.resourcemanagerreport.GetString("trDepositor")));

                paramarr.Add(new ReportParameter("trBranch1Header", AppSettings.resourcemanagerreport.GetString("trToBranch")));
                paramarr.Add(new ReportParameter("trBranch2Header", AppSettings.resourcemanagerreport.GetString("trFromBranch")));

            }


            paramarr.Add(new ReportParameter("trTitle", Title));
            //clsReports.cashTransferStsPos(temp, rep, reppath, paramarr);
            clsReports.posAccReportSTS(temp, rep, reppath, paramarr);
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
                //    Thread t1 = new Thread(() =>
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
                //    t1.Start();
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

        private void Btn_payments_Click(object sender, RoutedEventArgs e)
        {//deposit
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_formBranch, AppSettings.resourcemanager.GetString("trFromBranch") + "...");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toBranch, AppSettings.resourcemanager.GetString("trToBranch") + "...");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_formPos, AppSettings.resourcemanager.GetString("trDepositor") + "...");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toPos, AppSettings.resourcemanager.GetString("trRecepient") + "...");

                col_fromBranch.Header = AppSettings.resourcemanager.GetString("trFromBranch");
                col_fromPos.Header = AppSettings.resourcemanager.GetString("trDepositor");
                col_toBranch.Header = AppSettings.resourcemanager.GetString("trToBranch");
                col_toPos.Header = AppSettings.resourcemanager.GetString("trRecepient");

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_payments);
                path_payments.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                txt_search.Text = "";

                _transtype = "d";

                chk_allFromBranch.IsChecked = true;
                chk_allFromPos.IsChecked = true;
                chk_allToBranch.IsChecked = true;
                chk_allToPos.IsChecked = true;
                chk_allAccountant.IsChecked = true;

                fillEvents();

                fromBranches = statisticModel.getFromCombo(posLst);
                toBranches = statisticModel.getToCombo(posLst);
                fromPos = statisticModel.getFromPosCombo(posLst);
                toPos = statisticModel.getToPosCombo(posLst);

                fillComboBranches();
                fillComboFromPos();
                fillComboToPos();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        string _transtype = "d";
        private void Btn_pulls_Click(object sender, RoutedEventArgs e)
        {//pull
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_formBranch, AppSettings.resourcemanager.GetString("trToBranch") + "...");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toBranch, AppSettings.resourcemanager.GetString("trFromBranch") + "...");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_formPos, AppSettings.resourcemanager.GetString("trRecepient") + "...");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toPos, AppSettings.resourcemanager.GetString("trDepositor") + "...");

                col_fromBranch.Header = AppSettings.resourcemanager.GetString("trToBranch");
                col_fromPos.Header = AppSettings.resourcemanager.GetString("trRecepient");
                col_toBranch.Header = AppSettings.resourcemanager.GetString("trFromBranch");
                col_toPos.Header = AppSettings.resourcemanager.GetString("trDepositor");

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_pulls);
                path_pulls.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                txt_search.Text = "";

                _transtype = "p";

                chk_allFromBranch.IsChecked = true;
                chk_allFromPos.IsChecked = true;
                chk_allToBranch.IsChecked = true;
                chk_allToPos.IsChecked = true;
                chk_allAccountant.IsChecked = true;

                fillEvents();

                fromBranches = statisticModel.getFromCombo(posLst);
                toBranches = statisticModel.getToCombo(posLst);
                fromPos = statisticModel.getFromPosCombo(posLst);
                toPos = statisticModel.getToPosCombo(posLst);

                fillComboBranches();
                fillComboFromPos();
                fillComboToPos();


                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        public void paint()
        {
            path_payments.Fill = Brushes.White;
            path_pulls.Fill = Brushes.White;
        }

    }
}
