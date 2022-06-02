using laundryApp.Classes;
using laundryApp.View.windows;
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
using netoaster;


namespace laundryApp.View.settings.reportsSettings
{
    /// <summary>
    /// Interaction logic for uc_reportsSettings.xaml
    /// </summary>
    public partial class uc_reportsSettings : UserControl
    {
        private static uc_reportsSettings _instance;
        public static uc_reportsSettings Instance
        {
            get
            {
                if(_instance is null)
                _instance = new uc_reportsSettings();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

      

        public uc_reportsSettings()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }
        
        public class Replang
        {
            public int langId { get; set; }
            public string lang { get; set; }
            public string trlang { get; set; }
            public Nullable<int> isDefault { get; set; }

        }
        //string usersSettingsPermission = "reportsSettings_usersSettings";
        string companySettingsPermission = "reportsSettings_companySettings";


        SetValues setvalueModel = new SetValues();
        List<SetValues> replangList = new List<SetValues>();
        SetValues replangrow = new SetValues();
        static SettingCls setModel = new SettingCls();
        static SettingCls set = new SettingCls();
        static SetValues valueModel = new SetValues();
        static SetValues printCount = new SetValues();
        static int printCountId = 0;
       
        List<SetValues> printList = new List<SetValues>();


        List<Replang> langcomboList = new List<Replang>();
        SetValues show_header_row = new SetValues();
        string show_header;
        SetValues itemtax_note_row = new SetValues();
        string itemtax_note;
        SetValues sales_invoice_note_row = new SetValues();
        string sales_invoice_note;
        private void fillPrintHeader()
        {
            cb_printHeader.DisplayMemberPath = "Text";
            cb_printHeader.SelectedValuePath = "Value";
            var typelist = new[] {
                 new { Text = AppSettings.resourcemanager.GetString("trShowOption")       , Value = "1" },
                 new { Text = AppSettings.resourcemanager.GetString("trHide")       , Value = "0" },
                };
            cb_printHeader.ItemsSource = typelist;
            cb_printHeader.SelectedIndex = 0;

            getshowHeader();
            if (show_header_row.value == "1")
            {
                cb_printHeader.SelectedIndex = 0;
            }
            else
            {
                cb_printHeader.SelectedIndex = 1;
            }
            
        }
        public string getshowHeader()
        {

            show_header_row = printList.Where(X => X.name == "show_header").FirstOrDefault();

            show_header = show_header_row.value;
            return show_header;

        }
        async Task fillRepLang()
        {
            langcomboList = new List<Replang>();
            replangList = await setvalueModel.GetBySetName("report_lang");
            foreach (var reprow in replangList)
            {
                //  trEnglish resourcemanager.GetString("trMenu");
                //trArabic
                Replang comborow = new Replang();
                comborow.langId = reprow.valId;
                comborow.lang = reprow.value;

                if (reprow.value == "ar")
                {
                    comborow.trlang = AppSettings.resourcemanager.GetString("trArabic");
                }
                else if (reprow.value == "en")
                {
                    comborow.trlang = AppSettings.resourcemanager.GetString("trEnglish");
                }
                else
                {
                    comborow.trlang = "";
                }

                langcomboList.Add(comborow);
            }
            cb_reportlang.ItemsSource = langcomboList;
            cb_reportlang.DisplayMemberPath = "trlang";
            cb_reportlang.SelectedValuePath = "langId";
            replangrow = replangList.Where(r => r.isDefault == 1).FirstOrDefault();
            cb_reportlang.SelectedValue = replangrow.valId;
        }

        public static async Task<SetValues> getDefaultPrintCount()
        {
            List<SettingCls> settingsCls = await setModel.GetAll();
            List<SetValues> settingsValues = await valueModel.GetAll();
            set = settingsCls.Where(s => s.name == "Allow_print_inv_count").FirstOrDefault<SettingCls>();
            printCountId = set.settingId;
            printCount = settingsValues.Where(i => i.settingId == printCountId).FirstOrDefault();
            return printCount;
        }
        public async Task FillprintList()
        {
            printList = await setvalueModel.GetBySetvalNote("print");
        }

        private async void   UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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

                await translate();
                #endregion

                ///naji code
                ///
                await FillprintList();
                fillPrintHeader();
                await fillRepLang();
                #region get default print count
                await getDefaultPrintCount();
                if (printCount != null)
                    tb_printCount.Text = printCount.value;
                #endregion
 

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        #region validate - clearValidate - textChange - lostFocus - . . . . 

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


        #endregion


        private async Task translate()
        {

            txt_mainTitle.Text = AppSettings.resourcemanager.GetString("trReportsSettings");

            txt_printCount.Text = AppSettings.resourcemanager.GetString("trPrintCount");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_printCount, AppSettings.resourcemanager.GetString("trPrintCount"));

            txt_reportlang.Text = AppSettings.resourcemanager.GetString("trLanguage");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_reportlang, AppSettings.resourcemanager.GetString("trLanguage"));

            txt_printHeader.Text = AppSettings.resourcemanager.GetString("trPrintHeader");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_printHeader, AppSettings.resourcemanager.GetString("trPrintHeader"));

            txt_systmSetting.Text = AppSettings.resourcemanager.GetString("trDirectPrinting");
            txt_systmSettingHint.Text = AppSettings.resourcemanager.GetString("trDirectPrintingHint") + "...";

            txt_kitchenPrinting.Text = AppSettings.resourcemanager.GetString("kitchenPrinting");
            txt_kitchenPrintingHint.Text = AppSettings.resourcemanager.GetString("kitchenPrintingSettings") + "...";

            txt_printerSetting.Text = AppSettings.resourcemanager.GetString("trPrinterSettings");
            txt_printerSettingHint.Text = AppSettings.resourcemanager.GetString("trPrinterSettingsHint") + "...";

            txt_copyCount.Text = AppSettings.resourcemanager.GetString("trCopyCount");
            txt_copyCountHint.Text = AppSettings.resourcemanager.GetString("trCopyCountHint") + "...";

            //txt_printCount.Text = AppSettings.resourcemanager.GetString("trPrintCount");
            //txt_printHeader.Text = AppSettings.resourcemanager.GetString("trPrintHeader");
            txt_itemsTaxNote.Text = AppSettings.resourcemanager.GetString("trItemsTax");
            txt_itemsTaxNoteHint.Text = AppSettings.resourcemanager.GetString("trItemsTaxNote");
            txt_salesInvoiceNote.Text = AppSettings.resourcemanager.GetString("trSalesInvoice");
            txt_salesInvoiceNoteHint.Text = AppSettings.resourcemanager.GetString("trSalesInvoiceNote");



            // openButton
            List<TextBlock> openTextBlocksList = FindControls.FindVisualChildren<TextBlock>(this)
               .Where(x => x.Tag != null).ToList();
            if (openTextBlocksList.Count == 0)
            {
                await Task.Delay(0050);
                await translate();
            }
            openTextBlocksList = openTextBlocksList.Where(x => x.Tag.ToString().Contains("openButton")).ToList();
            foreach (var item in openTextBlocksList)
            {
                item.Text = AppSettings.resourcemanager.GetString("open");
            }

            // saveButton
            List<TextBlock> saveTextBlocksList = FindControls.FindVisualChildren<TextBlock>(this)
               .Where(x => x.Tag != null).ToList();
            saveTextBlocksList = saveTextBlocksList.Where(x => x.Tag.ToString().Contains("saveButton")).ToList();
            foreach (var item in saveTextBlocksList)
            {
                item.Text = AppSettings.resourcemanager.GetString("trSave");
            }


        }


        private async void Btn_reportlang_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //  string msg = "";
                int msg = 0;
                if (cb_reportlang.SelectedIndex != -1)
                {
                    replangrow = replangList.Where(r => r.valId == (int)cb_reportlang.SelectedValue).FirstOrDefault();
                    replangrow.isDefault = 1;
                    msg = await setvalueModel.Save(replangrow);
                    //  replangrow.valId=
                    await MainWindow.GetReportlang();
                    await fillRepLang();
                    if (msg > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_systmSetting_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one") )
                //{
                Window.GetWindow(this).Opacity = 0.2;
                wd_reportSystmSetting w = new wd_reportSystmSetting();
                w.windowType = "r";
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
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
        private void Btn_kitchenPrinting_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one") )
                //{
                Window.GetWindow(this).Opacity = 0.2;
                wd_kitchenPrinting w = new wd_kitchenPrinting();
                //w.windowType = "r";
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
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
        private void Btn_printerSetting_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                
                    HelpClass.StartAwait(grid_main);


                //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one") )
                //{
                Window.GetWindow(this).Opacity = 0.2;
                wd_reportPrinterSetting w = new wd_reportPrinterSetting();
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
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

        private void Btn_copyCount_Click(object sender, RoutedEventArgs e)
        {
            
                        try
                        {

                HelpClass.StartAwait(grid_main);
                            //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one") )
                            //{
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_reportCopyCountSetting w = new wd_reportCopyCountSetting();
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
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

        private async void Btn_printCount_Click(object sender, RoutedEventArgs e)
        {
            
            //save print count
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                //  HelpClass.validateEmptyTextBox(tb_printCount, p_errorPrintCount, tt_errorPrintCount, "trEmptyPrintCount");
                HelpClass.clearValidate(p_error_printCount);
                if (tb_printCount.Text.Equals(""))
                {
                    HelpClass.SetValidate( p_error_printCount,"trEmptyPrintCount");
                }
              else  if (int.Parse(tb_printCount.Text)<=0 )
                {
                    HelpClass.SetValidate(p_error_printCount, "trMustBeMoreThanZero");
                }
                else
                {

                    if (printCount == null)
                        printCount = new SetValues();
                    printCount.value = tb_printCount.Text;
                    printCount.isSystem = 1;
                    printCount.isDefault = 1;
                    printCount.settingId = printCountId;

                    int s = await valueModel.Save(printCount);
                    if (!s.Equals(0))
                    {
                        //update tax in main window
                        AppSettings.Allow_print_inv_count = printCount.value;

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
           
        }
        #region Numeric 

        private int _numValue_printCount = 0;
        public int NumValue_printCount
        {
            get {
                if (int.TryParse(tb_printCount.Text, out _numValue_printCount))
                    _numValue_printCount = int.Parse(tb_printCount.Text);
                return _numValue_printCount;
            }
            set
            {
                _numValue_printCount = value;
                tb_printCount.Text = value.ToString();
            }
        }

        private void Btn_cmdUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NumValue_printCount++;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_cmdDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NumValue_printCount > 0)
                    NumValue_printCount--;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        private async void Btn_printHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                //  SectionData.validateEmptyComboBox(cb_serverStatus, p_errorServerStatus, tt_errorServerStatus, "trEmptyServerStatus");
                if (!cb_printHeader.Text.Equals(""))
                {

                    int res = 0;
                    show_header_row.value = cb_printHeader.SelectedValue.ToString();
                    res = await setvalueModel.Save(show_header_row);

              


                    if (res > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    await FillprintList();
                    fillPrintHeader();
                    await MainWindow.Getprintparameter();
                }

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_itemsTaxNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;

                wd_notes w = new wd_notes();
                w.maxLength = 100;
                //   w.note = "Test note...";
                itemtax_note_row = printList.Where(X => X.name == "itemtax_note").FirstOrDefault();
                itemtax_note = itemtax_note_row.value;

                w.note = itemtax_note;
                w.ShowDialog();
                
                if(w.isOk)
                {
                    
                    int res = 0;
                    itemtax_note_row.value = w.note.Trim();
                    res = await setvalueModel.Save(itemtax_note_row);



                    if (res > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    await FillprintList();

                    await MainWindow.Getprintparameter();
                }

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
 
        
        }

        private async void Btn_salesInvoiceNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Opacity = 0.2;

                wd_notes w = new wd_notes();
                w.maxLength = 100;
                sales_invoice_note_row = printList.Where(X => X.name == "sales_invoice_note").FirstOrDefault();
                sales_invoice_note = sales_invoice_note_row.value;
                w.note = sales_invoice_note;
                w.ShowDialog();
                if (w.isOk)
                {
                    // MessageBox.Show(w.note);
                    //save
                    int res = 0;
                    sales_invoice_note_row.value = w.note.Trim();
                    res = await setvalueModel.Save(sales_invoice_note_row);


                    if (res > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);

                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    await FillprintList();

                    await MainWindow.Getprintparameter();

                }

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

            

        }

        
    }
}
