using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.Win32;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace laundryApp.View.settings
{
    /// <summary>
    /// Interaction logic for uc_general.xaml
    /// </summary>
    public partial class uc_general : UserControl
    {

        private static uc_general _instance;
        public static uc_general Instance
        {
            get
            {
                if(_instance is null)
                    _instance = new uc_general();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_general()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }
        static SettingCls setModel = new SettingCls();
        static SetValues valueModel = new SetValues();
        AvtivateServer ac = new AvtivateServer();
        static UserSetValues usValueModel = new UserSetValues();
        static CountryCode countryModel = new CountryCode();
        static SettingCls set = new SettingCls();
        static SetValues tax = new SetValues();
        //tax
        static SetValues setVInvoice = new SetValues();
        static SetValues setVInvoiceBool = new SetValues();
        static SetValues setVItem = new SetValues();
        static SetValues setVItemBool = new SetValues();
        ////////
        SetValues activationSite = new SetValues();
        static SetValues itemCost = new SetValues();
        static SetValues printCount = new SetValues();
        static SetValues accuracy = new SetValues();
        static SetValues maxDiscount = new SetValues();
        static SetValues statusesOfPreparingOrder = new SetValues();
        static SetValues dateForm = new SetValues();
        static SetValues cost = new SetValues();
        static public UserSetValues usLanguage = new UserSetValues();
        static CountryCode region = new CountryCode();
        static List<SetValues> languages = new List<SetValues>();
        static int taxId = 0, costId = 0, dateFormId, accuracyId, maxDiscountId, itemCostId = 0, statusesOfPreparingOrderId, printCountId = 0;
        string usersSettingsPermission = "general_usersSettings";
        string companySettingsPermission = "general_companySettings";

        static ProgramDetails progDetailsModel = new ProgramDetails();
        static ProgramDetails progDetails = new ProgramDetails();


        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        ReportCls reportclass = new ReportCls();

        public static List<SettingCls> settingsCls = new List<SettingCls>();
        public static List<SetValues> settingsValues = new List<SetValues>();

        LocalReport rep = new LocalReport();
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        #region loading
        bool firstLoading = true;
        List<keyValueBool> loadingList;
        async void loading_fillRegions()
        {
            try
            {
                await fillRegions();
                #region get default region
                await getDefaultRegion();
                if (region != null)
                {
                    cb_region.SelectedValue = region.countryId;
                    cb_region.Text = region.name;
                }
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillRegions"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async Task loading_fillLanguages()
        {
            try
            {
                await fillLanguages();
                #region get default language
                await getDefaultLanguage();
                if (usLanguage != null)
                    cb_language.SelectedValue = usLanguage.valId;
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillLanguages"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_fillCurrencies()
        {
            try
            {
                await fillCurrencies();
                #region get default currency
                if (region != null)
                {
                    tb_currency.Text = region.currency;
                }
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillCurrencies"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        async void loading_getDefaultItemCost()
        {
            try
            {
                #region get default item cost
                await getDefaultItemCost();
                if (itemCost != null)
                    tb_itemsCost.Text = itemCost.value;
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDefaultItemCost"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getDefaultActivationSite()
        {//??
            try
            {
                activationSite = await ac.getactivesite();
                if (activationSite != null)
                    tb_activationSite.Text = activationSite.value;
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDefaultActivationSite"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        async void loading_getDefaultDateForm()
        {
            try
            {
                #region fill dateform
                DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;
                var date = DateTime.Now;
                var typelist = new[] {
                    new { Text = date.ToString(dtfi.ShortDatePattern), Value = "ShortDatePattern" },
                    new { Text = date.ToString(dtfi.LongDatePattern) , Value = "LongDatePattern" },
                    new { Text =  date.ToString(dtfi.MonthDayPattern), Value = "MonthDayPattern" },
                    new { Text =  date.ToString(dtfi.YearMonthPattern), Value = "YearMonthPattern" },
                     };
                cb_dateForm.DisplayMemberPath = "Text";
                cb_dateForm.SelectedValuePath = "Value";
                cb_dateForm.ItemsSource = typelist;
                #endregion

                #region get default date form
                await getDefaultDateForm();
                if (dateForm != null)
                    cb_dateForm.SelectedValue = dateForm.value;
                else
                    cb_dateForm.SelectedIndex = -1;
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDefaultDateForm"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        async Task loading_getDefaultServerStatus()
        {
            try
            {
                fillTypeOnline();

                #region get default server status
                await getDefaultServerStatus();
                if (progDetails != null)
                {
                    if (progDetails.isOnlineServer.Value) cb_serverStatus.SelectedIndex = 0;
                    else cb_serverStatus.SelectedIndex = 1;
                }
                else
                    cb_serverStatus.SelectedIndex = -1;
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDefaultServerStatus"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_fillAccuracy()
        {
            try
            {
                fillAccuracy();
                #region get default accracy
                await getDefaultAccuracy();
                if (accuracy != null)
                {
                    cb_accuracy.SelectedValue = accuracy.value;
                }
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillAccuracy"))
                {
                    item.value = true;
                    break;
                }
            }
        }
         async void loading_fillMaxDiscount()
        {
            try
            {
                #region get default max discount
                await getDefaultMaxDiscount();
                if (maxDiscount != null)
                {
                    tb_maxDiscount.Text = maxDiscount.value;
                }
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillMaxDiscount"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        async void loading_fillStatusesOfPreparingOrder()
        {
            try
            {
                FillCombo.FillStatusesOfPreparingOrder(cb_statusesOfPreparingOrder);

                //cb_statusesOfPreparingOrder.SelectedValue = AppSettings.statusesOfPreparingOrder;
                #region get default statusesOfPreparingOrder
                await getDefaultStatusesOfPreparingOrder();
                if (statusesOfPreparingOrder != null)
                {
                    cb_statusesOfPreparingOrder.SelectedValue = statusesOfPreparingOrder.value;
                }
                #endregion
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillStatusesOfPreparingOrder"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        #endregion
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                //permission();
                chk_userSetting.IsChecked = true;

                settingsCls = await setModel.GetAll();
                settingsValues = await valueModel.GetAll();

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

                #region loading
                if (firstLoading)
                {
                    loadingList = new List<keyValueBool>();
                    bool isDone = true;
                    loadingList.Add(new keyValueBool { key = "loading_fillRegions", value = false });
                    loadingList.Add(new keyValueBool { key = "loading_fillCurrencies", value = false });
                    loadingList.Add(new keyValueBool { key = "loading_getDefaultItemCost", value = false });
                    loadingList.Add(new keyValueBool { key = "loading_getDefaultActivationSite", value = false });
                    loadingList.Add(new keyValueBool { key = "loading_getDefaultDateForm", value = false });
                    loadingList.Add(new keyValueBool { key = "loading_fillAccuracy", value = false });
                    loadingList.Add(new keyValueBool { key = "loading_fillMaxDiscount", value = false });
                    loadingList.Add(new keyValueBool { key = "loading_fillStatusesOfPreparingOrder", value = false });
                    //loadingList.Add(new keyValueBool { key = "loading_getDefaultServerStatus", value = false });

                    loading_fillRegions();
                    loading_fillCurrencies();
                    loading_getDefaultItemCost();
                    loading_getDefaultActivationSite();
                    loading_getDefaultDateForm();
                    loading_fillAccuracy();
                    loading_fillMaxDiscount();
                    loading_fillStatusesOfPreparingOrder();
                    //loading_getDefaultServerStatus();
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
                            //MessageBox.Show("not done");
                            //string s = "";
                            //foreach (var item in loadingList)
                            //{
                            //    s += item.name + " - " + item.value + "\n";
                            //}
                            //MessageBox.Show(s);
                            await Task.Delay(0500);
                            //MessageBox.Show("do");
                        }
                    }
                    while (!isDone);
                    await loading_fillLanguages();
                    await loading_getDefaultServerStatus();
                    fillBackup();
                    firstLoading = false;

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
        void permission()
        {
            
            List<string> userSettingList = new List<string> { "language" , "userPath", "errorsExport" };
            List<string> adminSettingList = new List<string> { "companyInfo" , "region" ,"currency" ,  "changePassword", "backup", "dateForm", "tableTimes", "statusesOfPreparingOrder", "typesOfService" };
            List<string> financeSettingList = new List<string> { "tax" ,  "accuracy", "itemsCost", "maxDiscount" };
            List<string> supportSettingList = new List<string> { "activationSite" , "serverStatus" };

            IEnumerable<Border> bordersList = FindControls.FindVisualChildren<Border>(this)
               .Where(x => x.Tag != null);

            if (!HelpClass.isAdminPermision() && !FillCombo.groupObject.HasPermission(companySettingsPermission, FillCombo.groupObjects))
            {
                sp_userSetting.Visibility = Visibility.Collapsed;
                
                foreach (var item in bordersList)
                {
                    if (userSettingList.Contains(item.Tag.ToString()))
                        item.Visibility = Visibility.Visible;
                    else
                        item.Visibility = Visibility.Collapsed;
                }

                //brd_companyInfo.Visibility = Visibility.Collapsed;
                //brd_region.Visibility = Visibility.Collapsed;
                //brd_currency.Visibility = Visibility.Collapsed;
                //brd_tax.Visibility = Visibility.Collapsed;
                //brd_dateForm.Visibility = Visibility.Collapsed;
                //brd_changePassword.Visibility = Visibility.Collapsed;
                //brd_accuracy.Visibility = Visibility.Collapsed;
                //brd_backup.Visibility = Visibility.Collapsed;
                //brd_itemsCost.Visibility = Visibility.Collapsed;
                //brd_maxDiscount.Visibility = Visibility.Collapsed;
                //brd_tableTimes.Visibility = Visibility.Collapsed;
                //brd_statusesOfPreparingOrder.Visibility = Visibility.Collapsed;
                //brd_typesOfService.Visibility = Visibility.Collapsed;
                //brd_activationSite.Visibility = Visibility.Collapsed;
                //brd_serverStatus.Visibility = Visibility.Collapsed;


            }
            else
            {
                if (HelpClass.isSupportPermision())
                {
                    chk_supportSetting.Visibility = Visibility.Visible;
                }
                else
                {
                    chk_supportSetting.IsChecked = false;
                    chk_supportSetting.Visibility = Visibility.Collapsed;
                }

                if(chk_userSetting.IsChecked.Value)
                {
                    foreach (var item in bordersList)
                    {
                        if (userSettingList.Contains(item.Tag.ToString()))
                            item.Visibility = Visibility.Visible;
                        else
                            item.Visibility = Visibility.Collapsed;
                    }
                }
                else if (chk_adminSetting.IsChecked.Value)
                {
                    foreach (var item in bordersList)
                    {
                        if (adminSettingList.Contains(item.Tag.ToString()))
                            item.Visibility = Visibility.Visible;
                        else
                            item.Visibility = Visibility.Collapsed;
                    }
                }
                else if (chk_financeSetting.IsChecked.Value)
                {
                   

                    foreach (var item in bordersList)
                    {
                        if (financeSettingList.Contains(item.Tag.ToString()))
                            item.Visibility = Visibility.Visible;
                        else
                            item.Visibility = Visibility.Collapsed;
                    }
                }
                else if (chk_supportSetting.IsChecked.Value)
                {
                    foreach (var item in bordersList)
                    {
                        if (supportSettingList.Contains(item.Tag.ToString()))
                            item.Visibility = Visibility.Visible;
                        else
                            item.Visibility = Visibility.Collapsed;
                    }
                }


                    //foreach (var item in bordersList)
                    //{
                    //    if (userSettingList.Contains(item.Tag.ToString()))
                    //        item.Visibility = Visibility.Visible;
                    //    else
                    //        item.Visibility = Visibility.Collapsed;
                    //}


            }

            //    if (HelpClass.isSupportPermision())
            //{
            //    brd_activationSite.Visibility = Visibility.Visible;
            //    brd_serverStatus.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    brd_activationSite.Visibility = Visibility.Collapsed;
            //    brd_serverStatus.Visibility = Visibility.Collapsed;
            //}
        }
        private  void userSetting_check(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.IsChecked == true)
                    {
                        if (cb.Name == "chk_userSetting")
                        {
                            chk_adminSetting.IsChecked = false;
                            chk_financeSetting.IsChecked = false;
                            chk_supportSetting.IsChecked = false;
                        }
                        else if (cb.Name == "chk_adminSetting")
                        {
                            chk_userSetting.IsChecked = false;
                            chk_financeSetting.IsChecked = false;
                            chk_supportSetting.IsChecked = false;
                        }
                        else if (cb.Name == "chk_financeSetting")
                        {
                            chk_userSetting.IsChecked = false;
                            chk_adminSetting.IsChecked = false;
                            chk_supportSetting.IsChecked = false;
                        }
                        else if (cb.Name == "chk_supportSetting")
                        {
                            chk_userSetting.IsChecked = false;
                            chk_adminSetting.IsChecked = false;
                            chk_financeSetting.IsChecked = false;
                        }
                    }
                }

                permission();
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void userSetting_uncheck(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_userSetting")
                        chk_userSetting.IsChecked = true;
                    else if (cb.Name == "chk_adminSetting")
                        chk_adminSetting.IsChecked = true;
                    else if (cb.Name == "chk_financeSetting")
                        chk_financeSetting.IsChecked = true;
                    else if (cb.Name == "chk_supportSetting")
                        chk_supportSetting.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_companyInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);


                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
                    Window.GetWindow(this).Opacity = 0.2;

                    wd_companyInfo w = new wd_companyInfo();
                    w.isFirstTime = false;
                    w.ShowDialog();

                    Window.GetWindow(this).Opacity = 1;
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
        private void fillAccuracy()
        {
            var list = new[] {
                new { Text = "0"       , Value = "0" },
                new { Text = "0.0"     , Value = "1" },
                new { Text = "0.00"    , Value = "2" },
                new { Text = "0.000"   , Value = "3" },
                 };
            cb_accuracy.DisplayMemberPath = "Text";
            cb_accuracy.SelectedValuePath = "Value";
            cb_accuracy.ItemsSource = list;
        }
        private void fillBackup()
        {
            cb_backup.DisplayMemberPath = "Text";
            cb_backup.SelectedValuePath = "Value";
            var typelist = new[] {
                 new { Text = AppSettings.resourcemanager.GetString("trBackup")       , Value = "backup" },
                 new { Text = AppSettings.resourcemanager.GetString("trRestore")       , Value = "restore" },
                };
            cb_backup.ItemsSource = typelist;
            cb_backup.SelectedIndex = 0;
        }
        public static async Task<SetValues> getDefaultAccuracy()
        {
            //set = settingsCls.Where(s => s.name == "accuracy").FirstOrDefault<SettingCls>();
            //accuracyId = set.settingId;

            //settingsValues = await valueModel.GetBySetName("accuracy");
            //accuracy = settingsValues.Where(i => i.settingId == accuracyId).FirstOrDefault();

            settingsValues = await valueModel.GetBySetName("accuracy");
            accuracy = settingsValues.FirstOrDefault();
            return accuracy;
        }
         public static async Task<SetValues> getDefaultMaxDiscount()
        {
            //set = settingsCls.Where(s => s.name == "maxDiscount").FirstOrDefault<SettingCls>();
            //maxDiscountId = set.settingId;
            //maxDiscount = settingsValues.Where(i => i.settingId == maxDiscountId).FirstOrDefault();


            settingsValues = await valueModel.GetBySetName("maxDiscount");
            maxDiscount = settingsValues.FirstOrDefault();
            return maxDiscount;
        }
        public static async Task<SetValues> getDefaultStatusesOfPreparingOrder()
        {

            //settingsValues = await valueModel.GetBySetName("statusesOfPreparingOrder");
            //statusesOfPreparingOrder = settingsValues.FirstOrDefault();
            //return statusesOfPreparingOrder;

            List<SetValues> settingsValues = await AppSettings.valueModel.GetBySetName("statusesOfPreparingOrder");
            statusesOfPreparingOrder = settingsValues.FirstOrDefault();
            if (statusesOfPreparingOrder != null)
                AppSettings.statusesOfPreparingOrder = statusesOfPreparingOrder.value;
            else
                AppSettings.statusesOfPreparingOrder = "directlyPrint";
            return statusesOfPreparingOrder;
        }
        public static async Task<ProgramDetails> getDefaultServerStatus()
        {
            progDetails = await progDetailsModel.getCurrentInfo();

            return progDetails;
        }
        public static async Task<SetValues> getDefaultCost()
        {
            //set = settingsCls.Where(s => s.name == "storage_cost").FirstOrDefault<SettingCls>();
            //costId = set.settingId;
            //cost = settingsValues.Where(i => i.settingId == costId).FirstOrDefault();

            settingsValues = await valueModel.GetBySetName("storage_cost");
            cost = settingsValues.FirstOrDefault();
            return cost;
        }
        public static async Task<CountryCode> getDefaultRegion()
        {
            List<CountryCode> regions = new List<CountryCode>();
            regions = await countryModel.GetAllRegion();
            region = regions.Where(r => r.isDefault == 1).FirstOrDefault<CountryCode>();

      
            return region;
        }
        /*
        public static async Task<SetValues> getDefaultTax()
        {
             
            set = settingsCls.Where(s => s.name == "tax").FirstOrDefault<SettingCls>();
            taxId = set.settingId;
            tax = settingsValues.Where(i => i.settingId == taxId).FirstOrDefault();

            return tax;
        }
        */
        public static async Task<List<string>> getDefaultTaxList()
        {
            //List<SetValues> sv = new List<SetValues>();
            //sv = settingsValues.Where(v => v.notes == "tax").ToList();
            //setVInvoiceBool = sv[0];
            //setVInvoice = sv[1];
            //setVItemBool = sv[2];
            //setVItem = sv[3];

            settingsValues = await valueModel.GetBySetvalNote("tax");
            cost = settingsValues.Where(v => v.name == "tax").FirstOrDefault();
            setVInvoiceBool = settingsValues.Where(v => v.name == "invoiceTax_bool").FirstOrDefault();
            setVInvoice = settingsValues.Where(v => v.name == "invoiceTax_decimal").FirstOrDefault();
            setVItemBool = settingsValues.Where(v => v.name == "itemsTax_bool").FirstOrDefault(); ;
           // setVItem = settingsValues.Where(v => v.name == "invoiceTax_bool").FirstOrDefault();
            List<string> taxLst = new List<string>();
            taxLst.Add(setVInvoiceBool.value);
            taxLst.Add(setVInvoice.value);
            taxLst.Add(setVItemBool.value);
           // taxLst.Add(setVItem.value);

            return taxLst;
        }
        public static async Task<SetValues> getDefaultItemCost()
        {
            //if (settingsCls==null)
            //    settingsCls = await setModel.GetAll();
            //set = settingsCls.Where(s => s.name == "item_cost").FirstOrDefault<SettingCls>();
            //itemCostId = set.settingId;
            //itemCost = settingsValues.Where(i => i.settingId == itemCostId).FirstOrDefault();


            settingsValues = await valueModel.GetBySetName("item_cost");
            itemCost = settingsValues.FirstOrDefault();

            return itemCost;
        }
        //public static async Task<SetValues> getDefaultPrintCount()
        //{

        //    set = settingsCls.Where(s => s.name == "Allow_print_inv_count").FirstOrDefault<SettingCls>();
        //    printCountId = set.settingId;
        //    printCount = settingsValues.Where(i => i.settingId == printCountId).FirstOrDefault();
        //    return printCount;
        //}
        public static async Task<SetValues> getDefaultDateForm()
        {
            
            //set = settingsCls.Where(s => s.name == "dateForm").FirstOrDefault<SettingCls>();
            //dateFormId = set.settingId;
            //dateForm = settingsValues.Where(i => i.settingId == dateFormId).FirstOrDefault();

            settingsValues = await valueModel.GetBySetName("dateForm");
            dateForm = settingsValues.FirstOrDefault();
            return dateForm;
        }
        public static async Task<UserSetValues> getDefaultLanguage()
        {
            //var lanSettings = await setModel.GetAll();
            //set = lanSettings.Where(l => l.name == "language").FirstOrDefault<SettingCls>();
            //var lanValues = await valueModel.GetAll();

            //languages = lanValues.Where(vl => vl.settingId == set.settingId).ToList<SetValues>();

            languages= await valueModel.GetBySetName("language");
            List<UserSetValues> usValues = new List<UserSetValues>();
            usValues = await usValueModel.GetAll();
            var curUserValues = usValues.Where(c => c.userId == MainWindow.userLogin.userId);
            foreach (var l in curUserValues)
                if (languages.Any(c => c.valId == l.valId))
                {
                    usLanguage = l;
                }
            return usLanguage;
        }
        int usValueId = 0;
        private async Task fillCurrencies()
        {
            /*
            cb_currency.ItemsSource = await countryModel.GetAllRegion();
            cb_currency.DisplayMemberPath = "currency";
            cb_currency.SelectedValuePath = "countryId";
            */
        }
        private async Task fillLanguages()
        {
            //    var lanSettings = await setModel.GetAll();
            //    set = lanSettings.Where(l => l.name == "language").FirstOrDefault<SettingCls>();
            //    var lanValues = await valueModel.GetAll();
            //    languages = lanValues.Where(vl => vl.settingId == set.settingId).ToList<SetValues>();

            languages = await valueModel.GetBySetName("language");
         
            foreach (var v in languages)
            {
                if (v.value.ToString().Equals("en")) v.value = AppSettings.resourcemanager.GetString("trEnglish");
                else if (v.value.ToString().Equals("ar")) v.value = AppSettings.resourcemanager.GetString("trArabic");
            }

            cb_language.ItemsSource = languages;
            cb_language.DisplayMemberPath = "value";
            cb_language.SelectedValuePath = "valId";

        }
        private async Task fillRegions()
        {
            cb_region.ItemsSource = await countryModel.GetAllRegion();
            cb_region.DisplayMemberPath = "name";
            cb_region.SelectedValuePath = "countryId";
        }
        private async Task translate()
        {
            txt_mainTitle.Text = AppSettings.resourcemanager.GetString("trGeneralSettings");

            txt_companyInfo.Text = AppSettings.resourcemanager.GetString("trComInfo");
            txt_companyInfoHint.Text = AppSettings.resourcemanager.GetString("trSettingHint");
            txt_typesOfService.Text = AppSettings.resourcemanager.GetString("typesOfService");
            txt_typesOfServiceHint.Text = AppSettings.resourcemanager.GetString("diningHallTakeAway");
            txt_region.Text = AppSettings.resourcemanager.GetString("trRegion");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_region, AppSettings.resourcemanager.GetString("trRegion"));
            txt_language.Text = AppSettings.resourcemanager.GetString("trLanguage");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_language, AppSettings.resourcemanager.GetString("trLanguage"));
            txt_currency.Text = AppSettings.resourcemanager.GetString("trCurrency");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_currency, AppSettings.resourcemanager.GetString("trCurrency"));
            txt_tax.Text = AppSettings.resourcemanager.GetString("trTax");
            txt_taxHint.Text = AppSettings.resourcemanager.GetString("trInvoice") + ", " + AppSettings.resourcemanager.GetString("trItem") + "...";
            txt_tableTimes.Text = AppSettings.resourcemanager.GetString("tablesTimes");
            txt_tableTimesHint.Text = AppSettings.resourcemanager.GetString("tablesTimes") + "...";
            txt_itemsCost.Text = AppSettings.resourcemanager.GetString("trItemCost");
            txt_dateForm.Text = AppSettings.resourcemanager.GetString("trDateForm");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_dateForm, AppSettings.resourcemanager.GetString("trDateForm"));
            txt_accuracy.Text = AppSettings.resourcemanager.GetString("trAccuracy");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_accuracy, AppSettings.resourcemanager.GetString("trAccuracy"));
            txt_maxDiscount.Text = AppSettings.resourcemanager.GetString("maxDiscount");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_maxDiscount, AppSettings.resourcemanager.GetString("maxDiscount"));
            txt_changePassword.Text = AppSettings.resourcemanager.GetString("trChangePassword");
            txt_changePasswordHint.Text = AppSettings.resourcemanager.GetString("trChangePasswordHint");
            txt_userPath.Text = AppSettings.resourcemanager.GetString("trUserPath");
            txt_userPathHint.Text = AppSettings.resourcemanager.GetString("trUserPath") + "...";
            txt_errorsExport.Text = AppSettings.resourcemanager.GetString("trErrorsFile");
            txt_errorsExportHint.Text = AppSettings.resourcemanager.GetString("trErrorFileDownload") + "...";
            txt_itemsCost.Text = AppSettings.resourcemanager.GetString("trPurchaseCost");
            brd_itemsCost.ToolTip = AppSettings.resourcemanager.GetString("trItemCostHint");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_itemsCost, AppSettings.resourcemanager.GetString("trItemCost"));
            txt_backup.Text = AppSettings.resourcemanager.GetString("trBackUp/Restore");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_backup, AppSettings.resourcemanager.GetString("trBackUp/Restore"));
            txt_activationSite.Text = AppSettings.resourcemanager.GetString("trActivationSite");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_activationSite, AppSettings.resourcemanager.GetString("trActivationSite") + "...");
            txt_serverStatus.Text = AppSettings.resourcemanager.GetString("trServerType");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_serverStatus, AppSettings.resourcemanager.GetString("trServerType") + "...");
            
            txt_statusesOfPreparingOrder.Text = AppSettings.resourcemanager.GetString("trPreparingOrders");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_statusesOfPreparingOrder, AppSettings.resourcemanager.GetString("statuses"));
            brd_statusesOfPreparingOrder.ToolTip = AppSettings.resourcemanager.GetString("statusesOfPreparingOrder");

            chk_userSetting.Content = AppSettings.resourcemanager.GetString("trUser");
            chk_adminSetting.Content = AppSettings.resourcemanager.GetString("trAdmin");
            chk_financeSetting.Content = AppSettings.resourcemanager.GetString("finance");
            chk_supportSetting.Content = AppSettings.resourcemanager.GetString("trSupport");



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
        private async void Btn_region_Click(object sender, RoutedEventArgs e)
        {//save region
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{
                int s = 0;

                if (cb_region.Text.Equals(""))
                {
                    //HelpClass.validateEmptyComboBox(cb_region, p_errorRegion, tt_errorRegion, "trEmptyRegion");
                    HelpClass.SetValidate(p_error_region, "trEmptyRegion");
                }
                else
                {
                    HelpClass.clearValidate(p_error_region);
                    int regionId = Convert.ToInt32(cb_region.SelectedValue);
                    if (regionId != 0)
                    {
                        s = await countryModel.UpdateIsdefault(regionId);
                        if (!s.Equals(0))
                        {
                            //update region and currency in main window
                            List<CountryCode> c = await countryModel.GetAllRegion();
                            AppSettings.Region = c.Where(r => r.countryId == s).FirstOrDefault<CountryCode>();
                            AppSettings.Currency = AppSettings.Region.currency;
                            AppSettings.CurrencyId = AppSettings.Region.currencyId;
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                }
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
        private async void Btn_language_Click(object sender, RoutedEventArgs e)
        {//save language
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(usersSettingsPermission, FillCombo.groupObjects, "one") ||
                //    FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{
                    if (cb_language.Text.Equals(""))
                    {
                        //HelpClass.validateEmptyComboBox(cb_language, p_errorLanguage, tt_errorLanguage, "trEmptyLanguage");
                        HelpClass.SetValidate(p_error_language, "trEmptyLanguage");
                    }
                    else
                    {
                        if (usLanguage == null)
                            usLanguage = new UserSetValues();
                        if (Convert.ToInt32(cb_language.SelectedValue) != 0)
                        {
                            
                                HelpClass.StartAwait(grid_main);
                            usLanguage.userId = MainWindow.userLogin.userId;
                            usLanguage.valId = Convert.ToInt32(cb_language.SelectedValue);
                            usLanguage.createUserId = MainWindow.userLogin.userId;
                             int s = await usValueModel.Save(usLanguage);
                            if (!s.Equals(0))
                            {
                                //update language in main window
                                SetValues v = await valueModel.GetByID(Convert.ToInt32(cb_language.SelectedValue));
                                AppSettings.lang = v.value;
                                //save to user settings
                                Properties.Settings.Default.Lang = v.value;
                                Properties.Settings.Default.Save();

                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            
                                HelpClass.EndAwait(grid_main);
                            uc_settings objUC1 = new uc_settings();
 
                            //update languge in main window
                            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;

                            if (parentWindow != null)
                            {
                            

                            //access property of the MainWindow class that exposes the access rights...
                            if (AppSettings.lang.Equals("en"))
                                {
                                    AppSettings.resourcemanager = new ResourceManager("laundryApp.en_file", Assembly.GetExecutingAssembly());
                                    parentWindow.grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
                                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                                }
                                else
                                {
                                    AppSettings.resourcemanager = new ResourceManager("laundryApp.ar_file", Assembly.GetExecutingAssembly());
                                    parentWindow.grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
                                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                                }

                                parentWindow.translate();
                                MainWindow.mainWindow.grid_main.Children.Clear();
                                MainWindow.loadingDefaultPath("general");
                               await translate();
                            }
                        }
                    }
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private  void Btn_tax_Click(object sender, RoutedEventArgs e)
        {//save Tax
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_taxSetting w = new wd_taxSetting();
                    w.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
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
        /*
        private void Btn_currency_Click(object sender, RoutedEventArgs e)
        {//save currency
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
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
        private void Cb_region_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                region = cb_region.SelectedItem as CountryCode;
                if (region != null)
                    tb_currency.Text = region.currency;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        /*
        private void Tb_PreventSpaces(object sender, KeyEventArgs e)
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
        private void Tb_decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        { //decimal
            try
            {
                var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Tb_validateEmptyTextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                validateEmpty(name, sender);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                 
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
         private void validateEmpty(string name, object sender)
        {//validate
            try
            {
                if (name == "TextBox")
                {
                    if ((sender as TextBox).Name == "tb_activationSite")
                        HelpClass.validateEmptyTextBox((TextBox)sender, p_errorActivationSite, tt_errorActivationSite, "trEmptyActivationSite");
                    if ((sender as TextBox).Name == "tb_itemsCost")
                        HelpClass.validateEmptyTextBox((TextBox)sender, p_errorItemsCost, tt_errorItemsCost, "trEmptyItemCost");

                }
                else if (name == "ComboBox")
                {
                    if ((sender as ComboBox).Name == "cb_region")
                        HelpClass.validateEmptyComboBox((ComboBox)sender, p_errorRegion, tt_errorRegion, "trEmptyRegion");
                    if ((sender as ComboBox).Name == "cb_language")
                        HelpClass.validateEmptyComboBox((ComboBox)sender, p_errorLanguage, tt_errorLanguage, "trEmptyLanguage");
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        User userModel = new User();
        User user = new User();
        */
       
        private async void Btn_dateForm_Click(object sender, RoutedEventArgs e)
        {
            //save date form
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
                    //HelpClass.validateEmptyComboBox(cb_dateForm, p_errorDateForm, tt_errorDateForm, "trEmptyDateFormat");
                    if (cb_dateForm.Text.Equals(""))
                    {
                        HelpClass.SetValidate(p_error_dateForm, "trEmptyDateFormat");
                    }
                    else
                    {
                        HelpClass.clearValidate(p_error_dateForm);
                        if (dateForm != null)
                        { 
                        dateForm.value = cb_dateForm.SelectedValue.ToString();
                        //dateForm.isSystem = 1;
                        //dateForm.settingId = dateFormId;
                        int s = await valueModel.Save(dateForm);
                        if (!s.Equals(0))
                        {
                            //update dateForm in main window
                            AppSettings.dateFormat = dateForm.value;

                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

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
        private void Btn_userPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(usersSettingsPermission, FillCombo.groupObjects, "one") ||
                //    FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_userPath w = new wd_userPath();
                    w.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_backup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                        HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
                    
                    if (cb_backup.SelectedValue.ToString() == "backup")
                    {
                        BackupCls back = new BackupCls();
                        saveFileDialog.Filter = "INC|*.inc;";

                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filepath = saveFileDialog.FileName;
                            string message = await back.GetFile(filepath);
                            if (message == "1")
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBackupDoneSuccessfuly"), animation: ToasterAnimation.FadeIn);
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBackupNotComplete"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    else
                    {
                        // restore
                        string filepath = "";
                        openFileDialog.Filter = "INC|*.inc; ";
                        BackupCls back = new BackupCls();
                        if (openFileDialog.ShowDialog() == true)
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            w.contentText = AppSettings.resourcemanager.GetString("trContinueProcess?");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                // here start restore if user click yes button Mr. Yasin //////////////////////////////////////////////////////
                                filepath = openFileDialog.FileName;
                                string message = await back.uploadFile(filepath);
                                if (message == "1")
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trRestoreDoneSuccessfuly"), animation: ToasterAnimation.FadeIn);
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trRestoreNotComplete"), animation: ToasterAnimation.FadeIn);


                            }
                            else
                            {
                                // here if user click no button

                            }
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
        /*
        private void Btn_book_Click(object sender, RoutedEventArgs e)
        {//book
            //grid_main.Children.Clear();
            //grid_main.Children.Add(uc_packageBookSetting.Instance);
            //Button button = sender as Button;
        }
        */
        private async void Btn_activationSite_Click(object sender, RoutedEventArgs e)
        {//activation Site
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                /*
                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
                    HelpClass.validateEmptyTextBox(tb_activationSite, p_errorActivationSite, tt_errorActivationSite, "trEmptyActivationSite");
                    if (!tb_activationSite.Text.Equals(""))
                    {

                        activationSite = await ac.getactivesite();

                        // save
                        activationSite.value = @tb_activationSite.Text;
                        int res = await valueModel.Save(activationSite);

                        if (!res.Equals(0))
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                    */
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private async void Btn_serverStatus_Click(object sender, RoutedEventArgs e)
        {//server status
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                /*
                HelpClass.validateEmptyComboBox(cb_serverStatus, p_errorServerStatus, tt_errorServerStatus, "trEmptyServerStatus");
                if (!cb_serverStatus.Text.Equals(""))
                {
                    if (progDetails == null)
                        progDetails = new ProgramDetails();

                    bool isOnline = bool.Parse(cb_serverStatus.SelectedValue.ToString());
                    int res = await progDetailsModel.updateIsonline(isOnline);

                    if (res > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        MainWindow.loadingDefaultPath("settings", "general");
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }
                */
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void fillTypeOnline()
        {
            cb_serverStatus.DisplayMemberPath = "Text";
            cb_serverStatus.SelectedValuePath = "Value";
            var typelist = new[] {
                 new { Text = AppSettings.resourcemanager.GetString("trOnlineType")       , Value = "True" },
                 new { Text = AppSettings.resourcemanager.GetString("trOfflineType")       , Value = "False" },
                };
            cb_serverStatus.ItemsSource = typelist;
        }
        private async void Btn_itemsCost_Click(object sender, RoutedEventArgs e)
        {//save purchase cost
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{

                if (tb_itemsCost.Text.Equals(""))
                {
                    //HelpClass.validateEmptyTextBox(tb_itemsCost, p_errorItemsCost, tt_errorItemsCost, "trEmptyItemCost");
                    HelpClass.SetValidate(p_error_itemsCost, "trEmptyItemCost");
                }
                else
                {
                    HelpClass.clearValidate(p_error_itemsCost);
                    if (itemCost != null)
                    {
                        itemCost.value = tb_itemsCost.Text;
                        //itemCost.isSystem = 1;
                        //itemCost.isDefault = 1;
                        //itemCost.settingId = itemCostId;

                        int s = await valueModel.Save(itemCost);
                        if (!s.Equals(0))
                        {
                            //update item cost in main window
                            AppSettings.itemCost = int.Parse(itemCost.value);

                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                }
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
        private async void Btn_maxDiscount_Click(object sender, RoutedEventArgs e)
        {
            //save maxDiscount
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{

                if (tb_maxDiscount.Text.Equals(""))
                {
                    //HelpClass.validateEmptyComboBox(cb_accuracy, p_errorAccuracy, tt_errorAccuracy, "trEmptyAccuracy");
                    HelpClass.SetValidate(p_error_maxDiscount, "trIsRequired");
                }
                else if (decimal.Parse(tb_maxDiscount.Text) < 0 || decimal.Parse(tb_maxDiscount.Text) > 100  )
                {
                    HelpClass.SetValidate(p_error_maxDiscount, "trValidRange");
                }
                else
                {
                    HelpClass.clearValidate(p_error_maxDiscount);

                    if (maxDiscount == null)
                        maxDiscount = new SetValues();
                    maxDiscount.value = tb_maxDiscount.Text;
                    //maxDiscount.isSystem = 1;
                    //maxDiscount.settingId = maxDiscountId;
                    int s = await valueModel.Save(maxDiscount);
                    if (!s.Equals(0))
                    {
                        //update maxDiscount in main window
                        AppSettings.maxDiscount =decimal.Parse( maxDiscount.value);

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }
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
        private void Btn_tableTimes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(usersSettingsPermission, FillCombo.groupObjects, "one") ||
                //    FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{
                Window.GetWindow(this).Opacity = 0.2;
                wd_tableTimes w = new wd_tableTimes();
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_statusesOfPreparingOrder_Click(object sender, RoutedEventArgs e)
        {
            //save statusesOfPreparingOrder
            try
            {

                HelpClass.StartAwait(grid_main);
                
                    if (cb_statusesOfPreparingOrder.Text.Equals(""))
                    {
                        HelpClass.SetValidate(p_error_statusesOfPreparingOrder, "trIsRequired");
                    }
                    else
                    {
                        HelpClass.clearValidate(p_error_statusesOfPreparingOrder);
                    /*
                        if (statusesOfPreparingOrder == null)
                        statusesOfPreparingOrder = new SetValues();

                        statusesOfPreparingOrder.value = cb_statusesOfPreparingOrder.SelectedValue.ToString();
                        statusesOfPreparingOrder.isSystem = 1;
                        statusesOfPreparingOrder.settingId = statusesOfPreparingOrderId;
                        int s = await valueModel.Save(statusesOfPreparingOrder);
                        if (!s.Equals(0))
                        {
                        //update statusesOfPreparingOrder in main window
                        AppSettings.statusesOfPreparingOrder = statusesOfPreparingOrder.value;

                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                  */


                


                    SetValues setV = new SetValues();
                    statusesOfPreparingOrder.value = cb_statusesOfPreparingOrder.SelectedValue.ToString();
                        //update statusesOfPreparingOrder in main window
                    AppSettings.statusesOfPreparingOrder = statusesOfPreparingOrder.value;

                    int msg = await setV.Save(statusesOfPreparingOrder);
                    if (msg > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }


                }
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_typesOfService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(usersSettingsPermission, FillCombo.groupObjects, "one") ||
                //    FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{
                Window.GetWindow(this).Opacity = 0.2;
                wd_typesOfService w = new wd_typesOfService();
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_errorsExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
                    saveFileDialog.Filter = "File|*.er;";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string DestPath = saveFileDialog.FileName;
                        ReportCls rc = new ReportCls();

                        List<ReportParameter> paramarr = new List<ReportParameter>();

                        string addpath;
                        bool isArabic = ReportCls.checkLang();
                        string pdfpath = "";
                        pdfpath = @"\Thumb\report\temp1.pdf";
                        pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                        addpath = @"\Reports\image\error.rdlc";
                        string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

                        List<ErrorClass> eList = new List<ErrorClass>();
                        ErrorClass errorModel = new ErrorClass();
                        eList = await errorModel.Get();

                        clsReports.ErrorsReport(eList, rep, reppath);
                        //  clsReports.setReportLanguage(paramarr);
                        clsReports.HeaderNoLogo(paramarr);

                        rep.SetParameters(paramarr);

                        rep.Refresh();
                        bool res = false;

                        LocalReportExtensions.ExportToExcel(rep, pdfpath);
                        res = rc.encodefile(pdfpath, DestPath);
                        rc.DelFile(pdfpath);
                        //  rc.decodefile(DestPath,@"D:\error.xls");
                        if (res)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
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
        private async void Btn_accuracy_Click(object sender, RoutedEventArgs e)
        {//save accuracy
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                //{

                if (cb_accuracy.Text.Equals(""))
                {
                    //HelpClass.validateEmptyComboBox(cb_accuracy, p_errorAccuracy, tt_errorAccuracy, "trEmptyAccuracy");
                    HelpClass.SetValidate(p_error_accuracy,  "trEmptyAccuracy");
                }
                else
                {
                    HelpClass.clearValidate(p_error_accuracy);

                    if (accuracy != null)
                    {
                        accuracy.value = cb_accuracy.SelectedValue.ToString();
                        //accuracy.isSystem = 1;
                        //accuracy.settingId = accuracyId;
                        int s = await valueModel.Save(accuracy);
                        if (!s.Equals(0))
                        {
                            //update accuracy in main window
                            AppSettings.accuracy = accuracy.value;

                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                }
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
        private  void Btn_changePassword_Click(object sender, RoutedEventArgs e)
        {//change password
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(companySettingsPermission, FillCombo.groupObjects, "one"))
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_adminChangePassword w = new wd_adminChangePassword();
                    w.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
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
             }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
    }
}
