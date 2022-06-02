using netoaster;
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

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_transfers.xaml
    /// </summary>
    public partial class wd_transfers : Window
    {
         public wd_transfers()
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
        CashTransfer cashModel = new CashTransfer();
        IEnumerable<CashTransfer> cashesQuery;
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.Key == Key.Return)
                //{
                //    Btn_select_Click(null, null);
                //}
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
                translat();
                #endregion

                await fillDataGrid();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void translat()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trCashtransfers");
            dg_transfers.Columns[0].Header = AppSettings.resourcemanager.GetString("trTransferNumberTooltip");
            dg_transfers.Columns[1].Header = AppSettings.resourcemanager.GetString("trDepositor");
            dg_transfers.Columns[2].Header = AppSettings.resourcemanager.GetString("trRecepient");
            dg_transfers.Columns[3].Header = AppSettings.resourcemanager.GetString("trCashTooltip");

        }

        async Task fillDataGrid()
        {
            cashesQuery = await cashModel.GetCashTransferForPosById("all", "p", (int)MainWindow.posLogin.posId); 
            cashesQuery = cashesQuery.Where(c => c.posId == MainWindow.posLogin.posId && c.isConfirm == 0 );

            foreach (var c in cashesQuery)
            {
                if (c.transType.Equals("p"))
                {
                    string s = c.posName;
                    c.posName = c.pos2Name;
                    c.pos2Name = s;
                }
            }

            dg_transfers.ItemsSource = cashesQuery;
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        #region Button In DataGrid

        CashTransfer cashtrans2 = new CashTransfer();
        CashTransfer cashtrans3 = new CashTransfer();
        IEnumerable<CashTransfer> cashes2;

        async void confirmRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                        Pos posModel = new Pos();
                        CashTransfer row = (CashTransfer) dg_transfers.SelectedItems[0];
                        #region old
                        //#region get two pos
                        //cashes2 = await cashModel.GetbySourcId("p", row.cashTransId);
                        ////to insure that the pull operation is in cashtrans2 
                        //if (row.transType == "p")
                        //{
                        //    cashtrans2 = cashes2.ToList()[0] as CashTransfer;
                        //    cashtrans3 = cashes2.ToList()[1] as CashTransfer;
                        //}
                        //else if (row.transType == "d")
                        //{
                        //    cashtrans2 = cashes2.ToList()[1] as CashTransfer;
                        //    cashtrans3 = cashes2.ToList()[0] as CashTransfer;
                        //}

                        //#endregion

                        //#region confirm

                        ////if another operation not confirmed then just confirm this
                        //////if another operation is confirmed then chk balance before confirm
                        //bool confirm = false;
                        //if (cashtrans2.cashTransId == row.cashTransId)//chk which record is selected
                        //{ if (cashtrans3.isConfirm == 0) confirm = false; else confirm = true; }
                        //else//chk which record is selected
                        //{ if (cashtrans2.isConfirm == 0) confirm = false; else confirm = true; }

                        //if (!confirm) await confirmOpr(row);
                        //else
                        //{
                        //    Pos posModel = new Pos();
                        //    Pos pos = await posModel.getById(cashtrans2.posId.Value);
                        //    //there is enough balance
                        //    if (pos.balance >= cashtrans2.cash)
                        //    {
                        //        cashtrans2.isConfirm = 1;
                        //        int s = await cashModel.Save(cashtrans2);
                        //        s = await cashModel.MovePosCash(cashtrans2.cashTransId, MainWindow.userID.Value);
                        //        //   if (s.Equals("transdone"))//tras done so confirm
                        //        if (s.Equals(1))//tras done so confirm
                        //            await confirmOpr(row);
                        //        else//error then do not confirm
                        //            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                        //    }
                        //    //there is not enough balance
                        //    else
                        //        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);
                        //}
                        //await MainWindow.refreshBalance();
                        //#endregion
                        #endregion

                        if (row.isConfirm2 == 0)
                            await confirmOpr(row);
                        else
                        {
                            Pos pos = await posModel.getById(row.posId.Value);
                            Pos pos2 = await posModel.getById(row.pos2Id.Value);
                            int s1 = 0;
                            if (row.transType == "d")
                            {
                                //there is enough balance
                                if (pos.balance >= row.cash)
                                {
                                    pos.balance -= row.cash;
                                    int s = await posModel.save(pos);

                                    pos2.balance += row.cash;
                                    s1 = await posModel.save(pos2);
                                    if (!s1.Equals(0))//tras done so confirm
                                        await confirmOpr(row);
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
                                if (pos2.balance >= row.cash)
                                {
                                    pos2.balance -= row.cash;
                                    int s = await posModel.save(pos2);

                                    pos.balance += row.cash;
                                    s1 = await posModel.save(pos);
                                    if (!s1.Equals(0))//tras done so confirm
                                        await confirmOpr(row);
                                    else//error then do not confirm
                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                                }

                                //there is not enough balance
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);
                            }
                            //await MainWindow.refreshBalance();////////////////??????????????????????????
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

        private async Task confirmOpr(CashTransfer cashtrans)
        {
            cashtrans.isConfirm = 1;
            int s = await cashModel.Save(cashtrans);
            if (!s.Equals(0))
            {
                await fillDataGrid();
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopConfirm"), animation: ToasterAnimation.FadeIn);
            }
        }
        
        async void cancelRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        CashTransfer row = (CashTransfer)dg_transfers.SelectedItems[0];

                        #region get two pos
                        cashes2 = await cashModel.GetbySourcId("p", row.cashTransId);
                        //to insure that the pull operation is in cashtrans2 
                        if (row.transType == "p")
                        {
                            cashtrans2 = cashes2.ToList()[0] as CashTransfer;
                            cashtrans3 = cashes2.ToList()[1] as CashTransfer;
                        }
                        else if (row.transType == "d")
                        {
                            cashtrans2 = cashes2.ToList()[1] as CashTransfer;
                            cashtrans3 = cashes2.ToList()[0] as CashTransfer;
                        }

                        #endregion

                        #region cancel
                        cashtrans2.isConfirm = 2;
                        cashtrans3.isConfirm = 2;

                        int s2 = await cashModel.Save(cashtrans2);
                        int s3 = await cashModel.Save(cashtrans3);

                        if ((!s2.Equals(0)) && (!s3.Equals(0)))
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopCanceled"), animation: ToasterAnimation.FadeIn);
                            await fillDataGrid();
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        #endregion
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
    }
}
