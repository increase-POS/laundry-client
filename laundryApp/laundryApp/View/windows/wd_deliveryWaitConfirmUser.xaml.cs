using netoaster;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
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
    /// Interaction logic for wd_deliveryWaitConfirmUser.xaml
    /// </summary>
    public partial class wd_deliveryWaitConfirmUser : Window
    {
        public wd_deliveryWaitConfirmUser()
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
            txt_title.Text = AppSettings.resourcemanager.GetString("trWaitConfirmUser");

            #region data grid
            col_invNum.Header = AppSettings.resourcemanager.GetString("trCharp");
            col_customer.Header = AppSettings.resourcemanager.GetString("trCustomer");
            col_itemsCount.Header = AppSettings.resourcemanager.GetString("trQTR");
            #endregion
        }

        public Invoice invoice = new Invoice();
        IEnumerable<Invoice> invoices;
        async Task fillDataGrid() 
        {
            invoices = await invoice.getUserDeliverOrders( MainWindow.userLogin.userId);

           dg_invoice.ItemsSource = invoices;
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

        async void confirmRowinDatagrid(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
              
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {
                        #region
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;
                        #endregion
                        if (w.isOk)
                        {
                            // Pos posModel = new Pos();
                            invoice = (Invoice)dg_invoice.SelectedItems[0];
                            await saveOrderStatus();
                            await fillDataGrid();
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
        private async Task saveOrderStatus()
        {
            OrderPreparing orderModel = new OrderPreparing();

            orderPreparingStatus st = new orderPreparingStatus();
            st.status = "Collected";
            st.createUserId = MainWindow.userLogin.userId;
            st.isActive = 1;

            int res = await orderModel.EditInvoiceOrdersStatus(invoice.invoiceId, invoice.shipUserId, (int)invoice.shippingCompanyId, st);
            if (res > 0)
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopConfirm"), animation: ToasterAnimation.FadeIn);
            else
                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        }
        #endregion
    }
}
