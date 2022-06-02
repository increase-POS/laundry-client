using netoaster;
using laundryApp.Classes;
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
    /// Interaction logic for wd_selectDelivery.xaml
    /// </summary>
    public partial class wd_selectDelivery : Window
    {
        public int? shippingCompanyId { get; set; }
        public int? shippingUserId { get; set; }
        public int? customerId { get; set; }
        public wd_selectDelivery()
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
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_select_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }

        public bool isOk { get; set; }
        public static List<string> requiredControlList = new List<string>();


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

             await FillCombo.FillComboShippingCompanies(cb_company);
                await FillCombo.FillComboUsersForDelivery(cb_user, "deliveryEmployee",(int)customerId);

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
            txt_title.Text = AppSettings.resourcemanager.GetString("trDelivery");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_company, AppSettings.resourcemanager.GetString("trCompanyHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_user, AppSettings.resourcemanager.GetString("trUserHint"));
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");

        }
        private void fillInputs()
        {
            if (shippingCompanyId != 0 && shippingCompanyId != null)
            {
                cb_company.SelectedValue = shippingCompanyId;
            }
            if (shippingUserId != 0 && shippingUserId != null)
            {
                cb_user.SelectedValue = shippingUserId;
            }
           
        }
        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            if (HelpClass.validate(requiredControlList, this))
            {
                isOk = true;
                this.Close();
            }
        }
        ShippingCompanies shippingCompany = new ShippingCompanies();
        public decimal _DeliveryCost = 0;
        public decimal _RealDeliveryCost = 0;
        private void Cb_company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);
                if (cb_company.SelectedIndex > 0)
                {
                    shippingCompany = FillCombo.shippingCompaniesList.Find(c => c.shippingCompanyId == (int)cb_company.SelectedValue);
                    _DeliveryCost = (decimal)shippingCompany.deliveryCost;
                    _RealDeliveryCost = (decimal)shippingCompany.realDeliveryCost;
                    shippingCompanyId = (int)cb_company.SelectedValue;

                    if (shippingCompany.deliveryType == "local")
                    {
                        brd_user.Visibility = Visibility.Visible;
                        requiredControlList = new List<string> { "user"};

                    }
                    else
                    {
                        shippingUserId = null;
                        cb_user.SelectedIndex = -1;
                        brd_user.Visibility = Visibility.Collapsed;
                        p_error_user.Visibility = Visibility.Collapsed;

                        requiredControlList = new List<string> { "" };

                    }
                }
                else
                {
                    shippingCompany = new ShippingCompanies();
                    shippingCompanyId = 0;
                        shippingUserId = null;
                    cb_user.SelectedIndex = -1;
                    _DeliveryCost = 0;
                    _RealDeliveryCost = 0;
                    brd_user.Visibility = Visibility.Collapsed;
                    p_error_user.Visibility = Visibility.Collapsed;

                    requiredControlList = new List<string> { "" };
                }
                tb_deliveryCost.Text = _DeliveryCost.ToString();
               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                HelpClass.StartAwait(grid_main);
                #region
                //com
                HelpClass.clearValidate(p_error_user);
                if (shippingCompany.deliveryType == "local" && cb_user.SelectedIndex != -1 )
                {
                    shippingUserId = (int)cb_user.SelectedValue;
                }
                #endregion
               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        string input;
        decimal _decimal;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
