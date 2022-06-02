using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
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
    /// Interaction logic for wd_subscriptionFees.xaml
    /// </summary>
    public partial class wd_subscriptionFees : Window
    {
        public int memberShipID = 0;
        public string memberShipType = "";
        public static List<string> requiredControlList;
        SubscriptionFees subscription = new SubscriptionFees();
       
        IEnumerable<SubscriptionFees> subscriptions;
        public wd_subscriptionFees()
        {
            try
            {
                InitializeComponent();
                if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1440)
                {
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;
                }
                else if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1360)
                {
                    txt_add_Icon.Visibility = Visibility.Collapsed;
                    txt_update_Icon.Visibility = Visibility.Collapsed;
                    txt_delete_Icon.Visibility = Visibility.Collapsed;
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;

                }
                else
                {
                    txt_deleteButton.Visibility = Visibility.Collapsed;
                    txt_addButton.Visibility = Visibility.Collapsed;
                    txt_updateButton.Visibility = Visibility.Collapsed;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;

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

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    //Btn_confirmation_Click(null, null);
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
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                HelpClass.StartAwait(grid_subscriptionFees);

                subscription = new SubscriptionFees();

                bool isExist = false;
                if (subscriptions.Any(s => (s.monthsCount == int.Parse(tb_monthsCount.Text)) &&
                                            s.Amount == decimal.Parse(tb_amount.Text)))
                    isExist = true;

                if ((int.Parse(tb_monthsCount.Text) == 0) || (decimal.Parse(tb_amount.Text) == 0) || isExist)
                {
                    if (int.Parse(tb_monthsCount.Text) == 0)
                        HelpClass.SetValidate(p_error_monthsCount, "trEqualZero");

                    if (decimal.Parse(tb_amount.Text) == 0)
                        HelpClass.SetValidate(p_error_amount, "trEqualZero");

                    if (isExist)
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAlreadyExists"), animation: ToasterAnimation.FadeIn);
                }
                else
                {
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        subscription.membershipId = memberShipID;
                        subscription.monthsCount = int.Parse(tb_monthsCount.Text);
                        subscription.Amount = decimal.Parse(tb_amount.Text);
                        subscription.createUserId = MainWindow.userLogin.userId;
                        subscription.updateUserId = MainWindow.userLogin.userId;
                        subscription.notes = "";
                        subscription.isActive = 1;

                        int s = await subscription.save(subscription);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            Clear();
                            await RefreshSubscriptionsList();
                        }
                    }
                }
                HelpClass.EndAwait(grid_subscriptionFees);
               
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_subscriptionFees);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                HelpClass.StartAwait(grid_subscriptionFees);
                if (subscription.subscriptionFeesId > 0)
                {
                    bool isExist = false;
                    if (subscriptions.Any(s => (s.monthsCount == int.Parse(tb_monthsCount.Text)) &&
                                                s.Amount == decimal.Parse(tb_amount.Text)))
                        isExist = true;

                    if ((int.Parse(tb_monthsCount.Text) == 0) || (decimal.Parse(tb_amount.Text) == 0) || isExist)
                    {
                        if (int.Parse(tb_monthsCount.Text) == 0)
                            HelpClass.SetValidate(p_error_monthsCount, "trEqualZero");

                        if (decimal.Parse(tb_amount.Text) == 0)
                            HelpClass.SetValidate(p_error_amount, "trEqualZero");

                        if (isExist)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAlreadyExists"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        if (HelpClass.validate(requiredControlList, this))
                        {
                            subscription.membershipId = memberShipID;
                            subscription.monthsCount = int.Parse(tb_monthsCount.Text);
                            subscription.Amount = decimal.Parse(tb_amount.Text);
                            subscription.updateUserId = MainWindow.userLogin.userId;
                            subscription.notes = "";
                            subscription.isActive = 1;

                            int s = await subscription.save(subscription);
                            if (s <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                await RefreshSubscriptionsList();
                            }
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);
                    }
                }
                HelpClass.EndAwait(grid_subscriptionFees);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_subscriptionFees);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                HelpClass.StartAwait(grid_subscriptionFees);
                if (subscription.subscriptionFeesId != 0)
                {
                    if ((!subscription.canDelete) && (subscription.isActive == 0))
                    {
                        #region
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxActivate");
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion
                        if (w.isOk)
                            await activate();
                    }
                    else
                    {
                        #region
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        if (subscription.canDelete)
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                        if (!subscription.canDelete)
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion
                        if (w.isOk)
                        {
                            string popupContent = "";
                            if (subscription.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                            if ((!subscription.canDelete) && (subscription.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                            int s = await subscription.delete(subscription.subscriptionFeesId, MainWindow.userLogin.userId, subscription.canDelete);
                            if (s < 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await RefreshSubscriptionsList();
                                Clear();
                            }
                        }
                    }
                }
                HelpClass.EndAwait(grid_subscriptionFees);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_subscriptionFees);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async Task activate()
        {//activate

            subscription.isActive = 1;
            int s = await subscription.save(subscription);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshSubscriptionsList();
            }

        }
        private void Dg_subscriptionFees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//selection
            try
            {
                if (dg_subscriptionFees.SelectedIndex != -1)
                {
                    subscription = dg_subscriptionFees.SelectedItem as SubscriptionFees;
                    this.DataContext = subscription;
                    if (subscription != null)
                    {
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_subscriptionFees);

                requiredControlList = new List<string> { "monthsCount", "amount" };

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_subscriptionFees.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_subscriptionFees.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                #endregion

                await RefreshSubscriptionsList();

                Clear();

                HelpClass.EndAwait(grid_subscriptionFees);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_subscriptionFees);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Clear()
        {
            this.DataContext = new SubscriptionFees();
            
            HelpClass.clearValidate(requiredControlList, this);
        }

        async Task<IEnumerable<SubscriptionFees>> RefreshSubscriptionsList()
        {
            subscriptions = await subscription.GetAll();
            subscriptions = subscriptions.Where(s => s.membershipId == memberShipID);
            dg_subscriptionFees.ItemsSource = subscriptions;

            return subscriptions;
        }
     
        private void translate()
        {
            txt_Title.Text = AppSettings.resourcemanager.GetString("trSubscriptionFees");

            if (memberShipType.Equals("m"))
            {
                dg_subscriptionFees.Columns[0].Header = AppSettings.resourcemanager.GetString("trMonthCount");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_monthsCount, AppSettings.resourcemanager.GetString("trMonthCountHint"));
            }
            else if (memberShipType.Equals("y"))
            {
                dg_subscriptionFees.Columns[0].Header = AppSettings.resourcemanager.GetString("trYearCount");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_monthsCount, AppSettings.resourcemanager.GetString("trYearCount")+"...");
            }

            dg_subscriptionFees.Columns[1].Header = AppSettings.resourcemanager.GetString("trAmount");
            
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_amount, AppSettings.resourcemanager.GetString("trAmountHint"));//

            //tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            //tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            //tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");
            tt_close.Content = AppSettings.resourcemanager.GetString("trClose");

            btn_add.Content = AppSettings.resourcemanager.GetString("trAdd");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");
            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only  digits
            try
            {
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

        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
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
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
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
    }
}
