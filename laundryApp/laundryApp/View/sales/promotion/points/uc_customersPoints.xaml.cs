using laundryApp.Classes;
using laundryApp.View.windows;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public static List<string> requiredControlList;
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
            txt_count.Text = customersQuery.Count().ToString();
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

            btn_clearPoints.Content = AppSettings.resourcemanager.GetString("trClearPoints");
            btn_clearHistory.Content = AppSettings.resourcemanager.GetString("trClearHistory");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_increasePoints, AppSettings.resourcemanager.GetString("trIncreasePoints")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_decreasePoints, AppSettings.resourcemanager.GetString("trDecreasePoints")+"...");

            dg_customer.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            dg_customer.Columns[1].Header = AppSettings.resourcemanager.GetString("trPoints");
            dg_customer.Columns[2].Header = AppSettings.resourcemanager.GetString("trPointsHistory");

            btn_increasePoints.ToolTip = AppSettings.resourcemanager.GetString("trSave");
            btn_decreasePoints.ToolTip = AppSettings.resourcemanager.GetString("trSave");

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
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
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
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only  digits
            try
            {
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
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

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
        int points = 0;
        private void Dg_customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
            {
                //HelpClass.StartAwait(grid_main);

                customer = dg_customer.SelectedItem as Agent;
                this.DataContext = customer;
                points = customer.points;
                //HelpClass.EndAwait(grid_main);
            }
            //else
            //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}
        }

        #endregion

        #region update

        private async void Btn_clearPoints_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    //HelpClass.StartAwait(grid_main);

                    int result = await customer.resetAllAgentsPoints(MainWindow.userLogin.userId, MainWindow.posLogin.posId);
                    if (result <= 0)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    else
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                        tb_increasePoints.Clear();
                        await RefreshCustomersList();
                        await Search();
                    }
                    
                    //HelpClass.EndAwait(grid_main);
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}

        }

        private async void Btn_clearHistory_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    //HelpClass.StartAwait(grid_main);

                    int result = await customer.resetAllPointsAndHistory(MainWindow.userLogin.userId, MainWindow.posLogin.posId);
                    if (result <= 0)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    else
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                        tb_increasePoints.Clear();
                        await RefreshCustomersList();
                        await Search();
                    }

                    //HelpClass.EndAwait(grid_main);
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}

        }

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {
            // refresh list
            await Search();
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
                            if (res <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                                tb_increasePoints.Clear();
                                await RefreshCustomersList();
                                await Search();
                            }
                        }
                        // rowIndex has the row index
                        // bindingPath has the column's binding
                        // el.Text has the new, user-entered value
                    }
                }
            }
        }

        private async void Btn_increasePoints_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    //HelpClass.StartAwait(grid_main);

                    requiredControlList = new List<string>() { "increasePoints" };

                    if (HelpClass.validate(requiredControlList, this))
                    {

                        int result = await customer.UpdateAllAgentsPoints(MainWindow.userLogin.userId, MainWindow.posLogin.posId, int.Parse(tb_increasePoints.Text));
                        if (result <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                                tb_increasePoints.Clear();
                                await RefreshCustomersList();
                                await Search();
                            }
                    }
                    //HelpClass.EndAwait(grid_main);
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}
            
        }

        private async void Btn_decreasePoints_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    //HelpClass.StartAwait(grid_main);

                    requiredControlList = new List<string>() { "decreasePoints" };

                    if (HelpClass.validate(requiredControlList, this))
                    {

                        int result = await customer.UpdateAllAgentsPoints(MainWindow.userLogin.userId, MainWindow.posLogin.posId, -1 * int.Parse(tb_decreasePoints.Text));
                        if (result <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                            tb_decreasePoints.Clear();
                            await RefreshCustomersList();
                            await Search();
                        }
                    }
                   // HelpClass.EndAwait(grid_main);
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            //}
            //catch (Exception ex)
            //{
            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this);
            //}
            
        }

        #endregion

        #region reports

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
                addpath = @"\Reports\SectionData\banksData\Ar\ArBank.rdlc";
            }
            else
            {
                addpath = @"\Reports\SectionData\banksData\En\EnBank.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            //clsReports.BanksReport(servicesQuery, rep, reppath, paramarr);
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
                {
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
                    #endregion
                }
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
        {//print
            try
            {
                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                    #endregion
                }
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
                {
                    #region
                    //Window.GetWindow(this).Opacity = 0.2;
                    //win_IvcAccount win = new win_IvcAccount(servicesQuery, 4);
                    //win.ShowDialog();
                    //Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
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
        {//excel
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
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
                }
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
        {//preview
            try
            {
                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
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
                }
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

    }
}
