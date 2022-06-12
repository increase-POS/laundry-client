using laundryApp.Classes;
using netoaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for wd_pointSettings.xaml
    /// </summary>
    public partial class wd_pointSettings : Window
    {
        public wd_pointSettings()
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

        private static wd_pointSettings _instance;
        public static wd_pointSettings Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new wd_pointSettings();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }


        public static List<string> requiredControlList;
        SetValues setValues = new SetValues();

        List<SetValues> setVInvoice = new List<SetValues>(); 
        List<SetValues> setVCash = new List<SetValues>();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "cash" , "invoice" };

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

                if (AppSettings.settingsList.Count == 0)
                    AppSettings.settingsList = await AppSettings.setModel.GetAll();
                
                setVCash = await setValues.GetBySetName("cashForPoint");
                setVInvoice = await setValues.GetBySetName("PointsForInvoice");


                tb_cash.Text = setVCash[0].value.ToString();
                tb_invoice.Text = setVInvoice[0].value.ToString();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region method
        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trPointSettings");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_cash, AppSettings.resourcemanager.GetString("trCash")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_invoice, AppSettings.resourcemanager.GetString("trInvoice")+"...");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
        }

        #endregion

        #region events
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

        string input;
        decimal _decimal = 0;
        private void Tb_decimal_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only  digits
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
                else if (textBox.Tag.ToString() == "decimal")
                {
                    input = e.Text;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);

                }
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
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
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

        private void Tb_lostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Tb_textChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                HelpClass.StartAwait(grid_main);

                if (HelpClass.validate(requiredControlList, this))
                {
                    if (setVCash == null)
                        setValues = new SetValues();
                    else
                        setValues = setVCash[0];
                    //save cash
                    SettingCls set = AppSettings.settingsList.Where(s => s.name == "cashForPoint").FirstOrDefault();
                    setValues.value = tb_cash.Text;
                    setValues.isSystem = 1;
                    setValues.settingId = set.settingId;
                    int cashRes = await setValues.Save(setValues);

                    if (setVInvoice == null)
                        setVInvoice = new List<SetValues>();
                    else
                        setValues = setVInvoice[0];
                    //save invoice
                    set = AppSettings.settingsList.Where(s => s.name == "PointsForInvoice").FirstOrDefault();
                    setValues.value = tb_invoice.Text;
                    setValues.isSystem = 1;
                    setValues.settingId = set.settingId;
                    int invoiceRes = await setValues.Save(setValues);

                    if ((cashRes > 0) && (invoiceRes > 0) )
                    {
                        //update tax in app settings
                        AppSettings.cashForPoint = int.Parse(tb_cash.Text);
                        AppSettings.PointsForInvoice = int.Parse(tb_invoice.Text);

                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        this.Close();
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                }

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
