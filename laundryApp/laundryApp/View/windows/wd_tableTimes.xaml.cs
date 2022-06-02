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

using System.Drawing.Printing;
using netoaster;
using System.Text.RegularExpressions;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_tableTimes.xaml
    /// </summary>
    public partial class wd_tableTimes : Window
    {

        public wd_tableTimes()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        BrushConverter bc = new BrushConverter();

        //print
        //string time_staying;
        //string maximumTimeToKeepReservation;
        //string warningTimeForLateReservation;
        //SettingCls set = new SettingCls();
        SetValues setV = new SetValues();

        public SetValues time_stayingSetValues = new SetValues();
        public SetValues warningTimeForLateReservationSetValues = new SetValues();
        public SetValues maximumTimeToKeepReservationSetValues = new SetValues();
        async Task refreshWindow()
        {
            await get_time_staying();
            tb_time_staying.Text = AppSettings.time_staying.ToString();
            await get_warningTimeForLateReservation();
            tb_warningTimeForLateReservation.Text = AppSettings.warningTimeForLateReservation.ToString();
            await get_maximumTimeToKeepReservation();
            tb_maximumTimeToKeepReservation.Text = AppSettings.maximumTimeToKeepReservation.ToString();
        }
        public  async Task get_time_staying()
        {
            List<SetValues> settingsValues = await AppSettings.valueModel.GetBySetName("time_staying");
            time_stayingSetValues = settingsValues.FirstOrDefault();
            if (time_stayingSetValues != null)
                AppSettings.time_staying = double.Parse(time_stayingSetValues.value);
            else
                AppSettings.time_staying = 0;
        }
        public async Task get_warningTimeForLateReservation()
        {
            List<SetValues> settingsValues = await AppSettings.valueModel.GetBySetName("warningTimeForLateReservation");
            warningTimeForLateReservationSetValues = settingsValues.FirstOrDefault();
            if (warningTimeForLateReservationSetValues != null)
                AppSettings.warningTimeForLateReservation = int.Parse(warningTimeForLateReservationSetValues.value);
            else
                AppSettings.warningTimeForLateReservation = 0;
        }
        public async Task get_maximumTimeToKeepReservation()
        {
            List<SetValues> settingsValues = await AppSettings.valueModel.GetBySetName("maximumTimeToKeepReservation");
            maximumTimeToKeepReservationSetValues = settingsValues.FirstOrDefault();
            if (maximumTimeToKeepReservationSetValues != null)
                AppSettings.maximumTimeToKeepReservation = double.Parse(maximumTimeToKeepReservationSetValues.value);
            else
                AppSettings.maximumTimeToKeepReservation = 0;
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load

            try
            {
                
                    HelpClass.StartAwait(grid_main);

                #region translate

                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;

                translate();
                #endregion

                //
                await refreshWindow();


                
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

            //txt_title.Text = AppSettings.resourcemanager.GetString("trCopyCount");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_time_staying, AppSettings.resourcemanager.GetString("trPurchasesCopyCount"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_maximumTimeToKeepReservation, AppSettings.resourcemanager.GetString("trSalesCopyCount"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_warningTimeForLateReservation, AppSettings.resourcemanager.GetString("trReportsCopyCount"));

            txt_title.Text = AppSettings.resourcemanager.GetString("tablesTimes");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_time_staying, AppSettings.resourcemanager.GetString("timeStaying"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_maximumTimeToKeepReservation, AppSettings.resourcemanager.GetString("maximumTimeToKeepReservation"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_warningTimeForLateReservation, AppSettings.resourcemanager.GetString("warningTimeForLateReservation"));
            txt_hour.Text = AppSettings.resourcemanager.GetString("hour");
            txt_hour1.Text = AppSettings.resourcemanager.GetString("hour");
            txt_minute.Text = AppSettings.resourcemanager.GetString("trMinute");
            
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");


        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    //Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                int msg;
            time_stayingSetValues.value = (string)tb_time_staying.Text;
            maximumTimeToKeepReservationSetValues.value = (string)tb_maximumTimeToKeepReservation.Text;
            warningTimeForLateReservationSetValues.value = (string)tb_warningTimeForLateReservation.Text;
            if (double.Parse(time_stayingSetValues.value) <= 0 ||
                double.Parse(maximumTimeToKeepReservationSetValues.value) <= 0 ||
                int.Parse(warningTimeForLateReservationSetValues.value) <= 0)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMustBeMoreThanZero"), animation: ToasterAnimation.FadeIn);
            }
            else
            {

                msg = await setV.Save(time_stayingSetValues);
                msg = await setV.Save(maximumTimeToKeepReservationSetValues);
                msg = await setV.Save(warningTimeForLateReservationSetValues);

                await refreshWindow();
                //await MainWindow.Getprintparameter();
                if (msg > 0)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    //await Task.Delay(1500);
                    this.Close();
                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
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
                if (name == "TextBox")
                {
                    if ((sender as TextBox).Name == "tb_time_staying" && string.IsNullOrWhiteSpace((sender as TextBox).Text))
                        HelpClass.SetValidate(p_errortime_staying, "trEmptyError");
                    else if ((sender as TextBox).Name == "tb_maximumTimeToKeepReservation" && string.IsNullOrWhiteSpace((sender as TextBox).Text))
                        HelpClass.SetValidate(p_errormaximumTimeToKeepReservation, "trEmptyError");
                    else if ((sender as TextBox).Name == "tb_warningTimeForLateReservation" && string.IsNullOrWhiteSpace((sender as TextBox).Text))
                        HelpClass.SetValidate(p_errorwarningTimeForLateReservation, "trEmptyError");
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }


        private void Tb_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                {
                    return;
                }



                if (textBox.Name == "tb_time_staying" || textBox.Name == "tb_maximumTimeToKeepReservation" || textBox.Name == "tb_warningTimeForLateReservation")
                    HelpClass.InputJustNumber(ref textBox);

                //if (textBox.Name == "tb_time_staying")
                //{
                //    if (int.TryParse(textBox.Text, out _numtime_staying))
                //        numtime_staying = int.Parse(textBox.Text);
                //}
                //else if (textBox.Name == "tb_maximumTimeToKeepReservation")
                //{
                //    if (int.TryParse(textBox.Text, out _nummaximumTimeToKeepReservation))
                //        nummaximumTimeToKeepReservation = int.Parse(textBox.Text);
                //}
                //else if (textBox.Name == "tb_warningTimeForLateReservation")
                //{
                //    if (int.TryParse(textBox.Text, out _numwarningTimeForLateReservation))
                //        numwarningTimeForLateReservation = int.Parse(textBox.Text);
                //}

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Tb_count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Tb_PreventSpaces(object sender, KeyEventArgs e)
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
        #region NumericCount



        private int _numValue_time_staying = 0;
        public int numValue_time_staying
        {
            get
            {
                if (int.TryParse(tb_time_staying.Text, out _numValue_time_staying))
                    _numValue_time_staying = int.Parse(tb_time_staying.Text);
                return _numValue_time_staying;
            }
            set
            {
                _numValue_time_staying = value;
                tb_time_staying.Text = value.ToString();
            }
        }


        private int _numValue_maximumTimeToKeepReservation = 0;
        public int numValue_maximumTimeToKeepReservation
        {
            get
            {
                if (int.TryParse(tb_maximumTimeToKeepReservation.Text, out _numValue_maximumTimeToKeepReservation))
                    _numValue_maximumTimeToKeepReservation = int.Parse(tb_maximumTimeToKeepReservation.Text);
                return _numValue_maximumTimeToKeepReservation;
            }
            set
            {
                _numValue_maximumTimeToKeepReservation = value;
                tb_maximumTimeToKeepReservation.Text = value.ToString();
            }
        }


        private int _numValue_warningTimeForLateReservation = 0;
        public int numValue_warningTimeForLateReservation
        {
            get
            {
                if (int.TryParse(tb_warningTimeForLateReservation.Text, out _numValue_warningTimeForLateReservation))
                    _numValue_warningTimeForLateReservation = int.Parse(tb_warningTimeForLateReservation.Text);
                return _numValue_warningTimeForLateReservation;
            }
            set
            {
                _numValue_warningTimeForLateReservation = value;
                tb_warningTimeForLateReservation.Text = value.ToString();
            }
        }
        private void Btn_countDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;

                if (button.Tag.ToString() == "time_staying" && numValue_time_staying > 0)
                    numValue_time_staying--;
                else if (button.Tag.ToString() == "maximumTimeToKeepReservation" && numValue_maximumTimeToKeepReservation > 0)
                    numValue_maximumTimeToKeepReservation--;
                else if (button.Tag.ToString() == "warningTimeForLateReservation" && numValue_warningTimeForLateReservation > 0)
                    numValue_warningTimeForLateReservation--;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_countUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                if (button.Tag.ToString() == "time_staying")
                    numValue_time_staying++;
                else if (button.Tag.ToString() == "maximumTimeToKeepReservation")
                    numValue_maximumTimeToKeepReservation++;
                else if (button.Tag.ToString() == "warningTimeForLateReservation")
                    numValue_warningTimeForLateReservation++;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
    }
}
