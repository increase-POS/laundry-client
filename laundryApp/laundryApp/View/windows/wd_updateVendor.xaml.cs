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
using System.Threading;
using Microsoft.Win32;
using System.Windows.Resources;
using System.IO;
using Microsoft.Reporting.WinForms;

using System.Data;
using laundryApp.View.windows;
using laundryApp.View.sectionData;


namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_updateVendor.xaml
    /// </summary>
    public partial class wd_updateVendor : Window
    {
        public wd_updateVendor()
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

        //string basicsPermission = "customers_basics";
        public Agent agent = new Agent();
        //IEnumerable<Agent> agentsQuery;
        //IEnumerable<Agent> agents;
        //byte tgl_agentState;
        //string searchText = "";
        public static List<string> requiredControlList;

        public string type = "";
        public bool isOk = false;
       

        private void translate()
        {
            if (type == "v")
            {
                txt_vendor.Text = AppSettings.resourcemanager.GetString("trVendor");
                path_title.Data = App.Current.Resources["vendor"] as Geometry;

            }
            else if (type == "c")
            {
                txt_vendor.Text = AppSettings.resourcemanager.GetString("trCustomer");
                path_title.Data = App.Current.Resources["customer"] as Geometry;
            }
            //btn_save
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_code, AppSettings.resourcemanager.GetString("trCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_address, AppSettings.resourcemanager.GetString("trAdressHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_company, AppSettings.resourcemanager.GetString("trCompanyHint"));
            txt_contactInformation.Text = AppSettings.resourcemanager.GetString("trContactInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_mobile, AppSettings.resourcemanager.GetString("trMobileHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_phone, AppSettings.resourcemanager.GetString("trPhoneHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_email, AppSettings.resourcemanager.GetString("trEmailHint"));
            txt_contentInformatin.Text = AppSettings.resourcemanager.GetString("trMoreInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_upperLimit, AppSettings.resourcemanager.GetString("trUpperLimitHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_fax, AppSettings.resourcemanager.GetString("trFaxHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_payType, AppSettings.resourcemanager.GetString("trDefaultPayType"));

          
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            txt_isCredit.Text = AppSettings.resourcemanager.GetString("trCredit");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_residentSecId, AppSettings.resourcemanager.GetString("trResidentialSectors") + "...");
            txt_canReserve.Text = AppSettings.resourcemanager.GetString("trReserve");
            txt_addressInformatin.Text = AppSettings.resourcemanager.GetString("trAddressInformation");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                requiredControlList = new List<string> { "name", "mobile" };
                if (type == "v")
                {
                    dp_addressInformation.Visibility = Visibility.Collapsed;
                    brd_residentSec.Visibility = Visibility.Collapsed;
                    brd_GPSAddress.Visibility = Visibility.Collapsed;
                    dp_credit.Visibility = Visibility.Collapsed;
                    dp_reserve.Visibility = Visibility.Collapsed;
                }
                if (type == "c")
                {
                    dp_addressInformation.Visibility = Visibility.Visible;
                    brd_residentSec.Visibility = Visibility.Visible;
                    brd_GPSAddress.Visibility = Visibility.Visible;
                    dp_credit.Visibility = Visibility.Visible;
                    dp_reserve.Visibility = Visibility.Visible;
                }

                
                    HelpClass.StartAwait(grid_main);

                #region translate
                if (AppSettings.lang.Equals("en"))
                {grid_main.FlowDirection = FlowDirection.LeftToRight; }
                else
                {   grid_main.FlowDirection = FlowDirection.RightToLeft; }

                translate();

                await FillCombo.FillComboResidentialSectors(cb_residentSecId);
                await FillCombo.fillCountries(cb_areaMobile);
                await FillCombo.fillCountries(cb_areaPhone);
                await FillCombo.fillCountries(cb_areaFax);
                FillCombo.FillDefaultPayType_cashBalanceCardMultiple(cb_payType);
                Keyboard.Focus(tb_code);
                Clear();
                #endregion
                 if (agent.agentId != 0 && type == "v")
                    agent = FillCombo.vendorsList.Where(x => x.agentId == agent.agentId).FirstOrDefault();
                else if (agent.agentId != 0 && type == "c")
                    agent = FillCombo.customersList.Where(x => x.agentId == agent.agentId).FirstOrDefault();
                if (agent != null)
                {
                    this.DataContext = agent;

                    getImg();
                    HelpClass.getMobile(agent.mobile, cb_areaMobile, tb_mobile);
                    HelpClass.getPhone(agent.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
                    HelpClass.getPhone(agent.fax, cb_areaFax, cb_areaFaxLocal, tb_fax);

                }
                
                    HelpClass.EndAwait(grid_main);
                Keyboard.Focus(tb_name);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {

                HelpClass.StartAwait(grid_main);
                //agent = new Agent();
                if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                {

                    //deserve
                    decimal maxDeserveValue = 0;
                    if (!tb_upperLimit.Text.Equals(""))
                        maxDeserveValue = decimal.Parse(tb_upperLimit.Text);

                    //payType
                    string payType = "";
                    if (cb_payType.SelectedIndex != -1)
                        payType = cb_payType.SelectedValue.ToString();

                    if (agent.agentId == 0)
                    {
                        agent.code = await agent.generateCodeNumber(type);
                        agent.type = type;
                        agent.accType = "";
                        agent.balance = 0;
                        agent.balanceType = 0;
                        agent.isActive = 1;
                        agent.createUserId = MainWindow.userLogin.userId;
                    }
                    agent.name = tb_name.Text;
                    agent.company = tb_company.Text;
                    if (cb_residentSecId.SelectedIndex != -1)
                        agent.residentSecId = (int)cb_residentSecId.SelectedValue;
                    agent.address = tb_address.Text;
                    agent.GPSAddress = tb_GPSAddress.Text;
                    agent.email = tb_email.Text;
                    agent.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text;
                    if (!tb_phone.Text.Equals(""))
                        agent.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                    if (!tb_fax.Text.Equals(""))
                        agent.fax = cb_areaFax.Text + "-" + cb_areaFaxLocal.Text + "-" + tb_fax.Text;
                    agent.payType = payType;
                    agent.isLimited = (bool)tgl_hasCredit.IsChecked;
                    agent.canReserve = (bool)tgl_canReserve.IsChecked;
                    agent.disallowReason = tb_disallowReason.Text;
                    agent.updateUserId = MainWindow.userLogin.userId;
                    agent.notes = tb_notes.Text;
                    agent.maxDeserve = maxDeserveValue;

                    int s = await agent.save(agent);
                    if (s <= 0)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    else
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                        
                        if (openFileDialog.FileName != "")
                        {
                            int agentId = s;
                            string b = await agent.uploadImage(imgFileName,
                                Md5Encription.MD5Hash("Inc-m" + agentId.ToString()), agentId);
                            agent.image = b;
                        }

                        isOk = true;
                        this.Close();

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
        #endregion
        #region events
         private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
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
            this.DataContext = new Agent();

            #region mobile-Phone-fax-email
            brd_areaPhoneLocal.Visibility =
                brd_areaFaxLocal.Visibility = Visibility.Collapsed;
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
            HelpClass.clearImg(btn_image);
            openFileDialog.FileName = "";
            #endregion


            // last 
            HelpClass.clearValidate(requiredControlList, this);
            p_error_email.Visibility = Visibility.Collapsed;
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
        #region Phone
        int? countryid;
        private async void Cb_areaPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaPhone.SelectedValue != null)
                {
                    if (cb_areaPhone.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaPhone.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaPhoneLocal, (int)countryid, brd_areaPhoneLocal);
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
        private async void Cb_areaFax_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaFax.SelectedValue != null)
                {
                    if (cb_areaFax.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaFax.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaFaxLocal, (int)countryid, brd_areaFaxLocal);
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

        #endregion
        #region Image
        string imgFileName = "pic/no-image-icon-125x125.png";
        bool isImgPressed = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {
            //select image
            try
            {
                HelpClass.StartAwait(grid_main);
                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    HelpClass.imageBrush = new ImageBrush();
                    HelpClass.imageBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    btn_image.Background = HelpClass.imageBrush;
                    imgFileName = openFileDialog.FileName;
                }
                else
                { }
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
                HelpClass.StartAwait(grid_image, "forImage");
                if (string.IsNullOrEmpty(agent.image))
                {
                    HelpClass.clearImg(btn_image);
                }
                else
                {
                    byte[] imageBuffer = await agent.downloadImage(agent.image); // read this as BLOB from your DB

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

                        btn_image.Background = new ImageBrush(bitmapImage);
                        // configure trmporary path
                        string dir = Directory.GetCurrentDirectory();
                        string tmpPath = System.IO.Path.Combine(dir, Global.TMPAgentsFolder, agent.image);
                        openFileDialog.FileName = tmpPath;
                    }
                    else
                        HelpClass.clearImg(btn_image);
                }
                HelpClass.EndAwait(grid_image, "forImage");
            }
            catch
            {
                HelpClass.EndAwait(grid_image, "forImage");
            }
        }
        #endregion

        private void Tgl_isOpenUpperLimit_Checked(object sender, RoutedEventArgs e)
        {
            tb_upperLimit.IsEnabled = true;
        }
        private void Tgl_isOpenUpperLimit_Unchecked(object sender, RoutedEventArgs e)
        {
            tb_upperLimit.IsEnabled = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_add_Click(btn_save, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
