using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using laundryApp.ApiClasses;
using laundryApp.Classes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_logIn.xaml
    /// </summary>
    public partial class wd_logIn : Window
    {
        BrushConverter brushConverter = new BrushConverter();
         bool logInProcessing = false;
        UsersLogs userLogsModel = new UsersLogs();
        User userModel = new User();
        User user = new User();
        public wd_logIn()
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                bdrLogIn.RenderTransform = Animations.borderAnimation(-100, bdrLogIn, true);
                //AppSettings.lang = "en";
                #region properties
                if (Properties.Settings.Default.userName != string.Empty)
                {
                    txtUserName.Text = Properties.Settings.Default.userName;
                    txtPassword.Password = Properties.Settings.Default.password;
                    AppSettings.lang = Properties.Settings.Default.Lang;
                    //menuIsOpen = Properties.Settings.Default.menuIsOpen;
                    cbxRemmemberMe.IsChecked = true;

                }
                else
                {
                    txtUserName.Clear();
                    txtPassword.Clear();
                    AppSettings.lang = "en";
                    //menuIsOpen = "close";
                    cbxRemmemberMe.IsChecked = false;
                }
                #endregion

                #region translate
                //AppSettings.lang = "en";
                if (AppSettings.lang.Equals("en"))
                {
                    AppSettings.resourcemanager = new ResourceManager("laundryApp.en_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                    //bdr_imageAr.Visibility = Visibility.Hidden;
                    //bdr_image.Visibility = Visibility.Visible;
                    bdr_image.CornerRadius = new CornerRadius(0, 10, 10, 0);
                }
                else
                {
                    AppSettings.resourcemanager = new ResourceManager("laundryApp.ar_file", Assembly.GetExecutingAssembly());
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                    //bdr_imageAr.Visibility = Visibility.Visible;
                    //bdr_image.Visibility = Visibility.Hidden;
                    bdr_image.CornerRadius = new CornerRadius(10, 0, 0, 10);
                }
                translate();
                #endregion

                #region Arabic Number
                CultureInfo ci = CultureInfo.CreateSpecificCulture(Thread.CurrentThread.CurrentCulture.Name);
                ci.NumberFormat.DigitSubstitution = DigitShapes.Context;
                Thread.CurrentThread.CurrentCulture = ci;
                #endregion

                HelpClass.EndAwait(grid_main);

                if (txtUserName.Text.Equals(""))
                    Keyboard.Focus(txtUserName);
                else if (txtPassword.Password.Equals(""))
                    Keyboard.Focus(txtPassword);

              
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translate()
        {
            cbxRemmemberMe.Content = AppSettings.resourcemanager.GetString("trRememberMe");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txtUserName, AppSettings.resourcemanager.GetString("trUserName"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txtPassword, AppSettings.resourcemanager.GetString("trPassword"));
            txt_logIn.Text = AppSettings.resourcemanager.GetString("trLogIn");
            txt_close.Text = AppSettings.resourcemanager.GetString("trClose");
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    btnLogIn_Click(btnLogIn, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void validateEmpty(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender.GetType().Name == "TextBox")
                {
                    if (txtUserName.Text.Equals(""))
                    {
                        HelpClass.SetValidate(p_errorUserName, "trEmptyUserNameToolTip");
                    }
                }
                else if (sender.GetType().Name == "PasswordBox")
                {
                    if (txtPassword.Password.Equals(""))
                    {
                        HelpClass.SetValidate(p_errorPassword, "trEmptyPasswordToolTip");
                    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateTextChanged(object sender, TextChangedEventArgs e)
        {
            clearValidate(p_errorUserName);
        }
        private void validatePasswordChanged(object sender, RoutedEventArgs e)
        {
            clearValidate(p_errorPassword);
        }

        private void clearValidate(Path p)
        {
            try
            {
                HelpClass.clearValidate(p);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void P_showPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                txtShowPassword.Text = txtPassword.Password;
                txtShowPassword.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void P_showPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                txtShowPassword.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void btnLogIn_Click(object sender, RoutedEventArgs e)
        {//log in
            try
            {
                if (!logInProcessing)
                {
                    logInProcessing = true;

                    HelpClass.StartAwait(grid_main);

                    HelpClass.clearValidate(p_errorUserName);
                    HelpClass.clearValidate(p_errorPassword);

                    string password = Md5Encription.MD5Hash("Inc-m" + txtPassword.Password);
                    string userName = txtUserName.Text;

                    //int canLogin = await userModel.checkLoginAvalability(MainWindow.posID.Value, userName, password);
                    //if (canLogin == 1)
                    //{


                    MainWindow.userLogin = new User();
                    MainWindow.posLogin = new Pos();
                    MainWindow.branchLogin = new Branch();
                    MainWindow.userLog = null;
                    //MainWindow.userLog;
                    user = await userModel.Getloginuser(userName, password);

                    if (user.username == null)
                    {
                        //user does not exist
                        HelpClass.SetValidate(p_errorUserName, "trUserNotFound");
                    }
                    else
                    {
                        if (user.userId == 0)
                        {
                            //wrong password
                            HelpClass.SetValidate(p_errorPassword, "trWrongPassword");
                        }
                        else
                        {
                            //correct
                            //send user info to main window
                            MainWindow.userLogin.userId = user.userId;
                            MainWindow.userLogin = user;
                             MainWindow.posLogin = await MainWindow.posLogin.getById(1); 
                            MainWindow.branchLogin = await MainWindow.branchLogin.getBranchById(MainWindow.posLogin.branchId);

                            //make user online
                            user.isOnline = 1;

                            //checkother
                            //string str1 = await userLogsModel.checkOtherUser(MainWindow.userLogin.userId);


                            int s = await userModel.save(user);

                            //create lognin record
                            UsersLogs userLog = new UsersLogs();
                            userLog.posId = MainWindow.posLogin.posId;

                            userLog.userId = user.userId;
                            MainWindow.userLog = userLog;
                            int str = await userLogsModel.Save(userLog);

                            if (!str.Equals(0))
                                MainWindow.userLog.logId = str;

                            #region remember me
                            if (cbxRemmemberMe.IsChecked.Value)
                            {
                                Properties.Settings.Default.userName = txtUserName.Text;
                                Properties.Settings.Default.password = txtPassword.Password;
                                Properties.Settings.Default.Lang = AppSettings.lang;
                                //Properties.Settings.Default.menuIsOpen = menuIsOpen;
                            }
                            else
                            {
                                Properties.Settings.Default.userName = "";
                                Properties.Settings.Default.password = "";
                                Properties.Settings.Default.Lang = "";
                                //Properties.Settings.Default.menuIsOpen = "";
                            }
                            Properties.Settings.Default.Save();
                            #endregion

                            MainWindow.go_out = false;
                            MainWindow.go_out_didNotAnyProcess = false;
                            MainWindow._CachTransfersCount = 0;

                            MainWindow main = new MainWindow();
                            main.Show();
                            this.Close();
                        }
                    }

                    // }
                    //else if (canLogin == -1) //program is expired
                    //    tb_msg.Text = resourcemanager.GetString("trPackageIsExpired");
                    //else if (canLogin == -2) //device code is not correct 
                    //    tb_msg.Text = resourcemanager.GetString("trPreventLogIn");
                    //else if (canLogin == -3) //serial is not active
                    //    tb_msg.Text = resourcemanager.GetString("trPackageIsNotActive");
                    //else if (canLogin == -4) //serial is not active
                    //    tb_msg.Text = resourcemanager.GetString("trServerNotCompatible");
                    //else if (canLogin == -5) //login date is before last login date
                    //    tb_msg.Text = resourcemanager.GetString("trDateNotCompatible");

                    HelpClass.EndAwait(grid_main);
                    logInProcessing = false;
                }
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                logInProcessing = false;
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        /*
        List<UserSetValues> usValues = new List<UserSetValues>();
        UserSetValues usLanguage = new UserSetValues();
        private async Task<string> getUserLanguage(int userId)
        {
            SettingCls setModel = new SettingCls();
            SettingCls set = new SettingCls();
            SetValues valueModel = new SetValues();
            List<SetValues> languages = new List<SetValues>();
            UserSetValues usValueModel = new UserSetValues();
            var lanSettings = await setModel.GetAll();
            set = lanSettings.Where(l => l.name == "language").FirstOrDefault<SettingCls>();
            var lanValues = await valueModel.GetAll();

            if (lanValues.Count > 0)
            {
                languages = lanValues.Where(vl => vl.settingId == set.settingId).ToList<SetValues>();

                usValues = await usValueModel.GetAll();
                if (usValues.Count > 0)
                {
                    var curUserValues = usValues.Where(c => c.userId == userId);

                    if (curUserValues.Count() > 0)
                    {
                        foreach (var l in curUserValues)
                            if (languages.Any(c => c.valId == l.valId))
                            {
                                usLanguage = l;
                            }

                        var lan = await valueModel.GetByID(usLanguage.valId.Value);
                        return lan.value;
                    }
                    else return "en";
                }
                else return "en";
            }
            else return "en";
        }
        */
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {//close
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #region get language from database
        /*
        List<UserSetValues> usValues = new List<UserSetValues>();
        private async Task<string> getUserLanguage(int userId)
        {
            SettingCls setModel = new SettingCls();
            SettingCls set = new SettingCls();
            SetValues valueModel = new SetValues();
            List<SetValues> languages = new List<SetValues>();
            UserSetValues usValueModel = new UserSetValues();
            var lanSettings = await setModel.GetAll();
            set = lanSettings.Where(l => l.name == "language").FirstOrDefault<SettingCls>();
            var lanValues = await valueModel.GetAll();

            if (lanValues.Count > 0)
            {
                languages = lanValues.Where(vl => vl.settingId == set.settingId).ToList<SetValues>();

                usValues = await usValueModel.GetAll();
                if (usValues.Count > 0)
                {
                    var curUserValues = usValues.Where(c => c.userId == userId);

                    if (curUserValues.Count() > 0)
                    {
                        foreach (var l in curUserValues)
                            if (languages.Any(c => c.valId == l.valId))
                            {
                                usLanguage = l;
                            }

                        var lan = await valueModel.GetByID(usLanguage.valId.Value);
                        return lan.value;
                    }
                    else return "en";
                }
                else return "en";
            }
            else return "en";
        }
        */
        #endregion
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBox.Show("Memory used before collection: " +
            //                 GC.GetTotalMemory(false).ToString());
            // Collect all generations of memory.
            GC.Collect();
            //MessageBox.Show("Memory used after full collection: " +
            //                 GC.GetTotalMemory(true).ToString());

        }
    }
}
