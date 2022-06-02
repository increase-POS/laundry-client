using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Microsoft.Reporting.WinForms;
using netoaster;
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
using static laundryApp.Classes.Statistics;
using Microsoft.Win32;
using System.Threading;


namespace laundryApp.View.reports.accountsReports
{
    /// <summary>
    /// Interaction logic for uc_statementAccountsReports.xaml
    /// </summary>
    public partial class uc_statementAccountsReports : UserControl
    {
        private static uc_statementAccountsReports _instance;
        public static uc_statementAccountsReports Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_statementAccountsReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_statementAccountsReports()
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

        List<CashTransferSts> statement;

        IEnumerable<VendorCombo> vendorCombo;
        IEnumerable<VendorCombo> customerCombo;
        IEnumerable<VendorCombo> userCombo;
        IEnumerable<ShippingCombo> ShippingCombo;

        //report
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public static string repTrRequires = "";

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

                statement = await statisticModel.GetStatement();

                Btn_vendor_Click(btn_vendor, null);

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
            tt_vendor.Content = AppSettings.resourcemanager.GetString("trVendors");
            tt_customer.Content = AppSettings.resourcemanager.GetString("trCustomers");
            tt_user.Content = AppSettings.resourcemanager.GetString("trUsers");
            tt_shipping.Content = AppSettings.resourcemanager.GetString("trShippingCompanies");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendors, AppSettings.resourcemanager.GetString("trVendorHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendorsDate, AppSettings.resourcemanager.GetString("trDateHint"));
            chk_allVendors.Content = AppSettings.resourcemanager.GetString("trAll");

            dgPayments.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo.");
            dgPayments.Columns[1].Header = AppSettings.resourcemanager.GetString("trDate");
            dgPayments.Columns[2].Header = AppSettings.resourcemanager.GetString("trDescription");
            dgPayments.Columns[3].Header = AppSettings.resourcemanager.GetString("trPayment");
            dgPayments.Columns[4].Header = AppSettings.resourcemanager.GetString("trCashTooltip");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_settings.Content = AppSettings.resourcemanager.GetString("trSettings");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
            tt_emailMessage.Content = AppSettings.resourcemanager.GetString("trSendEmail");
           
        }

        private void fillVendorCombo(IEnumerable<VendorCombo> list, ComboBox cb)
        {
            cb.SelectedValuePath = "VendorId";
            cb.DisplayMemberPath = "VendorName";
            cb.ItemsSource = list;
        }

        private void fillSalaryCombo(IEnumerable<VendorCombo> list, ComboBox cb)
        {
            cb.SelectedValuePath = "UserId";
            cb.DisplayMemberPath = "UserAcc";
            cb.ItemsSource = list;
        }
        private void fillShippingCombo(IEnumerable<ShippingCombo> list, ComboBox cb)
        {
            cb.SelectedValuePath = "ShippingId";
            cb.DisplayMemberPath = "ShippingName";
            cb.ItemsSource = list;
        }
        private void fillDateCombo(ComboBox cb)
        {
            cb.Items.Clear();
            if (statement.Count() > 0)
            {
                int firstYear = statement.Min(obj => obj.updateDate.Value.Year);
                int presentYear = DateTime.Now.Year;
                for (int i = firstYear; i <= presentYear; i++)
                {
                    cb.Items.Add(firstYear);
                    firstYear++;
                }
            }
        }

        private List<CashTransferSts> fillList(List<CashTransferSts> payments, ComboBox vendor, ComboBox date)
        {
            var selectedItem1 = vendor.SelectedItem as VendorCombo;
            var selectedItem2 = vendor.SelectedItem as ShippingCombo;
            var selectedItem3 = date.SelectedItem;


            var result = payments.Where(x => (
                      (vendor.SelectedItem != null ? x.agentId == selectedItem1.VendorId : false)
                   && (date.SelectedItem != null ? x.updateDate.Value.Year == (int)selectedItem3 : true)));

            if (selectedTab == 2)
            {
                result = payments.Where(x => (
                         (vendor.SelectedItem != null ? x.userId == selectedItem1.UserId : false)
                      && (date.SelectedItem != null ? x.updateDate.Value.Year == (int)selectedItem3 : true)));
            }

            if (selectedTab == 3)
            {
                result = payments.Where(x => (
                                (vendor.SelectedItem != null ? x.shippingCompanyId == selectedItem2.ShippingId : false)
                             && (date.SelectedItem != null ? x.updateDate.Value.Year == (int)selectedItem3 : true)));
            }

            return result.ToList();
        }

        /*Vendor*/
        /*********************************************************************************/
        string selectedItem = "";
        private void Cb_vendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (cb_vendors.SelectedItem != null)
                    selectedItem = cb_vendors.SelectedItem.ToString();
                else
                    selectedItem = "";
                fillEvents();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allVendors_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_vendorsDate.SelectedItem = null;
                cb_vendorsDate.IsEnabled = false;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allVendors_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_vendorsDate.IsEnabled = true;

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
            //bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);

            bdr_customer.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_vendor.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_user.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_shipping.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;

            path_customer.Fill = Brushes.White;
            path_vendor.Fill = Brushes.White;
            path_user.Fill = Brushes.White;
            path_shipping.Fill = Brushes.White;

        }

        private void hideAllColumn()
        {
            col_date.Visibility = Visibility.Hidden;
            col_amount.Visibility = Visibility.Hidden;
            col_proccesType.Visibility = Visibility.Hidden;
        }

        private void Btn_vendor_Click(object sender, RoutedEventArgs e)
        {//vendors
            try
            {
                HelpClass.StartAwait(grid_main);

                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendors, AppSettings.resourcemanager.GetString("trVendorHint"));
                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                cb_vendors.SelectedItem = null;
                selectedTab = 0;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_vendor);

                path_vendor.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                fillEvents();

                hideAllColumn();
                //show columns
                col_date.Visibility = Visibility.Visible;
                col_amount.Visibility = Visibility.Visible;
                col_proccesType.Visibility = Visibility.Visible;

                chk_allVendors.IsChecked = true;
                fillDateCombo(cb_vendorsDate);
                vendorCombo = statisticModel.getVendorCombo(statement, "v").Where(x => x.VendorId != null);
                fillVendorCombo(vendorCombo, cb_vendors);

               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_customer_Click(object sender, RoutedEventArgs e)
        {//customers
            try
            {
                HelpClass.StartAwait(grid_main);

                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendors, AppSettings.resourcemanager.GetString("trCustomerHint"));
                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                cb_vendors.SelectedItem = null;
                selectedTab = 1;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_customer);
                path_customer.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                fillEvents();

                hideAllColumn();
                //show columns
                col_date.Visibility = Visibility.Visible;
                col_amount.Visibility = Visibility.Visible;
                col_proccesType.Visibility = Visibility.Visible;

                chk_allVendors.IsChecked = true;
                fillDateCombo(cb_vendorsDate);
                //customerCombo = statisticModel.getCustomerForStatementCombo(statement, "c");
                customerCombo = statisticModel.getVendorCombo(statement, "c").Where(x => x.VendorId != null); ;
                fillVendorCombo(customerCombo, cb_vendors);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_user_Click(object sender, RoutedEventArgs e)
        {//users
            try
            {
                HelpClass.StartAwait(grid_main);

                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendors, AppSettings.resourcemanager.GetString("trUserHint"));
                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                cb_vendors.SelectedItem = null;
                selectedTab = 2;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_user);
                path_user.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                fillEvents();

                hideAllColumn();
                //show columns
                col_date.Visibility = Visibility.Visible;
                col_amount.Visibility = Visibility.Visible;
                col_proccesType.Visibility = Visibility.Visible;

                chk_allVendors.IsChecked = true;
                fillDateCombo(cb_vendorsDate);
                userCombo = statisticModel.getUserAcc(statement, "u");
                fillSalaryCombo(userCombo, cb_vendors);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_shipping_Click(object sender, RoutedEventArgs e)
        {//shippings
            try
            {
                HelpClass.StartAwait(grid_main);

                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendors, AppSettings.resourcemanager.GetString("trShippingCompanyHint"));
                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                cb_vendors.SelectedItem = null;
                selectedTab = 3;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_shipping);
                path_shipping.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                fillEvents();

                hideAllColumn();
                //show columns
                col_date.Visibility = Visibility.Visible;
                col_amount.Visibility = Visibility.Visible;
                col_proccesType.Visibility = Visibility.Visible;

                chk_allVendors.IsChecked = true;
                fillDateCombo(cb_vendorsDate);
                //ShippingCombo = statisticModel.getShippingForStatementCombo(statement);
                ShippingCombo = statisticModel.getShippingCombo(statement);
                fillShippingCombo(ShippingCombo, cb_vendors);

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
        private void fillEvents()
        {
            temp = statisticModel.getstate(fillList(statement, cb_vendors, cb_vendorsDate), selectedTab, statement);

            dgPayments.ItemsSource = temp;
            txt_count.Text = temp.Count().ToString();
            decimal cashTotal = temp.Select(x => x.cashTotal).LastOrDefault();

            //bool worthy = false;
            //if (cashTotal >= 0) worthy = true;
            //if(selectedItem.Equals(AppSettings.resourcemanager.GetString("trCashCustomer")))
            //    worthy = true;
            if (cashTotal >= 0)
            //if (worthy)
            {
                txt_total.Text = HelpClass.DecTostring(cashTotal);
                txt_for.Text = AppSettings.resourcemanager.GetString("trWorthy");

                repTrRequires = "trWorthy";
                tb_moneyIcon.Text = AppSettings.Currency;

                bdr_email.Visibility = Visibility.Collapsed;
            }
            else
            {
                cashTotal = -cashTotal;
                txt_total.Text = cashTotal.ToString();
                txt_for.Text = AppSettings.resourcemanager.GetString("trRequired");

                repTrRequires = "trRequired";
                tb_moneyIcon.Text = AppSettings.Currency;
                //if (cb_vendors.SelectedItem != null)
                //{
                bdr_email.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //bdr_email.Visibility = Visibility.Collapsed;
                //}
            }
            fillRowChart();
            fillColumnChart();
            fillPieChart();
        }

        /*Charts*/
        /*********************************************************************************/
        private void fillRowChart()
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>(12);
            List<CashTransferSts> resultList = new List<CashTransferSts>();
            int year = DateTime.Now.Year;
            if (cb_vendorsDate.SelectedItem != null)
            {
                year = (int)cb_vendorsDate.SelectedItem;
            }

            SeriesCollection rowChartData = new SeriesCollection();

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> cash = new List<decimal>();

            LineSeries l = new LineSeries();
            for (int month = 1; month <= 12; month++)
            {
                var firstOfThisMonth = new DateTime(year, month, 1);
                var firstOfNextMonth = firstOfThisMonth.AddMonths(1);
                var drawCash = temp.Where(c => c.updateDate > firstOfThisMonth && c.updateDate <= firstOfNextMonth).Select(x => x.cashTotal).LastOrDefault();

                if (drawCash > 0)
                {
                    names.Add(AppSettings.resourcemanager.GetString("trWorthy"));

                    btn_emailMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    names.Add(AppSettings.resourcemanager.GetString("trRequired"));

                    btn_emailMessage.Visibility = Visibility.Visible;
                }
                cash.Add(drawCash);


                MyAxis.Labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + "/" + 2021);
            }
            l.Values = cash.AsChartValues();
            rowChartData.Add(l);

            DataContext = this;
            rowChart.Series = rowChartData;
        }

        private void fillColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            List<CashTransferSts> resultList = new List<CashTransferSts>();

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<decimal> cash = new List<decimal>();
            List<decimal> card = new List<decimal>();
            List<decimal> doc = new List<decimal>();
            List<decimal> cheque = new List<decimal>();
            List<decimal> inv = new List<decimal>();

            cash.Add(temp.Where(x => x.processType == "cash").Select(x => x.cash.Value).Sum());
            card.Add(temp.Where(x => x.processType == "card").Select(x => x.cash.Value).Sum());
            doc.Add(temp.Where(x => x.processType == "doc").Select(x => x.cash.Value).Sum());
            cheque.Add(temp.Where(x => x.processType == "cheque").Select(x => x.cash.Value).Sum());
            inv.Add(temp.Where(x => x.processType == "inv").Select(x => x.cash.Value).Sum());

            columnChartData.Add(
            new ColumnSeries
            {
                Values = cash.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("trCash")
            });
            columnChartData.Add(
           new ColumnSeries
           {
               Values = card.AsChartValues(),
               DataLabels = true,
               Title = AppSettings.resourcemanager.GetString("trAnotherPaymentMethods")
           });
           // columnChartData.Add(
           //new ColumnSeries
           //{
           //    Values = doc.AsChartValues(),
           //    DataLabels = true,
           //    Title = AppSettings.resourcemanager.GetString("trDocument")

           //});
            columnChartData.Add(
           new ColumnSeries
           {
               Values = cheque.AsChartValues(),
               DataLabels = true,
               Title = AppSettings.resourcemanager.GetString("trCheque")
           });

            //columnChartData.Add(
            //new ColumnSeries
            //{
            //    Values = inv.AsChartValues(),
            //    DataLabels = true,
            //    Title = AppSettings.resourcemanager.GetString("tr_Invoice")
            //});

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillPieChart()
        {
            List<string> titles = new List<string>();
            List<int> resultList = new List<int>();
            titles.Clear();

            resultList.Add(temp.Where(x => x.processType != "inv" && x.transType == "p").Count());
            resultList.Add(temp.Where(x => x.processType != "inv" && x.transType == "d").Count());
            //resultList.Add(temp.Where(x => x.processType == "inv").Count());
            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < resultList.Count(); i++)
            {
                List<int> final = new List<int>();
                List<string> lable = new List<string>()
                {
                    AppSettings.resourcemanager.GetString("trOnePayment"),
                    AppSettings.resourcemanager.GetString("trOneDeposit"),
                    //AppSettings.resourcemanager.GetString("tr_Invoice")
                };
                final.Add(resultList.Skip(i).FirstOrDefault());
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

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                txt_search.Text = "";

                cb_vendors.SelectedItem = null;
                cb_vendorsDate.SelectedItem = null;
                chk_allVendors.IsChecked = false;
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
            IEnumerable<CashTransferSts> t = null;
            try
            {
                HelpClass.StartAwait(grid_main);

                temp = statisticModel.getstate(fillList(statement, cb_vendors, cb_vendorsDate), selectedTab, statement);

                t = temp.Where(obj => obj.transNum.Contains(txt_search.Text) ||
                obj.Description.Contains(txt_search.Text) ||
                obj.Description1.Contains(txt_search.Text)).ToList();

                dgPayments.ItemsSource = t;

                txt_count.Text = t.Count().ToString();
               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region report email
        private void Btn_emailMessage_Click(object sender, RoutedEventArgs e)
        {//email
            try
            {
                HelpClass.StartAwait(grid_main);

                //Thread t1 = new Thread(() =>
                //{
                sendEmail();
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

        private async void sendEmail()
        {
            //if (txt_for.Text == "Required")
            if (txt_for.Text == AppSettings.resourcemanager.GetString("trRequired"))
            {
                string total = txt_total.Text;
                SysEmails email = new SysEmails();
                EmailClass mailtosend = new EmailClass();

                Agent toAgent = new Agent();
                User toUser = new User();
                ShippingCompanies toShipCompanies = new ShippingCompanies();
                string emailto = "";
                bool toemailexist = false;
                email = await email.GetByBranchIdandSide((int)MainWindow.branchLogin.branchId, "accounting");
                switch (selectedTab)
                {
                    case 0:
                        //vendor
                        var objct0 = cb_vendors.SelectedItem as VendorCombo;

                        int agentId = (int)objct0.VendorId;
                        toAgent = await toAgent.getAgentById(agentId);
                        emailto = toAgent.email;

                        if (emailto is null || emailto == "")
                        {
                            toemailexist = false;

                            this.Dispatcher.Invoke(() =>
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheVendorHasNoEmail"), animation: ToasterAnimation.FadeIn);
                            });
                        }
                        else
                        {
                            toemailexist = true;
                        }

                        break;

                    case 1:
                        var objct1 = cb_vendors.SelectedItem as VendorCombo;
                        agentId = (int)objct1.VendorId;
                        toAgent = await toAgent.getAgentById(agentId);
                        emailto = toAgent.email;

                        if (emailto is null || emailto == "")
                        {
                            toemailexist = false;
                            this.Dispatcher.Invoke(() =>
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheCustomerHasNoEmail"), animation: ToasterAnimation.FadeIn);
                            });
                        }
                        else
                        {
                            toemailexist = true;
                        }
                        break;
                    case 2:
                        var objct2 = cb_vendors.SelectedItem as VendorCombo;
                        int userId = (int)objct2.UserId;
                        toUser = await toUser.getUserById(userId);
                        emailto = toUser.email;

                        if (emailto is null || emailto == "")
                        {
                            toemailexist = false;
                            this.Dispatcher.Invoke(() =>
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheUserHasNoEmail"), animation: ToasterAnimation.FadeIn);
                            });
                        }
                        else
                        {
                            toemailexist = true;
                        }
                        break;
                    case 3:
                        var objct3 = cb_vendors.SelectedItem as ShippingCombo;
                        int shipId = (int)objct3.ShippingId;

                        toShipCompanies = await toShipCompanies.GetByID(shipId);
                        emailto = toShipCompanies.email;

                        if (emailto is null || emailto == "")
                        {
                            toemailexist = false;
                            this.Dispatcher.Invoke(() =>
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTheShippingCompaniesHasNoEmail"), animation: ToasterAnimation.FadeIn);
                            });
                        }
                        else
                        {
                            toemailexist = true;
                        }

                        break;

                }

                if (email != null)
                {
                    if (email.emailId == 0)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoEmailForThisDept"), animation: ToasterAnimation.FadeIn);
                        });
                    }
                    else
                    {
                        if (toemailexist)
                        {
                            SetValues setvmodel = new SetValues();

                            List<SetValues> setvlist = new List<SetValues>();

                            setvlist = await setvmodel.GetBySetName("required_email_temp");

                            mailtosend = mailtosend.fillRequirdTempData(total, emailto, email, setvlist);

                            string msg = "";
                            //Thread t1 = new Thread(() =>
                            //{
                                msg = mailtosend.Sendmail();// temp comment
                                if (msg == "Failure sending mail.")
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoConnection"), animation: ToasterAnimation.FadeIn);
                                    });
                                }
                                else if (msg == "mailsent")
                                {

                                    this.Dispatcher.Invoke(() =>
                                    {
                                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailSent"), animation: ToasterAnimation.FadeIn);
                                    });

                                }
                                else
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailNotSent"), animation: ToasterAnimation.FadeIn);
                                    });
                                }
                            //});
                            //t1.Start();
                        }
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trNoEmailForThisDept"), animation: ToasterAnimation.FadeIn);
                    });
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                });
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



        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            string firstTitle = "accountStatement";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\StatisticReport\Accounts\AccountStatement\Ar\ArStatement.rdlc";

            }
            else
            {
                addpath = @"\Reports\StatisticReport\Accounts\AccountStatement\En\EnStatement.rdlc";

            }
            if (selectedTab == 0)
            {
                secondTitle = "vendors";
            }
            else if (selectedTab == 1)
            {
                secondTitle = "customers";

            }
            else if (selectedTab == 2)
            {
                secondTitle = "users";

            }
            else if (selectedTab == 3)
            {
                secondTitle = "shipping";

            }

            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            Title = AppSettings.resourcemanagerreport.GetString("trAccounting") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            clsReports.cashTransferStsStatement(temp, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);
            if (repTrRequires == "trRequired")
            {
                repTrRequires = AppSettings.resourcemanagerreport.GetString("trRequired");
            }
            else
            {
                repTrRequires = AppSettings.resourcemanagerreport.GetString("trWorthy");
            }
            paramarr.Add(new ReportParameter("trRequired", repTrRequires));
            paramarr.Add(new ReportParameter("finalAccount", txt_total.Text));
            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));

            rep.SetParameters(paramarr);

            rep.Refresh();
        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {
                HelpClass.StartAwait(grid_main);

                /////////////////////////////////////
                //Thread t1 = new Thread(() =>
                //{
                    pdfStatement();
                //});
                //t1.Start();
                //////////////////////////////////////
               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void pdfStatement()
        {
            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
            });
        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
                HelpClass.StartAwait(grid_main);

                /////////////////////////////////////
                //Thread t1 = new Thread(() =>
                //{
                    printStatement();
                //});
                //t1.Start();
                //////////////////////////////////////
               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void printStatement()
        {
            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
            });
        }

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
                HelpClass.StartAwait(grid_main);

                //Thread t1 = new Thread(() =>
                //{
                ExcelStatement();

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

        private void ExcelStatement()
        {
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
        }

        #endregion
    }
}
