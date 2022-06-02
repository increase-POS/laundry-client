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
    /// Interaction logic for uc_bankAccountsReports.xaml
    /// </summary>
    public partial class uc_bankAccountsReports : UserControl
    {
        private static uc_bankAccountsReports _instance;
        public static uc_bankAccountsReports Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_bankAccountsReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_bankAccountsReports()
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
        List<CashTransferSts> payments;
        List<CashTransferSts> recipient;

        IEnumerable<VendorCombo> userPaymentsCombo;
        IEnumerable<AccountantCombo> accPaymentsCombo;

        int selectedTab = 0;

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                payments = await statisticModel.GetPayments();// Deposite
                recipient = await statisticModel.GetReceipt();// Pull

                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                Btn_vendor_Click(btn_payments, null);
                
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
            tt_payments.Content = AppSettings.resourcemanager.GetString("trPull");
            tt_recipient.Content = AppSettings.resourcemanager.GetString("trDeposit");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_paymentsBank, AppSettings.resourcemanager.GetString("trBank") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_paymentsUser, AppSettings.resourcemanager.GetString("trUser") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_paymentsAccountant, AppSettings.resourcemanager.GetString("trAccoutant") + "...");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_paymentsStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_paymentsEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));

            chk_allPaymentsBanks.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allPyamentsUser.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allpaymentsAccountant.Content = AppSettings.resourcemanager.GetString("trAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_tansNum.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_Type.Header = AppSettings.resourcemanager.GetString("trType");
            col_updateUserAcc.Header = AppSettings.resourcemanager.GetString("trAccoutant");
            col_Bank.Header = AppSettings.resourcemanager.GetString("trBank");
            col_user.Header = AppSettings.resourcemanager.GetString("trUser");
            col_updateDate.Header = AppSettings.resourcemanager.GetString("trDate");
            col_cash.Header = AppSettings.resourcemanager.GetString("trAmount");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }

        private void fillBanksCombo(List<CashTransferSts> lst)
        {
            var iulist = lst.Where(g => g.bankId != null).GroupBy(g => g.bankId).Select(g => new { BankId = g.FirstOrDefault().bankId, BankName = g.FirstOrDefault().bankName }).ToList();
            cb_paymentsBank.SelectedValuePath = "BankId";
            cb_paymentsBank.DisplayMemberPath = "BankName";
            cb_paymentsBank.ItemsSource = iulist;
        }

        private void fillUserCombo(IEnumerable<VendorCombo> list, ComboBox cb)
        {
            cb.SelectedValuePath = "UserId";
            cb.DisplayMemberPath = "UserAcc";
            cb.ItemsSource = list;
        }

        private void fillAccCombo(IEnumerable<AccountantCombo> list, ComboBox cb)
        {
            cb.SelectedValuePath = "Accountant";
            cb.DisplayMemberPath = "Accountant";
            cb.ItemsSource = list;
        }

        List<CashTransferSts> bankLst;
        private List<CashTransferSts> fillList(List<CashTransferSts> payments, ComboBox vendor, ComboBox payType, ComboBox accountant
           , DatePicker startDate, DatePicker endDate)
        {
            var result = payments.Where(x => (
                           (cb_paymentsBank.SelectedItem != null ? x.bankId == (int)cb_paymentsBank.SelectedValue : true)
                        && (payType.SelectedItem != null ? x.userId == (int)cb_paymentsUser.SelectedValue : true)
                        && (accountant.SelectedItem != null ? x.updateUserAcc == (string)cb_paymentsAccountant.SelectedValue : true)
                        && (startDate.SelectedDate != null ? x.updateDate >= startDate.SelectedDate : true)
                        && (endDate.SelectedDate != null ? x.updateDate <= endDate.SelectedDate : true)));

            bankLst = result.ToList();
            return result.ToList();
        }

        /*********************************************************************/


        public void paint()
        {
            //bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);


            bdr_payments.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_recipient.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            path_payments.Fill = Brushes.White;
            path_recipient.Fill = Brushes.White;
        }

        private void isEnabledButtons()
        {
            btn_payments.IsEnabled = true;
            btn_recipient.IsEnabled = true;
        }

        private void hideAllColumn()
        {
            col_tansNum.Visibility = Visibility.Hidden;
            col_Type.Visibility = Visibility.Hidden;
            col_updateUserAcc.Visibility = Visibility.Hidden;
            col_user.Visibility = Visibility.Hidden;
            col_updateDate.Visibility = Visibility.Hidden;
            col_cash.Visibility = Visibility.Hidden;

        }

        private void Btn_vendor_Click(object sender, RoutedEventArgs e)
        {//payments
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                hideAllColumn();
                selectedTab = 0;
                //view columns
                col_tansNum.Visibility = Visibility.Visible;
                col_Type.Visibility = Visibility.Visible;
                col_updateUserAcc.Visibility = Visibility.Visible;
                col_user.Visibility = Visibility.Visible;
                col_updateDate.Visibility = Visibility.Visible;
                col_cash.Visibility = Visibility.Visible;

                txt_search.Text = "";
                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_payments);
                path_payments.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                fillBanksCombo(payments);

                userPaymentsCombo = statisticModel.getUserAcc(payments, "bn");
                fillUserCombo(userPaymentsCombo, cb_paymentsUser);

                accPaymentsCombo = statisticModel.getAccounantCombo(payments, "bn");
                fillAccCombo(accPaymentsCombo, cb_paymentsAccountant);

                fillEvents(payments);

                chk_allPaymentsBanks.IsChecked = true;
                chk_allPyamentsUser.IsChecked = true;
                chk_allpaymentsAccountant.IsChecked = true;

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_customer_Click(object sender, RoutedEventArgs e)
        {//received
            try
            {
                
                HelpClass.StartAwait(grid_main);
                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                selectedTab = 1;
                hideAllColumn();
                txt_search.Text = "";

                //view columns
                col_tansNum.Visibility = Visibility.Visible;
                col_Type.Visibility = Visibility.Visible;
                col_updateUserAcc.Visibility = Visibility.Visible;
                col_user.Visibility = Visibility.Visible;
                col_updateDate.Visibility = Visibility.Visible;
                col_cash.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_recipient);
                path_recipient.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                fillBanksCombo(recipient);

                userPaymentsCombo = statisticModel.getUserAcc(recipient, "bn");
                fillUserCombo(userPaymentsCombo, cb_paymentsUser);

                accPaymentsCombo = statisticModel.getAccounantCombo(recipient, "bn");
                fillAccCombo(accPaymentsCombo, cb_paymentsAccountant);

                fillEvents(recipient);

                chk_allPaymentsBanks.IsChecked = true;
                chk_allPyamentsUser.IsChecked = true;
                chk_allpaymentsAccountant.IsChecked = true;

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        /*Fill Events*/
        /*********************************************************************************/
        IEnumerable<CashTransferSts> temp = null;

        private void fillEvents(List<CashTransferSts> lst)
        {
            temp = fillList(lst, cb_paymentsBank, cb_paymentsUser, cb_paymentsAccountant, dp_paymentsStartDate, dp_paymentsEndDate).Where(s => s.side == "bn" && s.isConfirm == 1);
            dgPayments.ItemsSource = temp;
            txt_count.Text = dgPayments.Items.Count.ToString();
            //fillPieChart();
            fillColumnChart();
            fillRowChart();
        }

        /*Charts*/
        /*********************************************************************************/

        private void fillPieChart()
        {
            //List<string> titles = new List<string>();
            //List<int> resultList = new List<int>();
            //titles.Clear();
            //var temp = fillList(payments, cb_paymentsBank, cb_paymentsUser, cb_paymentsAccountant, dp_paymentsStartDate, dp_paymentsEndDate).Where(s => s.side == "bn");
            //if (selectedTab == 1)
            //{
            //    temp = fillList(recipient, cb_paymentsBank, cb_paymentsUser, cb_paymentsAccountant, dp_paymentsStartDate, dp_paymentsEndDate).Where(s => s.side == "bn");
            //}

            //var result = temp
            //    .GroupBy(s => new { s.transType })
            //    .Select(s => new CashTransferSts
            //    {
            //        processTypeCount = s.Count(),
            //        processType = s.FirstOrDefault().transType,
            //    });
            //resultList = result.Select(m => m.processTypeCount).ToList();
            //titles = result.Select(m => m.transType).ToList();
            //for (int t = 0; t < titles.Count; t++)
            //{
            //    string s = "";
            //    switch (titles[t])
            //    {
            //        case "p": s = AppSettings.resourcemanager.GetString("trPull"); break;
            //        case "d": s = AppSettings.resourcemanager.GetString("trDeposit"); break;
            //    }
            //    titles[t] = s;
            //}
            //SeriesCollection piechartData = new SeriesCollection();
            //for (int i = 0; i < resultList.Count(); i++)
            //{
            //    List<int> final = new List<int>();
            //    List<string> lable = new List<string>();

            //    final.Add(resultList.Skip(i).FirstOrDefault());
            //    lable = titles;
            //    piechartData.Add(
            //      new PieSeries
            //      {
            //          Values = final.AsChartValues(),
            //          Title = lable.Skip(i).FirstOrDefault(),
            //          DataLabels = true,
            //      }
            //  );

            //}
            //chart1.Series = piechartData;
        }
        private void fillColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            List<CashTransferSts> resultList = new List<CashTransferSts>();

            var res = temp.GroupBy(x => new { x.bankId, x.transType }).Select(x => new CashTransferSts
            {
                bankName = x.FirstOrDefault().bankName,
                transType = x.FirstOrDefault().transType,
                bankId = x.FirstOrDefault().bankId,
                pullCount = x.Where(g => g.transType == "d").Count(),
                depositCount = x.Where(g => g.transType == "p").Count(),
                pullSum = x.Where(g => g.transType == "p").Select(g => g.cash.Value).Sum(),
                depositSum = x.Where(g => g.transType == "d").Select(g => g.cash.Value).Sum()
            });
            resultList = res.GroupBy(x => x.bankId).Select(x => new CashTransferSts
            {
                bankName = x.FirstOrDefault().bankName,
                pullCount = x.Sum(g => g.pullCount),
                depositCount = x.Sum(g => g.depositCount),
                bankId = x.FirstOrDefault().bankId,
                transType = x.FirstOrDefault().transType,
                pullSum = x.FirstOrDefault().pullSum,
                depositSum = x.FirstOrDefault().depositSum
            }
            ).ToList();

            var tempName = res.GroupBy(s => new { s.bankId }).Select(s => new
            {
                itemName = s.FirstOrDefault().bankName,
            });
            names.AddRange(tempName.Select(nn => nn.itemName));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> deposite = new List<decimal>();
            List<decimal> pull = new List<decimal>();

            int xCount = 6;
            if (resultList.Count() <= 6)
                xCount = resultList.Count();
            for (int i = 0; i < xCount; i++)
            {
                deposite.Add(resultList.ToList().Skip(i).FirstOrDefault().depositSum);
                pull.Add(resultList.ToList().Skip(i).FirstOrDefault().pullSum);

                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            if (resultList.Count() > 6)
            {
                decimal pullSum = 0, depositSum = 0;
                for (int i = 6; i < resultList.Count; i++)
                {
                    pullSum = pullSum + resultList.ToList().Skip(i).FirstOrDefault().pullSum;
                    depositSum = depositSum + resultList.ToList().Skip(i).FirstOrDefault().depositSum;

                }
                if (!((pullSum == 0) && (depositSum == 0)))
                {
                    deposite.Add(depositSum);
                    pull.Add(pullSum);

                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = deposite.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("trDeposit")
            });
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = pull.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("trPull")
            });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillRowChart()
        {
            int endYear = DateTime.Now.Year;
            int startYear = endYear - 1;
            int startMonth = DateTime.Now.Month;
            int endMonth = startMonth;

            IEnumerable<CashTransferSts> payLst = null;
            IEnumerable<CashTransferSts> recLst = null;

            payLst = fillList(payments, cb_paymentsBank, cb_paymentsUser, cb_paymentsAccountant, dp_paymentsStartDate, dp_paymentsEndDate).Where(s => s.side == "bn" && s.isConfirm == 1);
            recLst = fillList(recipient, cb_paymentsBank, cb_paymentsUser, cb_paymentsAccountant, dp_paymentsStartDate, dp_paymentsEndDate).Where(s => s.side == "bn" && s.isConfirm == 1);

            if (dp_paymentsStartDate.SelectedDate != null && dp_paymentsEndDate.SelectedDate != null)
            {
                startYear = dp_paymentsStartDate.SelectedDate.Value.Year;
                endYear = dp_paymentsEndDate.SelectedDate.Value.Year;
                startMonth = dp_paymentsStartDate.SelectedDate.Value.Month;
                endMonth = dp_paymentsEndDate.SelectedDate.Value.Month;
            }

            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            List<CashTransferSts> resultList = new List<CashTransferSts>();
           
            SeriesCollection rowChartData = new SeriesCollection();
            var tempName = temp.GroupBy(s => new { s.bankId }).Select(s => new
            {
                itemName = s.FirstOrDefault().updateDate,
            });
            names.AddRange(tempName.Select(nn => nn.itemName.ToString()));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> cash = new List<decimal>();
            List<decimal> card = new List<decimal>();

            if (endYear - startYear <= 1)
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = startMonth; month <= 12; month++)
                    {
                        var firstOfThisMonth = new DateTime(year, month, 1);
                        var firstOfNextMonth = firstOfThisMonth.AddMonths(1);
                        var drawCash = payLst.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth && c.transType == "p").Sum(c => c.cash);
                        var drawCard = recLst.ToList().Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth && c.transType == "d").Sum(c => c.cash);
                        cash.Add((decimal)drawCash);
                        card.Add((decimal)drawCard);

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
                    var drawCash = payLst.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear && c.processType == "cash").Count();
                    var drawCard = recLst.ToList().Where(c => c.updateDate > firstOfThisYear && c.updateDate <= firstOfNextMYear && c.processType == "cash").Count();
                    cash.Add(drawCash);
                    card.Add(drawCard);
                    MyAxis.Labels.Add(year.ToString());
                }
            }
            rowChartData.Add(
          new LineSeries
          {
              Values = card.AsChartValues(),
              Title = AppSettings.resourcemanager.GetString("trDeposit")
          }); ;
            rowChartData.Add(
         new LineSeries
         {
             Values = cash.AsChartValues(),
             Title = AppSettings.resourcemanager.GetString("trPull")
         });

            DataContext = this;
            rowChart.Series = rowChartData;
        }

        private void Chk_allBanks_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_paymentsBank.IsEnabled = false;
                cb_paymentsBank.SelectedItem = null;
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allBanks_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_paymentsBank.IsEnabled = true;
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_paymentsUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                fillEvents(payments);
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allPyamentsUser_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);

                cb_paymentsUser.IsEnabled = false;
                cb_paymentsUser.SelectedItem = null;

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allPyamentsUser_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);

                cb_paymentsUser.IsEnabled = true;

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allpaymentsAccountant_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);

                cb_paymentsAccountant.IsEnabled = false;
                cb_paymentsAccountant.SelectedItem = null;
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allpaymentsAccountant_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);

                cb_paymentsAccountant.IsEnabled = true;

                
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
            try
            {
                HelpClass.StartAwait(grid_main);

                fillByType();
                
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

                fillByType();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void fillByType()
        {
            if (selectedTab == 0)
                fillEvents(payments);
            else if (selectedTab == 1)
                fillEvents(recipient);
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                txt_search.Text = "";

                cb_paymentsBank.SelectedItem = null;
                cb_paymentsUser.SelectedItem = null;
                cb_paymentsAccountant.SelectedItem = null;
                chk_allpaymentsAccountant.IsChecked = false;
                chk_allPaymentsBanks.IsChecked = false;
                chk_allPyamentsUser.IsChecked = false;
                dp_paymentsStartDate.SelectedDate = null;
                dp_paymentsEndDate.SelectedDate = null;

                fillByType();

                
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

                IEnumerable<CashTransferSts> tempSearch;
                if (selectedTab == 0)
                    tempSearch = fillList(payments, cb_paymentsBank, cb_paymentsUser, cb_paymentsAccountant, dp_paymentsStartDate, dp_paymentsEndDate).Where(s => s.side == "bn" && s.isConfirm == 1);
                else
                    tempSearch = fillList(recipient, cb_paymentsBank, cb_paymentsUser, cb_paymentsAccountant, dp_paymentsStartDate, dp_paymentsEndDate).Where(s => s.side == "bn" && s.isConfirm == 1);

                dgPayments.ItemsSource = tempSearch.Where(obj => (
                    obj.transNum.Contains(txt_search.Text) ||
                    obj.bankName.Contains(txt_search.Text) ||
                    obj.updateUserAcc.Contains(txt_search.Text) ||
                    obj.userAcc.Contains(txt_search.Text)
                    ));

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region report

        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
       
        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            string firstTitle = "banksReport";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Accounts\Bank\Ar\ArDeposite.rdlc";
                    

                    secondTitle = "pull";
                  

                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Accounts\Bank\Ar\ArPull.rdlc";
                  
                    secondTitle = "deposit";
                }

            }
            else
            {
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Accounts\Bank\En\Deposite.rdlc";
                    secondTitle = "pull";

                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Accounts\Bank\En\Pull.rdlc";
                    secondTitle = "deposit";

                }

            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("trAccounting") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            clsReports.cashTransferStsBank(temp, rep, reppath, paramarr);
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


                //    });
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
                Window.GetWindow(this).Opacity = 0.2;
                string pdfpath = "";

                List<ReportParameter> paramarr = new List<ReportParameter>();

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
