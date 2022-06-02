using laundryApp;
using laundryApp.ApiClasses;
using MaterialDesignThemes.Wpf;
using netoaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using Tulpep.NotificationWindow;
using System.Globalization;

namespace laundryApp.Classes
{
    class HelpClass
    {
        public static bool iscodeExist = false;
        public static Agent agentModel = new Agent();
        public static Branch branchModel = new Branch();
        public static Category categoryModel = new Category();
        public static Pos posModel = new Pos();
        public static Offer offerModel = new Offer();
        public static string code;
        static public BrushConverter brushConverter = new BrushConverter();
        public static ImageBrush imageBrush = new ImageBrush();

        static public bool isAdminPermision()
        {
            //if (MainWindow.userLogin.userId == 1 || MainWindow.userLogin.userId == 2)
            if (MainWindow.userLogin.isAdmin == true)
                return true;
            return false;
        }
        static public bool isSupportPermision()
        {
            //if (MainWindow.userLogin.userId == 1 || MainWindow.userLogin.userId == 2)
            if (MainWindow.userLogin.isAdmin == true && MainWindow.userLogin.username == "Support@Increase")
                return true;
            return false;
        }
        static public bool branchPermision(string _object, string type)
        {
            // "b" , "s"
            if (type == "b")
                return true;
            else if (type == "s")
            {
                List<string> listStorePermision = new List<string>();
                listStorePermision = new List<string> {"general_companySettings",
                    "catalog", "rawMaterials", "itemsRawMaterials", "units", "categorie",
                    "storage", "storageDivide", "storageOperations", "movementsOperations", "stocktakingOperations",
                    "locations", "storageSections", "storageCost", "storageInvoice", "itemsStorage", "storageMovements",
                    "spendingOrder", "itemsShortage", "itemsDestructive", "stocktaking",
                    "delivery", "deliveryManagement", "shippingCompanies", "driversManagement",
                    // report
                    "reports",
                    "storageReports", "stockStorageReports", "externalStorageReports", "internalStorageReports"
                    ,"directStorageReports", "stocktakingStorageReports", "destroiedStorageReports"
                    ,"purchaseReports", "invoicePurchaseReports", "itemPurchaseReports", "orderPurchaseReports"
                    ,"accountsReports", "paymentsAccountsReports", "recipientAccountsReports", "bankAccountsReports"
                    , "posAccountsReports", "statementAccountsReports", "fundAccountsReports", "profitsAccountsReports"
                    , "closingAccountsReports", "taxAccountsReports"
                    ,"deliveryReports" , "deliveryReports_view"
                    // sectionData
                    ,"sectionData", "persons", "branchesAndStores", "users", "pos", "residentialSectors",
                    //settings
                    "settings","general", "permissions"
                    // accounts
                    , "accounts" ,"posTransfers" ,"payments" ,"received" ,"banksAccounting" ,"ordersAccounting"};
                if (listStorePermision.Contains(_object))
                    return true;
            }
            return false;
        }
        public static bool validateEmpty(string str, Path p_error)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(str))
            {
                p_error.Visibility = Visibility.Visible;
                #region Tooltip
                ToolTip toolTip = new ToolTip();
                toolTip.Content = AppSettings.resourcemanager.GetString("trIsRequired");
                toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
                p_error.ToolTip = toolTip;
                #endregion
                isValid = false;
            }
            else
            {
                p_error.Visibility = Visibility.Collapsed;
            }
            return isValid;
        }
        public static bool validateEmptyCombo(ComboBox cmb, Path p_error)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(cmb.Text) || cmb.SelectedValue == null || cmb.SelectedValue.ToString() == "0")
            {
                p_error.Visibility = Visibility.Visible;
                #region Tooltip
                ToolTip toolTip = new ToolTip();
                toolTip.Content = AppSettings.resourcemanager.GetString("trIsRequired");
                toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
                p_error.ToolTip = toolTip;
                #endregion
                isValid = false;
            }
            else
            {
                p_error.Visibility = Visibility.Collapsed;
            }
            return isValid;
        }
        public static void SetValidate(Path p_error, string tr)
        {
            #region Tooltip error
            p_error.Visibility = Visibility.Visible;
            ToolTip toolTip = new ToolTip();
            toolTip.Content = AppSettings.resourcemanager.GetString(tr);
            toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
            p_error.ToolTip = toolTip;
            #endregion
        }
        public static void clearValidate(Path p_error)
        {
            p_error.Visibility = Visibility.Collapsed;
        }
        public static async Task<bool> isCodeExist(string randomNum, string type, string _class, int id)
        {
            iscodeExist = false;
            try
            {
                List<string> codes = new List<string>();

                if (_class.Equals("Agent"))
                {
                    List<Agent> agents = await agentModel.Get(type);
                    if (agents.Any(a => a.code == randomNum && a.agentId != id))
                        iscodeExist = true;
                    else
                        iscodeExist = false;
                }
                else if (_class.Equals("Branch"))
                {
                    List<Branch> branches = await branchModel.Get(type);
                    if (branches.Any(b => b.code == randomNum && b.branchId != id))
                        iscodeExist = true;
                    else
                        iscodeExist = false;
                }
                else if (_class.Equals("Category"))
                {
                    List<Category> categories = await categoryModel.GetAllCategories(MainWindow.userLogin.userId);
                    if (categories.Any(c => c.categoryCode == randomNum && c.categoryId != id))
                        iscodeExist = true;
                    else
                        iscodeExist = false;
                }
                else if (_class.Equals("Pos"))
                {
                    List<Pos> poss = await posModel.Get();

                    if (poss.Any(p => p.code == randomNum && p.posId != id))
                        iscodeExist = true;
                    else
                        iscodeExist = false;
                }
                else if (_class.Equals("Offer"))
                {
                    List<Offer> offer = await offerModel.Get();

                    if (offer.Any(o => o.code == randomNum && o.offerId != id))
                        iscodeExist = true;
                    else
                        iscodeExist = false;
                }
                //if (codes.Contains(randomNum.Trim()))
                //    iscodeExist = true;
                //else
                //    iscodeExist = false;

            }
            catch { }
            return iscodeExist;
        }
        #region validate
        public static bool validate(List<string> requiredControlList, UserControl userControl)
        {
            bool isValid = true;
            try
            {
                //TextBox
                foreach (var control in requiredControlList)
                {
                    TextBox textBox = FindControls.FindVisualChildren<TextBox>(userControl).Where(x => x.Name == "tb_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (textBox != null && path != null)
                        if (!HelpClass.validateEmpty(textBox.Text, path))
                            isValid = false;
                }
                //ComboBox
                foreach (var control in requiredControlList)
                {
                    ComboBox comboBox = FindControls.FindVisualChildren<ComboBox>(userControl).Where(x => x.Name == "cb_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (comboBox != null && path != null)
                        if (!HelpClass.validateEmptyCombo(comboBox, path))
                            isValid = false;
                }
                //TextBox
                foreach (var control in requiredControlList)
                {
                    TextBlock textBlock = FindControls.FindVisualChildren<TextBlock>(userControl).Where(x => x.Name == "txt_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (textBlock != null && path != null)
                        if (!HelpClass.validateEmpty(textBlock.Text, path))
                            isValid = false;
                }
                //DatePicker
                foreach (var control in requiredControlList)
                {
                    DatePicker datePicker = FindControls.FindVisualChildren<DatePicker>(userControl).Where(x => x.Name == "dp_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (datePicker != null && path != null)
                        if (!HelpClass.validateEmpty(datePicker.Text, path))
                            isValid = false;
                }
                //TimePicker
                foreach (var control in requiredControlList)
                {
                    TimePicker timePicker = FindControls.FindVisualChildren<TimePicker>(userControl).Where(x => x.Name == "tp_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (timePicker != null && path != null)
                        if (!HelpClass.validateEmpty(timePicker.Text, path))
                            isValid = false;
                }
                //PasswordBox
                foreach (var control in requiredControlList)
                {
                    PasswordBox passwordBox = FindControls.FindVisualChildren<PasswordBox>(userControl).Where(x => x.Name == "pb_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (passwordBox != null && path != null)
                        if (!HelpClass.validateEmpty(passwordBox.Password, path))
                            isValid = false;
                }
                #region Email
                IsValidEmail(userControl);
                #endregion


            }
            catch { }
            return isValid;
        }
        public static bool validate(List<string> requiredControlList, Window userControl)
        {
            bool isValid = true;
            try
            {
                //TextBox
                foreach (var control in requiredControlList)
                {
                    TextBox textBox = FindControls.FindVisualChildren<TextBox>(userControl).Where(x => x.Name == "tb_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (textBox != null && path != null)
                        if (!HelpClass.validateEmpty(textBox.Text, path))
                            isValid = false;
                }
                //ComboBox
                foreach (var control in requiredControlList)
                {
                    ComboBox comboBox = FindControls.FindVisualChildren<ComboBox>(userControl).Where(x => x.Name == "cb_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (comboBox != null && path != null)
                        if (!HelpClass.validateEmptyCombo(comboBox, path))
                            isValid = false;
                }
                //TextBox
                foreach (var control in requiredControlList)
                {
                    TextBlock textBlock = FindControls.FindVisualChildren<TextBlock>(userControl).Where(x => x.Name == "txt_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (textBlock != null && path != null)
                        if (!HelpClass.validateEmpty(textBlock.Text, path))
                            isValid = false;
                }
                //DatePicker
                foreach (var control in requiredControlList)
                {
                    DatePicker datePicker = FindControls.FindVisualChildren<DatePicker>(userControl).Where(x => x.Name == "dp_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (datePicker != null && path != null)
                        if (!HelpClass.validateEmpty(datePicker.Text, path))
                            isValid = false;
                }
                //TimePicker
                foreach (var control in requiredControlList)
                {
                    TimePicker timePicker = FindControls.FindVisualChildren<TimePicker>(userControl).Where(x => x.Name == "tp_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (timePicker != null && path != null)
                        if (!HelpClass.validateEmpty(timePicker.Text, path))
                            isValid = false;
                }
                //PasswordBox
                foreach (var control in requiredControlList)
                {
                    PasswordBox passwordBox = FindControls.FindVisualChildren<PasswordBox>(userControl).Where(x => x.Name == "pb_" + control)
                        .FirstOrDefault();
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (passwordBox != null && path != null)
                        if (!HelpClass.validateEmpty(passwordBox.Password, path))
                            isValid = false;
                }
                #region Email
                IsValidEmail(userControl);
                #endregion


            }
            catch { }
            return isValid;
        }
        public static bool IsValidEmail(UserControl userControl)
        {//for email
            bool isValidEmail = true;
            TextBox textBoxEmail = FindControls.FindVisualChildren<TextBox>(userControl).Where(x => x.Name == "tb_email")
                    .FirstOrDefault();
            Path pathEmail = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_email")
                    .FirstOrDefault();
            if (textBoxEmail != null && pathEmail != null)
            {
                if (textBoxEmail.Text.Equals(""))
                    return isValidEmail;
                else
                {
                    Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                          RegexOptions.CultureInvariant | RegexOptions.Singleline);
                    isValidEmail = regex.IsMatch(textBoxEmail.Text);

                    if (!isValidEmail)
                    {
                        pathEmail.Visibility = Visibility.Visible;
                        #region Tooltip
                        ToolTip toolTip = new ToolTip();
                        toolTip.Content = AppSettings.resourcemanager.GetString("trErrorEmailToolTip");
                        toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
                        pathEmail.ToolTip = toolTip;
                        #endregion
                        isValidEmail = false;
                    }
                    else
                    {
                        pathEmail.Visibility = Visibility.Collapsed;
                    }
                }
            }
            return isValidEmail;

        }
        public static bool IsValidEmail(Window userControl)
        {//for email
            bool isValidEmail = true;
            TextBox textBoxEmail = FindControls.FindVisualChildren<TextBox>(userControl).Where(x => x.Name == "tb_email")
                    .FirstOrDefault();
            Path pathEmail = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_email")
                    .FirstOrDefault();
            if (textBoxEmail != null && pathEmail != null)
            {
                if (textBoxEmail.Text.Equals(""))
                    return isValidEmail;
                else
                {
                    Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                          RegexOptions.CultureInvariant | RegexOptions.Singleline);
                    isValidEmail = regex.IsMatch(textBoxEmail.Text);

                    if (!isValidEmail)
                    {
                        pathEmail.Visibility = Visibility.Visible;
                        #region Tooltip
                        ToolTip toolTip = new ToolTip();
                        toolTip.Content = AppSettings.resourcemanager.GetString("trErrorEmailToolTip");
                        toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
                        pathEmail.ToolTip = toolTip;
                        #endregion
                        isValidEmail = false;
                    }
                    else
                    {
                        pathEmail.Visibility = Visibility.Collapsed;
                    }
                }
            }
            return isValidEmail;

        }
        public static void clearValidate(List<string> requiredControlList, UserControl userControl)
        {
            try
            {
                foreach (var control in requiredControlList)
                {
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (path != null)
                        HelpClass.clearValidate(path);
                }
            }
            catch { }
        }
        public static void clearValidate(List<string> requiredControlList, Window userControl)
        {
            try
            {
                foreach (var control in requiredControlList)
                {
                    Path path = FindControls.FindVisualChildren<Path>(userControl).Where(x => x.Name == "p_error_" + control)
                        .FirstOrDefault();
                    if (path != null)
                        HelpClass.clearValidate(path);
                }
            }
            catch { }
        }
        #endregion

        public static void getMobile(string _mobile, ComboBox _area, TextBox _tb)
        {//mobile
            if ((_mobile != null))
            {
                string area = _mobile;
                string[] pharr = area.Split('-');
                int j = 0;
                string phone = "";

                foreach (string strpart in pharr)
                {
                    if (j == 0)
                    {
                        area = strpart;
                    }
                    else
                    {
                        phone = phone + strpart;
                    }
                    j++;
                }

                _area.Text = area;

                _tb.Text = phone.ToString();
            }
            else
            {
                _area.SelectedIndex = -1;
                _tb.Clear();
            }
        }

        public static void getPhone(string _phone, ComboBox _area, ComboBox _areaLocal, TextBox _tb)
        {//phone
            if ((_phone != null))
            {
                string area = _phone;
                string[] pharr = area.Split('-');
                int j = 0;
                string phone = "";
                string areaLocal = "";
                foreach (string strpart in pharr)
                {
                    if (j == 0)
                        area = strpart;
                    else if (j == 1)
                        areaLocal = strpart;
                    else
                        phone = phone + strpart;
                    j++;
                }

                _area.Text = area;
                _areaLocal.Text = areaLocal;
                _tb.Text = phone.ToString();
            }
            else
            {
                _area.SelectedIndex = -1;
                _areaLocal.SelectedIndex = -1;
                _tb.Clear();
            }
        }

        public static void clearImg(Button img)
        {
            Uri resourceUri = new Uri("pic/no-image-icon-125x125.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            imageBrush.ImageSource = temp;
            img.Background = imageBrush;
        }
        public static decimal calcPercentage(decimal value, decimal percentage)
        {
            decimal percentageVal = (value * percentage) / 100;

            return percentageVal;
        }
        public static void defaultDatePickerStyle(DatePicker dp)
        {
            dp.Loaded += delegate
            {
                var textBox1 = (TextBox)dp.Template.FindName("PART_TextBox", dp);
                if (textBox1 != null)
                {
                    textBox1.Background = dp.Background;
                    textBox1.BorderThickness = dp.BorderThickness;
                }
            };
        }
        public static string DateTodbString(DateTime? date)
        {
            string sdate = "";
            if (date != null)
            {

                //"yyyy'-'MM'-'dd'T'HH':'mm':'ss"
                sdate = date.Value.ToString("yyyy'-'MM'-'dd");


            }

            return sdate;
        }
        static public void searchInComboBox(ComboBox cbm)
        {
            CollectionView itemsViewOriginal = (CollectionView)CollectionViewSource.GetDefaultView(cbm.Items);
            itemsViewOriginal.Filter = ((o) =>
            {
                if (String.IsNullOrEmpty(cbm.Text)) return true;
                else
                {
                    if (((string)o).Contains(cbm.Text)) return true;
                    else return false;
                }
            });
            itemsViewOriginal.Refresh();
        }
        public static async void getImg(string type, string imageUri, Button button)
        {
            try
            {

                //if (string.IsNullOrEmpty(category.image))
                //{
                //    SectionData.clearImg(button);
                //}
                //else
                //{

                if (type.Equals("Category"))
                {
                    Category category = new Category();
                    byte[] imageBuffer = await category.downloadImage(imageUri); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }
                else if (type.Equals("Item"))
                {
                    Item item = new Item();
                    byte[] imageBuffer = await item.downloadImage(imageUri); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }
                else if (type.Equals("User"))
                {
                    User user = new User();
                    byte[] imageBuffer = await user.downloadImage(imageUri); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }
                else if (type.Equals("Agent"))
                {
                    Agent agent = new Agent();
                    byte[] imageBuffer = await agent.downloadImage(imageUri); // read this as BLOB from your DB
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }

                //}
            }
            catch
            {
                clearImg(button);
            }
        }
        public static async void getLocalImg(string type, string imageUri, Button button)
        {
            try
            {

                if (type.Equals("Category"))
                {
                    Category category = new Category();
                    byte[] imageBuffer = readLocalImage(imageUri, Global.TMPFolder);
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }
                else if (type.Equals("Item"))
                {
                    Item item = new Item();
                    byte[] imageBuffer = readLocalImage(imageUri, Global.TMPItemsFolder);
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }
                else if (type.Equals("User"))
                {
                    User user = new User();
                    byte[] imageBuffer = readLocalImage(imageUri, Global.TMPUsersFolder);
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }
                else if (type.Equals("Agent"))
                {
                    Agent agent = new Agent();
                    byte[] imageBuffer = readLocalImage(imageUri, Global.TMPAgentsFolder);
                    var bitmapImage = new BitmapImage();
                    using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }
                    button.Background = new ImageBrush(bitmapImage);
                }

                //}
            }
            catch
            {
                clearImg(button);
            }
        }
        public static bool chkImgChng(string imageName, DateTime updateDate, string TMPFolder)
        {
            try
            {
                string dir = System.IO.Directory.GetCurrentDirectory();
                string tmpPath = System.IO.Path.Combine(dir, TMPFolder);
                tmpPath = System.IO.Path.Combine(tmpPath, imageName);
                DateTime mofdifyDate;
                if (!System.IO.File.Exists(tmpPath))
                {
                    return true;
                }
                else
                {
                    mofdifyDate = System.IO.File.GetLastWriteTime(tmpPath);
                    if (mofdifyDate < updateDate)
                        return true;
                }
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static byte[] readLocalImage(string imageName, string TMPFolder)
        {
            byte[] data = null;
            string dir = System.IO.Directory.GetCurrentDirectory();
            string tmpPath = System.IO.Path.Combine(dir, TMPFolder);
            if (!System.IO.Directory.Exists(tmpPath))
                System.IO.Directory.CreateDirectory(tmpPath);
            tmpPath = System.IO.Path.Combine(tmpPath, imageName);
            if (System.IO.File.Exists(tmpPath))
            {
                // Load file meta data with FileInfo
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(tmpPath);
                // The byte[] to save the data in
                data = new byte[fileInfo.Length];
                using (var stream = new System.IO.FileStream(tmpPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    stream.Read(data, 0, data.Length);
                }
                // Delete the temporary file
                // fileInfo.Delete();
            }
            return data;
        }
        public static string DateToString(DateTime? date)
        {
            string sdate = "";
            if (date != null)
            {
                DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;

                switch (AppSettings.dateFormat)
                {
                    case "ShortDatePattern":
                        sdate = date.Value.ToString(dtfi.ShortDatePattern);
                        break;
                    case "LongDatePattern":
                        sdate = date.Value.ToString(dtfi.LongDatePattern);
                        break;
                    case "MonthDayPattern":
                        sdate = date.Value.ToString(dtfi.MonthDayPattern);
                        break;
                    case "YearMonthPattern":
                        sdate = date.Value.ToString(dtfi.YearMonthPattern);
                        break;
                    default:
                        sdate = date.Value.ToString(dtfi.ShortDatePattern);
                        break;
                }
            }

            return sdate;
        }
        public static string DecTostring(decimal? dec)
        {
            string sdc = "0";
            if (dec == null)
            {

            }
            else
            {
                decimal dc = decimal.Parse(dec.ToString());

                switch (AppSettings.accuracy)
                {
                    case "0":
                        sdc = string.Format("{0:F0}", dc);
                        break;
                    case "1":
                        sdc = string.Format("{0:F1}", dc);
                        break;
                    case "2":
                        sdc = string.Format("{0:F2}", dc);
                        break;
                    case "3":
                        sdc = string.Format("{0:F3}", dc);
                        break;
                    default:
                        sdc = string.Format("{0:F1}", dc);
                        break;
                }

            }


            return sdc;
        }
        public static string PercentageDecTostring(decimal? dec)
        {
            string sdc = DecTostring(dec);

            sdc = string.Format("{0:G29}", decimal.Parse(sdc));
            return sdc;
        }
        /// <summary>
        /// لمنع  الصفر بالبداية
        /// </summary>
        /// <param name="txb"></param>
        static public void InputJustNumber(ref TextBox txb)
        {
            if (txb.Text.Count() == 2 && txb.Text == "00")
            {
                string myString = txb.Text;
                myString = Regex.Replace(myString, "00", "0");
                txb.Text = myString;
                txb.Select(txb.Text.Length, 0);
                txb.Focus();
            }
        }
        static async public void ExceptionMessage(Exception ex, object window)
        {
            try
            {
                

                //Message
                if (ex.HResult == -2146233088)
                    Toaster.ShowError(window as Window, message: AppSettings.resourcemanager.GetString("trNoConnection"), animation: ToasterAnimation.FadeIn);
                if (ex.HResult == -2147467261)
                {
                    //Void MoveNext()
                    //-2147467261

                    // dont't show message
                }
                else
                    Toaster.ShowError(window as Window, message: ex.HResult + " || " + ex.Message, animation: ToasterAnimation.FadeIn);

                ErrorClass errorClass = new ErrorClass();
                errorClass.num = ex.HResult.ToString();
                errorClass.msg = ex.Message;
                errorClass.stackTrace = ex.StackTrace;
                errorClass.targetSite = ex.TargetSite.ToString();
                errorClass.createUserId = MainWindow.userLogin.userId;
                await errorClass.save(errorClass);
            }
            catch
            {

            }
        }
        static public void StartAwait(Grid grid, string progressRingName = "")
        {
            grid.IsEnabled = false;
            grid.Opacity = 0.6;
            MahApps.Metro.Controls.ProgressRing progressRing = new MahApps.Metro.Controls.ProgressRing();
            progressRing.Name = "prg_awaitRing" + progressRingName;
            progressRing.Foreground = App.Current.Resources["MainColor"] as Brush;
            progressRing.IsActive = true;
            Grid.SetRowSpan(progressRing, 10);
            Grid.SetColumnSpan(progressRing, 10);
            grid.Children.Add(progressRing);
        }
        static public void EndAwait(Grid grid, string progressRingName = "")
        {
            MahApps.Metro.Controls.ProgressRing progressRing = FindControls.FindVisualChildren<MahApps.Metro.Controls.ProgressRing>(grid)
                .Where(x => x.Name == "prg_awaitRing" + progressRingName).FirstOrDefault();
            grid.Children.Remove(progressRing);

            var progressRingList = FindControls.FindVisualChildren<MahApps.Metro.Controls.ProgressRing>(grid)
                 .Where(x => x.Name == "prg_awaitRing" + progressRingName);
            if (progressRingList.Count() == 0)
            {
                grid.IsEnabled = true;
                grid.Opacity = 1;
            }

        }
        /// <summary>
        /// badged name , previous count, new count
        /// </summary>
        /// <param name="badged">badged name</param>
        /// <param name="_count">previous count</param>
        /// <param name="count">new count</param>
        static public void refreshNotification(Badged badged, ref int _count, int count)
        {
            if (count != _count)
            {
                if (count > 9)
                {
                    badged.Badge = "+9";
                }
                else if (count == 0) badged.Badge = "";
                else
                    badged.Badge = count.ToString();
            }
            _count = count;
        }
        static public void drawBarcode(string barcodeStr, Image img)
        {//barcode image
            // create encoding object
            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;

            if (barcodeStr != "")
            {
                System.Drawing.Bitmap serial_bitmap = (System.Drawing.Bitmap)barcode.Draw(barcodeStr, 60);
                System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();
                //generate bitmap
                img.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(serial_bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
                img.Source = null;
        }
        static public void activateCategoriesButtons(List<Item> items, List<Category> categories, List<Button> btns)
        {
            foreach (Category cat in categories)
            {
                string btn_name = "btn_" + cat.categoryCode;
                Button categoryBtn = new Button();
                foreach (Button btn in btns)
                {
                    if (btn.Name == btn_name)
                    {
                        categoryBtn = btn;
                        break;
                    }
                }

                var isExist = items.Where(x => x.categoryId == cat.categoryId).FirstOrDefault();
                if (isExist == null)
                    categoryBtn.IsEnabled = false;
                else
                    categoryBtn.IsEnabled = true;
            }
        }
        static public void activateCategoriesButtons(List<MenuSetting> items, List<Category> categories, List<Button> btns)
        {
            foreach (Category cat in categories)
            {
                string btn_name = "btn_" + cat.categoryCode;
                Button categoryBtn = new Button();
                foreach (Button btn in btns)
                {
                    if (btn.Name == btn_name)
                    {
                        categoryBtn = btn;
                        break;
                    }
                }

                var isExist = items.Where(x => x.categoryId == cat.categoryId).FirstOrDefault();
                if (isExist == null)
                    categoryBtn.IsEnabled = false;
                else
                    categoryBtn.IsEnabled = true;
            }
        }
        public async static void ReportTabTitle(TextBlock textBlock, string firstTitle, string secondTitle)
        {

            //////////////////////////////////////////////////////////////////////////////
            if (FillCombo.objectsList is null)
                await FillCombo.RefreshObjects();
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == firstTitle).FirstOrDefault().translate))
                firstTitle = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == firstTitle).FirstOrDefault().translate
               );
            #region
            //if (firstTitle == "invoiceSalesReports" || firstTitle == "invoicePurchaseReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trInvoices");
            //else if (firstTitle == "itemSalesReports" || firstTitle == "itemPurchaseReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trItems");
            //else if (firstTitle == "orderSalesReports" || firstTitle == "orderPurchaseReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trOrders");
            //else if (firstTitle == "promotionSalesReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trPromotion");
            //else if (firstTitle == "quotationSalesReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trQuotations");
            //else if (firstTitle == "stockStorageReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trStock");
            //else if (firstTitle == "stocktakingStorageReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trStocktaking");
            //else if (firstTitle == "dailySalesReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trDailySales");
            //else if (firstTitle == "taxAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trTax");
            //else if (firstTitle == "internalStorageReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trInternal");
            //else if (firstTitle == "externalStorageReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trExternal");
            //else if (firstTitle == "directStorageReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trDirectEntry");
            //else if (firstTitle == "destroiedStorageReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trDestructives");
            //else if (firstTitle == "recipientAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trReceived");
            //else if (firstTitle == "statementAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trAccountStatement");
            //else if (firstTitle == "paymentsAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trPayments");
            //else if (firstTitle == "posAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trPOS");
            //else if (firstTitle == "profitsAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trProfits");
            //else if (firstTitle == "fundAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trCashBalance");
            //else if (firstTitle == "bankAccountsReports")
            //    firstTitle = AppSettings.resourcemanager.GetString("trBanks");
            #endregion
            //////////////////////////////////////////////////////////////////////////////

            if (secondTitle == "branch")
                secondTitle = AppSettings.resourcemanager.GetString("trBranches");
            else if (secondTitle == "pos")
                secondTitle = AppSettings.resourcemanager.GetString("trPOS");
            else if (secondTitle == "vendors" || secondTitle == "vendor")
                secondTitle = AppSettings.resourcemanager.GetString("trVendors");
            else if (secondTitle == "customers" || secondTitle == "customer")
                secondTitle = AppSettings.resourcemanager.GetString("trCustomers");
            else if (secondTitle == "users" || secondTitle == "user")
                secondTitle = AppSettings.resourcemanager.GetString("trUsers");
            else if (secondTitle == "items" || secondTitle == "item")
                secondTitle = AppSettings.resourcemanager.GetString("trItems");
            else if (secondTitle == "coupon")
                secondTitle = AppSettings.resourcemanager.GetString("trCoupon");
            else if (secondTitle == "coupons")
                secondTitle = AppSettings.resourcemanager.GetString("trCoupons");
            else if (secondTitle == "offers")
                secondTitle = AppSettings.resourcemanager.GetString("trOffer");
            else if (secondTitle == "invoice")
                secondTitle = AppSettings.resourcemanager.GetString("tr_Invoice");
            else if (secondTitle == "invoices")
                secondTitle = AppSettings.resourcemanager.GetString("trInvoices");
            else if (secondTitle == "order")
                secondTitle = AppSettings.resourcemanager.GetString("trOrders");
            else if (secondTitle == "quotation")
                secondTitle = AppSettings.resourcemanager.GetString("trQuotations");
            else if (secondTitle == "operator")
                secondTitle = AppSettings.resourcemanager.GetString("trOperations");
            else if (secondTitle == "payments")
                secondTitle = AppSettings.resourcemanager.GetString("trPayments");
            else if (secondTitle == "recipient")
                secondTitle = AppSettings.resourcemanager.GetString("trRecepient");
            else if (secondTitle == "received")
                secondTitle = AppSettings.resourcemanager.GetString("trReceived");
            else if (secondTitle == "destroied")
                secondTitle = AppSettings.resourcemanager.GetString("trDestructives");
            else if (secondTitle == "agent")
                secondTitle = AppSettings.resourcemanager.GetString("trCustomers");
            else if (secondTitle == "stock")
                secondTitle = AppSettings.resourcemanager.GetString("trStock");
            else if (secondTitle == "external")
                secondTitle = AppSettings.resourcemanager.GetString("trExternal");
            else if (secondTitle == "internal")
                secondTitle = AppSettings.resourcemanager.GetString("trInternal");
            else if (secondTitle == "stocktaking")
                secondTitle = AppSettings.resourcemanager.GetString("trStocktaking");
            else if (secondTitle == "archives")
                secondTitle = AppSettings.resourcemanager.GetString("trArchive");
            else if (secondTitle == "shortfalls")
                secondTitle = AppSettings.resourcemanager.GetString("trShortages");
            else if (secondTitle == "location")
                secondTitle = AppSettings.resourcemanager.GetString("trLocations");
            else if (secondTitle == "collect")
                secondTitle = AppSettings.resourcemanager.GetString("trCollect");
            else if (secondTitle == "bestselling")
                secondTitle = AppSettings.resourcemanager.GetString("trBestSeller");
            else if (secondTitle == "bestbuys")
                secondTitle = AppSettings.resourcemanager.GetString("trMostPurchased");
            else if (secondTitle == "shipping")
                secondTitle = AppSettings.resourcemanager.GetString("trShipping");
            else if (secondTitle == "shippings")
                secondTitle = AppSettings.resourcemanager.GetString("trShippingCompanies");
            else if (secondTitle == "salary")
                secondTitle = AppSettings.resourcemanager.GetString("trSalary");
            else if (secondTitle == "salaries")
                secondTitle = AppSettings.resourcemanager.GetString("trSalaries");
            else if (secondTitle == "generalExpenses")
                secondTitle = AppSettings.resourcemanager.GetString("trGeneralExpenses");
            else if (secondTitle == "administrativePull")
                secondTitle = AppSettings.resourcemanager.GetString("trAdministrativePull");
            else if (secondTitle == "administrativePulls")
                secondTitle = AppSettings.resourcemanager.GetString("trAdministrativePulls");
            else if (secondTitle == "administrativeDeposit")
                secondTitle = AppSettings.resourcemanager.GetString("trAdministrativeDeposit");
            else if (secondTitle == "administrativeDeposits")
                secondTitle = AppSettings.resourcemanager.GetString("trAdministrativeDeposits");
            else if (secondTitle == "deposit")
                secondTitle = AppSettings.resourcemanager.GetString("trDeposit");
            else if (secondTitle == "receive")
                secondTitle = AppSettings.resourcemanager.GetString("trReceive");
            else if (secondTitle == "invoice")
                secondTitle = AppSettings.resourcemanager.GetString("trInvoice");
            else if (secondTitle == "deposits")
                secondTitle = AppSettings.resourcemanager.GetString("trDeposits");
            else if (secondTitle == "receipts")
                secondTitle = AppSettings.resourcemanager.GetString("trReceipts");
            else if (secondTitle == "preparingOrders")
                secondTitle = AppSettings.resourcemanager.GetString("trPreparingOrders");
            else if (secondTitle == "pull")
                secondTitle = AppSettings.resourcemanager.GetString("trPull");
            else if (secondTitle == "cash")
                secondTitle = AppSettings.resourcemanager.GetString("trCash_");
            else if (secondTitle == "invoicesClasses")
                secondTitle = AppSettings.resourcemanager.GetString("invoicesClasses");
            else if (secondTitle == "membership")
                secondTitle = AppSettings.resourcemanager.GetString("trMembership");
            else if (secondTitle == "delivery")
                secondTitle = AppSettings.resourcemanager.GetString("trDelivery");
            //////////////////////////////////////////////////////////////////////////////

            textBlock.Text = firstTitle + " / " + secondTitle;

        }


        public static string decimalToTime(decimal remainingTime)
        {
            TimeSpan span = TimeSpan.FromMinutes(double.Parse(remainingTime.ToString()));
            var timeArr = span.ToString().Split(':');

            var hoursToMinutes = int.Parse(timeArr[0]) * 60;

            timeArr[1] = (int.Parse(timeArr[1]) + hoursToMinutes).ToString();

            //string label = (int)span.TotalMinutes + ":" + span.Seconds;
            string label = timeArr[1].ToString().PadLeft(2, '0') + ":" + Math.Round(decimal.Parse(timeArr[2])).ToString().PadLeft(2, '0');
            return label;
        }
        static public string translate_days(string str)
        {
            switch (str)
            {
                case "sat":
                    return AppSettings.resourcemanager.GetString("trSaturday"); ;
                //break;
                case "sun":
                    return AppSettings.resourcemanager.GetString("trSunday");
                //break;
                case "mon":
                    return AppSettings.resourcemanager.GetString("trMonday");
                //break;
                case "tues":
                    return AppSettings.resourcemanager.GetString("trTuesday");
                //break;
                case "wed":
                    return AppSettings.resourcemanager.GetString("trWednsday");
                //break;
                case "thur":
                    return AppSettings.resourcemanager.GetString("trThursday");
                //break;
                case "fri":
                    return AppSettings.resourcemanager.GetString("trFriday");
            //break;
            default: return str;
                    //break;
            }

        }
    }
}
