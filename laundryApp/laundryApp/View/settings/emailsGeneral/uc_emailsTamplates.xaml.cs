using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
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
using System.Resources;
using System.Reflection;

namespace laundryApp.View.settings.emailsGeneral
{
    /// <summary>
    /// Interaction logic for uc_emailsTamplates.xaml
    /// </summary>
    public partial class uc_emailsTamplates : UserControl
    {
        public uc_emailsTamplates()
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
        private static uc_emailsTamplates _instance;
        public static uc_emailsTamplates Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_emailsTamplates();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string savePermission = "emailsTamplates_save";
        SetValues setValuesModel = new SetValues();
        SetValues setValues = new SetValues();
        IEnumerable<SetValues> setValuessQuery;
        IEnumerable<SetValues> setValuess;

        SettingCls setModel = new SettingCls();
        SettingCls sett = new SettingCls();
        IEnumerable<SettingCls> setQuery;
        IEnumerable<SettingCls> setQueryView;

      

        string searchText = "";
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
                requiredControlList = new List<string> { "name", };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                RefreshSettingsList();

               // RefreshSetttingsView();
                Keyboard.Focus(tb_title);
                 
                //await Search();
                //Clear();
                 
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
            txt_title.Text = AppSettings.resourcemanager.GetString("trEmailTemplates");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_title, AppSettings.resourcemanager.GetString("trTitle") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_text1, AppSettings.resourcemanager.GetString("trText1") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_text2, AppSettings.resourcemanager.GetString("trText2") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_link1text, AppSettings.resourcemanager.GetString("trLinkText1") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_link1url, AppSettings.resourcemanager.GetString("trUrlLink1") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_link2text, AppSettings.resourcemanager.GetString("trLinkText2") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_link2url, AppSettings.resourcemanager.GetString("trUrlLink2") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_link3text, AppSettings.resourcemanager.GetString("trLinkText3") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_link3url, AppSettings.resourcemanager.GetString("trUrlLink3") + "...");

            btn_refresh.ToolTip = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");


            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            txt_infoTitle.Text = AppSettings.resourcemanager.GetString("trTitle");
            txt_infoBody.Text = AppSettings.resourcemanager.GetString("trBody");
            txt_infoEmailSupport.Text = AppSettings.resourcemanager.GetString("trEmailSupport");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            dg_setValues.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
        
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(savePermission, FillCombo.groupObjects, "one") )
                {
                    HelpClass.StartAwait(grid_main);
                    if (HelpClass.validate(requiredControlList, this) )
                    {

                        //write here Mr.Naji

                        /////
                        int msg = 0;
                        setValues = setValuessQuery.Where(x => x.notes == "title").FirstOrDefault();
                        setValues.value = tb_title.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        //

                        setValues = setValuessQuery.Where(x => x.notes == "text1").FirstOrDefault();
                        setValues.value = tb_text1.Text;

                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        setValues = setValuessQuery.Where(x => x.notes == "text2").FirstOrDefault();
                        setValues.value = tb_text2.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        setValues = setValuessQuery.Where(x => x.notes == "link1text").FirstOrDefault();
                        setValues.value = tb_link1text.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        setValues = setValuessQuery.Where(x => x.notes == "link2text").FirstOrDefault();
                        setValues.value = tb_link2text.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        setValues = setValuessQuery.Where(x => x.notes == "link3text").FirstOrDefault();
                        setValues.value = tb_link3text.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        setValues = setValuessQuery.Where(x => x.notes == "link1url").FirstOrDefault();
                        setValues.value = tb_link1url.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        setValues = setValuessQuery.Where(x => x.notes == "link2url").FirstOrDefault();
                        setValues.value = tb_link2url.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                        setValues = setValuessQuery.Where(x => x.notes == "link3url").FirstOrDefault();
                        setValues.value = tb_link3url.Text;
                        msg += await setValuesModel.SaveValueByNotes(setValues);
                    }
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
        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                /*
                await Search();
                */
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
                /*
                 
                Clear();
                */
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dg_setValues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
          
                //selection
                if (dg_setValues.SelectedIndex != -1)
                {
                    if (dg_setValues.SelectedIndex != -1)
                    {
                        sett = dg_setValues.SelectedItem as SettingCls;

                        setValuessQuery = await setValuesModel.GetBySetName(sett.name);

                        //List<SettingCls> settLst = await setModel.GetAll();
                        //SettingCls setting = settLst.Where(s => s.settingId == sett.settingId).FirstOrDefault();

                        //setValuessQuery = await setValuesModel.GetBySetName(setting.name);
                        tb_title.Text = setValuessQuery.Where(x => x.notes == "title").FirstOrDefault() is null ? ""
                        : setValuessQuery.Where(x => x.notes == "title").FirstOrDefault().value.ToString();
                        tb_text1.Text = setValuessQuery.Where(x => x.notes == "text1").FirstOrDefault() is null ? ""
                           : setValuessQuery.Where(x => x.notes == "text1").FirstOrDefault().value.ToString();
                        tb_text2.Text = setValuessQuery.Where(x => x.notes == "text2").FirstOrDefault() is null ? ""
                        : setValuessQuery.Where(x => x.notes == "text2").FirstOrDefault().value.ToString();
                        tb_link1text.Text = setValuessQuery.Where(x => x.notes == "link1text").FirstOrDefault() is null ? ""
                        : setValuessQuery.Where(x => x.notes == "link1text").FirstOrDefault().value.ToString();

                        tb_link2text.Text = setValuessQuery.Where(x => x.notes == "link2text").FirstOrDefault() is null ? ""
                         : setValuessQuery.Where(x => x.notes == "link2text").FirstOrDefault().value.ToString();
                        tb_link3text.Text = setValuessQuery.Where(x => x.notes == "link3text").FirstOrDefault() is null ? ""
                        : setValuessQuery.Where(x => x.notes == "link3text").FirstOrDefault().value.ToString();


                        tb_link1url.Text = setValuessQuery.Where(x => x.notes == "link1url").FirstOrDefault() is null ? ""
                             : setValuessQuery.Where(x => x.notes == "link1url").FirstOrDefault().value.ToString();
                        tb_link2url.Text = setValuessQuery.Where(x => x.notes == "link2url").FirstOrDefault() is null ? ""
                               : setValuessQuery.Where(x => x.notes == "link2url").FirstOrDefault().value.ToString();

                        tb_link3url.Text = setValuessQuery.Where(x => x.notes == "link3url").FirstOrDefault() is null ? ""
                               : setValuessQuery.Where(x => x.notes == "link3url").FirstOrDefault().value.ToString();

                        this.DataContext = setValues;
                    }

                }

                if (setValues != null)
                {
                    //btn_addRange.IsEnabled = true;


                }
               
                HelpClass.clearValidate(requiredControlList, this);
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
            /*
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshTablesList();
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
            */
        }
        #endregion

        #region Refresh & Search
          async void RefreshSettingsList()
        {
            setQuery = await setModel.GetByNotes("emailtemp");
            foreach ( SettingCls row in setQuery )
            {
                switch (row.name)
                {
                    case "pur_email_temp":
                        row.trName = AppSettings.resourcemanager.GetString("trPurchasesEmailTemplate");
                        break;
                    case "pur_order_email_temp":
                        row.trName = AppSettings.resourcemanager.GetString("trPurchaseOrdersEmailTemplate");
                        break;
                    case "sale_email_temp":
                        row.trName = AppSettings.resourcemanager.GetString("trSalesEmailTemplate");
                        break;
                    case "sale_order_email_temp":
                        row.trName = AppSettings.resourcemanager.GetString("trSalesOrdersEmailTemplate");
                        break;
                    case "quotation_email_temp":
                        row.trName = AppSettings.resourcemanager.GetString("trQuotationsEmailTemplate");
                        break;
                    case "required_email_temp":
                        row.trName = AppSettings.resourcemanager.GetString("trRequirementsEmailTemplate");
                        break;

                }
                }
         
            dg_setValues.ItemsSource = setQuery;
         
        }
        //void RefreshSetttingsView()
        //{
        //    setQuery.ToList()[0].name = AppSettings.resourcemanager.GetString("trPurchaseOrdersEmailTemplate");
        //    setQuery.ToList()[1].name = AppSettings.resourcemanager.GetString("trSalesEmailTemplate");
        //    setQuery.ToList()[2].name = AppSettings.resourcemanager.GetString("trSalesOrdersEmailTemplate");
        //    setQuery.ToList()[3].name = AppSettings.resourcemanager.GetString("trQuotationsEmailTemplate");
        //    setQuery.ToList()[4].name = AppSettings.resourcemanager.GetString("trRequirementsEmailTemplate");
        //    setQuery.ToList()[5].name = AppSettings.resourcemanager.GetString("trPurchasesEmailTemplate");
        //    dg_setValues.ItemsSource = setQuery;
        //}

        //async Task Search()
        //{
        //    //search
        //    if (tables is null)
        //        await RefreshTablesList();
        //    searchText = tb_search.Text.ToLower();
        //    tablesQuery = tables.Where(s => (
        //    s.name.ToLower().Contains(searchText)
        //    ) && s.isActive == tgl_tableState);
        //    RefreshTablessView();
        //}
        //async Task<IEnumerable<Tables>> RefreshTablesList()
        //{
        //    tables = await table.Get(MainWindow.branchLogin.branchId);
        //    // tables = tables.Where(x => x.branchId == MainWindow.branchLogin.branchId );
        //    return tables;
        //}
        //void RefreshTablessView()
        //{
        //    dg_table.ItemsSource = tablesQuery;
        //    txt_count.Text = tablesQuery.Count().ToString();
        //}

        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 

        /*
    void Clear()
    {
        this.DataContext = new Tables();
        dg_table.SelectedIndex = -1;
        // last 
        HelpClass.clearValidate(requiredControlList, this);

    }
    */
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
                addpath = @"\Reports\SectionData\hallDivide\Ar\ArTables.rdlc";
            }
            else
            {
                addpath = @"\Reports\SectionData\hallDivide\En\EnTables.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            clsReports.tablesReport(tablesQuery.ToList(), rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            //pdf
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
                {
                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, FillCombo.rep_printer_name, FillCombo.rep_print_count == null ? short.Parse("1") : short.Parse(FillCombo.rep_print_count));
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

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
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
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
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



    }
}
