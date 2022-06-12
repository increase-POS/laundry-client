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

namespace laundryApp.View.sales.promotion.points
{
    /// <summary>
    /// Interaction logic for uc_customersPoints.xaml
    /// </summary>
    public partial class uc_customersPoints : UserControl
    {
        private static uc_customersPoints _instance;
        public static uc_customersPoints Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_customersPoints();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public uc_customersPoints()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }

        IEnumerable<Agent> customers;
        Agent customerModel = new Agent();
        Agent customer = new Agent();
        string searchText = "";

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            //try
            //{
            //    HelpClass.StartAwait(grid_main);

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

                await RefreshCustomersList();

                await Search();

            //    HelpClass.EndAwait(grid_main);
            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}

        }

        #region methods
       
        async Task<IEnumerable<Agent>> RefreshCustomersList()
        {
            //if (FillCombo.customersList == null)
            //    await FillCombo.RefreshCustomers();
            //else
            //    customers = FillCombo.customersList;
            await FillCombo.RefreshCustomers();
            customers = FillCombo.customersList.ToList();
            customers = customers.Where(c => c.isActive == 1).ToList();

            return customers;
        }
        async Task Search()
        {
            try
            {
                //if (customers is null)
                    await RefreshCustomersList();

                searchText = tb_search.Text.ToLower();
                customers = customers.Where(s => (
                    s.name.ToLower().Contains(searchText)
                || s.points.ToString().ToLower().Contains(searchText)
                || s.pointsHistory.ToString().ToLower().Contains(searchText)
                ));

                RefreshCustomersView();
               
            }
            catch { }
        }

        void RefreshCustomersView()
        {
            dg_customer.ItemsSource = customers.ToList();
        }
        private void translate()
        {
            // Title
            //if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
            //    txt_title.Text = AppSettings.resourcemanager.GetString(
            //   FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
            //   );

            //txt_title.Text = AppSettings.resourcemanager.GetString("trDriversManagement");
            txt_details.Text = AppSettings.resourcemanager.GetString("trDetails");

            //chk_drivers.Content = AppSettings.resourcemanager.GetString("drivers");
            //chk_shippingCompanies.Content = AppSettings.resourcemanager.GetString("trShippingCompanies");

            //txt_driverUserName.Text = AppSettings.resourcemanager.GetString("trUserName");
            //txt_driverName.Text = AppSettings.resourcemanager.GetString("trDriver");
            //txt_driverMobile.Text = AppSettings.resourcemanager.GetString("trMobile");
            //txt_driverSectorsCount.Text = AppSettings.resourcemanager.GetString("trResidentialSectors");
            //txt_driverOrdersCount.Text = AppSettings.resourcemanager.GetString("trOrders");
            //txt_driverStatus.Text = AppSettings.resourcemanager.GetString("trStatus");

            //txt_companyName.Text = AppSettings.resourcemanager.GetString("trCompany");
            //txt_companyMobile.Text = AppSettings.resourcemanager.GetString("trMobile");
            //txt_companyEmail.Text = AppSettings.resourcemanager.GetString("trEmail");
            //txt_companyOrdersCount.Text = AppSettings.resourcemanager.GetString("trOrders");
            //txt_companyStatus.Text = AppSettings.resourcemanager.GetString("trStatus");

            //txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            //txt_print.Text = AppSettings.resourcemanager.GetString("trPrint");
            //txt_residentialSectors.Text = AppSettings.resourcemanager.GetString("trResidentialSectors");
            //txt_activeInactive.Text = AppSettings.resourcemanager.GetString("activate");

            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            //col_driversUsername.Header = AppSettings.resourcemanager.GetString("trUserName");
            //col_driversName.Header = AppSettings.resourcemanager.GetString("trName");
            //col_driversMobile.Header = AppSettings.resourcemanager.GetString("trMobile");
            //col_driversAvailable.Header = AppSettings.resourcemanager.GetString("trStatus");

            //col_companyName.Header = AppSettings.resourcemanager.GetString("trName");
            //col_companyMobile.Header = AppSettings.resourcemanager.GetString("trMobile");
            //col_companyEmail.Header = AppSettings.resourcemanager.GetString("trEmail");
            //col_companyAvailable.Header = AppSettings.resourcemanager.GetString("trStatus");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

        }
        #endregion

        #region events
        #endregion

        #region reports
        #endregion

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dg_customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Btn_clearPoints_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_clearHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {


            // refresh list
            await Search();
        }
    }
}
