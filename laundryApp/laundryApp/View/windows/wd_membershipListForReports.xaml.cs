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
using System.Windows.Shapes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_membershipListForReports.xaml
    /// </summary>
    public partial class wd_membershipListForReports : Window
    {
        string _title = "";
        public string membershipType = "";

        public List<CouponInvoice> CouponInvoiceList = new List<CouponInvoice>();
        public List<ItemTransfer> itemsTransferList = new List<ItemTransfer>();
        public List<InvoicesClass> invoiceClassDiscountList = new List<InvoicesClass>();

        private static wd_membershipListForReports _instance;
        public static wd_membershipListForReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new wd_membershipListForReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public wd_membershipListForReports()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.Key == Key.Return)
                //{
                //    Btn_select_Click(null, null);
                //}
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
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
                #endregion

                await fillDataGrid();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {

        }

        #region methods
        private void translat()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString(_title);
            //coupn
            col_cCode.Header = AppSettings.resourcemanager.GetString("trCode");
            col_cName.Header = AppSettings.resourcemanager.GetString("trName");
            col_cTypeValue.Header = AppSettings.resourcemanager.GetString("trValue");
            //offer
            col_oName.Header = AppSettings.resourcemanager.GetString("trName");
            col_oTypeValue.Header = AppSettings.resourcemanager.GetString("trValue");
            col_oQuantity.Header = AppSettings.resourcemanager.GetString("trQTR");
            //invoice
            col_iName.Header = AppSettings.resourcemanager.GetString("trName");
            col_iTypeValue.Header = AppSettings.resourcemanager.GetString("trValue");

            col_discount.Header = AppSettings.resourcemanager.GetString("trDiscount");

        }


        async Task fillDataGrid()
        {
            hideAllColumns();

            if (membershipType == "c")
            {
                dg_memberships.ItemsSource = CouponInvoiceList;

                //view columns
                col_cCode.Visibility = Visibility.Visible;
                col_cName.Visibility = Visibility.Visible;
                col_cTypeValue.Visibility = Visibility.Visible;

                _title = "trCoupons";

                col_coupon.Width = new GridLength(1, GridUnitType.Star);
            }
            else if (membershipType == "o")
            {
                dg_memberships.ItemsSource = itemsTransferList;

                //view columns
                col_oName.Visibility = Visibility.Visible;
                col_oTypeValue.Visibility = Visibility.Visible;
                col_oQuantity.Visibility = Visibility.Visible;

                _title = "trOffers";

                col_offer.Width = new GridLength(1, GridUnitType.Star);
            }
            else if (membershipType == "i")
            {
                dg_memberships.ItemsSource = invoiceClassDiscountList;

                //view columns
                col_iName.Visibility = Visibility.Visible;
                col_iTypeValue.Visibility = Visibility.Visible;

                _title = "trInvoicesClasses";

                col_invoice.Width = new GridLength(1, GridUnitType.Star);
            }

            translat();
        }

        private void hideAllColumns()
        {
            col_coupon.Width = new GridLength(0);
            col_offer.Width = new GridLength(0);
            col_invoice.Width = new GridLength(0);

            for (int i = 0; i < dg_memberships.Columns.Count-1; i++)
                dg_memberships.Columns[i].Visibility = Visibility.Hidden;


        }
        #endregion
    }
}
