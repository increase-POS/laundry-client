using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
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
using Microsoft.Win32;
using System.IO;
 
using Microsoft.Reporting.WinForms;
using laundryApp.Classes.ApiClasses;
using System.ComponentModel;

namespace laundryApp.View.delivery
{
    /// <summary>
    /// Interaction logic for uc_driversManagement.xaml
    /// </summary>
    public partial class uc_driversManagement : UserControl
    {
        IEnumerable<User> drivers;
        User userModel = new User();
        User driver = new User();

        IEnumerable<ShippingCompanies> companies;
        ShippingCompanies companyModel = new ShippingCompanies();
        ShippingCompanies company = new ShippingCompanies();

        string searchText = "";
        byte tgl_driverState;

        string viewPermission = "driversManagement_view";
        string residentialSectorsPermission = "driversManagement_residentialSectors";
        string activateDriverPermission = "driversManagement_activateDriver";

        private static uc_driversManagement _instance;

        public static uc_driversManagement Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_driversManagement();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_driversManagement()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }
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

                await Search();
                await RefreshOrdersList();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region methods
        async Task Search()
        {
            try
            {
                if (chk_drivers.IsChecked.Value)
                {
                    if (drivers is null)
                        await RefreshDriversList();
                    searchText = tb_search.Text.ToLower();
                    drivers = drivers.Where(s => (
                       s.name.ToLower().Contains(searchText)
                    || s.mobile.ToString().ToLower().Contains(searchText)
                    ));
                    RefreshDriverView();
                }
                else if (chk_shippingCompanies.IsChecked.Value)
                {
                    if (companies is null)
                       await RefreshCompaniesList();
                    searchText = tb_search.Text.ToLower();
                    companies = companies.Where(x => x.deliveryType == "com").ToList();
                    companies = companies.Where(s => (
                       s.name.ToLower().Contains(searchText)
                    || s.mobile.ToString().ToLower().Contains(searchText)
                    ));
                    RefreshCompanyView();
                }
                    


            }
            catch { }
        }

        async Task refreshDriverSectors()
        {
            driverSectors = await residentialSector.GetResSectorsByUserId(driver.userId);
            tb_sectorsCount.Text = driverSectors.Count.ToString();
        }
        async Task<IEnumerable<User>> RefreshDriversList()
        {
            drivers = await userModel.GetUsersActive();
            drivers = drivers.Where(x => x.job == "deliveryEmployee");

            return drivers;
        }
        async Task<IEnumerable<ShippingCompanies>> RefreshCompaniesList()
        {
            companies = await companyModel.Get();
            return companies;
        }
        void RefreshDriverView()
        {
            dg_user.ItemsSource = drivers;
        }
        void RefreshCompanyView()
        {
            dg_user.ItemsSource = companies;
        }

        private void translate()
        {
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            //txt_title.Text = AppSettings.resourcemanager.GetString("trDriversManagement");
            txt_details.Text = AppSettings.resourcemanager.GetString("trDetails");

            chk_drivers.Content = AppSettings.resourcemanager.GetString("drivers");
            chk_shippingCompanies.Content = AppSettings.resourcemanager.GetString("trShippingCompanies");

            txt_driverUserName.Text = AppSettings.resourcemanager.GetString("trUserName");
            txt_driverName.Text = AppSettings.resourcemanager.GetString("trDriver");
            txt_driverMobile.Text = AppSettings.resourcemanager.GetString("trMobile");
            txt_driverSectorsCount.Text = AppSettings.resourcemanager.GetString("trResidentialSectors");
            txt_driverOrdersCount.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_driverStatus.Text = AppSettings.resourcemanager.GetString("trStatus");

            txt_companyName.Text = AppSettings.resourcemanager.GetString("trCompany");
            txt_companyMobile.Text = AppSettings.resourcemanager.GetString("trMobile");
            txt_companyEmail.Text = AppSettings.resourcemanager.GetString("trEmail");
            txt_companyOrdersCount.Text = AppSettings.resourcemanager.GetString("trOrders");
            txt_companyStatus.Text = AppSettings.resourcemanager.GetString("trStatus");

            txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            txt_print.Text = AppSettings.resourcemanager.GetString("trPrint");
            txt_residentialSectors.Text = AppSettings.resourcemanager.GetString("trResidentialSectors");
            txt_activeInactive.Text = AppSettings.resourcemanager.GetString("activate");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            col_driversUsername.Header = AppSettings.resourcemanager.GetString("trUserName");
            col_driversName.Header = AppSettings.resourcemanager.GetString("trName");
            col_driversMobile.Header = AppSettings.resourcemanager.GetString("trMobile");
            col_driversAvailable.Header = AppSettings.resourcemanager.GetString("trStatus");

            col_companyName.Header = AppSettings.resourcemanager.GetString("trName");
            col_companyMobile.Header = AppSettings.resourcemanager.GetString("trMobile");
            col_companyEmail.Header = AppSettings.resourcemanager.GetString("trEmail");
            col_companyAvailable.Header = AppSettings.resourcemanager.GetString("trStatus");


            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

        }
        #endregion
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                await RefreshDriversList();
                await RefreshCompaniesList();
                searchText = "";
                tb_search.Text = "";
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
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
        /*
        private async void Tgl_isActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (drivers is null)
                    await RefreshDriversList();
                tgl_driverState = 1;
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Tgl_isActive_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (drivers is null)
                    await RefreshDriversList();
                tgl_driverState = 0;
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        */
        private async void deliveryType_check(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                await RefreshOrdersList();
                if (cb.IsFocused)
                {
                    if (cb.IsChecked == true)
                    {
                        if (cb.Name == "chk_drivers")
                        {
                            chk_shippingCompanies.IsChecked = false;
                        }
                        else if (cb.Name == "chk_shippingCompanies")
                        {
                            chk_drivers.IsChecked = false;
                        }
                    }
                }
                
                await changeDeliveryType();
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void deliveryType_uncheck(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                   if (cb.Name == "chk_drivers")
                        chk_drivers.IsChecked = true;
                   else if (cb.Name == "chk_shippingCompanies")
                        chk_shippingCompanies.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task changeDeliveryType()
        {
            HelpClass.StartAwait(grid_main);

            if (col_driversUsername is null)
              await  Task.Delay(500);
            if (chk_drivers.IsChecked.Value)
            {
                col_driversUsername.Visibility = Visibility.Visible;
                col_driversName.Visibility = Visibility.Visible;
                col_driversMobile.Visibility = Visibility.Visible;
                col_driversAvailable.Visibility = Visibility.Visible;
                cd_residentialSectors.Width =  new GridLength(1, GridUnitType.Star) ;
                btn_residentialSectors.Visibility = Visibility.Visible;
                sp_driverDetails.Visibility = Visibility.Visible;
                tb_sectorsCount.Text = "0";
                this.DataContext = new User();

                col_companyName.Visibility = Visibility.Collapsed;
                col_companyMobile.Visibility = Visibility.Collapsed;
                col_companyEmail.Visibility = Visibility.Collapsed;
                col_companyAvailable.Visibility = Visibility.Collapsed;
                sp_companyDetails.Visibility = Visibility.Collapsed;
                
            }
            else if (chk_shippingCompanies.IsChecked.Value)
            {
                col_driversUsername.Visibility = Visibility.Collapsed;
                col_driversName.Visibility = Visibility.Collapsed;
                col_driversMobile.Visibility = Visibility.Collapsed;
                col_driversAvailable.Visibility = Visibility.Collapsed;
                cd_residentialSectors.Width = new GridLength(0, GridUnitType.Star);
                btn_residentialSectors.Visibility = Visibility.Collapsed;
                sp_driverDetails.Visibility = Visibility.Collapsed;

                col_companyName.Visibility = Visibility.Visible;
                col_companyMobile.Visibility = Visibility.Visible;
                col_companyEmail.Visibility = Visibility.Visible;
                col_companyAvailable.Visibility = Visibility.Visible;
                sp_companyDetails.Visibility = Visibility.Visible;
                this.DataContext = new ShippingCompanies();

            }



            await Search();

            HelpClass.EndAwait(grid_main);
        }

      

        int selectedIndexDataGrid = 0;
        List<ResidentialSectors> driverSectors = new List<ResidentialSectors>();
        ResidentialSectors residentialSector = new ResidentialSectors();

        
        private async void Dg_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//selection
            try
            {
                HelpClass.StartAwait(grid_main);
                
                if (dg_user.SelectedIndex != -1)
                {
                    int ordersCount = 0;

                    if (chk_drivers.IsChecked.Value)
                    {
                        driver = dg_user.SelectedItem as User;
                        selectedIndexDataGrid = dg_user.SelectedIndex;
                        this.DataContext = driver;
                        if (driver != null)
                        {
                            if (orders != null)
                                driverOrder = orders.Where(o => o.shipUserId == null ? false : (int)o.shipUserId == driver.userId).ToList();

                            if (driver.driverIsAvailable == 0)
                                txt_activeInactive.Text = AppSettings.resourcemanager.GetString("activate");
                            else
                                txt_activeInactive.Text = AppSettings.resourcemanager.GetString("deActivate");
                            await refreshDriverSectors();

                            if (chk_drivers.IsChecked == true)
                                ordersCount = driverOrder.Count();

                        }
                    }
                    else if (chk_shippingCompanies.IsChecked.Value)
                    {
                        
                        company = dg_user.SelectedItem as ShippingCompanies;
                        selectedIndexDataGrid = dg_user.SelectedIndex;
                        this.DataContext = company;
                        if (company != null)
                        {
                            if (orders != null)
                                driverOrder = orders.Where(o => (int)o.shippingCompanyId == company.shippingCompanyId && o.status == "Ready").ToList();

                            if (company.isActive == 0)
                                txt_activeInactive.Text = AppSettings.resourcemanager.GetString("activate");
                            else
                                txt_activeInactive.Text = AppSettings.resourcemanager.GetString("deActivate");

                            if (chk_shippingCompanies.IsChecked == true)
                                ordersCount = driverOrder.Count();

                        }
                    }
                    tb_driverOrdersCount.Text = ordersCount.ToString();

                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private async void Btn_residentialSectors_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (driver.userId > 0)
                {
                    HelpClass.StartAwait(grid_main);

                    if (FillCombo.groupObject.HasPermissionAction(residentialSectorsPermission, FillCombo.groupObjects, "one"))
                    {
                        Window.GetWindow(this).Opacity = 0.2;

                        wd_residentialSectorsList w = new wd_residentialSectorsList();
                        w.driverId = driver.userId;
                        w.ShowDialog();

                        Window.GetWindow(this).Opacity = 1;

                        await refreshDriverSectors();
                    }
                    else
                        Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                    HelpClass.EndAwait(grid_main);
                }
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_activeInactive_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(activateDriverPermission, FillCombo.groupObjects, "one"))
                {
                    string resultStr = "";
                    if(chk_drivers.IsChecked.Value)
                    {
                        if (driver.driverIsAvailable == 0)
                        {
                            driver.driverIsAvailable = 1;
                            resultStr = "popActivation";
                        }
                        else
                        {
                            driver.driverIsAvailable = 0;
                            resultStr = "popDeActivation";
                        }
                        int s = await driver.save(driver);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(resultStr), animation: ToasterAnimation.FadeIn);
                            await RefreshDriversList();
                            await Search();
                            dg_user.SelectedIndex = selectedIndexDataGrid;
                        }
                    }
                    else if (chk_shippingCompanies.IsChecked.Value)
                    {
                        if (company.isActive == 0)
                        {
                            company.isActive = 1;
                            resultStr = "popActivation";
                        }
                        else
                        {
                            company.isActive = 0;
                            resultStr = "popDeActivation";
                        }
                        int s = await company.save(company);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(resultStr), animation: ToasterAnimation.FadeIn);
                            await FillCombo.RefreshShippingCompanies();
                            await RefreshCompaniesList();
                            await Search();
                            dg_user.SelectedIndex = selectedIndexDataGrid;
                        }
                    }
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region report
        //report  parameters
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        List<Invoice> orders;
        OrderPreparing orderModel = new OrderPreparing();
        List<Invoice> driverOrder = new List<Invoice>();
        // end report parameters
        async Task<IEnumerable<Invoice>> RefreshOrdersList()
        {
            orders = await orderModel.GetOrdersWithDelivery(MainWindow.branchLogin.branchId, "Collected,Ready");
            orders = orders.Where(o => o.status == "Collected" || o.status == "Ready").ToList();
            return orders;
        }
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            
            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Delivery\Ar\ArDriversManag.rdlc";
            }
            else
            {
                addpath = @"\Reports\Delivery\En\EnDriversManag.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            string trDeliveryMan = "";
            string deliveryMan = "";
            if (chk_drivers.IsChecked.Value)
            {
                driver = dg_user.SelectedItem as User;

                trDeliveryMan = AppSettings.resourcemanagerreport.GetString("trDriver");
                if (driver != null)
                {
                    deliveryMan = driver.name + " " + driver.lastname;


                }
                else
                {
                    deliveryMan = "-";
                }
            }
            else if (chk_shippingCompanies.IsChecked.Value)
            {
                company = dg_user.SelectedItem as ShippingCompanies;
                trDeliveryMan = AppSettings.resourcemanagerreport.GetString("trShippingCompanynohint");

                if (company != null)
                {
                    deliveryMan = company.name;
                }
                else
                {
                    deliveryMan ="-";
                }
            }

            paramarr.Add(new ReportParameter("trDeliveryMan", trDeliveryMan));
            paramarr.Add(new ReportParameter("deliveryMan", deliveryMan));

            clsReports.driverManagement(driverOrder.ToList(), rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        //private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        //{
        //    //pdf
        //    try
        //    {

        //        HelpClass.StartAwait(grid_main);

        //        //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
        //        //{
        //        #region
        //        BuildReport();

        //        saveFileDialog.Filter = "PDF|*.pdf;";

        //        if (saveFileDialog.ShowDialog() == true)
        //        {
        //            string filepath = saveFileDialog.FileName;
        //            LocalReportExtensions.ExportToPDF(rep, filepath);
        //        }
        //        #endregion
        //        //}
        //        //else
        //        //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {

        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{

                #region
                BuildReport();
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                #endregion
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        //private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //        HelpClass.StartAwait(grid_main);

        //        //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
        //        //{
        //        #region
        //        //Thread t1 = new Thread(() =>
        //        //{
        //        BuildReport();
        //        this.Dispatcher.Invoke(() =>
        //        {
        //            saveFileDialog.Filter = "EXCEL|*.xls;";
        //            if (saveFileDialog.ShowDialog() == true)
        //            {
        //                string filepath = saveFileDialog.FileName;
        //                LocalReportExtensions.ExportToExcel(rep, filepath);
        //            }
        //        });


        //        //});
        //        //t1.Start();
        //        #endregion
        //        //}
        //        //else
        //        //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {

        //        HelpClass.EndAwait(grid_main);

        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
                #region
                Window.GetWindow(this).Opacity = 0.2;

                string pdfpath = "";
                //
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
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        //private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        //{//pie
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);

        //        //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
        //        //{
        //        #region
        //        //Window.GetWindow(this).Opacity = 0.2;
        //        //win_lvc win = new win_lvc(usersQuery, 3);
        //        //win.ShowDialog();
        //        //Window.GetWindow(this).Opacity = 1;
        //        #endregion
        //        //}
        //        //else
        //        //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }


        //}
        #endregion

    
    }
}
