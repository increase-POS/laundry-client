using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFTabTip;
using laundryApp;
using laundryApp.Classes;
using laundryApp.View.sectionData;
using laundryApp.View.catalog;
using laundryApp.View.purchase;
using laundryApp.View.storage;
using laundryApp.View.kitchen;
using laundryApp.View.sales;
using laundryApp.View.accounts;
using laundryApp.View.settings;
using Object = laundryApp.Classes.Object;
using laundryApp.View.sectionData.persons;
//using laundryApp.View.sectionData.hallDivide;
using laundryApp.View.sectionData.branchesAndStores;
using laundryApp.View.catalog.salesItems;
using laundryApp.View.storage.storageDivide;
using laundryApp.View.storage.storageOperations;
using laundryApp.View.storage.movementsOperations;
using laundryApp.View.storage.stocktakingOperations;
using laundryApp.View.sales.promotion;
using laundryApp.View.delivery;
using laundryApp.View.sectionData.banksData;
using laundryApp.View.catalog.rawMaterials;
using laundryApp.View.settings.reportsSettings;
//using laundryApp.View.sales.reservations;
using laundryApp.View.windows;
using laundryApp.View.settings.emailsGeneral;
using laundryApp.View.reports;
using laundryApp.View.reports.storageReports;
using laundryApp.View.reports.purchaseReports;
using laundryApp.View.reports.salesReports;
using laundryApp.View.reports.accountsReports;
using laundryApp.View.sales.promotion.membership;
using laundryApp.View;
using laundryApp.View.reports.kitchenReports;
using laundryApp.View.reports.deliveryReports;

namespace laundryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool menuState = false;
       

        
        internal static User userLogin; 
        internal static Pos posLogin; 
        internal static Branch branchLogin;
        internal static UsersLogs userLog;
        bool isHome = false;
        public static int Idletime = 5;
        public static int threadtime = 5;
        public static string menuIsOpen = "close";
        
        ImageBrush myBrush = new ImageBrush();

        public static DispatcherTimer timer;
        DispatcherTimer idletimer;//  logout timer
        DispatcherTimer threadtimer;//  repeat timer for check other login
        DispatcherTimer notTimer;//  repeat timer for notifications
                                 // print setting
       
        public static Boolean go_out = false;
        public static Boolean go_out_didNotAnyProcess = false;
       


        static public MainWindow mainWindow;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                mainWindow = this;
                windowFlowDirection();
            }
            catch
            { }
        }

        void windowFlowDirection()
        {
            #region translate
            if (AppSettings.lang.Equals("en"))
                grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
            else
                grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
            #endregion
        }

        #region loading
        List<keyValueBool> loadingList;
        List<string> catchError = new List<string>();
        int catchErrorCount = 0;
        async void loading_listObjects()
        {
            //get tax
            try
            {
                FillCombo.objectsList = await FillCombo.RefreshObjects();
            }
            catch
            {
                loading_listObjects();
                catchError.Add("loading_listObjects");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_listObjects"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getGroupObjects()
        {
            try
            {
                FillCombo.groupObjects = await FillCombo.groupObject.GetUserpermission(userLogin.userId);
            }
            catch (Exception)
            {
                loading_getGroupObjects();
                catchError.Add("loading_getGroupObjects");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getGroupObjects"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_globalItemUnitsList()
        {
            try
            {
                FillCombo.itemUnitList = await FillCombo.RefreshItemUnit();
            }
            catch (Exception)
            {
                loading_globalItemUnitsList();
                catchError.Add("loading_globalItemUnitsList");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_globalItemUnitsList"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_itemUnitsUsersList()
        {
            try
            {
                FillCombo.itemUnitsUsersList = await FillCombo.RefreshItemUnitUser();
            }
            catch (Exception)
            {
                loading_itemUnitsUsersList();
                catchError.Add("loading_itemUnitsUsersList");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_itemUnitsUsersList"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getUserPersonalInfo()
        {
            #region user personal info
            txt_userName.Text = userLogin.name;
            //txt_userJob.Text =  userLogin.job;
            txt_userJob.DataContext = userLogin;
            try
            {
                if (!string.IsNullOrEmpty(userLogin.image))
                {
                    byte[] imageBuffer = await FillCombo.user.downloadImage(userLogin.image); // read this as BLOB from your DB

                    var bitmapImage = new BitmapImage();

                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }

                    img_userLogin.Fill = new ImageBrush(bitmapImage);
                }
                else
                {
                    clearImg();
                }
            }
            catch
            {
                //clearImg();
                loading_getUserPersonalInfo();
                catchError.Add("loading_getUserPersonalInfo");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getUserPersonalInfo"))
                {
                    item.value = true;
                    break;
                }
            }
            #endregion
        }
        #region FillCombo
        async void loading_RefreshBranches()
        {
            try
            {
                await FillCombo.RefreshBranches();
            }
            catch (Exception)
            {
                loading_RefreshBranches();
                catchError.Add("loading_RefreshBranches");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefreshBranches"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_RefreshBranchesAllWithoutMain()
        {
            try
            {
                await FillCombo.RefreshBranchesAllWithoutMain();
            }
            catch (Exception)
            {
                loading_RefreshBranchesAllWithoutMain();
                catchError.Add("loading_RefreshBranchesAllWithoutMain");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefreshBranchesAllWithoutMain"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_RefreshByBranchandUser()
        {
            try
            {
                await FillCombo.RefreshByBranchandUser();
            }
            catch (Exception)
            {
                loading_RefreshByBranchandUser();
                catchError.Add("loading_RefreshByBranchandUser");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefreshByBranchandUser"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_RefreshCategory()
        {
            try
            {
                await FillCombo.RefreshCategory();
            }
            catch (Exception)
            {
                loading_RefreshCategory();
                catchError.Add("loading_RefreshCategory");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefreshCategory"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_RefreshUnit()
        {
            try
            {
                FillCombo.unitsList = await FillCombo.RefreshUnit();
            }
            catch (Exception)
            {
                loading_RefreshUnit();
                catchError.Add("loading_RefreshUnit");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefreshUnit"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_RefreshVendors()
        {
            try
            {
                await FillCombo.RefreshVendors();
            }
            catch (Exception)
            {
                loading_RefreshVendors();
                catchError.Add("loading_RefreshVendors");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefreshVendors"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_RefreshCards()
        {
            try
            {
                await FillCombo.RefreshCards();
            }
            catch (Exception)
            {
                loading_RefreshCards();
                catchError.Add("loading_RefreshCards");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefreshCards"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        #endregion
        async void loading_getUserPath()
        {
            #region get user path
            try
            {
                UserSetValues uSetValueModel = new UserSetValues();
                List<UserSetValues> lst = await uSetValueModel.GetAll();

                SetValues setValueModel = new SetValues();

                List<SetValues> setVLst = await setValueModel.GetBySetName("user_path");
                if (setVLst.Count > 0)
                {
                    int defaultPathId = setVLst[0].valId;
                    AppSettings.defaultPath = lst.Where(u => u.valId == defaultPathId && u.userId == userLogin.userId).FirstOrDefault().notes;
                }
                else
                {
                    AppSettings.defaultPath = "";
                }
            }
            catch
            {
                //AppSettings.defaultPath = "";
                loading_getUserPath();
                catchError.Add("loading_getUserPath");
                catchErrorCount++;
            }
            #endregion
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getUserPath"))
                {
                    item.value = true;
                    break;
                }
            }
        }


        async void loading_getDefaultInvoiceType()
        {
            try
            {
                //List<UserSetValues> lst = await userSetValuesModel.GetAll();
                //int defaulInvType;
                //int settingId;

                SetValues invSet = new SetValues();
                invSet = await v.GetBySetNameAndUserId("invType", MainWindow.userLogin.userId);
                if (invSet != null)
                {
                    //defaulInvType = invSet.valId;
                    //settingId = (int)invSet.settingId;
                    //notes = invSet.notes;
                    //try
                    //{
                    //defaultInvTypeSetValue = lst.Where(u => u.valId == defaulInvType && u.userId == MainWindow.userLogin.userId).FirstOrDefault();
                    AppSettings.invType = invSet.value;
                    //}
                    //catch { }
                }
                else
                {
                    AppSettings.invType = "clothes";
                }
            }
            catch
            {
                //AppSettings.invType = "clothes";
                loading_getDefaultInvoiceType();
                catchError.Add("loading_getDefaultInvoiceType");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDefaultInvoiceType"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        async void loading_getStatusesOfPreparingOrder()
        {
            try
            {
                SetValues invSet = new SetValues();
                List<SetValues> settingsValues = await AppSettings.valueModel.GetBySetName("statusesOfPreparingOrder");
                invSet = settingsValues.FirstOrDefault();
                if (invSet != null)
                    AppSettings.statusesOfPreparingOrder = invSet.value;
                else
                    AppSettings.statusesOfPreparingOrder = "directlyPrint";
            }
            catch
            {
                //AppSettings.statusesOfPreparingOrder = "directlyPrint";
                loading_getStatusesOfPreparingOrder();
                catchError.Add("loading_getStatusesOfPreparingOrder");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getStatusesOfPreparingOrder"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_typesOfService()
        {
            try
            {
                List<SetValues> serviceList = new List<SetValues>();

                // typesOfService
                SetValues clothesrow = new SetValues();
                SetValues carpetsrow = new SetValues();
                SetValues carsrow = new SetValues();

                string clothes;
                string carpets;
                string cars;

                serviceList = await AppSettings.valueModel.GetBySetvalNote("typesOfService");

                //try
                //{
                clothesrow = serviceList.Where(X => X.name == "typesOfService_clothes").FirstOrDefault();
                clothes = clothesrow.value;
                if (clothes == "1")
                {
                    AppSettings.typesOfService_clothes = "1";
                }
                else
                {
                    AppSettings.typesOfService_clothes = "0";
                }
                //}
                //catch
                //{
                //    // don't move this debug
                //    AppSettings.typesOfService_clothes = "1";
                //}
                //try
                //{
                carpetsrow = serviceList.Where(X => X.name == "typesOfService_cars").FirstOrDefault();
                carpets = carpetsrow.value;
                if (carpets == "1")
                {
                    AppSettings.typesOfService_carpets = "1";
                }
                else
                {
                    AppSettings.typesOfService_carpets = "0";
                }
                //}
                //catch
                //{
                //    // don't move this debug
                //    AppSettings.typesOfService_carpets = "1";
                //}
                //try
                //{
                carsrow = serviceList.Where(X => X.name == "typesOfService_carpets").FirstOrDefault();
                cars = carsrow.value;
                //
                if (cars == "1")
                {
                    AppSettings.typesOfService_cars = "1";
                }
                else
                {
                    AppSettings.typesOfService_cars = "0";
                }
                //}
                //catch
                //{
                //    // don't move this debug
                //    AppSettings.typesOfService_cars = "1";
                //}


            }
            catch
            {
                loading_typesOfService();
                //    // don't move this debug
                //AppSettings.typesOfService_clothes = "1";
                //AppSettings.typesOfService_carpets = "1";
                //AppSettings.typesOfService_cars = "1";
                catchError.Add("loading_typesOfService");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_typesOfService"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_maxDiscount()
        {
            try
            {
                SetValues maxDiscount = new SetValues();
                List<SetValues> settingsValues = await AppSettings.valueModel.GetBySetName("maxDiscount");
                maxDiscount = settingsValues.FirstOrDefault();
                AppSettings.maxDiscount = decimal.Parse(maxDiscount.value);
            }
            catch
            {
                //AppSettings.maxDiscount = 0;
                loading_maxDiscount();
                catchError.Add("loading_maxDiscount");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_maxDiscount"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getItemCost()
        {
            //get item cost
            try
            {
                AppSettings.itemCost = int.Parse(await getDefaultItemCost());
            }
            catch
            {
                //AppSettings.itemCost = 0;
                loading_getItemCost();
                catchError.Add("loading_getItemCost");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getItemCost"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        //async void loading_getPrintCount()
        //{
        //    //get print count
        //    try
        //    {
        //        AppSettings.Allow_print_inv_count = await getDefaultPrintCount();
        //    }
        //    catch
        //    {
        //        AppSettings.Allow_print_inv_count = "1";
        //    }
        //    foreach (var item in loadingList)
        //    {
        //        if (item.key.Equals("loading_getPrintCount"))
        //        {
        //            item.value = true;
        //            break;
        //        }
        //    }
        //}
        async void loading_getTaxDetails()
        {
            try
            {

                // List<SettingCls> settingsCls = await setModel.GetAll();
                //  List<SetValues> settingsValues = await valueModel.GetAll();
                List<SetValues> settingsValues = await AppSettings.valueModel.GetBySetvalNote("tax");
                //    SettingCls set = new SettingCls();
                SetValues setV = new SetValues();

                #region get invoice tax bool
                //get invoice tax bool
                //   set = settingsCls.Where(s => s.name == "invoiceTax_bool").FirstOrDefault<SettingCls>();
                setV = settingsValues.Where(i => i.name == "invoiceTax_bool").FirstOrDefault();
                if (setV != null)
                    AppSettings.invoiceTax_bool = bool.Parse(setV.value);
                else
                    AppSettings.invoiceTax_bool = false;

                #endregion

                #region  get invoice tax decimal
                //get invoice tax decimal
                //  set = settingsCls.Where(s => s.name == "invoiceTax_decimal").FirstOrDefault<SettingCls>();

                setV = settingsValues.Where(i => i.name == "invoiceTax_decimal").FirstOrDefault();
                if (setV != null)
                    AppSettings.invoiceTax_decimal = decimal.Parse(setV.value);
                else
                    AppSettings.invoiceTax_decimal = 0;
                #endregion

                #region  get item tax bool
                //get item tax bool
                //  set = settingsCls.Where(s => s.name == "itemsTax_bool").FirstOrDefault<SettingCls>();// itemsTax_bool
                setV = settingsValues.Where(i => i.name == "itemsTax_bool").FirstOrDefault();
                if (setV != null)
                    AppSettings.itemsTax_bool = bool.Parse(setV.value);
                else
                    AppSettings.itemsTax_bool = false;

                #endregion

                #region get item tax decimal

                ////get item tax decimal
                //set = settingsCls.Where(s => s.name == "itemsTax_decimal").FirstOrDefault<SettingCls>();
                //setV = settingsValues.Where(i => i.settingId == set.settingId).FirstOrDefault();
                //if (setV != null)
                //    itemsTax_decimal = decimal.Parse(setV.value);
                //else
                //    itemsTax_decimal = 0;

                #endregion

            }
            catch (Exception)
            {
                loading_getTaxDetails();
                //AppSettings.invoiceTax_bool = false;
                //AppSettings.invoiceTax_decimal = 0;
                //AppSettings.itemsTax_bool = false;
                //AppSettings.itemsTax_decimal = 0;
                catchError.Add("loading_getTaxDetails");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getTaxDetails"))
                {
                    item.value = true;
                    break;
                }
            }

        }
        public async void loading_getDefaultSystemInfo()
        {
            try
            {
                List<SettingCls> settingsCls = await AppSettings.setModel.GetAll();
                List<SetValues> settingsValues = await AppSettings.valueModel.GetAll();
                SettingCls set = new SettingCls();
                SetValues setV = new SetValues();
                List<char> charsToRemove = new List<char>() { '@', '_', ',', '.', '-' };
                #region get company name
                Thread t1 = new Thread(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //get company name
                        set = settingsCls.Where(s => s.name == "com_name").FirstOrDefault<SettingCls>();
                        AppSettings.nameId = set.settingId;
                        setV = settingsValues.Where(i => i.settingId == AppSettings.nameId).FirstOrDefault();
                        if (setV != null)
                            AppSettings.companyName = setV.value;

                    });
                });
                t1.Start();
                #endregion

                #region  get company address
                Thread t2 = new Thread(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //get company address
                        set = settingsCls.Where(s => s.name == "com_address").FirstOrDefault<SettingCls>();
                        AppSettings.addressId = set.settingId;
                        setV = settingsValues.Where(i => i.settingId == AppSettings.addressId).FirstOrDefault();
                        if (setV != null)
                            AppSettings.Address = setV.value;
                    });
                });
                t2.Start();
                #endregion

                #region  get company email
                Thread t3 = new Thread(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //get company email
                        set = settingsCls.Where(s => s.name == "com_email").FirstOrDefault<SettingCls>();
                        AppSettings.emailId = set.settingId;
                        setV = settingsValues.Where(i => i.settingId == AppSettings.emailId).FirstOrDefault();
                        if (setV != null)
                            AppSettings.Email = setV.value;
                    });
                });
                t3.Start();
                #endregion

                #region  get company mobile
                Thread t4 = new Thread(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //get company mobile
                        set = settingsCls.Where(s => s.name == "com_mobile").FirstOrDefault<SettingCls>();
                        AppSettings.mobileId = set.settingId;
                        setV = settingsValues.Where(i => i.settingId == AppSettings.mobileId).FirstOrDefault();
                        if (setV != null)
                        {
                            charsToRemove.ForEach(x => setV.value = setV.value.Replace(x.ToString(), String.Empty));
                            AppSettings.Mobile = setV.value;
                        }
                    });
                });
                t4.Start();
                #endregion

                #region  get company phone
                Thread t5 = new Thread(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //get company phone
                        set = settingsCls.Where(s => s.name == "com_phone").FirstOrDefault<SettingCls>();
                        AppSettings.phoneId = set.settingId;
                        setV = settingsValues.Where(i => i.settingId == AppSettings.phoneId).FirstOrDefault();
                        if (setV != null)
                        {
                            charsToRemove.ForEach(x => setV.value = setV.value.Replace(x.ToString(), String.Empty));
                            AppSettings.Phone = setV.value;
                        }
                    });
                });
                t5.Start();
                #endregion

                #region  get company fax
                Thread t6 = new Thread(() =>
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        //get company fax
                        set = settingsCls.Where(s => s.name == "com_fax").FirstOrDefault<SettingCls>();
                        AppSettings.faxId = set.settingId;
                        setV = settingsValues.Where(i => i.settingId == AppSettings.faxId).FirstOrDefault();
                        if (setV != null)
                        {
                            charsToRemove.ForEach(x => setV.value = setV.value.Replace(x.ToString(), String.Empty));
                            AppSettings.Fax = setV.value;
                        }
                    });
                });
                t6.Start();
                #endregion

                #region   get company logo
                //get company logo
                set = settingsCls.Where(s => s.name == "com_logo").FirstOrDefault<SettingCls>();
                AppSettings.logoId = set.settingId;
                setV = settingsValues.Where(i => i.settingId == AppSettings.logoId).FirstOrDefault();
                if (setV != null)
                {
                    AppSettings.logoImage = setV.value;
                    await setV.getImg(AppSettings.logoImage);
                }
                #endregion
            }
            catch (Exception)
            {
                loading_getDefaultSystemInfo();
                catchError.Add("loading_getDefaultSystemInfo");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDefaultSystemInfo"))
                {
                    item.value = true;
                    break;
                }
            }

        }
        async void loading_getDateForm()
        {
            //get dateform
            try
            {
                AppSettings.dateFormat = await getDefaultDateForm();
            }
            catch
            {
                //AppSettings.dateFormat = "ShortDatePattern";
                loading_getDateForm();
                catchError.Add("loading_getDateForm");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getDateForm"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getRegionAndCurrency()
        {
            //get region and currency
            try
            {
                CountryCode c = await getDefaultRegion();
                AppSettings.Region = c;
                AppSettings.Currency = c.currency;
                AppSettings.CurrencyId = c.currencyId;
                txt_cashSympol.Text = AppSettings.Currency;

            }
            catch
            {
                loading_getRegionAndCurrency();
                catchError.Add("loading_getRegionAndCurrency");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getRegionAndCurrency"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getStorageCost()
        {
            //get storage cost
            try
            {
                AppSettings.StorageCost = decimal.Parse(await getDefaultStorageCost());
            }
            catch
            {
                //AppSettings.StorageCost = 0;
                loading_getStorageCost();
                catchError.Add("loading_getStorageCost");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getStorageCost"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getAccurac()
        {
            //get accuracy
            try
            {
                AppSettings.accuracy = await getDefaultAccuracy();
            }
            catch
            {
                //AppSettings.accuracy = "1";
                loading_getAccurac();
                catchError.Add("loading_getAccurac");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getAccurac"))
                {
                    item.value = true;
                    break;
                }
            }
        }


        async void loading_getprintSitting()
        {
            try
            {
                await getprintSitting();
            }
            catch (Exception)
            {
                loading_getprintSitting();
                catchError.Add("loading_getprintSitting");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getprintSitting"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_POSList()
        {
            try
            {
                AppSettings.posList = await posLogin.Get();
            }
            catch (Exception)
            {
                loading_POSList();
                catchError.Add("loading_POSList");
                catchErrorCount++;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_POSList"))
                {
                    item.value = true;
                    break;
                }
            }
        }
        async void loading_getTableTimes()
        {
            try
            {
                SetValues setV = new SetValues();

                #region time_staying
                SetValues time_stayingSetValues = new SetValues();
                List<SetValues> settingsValues1 = await AppSettings.valueModel.GetBySetName("time_staying");
                time_stayingSetValues = settingsValues1.FirstOrDefault();
                if (time_stayingSetValues != null)
                    AppSettings.time_staying = double.Parse(time_stayingSetValues.value);
                else
                    AppSettings.time_staying = 0;
                #endregion

                #region warningTimeForLateReservation
                SetValues warningTimeForLateReservationSetValues = new SetValues();
                List<SetValues> settingsValues2 = await AppSettings.valueModel.GetBySetName("warningTimeForLateReservation");
                warningTimeForLateReservationSetValues = settingsValues2.FirstOrDefault();
                if (warningTimeForLateReservationSetValues != null)
                    AppSettings.warningTimeForLateReservation = int.Parse(warningTimeForLateReservationSetValues.value);
                else
                    AppSettings.warningTimeForLateReservation = 0;
                #endregion

                #region maximumTimeToKeepReservation
                SetValues maximumTimeToKeepReservationSetValues = new SetValues();
                List<SetValues> settingsValues3 = await AppSettings.valueModel.GetBySetName("maximumTimeToKeepReservation");
                maximumTimeToKeepReservationSetValues = settingsValues3.FirstOrDefault();
                if (maximumTimeToKeepReservationSetValues != null)
                    AppSettings.maximumTimeToKeepReservation = double.Parse(maximumTimeToKeepReservationSetValues.value);
                else
                    AppSettings.maximumTimeToKeepReservation = 0;
                #endregion

            }
            catch (Exception)
            {
                loading_getTableTimes();
                catchError.Add("loading_getTableTimes");
                catchErrorCount++;
                //AppSettings.time_staying = 0;
                //AppSettings.warningTimeForLateReservation = 0;
                //AppSettings.maximumTimeToKeepReservation = 0;
            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_getTableTimes"))
                {
                    item.value = true;
                    break;
                }
            }

        }
        #endregion
        #region get setting
        SetValues v = new SetValues();
        async Task<string> getDefaultStorageCost()
        {
            v = await uc_general.getDefaultCost();
            if (v != null)
                return v.value;
            else
                return "";
        }
        async Task<List<string>> getDefaultTaxList()
        {
            List<string> taxLst = await uc_general.getDefaultTaxList();
            if (taxLst == null)
                taxLst = new List<string>() { "false", "0", "false", "0" };
            return taxLst;
        }
        async Task<string> getDefaultItemCost()
        {
            v = await uc_general.getDefaultItemCost();
            if (v != null)
                return v.value;
            else
                return "";
        }
        //async Task<string> getDefaultPrintCount()
        //{
        //    v = await uc_general.getDefaultPrintCount();
        //    if (v != null)
        //        return v.value;
        //    else
        //        return "";
        //}
        async Task<string> getDefaultAccuracy()
        {
            v = await uc_general.getDefaultAccuracy();
            if (v != null)
                return v.value;
            else
                return "";
        }
        UserSetValues userSetValuesModel = new UserSetValues();

       
        async Task<string> getDefaultDateForm()
        {
            v = await uc_general.getDefaultDateForm();
            if (v != null)
                return v.value;
            else
                return "";
        }
        async Task<CountryCode> getDefaultRegion()
        {
            CountryCode c = await uc_general.getDefaultRegion();
            if (c != null)
                return c;
            else
                return null;
        }
        public static async Task getprintSitting()
        {
            await Getprintparameter();
            await GetReportlang();
            await getPrintersNames();
        }
        public static async Task Getprintparameter()
        {
            List<SetValues> printList = new List<SetValues>();
            printList = await AppSettings.valueModel.GetBySetvalNote("print");
            AppSettings.sale_copy_count = printList.Where(X => X.name == "sale_copy_count").FirstOrDefault().value;

            AppSettings.pur_copy_count = printList.Where(X => X.name == "pur_copy_count").FirstOrDefault().value;

            AppSettings.print_on_save_sale = printList.Where(X => X.name == "print_on_save_sale").FirstOrDefault().value;

            AppSettings.print_on_save_pur = printList.Where(X => X.name == "print_on_save_pur").FirstOrDefault().value;

            AppSettings.email_on_save_sale = printList.Where(X => X.name == "email_on_save_sale").FirstOrDefault().value;

            AppSettings.email_on_save_pur = printList.Where(X => X.name == "email_on_save_pur").FirstOrDefault().value;

            AppSettings.sale_copy_count = printList.Where(X => X.name == "sale_copy_count").FirstOrDefault().value;

            AppSettings.pur_copy_count = printList.Where(X => X.name == "pur_copy_count").FirstOrDefault().value;

            AppSettings.rep_print_count = printList.Where(X => X.name == "rep_copy_count").FirstOrDefault().value;

            AppSettings.Allow_print_inv_count = printList.Where(X => X.name == "Allow_print_inv_count").FirstOrDefault().value;
            AppSettings.show_header = printList.Where(X => X.name == "show_header").FirstOrDefault().value;


            AppSettings.print_kitchen_on_sale = printList.Where(X => X.name == "print_kitchen_on_sale").FirstOrDefault().value;
            AppSettings.print_kitchen_on_preparing = printList.Where(X => X.name == "print_kitchen_on_preparing").FirstOrDefault().value;


            if (AppSettings.show_header == null || AppSettings.show_header == "")
            {
                AppSettings.show_header = "1";
            }
           AppSettings.itemtax_note = printList.Where(X => X.name == "itemtax_note").FirstOrDefault().value;
           AppSettings.sales_invoice_note = printList.Where(X => X.name == "sales_invoice_note").FirstOrDefault().value;
           AppSettings.print_on_save_directentry = printList.Where(X => X.name == "print_on_save_directentry").FirstOrDefault().value;
           AppSettings.directentry_copy_count = printList.Where(X => X.name == "directentry_copy_count").FirstOrDefault().value;
            AppSettings.kitchen_copy_count = printList.Where(X => X.name == "kitchen_copy_count").FirstOrDefault().value;

         
        }
        public static async Task GetReportlang()
        {
            List<SetValues> replangList = new List<SetValues>();
            replangList = await AppSettings.valueModel.GetBySetName("report_lang");
            AppSettings.Reportlang = replangList.Where(r => r.isDefault == 1).FirstOrDefault().value;

        }
        public static async Task getPrintersNames()
        {

            AppSettings.posSetting = new PosSetting();

            AppSettings.posSetting = await AppSettings.posSetting.GetByposId((int)MainWindow.posLogin.posId);
            AppSettings.posSetting = AppSettings.posSetting.MaindefaultPrinterSetting(AppSettings.posSetting);
            // report
            if (AppSettings.posSetting.repname is null || AppSettings.posSetting.repname == "")
            {
                AppSettings.rep_printer_name = "";
            }
            else
            {
                AppSettings.rep_printer_name = Encoding.UTF8.GetString(Convert.FromBase64String(AppSettings.posSetting.repname));
            }
            //sale
            if (AppSettings.posSetting.salname is null || AppSettings.posSetting.salname == "")
            {
                AppSettings.posSetting.salname = "";
            }
            else
            {
                AppSettings.sale_printer_name = Encoding.UTF8.GetString(Convert.FromBase64String(AppSettings.posSetting.salname));
            }
            //kitchen
            if (AppSettings.posSetting.kitchenPrinter is null || AppSettings.posSetting.kitchenPrinter == "")
            {
                AppSettings.posSetting.kitchenPrinter = "";
            }
            else
            {
                AppSettings.kitchen_printer_name = Encoding.UTF8.GetString(Convert.FromBase64String(AppSettings.posSetting.kitchenPrinter));
            }
            AppSettings.salePaperSize = AppSettings.posSetting.saleSizeValue;
            AppSettings.docPapersize = AppSettings.posSetting.docPapersize;

            AppSettings.kitchenPaperSize = AppSettings.posSetting.kitchenSizeValue;
        }
        #endregion
        public static int _CachTransfersCount = 0;
        string deliveryPermission = "salesOrders_delivery";

        public async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                    HelpClass.StartAwait(grid_mainWindow , "mainWindow_loaded");
                #region bonni
#pragma warning disable CS0436 // Type conflicts with imported type
                TabTipAutomation.IgnoreHardwareKeyboard = HardwareKeyboardIgnoreOptions.IgnoreAll;
                #pragma warning restore CS0436 // Type conflicts with imported type
                #pragma warning disable CS0436 // Type conflicts with imported type
                #pragma warning restore CS0436 // Type conflicts with imported type
                #pragma warning disable CS0436 // Type conflicts with imported type
                TabTipAutomation.ExceptionCatched += TabTipAutomationOnTest;
                #pragma warning restore CS0436 // Type conflicts with imported type
                this.Height = SystemParameters.MaximizedPrimaryScreenHeight;
                //this.Width = SystemParameters.MaximizedPrimaryScreenHeight;
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();

                // idle timer
                idletimer = new DispatcherTimer();
                idletimer.Interval = TimeSpan.FromMinutes(Idletime);
                idletimer.Tick += timer_Idle;
                idletimer.Start();


                //thread
                threadtimer = new DispatcherTimer();
                threadtimer.Interval = TimeSpan.FromSeconds(threadtime);
                threadtimer.Tick += timer_Thread;
                threadtimer.Start();




                #endregion
                translate();


               
                #region loading 
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_listObjects", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getGroupObjects", value = false });
                loadingList.Add(new keyValueBool { key = "loading_globalItemUnitsList", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshBranches", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshBranchesAllWithoutMain", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshByBranchandUser", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshCategory", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshUnit", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshVendors", value = false });
                loadingList.Add(new keyValueBool { key = "loading_RefreshCards", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getUserPersonalInfo", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getUserPath", value = false });

                loadingList.Add(new keyValueBool { key = "loading_getItemCost", value = false });
              //  loadingList.Add(new keyValueBool { key = "loading_getPrintCount", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getTaxDetails", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getDefaultSystemInfo", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getDateForm", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getRegionAndCurrency", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getStorageCost", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getAccurac", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getprintSitting", value = false });
                loadingList.Add(new keyValueBool { key = "loading_POSList", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getTableTimes", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getDefaultInvoiceType", value = false });
                loadingList.Add(new keyValueBool { key = "loading_getStatusesOfPreparingOrder", value = false });
                loadingList.Add(new keyValueBool { key = "loading_typesOfService", value = false });
                loadingList.Add(new keyValueBool { key = "loading_maxDiscount", value = false });
                loadingList.Add(new keyValueBool { key = "loading_itemUnitsUsersList", value = false });



                loading_listObjects();
                loading_getGroupObjects();
                loading_globalItemUnitsList();
                loading_RefreshBranches();
                loading_RefreshBranchesAllWithoutMain();
                loading_RefreshByBranchandUser();
                loading_RefreshCategory();
                loading_RefreshUnit();
                loading_RefreshVendors();
                loading_RefreshCards();
                loading_getUserPersonalInfo();
                loading_getUserPath();

                loading_getItemCost();
              //  loading_getPrintCount();
                loading_getTaxDetails();
                loading_getDefaultSystemInfo();
                loading_getDateForm();
                loading_getRegionAndCurrency();
                loading_getStorageCost();
                loading_getAccurac();
                loading_getprintSitting();
                loading_POSList();
                loading_getTableTimes();
                loading_getDefaultInvoiceType();
                loading_getStatusesOfPreparingOrder();
                loading_typesOfService();
                loading_maxDiscount();
                loading_itemUnitsUsersList();

               
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
                //MessageBox.Show(catchError + " and count: " + catchErrorCount);
                #endregion

                #region notifications 
                setNotifications();
                setTimer();
                #endregion

                #region Permision
                permission();
                //if (FillCombo.groupObject.HasPermissionAction(deliveryPermission, FillCombo.groupObjects, "one"))
                if (userLogin.job == "deliveryManager" || userLogin.job == "deliveryEmployee" || HelpClass.isAdminPermision())
                    md_deliveryWaitConfirmUser.Visibility = Visibility.Visible;
                else
                    md_deliveryWaitConfirmUser.Visibility = Visibility.Collapsed;
                #endregion


                //SelectAllText
                EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText));
                txt_rightReserved.Text = DateTime.Now.Date.Year + " © All Right Reserved for Increase";




                HelpClass.EndAwait(grid_mainWindow, "mainWindow_loaded");
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = sender as System.Windows.Controls.TextBox;
            if (textBox != null)
                if (!textBox.IsReadOnly)
                    textBox.SelectAll();
        }
        public static bool loadingDefaultPath(string defaultPath)
        {
            string first, last;
            bool load = false;
            if (!string.IsNullOrEmpty(defaultPath))
            {
                if (FillCombo.groupObject.HasPermission(defaultPath, FillCombo.groupObjects))
                {
                    first = FillCombo.objectModel.GetParents(FillCombo.objectsList, defaultPath).FirstOrDefault().name;
                    last = defaultPath;

                    //MainWindow.mainWindow.Btn_purchase_Click(MainWindow.mainWindow.btn_purchase, null);
                    foreach (Button button in FindControls.FindVisualChildren<Button>(MainWindow.mainWindow))
                    {
                        if (button.Tag != null)
                            if (button.Tag.ToString() == first )
                            {
                                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                break;
                            }
                    }

                    MainWindow.mainWindow.initializationMainTrack(last);
                    MainWindow.mainWindow.loadPath(last);
                    load = true;
                }
            }
            return load;
        }

        void permission()
        {
            bool loadWindow = false;
            loadWindow = loadingDefaultPath(AppSettings.defaultPath);
            if (!HelpClass.isAdminPermision())
                foreach (Button button in FindControls.FindVisualChildren<Button>(this))
                {
                    if (button.Tag != null)
                        if (FillCombo.groupObject.HasPermission(button.Tag.ToString(), FillCombo.groupObjects))
                        {
                            button.Visibility = Visibility.Visible;
                            if (!loadWindow)
                            {
                                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                loadWindow = true;
                            }
                        }
                        else button.Visibility = Visibility.Collapsed;
                }
            else
            if (!loadWindow)
                Btn_home_Click(btn_home, null);
        }
        #region notifications
        Invoice invoice = new Invoice();
        int _OrdersWaitCount = 0;
        int _NotCount = 0;
        private void setTimer()
        {
            notTimer = new DispatcherTimer();
            notTimer.Interval = TimeSpan.FromSeconds(30);
            notTimer.Tick += notTimer_Tick;
            notTimer.Start();
        }
        private void notTimer_Tick(object sendert, EventArgs et)
        {
            try
            {
                setNotifications();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }



        }
        private async void setNotifications()
        {
            try
            {
                await refreshNotificationCount();
                await setCashTransferNotification();
                await refreshOrdersWaitNotification();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task refreshNotificationCount()
        {
            int notCount = await notificationUser.GetCountByUserId(userLogin.userId, "alert", posLogin.posId);

            //int previouseCount = 0;
            //if (md_notificationCount.Badge != null && md_notificationCount.Badge.ToString() != "") previouseCount = int.Parse(md_notificationCount.Badge.ToString());

            if (notCount != _NotCount)
            {
                if (notCount > 9)
                {
                    notCount = 9;
                    md_notificationCount.Badge = "+" + notCount.ToString();
                }
                else if (notCount == 0) md_notificationCount.Badge = "";
                else
                    md_notificationCount.Badge = notCount.ToString();
            }
            _NotCount = notCount;
        }
        private async Task refreshOrdersWaitNotification()
        {
            try
            {
                int ordersCount = await invoice.getDeliverOrdersCount( MainWindow.userLogin.userId);

                if (ordersCount != _OrdersWaitCount)
                {
                    if (ordersCount > 9)
                    {
                        md_deliveryWaitConfirmUser.Badge = "+9";
                    }
                    else if (ordersCount == 0) md_deliveryWaitConfirmUser.Badge = "";
                    else
                        md_deliveryWaitConfirmUser.Badge = ordersCount.ToString();
                }
                _OrdersWaitCount = ordersCount;
            }
            catch { }
        }
        private async Task setCashTransferNotification()
        {
            try
            {
                #region get cachtransfers for current pos
                CashTransfer cashModel = new CashTransfer();
                IEnumerable<CashTransfer> cashesQuery;
                cashesQuery = await cashModel.GetCashTransferForPosById("all", "p", (int)MainWindow.posLogin.posId);
                cashesQuery = cashesQuery.Where(c => c.posId == MainWindow.posLogin.posId && c.isConfirm == 0);
                int posCachTransfers = cashesQuery.Count();
                #endregion

                HelpClass.refreshNotification(md_transfers, ref _CachTransfersCount, posCachTransfers);
            }
            catch (Exception ex)
            {
                //SectionData.ExceptionMessage(ex, this);
            }
        }
        #endregion
        void timer_Idle(object sender, EventArgs e)
        {

            try
            {
                if (IdleClass.IdleTime.Minutes >= Idletime)
                {
                    go_out_didNotAnyProcess = true;
                    BTN_logOut_Click(BTN_logOut, null);
                    idletimer.Stop();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        async void timer_Thread(object sendert, EventArgs et)
        {
            try
            {
                //  User thruser = new User();
                //UsersLogs thrlog = new UsersLogs();

                //thrlog = await thrlog.GetByID((int)userLogInID);
                // check go_out == true do logout()
                //if (thrlog.sOutDate != null)
                if (go_out)
                {
                    BTN_logOut_Click(BTN_logOut, null);
                    threadtimer.Stop();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }


            try
            {

                posLogin = await posLogin.getById(posLogin.posId);
                txt_cashValue.Text = HelpClass.DecTostring(posLogin.balance);
                txt_cashSympol.Text = AppSettings.Currency;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }


        }
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {

                txtTime.Text = DateTime.Now.ToShortTimeString();
                txtDate.Text = DateTime.Now.ToShortDateString();


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void TabTipAutomationOnTest(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
        
        void FN_tooltipVisibility(Button btn)
        {
            ToolTip T = btn.ToolTip as ToolTip;
            if (T.Visibility == Visibility.Visible)
                T.Visibility = Visibility.Hidden;
            else T.Visibility = Visibility.Visible;
        }
        private async void BTN_logOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                HelpClass.StartAwait(grid_mainWindow);

                await close();

                //Application.Current.Shutdown();
                //System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);

                wd_logIn wdLogIn = new wd_logIn();
                wdLogIn.Show();
                this.Close();

                HelpClass.EndAwait(grid_mainWindow);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task close()
        {
            //log out
            //update lognin record
            if (!go_out)
            {
                await updateLogninRecord();
            }
            timer.Stop();
            idletimer.Stop();
            threadtimer.Stop();
        }
        async Task<bool> updateLogninRecord()
        {
            UsersLogs userLogModel = new UsersLogs();
            userLogModel = await userLogModel.GetByID(userLog.logId);
            //update user record
            userLogin.isOnline = 0;
            await userLogin.save(userLogin);

            //update lognin record
            await userLog.Save(userLogModel);

            return true;
        }
        private async void BTN_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_mainWindow);
                //if (go_out)
                //{
                //    await close();
                //    this.Visibility = Visibility.Hidden;
                //    #region
                //    Window.GetWindow(this).Opacity = 0.2;
                //    wd_messageBox w = new wd_messageBox();
                //    w.contentText2 = AppSettings.resourcemanager.GetString("trUserLoginFromOtherPos");
                //    w.ShowDialog();
                //    Window.GetWindow(this).Opacity = 1;
                //    #endregion

                //    Application.Current.Shutdown();
                //}
                //else if (go_out_didNotAnyProcess)
                //{
                //    await close();
                //    this.Visibility = Visibility.Hidden;
                //    #region
                //    Window.GetWindow(this).Opacity = 0.2;
                //    wd_messageBoxWithIcon w = new wd_messageBoxWithIcon();
                //    w.contentText1 = AppSettings.resourcemanager.GetString("trLoggedOutBecauseDidNotDoneAnyProcess");
                //    w.ShowDialog();
                //    Window.GetWindow(this).Opacity = 1;
                //    #endregion

                //    Application.Current.Shutdown();
                //}
                //else
                {
                    await close();
                    Application.Current.Shutdown();
                }


                HelpClass.EndAwait(grid_mainWindow);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void BTN_Minimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        void colorTextRefreash(TextBlock txt)
        {
            txt_home.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_catalog.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_storage.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_purchases.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_sales.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_kitchen.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_delivery.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_accounts.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_reports.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_sectiondata.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            txt_settings.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));

            txt.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
        }
        void fn_ColorIconRefreash(Path p)
        {
            path_iconSettings.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconSectionData.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconReports.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconAccounts.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconSales.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconKitchen.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconDelivery.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconPurchases.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconStorage.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconCatalog.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));
            path_iconHome.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#B1E9EA"));

            p.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
        }
        public void translate()
        {
            tt_menu.Content = AppSettings.resourcemanager.GetString("trMenu");
            tt_home.Content = AppSettings.resourcemanager.GetString("trHome");
            txt_home.Text = AppSettings.resourcemanager.GetString("trHome");
            tt_catalog.Content = AppSettings.resourcemanager.GetString("trCatalog");
            txt_catalog.Text = AppSettings.resourcemanager.GetString("trCatalog");
            tt_storage.Content = AppSettings.resourcemanager.GetString("trStore");
            txt_storage.Text = AppSettings.resourcemanager.GetString("trStore");
            tt_purchase.Content = AppSettings.resourcemanager.GetString("trPurchases");
            txt_purchases.Text = AppSettings.resourcemanager.GetString("trPurchases");
            tt_kitchen.Content = AppSettings.resourcemanager.GetString("trKitchen");
            txt_kitchen.Text = AppSettings.resourcemanager.GetString("trKitchen");
            tt_delivery.Content = AppSettings.resourcemanager.GetString("trDelivery");
            txt_delivery.Text = AppSettings.resourcemanager.GetString("trDelivery");
            tt_sales.Content = AppSettings.resourcemanager.GetString("trSales");
            txt_sales.Text = AppSettings.resourcemanager.GetString("trSales");
            tt_accounts.Content = AppSettings.resourcemanager.GetString("trAccounting");
            txt_accounts.Text = AppSettings.resourcemanager.GetString("trAccounting");
            tt_reports.Content = AppSettings.resourcemanager.GetString("trReports");
            txt_reports.Text = AppSettings.resourcemanager.GetString("trReports");
            tt_sectionData.Content = AppSettings.resourcemanager.GetString("trSectionData");
            txt_sectiondata.Text = AppSettings.resourcemanager.GetString("trSectionData");
            tt_settings.Content = AppSettings.resourcemanager.GetString("trSettings");
            txt_settings.Text = AppSettings.resourcemanager.GetString("trSettings");
            txt_cashTitle.Text = AppSettings.resourcemanager.GetString("trBalance");

            mi_changePassword.Header = AppSettings.resourcemanager.GetString("trChangePassword");
            mi_aboutUs.Header = AppSettings.resourcemanager.GetString("trAboutUs");
            BTN_logOut.Header = AppSettings.resourcemanager.GetString("trLogOut");

            txt_notifications.Text = AppSettings.resourcemanager.GetString("trNotifications");
            txt_noNoti.Text = AppSettings.resourcemanager.GetString("trNoNotifications");
            btn_showAll.Content = AppSettings.resourcemanager.GetString("trShowAll");


            BTN_Close.ToolTip = AppSettings.resourcemanager.GetString("trClose");
            BTN_Minimize.ToolTip = AppSettings.resourcemanager.GetString("minimize");
            btn_deliveryWaitConfirmUser.ToolTip = AppSettings.resourcemanager.GetString("trOrdersWaitConfirmUser");
            btn_transfers.ToolTip = AppSettings.resourcemanager.GetString("trDailyClosing");
            BTN_notifications.ToolTip = AppSettings.resourcemanager.GetString("trNotification");

        }

        //فتح
        private void BTN_Menu_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!menuState)
                {
                    Storyboard sb = this.FindResource("Storyboard1") as Storyboard;
                    sb.Begin();
                    menuState = true;
                }
                else
                {
                    Storyboard sb = this.FindResource("Storyboard2") as Storyboard;
                    sb.Begin();
                    menuState = false;
                }


                #region tooltipVisibility
                FN_tooltipVisibility(BTN_menu);
                FN_tooltipVisibility(btn_home);
                FN_tooltipVisibility(btn_catalog);
                FN_tooltipVisibility(btn_storage);
                FN_tooltipVisibility(btn_purchase);
                FN_tooltipVisibility(btn_sales);
                FN_tooltipVisibility(btn_reports);
                FN_tooltipVisibility(btn_accounts);
                FN_tooltipVisibility(btn_sectionData);
                FN_tooltipVisibility(btn_settings);
                #endregion


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        void fn_pathOpenCollapsed()
        {
            path_openCatalog.Visibility = Visibility.Collapsed;
            path_openStorage.Visibility = Visibility.Collapsed;
            path_openPurchases.Visibility = Visibility.Collapsed;
            path_openSales.Visibility = Visibility.Collapsed;
            path_openReports.Visibility = Visibility.Collapsed;
            path_openSectionData.Visibility = Visibility.Collapsed;
            path_openSettings.Visibility = Visibility.Collapsed;
            path_openHome.Visibility = Visibility.Collapsed;
            path_openAccounts.Visibility = Visibility.Collapsed;
            path_openKitchen.Visibility = Visibility.Collapsed;
            path_openDelivery.Visibility = Visibility.Collapsed;

        }
        void FN_pathVisible(Rectangle p)
        {
            fn_pathOpenCollapsed();
            p.Visibility = Visibility.Visible;
        }




        private void btn_Keyboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TabTip.Close())
                {
            #pragma warning disable CS0436 // Type conflicts with imported type
                    TabTip.OpenUndockedAndStartPoolingForClosedEvent();
            #pragma warning restore CS0436 // Type conflicts with imported type
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
       
        
        private void Btn_home_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                colorTextRefreash(txt_home);
                FN_pathVisible(path_openHome);
                fn_ColorIconRefreash(path_iconHome);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_home.Instance);
                if (isHome)
                {
                    uc_home.Instance.timerAnimation();
                    isHome = false;
                }
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void Btn_catalog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_catalog);
                FN_pathVisible(path_openCatalog);
                fn_ColorIconRefreash(path_iconCatalog);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_catalog.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public void Btn_purchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_purchases);
                FN_pathVisible(path_openPurchases);
                fn_ColorIconRefreash(path_iconPurchases);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_purchase.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public void Btn_storage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_storage);
                FN_pathVisible(path_openStorage);
                fn_ColorIconRefreash(path_iconStorage);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_storage.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        public void Btn_kitchen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_kitchen);
                FN_pathVisible(path_openKitchen);
                fn_ColorIconRefreash(path_iconKitchen);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_laundry.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public void Btn_sales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_sales);
                FN_pathVisible(path_openSales);
                fn_ColorIconRefreash(path_iconSales);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_sales.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_delivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_delivery);
                FN_pathVisible(path_openDelivery);
                fn_ColorIconRefreash(path_iconDelivery);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_delivery.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_accounts_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_accounts);
                FN_pathVisible(path_openAccounts);
                fn_ColorIconRefreash(path_iconAccounts);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_accounts.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_reports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_reports);
                FN_pathVisible(path_openReports);
                fn_ColorIconRefreash(path_iconReports);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_reports.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_settings);
                FN_pathVisible(path_openSettings);
                fn_ColorIconRefreash(path_iconSettings);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_settings.Instance);

                isHome = true;
                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_SectionData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                colorTextRefreash(txt_sectiondata);
                FN_pathVisible(path_openSectionData);
                fn_ColorIconRefreash(path_iconSectionData);
                grid_main.Children.Clear();
                grid_main.Children.Add(uc_sectionData.Instance);

                isHome = true;
                Button button = sender as Button;
                initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

      
        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_mainWindow);
                await close();
                Application.Current.Shutdown();

               
                    HelpClass.EndAwait(grid_mainWindow);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

     
        private  void Mi_changePassword_Click(object sender, RoutedEventArgs e)
        {//change password
            try
            {

                Window.GetWindow(this).Opacity = 0.2;
                wd_changePassword w = new wd_changePassword();
                // w.ShowInTaskbar = false;
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
                
                //userLogin = await FillCombo.user.getUserById(w.userID);

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Mi_aboutUs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                wd_info w = new wd_info();
                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
     
        public static string GetUntilOrEmpty(string text, string stopAt)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
        #region  Notification
        List<NotificationUser> notifications;
        NotificationUser notificationUser = new NotificationUser();
        private async void BTN_notifications_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bdrMain.Visibility == Visibility.Collapsed)
                {
                    bdrMain.Visibility = Visibility.Visible;
                    bdrMain.RenderTransform = Animations.borderAnimation(-25, bdrMain, true);
                    notifications = await notificationUser.GetByUserId(userLogin.userId, "alert", posLogin.posId);
                    IEnumerable<NotificationUser> orderdNotifications = notifications.OrderByDescending(x => x.createDate);
                    await notificationUser.setAsRead(userLogin.userId, posLogin.posId, "alert");
                    md_notificationCount.Badge = "";
                    if (notifications.Count == 0)
                    {
                        grd_notifications.Visibility = Visibility.Collapsed;
                        txt_noNoti.Visibility = Visibility.Visible;
                    }

                    else
                    {
                        grd_notifications.Visibility = Visibility.Visible;
                        txt_noNoti.Visibility = Visibility.Collapsed;

                        txt_firstNotiTitle.Text = AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.title).FirstOrDefault());

                        txt_firstNotiContent.Text = GetUntilOrEmpty(orderdNotifications.Select(obj => obj.ncontent).FirstOrDefault(), ":")
                          + " : " +
                          AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.ncontent).FirstOrDefault().Substring(orderdNotifications.Select(obj => obj.ncontent).FirstOrDefault().LastIndexOf(':') + 1));

                        txt_firstNotiDate.Text = orderdNotifications.Select(obj => obj.createDate).FirstOrDefault().ToString();

                        if (notifications.Count > 1)
                        {
                            txt_2NotiTitle.Text = AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.title).Skip(1).FirstOrDefault());
                            txt_2NotiContent.Text = GetUntilOrEmpty(orderdNotifications.Select(obj => obj.ncontent).Skip(1).FirstOrDefault(), ":")
                          + " : " + AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.ncontent).Skip(1).FirstOrDefault().Substring(orderdNotifications.Select(obj => obj.ncontent).Skip(1).FirstOrDefault().LastIndexOf(':') + 1));

                            txt_2NotiDate.Text = orderdNotifications.Select(obj => obj.createDate).Skip(1).FirstOrDefault().ToString();

                        }
                        if (notifications.Count > 2)
                        {
                            txt_3NotiTitle.Text = AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.title).Skip(2).FirstOrDefault());
                            txt_3NotiContent.Text = GetUntilOrEmpty(orderdNotifications.Select(obj => obj.ncontent).Skip(2).FirstOrDefault(), ":")
                          + " : " + AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.ncontent).Skip(2).FirstOrDefault().Substring(orderdNotifications.Select(obj => obj.ncontent).Skip(2).FirstOrDefault().LastIndexOf(':') + 1));

                            txt_3NotiDate.Text = orderdNotifications.Select(obj => obj.createDate).Skip(2).FirstOrDefault().ToString();

                        }
                        if (notifications.Count > 3)
                        {
                            txt_4NotiTitle.Text = AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.title).Skip(3).FirstOrDefault());
                            txt_4NotiContent.Text = GetUntilOrEmpty(orderdNotifications.Select(obj => obj.ncontent).Skip(3).FirstOrDefault(), ":")
                          + " : " + AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.ncontent).Skip(3).FirstOrDefault().Substring(orderdNotifications.Select(obj => obj.ncontent).Skip(3).FirstOrDefault().LastIndexOf(':') + 1));

                            txt_4NotiDate.Text = orderdNotifications.Select(obj => obj.createDate).Skip(3).FirstOrDefault().ToString();

                        }
                        if (notifications.Count > 4)
                        {
                            txt_5NotiTitle.Text = AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.title).Skip(4).FirstOrDefault());
                            txt_5NotiContent.Text = GetUntilOrEmpty(orderdNotifications.Select(obj => obj.ncontent).Skip(4).FirstOrDefault(), ":")
                          + " : " + AppSettings.resourcemanager.GetString(orderdNotifications.Select(obj => obj.ncontent).Skip(4).FirstOrDefault().Substring(orderdNotifications.Select(obj => obj.ncontent).Skip(4).FirstOrDefault().LastIndexOf(':') + 1));

                            txt_5NotiDate.Text = orderdNotifications.Select(obj => obj.createDate).Skip(4).FirstOrDefault().ToString();

                        }
                    }

                }
                else
                {
                    bdrMain.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_showAll_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Opacity = 0.2;
            wd_notifications w = new wd_notifications();
            w.notifications = notifications;
                    // w.ShowInTaskbar = false;
            w.ShowDialog();
            Window.GetWindow(this).Opacity = 1;
        }
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {

                bdr_showAll.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {

                bdr_showAll.Visibility = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                bdrMain.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        

       

       
        private void Btn_userImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                wd_userInfo w = new wd_userInfo();
                    // w.ShowInTaskbar = false;
                w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_applicationStop_Click(object sender, RoutedEventArgs e)
        {

            HelpClass.StartAwait(grid_mainWindow);

            Window.GetWindow(this).Opacity = 0.2;
            wd_applicationStop w = new wd_applicationStop();
                    // w.ShowInTaskbar = false;
            w.ShowDialog();
            if (w.status == "o")
            {
                txt_cashValue.Foreground = Application.Current.Resources["Green"] as SolidColorBrush;
                txt_cashSympol.Foreground = Application.Current.Resources["Green"] as SolidColorBrush;
            }
            else
            {
                txt_cashValue.Foreground = Application.Current.Resources["Red"] as SolidColorBrush;
                txt_cashSympol.Foreground = Application.Current.Resources["Red"] as SolidColorBrush;
            }
            Window.GetWindow(this).Opacity = 1;


            HelpClass.EndAwait(grid_mainWindow);
        }
        private void Btn_deliveryWaitConfirmUser_Click(object sender, RoutedEventArgs e)
        {
            HelpClass.StartAwait(grid_mainWindow);

            Window.GetWindow(this).Opacity = 0.2;
            wd_deliveryWaitConfirmUser w = new wd_deliveryWaitConfirmUser();
            w.ShowDialog();
            Window.GetWindow(this).Opacity = 1;

            refreshOrdersWaitNotification();
            HelpClass.EndAwait(grid_mainWindow);
        }
        private void Btn_transfers_Click(object sender, RoutedEventArgs e)
        {
           
                HelpClass.StartAwait(grid_mainWindow);

            Window.GetWindow(this).Opacity = 0.2;
            wd_transfers w = new wd_transfers();
                    // w.ShowInTaskbar = false;
            w.ShowDialog();
            Window.GetWindow(this).Opacity = 1;


            HelpClass.EndAwait(grid_mainWindow);
        }
        
        //internal static int? posID;
        public static async Task refreshBalance()
        {
            try
            {
                posLogin = await posLogin.getById(posLogin.posId);
                mainWindow.txt_cashValue.Text = HelpClass.DecTostring(posLogin.balance);
                mainWindow.txt_cashSympol.Text = AppSettings.Currency;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, mainWindow);
            }

        }

        private void clearImg()
        {
            Uri resourceUri = new Uri("pic/no-image-icon-90x90.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            myBrush.ImageSource = temp;
            img_userLogin.Fill = myBrush;

        }
        #region Main Path
        public void initializationMainTrack(string tag)
        {
            //sp_mainPath
            sp_mainPath.Children.Clear();
            List<Object> _listObjects = new List<Object>();
            _listObjects = FillCombo.objectModel.GetParents(FillCombo.objectsList, tag);
            int counter = 1;
            bool isLast = false;
            foreach (var item in _listObjects)
            {
                if (counter == _listObjects.Count)
                    isLast = true;
                else
                    isLast = false;
                sp_mainPath.Children.Add(initializationMainButton(item, isLast));
                counter++;
            }
        }
        Button initializationMainButton(Object _object, bool isLast)
        {
            Button button = new Button();
            button.Content = ">" + AppSettings.resourcemanager.GetString(_object.translate);
            button.Tag = _object.name;
            button.Click += MainButton_Click;
            button.Background = null;
            button.Margin = new Thickness(5, 0, 0, 0);
            button.BorderThickness = new Thickness(0);
            button.Padding = new Thickness(0);
            button.FontSize = 16;
            if (isLast)
                button.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
            else
                button.Foreground = Application.Current.Resources["Grey"] as SolidColorBrush;
            return button;
        }
        void MainButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            initializationMainTrack(button.Tag.ToString());
            loadPath(button.Tag.ToString());

        }
        void loadPath(string tag)
        {
            grid_main.Children.Clear();
            switch (tag)
            {
                //2
                case "home":
                    grid_main.Children.Add(uc_home.Instance);
                    break;
                case "catalog":
                    grid_main.Children.Add(uc_catalog.Instance);
                    break;
                case "purchase":
                    grid_main.Children.Add(uc_purchase.Instance);
                    break;
                case "storage":
                    grid_main.Children.Add(uc_storage.Instance);
                    break;
                case "kitchen":
                    grid_main.Children.Add(uc_laundry.Instance);
                    break;
                case "sales":
                    grid_main.Children.Add(uc_sales.Instance);
                    break;
                case "delivery":
                    grid_main.Children.Add(uc_delivery.Instance);
                    break;
                case "accounts":
                    grid_main.Children.Add(uc_accounts.Instance);
                    break;
                case "reports":
                    grid_main.Children.Add(uc_reports.Instance);
                    break;
                case "sectionData":
                    grid_main.Children.Add(uc_sectionData.Instance);
                    break;
                //12
                case "settings":
                    grid_main.Children.Add(uc_settings.Instance);
                    break;
                //case "hallDivide":
                //    grid_main.Children.Add(uc_hallDivide.Instance);
                //    break;
                case "persons":
                    grid_main.Children.Add(uc_persons.Instance);
                    break;
                case "branchesAndStores":
                    grid_main.Children.Add(uc_branchesAndStores.Instance);
                    break;
                case "banksData":
                    grid_main.Children.Add(uc_banksData.Instance);
                    break;
                //case "tables":
                //    grid_main.Children.Add(uc_tables.Instance);
                //    break;
                //case "hallSections":
                //    grid_main.Children.Add(uc_hallSections.Instance);
                //    break;
                case "vendors":
                    grid_main.Children.Add(uc_vendors.Instance);
                    break;
                case "customers":
                    grid_main.Children.Add(uc_customers.Instance);
                    break;
                case "users":
                    grid_main.Children.Add(uc_users.Instance);
                    break;
                //22
                case "branches":
                    grid_main.Children.Add(uc_branches.Instance);
                    break;
                case "stores":
                    grid_main.Children.Add(uc_stores.Instance);
                    break;
                case "pos":
                    grid_main.Children.Add(uc_pos.Instance);
                    break;
                case "banks":
                    grid_main.Children.Add(uc_banks.Instance);
                    break;
                case "cards":
                    grid_main.Children.Add(uc_cards.Instance);
                    break;
                case "rawMaterials":
                    grid_main.Children.Add(uc_rawMaterials.Instance);
                    break;
                case "foods":
                    grid_main.Children.Add(uc_salesItems.Instance);
                    break;
                case "appetizers":
                    grid_main.Children.Add(uc_salesItem.Instance);
                    uc_salesItem.categoryName = "appetizers";
                    break;
                case "beverages":
                    grid_main.Children.Add(uc_salesItem.Instance);
                    uc_salesItem.categoryName = "beverages";
                    break;
                case "fastFood":
                    grid_main.Children.Add(uc_salesItem.Instance);
                    uc_salesItem.categoryName = "fastFood";
                    break;
                    //32
                case "mainCourses":
                    grid_main.Children.Add(uc_salesItem.Instance);
                    uc_salesItem.categoryName = "mainCourses";
                    break;
                case "desserts":
                    grid_main.Children.Add(uc_salesItem.Instance);
                    uc_salesItem.categoryName = "desserts";
                    break;
                case "payInvoice":
                    grid_main.Children.Add(uc_payInvoice.Instance);
                    break;
                case "purchaseOrder":
                    grid_main.Children.Add(uc_purchaseOrder.Instance);
                    break;
                //case "purchaseStatistic":
                //    grid_main.Children.Add(uc_purchaseStatistic.Instance);
                //    break;
                case "storageDivide":
                    grid_main.Children.Add(uc_storageDivide.Instance);
                    break;
                case "storageOperations":
                    grid_main.Children.Add(uc_storageOperations.Instance);
                    break;
                case "movementsOperations":
                    grid_main.Children.Add(uc_movementsOperations.Instance);
                    break;
                case "stocktakingOperations":
                    grid_main.Children.Add(uc_stocktakingOperations.Instance);
                    break;
                case "locations":
                    grid_main.Children.Add(uc_storageLocations.Instance);
                    break;
                //42
                case "storageSections":
                    grid_main.Children.Add(uc_storageSections.Instance);
                    break;
                case "storageCost":
                    grid_main.Children.Add(uc_storageCost.Instance);
                    break;
                case "storageInvoice":
                    grid_main.Children.Add(uc_storageInvoice.Instance);
                    break;
                case "itemsStorage":
                    grid_main.Children.Add(uc_itemsStorage.Instance);
                    break;
                case "storageMovements":
                    grid_main.Children.Add(uc_storageMovements.Instance);
                    break;
                case "spendingOrder":
                    grid_main.Children.Add(uc_spendingOrder.Instance);
                    break;
                case "itemsShortage":
                    grid_main.Children.Add(uc_itemsShortage.Instance);
                    break;
                case "itemsDestructive":
                    grid_main.Children.Add(uc_itemsDestructive.Instance);
                    break;
                case "stocktaking":
                    grid_main.Children.Add(uc_stocktaking.Instance);
                    break;
                case "preparingOrders":
                    grid_main.Children.Add(uc_preparingOrders.Instance);
                    break;
                //52
                case "spendingRequest":
                    grid_main.Children.Add(uc_spendingRequest.Instance);
                    break;
                case "posTransfers":
                    grid_main.Children.Add(uc_posTransfers.Instance);
                    break;
                case "payments":
                    grid_main.Children.Add(uc_payments.Instance);
                    break;
                case "received":
                    grid_main.Children.Add(uc_received.Instance);
                    break;
                case "banksAccounting":
                    grid_main.Children.Add(uc_banksAccounting.Instance);
                    break;
                //case "accountsStatistic":
                //    grid_main.Children.Add(uc_accountsStatistic.Instance);
                //    break;
                case "subscriptions":
                    grid_main.Children.Add(uc_membershipsAccounting.Instance);
                    break;
                case "ordersAccounting":
                    grid_main.Children.Add(uc_ordersAccounting.Instance);
                    break;
                case "general":
                    grid_main.Children.Add(uc_general.Instance);
                    break;
                case "reportsSettings":
                    grid_main.Children.Add(uc_reportsSettings.Instance);
                    break;
                //62
                case "permissions":
                    grid_main.Children.Add(uc_permissions.Instance);
                    break;
                //case "emailSettings":
                //    grid_main.Children.Add(uc_emailSettings.Instance);
                //    break;
                //case "smsSettings":
                //    grid_main.Children.Add(uc_smsSettings.Instance);
                //    break;
                case "promotion":
                    grid_main.Children.Add(uc_promotion.Instance);
                    break;
                //case "reservations":
                //    grid_main.Children.Add(uc_reservations.Instance);
                //    break;
                case "clothes":
                    grid_main.Children.Add(uc_receiptInvoice.Instance);
                    break;
                //case "carpets":
                //    grid_main.Children.Add(uc_carpets.Instance);
                //    break;
                case "salesStatistic":
                    grid_main.Children.Add(uc_dailySalesStatistic.Instance);
                    break;
                case "membership":
                    grid_main.Children.Add(uc_membership.Instance);
                    break;
                case "coupon":
                    grid_main.Children.Add(uc_coupon.Instance);
                    break;
                    //72
                case "offer":
                    grid_main.Children.Add(uc_offer.Instance);
                    break;
                //case "quotation":
                //    grid_main.Children.Add(uc_quotation.Instance);
                //    break;
                //case "medals":
                //    grid_main.Children.Add(uc_medals.Instance);
                //    break;

                    //75  package
                case "package":
                    grid_main.Children.Add(uc_package.Instance);
                    break;
                    //76  deliveryManagement
                case "deliveryManagement":
                    grid_main.Children.Add(uc_deliveryManagement.Instance);
                    break;
                //77  shippingCompanies
                case "shippingCompanies":
                    grid_main.Children.Add(uc_shippingCompanies.Instance);
                    break;
                //78  itemsRawMaterials
                case "itemsRawMaterials":
                    grid_main.Children.Add(uc_itemsRawMaterials.Instance);
                    break;
                //79  units
                case "units":
                    grid_main.Children.Add(uc_units.Instance);
                    break;
                ////80  menuSettings
                //case "menuSettings":
                //    grid_main.Children.Add(uc_menuSettings.Instance);
                //    break;
                ////81  itemsCosting
                //case "itemsCosting":
                //    grid_main.Children.Add(uc_itemsCosting.Instance);
                //    break;
                //82  consumptionRawMaterials
                case "consumptionRawMaterials":
                    grid_main.Children.Add(uc_consumptionRawMaterials.Instance);
                    break;
                //83  reservationTable
                //case "reservationTable":
                //    grid_main.Children.Add(uc_reservationTable.Instance);
                //    break;
                //84  reservationsUpdate
                //case "reservationsUpdate":
                //    grid_main.Children.Add(uc_reservationsUpdate.Instance);
                //    break;
                //85  residentialSectors
                case "residentialSectors":
                    grid_main.Children.Add(uc_residentialSectors.Instance);
                    break;
                //86  emailsGeneral
                case "emailsGeneral":
                    grid_main.Children.Add(uc_emailsGeneral.Instance);
                    break;
                //87  emailsSetting
                case "emailsSetting":
                    grid_main.Children.Add(uc_emailsSetting.Instance);
                    break;
                //88  emailsTamplates
                case "emailsTamplates":
                    grid_main.Children.Add(uc_emailsTamplates.Instance);
                    break;
                //89  driversManagement
                case "driversManagement":
                    grid_main.Children.Add(uc_driversManagement.Instance);
                    break;
                //90  categorie
                case "categorie":
                    grid_main.Children.Add(uc_categorieRawMaterials.Instance);
                    break;

                //91  alerts
                //92  storageAlerts
                //93  kitchenAlerts
                //94  saleAlerts
                //95  accountsAlerts
                //96  storageAlerts_minMaxItem
                //97  storageAlerts_ImpExp
                //98  storageAlerts_ctreatePurchaseInvoice
                //99  storageAlerts_ctreatePurchaseReturnInvoice
                //100 storageAlerts_spendingOrderApprove
                //101 kitchenAlerts_spendingOrderRequest
                //102 saleAlerts_executeOrder

                //103 storageReports
                case "storageReports":
                    grid_main.Children.Add(uc_storageReports.Instance);
                    break;
                //104 purchaseReports
                case "purchaseReports":
                    grid_main.Children.Add(uc_purchaseReports.Instance);
                    break;
                //105 salesReports
                case "salesReports":
                    grid_main.Children.Add(uc_salesReports.Instance);
                    break;
                //106 accountsReports
                case "accountsReports":
                    grid_main.Children.Add(uc_accountsReports.Instance);
                    break;
                //107 stockStorageReports
                case "stockStorageReports":
                    grid_main.Children.Add(uc_stockStorageReports.Instance);
                    break;
                //108 externalStorageReports
                case "externalStorageReports":
                    grid_main.Children.Add(uc_externalStorageReports.Instance);
                    break;
                //109 internalStorageReports
                case "internalStorageReports":
                    grid_main.Children.Add(uc_internalStorageReports.Instance);
                    break;
                //110 directStorageReports
                case "directStorageReports":
                    grid_main.Children.Add(uc_directStorageReports.Instance);
                    break;
                //111 stocktakingStorageReports
                case "stocktakingStorageReports":
                    grid_main.Children.Add(uc_stocktakingStorageReports.Instance);
                    break;
                //112 destroiedStorageReports
                case "destroiedStorageReports":
                    grid_main.Children.Add(uc_destroiedStorageReports.Instance);
                    break;
                //113 invoicePurchaseReports
                case "invoicePurchaseReports":
                    grid_main.Children.Add(uc_invoicePurchaseReports.Instance);
                    break;
                //114 itemPurchaseReports
                case "itemPurchaseReports":
                    grid_main.Children.Add(uc_itemPurchaseReports.Instance);
                    break;
                //115 orderPurchaseReports
                case "orderPurchaseReports":
                    grid_main.Children.Add(uc_orderPurchaseReports.Instance);
                    break;
                //116 invoiceSalesReports
                case "invoiceSalesReports":
                    grid_main.Children.Add(uc_invoiceSalesReports.Instance);
                    break;
                //117 itemSalesReports
                case "itemSalesReports":
                    grid_main.Children.Add(uc_itemSalesReports.Instance);
                    break;
                //118 promotionSalesReports
                case "promotionSalesReports":
                    grid_main.Children.Add(uc_promotionSalesReports.Instance);
                    break;
                //119 orderSalesReports
                case "orderSalesReports":
                    grid_main.Children.Add(uc_orderSalesReports.Instance);
                    break;
                ////120 quotationSalesReports
                //case "quotationSalesReports":
                //    grid_main.Children.Add(uc_quotationSalesReports.Instance);
                //    break;
                //121 dailySalesReports
                case "dailySalesReports":
                    grid_main.Children.Add(uc_dailySalesReports.Instance);
                    break;
                //122 paymentsAccountsReports
                case "paymentsAccountsReports":
                    grid_main.Children.Add(uc_paymentsAccountsReports.Instance);
                    break;
                //123 recipientAccountsReports
                case "recipientAccountsReports":
                    grid_main.Children.Add(uc_recipientAccountsReports.Instance);
                    break;
                //124 bankAccountsReports
                case "bankAccountsReports":
                    grid_main.Children.Add(uc_bankAccountsReports.Instance);
                    break;
                //125 posAccountsReports
                case "posAccountsReports":
                    grid_main.Children.Add(uc_posAccountsReports.Instance);
                    break;
                //126 statementAccountsReports
                case "statementAccountsReports":
                    grid_main.Children.Add(uc_statementAccountsReports.Instance);
                    break;
                //127 fundAccountsReports
                case "fundAccountsReports":
                    grid_main.Children.Add(uc_fundAccountsReports.Instance);
                    break;
                //128 profitsAccountsReports
                case "profitsAccountsReports":
                    grid_main.Children.Add(uc_profitsAccountsReports.Instance);
                    break;
                //183 closingAccountsReports
                case "closingAccountsReports":
                    grid_main.Children.Add(uc_closingAccountsReports.Instance);
                    break;
                //184 taxAccountsReports
                case "taxAccountsReports":
                    grid_main.Children.Add(uc_taxAccountsReports.Instance);
                    break;
                //200 membershipCreate
                case "membershipCreate":
                    grid_main.Children.Add(uc_membershipCreate.Instance);
                    break;
                //201 membershipUpdate
                case "membershipUpdate":
                    grid_main.Children.Add(uc_membershipUpdate.Instance);
                    break;
                //202 invoicesClasses
                case "invoicesClasses":
                    grid_main.Children.Add(uc_invoicesClasses.Instance);
                    break;

                //216 dashboard
                //290 membershipSalesReports
                case "membershipSalesReports":
                    grid_main.Children.Add(uc_membershipSalesReports.Instance);
                    break;
                //291 kitchenReports
                case "kitchenReports":
                    grid_main.Children.Add(uc_laundryReports.Instance);
                    break;
                //292 deliveryReports
                case "deliveryReports":
                    grid_main.Children.Add(uc_deliveryReports.Instance);
                    break;                
                //293 preparingOrdersKitchenReports
                case "preparingOrdersKitchenReports":
                    grid_main.Children.Add(uc_preparingOrdersLaundryReports.Instance);
                    break;                 
                //294 spendingRequestsKitchenReports
                case "spendingRequestsKitchenReports":
                    grid_main.Children.Add(uc_spendingRequestsLaundryReports.Instance);
                    break;                 
                //295 consumptionKitchenReports
                case "consumptionKitchenReports":
                    grid_main.Children.Add(uc_consumptionLaundryReports.Instance);
                    break;
                //303 spendingStorageReports
                case "spendingStorageReports":
                    grid_main.Children.Add(uc_spendingStorageReports.Instance);
                    break;



                default:
                    return;

            }
        }

        #endregion

       
    }
}
