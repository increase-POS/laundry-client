using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
 

namespace laundryApp.View.accounts
{
    /// <summary>
    /// Interaction logic for uc_posTransfers.xaml
    /// </summary>
    public partial class uc_posTransfers : UserControl
    {
        public uc_posTransfers()
        {
            try
            {
                InitializeComponent();
                if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1440)
                {
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_add_Icon.Visibility = Visibility.Visible;
                }
                else if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1360)
                {
                    txt_add_Icon.Visibility = Visibility.Collapsed;
                    txt_addButton.Visibility = Visibility.Visible;

                }
                else
                {
                    txt_addButton.Visibility = Visibility.Collapsed;
                    txt_add_Icon.Visibility = Visibility.Visible;

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private static uc_posTransfers _instance;
        public static uc_posTransfers Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                    _instance = new uc_posTransfers();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        bool UserControlLoaded = false;
        Pos posModel = new Pos();
        Branch branchModel = new Branch();
        IEnumerable<Pos> poss;
        IEnumerable<Branch> branches;
        CashTransfer cashtrans = new CashTransfer();
        CashTransfer cashModel = new CashTransfer();
        IEnumerable<CashTransfer> cashesQuery;
        IEnumerable<CashTransfer> cashesQueryExcel;
        IEnumerable<CashTransfer> cashes;
        string searchText = "";
        CashTransfer cashtrans2 = new CashTransfer();
        CashTransfer cashtrans3 = new CashTransfer();
        IEnumerable<CashTransfer> cashes2;
        string basicsPermission = "posTransfers_basics";
        string transAdminPermission = "posTransfers_transAdmin";

        public static List<string> requiredControlList;
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "cash", "fromBranch", "pos1", "toBranch" , "pos2" };
                
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

                dp_searchStartDate.SelectedDate = DateTime.Now.Date;
                dp_searchEndDate.SelectedDate = DateTime.Now.Date;

                dp_searchStartDate.SelectedDateChanged += this.dp_SelectedStartDateChanged;
                dp_searchEndDate.SelectedDateChanged += this.dp_SelectedEndDateChanged;

                try
                {
                    poss = await posModel.Get();
                }
                catch { }

                if (!FillCombo.groupObject.HasPermissionAction(transAdminPermission, FillCombo.groupObjects, "one"))
                {
                    cb_fromBranch.IsEnabled = false;////////////permissions
                    cb_toBranch.IsEnabled = false;/////////////permissions
                }

                #region fill branch combo1
                try
                {
                    branches = await branchModel.GetBranchesActive("b");
                    cb_fromBranch.ItemsSource = branches;
                    cb_fromBranch.DisplayMemberPath = "name";
                    cb_fromBranch.SelectedValuePath = "branchId";
                    cb_fromBranch.SelectedValue = MainWindow.branchLogin.branchId;
                }
                catch { }
                #endregion

                #region fill branch combo2
                try
                {
                    cb_toBranch.ItemsSource = branches;
                    cb_toBranch.DisplayMemberPath = "name";
                    cb_toBranch.SelectedValuePath = "branchId";
                }
                catch { }
                #endregion

                await RefreshCashesList();

                await Search();
                UserControlLoaded = true;
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
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trTransaferDetails");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            //txt_title.Text = AppSettings.resourcemanager.GetString("trTransfers");
            txt_Cash.Text = AppSettings.resourcemanager.GetString("trCash_") + " : ";

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_transNum, AppSettings.resourcemanager.GetString("trNo."));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_cash, AppSettings.resourcemanager.GetString("trCashHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_pos1, AppSettings.resourcemanager.GetString("trDepositor") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_pos2, AppSettings.resourcemanager.GetString("trRecepient") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_fromBranch, AppSettings.resourcemanager.GetString("trFromBranch")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toBranch, AppSettings.resourcemanager.GetString("trToBranch")+"...");

            chb_all.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_deposit.Content = AppSettings.resourcemanager.GetString("trDeposits");
            chk_receive.Content = AppSettings.resourcemanager.GetString("trReceipts");
            /*
           //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_state, MainWindow.resourcemanager.GetString("trStateHint"));
            */
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_searchStartDate, AppSettings.resourcemanager.GetString("trStartDate")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_searchEndDate, AppSettings.resourcemanager.GetString("trEndDate")+"...");

            dg_posAccounts.Columns[0].Header = AppSettings.resourcemanager.GetString("trTransferNumberTooltip");
            dg_posAccounts.Columns[1].Header = AppSettings.resourcemanager.GetString("trCreator");
            if (chk_deposit.IsChecked == true)
            {
                dg_posAccounts.Columns[2].Header = AppSettings.resourcemanager.GetString("trDepositor");
                dg_posAccounts.Columns[3].Header = AppSettings.resourcemanager.GetString("trRecepient");
            }
            else if (chk_receive.IsChecked == true)
            {
                dg_posAccounts.Columns[2].Header = AppSettings.resourcemanager.GetString("trRecepient");
                dg_posAccounts.Columns[3].Header = AppSettings.resourcemanager.GetString("trDepositor");
            }
            dg_posAccounts.Columns[5].Header = AppSettings.resourcemanager.GetString("trStatus");
            dg_posAccounts.Columns[4].Header = AppSettings.resourcemanager.GetString("trDate");
            dg_posAccounts.Columns[6].Header = AppSettings.resourcemanager.GetString("trCashTooltip");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            btn_confirm.Content = AppSettings.resourcemanager.GetString("trConfirm");
            btn_cancel.Content = AppSettings.resourcemanager.GetString("trCancel_");
            btn_add.Content = AppSettings.resourcemanager.GetString("trAdd");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") )
                {

                    HelpClass.StartAwait(grid_main);
                    if (MainWindow.posLogin.boxState == "o") // box is open
                    {
                        #region add
                        if (HelpClass.validate(requiredControlList, this))
                        {
                        Pos pos = await posModel.getById(Convert.ToInt32(cb_pos1.SelectedValue));
                        if (pos.balance < decimal.Parse(tb_cash.Text))
                        { Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn); }
                        else
                        {
                            //first operation
                            CashTransfer cash1 = new CashTransfer();

                            cash1.transType = "d";//deposit
                            cash1.transNum = await cashModel.generateCashNumber(cash1.transType + "p");
                            cash1.cash = decimal.Parse(tb_cash.Text);
                            cash1.createUserId = MainWindow.userLogin.userId;
                            cash1.notes = tb_notes.Text;
                            cash1.posIdCreator = MainWindow.posLogin.posId;
                            if (Convert.ToInt32(cb_pos1.SelectedValue) == MainWindow.posLogin.posId)
                                cash1.isConfirm = 1;
                            else cash1.isConfirm = 0;
                            cash1.side = "p";//pos
                            cash1.posId = Convert.ToInt32(cb_pos1.SelectedValue);

                            int s1 = await cashModel.Save(cash1);

                            if (!s1.Equals(0))
                            {
                                await MainWindow.refreshBalance();
                                //second operation
                                CashTransfer cash2 = new CashTransfer();

                                cash2.transType = "p";//pull
                                cash2.transNum = await cashModel.generateCashNumber(cash2.transType + "p");
                                cash2.cash = decimal.Parse(tb_cash.Text);
                                cash2.createUserId = MainWindow.userLogin.userId;
                                cash2.posIdCreator = MainWindow.posLogin.posId;
                                if (Convert.ToInt32(cb_pos2.SelectedValue) == MainWindow.posLogin.posId)
                                    cash2.isConfirm = 1;
                                else cash2.isConfirm = 0;
                                cash2.side = "p";//pos
                                cash2.posId = Convert.ToInt32(cb_pos2.SelectedValue);
                                cash2.cashTransIdSource = s1;//id from first operation

                                int s2 = await cashModel.Save(cash2);

                                if (!s2.Equals(0))
                                {
                                    #region notification Object
                                    int pos1 = 0;
                                    int pos2 = 0;
                                    if ((int)cb_pos1.SelectedValue != MainWindow.posLogin.posId)
                                        pos1 = (int)cb_pos1.SelectedValue;
                                    if ((int)cb_pos2.SelectedValue != MainWindow.posLogin.posId)
                                        pos2 = (int)cb_pos2.SelectedValue;
                                    Notification not = new Notification()
                                    {
                                        title = "trTransferAlertTilte",
                                        ncontent = "trTransferAlertContent",
                                        msgType = "alert",
                                        createUserId = MainWindow.userLogin.userId,
                                        updateUserId = MainWindow.userLogin.userId,
                                    };
                                    if (pos1 != 0)
                                        await not.save(not, (int)cb_pos1.SelectedValue, "accountsAlerts_transfers", cb_pos2.Text, 0, pos1);
                                    if (pos2 != 0)
                                        await not.save(not, (int)cb_pos2.SelectedValue, "accountsAlerts_transfers", cb_pos1.Text, 0, pos2);

                                    #endregion

                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                    Clear();

                                    await RefreshCashesList();
                                    await Search();
                                    await MainWindow.refreshBalance();
                                }
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            }
                        }
                    }
                        #endregion
                    }
                    else //box is closed
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBoxIsClosed"), animation: ToasterAnimation.FadeIn);
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                {
                    HelpClass.StartAwait(grid_main);

                    #region update
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        Pos pos = await posModel.getById(Convert.ToInt32(cb_pos1.SelectedValue));
                        if (pos.balance < decimal.Parse(tb_cash.Text))
                        { Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn); }
                        else
                        {
                            #region first operation
                            //first operation (pull)
                            cashtrans2.cash = decimal.Parse(tb_cash.Text);
                            cashtrans2.notes = tb_notes.Text;
                            cashtrans2.posId = Convert.ToInt32(cb_pos1.SelectedValue);

                            int s1 = await cashModel.Save(cashtrans2);
                            #endregion

                            if (!s1.Equals(0))
                            {
                                #region second operation
                                //second operation (deposit)
                                cashtrans3.cash = decimal.Parse(tb_cash.Text);
                                cashtrans3.posId = Convert.ToInt32(cb_pos2.SelectedValue);
                                cashtrans3.notes = tb_notes.Text;

                                int s2 = await cashModel.Save(cashtrans3);
                                #endregion

                                if (!s2.Equals(0))
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                                    await RefreshCashesList();
                                    await Search();
                                }
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            }
                        }
                    }
                    #endregion

                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") )
                {
                    HelpClass.StartAwait(grid_main);
                    if (cashtrans.cashTransId != 0)
                    {
                        #region
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                    // w.ShowInTaskbar = false;
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion
                        if (w.isOk)
                        {
                            int b = await cashModel.deletePosTrans(cashtrans.cashTransId);

                            if (b == 1)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);
                                //clear textBoxs
                                Btn_clear_Click(sender, e);
                            }
                            else if (b == 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopCanNotDeleteRequest"), animation: ToasterAnimation.FadeIn);
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                            await RefreshCashesList();
                            Tb_search_TextChanged(null, null);
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_confirm_Click(object sender, RoutedEventArgs e)
        {//confirm
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (MainWindow.posLogIn.boxState == "o")
                //{
                    if (cashtrans.cashTransId != 0)
                    {
                        if (cashtrans.isConfirm2 == 0)
                            await confirmOpr();
                        else
                        {
                            Pos pos = await posModel.getById(cashtrans.posId.Value);
                            Pos pos2 = await posModel.getById(cashtrans.pos2Id.Value);
                            int s1 = 0;
                            if (cashtrans.transType == "d")
                            {
                                //there is enough balance
                                if (pos.balance >= cashtrans.cash)
                                {
                                    pos.balance -= cashtrans.cash;
                                    int s = await posModel.save(pos);

                                    pos2.balance += cashtrans.cash;
                                    s1 = await posModel.save(pos2);
                                    if (!s1.Equals(0))//tras done so confirm
                                        await confirmOpr();
                                    else//error then do not confirm
                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                                }
                                //there is not enough balance
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);
                            }
                            else
                            {
                                //there is enough balance
                                if (pos2.balance >= cashtrans.cash)
                                {
                                    pos2.balance -= cashtrans.cash;
                                    int s = await posModel.save(pos2);

                                    pos.balance += cashtrans.cash;
                                    s1 = await posModel.save(pos);
                                    if (!s1.Equals(0))//tras done so confirm
                                        await confirmOpr();
                                    else//error then do not confirm
                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                                }

                                //there is not enough balance
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);
                            }
                            await MainWindow.refreshBalance();
                        }
                    }
                //}
                //else //box is closed
                //{
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBoxIsClosed"), animation: ToasterAnimation.FadeIn);
                //}

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task confirmOpr()
        {
            cashtrans.isConfirm = 1;
            int s = await cashModel.Save(cashtrans);
            if (!s.Equals(0))
            {
                await RefreshCashesList();
                Tb_search_TextChanged(null, null);

                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopConfirm"), animation: ToasterAnimation.FadeIn);

                btn_confirm.Content = AppSettings.resourcemanager.GetString("trIsConfirmed");
                btn_confirm.IsEnabled = false;
                btn_cancel.IsEnabled = false;
            }
        }
        #endregion
        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void dp_SelectedEndDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);
                await RefreshCashesList();
                Tb_search_TextChanged(null, null);
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void dp_SelectedStartDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                HelpClass.StartAwait(grid_main);
                await RefreshCashesList();
                Tb_search_TextChanged(null, null);
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dg_posAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//selection
            try
            {
                HelpClass.StartAwait(grid_main);

                if (dg_posAccounts.SelectedIndex != -1)
                {
                    cashtrans = dg_posAccounts.SelectedItem as CashTransfer;
                    this.DataContext = cashtrans;
                    if (cashtrans != null)
                    {
                        //login pos is operation pos
                        if (cashtrans.posId == MainWindow.posLogin.posId)
                        {
                            if (cashtrans.isConfirm == 0)
                            {
                                btn_confirm.Content = AppSettings.resourcemanager.GetString("trConfirm");
                                btn_confirm.IsEnabled = true;
                                btn_cancel.Content = AppSettings.resourcemanager.GetString("trCancel_");
                                btn_cancel.IsEnabled = true;
                            }
                            else if (cashtrans.isConfirm == 1)
                            {
                                btn_confirm.Content = AppSettings.resourcemanager.GetString("trIsConfirmed");
                                btn_confirm.IsEnabled = false;
                                btn_cancel.Content = AppSettings.resourcemanager.GetString("trCancel_");
                                btn_cancel.IsEnabled = false;
                            }
                            else if (cashtrans.isConfirm == 2)
                            {
                                btn_confirm.Content = AppSettings.resourcemanager.GetString("trConfirm");
                                btn_confirm.IsEnabled = false;
                                btn_cancel.Content = AppSettings.resourcemanager.GetString("trCanceled");
                                btn_cancel.IsEnabled = false;
                            }
                        }
                        else
                        {
                            btn_confirm.IsEnabled = false;
                            btn_cancel.IsEnabled = false;
                            if (cashtrans.isConfirm == 0)
                            {
                                btn_confirm.Content = AppSettings.resourcemanager.GetString("trConfirm");
                                btn_cancel.Content = AppSettings.resourcemanager.GetString("trCancel_");
                            }
                            else if (cashtrans.isConfirm == 1)
                            {
                                btn_confirm.Content = AppSettings.resourcemanager.GetString("trIsConfirmed");
                                btn_cancel.Content = AppSettings.resourcemanager.GetString("trCancel_");
                                btn_cancel.IsEnabled = false;
                            }
                            else if (cashtrans.isConfirm == 2)
                            {
                                btn_confirm.Content = AppSettings.resourcemanager.GetString("trConfirm");
                                btn_confirm.IsEnabled = false;
                                btn_cancel.Content = AppSettings.resourcemanager.GetString("trCanceled");
                            }
                        }

                        #region get two pos
                        cashes2 = await cashModel.GetbySourcId("p", cashtrans.cashTransId);
                        //to insure that the pull operation is in cashtrans2 
                        if (cashtrans.transType == "p")
                        {
                            cashtrans2 = cashes2.ToList()[0] as CashTransfer;
                            cashtrans3 = cashes2.ToList()[1] as CashTransfer;
                        }
                        else if (cashtrans.transType == "d")
                        {
                            cashtrans2 = cashes2.ToList()[1] as CashTransfer;
                            cashtrans3 = cashes2.ToList()[0] as CashTransfer;
                        }

                        cb_fromBranch.SelectedValue = (await posModel.getById(cashtrans3.posId.Value)).branchId;
                        cb_pos1.SelectedValue = cashtrans3.posId;

                        cb_toBranch.SelectedValue = (await posModel.getById(cashtrans2.posId.Value)).branchId;
                        Cb_toBranch_SelectionChanged(cb_toBranch, null);
                        cb_pos2.SelectedValue = cashtrans2.posId;

                        #endregion
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
            HelpClass.EndAwait(grid_main);
        }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
        }
    }

        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                tb_search.Text = "";
                searchText = "";
                await RefreshCashesList();
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Refresh & Search
        async Task Search()
        {//search
            if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "show") )
            {
                try
                {
                    if (cashes is null)
                        await RefreshCashesList();

                    searchText = tb_search.Text;
                    if (chb_all.IsChecked == false)
                    {
                        if (chk_deposit.IsChecked == true)
                        {
                            cashesQuery = cashes.Where(s => (s.transNum.Contains(searchText)
                                || s.transType.Contains(searchText)
                                || s.cash.ToString().Contains(searchText)
                                || s.posName.Contains(searchText)
                                )
                            && s.updateDate.Value.Date <= dp_searchEndDate.SelectedDate.Value.Date
                            && s.updateDate.Value.Date >= dp_searchStartDate.SelectedDate.Value.Date
                            && s.transType == "d"
                            );
                        }
                        else if (chk_receive.IsChecked == true)
                        {
                            cashesQuery = cashes.Where(s => (s.transNum.Contains(searchText)
                            || s.transType.Contains(searchText)
                            || s.cash.ToString().Contains(searchText)
                            || s.posName.Contains(searchText)
                            )
                            && s.updateDate.Value.Date <= dp_searchEndDate.SelectedDate.Value.Date
                            && s.updateDate.Value.Date >= dp_searchStartDate.SelectedDate.Value.Date
                            //&& s.posId == MainWindow.posID.Value
                            && s.transType == "p"
                            );
                        }
                    }
                    else
                    {
                        if (chk_deposit.IsChecked == true)
                        {
                            cashesQuery = cashes.Where(s => (s.transNum.Contains(searchText)
                                || s.transType.Contains(searchText)
                                || s.cash.ToString().Contains(searchText)
                                || s.posName.Contains(searchText)
                                )
                            && s.transType == "d"
                            );
                        }
                        else if (chk_receive.IsChecked == true)
                        {
                            cashesQuery = cashes.Where(s => (s.transNum.Contains(searchText)
                            || s.transType.Contains(searchText)
                            || s.cash.ToString().Contains(searchText)
                            || s.posName.Contains(searchText)
                            )
                            && s.transType == "p"
                            );
                        }
                    }

                    RefreshCashView();
                    cashesQueryExcel = cashesQuery.ToList();
                    txt_count.Text = cashesQuery.Count().ToString();
                }
                catch { }
            }
        }
        async Task<IEnumerable<CashTransfer>> RefreshCashesList()
        {
            cashes = await cashModel.GetCashTransferForPosById("all", "p", (int)MainWindow.posLogin.posId);
            return cashes;

        }
        void RefreshCashView()
        {
            dg_posAccounts.ItemsSource = cashesQuery;
            txt_count.Text = cashesQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Bank();

            cb_fromBranch.SelectedIndex = -1;
            cb_pos1.SelectedIndex = -1;
            cb_toBranch.SelectedIndex = -1;
            cb_pos2.SelectedIndex = -1;

            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
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
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only english and digits
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
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

        #endregion
      
        private void Cb_pos1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//pos1selection
            try
            {
                HelpClass.StartAwait(grid_main);

                int bToId = Convert.ToInt32(cb_toBranch.SelectedValue);
                int pFromId = Convert.ToInt32(cb_pos1.SelectedValue);
                var toPos = poss.Where(p => p.branchId == bToId && p.posId != pFromId);
                cb_pos2.ItemsSource = toPos;
                cb_pos2.DisplayMemberPath = "name";
                cb_pos2.SelectedValuePath = "posId";
                cb_pos2.SelectedIndex = -1;
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_pos2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Cb_fromBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//fill pos1
            try
            {
                HelpClass.StartAwait(grid_main);

                int bFromId = Convert.ToInt32(cb_fromBranch.SelectedValue);
                var fromPos = poss.Where(p => p.branchId == bFromId);
                cb_pos1.ItemsSource = fromPos;
                cb_pos1.DisplayMemberPath = "name";
                cb_pos1.SelectedValuePath = "posId";
                //cb_pos1.SelectedIndex = -1;
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_toBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { //fill pos combo2
            try
            {
                HelpClass.StartAwait(grid_main);

                int bToId = Convert.ToInt32(cb_toBranch.SelectedValue);
                int pFromId = Convert.ToInt32(cb_pos1.SelectedValue);
                var toPos = poss.Where(p => p.branchId == bToId && p.posId != pFromId);
                cb_pos2.ItemsSource = toPos;
                cb_pos2.DisplayMemberPath = "name";
                cb_pos2.SelectedValuePath = "posId";
                //cb_pos2.SelectedIndex = -1;
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Chb_all_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                dp_searchStartDate.IsEnabled =
            dp_searchEndDate.IsEnabled = false;

                Btn_refresh_Click(btn_refresh, null);

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Chb_all_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                dp_searchStartDate.IsEnabled =
                dp_searchEndDate.IsEnabled = true;

                Btn_refresh_Click(btn_refresh, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void search_Checking(object sender, RoutedEventArgs e)
        {
            try
            {
                if(UserControlLoaded)
                { 
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_deposit")
                    {
                        chk_receive.IsChecked = false;
                    }
                    else if (cb.Name == "chk_receive")
                    {
                        chk_deposit.IsChecked = false;
                    }
                }
                HelpClass.StartAwait(grid_main);

                translate();
                Clear();
                await RefreshCashesList();
                Tb_search_TextChanged(null, null);

                HelpClass.EndAwait(grid_main);
                }
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void chk_uncheck(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_deposit")
                        chk_deposit.IsChecked = true;
                    else if (cb.Name == "chk_receive")
                        chk_receive.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        //private async void search_Checking(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        CheckBox cb = sender as CheckBox;
        //        if (cb.IsFocused)
        //        {
        //            if (cb.Name == "chk_unConfirmed")
        //            {
        //                chk_waiting.IsChecked = false;
        //                chk_confirmed.IsChecked = false;
        //                chk_createdOper.IsChecked = false;
        //            }
        //            else if (cb.Name == "chk_waiting")
        //            {
        //                chk_unConfirmed.IsChecked = false;
        //                chk_confirmed.IsChecked = false;
        //                chk_createdOper.IsChecked = false;
        //            }
        //            else if (cb.Name == "chk_confirmed")
        //            {
        //                chk_unConfirmed.IsChecked = false;
        //                chk_waiting.IsChecked = false;
        //                chk_createdOper.IsChecked = false;
        //            }
        //            else if(cb.Name == "chk_createdOper")
        //            {
        //                chk_unConfirmed.IsChecked = false;
        //                chk_confirmed.IsChecked = false;
        //                chk_waiting.IsChecked = false;
        //            }
        //        }
        //        HelpClass.StartAwait(grid_main);

        //        await RefreshCashesList();
        //        await Search();
        //        Clear();

        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {

        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        //private void chk_uncheck(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        CheckBox cb = sender as CheckBox;
        //        if (cb.IsFocused)
        //        {
        //            if (cb.Name == "chk_unConfirmed")
        //                chk_unConfirmed.IsChecked = true;
        //            else if (cb.Name == "chk_confirmed")
        //                chk_confirmed.IsChecked = true;
        //            else if (cb.Name == "chk_waiting")
        //                chk_waiting.IsChecked = true;
        //            else if (cb.Name == "chk_createdOper")
        //                chk_createdOper.IsChecked = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        #region reports

        string reportsPermission = "";
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Account\report\Ar\ArPosAcc.rdlc";
            }
            else
            {
                addpath = @"\Reports\Account\report\En\EnPosAcc.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            clsReports.posAccReport(cashesQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);
            string title = AppSettings.resourcemanagerreport.GetString("trTransfers");



            if (chk_deposit.IsChecked == true)
            {
                title = title + "/" + AppSettings.resourcemanagerreport.GetString("trDeposits");

                paramarr.Add(new ReportParameter("trCol2Header", AppSettings.resourcemanagerreport.GetString("trDepositor")));
                paramarr.Add(new ReportParameter("trCol3Header", AppSettings.resourcemanagerreport.GetString("trRecepient")));
            }
            else if (chk_receive.IsChecked == true)
            {

                title = title + "/" + AppSettings.resourcemanagerreport.GetString("trReceives");
                paramarr.Add(new ReportParameter("trCol2Header", AppSettings.resourcemanagerreport.GetString("trRecepient")));
                paramarr.Add(new ReportParameter("trCol3Header", AppSettings.resourcemanagerreport.GetString("trDepositor")));




            }
            paramarr.Add(new ReportParameter("trTitle", title));
            rep.SetParameters(paramarr);

            rep.Refresh();
        }
    

        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);
                /////////////////////
                if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    Window.GetWindow(this).Opacity = 0.2;
                    win_IvcAccount win = new win_IvcAccount(cashesQuery, 1);
                    // // w.ShowInTaskbar = false;
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                /////////////////////

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_pdf_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_preview_Click_1(object sender, RoutedEventArgs e)
        {
            //preview
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;

                    string pdfpath = "";
                    //
                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    BuildReport();

                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    wd_previewPdf w = new wd_previewPdf();
                    w.pdfPath = pdfpath;
                    if (!string.IsNullOrEmpty(w.pdfPath))
                    {
                        // w.ShowInTaskbar = false;
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();
                    }
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_print_Click_1(object sender, RoutedEventArgs e)
        {
            //print
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_exportToExcel_Click_1(object sender, RoutedEventArgs e)
        {
            //print
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        private async void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {//cancel
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (MainWindow.posLogIn.boxState == "o")
                //{
                    if (cashtrans.cashTransId != 0)
                    {
                        cashtrans2.isConfirm = 2;
                        cashtrans3.isConfirm = 2;

                        int s2 = await cashModel.Save(cashtrans2);
                        int s3 = await cashModel.Save(cashtrans3);

                        if ((!s2.Equals(0)) && (!s3.Equals(0)))
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopCanceled"), animation: ToasterAnimation.FadeIn);
                            await RefreshCashesList();
                            Tb_search_TextChanged(null, null);
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                //}
                //else //box is closed
                //{
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBoxIsClosed"), animation: ToasterAnimation.FadeIn);
                //}
                
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
