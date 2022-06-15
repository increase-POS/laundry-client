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
        IEnumerable<Agent> customersQuery;
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
                customersQuery = customers.Where(s => (
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
            dg_customer.ItemsSource = customersQuery.ToList();
        }
        private void translate()
        {
            // Title
            //if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
            //    txt_title.Text = AppSettings.resourcemanager.GetString(
            //   FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
            //   );

            txt_title.Text = AppSettings.resourcemanager.GetString("trCustomer");

            txt_details.Text = AppSettings.resourcemanager.GetString("trDetails");
            txt_name.Text = AppSettings.resourcemanager.GetString("trName");
            txt_code.Text = AppSettings.resourcemanager.GetString("trCode");
            txt_mobile.Text = AppSettings.resourcemanager.GetString("trMobile");
            txt_company.Text = AppSettings.resourcemanager.GetString("trCompany");
            txt_points.Text = AppSettings.resourcemanager.GetString("trPoints");
            txt_pointsHistory.Text = AppSettings.resourcemanager.GetString("trPointsHistory");

            //btn_clearPoints.Content = AppSettings.resourcemanager.GetString("trClearPoints");
            //btn_clearHistory.Content = AppSettings.resourcemanager.GetString("trClearHistory");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            dg_customer.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            //dg_customer.Columns[1].Header = AppSettings.resourcemanager.GetString("trPoints");
            //dg_customer.Columns[2].Header = AppSettings.resourcemanager.GetString("trPointsHistory");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");

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
        int points = 0;
        private void Dg_customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customer = dg_customer.SelectedItem as Agent;
            points = customer.points;
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

        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {

        }

        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private async void Dg_customer_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    if (bindingPath == "points")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var el = e.EditingElement as TextBox;
                        if (points != int.Parse(el.Text))
                        {
                            int res = await customer.UpdateAgentPoints(customer, MainWindow.posLogin.posId);
                        }
                        // rowIndex has the row index
                        // bindingPath has the column's binding
                        // el.Text has the new, user-entered value
                    }
                }
            }
        }
    }
}
