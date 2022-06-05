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
        SetValues clothesrow = new SetValues();
        SetValues carpetsrow = new SetValues();
        SetValues carsrow = new SetValues();

        string clothes;
        string carpets;
        string cars;

        List<SetValues> serviceList = new List<SetValues>();
        async Task Getprintparameter()
        {

            serviceList = await setvalueModel.GetBySetvalNote("typesOfService");

            clothesrow = serviceList.Where(X => X.name == "typesOfService_clothes").FirstOrDefault();
            clothes = clothesrow.value;

            carsrow = serviceList.Where(X => X.name == "typesOfService_carpets").FirstOrDefault();
            cars = carsrow.value;
           
            carpetsrow = serviceList.Where(X => X.name == "typesOfService_cars").FirstOrDefault();
            carpets = carpetsrow.value;


            if (clothes == "1")
            {
                tgl_clothes.IsChecked = true;
            }
            else
            {
                tgl_clothes.IsChecked = false;
            }
            //
             if (carpets == "1")
            {
                tgl_carpets.IsChecked = true;
            }
            else
            {
                tgl_carpets.IsChecked = false;
            }
            //
            if (cars == "1")
            {
                tgl_cars.IsChecked = true;
            }
            else
            {
                tgl_cars.IsChecked = false;
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
           
            txt_clothes.Text = AppSettings.resourcemanager.GetString("clothes");
            txt_carpets.Text = AppSettings.resourcemanager.GetString("carpets");
            txt_cars.Text = AppSettings.resourcemanager.GetString("cars");

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
                if (!(bool)tgl_clothes.IsChecked && !(bool)tgl_carpets.IsChecked && !(bool)tgl_cars.IsChecked)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("atLeastOneMustBeSelected"), animation: ToasterAnimation.FadeIn);
                }
                else
                {

                    HelpClass.StartAwait(grid_main);
                    //  string msg = "";
                    int msg = 0;
                    if ((bool)tgl_clothes.IsChecked)
                    {
                        clothesrow.value = "1";

                    }
                    else
                    {
                        clothesrow.value = "0";
                    }

                    if ((bool)tgl_carpets.IsChecked)
                    {
                        carpetsrow.value = "1";

                    }
                    else
                    {
                        carpetsrow.value = "0";
                    }

                    if ((bool)tgl_cars.IsChecked)
                    {
                        carsrow.value = "1";

                    }
                    else
                    {
                        carsrow.value = "0";
                    }


                    AppSettings.typesOfService_clothes = clothesrow.value;
                    AppSettings.typesOfService_carpets = carpetsrow.value;
                    AppSettings.typesOfService_cars = carsrow.value;

                    msg = await setvalueModel.Save(clothesrow);
                    msg = await setvalueModel.Save(carpetsrow);
                    msg = await setvalueModel.Save(carsrow);
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
