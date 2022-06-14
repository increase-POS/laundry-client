using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
//using System.IO;
using laundryApp.View.windows;
using Microsoft.Reporting.WinForms;
using laundryApp.View.sales;
using System.Text.RegularExpressions;
using laundryApp.Classes.ApiClasses;
using laundryApp.View.accounts;

namespace laundryApp.View.catalog.salesItems
{
    /// <summary>
    /// Interaction logic for uc_servicePricing.xaml
    /// </summary>
    public partial class uc_servicePricing : UserControl
    {

        public uc_servicePricing()
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
        private static uc_servicePricing _instance;
        public static uc_servicePricing Instance
        {
            get
            {
                //if (_instance == null)
                if (_instance is null)
                    _instance = new uc_servicePricing();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string updatePermission = "itemsCosting_update";
        string searchText = "";
        int categoryId = 0;
        public static string categoryName;



        IEnumerable<Service> servicesLst;
        Service service = new Service();

        IEnumerable<ItemsUnitsServices> iuServices;
        IEnumerable<ItemsUnitsServices> iuServicesQuery;
        ItemsUnitsServices itemsUnitsServices = new ItemsUnitsServices();

        public static List<string> requiredControlList;

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    HelpClass.StartAwait(grid_main);

                categoryId = FillCombo.GetCategoryId(categoryName);

                await RefreshServicesList();

                if (categoryName.Equals("carpets"))
                {
                    rowToHide.Height = new GridLength(0);
                    service = servicesLst.FirstOrDefault();

                    await RefreshItemsList();
                    await Search();

                    refreshTextBoxs();
                }
                else
                {
                    rowToHide.Height = new GridLength(2, GridUnitType.Star);
                }

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

                

            //    HelpClass.EndAwait(grid_main);
            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}
        }

        #region events
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
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
      
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Dg_items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select item
            try
            {
                HelpClass.StartAwait(grid_main);

                if (dg_items.SelectedIndex != -1)
                {
                    itemsUnitsServices = dg_items.SelectedItem as ItemsUnitsServices;
                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dg_service_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select service
            try
            {
                HelpClass.StartAwait(grid_main);
                if (dg_service.SelectedIndex != -1)
                {
                    service = dg_service.SelectedItem as Service;

                    await RefreshItemsList();
                    await Search();

                    refreshTextBoxs();
                   
                }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);

                await RefreshItemsList();
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only  digits
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
                else if (textBox.Tag.ToString() == "decimal")
                {
                    input = e.Text;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only english and digits
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                //HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        #region methods
        void refreshTextBoxs()
        {
            decimal cost = iuServicesQuery.First().cost;
            if (iuServicesQuery.Any(iu => iu.cost != cost))
                tb_cost.Text = "0";
            else
                tb_cost.Text = cost.ToString();

            decimal normal = iuServicesQuery.First().normalPrice;
            if (iuServicesQuery.Any(iu => iu.normalPrice != normal))
                tb_normalPrice.Text = "0";
            else
                tb_normalPrice.Text = normal.ToString();

            decimal instant = iuServicesQuery.First().instantPrice;
            if (iuServicesQuery.Any(iu => iu.instantPrice != instant))
                tb_instantPrice.Text = "0";
            else
                tb_instantPrice.Text = instant.ToString();
        }
        async Task<IEnumerable<Service>> RefreshServicesList()
        {
            servicesLst = await service.Get();
            servicesLst = servicesLst.Where(s => s.categoryId == categoryId && s.isActive == 1);
            dg_service.ItemsSource = servicesLst;
            return servicesLst;
        }
        private void translate()
        {
            // Title
            //if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
            //    txt_title.Text = AppSettings.resourcemanager.GetString(
            //   FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
            //   );
            txt_services.Text = AppSettings.resourcemanager.GetString("trServices");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");

            dg_items.Columns[0].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_items.Columns[1].Header = AppSettings.resourcemanager.GetString("trCost");
            dg_items.Columns[2].Header = AppSettings.resourcemanager.GetString("normalPrice");
            dg_items.Columns[3].Header = AppSettings.resourcemanager.GetString("instantPrice");

            dg_service.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_cost, AppSettings.resourcemanager.GetString("trCost")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_normalPrice, AppSettings.resourcemanager.GetString("normalPrice")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_instantPrice, AppSettings.resourcemanager.GetString("instantPrice") + "...");

            btn_updateServiceCost.ToolTip = AppSettings.resourcemanager.GetString("trSave");
            btn_updateServiceNormalPrice.ToolTip = AppSettings.resourcemanager.GetString("trSave");
            btn_updateServiceInstantPrice.ToolTip = AppSettings.resourcemanager.GetString("trSave");
        }
        async Task Search()
        {
            try
            {
                if (iuServices == null)
                    await RefreshItemsList();
            
                searchText = tb_search.Text.ToLower();

                iuServicesQuery = iuServices
                    .Where(s => s.itemName.ToLower().Contains(searchText)
                                                        || s.cost.ToString().ToLower().Contains(searchText)
                                                        || s.normalPrice.ToString().ToLower().Contains(searchText)
                                                        || s.instantPrice.ToString().ToLower().Contains(searchText))
                   .ToList();

                RefreshItemsView();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task<IEnumerable<ItemsUnitsServices>> RefreshItemsList()
        {
            iuServices = await itemsUnitsServices.GetIUServicesByServiceId(service.serviceId);
            return iuServices;
        }
        void RefreshItemsView()
        {
            dg_items.ItemsSource = iuServicesQuery;
            txt_count.Text = iuServicesQuery.Count().ToString();
        }
        private void Clear()
        {
            try
            {
                dg_service.SelectedIndex = -1;
                service = new Service();
                dg_items.ItemsSource = null;
                tb_cost.Text = "0";
                tb_normalPrice.Text = "0";
                tb_instantPrice.Text = "0";

            }
            catch { }
        }

        #endregion

        #region report

        //report  parameters
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        // end report parameters
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Kitchen\Ar\ArItemsCosting.rdlc";
            }
            else
            {
                addpath = @"\Reports\Kitchen\En\EnItemsCosting.rdlc";
            }
            string reppath = reportclass.PathUp(System.IO.Directory.GetCurrentDirectory(), 2, addpath);
            //clsReports.itemCosting(iuServicesQuery, rep, reppath, paramarr);
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

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
                #region
                BuildReport();

                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
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
                pdfpath = reportclass.PathUp(System.IO.Directory.GetCurrentDirectory(), 2, pdfpath);

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
        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
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
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                //{
                #region
                //Window.GetWindow(this).Opacity = 0.2;
                //win_IvcAccount win = new win_IvcAccount(itemsUnitsServices, 4);
                //win.ShowDialog();
                //Window.GetWindow(this).Opacity = 1;
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
        #endregion

        #region save
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update list
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "one"))
                {
                    int res = await itemsUnitsServices.UpdateIUServiceList(iuServicesQuery.ToList(), service.serviceId, MainWindow.userLogin.userId);
                    if (res > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                        await RefreshItemsList();
                        await Search();
                        refreshTextBoxs();
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
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

        private async void Btn_updateServiceCost_Click(object sender, RoutedEventArgs e)
        {//update cost
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (service.serviceId > 0)
                    {
                        requiredControlList = new List<string>() { "cost" };
                        if (HelpClass.validate(requiredControlList, this))
                        {
                            decimal cost = 0;
                            try { cost = decimal.Parse(tb_cost.Text); } catch { }
                            int result = await itemsUnitsServices.UpdateCostByServiceId(service.serviceId, MainWindow.userLogin.userId , cost);
                            if (result <= 0)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            }
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await RefreshItemsList();
                                await Search();
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }


        }

        private async void Btn_updateServiceNormalPrice_Click(object sender, RoutedEventArgs e)
        {//update normal
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (service.serviceId > 0)
                    {
                        requiredControlList = new List<string>() { "normalPrice" };
                        if (HelpClass.validate(requiredControlList, this))
                        {
                            decimal normal = 0;
                            try { normal = decimal.Parse(tb_normalPrice.Text); } catch { }
                            int result = await itemsUnitsServices.UpdateNormalByServiceId(service.serviceId, MainWindow.userLogin.userId, normal);
                            if (result <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await RefreshItemsList();
                                await Search();
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_updateServiceInstantPrice_Click(object sender, RoutedEventArgs e)
        {//update normal
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "update") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (service.serviceId > 0)
                    {
                        requiredControlList = new List<string>() { "instantPrice" };
                        if (HelpClass.validate(requiredControlList, this))
                        {
                            decimal instant = 0;
                            try { instant = decimal.Parse(tb_instantPrice.Text); } catch { }
                            int result = await itemsUnitsServices.UpdateInstantByServiceId(service.serviceId, MainWindow.userLogin.userId, instant);
                            if (result <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await RefreshItemsList();
                                await Search();
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

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
