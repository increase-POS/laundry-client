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
    /// Interaction logic for wd_selectTime.xaml
    /// </summary>
    public partial class wd_selectTime : Window
    {
        public wd_selectTime()
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
        public bool isOk { get; set; }
        public DateTime orderTime { get; set; }
        public static List<string> requiredControlList = new List<string>();
        private  void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "time" };

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                fillInputs();
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
            txt_title.Text = AppSettings.resourcemanager.GetString("time");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tp_time, AppSettings.resourcemanager.GetString("time"));
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");
        }

        private void fillInputs()
        {
            if (orderTime != null && orderTime != DateTime.MinValue)
            {
                tp_time.SelectedTime = orderTime;
            }
          
        }
        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            // if have id return true

            if (HelpClass.validate(requiredControlList, this))
            {
                isOk = true;
                orderTime =(DateTime)tp_time.SelectedTime;
                
                this.Close();
            }
        }
    }
}
