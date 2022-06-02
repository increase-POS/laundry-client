using laundryApp.Classes;
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
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Resources;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Text.RegularExpressions;
using netoaster;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls.Primitives;
using laundryApp.ApiClasses;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_companyInfo.xaml
    /// </summary>
    public partial class wd_companyInfo : Window
    {

        //phone variabels
        IEnumerable<CountryCode> countrynum;
        IEnumerable<City> citynum;
        IEnumerable<City> citynumofcountry;
        string imgFileName = "pic/no-image-icon-125x125.png";
        bool isImgPressed = false;

        int? countryid;
        bool firstchangephone = true;
        bool firstchangefax = true;

        CountryCode countrycodes = new CountryCode();
        City cityCodes = new City();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        ImageBrush brush = new ImageBrush();
        BrushConverter bc = new BrushConverter();
        int nameId = 0, addressId = 0, emailId = 0, mobileId = 0, phoneId = 0, faxId = 0, logoId = 0;
        SettingCls set = new SettingCls();
        SetValues setVName = new SetValues(); SetValues setVAddress = new SetValues(); SetValues setVEmail = new SetValues();
        SetValues setVMobile = new SetValues();
        SetValues setVPhone = new SetValues();
        SetValues setVFax = new SetValues();
        SetValues setVLogo = new SetValues();
        SettingCls setModel = new SettingCls();
        SetValues valueModel = new SetValues();
        public static List<string> requiredControlList;

        public bool isFirstTime = false;

        public wd_companyInfo()
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
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {
                
                    HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "name", "address", "email", "mobile" };

                /*
                await fillCountries();
                await fillCity();
                */
                await FillCombo.fillCountries(cb_areaMobile);
                await FillCombo.fillCountries(cb_areaPhone);
                await FillCombo.fillCountries(cb_areaFax);

                List<SettingCls> settingsCls = await setModel.GetAll();
                List<SetValues> settingsValues = await valueModel.GetAll();

                #region get settings Ids

                //get company name id
                set = settingsCls.Where(s => s.name == "com_name").FirstOrDefault<SettingCls>();
                nameId = set.settingId;
                setVName = settingsValues.Where(i => i.settingId == nameId).FirstOrDefault();
                //get company address id
                set = settingsCls.Where(s => s.name == "com_address").FirstOrDefault<SettingCls>();
                addressId = set.settingId;
                setVAddress = settingsValues.Where(i => i.settingId == addressId).FirstOrDefault();
                //get company email id
                set = settingsCls.Where(s => s.name == "com_email").FirstOrDefault<SettingCls>();
                emailId = set.settingId;
                setVEmail = settingsValues.Where(i => i.settingId == emailId).FirstOrDefault();
                //get company mobile id
                set = settingsCls.Where(s => s.name == "com_mobile").FirstOrDefault<SettingCls>();
                mobileId = set.settingId;
                setVMobile = settingsValues.Where(i => i.settingId == mobileId).FirstOrDefault();
                //get company phone id
                set = settingsCls.Where(s => s.name == "com_phone").FirstOrDefault<SettingCls>();
                phoneId = set.settingId;
                setVPhone = settingsValues.Where(i => i.settingId == phoneId).FirstOrDefault();
                //get company fax id
                set = settingsCls.Where(s => s.name == "com_fax").FirstOrDefault<SettingCls>();
                faxId = set.settingId;
                setVFax = settingsValues.Where(i => i.settingId == faxId).FirstOrDefault();
                //get company logo id
                set = settingsCls.Where(s => s.name == "com_logo").FirstOrDefault<SettingCls>();
                logoId = set.settingId;
                setVLogo = settingsValues.Where(i => i.settingId == logoId).FirstOrDefault();
                #endregion

                if (!isFirstTime)
                {
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

                    #region get values

                    //get company name
                    tb_name.Text = setVName.value;
                    //get company address
                    tb_address.Text = setVAddress.value;
                    //get company email
                    tb_email.Text = setVEmail.value;
                    //get company mobile
                    HelpClass.getMobile(setVMobile.value, cb_areaMobile, tb_mobile);
                    //get company phone
                    HelpClass.getPhone(setVPhone.value, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
                    //get company fax
                    HelpClass.getPhone(setVFax.value, cb_areaFax, cb_areaFaxLocal, tb_fax);
                    //get company logo
                    await getImg();
                    #endregion

                }
                
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
            txt_companyInfo.Text = AppSettings.resourcemanager.GetString("trComInfo");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_address, AppSettings.resourcemanager.GetString("trAdressHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_mobile, AppSettings.resourcemanager.GetString("trMobileHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_phone, AppSettings.resourcemanager.GetString("trPhoneHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_email, AppSettings.resourcemanager.GetString("trEmailHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_fax, AppSettings.resourcemanager.GetString("trFaxHint"));
            //tt_name.Content = AppSettings.resourcemanager.GetString("trName");
            //tt_mobile.Content = AppSettings.resourcemanager.GetString("trMobile");
            //tt_phone.Content = AppSettings.resourcemanager.GetString("trPhone");
            //tt_fax.Content = AppSettings.resourcemanager.GetString("trFax");
            //tt_email.Content = AppSettings.resourcemanager.GetString("trEmail");
            //tt_address.Content = AppSettings.resourcemanager.GetString("trAddress");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
        }


        private void Img_customer_Click(object sender, RoutedEventArgs e)
        {//select image

            try
            {
                
                    HelpClass.StartAwait(grid_main);

                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    brush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    img_customer.Background = brush;
                    imgFileName = openFileDialog.FileName;
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        //area code methods
        /*
        async Task<IEnumerable<CountryCode>> RefreshCountry()
        {
            countrynum = await countrycodes.GetAllCountries();
            return countrynum;
        }
        private async Task fillCountries()
        {
            if (countrynum is null)
                await RefreshCountry();

            cb_areaPhone.ItemsSource = countrynum.ToList();
            cb_areaPhone.SelectedValuePath = "countryId";
            cb_areaPhone.DisplayMemberPath = "code";

            cb_areaMobile.ItemsSource = countrynum.ToList();
            cb_areaMobile.SelectedValuePath = "countryId";
            cb_areaMobile.DisplayMemberPath = "code";

            cb_areaFax.ItemsSource = countrynum.ToList();
            cb_areaFax.SelectedValuePath = "countryId";
            cb_areaFax.DisplayMemberPath = "code";

            //cb_areaMobile.SelectedIndex = 8;
            //cb_areaPhone.SelectedIndex = 8;
            //cb_areaFax.SelectedIndex = 8;
            cb_areaMobile.SelectedValue = FillCombo.Region.countryId;
            cb_areaPhone.SelectedValue = FillCombo.Region.countryId;
            cb_areaFax.SelectedValue = FillCombo.Region.countryId;
        }
        async Task<IEnumerable<City>> RefreshCity()
        {
            citynum = await cityCodes.Get();
            return citynum;
        }
        private async Task fillCity()
        {
            if (citynum is null)
                await RefreshCity();
        }
        */
        //end areacod
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }







        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {//decimal
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

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);


                if (e.Key == Key.Return)
                {
                    Btn_save_Click(null, null);
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save

            try
            {
                
                    HelpClass.StartAwait(grid_main);

                #region validate
                if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                {
                    //save name
                    if (!tb_name.Text.Equals(""))
                    {
                        setVName.value = tb_name.Text;
                        setVName.isSystem = 1;
                        setVName.isDefault = 1;
                        setVName.settingId = nameId;
                        // string sName = await valueModel.Save(setVName);
                        int sName = await valueModel.Save(setVName);
                        if (!sName.Equals(0))
                            AppSettings.companyName = tb_name.Text;
                    }
                    //save address
                    if (!tb_address.Text.Equals(""))
                    {
                        setVAddress.value = tb_address.Text;
                        setVAddress.isSystem = 1;
                        setVAddress.isDefault = 1;
                        setVAddress.settingId = addressId;
                        int sAddress = await valueModel.Save(setVAddress);
                        //   string sAddress = await valueModel.Save(setVAddress);
                        if (!sAddress.Equals(0))
                            AppSettings.Address = tb_address.Text;
                    }
                    //save email
                    if ((!tb_email.Text.Equals("")))
                    {
                        setVEmail.value = tb_email.Text;
                        setVEmail.isSystem = 1;
                        setVEmail.settingId = emailId;
                        setVEmail.isDefault = 1;
                        //  string sEmail = await valueModel.Save(setVEmail);
                        int sEmail = await valueModel.Save(setVEmail);
                        if (!sEmail.Equals(0))
                            AppSettings.Email = tb_email.Text;
                    }
                    //save mobile
                    if (!tb_mobile.Text.Equals(""))
                    {
                        setVMobile.value = cb_areaMobile.Text + "-" + tb_mobile.Text;
                        setVMobile.isSystem = 1;
                        setVMobile.isDefault = 1;
                        setVMobile.settingId = mobileId;
                        int sMobile = await valueModel.Save(setVMobile);
                        if (!sMobile.Equals(0))
                            AppSettings.Mobile = cb_areaMobile.Text + tb_mobile.Text;
                    }
                    //save phone
                    //if (!tb_phone.Text.Equals(""))
                    //{
                    setVPhone.value = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                    setVPhone.isSystem = 1;
                    setVPhone.isDefault = 1;
                    setVPhone.settingId = phoneId;
                    int sPhone = await valueModel.Save(setVPhone);
                    if (!sPhone.Equals(0))
                        AppSettings.Phone = cb_areaPhone.Text + cb_areaPhoneLocal.Text + tb_phone.Text;
                    //}
                    //save fax

                    setVFax.value = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                    setVFax.isSystem = 1;
                    setVFax.isDefault = 1;
                    setVFax.settingId = faxId;
                    int sFax = await valueModel.Save(setVFax);
                    if (!sFax.Equals(0))
                        AppSettings.Fax = cb_areaFax.Text + cb_areaFaxLocal.Text + tb_fax.Text;


                    //  save logo
                    // image
                    //  string sLogo = "";
                    int sLogo = 0;
                    if (isImgPressed)
                    {
                        isImgPressed = false;

                        setVLogo.value = sLogo.ToString();
                        setVLogo.isSystem = 1;
                        setVLogo.isDefault = 1;
                        setVLogo.settingId = logoId;
                        sLogo = await valueModel.Save(setVLogo);
                        if (!sLogo.Equals(0))
                        {
                            AppSettings.logoImage = setVLogo.value;
                            string b = await setVLogo.uploadImage(imgFileName, Md5Encription.MD5Hash("Inc-m" + sLogo), sLogo);
                            setVLogo.value = b;
                            AppSettings.logoImage = b;
                            sLogo = await valueModel.Save(setVLogo);
                            await valueModel.getImg(setVLogo.value);
                        }
                    }

                    #endregion
                    if (!isFirstTime)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        await Task.Delay(1500);

                    }
                    MainWindow.mainWindow.loading_getDefaultSystemInfo();
                    this.Close();
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private async Task getImg()
        {

            try
            {
                if (string.IsNullOrEmpty(setVLogo.value))
                {
                    HelpClass.clearImg(img_customer);
                }
                else
                {
                    byte[] imageBuffer = await setVLogo.downloadImage(setVLogo.value); // read this as BLOB from your DB

                    var bitmapImage = new BitmapImage();
                    if (imageBuffer != null)
                    {
                        using (var memoryStream = new MemoryStream(imageBuffer))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();
                        }

                        img_customer.Background = new ImageBrush(bitmapImage);
                        // configure trmporary path
                        // string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                        string dir = Directory.GetCurrentDirectory();
                        string tmpPath = System.IO.Path.Combine(dir, Global.TMPSettingFolder);
                        tmpPath = System.IO.Path.Combine(tmpPath, setVLogo.value);
                        openFileDialog.FileName = tmpPath;
                    }
                    else
                        HelpClass.clearImg(img_customer);
                }
            }
            catch { }

        }
        #region Phone

        private async void Cb_areaPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
              //  if (cb_areaPhone.SelectedValue != null && !firstchangephone)
                    if (cb_areaPhone.SelectedValue != null )
                {
                    if (cb_areaPhone.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaPhone.SelectedValue.ToString());
                        //await FillCombo.fillCountriesLocal(cb_areaPhoneLocal, (int)countryid );
                        await FillCombo.fillCountriesLocal(cb_areaPhoneLocal, (int)countryid, brd_areaPhoneLocal);

                    }
                }
                firstchangephone = false;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Cb_areaFax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
             //   if (cb_areaFax.SelectedValue != null && !firstchangefax)
                    if (cb_areaFax.SelectedValue != null  )
                {
                    if (cb_areaFax.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaFax.SelectedValue.ToString());
                        //await FillCombo.fillCountriesLocal(cb_areaFaxLocal, (int)countryid );
                        await FillCombo.fillCountriesLocal(cb_areaFaxLocal, (int)countryid, brd_areaFaxLocal);

                    }

                }
                firstchangefax = false;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 

        void Clear()
        {
            this.DataContext = new User();


            #region mobile-Phone-fax-country
            //cb_areaMobile.SelectedValue = MainWindow.Region.countryId;
            //cb_areaPhone.SelectedValue = MainWindow.Region.countryId;
            //cb_areaFax.SelectedValue = MainWindow.Region.countryId;

            cb_areaMobile.SelectedIndex = -1;
            cb_areaPhone.SelectedIndex = -1;
            cb_areaFax.SelectedIndex = -1;
            cb_areaPhoneLocal.SelectedIndex = -1;
            cb_areaFaxLocal.SelectedIndex = -1;
            tb_mobile.Clear();
            tb_phone.Clear();
            tb_fax.Clear();
            tb_email.Clear();
            #endregion
            #region image
            HelpClass.clearImg(img_customer);
            #endregion

            // last 
            HelpClass.clearValidate(requiredControlList, this);
            p_error_email.Visibility = Visibility.Collapsed;
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only  digits
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
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
                //p_error_password.Visibility = Visibility.Collapsed;
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

    }
}
