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
    /// Interaction logic for wd_selectUser.xaml
    /// </summary>
    public partial class wd_selectUser : Window
    {
        public wd_selectUser()
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
                    Btn_select_Click(btn_select, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public string userJob;
        public int userId;
        public bool isOk { get; set; }
        public static List<string> requiredControlList = new List<string>();
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "userId" };

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                if(userJob == "waiter")
                    path_title.Data = App.Current.Resources["waiter"] as Geometry;

                await FillCombo.FillComboUsersWithJob(cb_userId, userJob);
                if (userId != 0)
                    cb_userId.SelectedValue = userId;
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

            if (userJob == "waiter")
                txt_title.Text = AppSettings.resourcemanager.GetString("trWaiter");


            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_userId, AppSettings.resourcemanager.GetString("trUser"));
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");



        }

        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            // if have id return true
            
            if (HelpClass.validate(requiredControlList, this))
            {
                isOk = true;
                userId = (int) cb_userId.SelectedValue;
                this.Close();
            }
            // else return false
            //isOk = false;
        }
    }
}
