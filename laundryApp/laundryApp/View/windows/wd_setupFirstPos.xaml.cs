using netoaster;
using laundryApp.Classes;
using laundryApp.View.setup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_setupFirstPos.xaml
    /// </summary>
    public partial class wd_setupFirstPos : Window
    {
        public wd_setupFirstPos()
        {
            InitializeComponent();
        }
        public static ResourceManager resourcemanager;
        SetValues setVLogo = new SetValues();
        public bool isValid = false;
        int _pageIndex;
        uc_serverConfig serverConfigInstance;
        uc_FirstPosGeneralSettings posGeneralInstance;
        uc_companyInfo comInfoInstance;
        int pageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value;
                OnPropertyChanged();
            }
        }
        static public int countryId;
        static public string imgFileName = "pic/no-image-icon-125x125.png";
        static public ImageBrush brush = new ImageBrush();
        protected void OnPropertyChanged()
        {
            //txt_pageIndex.Text =(_pageIndex+1).ToString();
            if (_pageIndex == 0)
            {
                path_step1.Visibility = Visibility.Visible;
                path_step2.Visibility = Visibility.Hidden;
                path_step3.Visibility = Visibility.Hidden;
            }
            else if (_pageIndex == 1)
            {
                path_step1.Visibility = Visibility.Hidden;
                path_step2.Visibility = Visibility.Visible;
                path_step3.Visibility = Visibility.Hidden;
            }
            else if (_pageIndex == 2)
            {
                path_step1.Visibility = Visibility.Hidden;
                path_step2.Visibility = Visibility.Hidden;
                path_step3.Visibility = Visibility.Visible;
            }

            if (_pageIndex == 0)
                btn_back.IsEnabled = false;
            else
                btn_back.IsEnabled = true;

            if (_pageIndex == 2)
                btn_next.Content = "Done";
            else
                btn_next.Content = "Next";
        }
        static public List<keyValueString> list = new List<keyValueString>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //load
            try
            {
                pageIndex = 0;
                AppSettings.resourcemanager = new ResourceManager("laundryApp.en_file", Assembly.GetExecutingAssembly());
                grid_main.FlowDirection = FlowDirection.LeftToRight;

                #region InitializeList
                list.Add(new keyValueString { key = "serverUri", value = "" });
                list.Add(new keyValueString { key = "activationkey", value = "" });

                list.Add(new keyValueString { key = "countryId", value = "" });
                list.Add(new keyValueString { key = "userName", value = "" });
                list.Add(new keyValueString { key = "userPassword", value = "" });
                list.Add(new keyValueString { key = "branchName", value = "" });
                list.Add(new keyValueString { key = "branchCode", value = "" });
                list.Add(new keyValueString { key = "branchMobile", value = "" });
                list.Add(new keyValueString { key = "posName", value = "" });

                list.Add(new keyValueString { key = "companyName", value = "" });
                list.Add(new keyValueString { key = "address", value = "" });
                list.Add(new keyValueString { key = "email", value = "" });
                list.Add(new keyValueString { key = "mobile", value = "" });
                list.Add(new keyValueString { key = "phone", value = "" });
                list.Add(new keyValueString { key = "fax", value = "" });


                #endregion

                CallPage(0, btn_next.Tag.ToString());

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        void CallPage(int index, string type = "")
        {
            if (index == 0)
            {
                grid_main.Children.Clear();
                serverConfigInstance = uc_serverConfig.Instance;
                grid_main.Children.Add(uc_serverConfig.Instance);
            }
            else if (index == 1)
            {
                grid_main.Children.Clear();
                posGeneralInstance = uc_FirstPosGeneralSettings.Instance;
                grid_main.Children.Add(uc_FirstPosGeneralSettings.Instance);
            }
            else if (index == 2)
            {
                grid_main.Children.Clear();
                comInfoInstance = uc_companyInfo.Instance;
                grid_main.Children.Add(uc_companyInfo.Instance);
            }
        }
        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            pageIndex--;
            CallPage(pageIndex, (sender as Button).Tag.ToString());
        }
        private async void Btn_next_Click(object sender, RoutedEventArgs e)
        {
            isValid = true;
            // uc_serverConfig
            if (pageIndex == 0)
            {
                var supsublist = list.Take(2);
                foreach (var item in supsublist)
                {
                    if (item.key.Equals("serverUri"))
                    {
                        // if (string.IsNullOrWhiteSpace(uc_serverConfig.Instance.serverUri))
                        if (string.IsNullOrWhiteSpace(serverConfigInstance.serverUri))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                        {
                            item.value = serverConfigInstance.serverUri;
                            bool validUrl = setupConfiguration.validateUrl(item.value);

                            if (!validUrl)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorWrongUrl"), animation: ToasterAnimation.FadeIn);
                                isValid = false;
                                return;
                            }
                            else
                            {
                                Global.APIUri = serverConfigInstance.serverUri + "/api/";
                                int installationNum = await setupConfiguration.getInstallationNum();
                                if (installationNum == 2)
                                {
                                    wd_setupOtherPos logInOther = new wd_setupOtherPos();
                                    //uc_serverConfig.Instance
                                    grid_main.Children.Clear();
                                    logInOther.Show();
                                    this.Close();
                                    return;
                                }
                            }
                        }

                    }
                    else if (item.key.Equals("activationkey"))
                    {
                        if (string.IsNullOrWhiteSpace(serverConfigInstance.activationkey))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                        {
                            item.value = serverConfigInstance.activationkey;
                        }
                    }
                }
            }
            else if (pageIndex == 1)
            {
                var supsublist = list.Skip(2).Take(6);
                foreach (var item in supsublist)
                {
                    if (item.key.Equals("countryId"))
                    {
                        if (string.IsNullOrWhiteSpace(posGeneralInstance.countryId))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                        {
                            item.value = posGeneralInstance.countryId;
                            countryId = int.Parse(item.value);
                        }

                    }
                    else if (item.key.Equals("userName"))
                    {
                        if (string.IsNullOrWhiteSpace(posGeneralInstance.userName))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                            item.value = posGeneralInstance.userName;

                    }
                    else if (item.key.Equals("userPassword"))
                    {
                        if (string.IsNullOrWhiteSpace(posGeneralInstance.userPassword))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                        {
                            item.value = posGeneralInstance.userPassword;
                            isValid = chkPasswordLength(item.value);
                            if (!isValid)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorPasswordLengthToolTip"), animation: ToasterAnimation.FadeIn);
                                isValid = false;
                                return;
                                //break;
                            }
                        }

                    }
                    else if (item.key.Equals("branchName"))
                    {
                        if (string.IsNullOrWhiteSpace(posGeneralInstance.branchName))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                            item.value = posGeneralInstance.branchName;

                    }
                    else if (item.key.Equals("branchCode"))
                    {
                        if (string.IsNullOrWhiteSpace(posGeneralInstance.branchCode))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                            item.value = posGeneralInstance.branchCode;

                    }
                    else if (item.key.Equals("branchMobile"))
                    {
                        if (string.IsNullOrWhiteSpace(posGeneralInstance.branchMobile))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                            item.value = posGeneralInstance.branchMobile;

                    }
                    else if (item.key.Equals("posName"))
                    {
                        if (string.IsNullOrWhiteSpace(posGeneralInstance.posName))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                            item.value = posGeneralInstance.posName;

                    }

                }
            }
            else if (pageIndex == 2)
            {
                var supsublist = list.Skip(8).Take(6);
                foreach (var item in supsublist)
                {
                    if (item.key.Equals("companyName"))
                    {
                        if (string.IsNullOrWhiteSpace(comInfoInstance.companyName))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                        {
                            item.value = comInfoInstance.companyName;
                        }
                    }
                    else if (item.key.Equals("address"))
                    {
                        if (string.IsNullOrWhiteSpace(comInfoInstance.address))
                        {
                            item.value = "";
                        }
                        else
                        {
                            item.value = comInfoInstance.address;
                        }
                    }
                    else if (item.key.Equals("email"))
                    {
                        if (string.IsNullOrWhiteSpace(comInfoInstance.email))
                        {
                            item.value = "";
                        }
                        else
                        {
                            item.value = comInfoInstance.email;
                        }
                    }
                    else if (item.key.Equals("mobile"))
                    {
                        if (string.IsNullOrWhiteSpace(comInfoInstance.mobile))
                        {
                            item.value = "";
                            isValid = false;
                            break;
                        }
                        else
                        {
                            item.value = comInfoInstance.mobile;
                        }
                    }
                    else if (item.key.Equals("phone"))
                    {
                        if (string.IsNullOrWhiteSpace(comInfoInstance.phone))
                        {
                            item.value = "";
                        }
                        else
                        {
                            item.value = comInfoInstance.phone;
                        }
                    }
                    else if (item.key.Equals("fax"))
                    {
                        if (string.IsNullOrWhiteSpace(comInfoInstance.fax))
                        {
                            item.value = "";
                        }
                        else
                        {
                            item.value = comInfoInstance.fax;
                        }
                    }
                }

            }
          
            if (isValid)
            {
                if (pageIndex == 2)
                {
                    //server INFO
                    string url = serverConfigInstance.serverUri;
                    string activationkey = serverConfigInstance.activationkey;
                    // user INFO
                    string userName = posGeneralInstance.userName;
                    string password = Md5Encription.MD5Hash("Inc-m" + posGeneralInstance.userPassword);
                    // branch INFO
                    string branchName = posGeneralInstance.branchName;
                    string branchCode = posGeneralInstance.branchCode;
                    string branchMobile = posGeneralInstance.branchMobile;
                    // pos INFO
                    string posName = posGeneralInstance.posName;
                    string motherCode = setupConfiguration.GetMotherBoardID();
                    string hardCode = setupConfiguration.GetHDDSerialNo();
                    string deviceCode = motherCode + "-" + hardCode;
                    // company INFO
                    string imageFileName = string.IsNullOrWhiteSpace(imgFileName) ? "" : Md5Encription.MD5Hash("Inc-m" + imgFileName);
                    List<SetValues> company = new List<SetValues>();
                    company.Add(new SetValues { name = "com_name", value = comInfoInstance.companyName ,isDefault=1,isSystem=1});
                    company.Add(new SetValues { name = "com_address", value = string.IsNullOrWhiteSpace(comInfoInstance.address) ? "" : comInfoInstance.address, isDefault = 1, isSystem = 1 });
                    company.Add(new SetValues { name = "com_email", value = string.IsNullOrWhiteSpace(comInfoInstance.email) ? "" : comInfoInstance.email, isDefault = 1, isSystem = 1 });
                    company.Add(new SetValues { name = "com_mobile", value = comInfoInstance.mobile , isDefault = 1, isSystem = 1 });
                    company.Add(new SetValues { name = "com_phone", value = string.IsNullOrWhiteSpace(comInfoInstance.phone) ? "" :  comInfoInstance.phone , isDefault = 1, isSystem = 1 });
                    company.Add(new SetValues { name = "com_fax", value = string.IsNullOrWhiteSpace(comInfoInstance.fax)  ? "" :  comInfoInstance.fax , isDefault = 1, isSystem = 1 });
                    company.Add(new SetValues { name = "com_logo", value = imageFileName , isDefault = 1, isSystem = 1 });
                    Global.APIUri = url + "/api/";
                    int res = await setupConfiguration.setConfiguration(activationkey, deviceCode, countryId, userName, password, branchName, branchCode, branchMobile, posName, company);
                    if (res == -2 || res == -3) // invalid or resrved activation key
                    {
                        serverConfigInstance.activationkey = "";
                        pageIndex = 0;
                        CallPage(0);
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorWrongActivation"), animation: ToasterAnimation.FadeIn);
                        return;
                    }
                    else if (res > 0)
                    {
                        #region upload image
                        if(imageFileName != "")
                             await setVLogo.uploadImage(imgFileName, imageFileName,0);
                        #endregion
                        Properties.Settings.Default.APIUri = Global.APIUri;
                        Properties.Settings.Default.posId = res.ToString();
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();
                        System.Configuration.Configuration config =ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                        config.AppSettings.Settings.Add("posId",res.ToString());

                        config.Save(ConfigurationSaveMode.Modified);

                        // Force a reload of a changed section.
                        ConfigurationManager.RefreshSection("appSettings");
                        this.Close();
                        return;
                    }
                    else
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }

                }
                if (pageIndex < 2)
                {
                    pageIndex++;
                    CallPage(pageIndex, (sender as Button).Tag.ToString());
                }
            }
            else
                Toaster.ShowWarning(Window.GetWindow(this), message: "Should fill form first", animation: ToasterAnimation.FadeIn);

        }
        private void restartApplication()
        {
            System.Diagnostics.Process.Start("pos.exe");
            Application.Current.Shutdown();
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                //if (sender != null)
                //    HelpClass.StartAwait(grid_main);

                if (e.Key == Key.Return)
                {
                    Btn_next_Click(btn_next, null);
                }
                //if (sender != null)
                //    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                //if (sender != null)
                //    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex)
            {
                //HelpClass.ExceptionMessage(ex, this);
            }
        }

        private bool chkPasswordLength(string password)
        {
            bool isValid = true;

            if (password.Length < 6)
                isValid = false;

            if (!isValid)
            {
               // p_error_password.Visibility = Visibility.Visible;
                #region Tooltip
                ToolTip toolTip = new ToolTip();
                toolTip.Content = AppSettings.resourcemanager.GetString("trErrorPasswordLengthToolTip");
                toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
               // p_error_password.ToolTip = toolTip;
                #endregion
            }

            return isValid;
        }
    }
}
