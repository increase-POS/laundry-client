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
    /// Interaction logic for wd_changePassword.xaml
    /// </summary>
    public partial class wd_changePassword : Window
    {
        public wd_changePassword()
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
        User newUser = new User();

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                this.Close();

            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
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
                newUser = MainWindow.userLogin;
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
            txt_title.Text = AppSettings.resourcemanager.GetString("trChangePassword");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pb_oldPassword, AppSettings.resourcemanager.GetString("trOldPasswordHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_oldPassword, AppSettings.resourcemanager.GetString("trOldPasswordHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pb_newPassword, AppSettings.resourcemanager.GetString("trNewPasswordHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_newPassword, AppSettings.resourcemanager.GetString("trNewPasswordHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pb_confirmPassword, AppSettings.resourcemanager.GetString("trConfirmedPasswordHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_confirmPassword, AppSettings.resourcemanager.GetString("trConfirmedPasswordHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            //tt_oldPassword.Content = AppSettings.resourcemanager.GetString("trOldPassword");
            //tt_newPassword.Content = AppSettings.resourcemanager.GetString("trNewPassword");
            //tt_confirmPassword.Content = AppSettings.resourcemanager.GetString("trConfirmedPassword");
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
                 
                    HelpClass.StartAwait(grid_main);

                bool wrongOldPasswordLength = false, wrongNewPasswordLength = false, wrongConfirmPasswordLength = false;
                //chk empty old password
                if (pb_oldPassword.Password.Equals(""))
                    //HelpClass.SetValidate(pb_oldPassword, p_errorOldPassword, tt_errorOldPassword, "trEmptyPasswordToolTip");
                    HelpClass.SetValidate(p_error_oldPassword, "trEmptyPasswordToolTip");
                else
                {
                    //chk password length
                    wrongOldPasswordLength = chkPasswordLength(pb_oldPassword.Password);
                    if (wrongOldPasswordLength)
                        HelpClass.SetValidate(p_error_oldPassword, "trErrorPasswordLengthToolTip");
                    else
                        HelpClass.clearValidate(p_error_oldPassword);
                }
                //chk empty new password
                if (pb_newPassword.Password.Equals(""))
                    HelpClass.SetValidate( p_error_newPassword,  "trEmptyPasswordToolTip");
                else
                {
                    //chk password length
                    wrongNewPasswordLength = chkPasswordLength(pb_newPassword.Password);
                    if (wrongNewPasswordLength)
                        HelpClass.SetValidate( p_error_newPassword, "trErrorPasswordLengthToolTip");
                    else
                        HelpClass.clearValidate( p_error_newPassword);
                }
                //chk empty confirm password
                if (pb_confirmPassword.Password.Equals(""))
                    HelpClass.SetValidate( p_error_confirmPassword,  "trEmptyPasswordToolTip");
                else
                {
                    //chk password length
                    wrongConfirmPasswordLength = chkPasswordLength(pb_confirmPassword.Password);
                    if (wrongConfirmPasswordLength)
                        HelpClass.SetValidate( p_error_confirmPassword,  "trErrorPasswordLengthToolTip");
                    else
                        HelpClass.clearValidate( p_error_confirmPassword);
                }


                if ((!pb_oldPassword.Password.Equals("")) && (!wrongOldPasswordLength) &&
                   (!pb_newPassword.Password.Equals("")) && (!wrongNewPasswordLength) &&
                   (!pb_confirmPassword.Password.Equals("")) && (!wrongConfirmPasswordLength))
                {
                    //get password for logined user
                    //string loginPassword = MainWindow.userLogin.password;
                    string loginPassword = newUser.password;

                    string enteredPassword = Md5Encription.MD5Hash("Inc-m" + pb_oldPassword.Password);

                    if (!loginPassword.Equals(enteredPassword))
                    {
                        HelpClass.SetValidate( p_error_oldPassword,  "trWrongPassword");
                    }
                    else
                    {
                        HelpClass.clearValidate( p_error_oldPassword);
                        bool isNewEqualConfirmed = true;
                        if (pb_newPassword.Password.Equals(pb_confirmPassword.Password)) isNewEqualConfirmed = true;
                        else isNewEqualConfirmed = false;

                        if (!isNewEqualConfirmed)
                        {
                            HelpClass.SetValidate( p_error_newPassword,  "trErrorNewPasswordNotEqualConfirmed");
                            HelpClass.SetValidate(p_error_confirmPassword, "trErrorNewPasswordNotEqualConfirmed");
                        }
                        else
                        {
                            HelpClass.clearValidate( p_error_newPassword);
                            HelpClass.clearValidate( p_error_confirmPassword);
                            //change password
                            string newPassword = Md5Encription.MD5Hash("Inc-m" + pb_newPassword.Password);
                            newUser.password = newPassword;
                            int s = await userModel.save(newUser);

                            //MainWindow.userLogin.password = newPassword;
                            //int s = await userModel.save(MainWindow.userLogin);

                            if (s>0)
                            {
                                userID = s;
                                if (!Properties.Settings.Default.password.Equals(""))
                                {
                                    Properties.Settings.Default.password = pb_newPassword.Password;
                                    Properties.Settings.Default.Save();
                                }
                                MainWindow.userLogin = await FillCombo.user.getUserById(userID);
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopPasswordChanged"), animation: ToasterAnimation.FadeIn);
                                this.Close();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
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


        private void P_showOldPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {

                tb_oldPassword.Text = pb_oldPassword.Password;
                tb_oldPassword.Visibility = Visibility.Visible;
                pb_oldPassword.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void P_showOldPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {


                tb_oldPassword.Visibility = Visibility.Collapsed;
                pb_oldPassword.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void P_showNewPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {


                tb_newPassword.Text = pb_newPassword.Password;
                tb_newPassword.Visibility = Visibility.Visible;
                pb_newPassword.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void P_showNewPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {


                tb_newPassword.Visibility = Visibility.Collapsed;
                pb_newPassword.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void P_showConfirmPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {


                tb_confirmPassword.Text = pb_confirmPassword.Password;
                tb_confirmPassword.Visibility = Visibility.Visible;
                pb_confirmPassword.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void P_showConfirmPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {


                tb_confirmPassword.Visibility = Visibility.Collapsed;
                pb_confirmPassword.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {


                pb_oldPassword.Clear();
                tb_oldPassword.Clear();
                pb_newPassword.Clear();
                tb_newPassword.Clear();
                pb_confirmPassword.Clear();
                tb_confirmPassword.Clear();
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
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

        private void validateEmpty(string name, object sender)
        {
            try
            {


                if (name == "PasswordBox")
                {
                    if ((sender as PasswordBox).Name == "pb_oldPassword")
                        if (((PasswordBox)sender).Password.Equals(""))
                            HelpClass.SetValidate( p_error_oldPassword,  "trEmptyPasswordToolTip");
                        else
                            HelpClass.clearValidate( p_error_oldPassword);
                    else if ((sender as PasswordBox).Name == "pb_newPassword")
                        if (((PasswordBox)sender).Password.Equals(""))
                            HelpClass.SetValidate( p_error_newPassword, "trEmptyPasswordToolTip");
                        else
                            HelpClass.clearValidate( p_error_newPassword);
                    else if ((sender as PasswordBox).Name == "pb_confirmPassword")
                        if (((PasswordBox)sender).Password.Equals(""))
                            HelpClass.SetValidate( p_error_confirmPassword,  "trEmptyPasswordToolTip");
                        else
                            HelpClass.clearValidate( p_error_confirmPassword);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

    }
}
