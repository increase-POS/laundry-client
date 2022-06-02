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

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_messageBoxWithIcon.xaml
    /// </summary>
    public partial class wd_messageBoxWithIcon : Window
    {

        public bool isOk;

        public wd_messageBoxWithIcon()
        {
            try
            {
                InitializeComponent();
                this.DataContext = this;
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

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void translate()
        {
                btn_ok.Content = AppSettings.resourcemanager.GetString("trOK");
            txt_title.Text = AppSettings.resourcemanager.GetString("trWarning");
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isOk = false;
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        #region contentText1
        public static readonly DependencyProperty contentText1DependencyProperty = DependencyProperty.Register("contentText1",
            typeof(string),
            typeof(wd_acceptCancelPopup),
            new PropertyMetadata("DEFAULT"));
        public string contentText1
        {
            set
            { SetValue(contentText1DependencyProperty, value); }
            get
            { return (string)GetValue(contentText1DependencyProperty); }
        }
        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }


        private void HandleKeyPress(object sender, KeyEventArgs e)
        {

        }


    }
}
