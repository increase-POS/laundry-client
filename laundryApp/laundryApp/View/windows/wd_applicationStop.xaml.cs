using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_applicationStop.xaml
    /// </summary>
    public partial class wd_applicationStop : Window
    {
        public wd_applicationStop()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        BrushConverter bc = new BrushConverter();
        Pos posModel = new Pos();
        IEnumerable<Pos> poss;
        IEnumerable<CashTransfer> cashesQuery;
        IEnumerable<CashTransfer> cashes;
        CashTransfer cashModel = new CashTransfer();
        bool isAdmin;
        public string status = "";
        public int settingsPoSId = 0;
        public int userId;
        bool flag = false;

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_changePassword);

                txt_moneyIcon.Text = AppSettings.Currency;

                #region translate

                if (AppSettings.lang.Equals("en"))
                     grid_changePassword.FlowDirection = FlowDirection.LeftToRight;
                else
                     grid_changePassword.FlowDirection = FlowDirection.RightToLeft;

                translate();
                #endregion

                await fillPosInfo();
                await fillPos();
                isAdmin = HelpClass.isAdminPermision();
                
                HelpClass.EndAwait(grid_changePassword);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_changePassword);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task fillPos()
        {
            poss = await posModel.Get();
            var pos = poss.Where(p => p.branchId == MainWindow.branchLogin.branchId && p.posId != MainWindow.posLogin.posId);
            cb_pos.ItemsSource = pos;
            cb_pos.DisplayMemberPath = "name";
            cb_pos.SelectedValuePath = "posId";
            cb_pos.SelectedIndex = -1;
        }
        private async Task fillPosInfo()
        {
            await MainWindow.refreshBalance();
            //cashes = await cashModel.GetCashTransfer("d", "p");
            cashes = await cashModel.GetCashTransferForPosById("all", "p",(int)MainWindow.posLogin.posId);
            cashesQuery = cashes.Where(s => s.isConfirm == 1 
                                                && s.posId == MainWindow.posLogin.posId
                                                && s.isConfirm2 == 0).ToList();


            if (cashesQuery.Count() == 0)
            {
                txt_balanceState.Text = AppSettings.resourcemanager.GetString("trAvailable");
                btn_save.IsEnabled = true;
            }
            else
            {
                txt_balanceState.Text = AppSettings.resourcemanager.GetString("trWaiting");
                btn_save.IsEnabled = false;
            }

            if (MainWindow.posLogin.balance != 0)
                txt_cashValue.Text = HelpClass.DecTostring(MainWindow.posLogin.balance);
            else
                txt_cashValue.Text = "0";

            status = MainWindow.posLogin.boxState;
            if (MainWindow.posLogin.boxState == "c")
            {
                txt_balanceState.Text = AppSettings.resourcemanager.GetString("trUnavailable");
                txt_stateValue.Text = AppSettings.resourcemanager.GetString("trClosed");
                txt_stateValue.Foreground = Application.Current.Resources["MainColorRed"] as SolidColorBrush; ;
                tgl_isClose.IsChecked = false;
                btn_save.IsEnabled = false;
                cb_pos.IsEnabled = false;

            }
            else
            {
                txt_stateValue.Text = AppSettings.resourcemanager.GetString("trOpen");
                txt_stateValue.Foreground = Application.Current.Resources["mediumGreen"] as SolidColorBrush; ;
                tgl_isClose.IsChecked = true;
                cb_pos.IsEnabled = true;

            }
        }
        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trDailyClosing");
            txt_cash.Text = AppSettings.resourcemanager.GetString("trCash");
            txt_boxState.Text = AppSettings.resourcemanager.GetString("trBoxState");
            txt_cashBalance.Text = AppSettings.resourcemanager.GetString("trCashBalance");
            txt_transfer.Text = AppSettings.resourcemanager.GetString("trTransfer");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_pos, AppSettings.resourcemanager.GetString("trPosHint"));

            tt_pos.Content = AppSettings.resourcemanager.GetString("trPosTooltip");
            btn_save.Content = AppSettings.resourcemanager.GetString("trTransfer");
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
            { HelpClass.ExceptionMessage(ex, this); }
        }
         private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
              try
              {
                  HelpClass.StartAwait(grid_changePassword);
                  if (cashesQuery.Count() > 0)
                      Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trCantDoProcess"), animation: ToasterAnimation.FadeIn);
                  else
                  { 
                      bool valid = validate();
                      if (valid)
                      {
                          await transfer();
                          await fillPosInfo();
                      }
                  }

                  HelpClass.EndAwait(grid_changePassword);
              }
              catch (Exception ex)
              {
                  HelpClass.EndAwait(grid_changePassword);
                  HelpClass.ExceptionMessage(ex, this);
              }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                cb_pos.SelectedIndex = -1;
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
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
            if (name == "ComboBox")
            {
                if ((sender as ComboBox).Name == "cb_pos")
                    validateEmptyComboBox((ComboBox)sender, p_error_pos, tt_errorPos, "trErrorEmptyPosToolTip");
            }
        }
         private void validateEmptyComboBox(ComboBox cb, Path p_error, ToolTip tt_error, string tr)
        {
            if (cb.SelectedIndex == -1)
            {
                p_error.Visibility = Visibility.Visible;
                tt_error.Content = AppSettings.resourcemanager.GetString(tr);
                cb.Background = (Brush)bc.ConvertFrom("#15FF0000");
            }
            else
            {
                cb.Background = (Brush)bc.ConvertFrom("#f8f8f8");
                p_error.Visibility = Visibility.Collapsed;
            }
        }

        private async void Tgl_isClose_Checked(object sender, RoutedEventArgs e)
        {
            if (flag)
                return;

            flag = true;
            ToggleButton cb = sender as ToggleButton;
            if (cb.IsFocused == true)
            {
                #region Accept
                this.Opacity = 0;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxConfirm");
                w.ShowDialog();
                
                #endregion
                if (w.isOk)
                {
                    status = "o";

                    await openCloseBox(status);                   
                    await fillPosInfo();
                }
                else
                    tgl_isClose.IsChecked = false;
                this.Opacity = 1;

            }
            flag = false;
        }

        private async void Tgl_isClose_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flag)
                return;
            flag = true;
            ToggleButton cb = sender as ToggleButton;
            if (cb.IsFocused == true)
            {
                if (cashesQuery.Count() > 0)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trCantDoProcess"), animation: ToasterAnimation.FadeIn);
                    tgl_isClose.IsChecked = true;
                }
                else
                {
                    #region Accept
                    //this.Visibility = Visibility.Collapsed;
                    this.Opacity = 0;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxConfirm");
                    w.ShowDialog();
                    #endregion
                    if (w.isOk)
                    {
                        status = "c";

                        await openCloseBox(status);
                        await fillPosInfo();
                    }
                    else
                        tgl_isClose.IsChecked = true;
                    this.Opacity = 1;
                    //this.Visibility = Visibility.Visible;
                }
            }
            flag = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
        #region open - close - validate
        private async Task openCloseBox(string status)
        {
            CashTransfer cashTransfer = new CashTransfer();
            cashTransfer.processType = "box";
            cashTransfer.transType = status;
            cashTransfer.cash = MainWindow.posLogin.balance;
            cashTransfer.createUserId = MainWindow.userLogin.userId;
            cashTransfer.posId = (int)MainWindow.posLogin.posId;
            if (status == "o")
                cashTransfer.transNum = await cashTransfer.generateCashNumber("bc");
            else
                cashTransfer.transNum = await cashTransfer.getLastOpenTransNum((int)MainWindow.posLogin.posId);
            int res = await posModel.updateBoxState((int)MainWindow.posLogin.posId, status,Convert.ToInt32(isAdmin),MainWindow.userLogin.userId,cashTransfer);
            if (res > 0)
            {
                await MainWindow.refreshBalance();
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
            }
            else
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        }
        private async Task transfer()
        {
            //add cash transfer
            CashTransfer cash1 = new CashTransfer();

            cash1.transType = "p";//pull
            cash1.transNum = await cash1.generateCashNumber(cash1.transType + "p");
            cash1.cash = MainWindow.posLogin.balance;
            cash1.createUserId = MainWindow.userLogin.userId;
            cash1.posIdCreator = MainWindow.posLogin.posId;
            cash1.isConfirm = 1;
            cash1.side = "p";//pos
            cash1.posId = Convert.ToInt32(MainWindow.posLogin.posId);

            int s1 = await cash1.Save(cash1);

            if (!s1.Equals(0))
            {
                //second operation
                CashTransfer cash2 = new CashTransfer();

                cash2.transType = "d";//deposite
                cash2.transNum = await cash2.generateCashNumber(cash2.transType + "p");
                cash2.cash = MainWindow.posLogin.balance;
                cash2.createUserId = MainWindow.userLogin.userId;
                cash2.posIdCreator = MainWindow.posLogin.posId;
                cash2.isConfirm = 0;
                cash2.side = "p";//pos
                cash2.posId = Convert.ToInt32(cb_pos.SelectedValue);
                cash2.cashTransIdSource = s1;//id from first operation

                int s2 = await cash2.Save(cash2);
                if(s2 > 0)
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        }
        }
        
        private Boolean validate()
        {
            if(MainWindow.posLogin.balance == 0)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trZeroBalance"), animation: ToasterAnimation.FadeIn);
                return false;
            }
            HelpClass.clearValidate(p_error_pos);
            if (cb_pos.SelectedIndex == -1)
                return false;
            return true;
        }
        
        #endregion
    }
}
