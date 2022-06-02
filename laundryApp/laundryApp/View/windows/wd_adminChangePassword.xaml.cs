using netoaster;
using laundryApp.Classes;
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
using System.Windows.Shapes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_adminChangePassword.xaml
    /// </summary>
    public partial class wd_adminChangePassword : Window
    {
        public wd_adminChangePassword()
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
        BrushConverter bc = new BrushConverter();
        public int userID = 0;
        User userModel = new User();
        User user = new User();

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                
                    HelpClass.StartAwait(grid_changePassword);

                #region translate

                if (AppSettings.lang.Equals("en"))
            {
                grid_changePassword.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                grid_changePassword.FlowDirection = FlowDirection.RightToLeft;
            }

            translate();
                #endregion

                await fillUsers();

                
                    HelpClass.EndAwait(grid_changePassword);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_changePassword);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async Task fillUsers()
        {
            List<User> users =  await userModel.GetUsersActive();
            if (!HelpClass.isSupportPermision())
                users = users.Where(x => x.isAdmin == false).ToList();
            cb_user.ItemsSource = users;
            cb_user.DisplayMemberPath = "username";
            cb_user.SelectedValuePath = "userId";
        }

        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trChangePassword");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_user, AppSettings.resourcemanager.GetString("trUserHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pb_password, AppSettings.resourcemanager.GetString("trPasswordHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_password, AppSettings.resourcemanager.GetString("trPasswordHint"));
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private bool chkPasswordLength(string password)
        {
            bool b = false;
            if (password.Length < 6)
                b = true;
            return b;
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                
                    HelpClass.StartAwait(grid_changePassword);

                bool wrongPasswordLength = false;
                //chk empty user
                //HelpClass.validateEmptyComboBox(cb_user , p_error_user , tt_errorUser , "trEmptyUser");
                if (string.IsNullOrWhiteSpace(cb_user.Text))
                    HelpClass.SetValidate(p_error_user, "trEmptyUser");
                else
                    HelpClass.clearValidate(p_error_user);
                //chk empty password
                if (pb_password.Password.Equals(""))
                    //HelpClass.showPasswordValidate(pb_password, p_error_password, tt_errorPassword, "trEmptyPasswordToolTip");
                    HelpClass.SetValidate( p_error_password, "trEmptyPasswordToolTip");
                else
                {
                    //chk password length
                    wrongPasswordLength = chkPasswordLength(pb_password.Password);
                    if (wrongPasswordLength)
                        //HelpClass.showPasswordValidate(pb_password, p_error_password, tt_errorPassword, "trErrorPasswordLengthToolTip");
                        HelpClass.SetValidate(p_error_password, "trErrorPasswordLengthToolTip");
                    else
                        HelpClass.clearValidate(p_error_password);
                }

                if ((!cb_user.Text.Equals("")) &&(!pb_password.Password.Equals("")) && (!wrongPasswordLength))
                {
                    if (user != null)
                    {
                        string password = Md5Encription.MD5Hash("Inc-m" + pb_password.Password);

                        user.password = password ;

                        int s = await userModel.save(user);

                        if (s>0)
                        {
                            if (Properties.Settings.Default.password != string.Empty && user.userId == MainWindow.userLogin.userId)
                            {
                                Properties.Settings.Default.password = pb_password.Password;
                                Properties.Settings.Default.Save();
                            }
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopPasswordChanged"), animation: ToasterAnimation.FadeIn);
                            await Task.Delay(2000);
                            this.Close();

                            userID =s;
                       
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                }
                
                    HelpClass.EndAwait(grid_changePassword);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_changePassword);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
      
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                cb_user.SelectedIndex = -1;
                pb_password.Clear();
                tb_password.Clear();
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
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
                tb_password.Text = pb_password.Password;
                tb_password.Visibility = Visibility.Visible;
                pb_password.Visibility = Visibility.Collapsed;
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
                tb_password.Visibility = Visibility.Collapsed;
                pb_password.Visibility = Visibility.Visible;
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
                string name = sender.GetType().Name;
                validateEmpty(name, sender);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Tb_validateEmptyTextChange(object sender, RoutedEventArgs e)
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

        private void validateEmpty(string name, object sender)
        {
            if (name == "PasswordBox")
            {
                if ((sender as PasswordBox).Name == "pb_password")
                    if (((PasswordBox)sender).Password.Equals(""))
                        //HelpClass.showPasswordValidate((PasswordBox)sender, p_error_password, tt_errorPassword, "trEmptyPasswordToolTip");
                        HelpClass.SetValidate(p_error_password, "trEmptyPasswordToolTip");
                    else
                        //HelpClass.clearPasswordValidate((PasswordBox)sender , p_error_password);
                        HelpClass.clearValidate( p_error_password);
            }
            else if (name == "ComboBox")
            {
                if ((sender as ComboBox).Name == "cb_user")
                    if (string.IsNullOrWhiteSpace( ((ComboBox)sender).Text ))
                        HelpClass.SetValidate( p_error_user,  "trEmptyUser");
                      else
                        HelpClass.clearValidate(p_error_user);
            }
        }

        private async void Cb_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//select user
            try
            {
                user = await userModel.getUserById(Convert.ToInt32(cb_user.SelectedValue));
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }


        }
    }
}
