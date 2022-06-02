using laundryApp.Classes;
using netoaster;
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
using System.Text.RegularExpressions;
using System.Net.Mail;
using Tulpep.NotificationWindow;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Resources;
using System.IO;
using System.Drawing;
using laundryApp.View.windows;

namespace laundryApp.View.setup
{
    /// <summary>
    /// Interaction logic for uc_companyInfo.xaml
    /// </summary>
    public partial class uc_companyInfo : UserControl
    {
        public uc_companyInfo()
        {
            InitializeComponent();
        }
        private static uc_companyInfo _instance;
        public static uc_companyInfo Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_companyInfo();
                return _instance;
            }
        }

        public string companyName { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
            

        static CountryCode countryModel = new CountryCode();
        CountryCode countrycodes = new CountryCode();
        IEnumerable<CountryCode> countrynum;
        IEnumerable<City> citynum;
        IEnumerable<City> citynumofcountry;
        City cityCodes = new City();
        int? countryid;
        Boolean firstchange = false;
        Boolean firstchangefax = false;

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AppSettings.resourcemanager = new ResourceManager("laundryApp.en_file", Assembly.GetExecutingAssembly());
            grid_main.FlowDirection = FlowDirection.LeftToRight;

            await fillCountries();
            await fillLocalArea();
           
            if (citynumofcountry.Count() > 0)
            {
                cb_areaFaxLocal.Visibility = Visibility.Visible;
                brd_areaFaxLocal.Visibility = Visibility.Visible;
            }
            else
            {
                cb_areaFaxLocal.Visibility = Visibility.Collapsed;
                brd_areaFaxLocal.Visibility = Visibility.Collapsed;
            }
        }

        private async Task fillLocalArea()
        {
            citynum = await cityCodes.Get();
            citynumofcountry = citynum.Where(b => b.countryId == countryid).OrderBy(b => b.cityCode).ToList();
            cb_areaFaxLocal.ItemsSource = citynumofcountry;
            cb_areaFaxLocal.SelectedValuePath = "cityId";
            cb_areaFaxLocal.DisplayMemberPath = "cityCode";
        }
        //area code methods
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

            cb_areaMobile.SelectedValue = wd_setupFirstPos.countryId;
            cb_areaPhone.SelectedValue = wd_setupFirstPos.countryId;
            cb_areaFax.SelectedValue = wd_setupFirstPos.countryId;

        }
        private void Tb_validateEmptyTextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                validateEmpty(name, sender);

                TextBox textBox = sender as TextBox;
                if (textBox.Name.Equals("tb_name"))
                {
                    companyName = tb_name.Text;
                }
                else if (textBox.Name.Equals("tb_address"))
                {
                    address = tb_address.Text;
                }
                else if (textBox.Name.Equals("tb_email"))
                {
                    if (validateEmail(tb_email, p_errorEmail, tt_errorEmail))
                        email = tb_email.Text;
                    else
                        email = "";
                }
                else if (textBox.Name.Equals("tb_mobile"))
                {
                    mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                }
                else if (textBox.Name.Equals("tb_phone"))
                {
                    phone = cb_areaPhone.Text + "-"+cb_areaPhoneLocal.Text + "-"+ tb_phone.Text;

                }
                else if (textBox.Name.Equals("tb_fax"))
                {
                    if(cb_areaFaxLocal.Visibility == Visibility.Visible)
                        fax = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                    else 
                        fax = cb_areaFax.Text +  "-" + tb_fax.Text;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateEmpty(string name, object sender)
        {
            if (name == "TextBox")
            {
                if ((sender as TextBox).Name == "tb_name")
                    HelpClass.validateEmpty(tb_name.Text, p_errorName);
              
                else if ((sender as TextBox).Name == "tb_email")
                {
                 
                    validateEmail(tb_email, p_errorEmail, tt_errorEmail);
                }
                else if ((sender as TextBox).Name == "tb_mobile")
                    HelpClass.validateEmpty(tb_mobile.Text, p_errorMobile);               
              
            }
        }

       

        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
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
        private void Tb_email_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isFirstTime)
                    validateEmail(tb_email, p_errorEmail, tt_errorEmail);
                else
                    HelpClass.IsValidEmail(this);
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        private bool validateEmail(TextBox tb, System.Windows.Shapes.Path p_error, ToolTip tt_error)
        {
            bool isValid = true;
            if (!tb.Text.Equals(""))
            {
                if (!ValidatorExtensions.IsValid(tb.Text))
                {
                    p_error.Visibility = Visibility.Visible;
                    tt_error.Content = ("Email address is not valid");
                    //tb.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#15FF0000"));   
                    isValid = false;
                }
                else
                {
                    p_error.Visibility = Visibility.Collapsed;
                    //tb.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f8f8f8")); 
                    isValid = true;
                }
            }
            return isValid;
        }
        private void Tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //doesn't allow spaces into textbox
            e.Handled = e.Key == Key.Space;
        }
        private void Cb_areaPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);
                if (firstchange == true)
                {
                    if (cb_areaPhone.SelectedValue != null)
                    {
                        if (cb_areaPhone.SelectedIndex >= 0)
                            countryid = int.Parse(cb_areaPhone.SelectedValue.ToString());

                        if (citynum != null)
                        {
                            citynumofcountry = citynum.Where(b => b.countryId == countryid).OrderBy(b => b.cityCode).ToList();
                            cb_areaPhoneLocal.ItemsSource = citynumofcountry;
                            cb_areaPhoneLocal.SelectedValuePath = "cityId";
                            cb_areaPhoneLocal.DisplayMemberPath = "cityCode";
                            if (citynumofcountry.Count() > 0)
                            {

                                cb_areaPhoneLocal.Visibility = Visibility.Visible;
                                brd_areaPhoneLocal.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                cb_areaPhoneLocal.Visibility = Visibility.Collapsed;
                                brd_areaPhoneLocal.Visibility = Visibility.Collapsed;
                            }
                        }

                    }
                }
                else
                {
                    firstchange = true;
                }
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_areaFax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (firstchangefax == true)
                {
                    if (cb_areaFax.SelectedValue != null)
                    {
                        if (cb_areaFax.SelectedIndex >= 0)
                            countryid = int.Parse(cb_areaFax.SelectedValue.ToString());
                        if (citynum != null)
                        {
                            citynumofcountry = citynum.Where(b => b.countryId == countryid).OrderBy(b => b.cityCode).ToList();

                            cb_areaFaxLocal.ItemsSource = citynumofcountry;
                            cb_areaFaxLocal.SelectedValuePath = "cityId";
                            cb_areaFaxLocal.DisplayMemberPath = "cityCode";
                            if (citynumofcountry.Count() > 0)
                            {
                                cb_areaFaxLocal.Visibility = Visibility.Visible;
                                brd_areaFaxLocal.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                cb_areaFaxLocal.Visibility = Visibility.Collapsed;
                                brd_areaFaxLocal.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
                else
                {
                    firstchangefax = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        string imgFileName = "pic/no-image-icon-125x125.png";
        bool isImgPressed = false;
        ImageBrush brush = new ImageBrush();
        BrushConverter bc = new BrushConverter();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public bool isFirstTime = false;
        private void Img_customer_Click(object sender, RoutedEventArgs e)
        {
            //select image
            try
            {
                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    brush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    img_customer.Background = brush;
                    imgFileName = openFileDialog.FileName;

                    wd_setupFirstPos.brush = brush;
                    wd_setupFirstPos.imgFileName = imgFileName;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
