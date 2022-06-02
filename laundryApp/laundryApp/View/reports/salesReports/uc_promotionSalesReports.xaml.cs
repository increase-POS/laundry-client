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
using System.Globalization;
using System.IO;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.Threading;
using System.Resources;
using System.Reflection;

namespace laundryApp.View.reports.salesReports
{
    /// <summary>
    /// Interaction logic for uc_promotionSalesReports.xaml
    /// </summary>
    public partial class uc_promotionSalesReports : UserControl
    {
        private static uc_promotionSalesReports _instance;
        public static uc_promotionSalesReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_promotionSalesReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_promotionSalesReports()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }
      
        //prin & pdf
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private int selectedTab = 0;
        string searchText = "";
        Statistics statisticModel = new Statistics();

        List<ItemTransferInvoice> coupons;
        List<ItemTransferInvoice> offers;
        List<SalesMembership> invoicesClasses;

        List<ItemTransferInvoice> couponsQuery;
        List<ItemTransferInvoice> offersQuery;
        List<SalesMembership> invoicesClassesQuery;

        //for combo boxes
        /*************************/
        Coupon selectedCoupon;
        Offer selectedOffer;
        InvoicesClass selectedInvoice;

        List<Coupon> comboCouponTemp = new List<Coupon>();
        List<Offer> comboOfferTemp = new List<Offer>();
        List<InvoicesClass> comboInvoiceTemp = new List<InvoicesClass>();

        List<Coupon> dynamicComboCoupon = new List<Coupon>();
        List<Offer> dynamicComboOffer = new List<Offer>();
        List<InvoicesClass> dynamicComboInvoice = new List<InvoicesClass>();

        /*************************/

        List<int> selectedcouponId = new List<int>();
        List<int> selectedOfferId = new List<int>();
        List<int> selectedInvoiceId = new List<int>();

        Coupon couponModel = new Coupon();
        Offer offerModel = new Offer();
        InvoicesClass invoiceModel = new InvoicesClass();
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

                chk_allCoupon.IsChecked = true;

                //await Task.Delay(500);
                btn_coupons_Click(btn_coupons , null);
                //btn_offers_Click(btn_offers , null);
                //Btn_invoicesClasses_Click(btn_invoicesClasses , null);

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
        async Task Search()
        {
            searchText = txt_search.Text.ToLower();
            if (selectedTab == 0)
            {
                if (coupons is null)
                    await RefreshCouponsList();
                await coupounSearch();
            }
            else if(selectedTab == 1)
            {
                if (offers is null)
                    await RefreshOffersList();
                await offerSearch();
            }
            else if (selectedTab == 2)
            {
                if (invoicesClasses is null)
                    await RefreshInvoicesList();
                await invoiceSearch();
            }
           
            RefreshView();
        }
        void RefreshView()
        {
            List<int> selected = new List<int>();
            if (selectedTab == 0)
            {
                selected = selectedcouponId;
                dgInvoice.ItemsSource = couponsQuery;
            }
            if (selectedTab == 1)
            {
                selected = selectedOfferId;
                dgInvoice.ItemsSource = offersQuery;
            }
            if (selectedTab == 2)
            {
                selected = selectedInvoiceId;
                dgInvoice.ItemsSource = invoicesClassesQuery;
            }

            txt_count.Text = dgInvoice.Items.Count.ToString();

            fillColumnChart(selected);
            fillPieChart(selected);
            fillRowChart(selected);
        }

        async Task coupounSearch()
        {
            couponsQuery = coupons
               .Where(s =>
            (
            s.invNumber.ToLower().Contains(searchText)
            ||
            (s.invBarcode != null ? s.invBarcode.ToLower().Contains(searchText) : false)
            ||
            (s.Copname != null ? s.Copname.ToLower().Contains(searchText) : false)
            ||
            (s.Copcode != null ? s.Copcode.ToLower().Contains(searchText) : false)
            ||
            (s.ITitemUnitName1 != null ? s.ITitemUnitName1.ToLower().Contains(searchText) : false)
            )
            &&
            //start date
            (dp_couponStartDate.SelectedDate != null   ? s.invDate >= dp_couponStartDate.SelectedDate : true)
            &&
            //end date
            (dp_couponEndDate.SelectedDate   != null   ? s.invDate <= dp_couponEndDate.SelectedDate : true)
            &&
            //start time
            (dt_couponStartTime.SelectedTime != null   ? s.invDate >= dt_couponStartTime.SelectedTime : true)
            &&
            //end time
            (dt_couponEndTime.SelectedTime   != null   ? s.invDate <= dt_couponEndTime.SelectedTime : true)
            ).ToList();
            
            couponsQuery = couponsQuery.Where(j => (selectedcouponId.Count != 0 ? selectedcouponId.Contains((int)j.CopcId) : true)).ToList();
        }

        async Task offerSearch()
        {
            offersQuery = offers
               .Where(s =>
            (
            s.invNumber.ToLower().Contains(searchText)
            ||
            (s.invBarcode != null ? s.invBarcode.ToLower().Contains(searchText) : false)
            ||
            (s.Oname != null ? s.Oname.ToLower().Contains(searchText) : false)
            ||
            (s.Ocode != null ? s.Ocode.ToLower().Contains(searchText) : false)
            ||
            (s.ITitemUnitName1 != null ? s.ITitemUnitName1.ToLower().Contains(searchText) : false)
            )
            &&
            //start date
            (dp_couponStartDate.SelectedDate != null ? s.invDate >= dp_couponStartDate.SelectedDate : true)
            &&
            //end date
            (dp_couponEndDate.SelectedDate != null ? s.invDate <= dp_couponEndDate.SelectedDate : true)
            &&
            //start time
            (dt_couponStartTime.SelectedTime != null ? s.invDate >= dt_couponStartTime.SelectedTime : true)
            &&
            //end time
            (dt_couponEndTime.SelectedTime != null ? s.invDate <= dt_couponEndTime.SelectedTime : true)
            ).ToList();

            offersQuery = offersQuery.Where(j => (selectedOfferId.Count != 0 ? selectedOfferId.Contains((int)j.OofferId) : true)).ToList();
        }

        async Task invoiceSearch()
        {
            invoicesClassesQuery = invoicesClasses
              .Where(s =>
           (
           s.invNumber.ToLower().Contains(searchText)
           ||
            (s.invBarcode != null ? s.invBarcode.ToLower().Contains(searchText) : false)
           ||
           (s.invoicesClassName != null ? s.invoicesClassName.ToLower().Contains(searchText) : false)
           )
           &&
           //start date
           (dp_couponStartDate.SelectedDate != null ? s.invDate >= dp_couponStartDate.SelectedDate : true)
           &&
           //end date
           (dp_couponEndDate.SelectedDate != null ? s.invDate <= dp_couponEndDate.SelectedDate : true)
           &&
           //start time
           (dt_couponStartTime.SelectedTime != null ? s.invDate >= dt_couponStartTime.SelectedTime : true)
           &&
           //end time
           (dt_couponEndTime.SelectedTime != null ? s.invDate <= dt_couponEndTime.SelectedTime : true)
           ).ToList();

            invoicesClassesQuery = invoicesClassesQuery.Where(j => (selectedInvoiceId.Count != 0 ? selectedInvoiceId.Contains((int)j.invClassId) : true)).ToList();
        }

        async Task<IEnumerable<ItemTransferInvoice>> RefreshCouponsList()
        {
            coupons = await statisticModel.GetSalecoupon((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);
            return coupons;
        }
        async Task<IEnumerable<ItemTransferInvoice>> RefreshOffersList()
        {
            offers = await statisticModel.GetPromoOffer((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);
            return offers;
        }
        async Task<IEnumerable<SalesMembership>> RefreshInvoicesList()
        {
            invoicesClasses = await statisticModel.GetInvoiceClass((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);
            return invoicesClasses;
        }
        private void translate()
        {
            tt_coupon.Content = AppSettings.resourcemanager.GetString("trCoupons");
            tt_offers.Content = AppSettings.resourcemanager.GetString("trOffer");
            tt_invoicesClasses.Content = AppSettings.resourcemanager.GetString("invoicesClasses");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Coupons, AppSettings.resourcemanager.GetString("trCouponHint"));

            chk_allCoupon.Content = AppSettings.resourcemanager.GetString("trAll");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_couponEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_couponStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dt_couponEndTime, AppSettings.resourcemanager.GetString("trEndTime") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dt_couponStartTime, AppSettings.resourcemanager.GetString("trStartTime") + "...");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_No.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_date.Header = AppSettings.resourcemanager.GetString("trDate");
            col_coupon.Header = AppSettings.resourcemanager.GetString("trCoupon");
            col_code.Header = AppSettings.resourcemanager.GetString("trCode");
            col_coupoValue.Header = AppSettings.resourcemanager.GetString("trValue");
            col_offersValue.Header = AppSettings.resourcemanager.GetString("trValue");
            col_couponTotalValue.Header = AppSettings.resourcemanager.GetString("trDiscount");
            col_offersTotalValue.Header = AppSettings.resourcemanager.GetString("trDiscount");
            col_offers.Header = AppSettings.resourcemanager.GetString("trOffer");
            col_item.Header = AppSettings.resourcemanager.GetString("trItem");
            col_price.Header = AppSettings.resourcemanager.GetString("trPrice");

            col_offerCode.Header = AppSettings.resourcemanager.GetString("trCode");
            col_itQuantity.Header = AppSettings.resourcemanager.GetString("trQTR");

            col_invoiceTotal.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_invoiceClass.Header = AppSettings.resourcemanager.GetString("trInvoiceClass");
            col_invoiceValue.Header = AppSettings.resourcemanager.GetString("trValue");
            col_invoiceTotalValue.Header = AppSettings.resourcemanager.GetString("trDiscount");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }

        private void fillComboCoupon()
        {
            cb_Coupons.SelectedValuePath = "CopcId";
            cb_Coupons.DisplayMemberPath = "Copname";
            var lst = coupons.GroupBy(i => i.CopcId).Select(i => new { i.FirstOrDefault().CopcId, i.FirstOrDefault().Copname });
            cb_Coupons.ItemsSource = lst;

        }

        private void fillComboOffer()
        {
            cb_Coupons.SelectedValuePath = "OofferId";
            cb_Coupons.DisplayMemberPath = "Oname";
            var lst = offers.GroupBy(i => i.OofferId).Select(i => new { i.FirstOrDefault().OofferId, i.FirstOrDefault().Oname });
            cb_Coupons.ItemsSource = lst;
        }

        private void fillComboInvoice()
        {
            cb_Coupons.SelectedValuePath = "invClassId";
            cb_Coupons.DisplayMemberPath = "invoicesClassName";
            var lst = invoicesClasses.GroupBy(i => i.invClassId).Select(i => new { i.FirstOrDefault().invClassId, i.FirstOrDefault().invoicesClassName });
            cb_Coupons.ItemsSource = lst;
        }
        private static void fillDates(DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime)
        {
            if (startDate.SelectedDate != null && startTime.SelectedTime != null)
            {
                string x = startDate.SelectedDate.Value.Date.ToShortDateString();
                string y = startTime.SelectedTime.Value.ToShortTimeString();
                string resultStartTime = x + " " + y;
                startTime.SelectedTime = DateTime.Parse(resultStartTime);
                startDate.SelectedDate = DateTime.Parse(resultStartTime);
            }
            if (endDate.SelectedDate != null && endTime.SelectedTime != null)
            {
                string x = endDate.SelectedDate.Value.Date.ToShortDateString();
                string y = endTime.SelectedTime.Value.ToShortTimeString();
                string resultEndTime = x + " " + y;
                endTime.SelectedTime = DateTime.Parse(resultEndTime);
                endDate.SelectedDate = DateTime.Parse(resultEndTime);
            }
        }
        IEnumerable<ItemTransferInvoice> invLst = null;
        private IEnumerable<ItemTransferInvoice> fillList(IEnumerable<ItemTransferInvoice> Invoices, DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime)
        {
            fillDates(startDate, endDate, startTime, endTime);
            var result = Invoices.Where(x => (

               ((startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
                && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
                && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
                && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)
                )));

            invLst = result;
            return result;
        }
        IEnumerable<SalesMembership> salesLst = null;
        private IEnumerable<SalesMembership> fillSalesList(IEnumerable<SalesMembership> sales, DatePicker startDate, DatePicker endDate, TimePicker startTime, TimePicker endTime)
        {
            fillDates(startDate, endDate, startTime, endTime);
            var result = sales.Where(x => (

               ((startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
                && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)
                && (startTime.SelectedTime != null ? x.invDate >= startTime.SelectedTime : true)
                && (endTime.SelectedTime != null ? x.invDate <= endTime.SelectedTime : true)
                )));

            salesLst = result;
            return result;
        }

      

      
        private void hideSatacks()
        {
            stk_tagsCoupons.Visibility = Visibility.Collapsed;
            stk_tagsOffers.Visibility = Visibility.Collapsed;
            stk_tagsInvoices.Visibility = Visibility.Collapsed;
        }

        public void paint()
        {
            //bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);

            bdr_coupon.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_offers.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_invoiceClass.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;

            path_coupons.Fill = Brushes.White;
            path_offers.Fill = Brushes.White;
            path_invoicesClasses.Fill = Brushes.White;
        }

        private void hidAllColumns()
        {
            for (int i = 2; i < dgInvoice.Columns.Count; i++)
                dgInvoice.Columns[i].Visibility = Visibility.Hidden;
        }

        #endregion

        #region charts
        private void fillPieChart(List<int> stackedButton)
        {
            List<string> titles = new List<string>();
            IEnumerable<int> x = null;

           
            if (selectedTab == 0)
            {
                var temp = couponsQuery;

                titles.Clear();
                temp = temp.Where(j => (selectedcouponId.Count != 0 ? stackedButton.Contains((int)j.CopcId) : true)).ToList();
                var titleTemp = temp.GroupBy(m => m.Copname);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.CopcId).Select(s => new { CopcId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }

            else if (selectedTab == 1)
            {
                var temp = offersQuery;

                titles.Clear();
                temp = temp.Where(j => (selectedOfferId.Count != 0 ? stackedButton.Contains((int)j.OofferId) : true)).ToList();
                var titleTemp = temp.GroupBy(m => m.Oname);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.OofferId).Select(s => new { OofferId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }

            else if (selectedTab == 2)
            {
                var temp = invoicesClassesQuery;

                titles.Clear();
                temp = temp.Where(j => (selectedInvoiceId.Count != 0 ? stackedButton.Contains((int)j.invClassId) : true)).ToList();
                var titleTemp = temp.GroupBy(m => m.invoicesClassName);
                titles.AddRange(titleTemp.Select(jj => jj.Key));
                var result = temp.GroupBy(s => s.invClassId).Select(s => new { OofferId = s.Key, count = s.Count() });
                x = result.Select(m => m.count);
            }

            SeriesCollection piechartData = new SeriesCollection();
            int xCount = 0;
            if (x.Count() <= 6) xCount = x.Count();
            for (int i = 0; i < xCount; i++)
            {
                List<int> final = new List<int>();

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
                int finalSum = 0;
                for (int i = 6; i < x.Count(); i++)
                {
                    finalSum = finalSum + x.ToList().Skip(i).FirstOrDefault();
                }
                if (finalSum != 0)
                {
                    List<int> final = new List<int>();
                    final.Add(finalSum);
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

        private void fillColumnChart(List<int> stackedButton)
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<int> x = null;
            IEnumerable<int> y = null;
            IEnumerable<int> z = null;

            if (selectedTab == 0)
            {
                var temp = couponsQuery;

                temp = temp.Where(j => (selectedcouponId.Count != 0 ? stackedButton.Contains((int)j.CopcId) : true)).ToList();
                var result = temp.GroupBy(s => s.CopcId).Select(s => new
                {
                    CopcId = s.Key,
                    countP = s.Where(m => m.invType == "s").Count(),
                    countPb = s.Where(m => m.invType == "ts").Count(),
                    countD = s.Where(m => m.invType == "ss").Count()

                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.Copname).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }

            else if (selectedTab == 1)
            {
                var temp = offersQuery;

                temp = temp.Where(j => (selectedOfferId.Count != 0 ? stackedButton.Contains((int)j.OofferId) : true)).ToList();
                var result = temp.GroupBy(s => s.OofferId).Select(s => new
                {
                    CopcId = s.Key,
                    countP = s.Where(m => m.invType == "s").Count(),
                    countPb = s.Where(m => m.invType == "ts").Count(),
                    countD = s.Where(m => m.invType == "ss" ).Count()

                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.Oname).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }
            else if (selectedTab == 2)
            {
                var temp = invoicesClassesQuery;

                temp = temp.Where(j => (selectedInvoiceId.Count != 0 ? stackedButton.Contains((int)j.invClassId) : true)).ToList();
                var result = temp.GroupBy(s => s.invClassId).Select(s => new
                {
                    CopcId = s.Key,
                    countP = s.Where(m => m.invType == "s").Count(),
                    countPb = s.Where(m => m.invType == "ts").Count(),
                    countD = s.Where(m => m.invType == "ss").Count()

                });
                x = result.Select(m => m.countP);
                y = result.Select(m => m.countPb);
                z = result.Select(m => m.countD);
                var tempName = temp.GroupBy(s => s.invoicesClassName).Select(s => new
                {
                    uUserName = s.Key
                });
                names.AddRange(tempName.Select(nn => nn.uUserName));
            }

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> cP = new List<int>();
            List<int> cPb = new List<int>();
            List<int> cD = new List<int>();

            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trDiningHallType"),
                AppSettings.resourcemanager.GetString("trTakeAway"),
                AppSettings.resourcemanager.GetString("trSelfService")
            };
            int xCount = 0;
            if (x.Count() <= 6) xCount = x.Count();
            for (int i = 0; i < xCount; i++)
            {
                cP.Add(x.ToList().Skip(i).FirstOrDefault());
                cPb.Add(y.ToList().Skip(i).FirstOrDefault());
                cD.Add(z.ToList().Skip(i).FirstOrDefault());
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            if (x.Count() > 6)
            {
                int cPSum = 0, cPbSum = 0, cDSum = 0;
                for (int i = 0; i < xCount; i++)
                {
                    cPSum = cPSum + x.ToList().Skip(i).FirstOrDefault();
                    cPbSum = cPbSum + y.ToList().Skip(i).FirstOrDefault();
                    cDSum = cDSum + z.ToList().Skip(i).FirstOrDefault();
                }
                if (!((cPSum == 0) && (cPbSum == 0) && (cDSum == 0)))
                {
                    cP.Add(cPSum);
                    cPb.Add(cPbSum);
                    cD.Add(cDSum);
                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            //3 فوق بعض
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cP.AsChartValues(),
                Title = titles[0],
                DataLabels = true,
            });

            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cPb.AsChartValues(),
                Title = titles[1],
                DataLabels = true,
            });

            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cD.AsChartValues(),
                Title = titles[2],
                DataLabels = true,
            });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillRowChart(List<int> stackedButton)
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<decimal> pTemp = null;

            if (selectedTab == 0)
            {
                names.Clear();

                var temp = couponsQuery;

                temp = temp.Where(j => (selectedcouponId.Count != 0 ? stackedButton.Contains((int)j.CopcId) : true)).ToList();
                var result = temp.GroupBy(s => new { s.CopcId }).Select(s => new
                {
                    CopcId = s.FirstOrDefault().CopcId,
                    copName = s.FirstOrDefault().Copname,
                    invId = s.FirstOrDefault().invoiceId,
                    copTotalValue = s.Sum(x => x.couponTotalValue)
                }
                );

                var name = temp.GroupBy(s => s.invoiceId).Select(s => new
                {
                    uUserName = s.FirstOrDefault().Copname
                });
                names.AddRange(name.Select(nn => nn.uUserName));
                pTemp = result.Select(x => (decimal)x.copTotalValue);
            }
            else if (selectedTab == 1)
            {
                names.Clear();

                var temp = offersQuery;

                temp = temp.Where(j => (selectedOfferId.Count != 0 ? stackedButton.Contains((int)j.OofferId) : true)).ToList();
                var result1 = temp.GroupBy(s => new { s.OofferId, s.ITitemUnitId }).Select(s => new
                {
                    offerId = s.FirstOrDefault().OofferId,
                    offerName = s.FirstOrDefault().Oname,
                    itemId = s.FirstOrDefault().ITitemUnitId,
                    offerTotalValue = s.Sum(x => x.offerTotalValue)
                }
             );

                var name = result1.GroupBy(s => s.offerId).Select(s => new
                {
                    uUserName = s.FirstOrDefault().offerName,
                    offerTotalValue = s.Sum(x => x.offerTotalValue)
                });
                names.AddRange(name.Select(nn => nn.uUserName));
                pTemp = name.Select(x => (decimal)x.offerTotalValue);
            }

            else if (selectedTab == 2)
            {
                names.Clear();

                var temp = invoicesClassesQuery;

                temp = temp.Where(j => (selectedInvoiceId.Count != 0 ? stackedButton.Contains((int)j.invClassId) : true)).ToList();
                var result1 = temp.GroupBy(s => new { s.invClassId}).Select(s => new
                {
                    invClassId = s.FirstOrDefault().invClassId,
                    invoicesClassName = s.FirstOrDefault().invoicesClassName,
                    invId = s.FirstOrDefault().invoiceId,
                    totalValue = s.Sum(x => x.total)
                }
             );

                var name = result1.GroupBy(s => s.invClassId).Select(s => new
                {
                    uUserName = s.FirstOrDefault().invoicesClassName,
                    offerTotalValue = s.Sum(x => x.totalValue)
                });
                names.AddRange(name.Select(nn => nn.uUserName));
                pTemp = name.Select(x => (decimal)x.offerTotalValue);
            }

            SeriesCollection rowChartData = new SeriesCollection();
            List<decimal> purchase = new List<decimal>();
            List<decimal> returns = new List<decimal>();
            List<decimal> sub = new List<decimal>();
            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trTotalDiscountValue"),
                //MainWindow.resourcemanager.GetString("trTotalReturn"),
                //MainWindow.resourcemanager.GetString("trSum"),
            };
            int xCount = 0;
            if (pTemp.Count() <= 6) xCount = pTemp.Count();
            for (int i = 0; i < xCount; i++)
            {
                purchase.Add(pTemp.ToList().Skip(i).FirstOrDefault());
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            if (pTemp.Count() > 6)
            {
                decimal purchaseSum = 0;
                for (int i = xCount; i < pTemp.Count(); i++)
                {
                    purchaseSum = purchaseSum + pTemp.ToList().Skip(i).FirstOrDefault();
                }
                if (purchaseSum != 0)
                {
                    purchase.Add(purchaseSum);
                    MyAxis.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            rowChartData.Add(
                  new LineSeries
                  {
                      Values = purchase.AsChartValues(),
                      Title = titles[0]
                  });

            DataContext = this;
            rowChart.Series = rowChartData;
        }


        #endregion

        #region Events

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        #region tabControl

        private async void btn_coupons_Click(object sender, RoutedEventArgs e)
        {//coupons
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Coupons, AppSettings.resourcemanager.GetString("trCoponHint"));
                selectedTab = 0;
                txt_search.Text = "";
                hideSatacks();
                stk_tagsCoupons.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_coupon);
                path_coupons.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                await Search();
                fillComboCoupon();

                hidAllColumns();
                col_code.Visibility = Visibility.Visible;
                col_coupon.Visibility = Visibility.Visible;
                col_coupoValue.Visibility = Visibility.Visible;
                col_couponTotalValue.Visibility = Visibility.Visible;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void btn_offers_Click(object sender, RoutedEventArgs e)
        {//offers
            try
            {
                 HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Coupons, AppSettings.resourcemanager.GetString("trOfferHint"));
                selectedTab = 1;
                txt_search.Text = "";
                hideSatacks();
                stk_tagsOffers.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_offers);
                path_offers.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                await Search();
                fillComboOffer();

                hidAllColumns();

                col_offerCode.Visibility = Visibility.Visible;
                col_offers.Visibility = Visibility.Visible;
                col_offersValue.Visibility = Visibility.Visible;
                col_item.Visibility = Visibility.Visible;
                col_price.Visibility = Visibility.Visible;
                col_itQuantity.Visibility = Visibility.Visible;
                col_offersTotalValue.Visibility = Visibility.Visible;
                //col_total.Visibility = Visibility.Visible;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_invoicesClasses_Click(object sender, RoutedEventArgs e)
        {//invoicesClasses
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());
                MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Coupons, AppSettings.resourcemanager.GetString("trInvoiceClass")+"...");
                selectedTab = 2;
                txt_search.Text = "";
                hideSatacks();
                stk_tagsInvoices.Visibility = Visibility.Visible;

                paint();
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_invoiceClass);
                path_invoicesClasses.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                await Search();
                fillComboInvoice();

                hidAllColumns();

                col_invoiceTotal.Visibility = Visibility.Visible;
                col_invoiceClass.Visibility = Visibility.Visible;
                col_invoiceValue.Visibility = Visibility.Visible;
                col_invoiceTotalValue.Visibility = Visibility.Visible;
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            callSearch(sender);
        }


        private async void Chk_allCoupon_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_Coupons.SelectedIndex = -1;
                cb_Coupons.IsEnabled = false;

                hideSatacks();
                //coupon stack
                selectedCoupon = null;
                selectedcouponId.Clear();
                comboCouponTemp.Clear();
                dynamicComboCoupon.Clear();
                stk_tagsCoupons.Children.Clear();
                //offer stack
                selectedOffer = null;
                selectedOfferId.Clear();
                comboOfferTemp.Clear();
                dynamicComboOffer.Clear();
                stk_tagsOffers.Children.Clear();
                //invoice stack
                selectedInvoice = null;
                selectedInvoiceId.Clear();
                comboInvoiceTemp.Clear();
                dynamicComboInvoice.Clear();
                stk_tagsInvoices.Children.Clear();

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Chk_allCoupon_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                cb_Coupons.IsEnabled = true;

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                chk_allCoupon.IsChecked = true;
                dp_couponStartDate.SelectedDate = null;
                dp_couponEndDate.SelectedDate = null;
                dt_couponStartTime.SelectedTime = null;
                dt_couponEndTime.SelectedTime = null;

                txt_search.Text = "";

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Chip_OnDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                var currentChip = (Chip)sender;
                if (selectedTab == 0)
                {
                    stk_tagsCoupons.Children.Remove(currentChip);
                    var m = comboCouponTemp.Where(j => j.cId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                    dynamicComboCoupon.Add(m.FirstOrDefault());
                    selectedcouponId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                    if (selectedcouponId.Count == 0)
                    {
                        cb_Coupons.SelectedItem = null;
                    }
                }
                else if (selectedTab == 1)
                {
                    stk_tagsOffers.Children.Remove(currentChip);
                    var m = comboOfferTemp.Where(j => j.offerId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                    dynamicComboOffer.Add(m.FirstOrDefault());
                    selectedOfferId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                    if (selectedOfferId.Count == 0)
                    {
                        cb_Coupons.SelectedItem = null;
                    }
                }
                else if (selectedTab == 2)
                {
                    stk_tagsInvoices.Children.Remove(currentChip);
                    var m = comboInvoiceTemp.Where(j => j.invClassId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                    dynamicComboInvoice.Add(m.FirstOrDefault());
                    selectedInvoiceId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                    if (selectedInvoiceId.Count == 0)
                    {
                        cb_Coupons.SelectedItem = null;
                    }
                }
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            callSearch(sender);
        }

        private void dt_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            callSearch(sender);
        }

        private async void cb_Coupons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (cb_Coupons.SelectedItem != null)
                {
                    if (selectedTab == 0)
                    {
                        stk_tagsCoupons.Visibility = Visibility.Visible;
                        if (stk_tagsCoupons.Children.Count < 5)
                        {
                            int cId = (int)cb_Coupons.SelectedValue;

                            selectedCoupon = await couponModel.getById(cId);
                            var c = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedCoupon.name,
                                Name = "btn" + selectedCoupon.cId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            c.DeleteClick += Chip_OnDeleteClick;
                            stk_tagsCoupons.Children.Add(c);
                            comboCouponTemp.Add(selectedCoupon);
                            selectedcouponId.Add(selectedCoupon.cId);
                            dynamicComboCoupon.Remove(selectedCoupon);
                        }
                    }
                    if (selectedTab == 1)
                    {
                        stk_tagsOffers.Visibility = Visibility.Visible;
                        if (stk_tagsOffers.Children.Count < 5)
                        {
                            int oId = (int)cb_Coupons.SelectedValue;

                            selectedOffer = await offerModel.getOfferById(oId);
                            var o = new MaterialDesignThemes.Wpf.Chip()
                            {
                                Content = selectedOffer.name,
                                Name = "btn" + selectedOffer.offerId.ToString(),
                                IsDeletable = true,
                                Margin = new Thickness(5, 0, 5, 0)
                            };
                            o.DeleteClick += Chip_OnDeleteClick;
                            stk_tagsOffers.Children.Add(o);
                            comboOfferTemp.Add(selectedOffer);
                            selectedOfferId.Add(selectedOffer.offerId);
                            dynamicComboOffer.Remove(selectedOffer);
                        }
                    }
                    if (selectedTab == 2)
                    {
                        stk_tagsInvoices.Visibility = Visibility.Visible;
                        if (stk_tagsInvoices.Children.Count < 5)
                        {
                            int iId = (int)cb_Coupons.SelectedValue;

                            selectedInvoice = await invoiceModel.GetById(iId);
                            try
                            {
                                var i = new MaterialDesignThemes.Wpf.Chip()
                                {
                                    Content = selectedInvoice.name,
                                    Name = "btn" + selectedInvoice.invClassId.ToString(),
                                    IsDeletable = true,
                                    Margin = new Thickness(5, 0, 5, 0)
                                };
                                i.DeleteClick += Chip_OnDeleteClick;
                                stk_tagsInvoices.Children.Add(i);
                                comboInvoiceTemp.Add(selectedInvoice);
                                selectedInvoiceId.Add(selectedInvoice.invClassId);
                                dynamicComboInvoice.Remove(selectedInvoice);
                            }
                            catch { }
                        }
                    }

                    await Search();

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
       
        #region reports
        private void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            List<ItemTransferInvoice> query = new List<ItemTransferInvoice>();

            string addpath = "";

            string firstTitle = "promotion";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Promotion\Ar\ArCoupons.rdlc";
                    secondTitle = "coupon";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if(selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Promotion\Ar\ArOffers.rdlc";
                    secondTitle = "offers";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                 else
                {
                    addpath = @"\Reports\StatisticReport\Sale\Promotion\Ar\ArInvClass.rdlc";
                    secondTitle = "invClasses";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
            }
            else
            {
                //english
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Promotion\En\EnCoupons.rdlc";
                    secondTitle = "coupon";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if(selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Sale\Promotion\En\EnOffers.rdlc";
                    secondTitle = "offers";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

                }
                else  
                {
                    addpath = @"\Reports\StatisticReport\Sale\Promotion\En\EnInvClass.rdlc";
                    secondTitle = "invClasses";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

                }
            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            Title = AppSettings.resourcemanagerreport.GetString("SalesReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            //            couponsQuery
            //offersQuery
            //invoicesClassesQuery
            if (selectedTab == 0)
            {
                clsReports.SalePromoStsReport(couponsQuery, rep, reppath, paramarr);
            }
            else if (selectedTab == 1)
            {
                clsReports.SalePromoStsReport(offersQuery, rep, reppath, paramarr);
            }
            else
            {
                //invoicesClassesQuery
                clsReports.invoicClassReport(invoicesClassesQuery, rep, reppath, paramarr);
         
            }
            //  clsReports.SalePromoStsReport(couponsQuery, rep, reppath, paramarr); invoicClassReport
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
                //    {
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
