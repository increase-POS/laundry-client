using laundryApp.ApiClasses;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using System;
using laundryApp.Classes.ApiClasses;

namespace laundryApp.Classes
{
    public class FillCombo
    {
        #region  Object
        static public Object objectModel = new Object();
        static public List<Object> objectsList;
        static public async Task<List<Object>> RefreshObjects()
        {
            //objectsList = await objectModel.GetAll();
            objectsList = new List<Object>();
            return objectsList;
        }

        //static public List<Object> listObjects = new List<Object>();
        static public GroupObject groupObject = new GroupObject();
        static public List<GroupObject> groupObjects = new List<GroupObject>();
        #endregion
        #region Groups
        static public Group group = new Group();
        static public List<Group> groupsList;

        static public async Task<IEnumerable<Group>> RefreshGroups()
        {
            groupsList = await group.GetAll();
            groupsList = groupsList.Where(X => X.isActive == 1).ToList();
            return groupsList;
        }

        static public async Task FillComboGroups_withDefault(ComboBox cmb)
        {
            if (groupsList is null)
                await RefreshGroups();

            var groups = groupsList.ToList();
            group = new Group();
            group.groupId = 0;
            group.name = "-";
            groups.Insert(0, group);

            cmb.ItemsSource = groups;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "groupId";
            cmb.SelectedIndex = -1;
        }



        #endregion
        #region branch
        static public Branch branch = new Branch();
        static public List<Branch> branchsList;
        static public List<Branch> branchesAllWithoutMain;
        static public List<Branch> BranchesByBranchandUser;
        static public List<Branch> branchesWithAll;
        static public async Task<IEnumerable<Branch>> RefreshBranches()
        {
            branchsList = await branch.GetAll();
            return branchsList;
        }
        static public async Task fillComboBranchParent(ComboBox cmb)
        {
            if (branchsList is null)
                await RefreshBranches();
            cmb.ItemsSource = branchsList.Where(b => b.type == "b" || b.type == "bs");
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "branchId";
            cmb.SelectedIndex = -1;
        }
        static public async Task<IEnumerable<Branch>> RefreshBranchesAllWithoutMain()
        {
            branchesAllWithoutMain = await branch.GetAllWithoutMain("all");
            return branchesAllWithoutMain;
        }
        static public async Task fillComboBranchesAllWithoutMain(ComboBox cmb)
        {
            if (branchesAllWithoutMain is null)
                await RefreshBranchesAllWithoutMain();
            cmb.ItemsSource = branchesAllWithoutMain;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "branchId";
            cmb.SelectedIndex = -1;

        }


        static public async Task<IEnumerable<Branch>> RefreshByBranchandUser()
        {
            BranchesByBranchandUser = await branch.BranchesByBranchandUser(MainWindow.branchLogin.branchId, MainWindow.userLogin.userId);
            return BranchesByBranchandUser;
        }
        static public async Task fillBranchesWithAll(ComboBox combo, string type = "")
        {

            if (HelpClass.isAdminPermision())
            {
                if (branchsList is null)
                    await RefreshBranches();
                branchesWithAll = branchsList.ToList();
            }
            else
            {
                if (BranchesByBranchandUser is null)
                    await RefreshByBranchandUser();
                branchesWithAll = BranchesByBranchandUser.ToList();
            }
            //branchesWithAll = branches.ToList();
            Branch branch = new Branch();
            branch.name = "All";
            branch.branchId = 0;
            branchesWithAll.Insert(0, branch);

            combo.ItemsSource = branchesWithAll.Where(b => b.type != type && b.branchId != 1);
            combo.SelectedValuePath = "branchId";
            combo.DisplayMemberPath = "name";
            combo.SelectedIndex = -1;
        }

        static public async Task fillBranchesWithoutCurrent(ComboBox cmb, string type = "")
        {
            List<Branch> branches = new List<Branch>();

            if (branchesAllWithoutMain is null)
                await RefreshBranchesAllWithoutMain();
            if (BranchesByBranchandUser is null)
                await RefreshByBranchandUser();

            if (HelpClass.isAdminPermision())
                branches = branchesAllWithoutMain.ToList();
            else
                branches = BranchesByBranchandUser.ToList();

            branch = branches.Where(s => s.branchId == MainWindow.branchLogin.branchId).FirstOrDefault<Branch>();
            branches.Remove(branch);
            var br = new Branch();
            br.branchId = 0;
            br.name = "-";
            branches.Insert(0, br);
            cmb.ItemsSource = branches.Where(b => b.type != type && b.branchId != 1);
            cmb.SelectedValuePath = "branchId";
            cmb.DisplayMemberPath = "name";
            cmb.SelectedIndex = -1;
        }
        static public async Task fillBranchesNoCurrentDefault(ComboBox cmb, string type = "")
        {
            List<Branch> branches = new List<Branch>();

            if (branchesAllWithoutMain is null)
                await RefreshBranchesAllWithoutMain();
            if (BranchesByBranchandUser is null)
                await RefreshByBranchandUser();

            if (HelpClass.isAdminPermision())
                branches = branchesAllWithoutMain;
            else
                branches = BranchesByBranchandUser;

            branch = branches.Where(s => s.branchId == MainWindow.branchLogin.branchId).FirstOrDefault<Branch>();
            branches.Remove(branch);
            cmb.ItemsSource = branches.Where(b => b.type != type && b.branchId != 1);
            cmb.SelectedValuePath = "branchId";
            cmb.DisplayMemberPath = "name";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #region fill process type PayType

        static public void FillDefaultPayType_cashBalanceCardMultiple(ComboBox cmb)
        {
            #region fill process type
            var typelist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trCash")       , Value = "cash" },
                new { Text = AppSettings.resourcemanager.GetString("trCredit") , Value = "balance" },
                new { Text = AppSettings.resourcemanager.GetString("trAnotherPaymentMethods") , Value = "card" },
                new { Text = AppSettings.resourcemanager.GetString("trMultiplePayment") , Value = "multiple" }, 
                //new { Text = AppSettings.resourcemanager.GetString("trDocument")   , Value = "doc" },
                //new { Text = AppSettings.resourcemanager.GetString("trCheque")     , Value = "cheque" },
                 };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = typelist;
            cmb.SelectedIndex = 0;
            #endregion
        }
        static public void FillDefaultPayType_cashChequeCard(ComboBox cmb)
        {
            var typelist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trCash")       , Value = "cash" },
                new { Text = AppSettings.resourcemanager.GetString("trCheque")     , Value = "cheque" },
                new { Text = AppSettings.resourcemanager.GetString("trAnotherPaymentMethods") , Value = "card" },
                 };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = typelist;
        }
        #endregion
        #region fill statuses Of Preparing Order

        static public void FillStatusesOfPreparingOrder(ComboBox cmb)
        {
            var typelist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("directlyPrint")       , Value = "directlyPrint" },
                new { Text = AppSettings.resourcemanager.GetString("statusesSequence") , Value = "statusesSequence" },
                 };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = typelist;
            cmb.SelectedIndex = 0;
        }
        #endregion
        #region job
        static public List<keyValueString> UserJobList;
        static public List<keyValueString> RefreshUserJobs()
        {
            UserJobList = new List<keyValueString> {
                //manager
                new keyValueString { key = "admin" ,  value = AppSettings.resourcemanager.GetString("trAdmin") },
                new keyValueString { key = "generalManager" , value = AppSettings.resourcemanager.GetString("trGeneralManager") },
                new keyValueString { key = "assistantManager" , value = AppSettings.resourcemanager.GetString("trAssistantManager") },
                new keyValueString { key = "purchasingManager",   value = AppSettings.resourcemanager.GetString("trPurchasingManager") },
                new keyValueString { key = "salesManager",    value = AppSettings.resourcemanager.GetString("trSalesManager") },
                new keyValueString { key = "accountant" , value = AppSettings.resourcemanager.GetString("trAccountant")},
                //Kitchen
                new keyValueString { key = "kitchenManager",  value = AppSettings.resourcemanager.GetString("trKitchenManager") },
                new keyValueString { key = "executiveChef",   value = AppSettings.resourcemanager.GetString("trExecutiveChef")  },
                new keyValueString { key = "sousChef" ,   value = AppSettings.resourcemanager.GetString("trSousChef") },
                new keyValueString { key = "chef" ,   value = AppSettings.resourcemanager.GetString("trChef") },
                new keyValueString { key = "dishwasher" , value = AppSettings.resourcemanager.GetString("trDishwasher")},
                new keyValueString { key = "kitchenEmployee" , value = AppSettings.resourcemanager.GetString("trKitchenEmployee")},
                //hall
                new keyValueString { key = "Headwaiter" , value = AppSettings.resourcemanager.GetString("trHeadWaiter")},
                new keyValueString { key = "waiter",  value = AppSettings.resourcemanager.GetString("trWaiter")  },
                new keyValueString { key = "cashier" ,    value = AppSettings.resourcemanager.GetString("trCashier") },
                new keyValueString { key = "receptionist" , value = AppSettings.resourcemanager.GetString("trReceptionist")},
                //warehouse manager
                new keyValueString { key = "warehouseManager", value = AppSettings.resourcemanager.GetString("trWarehouseManager")  },
                new keyValueString { key = "warehouseEmployee" ,  value = AppSettings.resourcemanager.GetString("trWarehouseEmployee")},
                //Delivery
                new keyValueString { key = "deliveryManager", value = AppSettings.resourcemanager.GetString("trDeliveryManager") },
                new keyValueString { key = "deliveryEmployee" ,   value = AppSettings.resourcemanager.GetString("trDeliveryEmployee")},
                //other
                new keyValueString { key = "cleaningEmployee" ,   value = AppSettings.resourcemanager.GetString("trCleaningEmployee") },
                new keyValueString { key = "employee",    value = AppSettings.resourcemanager.GetString("trEmployee") },
                 };
            return UserJobList;
        }
        static public void FillUserJob(ComboBox cmb)
        {
            #region fill job
            if (UserJobList is null)
                RefreshUserJobs();
            cmb.SelectedValuePath = "key";
            cmb.DisplayMemberPath = "value";
            cmb.ItemsSource = UserJobList;
            //cmb.SelectedIndex = 0;
            #endregion
        }
        static public List<keyValueString> RefreshUserJobsReport()
        {
            UserJobListReport = new List<keyValueString> {
                //manager
                new keyValueString { key = "admin" ,  value = AppSettings.resourcemanagerreport.GetString("trAdmin") },
                new keyValueString { key = "generalManager" , value = AppSettings.resourcemanagerreport.GetString("trGeneralManager") },
                new keyValueString { key = "assistantManager" , value = AppSettings.resourcemanagerreport.GetString("trAssistantManager") },
                new keyValueString { key = "purchasingManager",   value = AppSettings.resourcemanagerreport.GetString("trPurchasingManager") },
                new keyValueString { key = "salesManager",    value = AppSettings.resourcemanagerreport.GetString("trSalesManager") },
                new keyValueString { key = "accountant" , value = AppSettings.resourcemanagerreport.GetString("trAccountant")},
                //Kitchen
                new keyValueString { key = "kitchenManager",  value = AppSettings.resourcemanagerreport.GetString("trKitchenManager") },
                new keyValueString { key = "executiveChef",   value = AppSettings.resourcemanagerreport.GetString("trExecutiveChef")  },
                new keyValueString { key = "sousChef" ,   value = AppSettings.resourcemanagerreport.GetString("trSousChef") },
                new keyValueString { key = "chef" ,   value = AppSettings.resourcemanagerreport.GetString("trChef") },
                new keyValueString { key = "dishwasher" , value = AppSettings.resourcemanagerreport.GetString("trDishwasher")},
                new keyValueString { key = "kitchenEmployee" , value = AppSettings.resourcemanagerreport.GetString("trKitchenEmployee")},
                //hall
                new keyValueString { key = "Headwaiter" , value = AppSettings.resourcemanagerreport.GetString("trHeadWaiter")},
                new keyValueString { key = "waiter",  value = AppSettings.resourcemanagerreport.GetString("trWaiter")  },
                new keyValueString { key = "cashier" ,    value = AppSettings.resourcemanagerreport.GetString("trCashier") },
                new keyValueString { key = "receptionist" , value = AppSettings.resourcemanagerreport.GetString("trReceptionist")},
                //warehouse manager
                new keyValueString { key = "warehouseManager", value = AppSettings.resourcemanagerreport.GetString("trWarehouseManager")  },
                new keyValueString { key = "warehouseEmployee" ,  value = AppSettings.resourcemanagerreport.GetString("trWarehouseEmployee")},
                //Delivery
                new keyValueString { key = "deliveryManager", value = AppSettings.resourcemanagerreport.GetString("trDeliveryManager") },
                new keyValueString { key = "deliveryEmployee" ,   value = AppSettings.resourcemanagerreport.GetString("trDeliveryEmployee")},
                //other
                new keyValueString { key = "cleaningEmployee" ,   value = AppSettings.resourcemanagerreport.GetString("trCleaningEmployee") },
                new keyValueString { key = "employee",    value = AppSettings.resourcemanagerreport.GetString("trEmployee") },
                 };
            return UserJobListReport;
        }
        #endregion
        #region ItemTypeSales
        static public List<string> salesTypes = new List<string>() { "SalesNormal", "packageItems" };

        #endregion
        #region ItemTypePurchase
        static public List<string> purchaseTypes = new List<string>() { "PurchaseNormal", "PurchaseExpire" };

        static public void FillItemTypePurchase(ComboBox cmb)
        {
            #region fill process type
            var typelist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trNormal"), Value = "PurchaseNormal" },
                new { Text = AppSettings.resourcemanager.GetString("trExpire") , Value = "PurchaseExpire" },
                 };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = typelist;
            #endregion
        }
        #endregion
        #region DiscountType
        static public void FillDiscountType(ComboBox cmb)
        {
            #region fill process type
            var dislist = new[] {
            new { Text = "", Value = -1 },
            new { Text = AppSettings.resourcemanager.GetString("trValueDiscount"), Value = 1 },
            new { Text = AppSettings.resourcemanager.GetString("trPercentageDiscount"), Value = 2 },
             };

            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = dislist;
            cmb.SelectedIndex = 0;
            #endregion
        }
        private void configure()
        {

        }
        #endregion
        #region Category
        static public Category category = new Category();
        static public List<Category> categoriesList;
        static public int GetCategoryId(string categoryName)
        {
            return categoriesList.Where(x => x.name.ToLower() == categoryName.ToLower()).FirstOrDefault().categoryId;
        }
        static public async Task<IEnumerable<Category>> RefreshCategory()
        {
            categoriesList = await category.Get();
            return categoriesList;
        }
        static public async void FillCategoryPurchase(ComboBox cmb)
        {
            #region FillCategoryPurchase
            if (categoriesList is null)
                await RefreshCategory();
            cmb.ItemsSource = categoriesList.Where(x => x.type == "p" && x.isActive == 1).ToList();
            cmb.SelectedValuePath = "categoryId";
            cmb.DisplayMemberPath = "name";
            #endregion
        }
        static public async Task FillCategorySale(ComboBox cmb)
        {
            #region FillCategorySale
            if (categoriesList is null)
                await RefreshCategory();
            List<Category> newList = categoriesList.Where(x => x.type == "s").ToList();
            var ca = new Category();
            ca.categoryId = 0;
            ca.name = "-";
            newList.Insert(0, ca);

            cmb.ItemsSource = newList;
            cmb.SelectedValuePath = "categoryId";
            //cmb.DisplayMemberPath = "name";
            #endregion
        }
        #endregion
        #region tags
        public static Tag tag = new Tag();
        public static async Task<List<Tag>> fillTags(ComboBox cmb, int categoryId)
        {
            List<Tag> tags;
            if (categoryId == -1)
                tags = new List<Tag>();
            else
                tags = await tag.Get(categoryId);
            tags = tags.Where(x => x.isActive == 1).ToList();
            cmb.ItemsSource = tags;
            cmb.SelectedValuePath = "tagId";
            cmb.DisplayMemberPath = "tagName";
            return tags;
        }
        public static async Task fillTagsWithDefault(ComboBox cmb, int categoryId)
        {
            List<Tag> tags;
            if (categoryId == -1)
                tags = new List<Tag>();
            else
            {
                tags = await tag.Get(categoryId);

                var newTag = new Tag();
                newTag.tagId = 0;
                newTag.tagName = "-";
                tags.Insert(0, newTag);
            }
            cmb.ItemsSource = tags;
            cmb.SelectedValuePath = "tagId";
            cmb.DisplayMemberPath = "tagName";
        }
        #endregion
        #region services
        static public Service service = new Service();
        static public List<Service> servicesList;
        static public async Task<IEnumerable<Service>> RefreshService()
        {
            servicesList = await service.Get();
            return servicesList;
        }
        #endregion
        #region Unit
        static public Unit unit = new Unit();
        static public Unit saleUnit = new Unit();

        static public List<Unit> unitsList;
        static public async Task<List<Unit>> RefreshUnit()
        {

            unitsList = await unit.GetActive();
            saleUnit = unitsList.Where(x => x.name == "saleUnit").FirstOrDefault();
            unitsList = unitsList.Where(u => u.name != "saleUnit").ToList();
            return unitsList;
        }
        static public async void FillUnits(ComboBox cmb)
        {
            #region Fill Active Units
            if (unitsList is null)
                await RefreshUnit();
            cmb.ItemsSource = unitsList.ToList();
            cmb.SelectedValuePath = "unitId";
            cmb.DisplayMemberPath = "name";
            #endregion
        }
        static public async Task FillSmallUnits(ComboBox cmb, int mainUnitId, int itemId)
        {
            #region Fill small units
            List<Unit> smallUnits;
            if (mainUnitId == -1)
                smallUnits = new List<Unit>();
            else
                smallUnits = await unit.getSmallUnits(itemId, mainUnitId);
            cmb.ItemsSource = smallUnits.Where(u => u.name != "saleUnit").ToList();
            cmb.SelectedValuePath = "unitId";
            cmb.DisplayMemberPath = "name";
            #endregion
        }
        #endregion
        #region item units
        static public ItemUnit itemUnit = new ItemUnit();
        static public List<ItemUnit> itemUnitList;
        static public async Task<List<ItemUnit>> RefreshItemUnit()
        {
            itemUnitList = await itemUnit.GetIU();
            itemUnitList = itemUnitList.Where(u => u.isActive == 1).ToList();
            return itemUnitList;
        }
        #endregion
        #region FillDeliveryType
        static public void FillDeliveryType(ComboBox cmb)
        {
            var typelist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trLocaly")     , Value = "local" },
                new { Text = AppSettings.resourcemanager.GetString("trShippingCompany")   , Value = "com" },
                 };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = typelist;
        }
        #endregion
        #region Countries
        /// <summary>
        /// area code methods
        /// </summary>
        /// <returns></returns>
        /// 
        //phone 
        public static IEnumerable<CountryCode> countrynum;
        public static IEnumerable<City> citynum;
        public static IEnumerable<City> citynumofcountry;
        public static CountryCode countrycodes = new CountryCode();
        public static City cityCodes = new City();

        public static CountryCode Region = new CountryCode();
        static public void fillRegion()
        {
            Region = countrynum.Where(C => C.isDefault == 1).FirstOrDefault();

        }
        static async Task<IEnumerable<CountryCode>> RefreshCountry()
        {
            countrynum = await countrycodes.GetAllCountries();
            await RefreshCity();
            return countrynum;
        }
        static public async Task<IEnumerable<City>> RefreshCity()
        {
            citynum = await cityCodes.Get();
            return citynum;
        }
        static public async Task fillCountries(ComboBox cmb)
        {
            if (countrynum is null)
                await RefreshCountry();

            cmb.ItemsSource = countrynum.ToList();
            cmb.SelectedValuePath = "countryId";
            cmb.DisplayMemberPath = "code";
        }
        static public async Task fillCountriesLocal(ComboBox cmb, int countryid, Border border)
        {
            if (citynum is null)
                await RefreshCity();
            FillCombo.citynumofcountry = FillCombo.citynum.Where(b => b.countryId == countryid).OrderBy(b => b.cityCode).ToList();
            cmb.ItemsSource = FillCombo.citynumofcountry;
            cmb.SelectedValuePath = "cityId";
            cmb.DisplayMemberPath = "cityCode";
            if (FillCombo.citynumofcountry.Count() > 0)
                border.Visibility = Visibility.Visible;
            else
                border.Visibility = Visibility.Collapsed;
        }
        #endregion
        #region agent
        static public Agent agent = new Agent();
        #region Vendors
        static public List<Agent> vendorsList;
        static public async Task<IEnumerable<Agent>> RefreshVendors()
        {
            vendorsList = await agent.GetAgentsActive("v");
            agent = new Agent();
            agent.agentId = 0;
            agent.name = "-";
            vendorsList.Insert(0, agent);
            return vendorsList;
        }
        static public async Task FillComboVendors(ComboBox cmb)
        {
            if (vendorsList is null)
                await RefreshVendors();
            cmb.ItemsSource = vendorsList;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "agentId";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #region Customers
        static public List<Agent> customersList;
        static public async Task<IEnumerable<Agent>> RefreshCustomers()
        {
            customersList = await agent.GetAgentsActive("c");
            agent = new Agent();
            agent.agentId = 0;
            agent.name = "-";
            customersList.Insert(0, agent);
            return customersList;
        }
        static public async Task FillComboCustomers(ComboBox cmb)
        {
            if (customersList is null)
                await RefreshCustomers();
            cmb.ItemsSource = customersList;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "agentId";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #endregion
        #region User
        static public User user = new User();
        static public List<User> usersList;
        static public List<User> driversList;
        //static public List<User> salesManList;
        static public async Task<IEnumerable<User>> RefreshUsers()
        {
            usersList = await user.Get();
            return usersList;
        }
        static public async Task<IEnumerable<User>> RefreshDrivers()
        {
            if (usersList is null)
                await RefreshUsers();
            driversList = usersList.Where(x => x.isActive == 1 && x.isAdmin != true && x.job == "deliveryEmployee").ToList();

            return driversList;
        }

        static public async Task FillComboUsers(ComboBox cmb)
        {
            if (usersList is null)
                await RefreshUsers();
            var users = usersList.Where(x => x.isActive == 1 && x.isAdmin != true).ToList();
            user = new User();
            user.userId = 0;
            user.name = "-";
            users.Insert(0, user);
            cmb.ItemsSource = users;
            //cmb.DisplayMemberPath = "name";
            cmb.DisplayMemberPath = "fullName";
            cmb.SelectedValuePath = "userId";
            cmb.SelectedIndex = -1;
        }

        static public async Task FillComboDrivers(ComboBox cmb)
        {
            if (driversList is null)
                await RefreshDrivers();
            cmb.ItemsSource = driversList;
            //cmb.DisplayMemberPath = "name";
            cmb.DisplayMemberPath = "fullName";
            cmb.SelectedValuePath = "userId";
            cmb.SelectedIndex = -1;
        }
        static public async Task FillComboDrivers_withDefault(ComboBox cmb)
        {
            if (driversList is null)
                await RefreshDrivers();
            List<User> _driversList = driversList.ToList();
            var dr = new User();
            dr.userId = 0;
            dr.name = "-";
            _driversList.Insert(0, dr);

            cmb.ItemsSource = _driversList;
            cmb.DisplayMemberPath = "fullName";
            cmb.SelectedValuePath = "userId";
            cmb.SelectedIndex = -1;
        }

        static public async Task FillComboUsersWithJob(ComboBox cmb, string job)
        {
            if (usersList is null)
                await RefreshUsers();
            var users = usersList.Where(x => x.isActive == 1 && x.job == job).ToList();

            cmb.ItemsSource = users;
            cmb.DisplayMemberPath = "fullName";
            cmb.SelectedValuePath = "userId";
            cmb.SelectedIndex = -1;
        }
        static public async Task FillComboUsersForDelivery(ComboBox cmb, string job, int customerId)
        {
            var users = await user.getUsersForDelivery(job, customerId);
            cmb.ItemsSource = users;
            cmb.DisplayMemberPath = "fullName";
            cmb.SelectedValuePath = "userId";
            cmb.SelectedIndex = -1;
        }

        #endregion
        #region ShippingCompanies
        static public ShippingCompanies shippingCompanie = new ShippingCompanies();
        static public List<ShippingCompanies> shippingCompaniesList;

        static public async Task<IEnumerable<ShippingCompanies>> RefreshShippingCompanies()
        {
            shippingCompaniesList = await shippingCompanie.Get();
            shippingCompaniesList = shippingCompaniesList.Where(X => X.isActive == 1).ToList();
            return shippingCompaniesList;
        }

        static public async Task FillComboShippingCompanies(ComboBox cmb)
        {
            if (shippingCompaniesList is null)
                await RefreshShippingCompanies();

            var shippingCompanies = shippingCompaniesList.ToList();
            shippingCompanie = new ShippingCompanies();
            shippingCompanie.shippingCompanyId = 0;
            shippingCompanie.name = "-";
            shippingCompanies.Insert(0, shippingCompanie);
            cmb.ItemsSource = shippingCompanies;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "shippingCompanyId";
            cmb.SelectedIndex = -1;
        }

        static public async Task FillComboShippingCompaniesForDelivery(ComboBox cmb)
        {
            if (shippingCompaniesList is null)
                await RefreshShippingCompanies();

            var shippingCompanies = shippingCompaniesList.Where(s => s.deliveryType != "local").ToList();
            cmb.ItemsSource = shippingCompanies;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "shippingCompanyId";
            cmb.SelectedIndex = -1;
        }
        static public async Task FillComboShippingCompaniesForDelivery_withDefault(ComboBox cmb)
        {
            if (shippingCompaniesList is null)
                await RefreshShippingCompanies();

            var shippingCompanies = shippingCompaniesList.Where(s => s.deliveryType != "local").ToList();

            var sh = new ShippingCompanies();
            sh.shippingCompanyId = 0;
            sh.name = "-";
            shippingCompanies.Insert(0, sh);

            cmb.ItemsSource = shippingCompanies;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "shippingCompanyId";
            cmb.SelectedIndex = -1;
        }



        #endregion
        #region ResidentialSectors
        static public ResidentialSectors residentialSec = new ResidentialSectors();
        static public List<ResidentialSectors> residentialSecsList;

        static public async Task<IEnumerable<ResidentialSectors>> RefreshResidentialSectors()
        {
            residentialSecsList = await residentialSec.Get();
            return residentialSecsList;
        }

        static public async Task FillComboResidentialSectors(ComboBox cmb)
        {
            if (residentialSecsList is null)
                await RefreshResidentialSectors();
            cmb.ItemsSource = residentialSecsList;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "residentSecId";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #region fill cards combo
        static public Card card = new Card();
        static public List<Card> cardsList;
        static public async Task<IEnumerable<Card>> RefreshCards()
        {
            cardsList = await card.GetAll();
            return cardsList;
        }
        static public async Task FillComboCards(ComboBox cmb)
        {
            if (cardsList is null)
                await RefreshCards();
            cmb.ItemsSource = cardsList;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "agentId";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #region section
        static public Section section = new Section();
        static public List<Section> sectionsByBranchList;
        static public async Task<IEnumerable<Section>> RefreshSectionsByBranch()
        {
            sectionsByBranchList = await section.getBranchSections(MainWindow.branchLogin.branchId);
            return sectionsByBranchList;
        }
        static public async Task FillComboSections(ComboBox cmb)
        {
            if (sectionsByBranchList is null)
                await RefreshSectionsByBranch();
            cmb.ItemsSource = sectionsByBranchList;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "sectionId";
            cmb.SelectedIndex = -1;
        }
        static public async Task FillComboSectionsWithDefault(ComboBox cmb)
        {
            if (sectionsByBranchList is null)
                await RefreshSectionsByBranch();

            List<Section> sections = new List<Section>();
            sections = sectionsByBranchList.ToList();
            var sec = new Section();
            sec.sectionId = 0;
            sec.name = "-";
            sections.Insert(0, sec);

            cmb.ItemsSource = sections;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "sectionId";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #region hall section
        static public HallSection hallSection = new HallSection();
        static public List<HallSection> hallSectionsList;

        static public async Task<IEnumerable<HallSection>> RefreshHallSectionsByBranch()
        {
            hallSectionsList = await hallSection.getBranchSections(MainWindow.branchLogin.branchId);
            return hallSectionsList;
        }

        static public async Task FillComboHallSectionsWithDefault(ComboBox cmb)
        {
            if (hallSectionsList is null)
                await RefreshHallSectionsByBranch();

            List<HallSection> sections = new List<HallSection>();
            sections = hallSectionsList.ToList();
            var sec = new HallSection();
            sec.sectionId = 0;
            sec.name = "-";
            sections.Insert(0, sec);

            cmb.ItemsSource = sections;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "sectionId";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #region location

        static public Location location = new Location();
        static public List<Location> locationsList;
        static public List<Location> locationsBySectionList;
        static public async Task<IEnumerable<Location>> RefreshLocationsBySection(int sectionId)
        {
            locationsBySectionList = await location.getLocsBySectionId(sectionId);
            return locationsBySectionList;
        }
        static public async Task FillComboLocationsBySection(ComboBox cmb, int sectionId)
        {
            if (sectionId == -1)
                cmb.ItemsSource = new List<Location>();
            else
            {
                await RefreshLocationsBySection(sectionId);
                cmb.ItemsSource = locationsBySectionList;
            }
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "locationId";
            cmb.SelectedIndex = -1;
        }
        #endregion
        #region movements
        static public void FillMovementsProcessType(ComboBox cmb)
        {
            #region fill process type
            var processList = new[] {
                 new { Text = AppSettings.resourcemanager.GetString("trImport"), Value = "im" },
            new { Text = AppSettings.resourcemanager.GetString("trExport"), Value = "ex"},
            };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = processList;
            cmb.SelectedIndex = 0;
            #endregion
        }
        #endregion
        #region items
        static public Item item = new Item();
        static public List<Item> purchaseItems;
        static public List<Item> salesItems;
        static public async Task<IEnumerable<Item>> RefreshPurchaseItems()
        {
            purchaseItems = await item.GetPurchaseItems();
            return purchaseItems;
        }
        static public async Task<IEnumerable<Item>> FillComboPurchaseItemsHasQuant(ComboBox cmb)
        {
            var items = await item.GetItemsHasQuant(MainWindow.branchLogin.branchId);

            cmb.ItemsSource = items;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "itemId";
            cmb.SelectedIndex = -1;

            return items;
        }
        static public async Task<IEnumerable<Item>> FillComboPurchaseItems(ComboBox cmb)
        {
            if (purchaseItems == null)
                await RefreshPurchaseItems();

            cmb.ItemsSource = purchaseItems;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "itemId";
            cmb.SelectedIndex = -1;

            return purchaseItems;
        }
        static public async Task<IEnumerable<Item>> RefreshSalesItems()
        {
            salesItems = await item.GetAllSalesItems();
            return salesItems;
        }
        static public async Task<IEnumerable<Item>> FillComboSalesItemsWithDefault(ComboBox cmb)
        {
            if (salesItems == null)
                await RefreshSalesItems();

            var it = new Item();
            it.itemId = 0;
            it.name = "-";
            salesItems.Insert(0, it);

            cmb.ItemsSource = salesItems;
            cmb.DisplayMemberPath = "name";
            cmb.SelectedValuePath = "itemId";
            cmb.SelectedIndex = -1;

            return salesItems;
        }
        #endregion
        #region coupon
        static public Coupon coupon = new Coupon();
        static public void fillDiscountType(ComboBox cmb)
        {
            var dislist = new[] {
            new { Text = AppSettings.resourcemanager.GetString("trValueDiscount"), Value = "1" },
            new { Text = AppSettings.resourcemanager.GetString("trPercentageDiscount"), Value = "2" },
             };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = dislist;
        }
        #endregion
        #region membership
        static public Memberships membership = new Memberships();
        static public void fillSubscriptionType(ComboBox cmb)
        {
            var dislist = new[] {
            new { Text = AppSettings.resourcemanager.GetString("trFree")    , Value = "f" },
            new { Text = AppSettings.resourcemanager.GetString("trMonthly") , Value = "m" },
            new { Text = AppSettings.resourcemanager.GetString("trYearly")  , Value = "y" },
            new { Text = AppSettings.resourcemanager.GetString("trOnce")    , Value = "o" },
             };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = dislist;

        }
        #endregion
        #region tables
        static public List<string> tablesStatus = new List<string>() { "empty", "opened", "reserved", "openedReserved" };
        static public Tables table = new Tables();
        static public void FillTablesStatus(ComboBox cmb)
        {
            #region fill status
            var statusList = new[] {
                 new { Text = "-", Value = "" },
                 new { Text = AppSettings.resourcemanager.GetString("trEmpty"), Value = "empty" },
                 new { Text = AppSettings.resourcemanager.GetString("trOpened"), Value = "opened" },
            new { Text = AppSettings.resourcemanager.GetString("trReserved"), Value = "reserved"},
            new { Text = AppSettings.resourcemanager.GetString("trOpenedReserved"), Value = "openedReserved"},
            };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = statusList;
            cmb.SelectedIndex = -1;
            #endregion
        }
        #endregion
        #region Email
        static public void FillSideCombo(ComboBox COMBO)
        {
            #region fill deposit to combo
            var list = new[] {
  new { Text = AppSettings.resourcemanager.GetString("trAccounting")  , Value = "accounting" },
            new { Text = AppSettings.resourcemanager.GetString("trSales")  , Value = "sales" },
            new { Text = AppSettings.resourcemanager.GetString("trPurchases")  , Value = "purchase" },

             };
            COMBO.DisplayMemberPath = "Text";
            COMBO.SelectedValuePath = "Value";
            COMBO.ItemsSource = list;
            #endregion

        }
        #endregion
        #region ItemUnitUser
        static public List<ItemUnitUser> itemUnitsUsersList;
        static public ItemUnitUser itemUnitsUser = new ItemUnitUser();
        static public async Task<List<ItemUnitUser>> RefreshItemUnitUser()
        {
            itemUnitsUsersList = await itemUnitsUser.GetByUserId(MainWindow.userLogin.userId);
            return itemUnitsUsersList;
        }


        #endregion
        #region availability type
        static public void fillAvailabilityType(ComboBox cmb)
        {
            var dislist = new[] {
            new { Text = AppSettings.resourcemanager.GetString("private"), Value = "pr" },
            new { Text = AppSettings.resourcemanager.GetString("public"), Value = "pb" },
             };
            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = dislist;
        }
        #endregion
        #region preparing Order Status
        static public void FillPreparingOrderStatusWithDefault(ComboBox cmb)
        {
            #region fill process type
            var typelist = new[] {
                new { Text = "-"       , Value = "" },
                new { Text = AppSettings.resourcemanager.GetString("trListed")       , Value = "Listed" },
                new { Text = AppSettings.resourcemanager.GetString("trPreparing") , Value = "Preparing" },
                new { Text = AppSettings.resourcemanager.GetString("trReady") , Value = "Ready" },
                //new { Text = AppSettings.resourcemanager.GetString("trDone") , Value = "Done" }, 
                 };

            cmb.DisplayMemberPath = "Text";
            cmb.SelectedValuePath = "Value";
            cmb.ItemsSource = typelist;
            cmb.SelectedIndex = 0;
            #endregion
        }
        #endregion
        #region invoice type
        static public void FillInvoiceTypeWithDefault(ComboBox cmb)
        {
            //var typelist = new[] {
            //    new { Text = "-"       , Value = "" },
            //    new { Text = AppSettings.resourcemanager.GetString("clothes")       , Value = "clothes" },
            //    new { Text = AppSettings.resourcemanager.GetString("carpets") , Value = "carpets" },
            //    new { Text = AppSettings.resourcemanager.GetString("cars") , Value = "cars" },
            //     };

            invoiceTypelist = new List<keyValueString>();
            invoiceTypelist.Add(new keyValueString { key = "", value = "-" });
            if (AppSettings.typesOfService_clothes == "1")
                invoiceTypelist.Add(new keyValueString { key = "clothes", value = AppSettings.resourcemanager.GetString("clothes") });

            if (AppSettings.typesOfService_carpets == "1")
                invoiceTypelist.Add(new keyValueString { key = "carpets", value = AppSettings.resourcemanager.GetString("carpets") });

            if (AppSettings.typesOfService_cars == "1")
                invoiceTypelist.Add(new keyValueString { key = "cars", value = AppSettings.resourcemanager.GetString("cars") });

            cmb.SelectedValuePath = "key";
            cmb.DisplayMemberPath = "value";
            cmb.ItemsSource = invoiceTypelist;
            cmb.SelectedIndex = 0;
        }
        static public List<keyValueString> invoiceTypelist;

        static public void FillInvoiceType(ComboBox cmb)
        {


            invoiceTypelist = new List<keyValueString>();
            if (AppSettings.typesOfService_clothes == "1")
                invoiceTypelist.Add(new keyValueString { key = "clothes", value = AppSettings.resourcemanager.GetString("clothes") });

            if (AppSettings.typesOfService_carpets == "1")
                invoiceTypelist.Add(new keyValueString { key = "carpets", value = AppSettings.resourcemanager.GetString("carpets") });

            if (AppSettings.typesOfService_cars == "1")
                invoiceTypelist.Add(new keyValueString { key = "cars", value = AppSettings.resourcemanager.GetString("cars") });

            cmb.SelectedValuePath = "key";
            cmb.DisplayMemberPath = "value";
            cmb.ItemsSource = invoiceTypelist;
            cmb.SelectedIndex = 0;
        }
        #endregion
        static public Pos pos = new Pos();
        static public ItemLocation itemLocation = new ItemLocation();
        static public Invoice invoice = new Invoice();
        static public List<Invoice> invoices;
        static public ShippingCompanies ShipCompany = new ShippingCompanies();
        static public List<keyValueString> UserJobListReport;
        
       
    }
}
