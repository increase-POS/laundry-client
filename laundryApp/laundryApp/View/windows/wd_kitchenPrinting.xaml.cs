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
    /// Interaction logic for wd_kitchenPrinting.xaml
    /// </summary>
    public partial class wd_kitchenPrinting : Window
    {

        public wd_kitchenPrinting()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }

        SetValues setvalueModel = new SetValues();
        // print
        SetValues printOnSalerow = new SetValues();
        SetValues printOnPreparingrow = new SetValues();

        string printOnSale;
        string printOnPreparing;

        List<SetValues> printList = new List<SetValues>();
        async Task Getprintparameter()
        {

            printList = await setvalueModel.GetBySetvalNote("print");




            printOnSalerow = printList.Where(X => X.name == "print_kitchen_on_sale").FirstOrDefault();
            printOnSale = printOnSalerow.value;

            printOnPreparingrow = printList.Where(X => X.name == "print_kitchen_on_preparing").FirstOrDefault();
            printOnPreparing = printOnPreparingrow.value;

            
            if (printOnPreparing == "1")
            {
                tgl_printOnPreparing.IsChecked = true;
            }
            else
            {
                tgl_printOnPreparing.IsChecked = false;
            }
            //
            if (printOnSale == "1")
            {
                tgl_printOnSale.IsChecked = true;
            }
            else
            {
                tgl_printOnSale.IsChecked = false;
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

            txt_title.Text = AppSettings.resourcemanager.GetString("kitchenPrinting");
            txt_printOnSale.Text = AppSettings.resourcemanager.GetString("printOnSale");
            txt_printOnPreparing.Text = AppSettings.resourcemanager.GetString("printOnPreparing");

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

                HelpClass.StartAwait(grid_main);
                //  string msg = "";
                int msg = 0;
                if ((bool)tgl_printOnPreparing.IsChecked)
                {
                    printOnPreparingrow.value = "1";

                }
                else
                {
                    printOnPreparingrow.value = "0";
                }

                if ((bool)tgl_printOnSale.IsChecked)
                {
                    printOnSalerow.value = "1";

                }
                else
                {
                    printOnSalerow.value = "0";
                }

                AppSettings.print_kitchen_on_sale = printOnSalerow.value;
                AppSettings.print_kitchen_on_preparing = printOnPreparingrow.value;

                msg = await setvalueModel.Save(printOnPreparingrow);
                msg = await setvalueModel.Save(printOnSalerow);
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
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
