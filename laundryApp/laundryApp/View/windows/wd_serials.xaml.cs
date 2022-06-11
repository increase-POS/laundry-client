using netoaster;
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
    /// Interaction logic for wd_serials.xaml
    /// </summary>
    public partial class wd_serials : Window
    {
        private static wd_serials _instance;
        public static wd_serials Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new wd_serials();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public wd_serials()
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
        public string activationCode = "";
        public static List<string> requiredControlList;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "serial" };

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

                tb_serial.Text = activationCode;

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
            txt_title.Text = AppSettings.resourcemanager.GetString("trSerial");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_serial, AppSettings.resourcemanager.GetString("trSerial") + "...");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
            tt_close.Content = AppSettings.resourcemanager.GetString("trClose");
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {//close
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {//closing

            try
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        AvtivateServer asModel = new AvtivateServer();
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            int res = 0;

            if (HelpClass.validate(requiredControlList, this))
            {
                res = await asModel.updatesalecode(tb_serial.Text);

                if (res > 0)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    await Task.Delay(2000);
                    this.Close();
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            }

        }
    }
}
