using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using static laundryApp.MainWindow;

namespace laundryApp.View
{
    /// <summary>
    /// Interaction logic for uc_home.xaml
    /// </summary>
    public partial class uc_home : UserControl
    {
        int x;
        int y;
        int z;
        int value_items;
        int value_purchase;
        int value_sales;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer1 = new DispatcherTimer();
        Tables tablesModel = new Tables();


        private static uc_home _instance;
        public static uc_home Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_home();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_home()
        {
            try
            {
                InitializeComponent();
            timerAnimation();
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }
        int NumberDaysInMonth { get; set; }
        DispatcherTimer threadtimer10 = new DispatcherTimer();
        DispatcherTimer threadtimer30 = new DispatcherTimer();
        //public static int secondTimer10 = 10;
        public static int secondTimer10 = 10;
        //public static int secondTimer30 = 30;
        public static int secondTimer30 = 30;
        int SkipBestSeller = 0;
        int SkipIUStorage = 0;
        bool firstLoad = true;
        string branchesPermission = "dashboard_branches";
        Dash dash = new Dash();
        User user = new User();
        //List<User> users = new List<User>();
        ImageBrush brush = new ImageBrush();

        public List<BestSeller> listBestSeller = new List<BestSeller>();
        public List<TotalPurSale> listMonthlyInvoice = new List<TotalPurSale>();
        public List<IUStorage> listIUStorage = new List<IUStorage>();
        public List<ItemUnit> IUList = new List<ItemUnit>();
        bool firstLoading = true;
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if(threadtimer10 != null)
            threadtimer10.Stop();
            if(threadtimer30 != null)
            threadtimer30.Stop();
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 
                //    SectionData.StartAwait(grid_main);

                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight; 
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion
                if (AppSettings.typesOfService_clothes != "1")
                {
                    txt_clothes.Foreground = Application.Current.Resources["veryLightGrey"] as SolidColorBrush;
                    txt_clothesValue.Foreground = Application.Current.Resources["veryLightGrey"] as SolidColorBrush;
                }
                if (AppSettings.typesOfService_carpets != "1")
                {
                    txt_carpets.Foreground = Application.Current.Resources["veryLightGrey"] as SolidColorBrush;
                    txt_carpetsValue.Foreground = Application.Current.Resources["veryLightGrey"] as SolidColorBrush;
                }
                if (AppSettings.typesOfService_cars != "1")
                {
                    txt_cars.Foreground = Application.Current.Resources["veryLightGrey"] as SolidColorBrush;
                    txt_carsValue.Foreground = Application.Current.Resources["veryLightGrey"] as SolidColorBrush;
                }

                if (FillCombo.itemUnitsUsersList is null)
                   await FillCombo.RefreshItemUnitUser();
                refrishIUList(FillCombo.itemUnitsUsersList);

               

                SkipBestSeller = 0;
                SkipIUStorage = 0;
                firstLoad = true;



                CalculateNumberDaysInMonth calculate = new CalculateNumberDaysInMonth();
                NumberDaysInMonth = calculate.getdays(DateTime.Now);

                //await refreshViewTask();

                if (firstLoading)
                {
                    await FillCombo.fillBranchesWithAll(cb_branch,  "bs");
                    //await SectionData.fillBranchesWithAll(cb_branch);
                    firstLoading = false;
                    cb_branch.SelectedValue = MainWindow.branchLogin.branchId;
                }
                if (FillCombo.groupObject.HasPermissionAction(branchesPermission, FillCombo.groupObjects, "one"))
                    cb_branch.IsEnabled = true;
                else cb_branch.IsEnabled = false;

                rdb_times.IsChecked = true;


                //txt_rightReserved.Text =DateTime.Now.Date.Year + " © All Right Reserved for Increase";
            starTimerAfter10();
            starTimerAfter30();
                // 
                //    SectionData.EndAwait(grid_main);
            }
            catch 
            {
                // 
                //    SectionData.EndAwait(grid_main);
                //SectionData.ExceptionMessage(ex, this);
            }

        }
        async void starTimerAfter10()
        {
            await Task.Delay(10000);
            // thread 10
            //threadtimer10 = new DispatcherTimer();
            threadtimer10.Interval = TimeSpan.FromSeconds(secondTimer10);
            threadtimer10.Tick += timer_Thread10;
            threadtimer10.Start();
            ////////////////////
        }
        async void starTimerAfter30()
        {
            await Task.Delay(30000);
            //thread 30
            //threadtimer30 = new DispatcherTimer();
            threadtimer30.Interval = TimeSpan.FromSeconds(secondTimer30);
            threadtimer30.Tick += timer_Thread30;
            threadtimer30.Start();
            ////////////////////
        }
        private void translate()
        {
            txt_totalCash.Text = AppSettings.resourcemanager.GetString("trCashBalance");
            txt_emptyTables.Text = AppSettings.resourcemanager.GetString("emptyTables");
            txt_openTables.Text = AppSettings.resourcemanager.GetString("openTables");
            txt_reservations.Text = AppSettings.resourcemanager.GetString("trReservations");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branch, AppSettings.resourcemanager.GetString("trStore/BranchHint"));
            txt_mostPopularFoods.Text = AppSettings.resourcemanager.GetString("mostPopularFoods");
            txt_storageTitle.Text = AppSettings.resourcemanager.GetString("trStorage");
            txt_bestOf.Text = AppSettings.resourcemanager.GetString("bestOf") + "...";
            txt_invoicesTypeInDay.Text = AppSettings.resourcemanager.GetString("invoicesTypesInDay");

            txt_times.Text = AppSettings.resourcemanager.GetString("times");
            txt_days.Text = AppSettings.resourcemanager.GetString("trDays");
            txt_months.Text = AppSettings.resourcemanager.GetString("months");
            txt_clothes.Text = AppSettings.resourcemanager.GetString("clothes");
            txt_carpets.Text = AppSettings.resourcemanager.GetString("carpets");
            txt_cars.Text = AppSettings.resourcemanager.GetString("cars");

            txt_userOnlineTitle.Text = AppSettings.resourcemanager.GetString("trUsers");
            txt_purchaseAndSalesTitle.Text = AppSettings.resourcemanager.GetString("trPurchase&Sales");
            txt_branchOnlineTitle.Text = AppSettings.resourcemanager.GetString("trBranch&Store");
            txt_dailyPurchaseTitle.Text = AppSettings.resourcemanager.GetString("trDailyPurchase");
            txt_dailySalesTitle.Text = AppSettings.resourcemanager.GetString("trDailySales");

            txt_userOnline.Text = txt_branchOnline.Text = AppSettings.resourcemanager.GetString("trOnline") + ":";
            txt_countDailyPurchase.Text = txt_countDailySales.Text = AppSettings.resourcemanager.GetString("trCount") + ":";

            txt_cashSympol.Text = AppSettings.Currency;

        }
        void timer_Thread30(object sendert, EventArgs et)
        {
            try
            {
                refreshView();
            }
            catch
            {}
        }
        void timer_Thread10(object sendert, EventArgs et)
        {
            try
            {

                #region BestSeller
                if (!firstLoad)
                {
                    if ((listBestSeller.Count < 4 || SkipBestSeller == 2) || (listBestSeller.Count == 3)
                     || (listBestSeller.Count >= 4 && listBestSeller.Count <= 6 && SkipBestSeller == 1)
                     || (listBestSeller.Count >= 7 && listBestSeller.Count <= 9 && SkipBestSeller == 2))
                        SkipBestSeller = 0;
                    else if (listBestSeller.Count < 7 && SkipBestSeller < 1)
                        SkipBestSeller++;
                    else if (listBestSeller.Count < 10 && SkipBestSeller < 2)
                        SkipBestSeller++;
                }
                paginationBestSeller(listBestSeller, SkipBestSeller);
                #endregion
                #region IUStorage
                if (!firstLoad)
                {
                    if ((listIUStorage.Count < 4 || SkipIUStorage == 2) || (listIUStorage.Count == 3)
                      || (listIUStorage.Count >= 4 && listIUStorage.Count <= 6 && SkipIUStorage == 1)
                      || (listIUStorage.Count >= 7 && listIUStorage.Count <= 9 && SkipIUStorage == 2))
                        SkipIUStorage = 0;
                    else if (listIUStorage.Count < 7 && SkipIUStorage < 1)
                        SkipIUStorage++;
                    else if (listIUStorage.Count < 10 && SkipIUStorage < 2)
                        SkipIUStorage++;
                }
                paginationIUStorage(listIUStorage, SkipIUStorage);
                #endregion

                firstLoad = false;

            }
            catch
            {}
        }

        List<keyValueBool> loadingList;
         List<string> catchError = new List<string>();
        int catchErrorCount = 0;
        async void loading_getDailySalPur()
        {
            try
            {
                await CountMonthlySalPur();
                await DailySalPur();
            }
            catch
            {
                loading_getDailySalPur();
                catchError.Add("loading_getDailySalPur");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDailySalPur"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task refreshViewTask()
        {
            try
            {
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                //loadingList.Add(new keyValueBool { key = "AllSalPur", value = false });
                //loadingList.Add(new keyValueBool { key = "AgentCount", value = false });
                loadingList.Add(new keyValueBool { key = "getCountByInvType", value = false });
                loadingList.Add(new keyValueBool { key = "getBranchBalance", value = false });
                loadingList.Add(new keyValueBool { key = "getTableCount", value = false });
                loadingList.Add(new keyValueBool { key = "getBestOf", value = false });
                loadingList.Add(new keyValueBool { key = "UserOnline", value = false });
                loadingList.Add(new keyValueBool { key = "BranchOnline", value = false });
                loadingList.Add(new keyValueBool { key = "IUStorageRefresh", value = false });
                loadingList.Add(new keyValueBool { key = "BestSellerRefresh", value = false });
                loadingList.Add(new keyValueBool { key = "UserOnlinePic", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getDailySalPur", value = false });
                loadingList.Add(new keyValueBool { key = "AmountMonthlySalPur", value = false });

                //AllSalPur();
                //AgentCount();
                getCountByInvType();
                getBranchBalance();
                getTableCount();
                getBestOf();

                UserOnline();
                BranchOnline();
                IUStorageRefresh();
                BestSellerRefresh();
                UserOnlinePic();
                loading_getDailySalPur();
                AmountMonthlySalPur();
                do
                {
                    isDone = true;
                    foreach (var item in loadingList)
                    {
                        if (item.value == false)
                        {
                            isDone = false;
                            break;
                        }
                    }
                    if (!isDone)
                    {
                        //string s = "";
                        //foreach (var item in loadingList)
                        //{
                        //    s += item.name + " - " + item.value + "\n";
                        //}
                        //MessageBox.Show(s);
                        await Task.Delay(0500);
                        //MessageBox.Show("do");
                    }
                }
                while (!isDone);
                //MessageBox.Show(catchError + " and count: " + catchErrorCount);
                #endregion

                this.DataContext = new Dash();
                this.DataContext = dash;
            }
            catch
            {}
        }

        async void IUStorageRefresh()
        {
            try
            {
                #region IUStorage
                await IUStorage();
                paginationIUStorage(listIUStorage, SkipIUStorage);
                #endregion
            }
            catch (Exception)
            {
                IUStorageRefresh();
                catchError.Add("IUStorageRefresh");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("IUStorageRefresh"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void BestSellerRefresh()
        {
            try
            {
                #region BestSeller
                await BestSeller();
                paginationBestSeller(listBestSeller, SkipBestSeller);
                #endregion
            }
            catch (Exception)
            {
                BestSellerRefresh();
                catchError.Add("BestSellerRefresh");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("BestSellerRefresh"))
                {
                    item.value = true;
                    break;
                }
            }
        }
         
        void refreshView()
        {
            try
            {
                //AllSalPur();
                CountMonthlySalPur();
                DailySalPur();
                //AgentCount();
                getCountByInvType();
                getBranchBalance();
                getTableCount();
                getBestOf();
                UserOnline();
                BranchOnline();
                #region BestSeller
                BestSeller();
                paginationBestSeller(listBestSeller, SkipBestSeller);
                #endregion
                #region IUStorage
                IUStorage();
                paginationIUStorage(listIUStorage, SkipIUStorage);
                #endregion
                UserOnlinePic();
                AmountMonthlySalPur();

            this.DataContext = new Dash();
            this.DataContext = dash;
            }
            catch
            { }
        }
        //async Task AllSalPur()
        //{
        //    try
        //    {
        //        List<InvoiceCount> listSalPur = await dash.Getdashsalpur();
        //        if (cb_branch.SelectedValue != null)
        //            if ((int)cb_branch.SelectedValue == 0)
        //            {
        //                dash.countAllPurchase = listSalPur.Sum(x => x.purCount).ToString();
        //                dash.countAllSalesValue = listSalPur.Sum(x => x.saleCount).ToString();
        //            }
        //            else
        //            {
        //                var newSalPur = listSalPur.Where(s => s.branchCreatorId == (int)cb_branch.SelectedValue).FirstOrDefault();
        //                if (newSalPur != null)
        //                {
        //                    dash.countAllPurchase = newSalPur.purCount.ToString();
        //                    dash.countAllSalesValue = newSalPur.saleCount.ToString();
        //                }
        //                else
        //                    dash.countAllPurchase = dash.countAllSalesValue = "0";
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        //SectionData.ExceptionMessage(ex, this);
        //    }
        //    foreach (var item in loadingList)
        //    {
        //        if (item.key.Equals("AllSalPur"))
        //        {
        //            item.value = true;
        //            break;
        //        }
        //    }
        //}
        async Task CountMonthlySalPur()
        {
            try
            {
                List<TotalPurSale> listSalPur = await dash.GetTotalPurSale(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        dash.countMonthlyPurchase = listSalPur.Sum(x => x.countPur).ToString();
                        dash.countMonthlySales = listSalPur.Sum(x => x.countSale).ToString();
                    }
                    else
                    {
                        var newSalPur = listSalPur.Where(s => s.branchCreatorId == (int)cb_branch.SelectedValue);
                        if (newSalPur != null)
                        {
                            dash.countMonthlyPurchase = newSalPur.Sum(x=> x.countPur).ToString();
                            dash.countMonthlySales = newSalPur.Sum(x => x.countSale).ToString();
                        }
                        else
                            dash.countMonthlyPurchase = dash.countMonthlySales = "0";
                    }
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }
        async Task DailySalPur()
        {
            try
            {
                List<InvoiceCount> listSalPur = await dash.GetdashsalpurDay(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        dash.countDailyPurchase = listSalPur.Sum(x => x.purCount).ToString();
                        dash.countDailySales = listSalPur.Sum(x => x.saleCount).ToString();
                        //dash.countDailySales = listSalPur.Sum(x => x.saleCount).ToString();
                        //dash.countDailySales = listSalPur.Sum(x => x.saleCount).ToString();
                        //dash.countDailySales = listSalPur.Sum(x => x.saleCount).ToString();
                    }
                    else
                    {
                        var newSalPur = listSalPur.Where(s => s.branchCreatorId == (int)cb_branch.SelectedValue).FirstOrDefault();
                        if (newSalPur != null)
                        {
                            dash.countDailyPurchase = newSalPur.purCount.ToString();
                            dash.countDailySales = newSalPur.saleCount.ToString();
                            //dash.clothesCount = newSalPur.clothesCount.ToString();
                            //dash.carpetsCount = newSalPur.carpetsCount.ToString();
                            //dash.carsCount = newSalPur.carsCount.ToString();
                        }
                        else
                            dash.countDailyPurchase = dash.countDailySales = "0";
                        //  dash.clothesCount = dash.carpetsCount = dash.carsCount 
                    }
                InitializePieChart(pch_dailyPurchaseInvoice,
                    int.Parse(dash.countDailyPurchase),
                    int.Parse(dash.countMonthlyPurchase));

                InitializePieChart(pch_dailySalesInvoice,
                    int.Parse(dash.countDailySales),
                    int.Parse(dash.countMonthlySales));

             
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }
        //async Task AgentCount()
        //{
        //    try
        //    {
        //        List<AgentsCount> listAgentCount = await dash.GetAgentCount();
        //        var newAgentCount = listAgentCount.FirstOrDefault();
        //        if (newAgentCount != null)
        //        {
        //            dash.customerCount = newAgentCount.customerCount.ToString();
        //            dash.vendorCount = newAgentCount.vendorCount.ToString();
        //        }
        //        else
        //        {
        //            dash.customerCount = dash.vendorCount = "0";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //SectionData.ExceptionMessage(ex, this);
        //    }
        //    foreach (var item in loadingList)
        //    {
        //        if (item.key.Equals("AgentCount"))
        //        {
        //            item.value = true;
        //            break;
        //        }
        //    }
        //}

        async Task getBestOf()
        {

            try
            {
                //List<BranchInvoiceCount> CountinHoursList  = new List<BranchInvoiceCount>();
                //List<BranchInvoiceCount> CountinMonthsList = new List<BranchInvoiceCount>();
                //List<BranchInvoiceCount> CountinDaysList   = new List<BranchInvoiceCount>();
                dash.listBestOfCount = await dash.GetBestOf(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if(dash.listBestOfCount != null)
                changeTypeBestOf(dash.listBestOfCount);
            }
            catch
            {
                getBestOf();
                catchError.Add("getBestOf");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("getBestOf"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        
        void changeTypeBestOf(List<BestOfCount> listBestOfCount)
        {
            List<BranchInvoiceCount> listBranchInvoiceCount = new List<BranchInvoiceCount>();

            if (cb_branch.SelectedValue != null)
            {
                var newBestOfCount = listBestOfCount.Where(s => s.branchId == (int)cb_branch.SelectedValue).FirstOrDefault();
                if (newBestOfCount != null)
                {

                    if (rdb_times.IsChecked.Value)
                    {
                        listBranchInvoiceCount = newBestOfCount.CountinHoursList;
                    }
                    else if (rdb_days.IsChecked.Value)
                    {
                        listBranchInvoiceCount = newBestOfCount.CountinDaysList;
                    }
                    else if (rdb_months.IsChecked.Value)
                    {
                        listBranchInvoiceCount = newBestOfCount.CountinMonthsList;
                    }
                }
            }
            List<keyValueInt> listInvoiceType = new List<keyValueInt>();


            int count = 0;
            foreach (var item in listBranchInvoiceCount)
            {
                count++;
                if (listBranchInvoiceCount.Count != count)
                {
                    //listInvoiceType.Add(new keyValueInt { key = item.duration, value = item.count });
                    listInvoiceType.Add(new keyValueInt { key = (count + "# " + HelpClass.translate_days(item.duration)), value = item.count });
                }
                else
                {
                    //listInvoiceType.Add(new keyValueInt { key = item.duration, value = item.count });
                    listInvoiceType.Add(new keyValueInt { key = (AppSettings.resourcemanager.GetString("trOthers")), value = item.count });
                }
            }
            InitializePieChartList(pch_bestOf, listInvoiceType);
        }
        private void Rdb_bestOf_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as RadioButton).IsFocused && dash.listBestOfCount != null)
                    changeTypeBestOf(dash.listBestOfCount);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task getCountByInvType()
        {
            
             try
            {
                List<CountByInvType> listCountByInvType = await dash.GetCountByInvType(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        dash.dhallCount = listCountByInvType.Sum(x => x.dhallCount).ToString();
                        dash.selfCount = listCountByInvType.Sum(x => x.selfCount).ToString();
                        dash.tawayCount = listCountByInvType.Sum(x => x.tawayCount).ToString();
                    }
                    else
                    {
                        var newCountByInvType = listCountByInvType.Where(s => s.branchId == (int)cb_branch.SelectedValue).FirstOrDefault();
                        if (newCountByInvType != null)
                        {
                            dash.dhallCount = newCountByInvType.dhallCount.ToString();
                            dash.selfCount = newCountByInvType.selfCount.ToString();
                            dash.tawayCount = newCountByInvType.tawayCount.ToString();
                        }
                        else
                             dash.dhallCount =
                                dash.selfCount = dash.tawayCount = "0";
                    }
                List<keyValueInt> listInvoiceType = new List<keyValueInt>
                    {
                        new keyValueInt {key = txt_clothes.Text, value = int.Parse(dash.dhallCount) },
                        new keyValueInt {key = txt_carpets.Text, value = int.Parse(dash.tawayCount) },
                        new keyValueInt {key = txt_cars.Text, value = int.Parse(dash.selfCount) },
                    };
                InitializePieChartList(pch_invoicesTypeInDay, listInvoiceType);
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
                getCountByInvType();
                catchError.Add("getCountByInvType");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("getCountByInvType"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task getBranchBalance()
        {

            try
            {
                List<BranchBalance> listBranchBalance = await dash.GetCashBalance(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        dash.balance = listBranchBalance.Sum(x => x.balance).ToString();
                    }
                    else
                    {
                        var newBranchBalance = listBranchBalance.Where(s => s.branchId == (int)cb_branch.SelectedValue).FirstOrDefault();
                        if (newBranchBalance != null)
                        {
                            dash.balance = newBranchBalance.balance.ToString();
                        }
                        else
                            dash.balance = "0";
                    }

            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);

                getBranchBalance();
                catchError.Add("getBranchBalance");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("getBranchBalance"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task getTableCount()
        {

            try
            {
                //List<TablesStatistics> listTablesStatistics = await tablesModel.GetTablesStatistics(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                List<TablesStatistics> listTablesStatistics = await tablesModel.GetTablesStatistics(DateTime.Now.ToString(), MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        dash.emptyCount = listTablesStatistics.Sum(x => x.emptyCount).ToString();
                        dash.openedCount = listTablesStatistics.Sum(x => x.openedCount).ToString();
                        dash.reservedCount = listTablesStatistics.Sum(x => x.reservedCount).ToString();
                    }
                    else
                    {
                        var newTablesStatistics = listTablesStatistics.Where(s => s.branchId == (int)cb_branch.SelectedValue).FirstOrDefault();
                        if (newTablesStatistics != null)
                        {
                            dash.emptyCount = newTablesStatistics.emptyCount.ToString();
                            dash.openedCount = newTablesStatistics.openedCount.ToString();
                            dash.reservedCount = newTablesStatistics.reservedCount.ToString();
                        }
                        else
                            dash.emptyCount = dash.openedCount =  dash.reservedCount = "0";
                    }
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
                getTableCount();
                catchError.Add("getTableCount");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("getTableCount"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task UserOnline()
        {
            try
            {
                List<UserOnlineCount> listUserOnline = await dash.Getuseronline(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        dash.userOnline = listUserOnline.Sum(x => x.userOnlineCount).ToString();
                        dash.userOffline = listUserOnline.Sum(x => x.offlineUsers).ToString();
                    }
                    else
                    {
                        var newUserOnline = listUserOnline.Where(s => s.branchId == (int)cb_branch.SelectedValue).FirstOrDefault();
                        if (newUserOnline != null)
                        {
                            dash.userOnline = newUserOnline.userOnlineCount.ToString();
                            dash.userOffline = newUserOnline.offlineUsers.ToString();
                        }
                        else
                            dash.userOnline = dash.userOffline = "0";
                    }
                InitializePieChart(pch_userOnline,
                int.Parse(dash.userOnline),
                (int.Parse(dash.userOnline) + int.Parse(dash.userOffline)));
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
                UserOnline();
                catchError.Add("UserOnline");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("UserOnline"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task UserOnlinePic()
        {
            try
            {
                dash.listUserOnline = await dash.GetuseronlineInfo(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        InitializeUserOnlinePic(dash.listUserOnline);
                    }
                    else
                    {
                        List<userOnlineInfo> newUserOnline = dash.listUserOnline.Where(s => s.branchId == (int)cb_branch.SelectedValue).ToList();
                        if (newUserOnline != null && newUserOnline.Count != 0)
                        {
                            InitializeUserOnlinePic(newUserOnline);
                        }
                        else
                            InitializeNoUserOnlinePic();
                    }
                InitializePieChart(pch_userOnline,
                int.Parse(dash.userOnline),
                (int.Parse(dash.userOnline) + int.Parse(dash.userOffline)));
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
                UserOnlinePic();
                catchError.Add("UserOnlinePic");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("UserOnlinePic"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        void InitializeUserOnlinePic(List<userOnlineInfo> users)
        {
            #region userImageLoad
            grid_userImages.Children.Clear();
            int userCount = 0;
            foreach (var item in users)
            {
                if (userCount > 4)
                //if (true)
                {
                    #region Button
                    Button button = new Button();
                    button.Padding = new Thickness(0, 0, 0, 0);
                    button.Margin = new Thickness(-5, 0, -5, 0);
                    button.Background = null;
                    button.BorderBrush = null;
                    button.Height = 40;
                    button.Width = 40;
                    button.Click += userOnlineListWindow;
                    Grid.SetColumn(button, 4);
                    #region grid
                    Grid grid = new Grid();
                    //grid.Margin = new Thickness(-5, 0, -5, 0);
                    //Grid.SetColumn(grid, 4);
                    #region rectangle
                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = Application.Current.Resources["Orange"] as SolidColorBrush;
                    rectangle.RadiusX = 90;
                    rectangle.RadiusY = 90;
                    rectangle.Height = 40;
                    rectangle.Width = 40;
                    rectangle.StrokeThickness = 1;
                    rectangle.Stroke = Application.Current.Resources["White"] as SolidColorBrush; ;
                    grid.Children.Add(rectangle);
                    #endregion
                    #region Text
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "+" + (users.Count() - 4).ToString();
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.FontWeight = FontWeights.Bold;
                    textBlock.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    grid.Children.Add(textBlock);
                    #endregion
                    #endregion
                    button.Content = grid;
                    #endregion


                    grid_userImages.Children.Add(button);
                    break;
                }
                else
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Margin = new Thickness(-5, 0, -5, 0);
                    ellipse.StrokeThickness = 1;
                    ellipse.Stroke = Application.Current.Resources["White"] as SolidColorBrush;
                    ellipse.Height = 40;
                    ellipse.Width = 40;
                    ellipse.FlowDirection = FlowDirection.LeftToRight;
                    ellipse.ToolTip = item.userName;
                    userImageLoad(ellipse, item.image);
                    Grid.SetColumn(ellipse, userCount);
                    grid_userImages.Children.Add(ellipse);
                    userCount++;
                }
            }
            #endregion
        }
        void InitializeNoUserOnlinePic()
        {
            grid_userImages.Children.Clear();
            Grid grid = new Grid();
            grid.Margin = new Thickness(-5, 0, -5, 0);
            Grid.SetColumn(grid, 4);
            #region rectangle
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = Application.Current.Resources["Orange"] as SolidColorBrush;
            rectangle.RadiusX = 90;
            rectangle.RadiusY = 90;
            rectangle.Height = 40;
            rectangle.Width = 40;
            rectangle.StrokeThickness = 1;
            rectangle.Stroke = Application.Current.Resources["White"] as SolidColorBrush; ;
            grid.Children.Add(rectangle);
            #endregion
            #region rectangle
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "0";
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
            grid.Children.Add(textBlock);
            #endregion
            grid_userImages.Children.Add(grid);
        }
        async Task BranchOnline()
        {
            try
            {
                List<BranchOnlineCount> listBranchOnline = await dash.GetBrachonline(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                var newBranchOnline = listBranchOnline.FirstOrDefault();
                if (newBranchOnline != null)
                {
                    dash.branchOnline = newBranchOnline.branchOnline.ToString();
                    dash.branchOffline = newBranchOnline.branchOffline.ToString();
                }
                else
                    dash.branchOnline = dash.branchOffline = "0";
                InitializePieChart(pch_branch,
                    int.Parse(dash.branchOnline),
                    (int.Parse(dash.branchOnline) + int.Parse(dash.branchOffline)));
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
                BranchOnline();
                catchError.Add("BranchOnline");
                catchErrorCount++;

            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("BranchOnline"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        void userOnlineListWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                wd_usersOnline w = new wd_usersOnline();
                w.usersOnline = dash.listUserOnline;
                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }

        #region BestSeller
        async Task BestSeller()
        {
            try
            {
                List<BestSeller> listAllBestSeller = await dash.Getbestseller(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        listBestSeller = listAllBestSeller.GroupBy(x => new { x.itemName, x.unitName }).Select(s => new BestSeller
                        {
                            itemName = s.FirstOrDefault().itemName,
                            unitName = s.FirstOrDefault().unitName,
                            quantity = s.Sum(g => g.quantity),
                        }).ToList();
                    }
                    else
                    {
                        listBestSeller = listAllBestSeller.Where(s => s.branchCreatorId == (int)cb_branch.SelectedValue).ToList();
                        if (listBestSeller != null)
                        {
                            listBestSeller = listBestSeller.GroupBy(x => new { x.itemName, x.unitName }).Select(s => new BestSeller
                            {
                                itemName = s.FirstOrDefault().itemName,
                                unitName = s.FirstOrDefault().unitName,
                                quantity = s.Sum(g => g.quantity),
                            }).ToList();
                        }
                        else
                            listBestSeller = new List<BestSeller>();
                    }
            }
            catch (Exception ex)
            {
                //    SectionData.ExceptionMessage(ex, this);
            }
        }
        void paginationBestSeller(List<BestSeller> listBestSeller, int skip)
        {
            grid_bestSeller.Children.Clear();
            int order = (skip * 3) + 1;
            int row = 1;
            listBestSeller = listBestSeller.Skip(skip * 3).Take(3).ToList();
            foreach (var item in listBestSeller)
            {
                //InitializeBestSellerRow(order.ToString(), item.itemName + "-" + item.unitName,
                //  item.quantity.ToString(), row);
                InitializeBestSellerRow(order.ToString(), item.itemName , item.quantity.ToString(), row);
                order++;
                row += 2;
            }

            #region Border
            #region   firstBorder 
            var firstBorder = new Border();
            firstBorder.Margin = new Thickness(10, 2.5, 10, 2.5);
            firstBorder.Height = 1;
            firstBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            firstBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#C2F3E8"));
            Grid.SetRow(firstBorder, 2);
            Grid.SetColumnSpan(firstBorder, 7);
            #endregion
            #region   firstBorder 
            var secondBorder = new Border();
            secondBorder.Margin = new Thickness(10, 2.5, 10, 2.5);
            secondBorder.Height = 1;
            secondBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            secondBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#C2F3E8"));
            Grid.SetRow(secondBorder, 4);
            Grid.SetColumnSpan(secondBorder, 7);
            #endregion
            grid_bestSeller.Children.Add(firstBorder);
            grid_bestSeller.Children.Add(secondBorder);
            #endregion


        }
        void InitializeBestSellerRow(string No, string item, string count, int row)
        {
            #region   itemNo
            var itemNo = new TextBlock();
            itemNo.Text = "#" + No;
            itemNo.Tag = "itemTitle";
            itemNo.VerticalAlignment = VerticalAlignment.Center;
            itemNo.HorizontalAlignment = HorizontalAlignment.Center;
            itemNo.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1D75B8"));
            itemNo.FontSize = 16;
            itemNo.Margin = new Thickness(5, 1, 2.5, 1);
            Grid.SetRow(itemNo, row);
            Grid.SetColumn(itemNo, 0);
            #endregion
            #region   itemTitle
            var itemTitle = new TextBlock();
            itemTitle.Text = AppSettings.resourcemanager.GetString("trItem") + ":";
            itemTitle.Tag = "itemTitle";
            itemTitle.VerticalAlignment = VerticalAlignment.Center;
            itemTitle.HorizontalAlignment = HorizontalAlignment.Center;
            itemTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#707070"));
            itemTitle.FontSize = 16;
            itemTitle.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(itemTitle, row);
            Grid.SetColumn(itemTitle, 1);
            #endregion
            #region   itemName
            var itemName = new TextBlock();
            itemName.Text = item;
            itemName.Tag = "itemName";
            itemName.VerticalAlignment = VerticalAlignment.Center;
            itemName.HorizontalAlignment = HorizontalAlignment.Left;
            itemName.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#707070"));
            itemName.FontSize = 16;
            itemName.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(itemName, row);
            Grid.SetColumn(itemName, 2);
            #endregion
            #region   countTitle
            var countTitle = new TextBlock();
            countTitle.Text = AppSettings.resourcemanager.GetString("trCount") + ":";
            countTitle.Tag = "countTitle";
            countTitle.VerticalAlignment = VerticalAlignment.Center;
            countTitle.HorizontalAlignment = HorizontalAlignment.Center;
            countTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#707070"));
            countTitle.FontSize = 16;
            countTitle.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(countTitle, row);
            Grid.SetColumn(countTitle, 3);
            #endregion
            #region   countItem
            var countItem = new TextBlock();
            countItem.Text = count;
            countItem.Tag = "itemName";
            countItem.VerticalAlignment = VerticalAlignment.Center;
            countItem.HorizontalAlignment = HorizontalAlignment.Left;
            countItem.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1D75B8"));
            countItem.FontSize = 16;
            countItem.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(countItem, row);
            Grid.SetColumn(countItem, 4);
            #endregion
            grid_bestSeller.Children.Add(itemNo);
            grid_bestSeller.Children.Add(itemTitle);
            grid_bestSeller.Children.Add(itemName);
            grid_bestSeller.Children.Add(countTitle);
            grid_bestSeller.Children.Add(countItem);
        }
        #endregion
        #region IUStorage
        private async void Btn_storageSetting_Click(object sender, RoutedEventArgs e)
        {//settings
            try
            {
                // 
                //    SectionData.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;

                wd_itemsUnitList w = new wd_itemsUnitList();
                w.itemId = 0;
                w.itemUnitId = 0;
                w.CallerName = "IUList";
                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                if (w.isActive)
                {
                    ItemUnitUser itemUnitUserModel = new ItemUnitUser();
                    ItemUnit itemUnitModel = new ItemUnit();

                    foreach (var x in w.selectedItemUnitsHome)
                    {
                        x.id = 0;
                        ItemUnit iu = new ItemUnit();
                        iu = await itemUnitModel.GetById(x.itemUnitId.Value);
                        x.itemId = iu.itemId;
                        x.unitId = iu.unitId;
                    }
                    int result = await itemUnitUserModel.UpdateList(w.selectedItemUnitsHome, MainWindow.userLogin.userId);

                    if (result > 0)
                    {
                        FillCombo.itemUnitsUsersList = w.selectedItemUnitsHome;

                        refrishIUList(FillCombo.itemUnitsUsersList);
                        await IUStorage();
                    }
                    //SkipIUStorage = 0;
                    //refreshView();
                }
                Window.GetWindow(this).Opacity = 1;
                // 
                //    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                // 
                //    SectionData.EndAwait(grid_main);
                //SectionData.ExceptionMessage(ex, this);
            }

        }
        void refrishIUList(List<ItemUnitUser> itemUnitsUsers)
        {
            try
            {
                IUList.Clear();
                foreach (var item in itemUnitsUsers)
                {
                    var itemUnit = new ItemUnit();
                    itemUnit.itemUnitId = item.itemUnitId.Value;
                    itemUnit.itemId = item.itemId;
                    itemUnit.unitId = item.unitId;
                    IUList.Add(itemUnit);
                }
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }
        async Task IUStorage()
        {
            try
            {
                List<IUStorage> listAllIUStorage = await dash.GetIUStorage(IUList, MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        listIUStorage = listAllIUStorage.GroupBy(x => new { x.itemName, x.unitName }).Select(s => new IUStorage
                        {
                            itemName = s.FirstOrDefault().itemName,
                            unitName = s.FirstOrDefault().unitName,
                            quantity = s.Sum(g => g.quantity),
                        }).ToList();
                    }
                    else
                    {
                        listIUStorage = listAllIUStorage.Where(s => s.branchId == (int)cb_branch.SelectedValue).ToList();
                        if (listIUStorage != null)
                        {
                            listIUStorage = listIUStorage.GroupBy(x => new { x.itemName, x.unitName }).Select(s => new IUStorage
                            {
                                itemName = s.FirstOrDefault().itemName,
                                unitName = s.FirstOrDefault().unitName,
                                quantity = s.Sum(g => g.quantity),
                            }).ToList();
                        }
                        else
                            listIUStorage = new List<IUStorage>();

                        if (listIUStorage.Count >9)
                        {
                            listIUStorage = listIUStorage.Take(9).ToList();
                        }
                    }
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }
        void paginationIUStorage(List<IUStorage> listIUStorage, int skip = 0)
        {
            grid_IUStorage.Children.Clear();
            int order = (skip * 3) + 1;
            int row = 1;
            listIUStorage = listIUStorage.Skip(skip * 3).Take(3).ToList();
            foreach (var item in listIUStorage)
            {
                InitializeIUStorageRow(order.ToString(), item.itemName + "-" + item.unitName,
                  item.quantity.ToString(), row);
                order++;
                row += 2;
            }

            #region Border
            #region   firstBorder 
            var firstBorder = new Border();
            firstBorder.Margin = new Thickness(10, 2.5, 10, 2.5);
            firstBorder.Height = 1;
            firstBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            firstBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#BC80EF"));
            Grid.SetRow(firstBorder, 2);
            Grid.SetColumnSpan(firstBorder, 7);
            #endregion
            #region   firstBorder 
            var secondBorder = new Border();
            secondBorder.Margin = new Thickness(10, 2.5, 10, 2.5);
            secondBorder.Height = 1;
            secondBorder.BorderThickness = new Thickness(0, 0, 0, 0);
            secondBorder.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#BC80EF"));
            Grid.SetRow(secondBorder, 4);
            Grid.SetColumnSpan(secondBorder, 7);
            #endregion
            grid_IUStorage.Children.Add(firstBorder);
            grid_IUStorage.Children.Add(secondBorder);
            #endregion


        }
        void InitializeIUStorageRow(string No, string item, string count, int row)
        {
            #region   itemNo
            var itemNo = new TextBlock();
            itemNo.Text = "#" + No;
            itemNo.Tag = "itemTitle";
            itemNo.VerticalAlignment = VerticalAlignment.Center;
            itemNo.HorizontalAlignment = HorizontalAlignment.Center;
            itemNo.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1D75B8"));
            itemNo.FontSize = 16;
            itemNo.Margin = new Thickness(5, 1, 2.5, 1);
            Grid.SetRow(itemNo, row);
            Grid.SetColumn(itemNo, 0);
            #endregion
            #region   itemTitle
            var itemTitle = new TextBlock();
            itemTitle.Text = AppSettings.resourcemanager.GetString("trItem") +":";
            itemTitle.Tag = "itemTitle";
            itemTitle.VerticalAlignment = VerticalAlignment.Center;
            itemTitle.HorizontalAlignment = HorizontalAlignment.Center;
            itemTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#707070"));
            itemTitle.FontSize = 16;
            itemTitle.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(itemTitle, row);
            Grid.SetColumn(itemTitle, 1);
            #endregion
            #region   itemName
            var itemName = new TextBlock();
            itemName.Text = item;
            itemName.Tag = "itemName";
            itemName.VerticalAlignment = VerticalAlignment.Center;
            itemName.HorizontalAlignment = HorizontalAlignment.Left;
            itemName.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#707070"));
            itemName.FontSize = 16;
            itemName.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(itemName, row);
            Grid.SetColumn(itemName, 2);
            #endregion
            #region   countTitle
            var countTitle = new TextBlock();
            countTitle.Text = AppSettings.resourcemanager.GetString("trCount") + ":";  
            countTitle.Tag = "countTitle";
            countTitle.VerticalAlignment = VerticalAlignment.Center;
            countTitle.HorizontalAlignment = HorizontalAlignment.Center;
            countTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#707070"));
            countTitle.FontSize = 16;
            countTitle.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(countTitle, row);
            Grid.SetColumn(countTitle, 3);
            #endregion
            #region   countItem
            var countItem = new TextBlock();
            countItem.Text = count;
            countItem.Tag = "itemName";
            countItem.VerticalAlignment = VerticalAlignment.Center;
            countItem.HorizontalAlignment = HorizontalAlignment.Left;
            countItem.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#1D75B8"));
            countItem.FontSize = 16;
            countItem.Margin = new Thickness(5, 1, 5, 1);
            Grid.SetRow(countItem, row);
            Grid.SetColumn(countItem, 4);
            #endregion
            grid_IUStorage.Children.Add(itemNo);
            grid_IUStorage.Children.Add(itemTitle);
            grid_IUStorage.Children.Add(itemName);
            grid_IUStorage.Children.Add(countTitle);
            grid_IUStorage.Children.Add(countItem);
        }
        #endregion
        async Task AmountMonthlySalPur()
        {
            try
            {
                double[] ArrayS = new double[NumberDaysInMonth];
                double[] ArrayP = new double[NumberDaysInMonth];
                string[] ArrayCount = new string[NumberDaysInMonth];
                List<TotalPurSale> listAllBestSeller = await dash.GetTotalPurSale(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
                if (cb_branch.SelectedValue != null)
                    if ((int)cb_branch.SelectedValue == 0)
                    {
                        listMonthlyInvoice = listAllBestSeller.GroupBy(x => new { x.day }).Select(s => new TotalPurSale
                        {
                            branchCreatorId = s.FirstOrDefault().branchCreatorId,
                            branchCreatorName = s.FirstOrDefault().branchCreatorName,
                            day = s.FirstOrDefault().day,
                            totalPur = s.Sum(g => g.totalPur),
                            totalSale = s.Sum(g => g.totalSale),
                            countPur = s.Sum(g => g.countPur),
                            countSale = s.Sum(g => g.countSale)
                        }).ToList();


                    }
                    else
                    {
                        listMonthlyInvoice = listAllBestSeller.Where(s => s.branchCreatorId == (int)cb_branch.SelectedValue).ToList();
                        if (listMonthlyInvoice != null)
                        {
                            listMonthlyInvoice = listMonthlyInvoice.GroupBy(x => new { x.day }).Select(s => new TotalPurSale
                            {
                                branchCreatorId = s.FirstOrDefault().branchCreatorId,
                                branchCreatorName = s.FirstOrDefault().branchCreatorName,
                                day = s.FirstOrDefault().day,
                                totalPur = s.Sum(g => g.totalPur),
                                totalSale = s.Sum(g => g.totalSale),
                                countPur = s.Sum(g => g.countPur),
                                countSale = s.Sum(g => g.countSale)
                            }).ToList();
                        }
                        else
                            listMonthlyInvoice = new List<TotalPurSale>();


                    }

                int count = 0;
                foreach (var item in listMonthlyInvoice)
                {
                    ArrayS[count] = double.Parse(item.totalSale.ToString()); 
                    ArrayP[count] = double.Parse(item.totalPur.ToString());
                    ArrayCount[count] = (count+1).ToString();
                    count++;
                }
                dash.SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = AppSettings.resourcemanager.GetString("trSales") ,
                    Values = ArrayS.AsChartValues()
                },
                 new LineSeries
                {
                    Title = AppSettings.resourcemanager.GetString("trPurchases"),
                    Values = ArrayP.AsChartValues()
                }
            };

                axs_AxisY.Title = AppSettings.resourcemanager.GetString("trTotal");
                axs_AxisX.Title = AppSettings.resourcemanager.GetString("trDays") ;
                dash.Labels = ArrayCount;
                dash.YFormatter = value => value.ToString("C");
                //DataContext = this;
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
                AmountMonthlySalPur();
                catchError.Add("AmountMonthlySalPur");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("AmountMonthlySalPur"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        void InitializePieChart(PieChart pieChart, int partial = 0, int all = 0)
        {
            SeriesCollection series = new SeriesCollection();
            series.Add(
                 new PieSeries
                 {
                     Values = new ChartValues<int> { partial },
                     Title = "",
                     DataLabels = false,
                     Fill = Application.Current.Resources["mediumGreen"] as SolidColorBrush
                 }
             );
            series.Add(
                 new PieSeries
                 {
                     Values = new ChartValues<int> { all - partial },
                     Title = "",
                     DataLabels = false,
                     Fill = Application.Current.Resources["MainColorlightGrey"] as SolidColorBrush
                 }
             );
            pieChart.Series = series;
        }
        void InitializePieChartList(PieChart pieChart, List<keyValueInt> valueList)
        {
            SeriesCollection series = new SeriesCollection();
            foreach (var item in valueList)
            {
                series.Add(
                    new PieSeries
                    {
                        Values = new ChartValues<int> { item.value },
                        Title = item.key,
                        DataLabels = false,
                        
                        Fill = Application.Current.Resources["mediumGreen"] as SolidColorBrush
                    }
                );
            }
            pieChart.Series = series;
        }
        async void userImageLoad(Ellipse ellipse, string image)
        {
            try
            {
                if (!string.IsNullOrEmpty(image))
                {
                    clearImg(ellipse);

                    byte[] imageBuffer = await user.downloadImage(image); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    ellipse.Fill = new ImageBrush(bitmapImage);
                }
                else
                {
                    clearImg(ellipse);
                }
            }
            catch
            {
                clearImg(ellipse);
            }
        }
        private void clearImg(Ellipse ellipse)
        {
            Uri resourceUri = new Uri("pic/no-image-icon-90x90.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            brush.ImageSource = temp;
            ellipse.Fill = brush;
        }
        public void timerAnimation()
        {
            x = 1000;
            y = 2000;
            z = 3000;
            //value_items = Convert.ToInt32(tb_items.Text);
            //value_purchase = Convert.ToInt32(tb_purchase.Text);
            //value_sales = Convert.ToInt32(tb_sales.Text);
            //value_sales = Convert.ToInt32(txt_cashSympol.Text);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {

                //x += 86;
                //y += 74;
                //z += 65;

                //tb_items.Text = x.ToString();
                //tb_purchase.Text = x.ToString();
                //tb_sales.Text = x.ToString();
                //txt_cashSympol.Text = x.ToString();
            //if (x >= 5000)
            //{
                    //tb_items.Text = Convert.ToString(value_items);
                    //tb_purchase.Text = Convert.ToString(value_purchase);
                    //tb_sales.Text = Convert.ToString(value_sales);
                    //txt_cashSympol.Text = Convert.ToString(value_sales);
                    timer.Stop();
            //}
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }
        private void UserControl_TouchLeave(object sender, TouchEventArgs e)
        {
            MessageBox.Show("hey i'm here in TouchLeave");
        }
        private async void Cb_branch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                 
                    HelpClass.StartAwait(grid_main);

                await refreshViewTask();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                //SectionData.ExceptionMessage(ex, this);
            }
        }

       
    }
}
