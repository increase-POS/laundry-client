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
    /// Interaction logic for wd_typesOfService.xaml
    /// </summary>
    public partial class wd_typesOfService : Window
    {

        public wd_typesOfService()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }

        SetValues setvalueModel = new SetValues();
        // typesOfService
        SetValues diningHallrow = new SetValues();
        SetValues takeAwayrow = new SetValues();
        SetValues selfServicerow = new SetValues();

        string diningHall;
        string takeAway;
        string selfService;

        List<SetValues> serviceList = new List<SetValues>();
        async Task Getprintparameter()
        {

            serviceList = await setvalueModel.GetBySetvalNote("typesOfService");

            diningHallrow = serviceList.Where(X => X.name == "typesOfService_diningHall").FirstOrDefault();
            diningHall = diningHallrow.value;

            selfServicerow = serviceList.Where(X => X.name == "typesOfService_takeAway").FirstOrDefault();
            selfService = selfServicerow.value;
           
            takeAwayrow = serviceList.Where(X => X.name == "typesOfService_selfService").FirstOrDefault();
            takeAway = takeAwayrow.value;


            if (diningHall == "1")
            {
                tgl_diningHall.IsChecked = true;
            }
            else
            {
                tgl_diningHall.IsChecked = false;
            }
            //
             if (takeAway == "1")
            {
                tgl_takeAway.IsChecked = true;
            }
            else
            {
                tgl_takeAway.IsChecked = false;
            }
            //
            if (selfService == "1")
            {
                tgl_selfService.IsChecked = true;
            }
            else
            {
                tgl_selfService.IsChecked = false;
            }



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
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                #endregion
                await Getprintparameter();



                
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
            
            txt_title.Text = AppSettings.resourcemanager.GetString("typesOfService");
           
            txt_diningHall.Text = AppSettings.resourcemanager.GetString("trDiningHallType");
            txt_takeAway.Text = AppSettings.resourcemanager.GetString("trTakeAway");
            txt_selfService.Text = AppSettings.resourcemanager.GetString("trSelfService");

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

        List<SettingCls> set = new List<SettingCls>();

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
                if (!(bool)tgl_diningHall.IsChecked && !(bool)tgl_takeAway.IsChecked && !(bool)tgl_selfService.IsChecked)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("atLeastOneMustBeSelected"), animation: ToasterAnimation.FadeIn);
                }
                else
                {

                    HelpClass.StartAwait(grid_main);
                    //  string msg = "";
                    int msg = 0;
                    if ((bool)tgl_diningHall.IsChecked)
                    {
                        diningHallrow.value = "1";

                    }
                    else
                    {
                        diningHallrow.value = "0";
                    }

                    if ((bool)tgl_takeAway.IsChecked)
                    {
                        takeAwayrow.value = "1";

                    }
                    else
                    {
                        takeAwayrow.value = "0";
                    }

                    if ((bool)tgl_selfService.IsChecked)
                    {
                        selfServicerow.value = "1";

                    }
                    else
                    {
                        selfServicerow.value = "0";
                    }


                    AppSettings.typesOfService_diningHall = diningHallrow.value;
                    AppSettings.typesOfService_takeAway = takeAwayrow.value;
                    AppSettings.typesOfService_selfService = selfServicerow.value;

                    msg = await setvalueModel.Save(diningHallrow);
                    msg = await setvalueModel.Save(takeAwayrow);
                    msg = await setvalueModel.Save(selfServicerow);
                    await Getprintparameter();
                    await MainWindow.Getprintparameter();
                    if (msg > 0)
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        await Task.Delay(1500);
                        this.Close();
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }

            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
