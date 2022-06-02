using netoaster;
using laundryApp.View.windows;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
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

namespace laundryApp.View.sales.reservations
{
    /// <summary>
    /// Interaction logic for uc_reservationTable.xaml
    /// </summary>
    public partial class uc_reservationTable : UserControl
    {
        public uc_reservationTable()
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
        private static uc_reservationTable _instance;
        public static uc_reservationTable Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_reservationTable();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string createPermission = "reservationTable_create";
        List<Tables> selectedTables = new List<Tables>();
        TablesReservation TablesReservation = new TablesReservation();
        int _PersonsCount = 0;
        //byte tgl_userState;
        #region for search
        int personCount = 0;
        int sectionId = 0;
        #endregion
        public static List<string> requiredControlList;
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
      

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "reservationDate", "reservationStartTime" , "reservationEndTime" , "personsCount" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_tables", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillCustomerCombo", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillSectionCombo", value = false });

                loading_tables();
                loading_fillCustomerCombo();
                loading_fillSectionCombo();
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
                        await Task.Delay(0500);
                    }
                }
                while (!isDone);
                #endregion
            
               await Search();
               
               dg_tables.ItemsSource = selectedTables;

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

            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            //txt_title.Text = AppSettings.resourcemanager.GetString("trReservations");
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            txt_tables.Text = AppSettings.resourcemanager.GetString("trTables");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_customerId, AppSettings.resourcemanager.GetString("trCustomerHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_reservationDate, AppSettings.resourcemanager.GetString("trReservationDaterHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tp_reservationStartTime, AppSettings.resourcemanager.GetString("trStartTimeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tp_reservationEndTime, AppSettings.resourcemanager.GetString("trEndTimeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_personsCount, AppSettings.resourcemanager.GetString("trPersonsCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_searchPersonsCount, AppSettings.resourcemanager.GetString("trPersonsCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_searchSection, AppSettings.resourcemanager.GetString("trSectionHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_searchDate, AppSettings.resourcemanager.GetString("trDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tp_searchStartTime, AppSettings.resourcemanager.GetString("trStartTimeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tp_searchEndTime, AppSettings.resourcemanager.GetString("trEndTimeHint"));


            txt_statusEmpty.Text = AppSettings.resourcemanager.GetString("trEmpty");
            txt_statusOpen.Text = AppSettings.resourcemanager.GetString("trOpened");
            txt_statusReservated.Text = AppSettings.resourcemanager.GetString("trReserved");


            btn_refresh.ToolTip = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            btn_addCustomer.ToolTip = AppSettings.resourcemanager.GetString("trAddCustomer");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

        }
        #region loading
        List<keyValueBool> loadingList;
        async Task loading_fillCustomerCombo()
        {
            try
            {
                await FillCombo.FillComboCustomers(cb_customerId);
            }
            catch { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillCustomerCombo"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task loading_fillSectionCombo()
        {
            try
            {
                await FillCombo.FillComboHallSectionsWithDefault(cb_searchSection);
            }
            catch { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillSectionCombo"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task loading_tables()
        {
            //try
            {
                await refreshTablesList();
            }
            //catch { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_tables"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        #endregion
        #region Add - Update - Delete - Tgl - Clear - DG_SelectionChanged - refresh - validate
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        { //add
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(createPermission, FillCombo.groupObjects, "one"))
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        bool valid1 = await validateReservationTime();
                        bool valid2 = validatePersonsCount();
                        bool valid3 = validateReservationStartTime();
                        if (valid1 && valid2 && valid3)
                        {
                            TablesReservation reserve = new TablesReservation();
                            reserve.branchId = MainWindow.branchLogin.branchId;
                            reserve.code = await reserve.generateReserveCode("tr",MainWindow.branchLogin.code,MainWindow.branchLogin.branchId);
                            if (cb_customerId.SelectedIndex > 0)
                                reserve.customerId = (int)cb_customerId.SelectedValue;
                            reserve.reservationDate = dp_reservationDate.SelectedDate;

                            #region reservation time period                      
                            DateTime startTime = DateTime.Parse(dp_reservationDate.SelectedDate.ToString().Split(' ')[0]
                                    + ' ' + tp_reservationStartTime.SelectedTime.ToString().Split(' ')[1]
                                    + ' ' + tp_reservationStartTime.SelectedTime.ToString().Split(' ')[2]);

                            var dateOfStartTime = DateTime.Parse(tp_reservationStartTime.SelectedTime.ToString().Split(' ')[0]);
                            var dateOfEndTime = DateTime.Parse(tp_reservationEndTime.SelectedTime.ToString().Split(' ')[0]);
                            var difference = (dateOfEndTime - dateOfStartTime).Days;
                            DateTime reservationEndDate = DateTime.Parse(dp_reservationDate.SelectedDate.ToString().Split(' ')[0]) ;
                            if (difference > 0)
                            {
                                reservationEndDate = DateTime.Parse(dp_reservationDate.SelectedDate.ToString().Split(' ')[0]).AddDays(1);
                            }
                            DateTime endTime = DateTime.Parse(reservationEndDate.ToString().Split(' ')[0]
                                                + ' ' + tp_reservationEndTime.SelectedTime.ToString().Split(' ')[1]
                                                + ' ' + tp_reservationEndTime.SelectedTime.ToString().Split(' ')[2]);

                            reserve.reservationTime = startTime;
                            reserve.endTime = endTime;
                            #endregion
                            reserve.personsCount = int.Parse(tb_personsCount.Text);
                            reserve.notes = tb_notes.Text;
                            reserve.createUserId = MainWindow.userLogin.userId;
                            reserve.isActive = 1;
                            //save
                            int res = await reserve.addReservation(reserve, selectedTables);
                            if(res > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                Clear();
                                await refreshTablesList();
                                await Search();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

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
        bool validatePersonsCount()
        {
            bool valid = true;
            if(int.Parse(tb_personsCount.Text) > _PersonsCount)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message:AppSettings.resourcemanager.GetString("trCountMoreTableCapacity"), animation: ToasterAnimation.FadeIn);
                HelpClass.SetValidate(p_error_personsCount, AppSettings.resourcemanager.GetString("trCountMoreTableCapacity"));
                valid = false;
            }
            return valid;
        }
        async Task<Boolean> validateReservationTime()
        {
            bool valid = true;
            string message = "";
            DateTime startTime = DateTime.Parse(dp_reservationDate.SelectedDate.ToString().Split(' ')[0]
                                +' ' + tp_reservationStartTime.SelectedTime.ToString().Split(' ')[1]
                                + ' ' + tp_reservationStartTime.SelectedTime.ToString().Split(' ')[2]);

            DateTime endTime = DateTime.Parse(dp_reservationDate.SelectedDate.ToString().Split(' ')[0]
                                + ' ' + tp_reservationEndTime.SelectedTime.ToString().Split(' ')[1]
                                + ' ' + tp_reservationEndTime.SelectedTime.ToString().Split(' ')[2]);
            foreach (Tables tb in selectedTables)
            {
                int notReserved = await FillCombo.table.checkTableAvailabiltiy(tb.tableId,MainWindow.branchLogin.branchId,
                                                         dp_reservationDate.SelectedDate.ToString(),
                                                         startTime.ToString(),
                                                         endTime.ToString());

                if(notReserved == 0)
                {
                    valid = false;
                    if (message == "")
                        message += tb.name;
                    else
                        message += ", " + tb.name;
                }
                message += " ";
                if(!valid)
                    Toaster.ShowWarning(Window.GetWindow(this), message:message + AppSettings.resourcemanager.GetString("trReserved"), animation: ToasterAnimation.FadeIn);
            }
            return valid;
        }
        Boolean validateReservationStartTime()
        {
            bool valid = true;
            DateTime startTime = DateTime.Parse(dp_reservationDate.SelectedDate.ToString().Split(' ')[0]
                                + ' ' + tp_reservationStartTime.SelectedTime.ToString().Split(' ')[1]
                                + ' ' + tp_reservationStartTime.SelectedTime.ToString().Split(' ')[2]);
            if(dp_reservationDate.SelectedDate == DateTime.Now.Date && startTime.TimeOfDay < DateTime.Now.TimeOfDay)
            {
                valid = false;
                HelpClass.SetValidate(p_error_reservationStartTime, AppSettings.resourcemanager.GetString("wrongTime"));
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("wrongTime"), animation: ToasterAnimation.FadeIn);
            }



            return valid;
        }
        #endregion
        #region events
        private async void Cb_searchSection_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                tp_reservationStartTime.SelectedTime = null;
                tp_reservationStartTime.Text = "";
                tp_reservationEndTime.SelectedTime = null;
                tp_reservationEndTime.Text = "";
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
                await refreshTablesList();
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dp_searchDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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
        private async void Tp_searchTime_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
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
        private async void Btn_addCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;
                wd_updateVendor w = new wd_updateVendor();
                //// pass agent id to update windows
                w.agent.agentId = 0;
                w.type = "c";
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                if (w.isOk == true)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    await FillCombo.RefreshCustomers();
                    await FillCombo.FillComboCustomers(cb_customerId);
                }

                HelpClass.EndAwait(grid_main);
                
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_updateCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cb_customerId.SelectedIndex != -1)
                {
                    HelpClass.StartAwait(grid_main);
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_updateVendor w = new wd_updateVendor();
                    //// pass agent id to update windows
                    w.agent.agentId = (int)cb_customerId.SelectedValue;
                    w.type = "c";
                    w.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    if (w.isOk == true)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        await FillCombo.RefreshCustomers();
                        //await FillCombo.FillComboCustomers(cb_customerId);
                    }

                    HelpClass.EndAwait(grid_main);

                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void Tp_reservationStartTime_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            try
            {
                if (tp_reservationStartTime.SelectedTime != null)
                {
                    TimeSpan startTime =TimeSpan.Parse(tp_reservationStartTime.SelectedTime.ToString().Split(' ')[1]);
                    TimeSpan timeStaying = TimeSpan.FromHours(AppSettings.time_staying);

                    tp_reservationEndTime.SelectedTime = tp_reservationStartTime.SelectedTime.Value.Add(timeStaying);
                }
            }
            catch { }
        }
        #endregion
        #region Refresh & Search

        async Task Search()
        {
            //search
            if (tablesList is null 
                || dp_searchDate.SelectedDate != null 
                || tp_searchStartTime.SelectedTime != null 
                || tp_searchStartTime.SelectedTime != null)
                await refreshTablesList();

            tablesQuery = tablesList;
            try
            {
                personCount = int.Parse(tb_searchPersonsCount.Text);
                tablesQuery = tablesQuery.Where(s => s.personsCount == personCount).ToList();
            }
            catch { }

            if (cb_searchSection.SelectedIndex > 0)
            {
                sectionId = (int)cb_searchSection.SelectedValue;
                tablesQuery = tablesQuery.Where(s => s.sectionId == sectionId).ToList();
            }
            else
                sectionId = 0;

            BuildTablesDesign();
        }
        async Task refreshTablesList()
        {
            tablesList =  await FillCombo.table.GetTablesStatusInfo(MainWindow.branchLogin.branchId,dp_searchDate.SelectedDate.ToString(), tp_searchStartTime.SelectedTime.ToString(),tp_searchEndTime.SelectedTime.ToString());
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            selectedTables.Clear();
            _PersonsCount = 0;
            dg_tables.ItemsSource = null;
            dg_tables.ItemsSource = selectedTables;
            this.DataContext = TablesReservation ;


            // last 
            HelpClass.clearValidate(requiredControlList, this);
           

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

        #endregion
        #region report
        /*
        // report
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Sale\Ar\PackageReport.rdlc";
            }
            else
                addpath = @"\Reports\Sale\En\PackageReport.rdlc";
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            clsReports.packageReport(itemsQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        public void pdfpackage()
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
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {

               
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || HelpClass.isAdminPermision())
                {
                    /////////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        pdfpackage();
                    });
                    t1.Start();
                    //////////////////////////////////////
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
        public void printpackage()
        {
            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, MainWindow.rep_printer_name, short.Parse(MainWindow.rep_print_count));
            });
        }
        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || HelpClass.isAdminPermision())
                {
                    /////////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        printpackage();
                    });
                    t1.Start();
                    //////////////////////////////////////

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
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || HelpClass.isAdminPermision())
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvcCatalog win = new win_lvcCatalog(itemsQuery, 3);
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
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
        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || HelpClass.isAdminPermision())
                {
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
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();
                    }
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
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
        public void ExcelPackage()
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
        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || HelpClass.isAdminPermision())
                {
                    Thread t1 = new Thread(() =>
                    {
                        ExcelPackage();

                    });
                    t1.Start();
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
        */
        #endregion
        #region Button In DataGrid

        void cancelRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        Tables row = (Tables)dg_tables.SelectedItems[0];
                        selectedTables.Remove(row);
                        _PersonsCount -= row.personsCount;

                        dg_tables.ItemsSource = null;
                        dg_tables.ItemsSource = selectedTables;
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
        private void Cb_customerId_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                cb_customerId.ItemsSource = FillCombo.customersList.Where(x => x.name.Contains(cb_customerId.Text));

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
       

        private void Btn_clearTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                /*
                _Discount = 0;
                selectedCoupons.Clear();
                lst_coupons.Items.Clear();
                cb_coupon.SelectedIndex = -1;
                cb_coupon.SelectedItem = "";
                refreshTotalValue();
                */
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region table
        List<Tables> tablesList = new List<Tables>();
        List<Tables> tablesQuery = new List<Tables>();
        void BuildTablesDesign()
        {
            wp_tablesContainer.Children.Clear();
            foreach (var item in tablesQuery)
            {
                #region button
                Button tableButton = new Button();
                tableButton.Tag = item.name;
                tableButton.Margin = new Thickness(5);
                tableButton.Padding = new Thickness(0);
                tableButton.Background = null;
                tableButton.BorderBrush = null;

                if (item.personsCount <= 2)
                {
                    tableButton.Height = 90;
                    tableButton.Width = 80;
                }
                else if (item.personsCount == 3)
                {
                    tableButton.Height = 130;
                    tableButton.Width = 90;
                }
                else if (item.personsCount == 4)
                {
                    tableButton.Height = 140;
                    tableButton.Width = 100;
                }
                else if (item.personsCount == 5)
                {
                    tableButton.Height = 150;
                    tableButton.Width = 110;
                }
                else if (item.personsCount == 6)
                {
                    tableButton.Height = 160;
                    tableButton.Width = 120;
                }
                else if (item.personsCount == 7)
                {
                    tableButton.Height = 170;
                    tableButton.Width = 130;
                }
                else if (item.personsCount == 8)
                {
                    tableButton.Height = 180;
                    tableButton.Width = 140;
                }
                else if (item.personsCount == 9)
                {
                    tableButton.Height = 190;
                    tableButton.Width = 150;
                }
                else if (item.personsCount > 9)
                {
                    tableButton.Height = 200;
                    tableButton.Width = 160;
                }

                tableButton.Click += tableButton_Click;

                #region Grid Container
                Grid gridContainer = new Grid();
                int rowCount = 3;
                RowDefinition[] rd = new RowDefinition[rowCount];
                for (int i = 0; i < rowCount; i++)
                {
                    rd[i] = new RowDefinition();
                }
                rd[0].Height = new GridLength(1, GridUnitType.Star);
                rd[1].Height = new GridLength(20, GridUnitType.Pixel);
                rd[2].Height = new GridLength(20, GridUnitType.Pixel);
                for (int i = 0; i < rowCount; i++)
                {
                    gridContainer.RowDefinitions.Add(rd[i]);
                }
                /////////////////////////////////////////////////////
                #region Path table
                Path pathTable = new Path();
                pathTable.Stretch = Stretch.Fill;
                pathTable.FlowDirection = FlowDirection.LeftToRight;
                pathTable.Margin = new Thickness(5);

                if (item.status == "opened" || item.status == "openedReserved")
                    pathTable.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
                else if (item.status == "reserved")
                    pathTable.Fill = Application.Current.Resources["BlueTables"] as SolidColorBrush;
                else
                    pathTable.Fill = Application.Current.Resources["GreenTables"] as SolidColorBrush;

                if (item.personsCount <= 2)
                    pathTable.Data = App.Current.Resources["tablePersons2"] as Geometry;
                else if (item.personsCount == 3)
                    pathTable.Data = App.Current.Resources["tablePersons3"] as Geometry;
                else if (item.personsCount == 4)
                    pathTable.Data = App.Current.Resources["tablePersons4"] as Geometry;
                else if (item.personsCount == 5)
                    pathTable.Data = App.Current.Resources["tablePersons5"] as Geometry;
                else if (item.personsCount == 6)
                    pathTable.Data = App.Current.Resources["tablePersons6"] as Geometry;
                else if (item.personsCount == 7)
                    pathTable.Data = App.Current.Resources["tablePersons7"] as Geometry;
                else if (item.personsCount == 8)
                    pathTable.Data = App.Current.Resources["tablePersons8"] as Geometry;
                else if (item.personsCount == 9)
                    pathTable.Data = App.Current.Resources["tablePersons9"] as Geometry;
                else if (item.personsCount > 9)
                    pathTable.Data = App.Current.Resources["tablePersons9Plus"] as Geometry;

                gridContainer.Children.Add(pathTable);
                #endregion
                #region   personCount 
                if (item.personsCount > 9)
                {
                    var itemPersonCountText = new TextBlock();
                    itemPersonCountText.Text = item.personsCount.ToString();
                    itemPersonCountText.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    itemPersonCountText.FontSize = 32;
                    itemPersonCountText.VerticalAlignment = VerticalAlignment.Center;
                    itemPersonCountText.HorizontalAlignment = HorizontalAlignment.Center;
                    gridContainer.Children.Add(itemPersonCountText);
                }
                #endregion
                #region   name
                var itemNameText = new TextBlock();
                itemNameText.Text = item.name;
                itemNameText.VerticalAlignment = VerticalAlignment.Center;
                itemNameText.HorizontalAlignment = HorizontalAlignment.Center;
                itemNameText.Foreground = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                Grid.SetRow(itemNameText, 1);
                gridContainer.Children.Add(itemNameText);
                #endregion
                #region   status
                var itemStatusText = new TextBlock();
                
                if (item.status == "opened" || item.status == "openedReserved")
                    itemStatusText.Text = AppSettings.resourcemanager.GetString("trOpened"); 
                else if (item.status == "reserved")
                    itemStatusText.Text = AppSettings.resourcemanager.GetString("trReserved");
                else
                    itemStatusText.Text = AppSettings.resourcemanager.GetString("trEmpty");

                //itemStatusText.Text = item.status;
                itemStatusText.VerticalAlignment = VerticalAlignment.Center;
                itemStatusText.HorizontalAlignment = HorizontalAlignment.Center;
                itemStatusText.Foreground = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                Grid.SetRow(itemStatusText, 2);
                gridContainer.Children.Add(itemStatusText);
                #endregion
                tableButton.Content = gridContainer;

                #endregion
                wp_tablesContainer.Children.Add(tableButton);
                #endregion
            }
        }
        void tableButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string tableName = button.Tag.ToString();
            var table = tablesList.Where(x => x.name == tableName).FirstOrDefault();
            if (!selectedTables.Contains(table))
            {
                selectedTables.Add(table);
                _PersonsCount += table.personsCount;
                dg_tables.ItemsSource = null;
                dg_tables.ItemsSource = selectedTables;
            }
        }

        #endregion

       
    }
}
