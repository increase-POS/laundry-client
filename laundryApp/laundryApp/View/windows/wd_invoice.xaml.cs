using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for wd_invoice.xaml
    /// </summary>
    public partial class wd_invoice : Window
    {
        public wd_invoice()
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
        /// <summary>
        /// for filtering store
        /// </summary>
        public Invoice invoice = new Invoice();
        IEnumerable<Invoice> invoices;
        public int posId { get; set; }
        public int branchId { get; set; }
        public int branchCreatorId { get; set; }
        public int userId { get; set; }
        /// <summary>
        /// for filtering invoice type
        /// </summary>
        /// 
       public string icon { get; set; }
       public string page { get; set; }
        public string invoiceType { get; set; }
        public int duration { get; set; }
        public int hours { get; set; }

        List<string> invTypeL;

        public string invoiceStatus { get; set; }
        public string title { get; set; }
        public string condition { get; set; }
        public bool fromOrder = false;
        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                invoice = dg_Invoice.SelectedItem as Invoice;
                DialogResult = true;
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
                    Btn_select_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            { 
                dg_Invoice.ItemsSource = FillCombo.invoices.Where(x => x.invNumber.ToLower().Contains(txb_search.Text)).ToList();
                txt_count.Text = dg_Invoice.Items.Count.ToString() ;

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_ucInvoice);

                invTypeL = invoiceType.Split(',').ToList();

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_ucInvoice.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_ucInvoice.FlowDirection = FlowDirection.RightToLeft;
                }
                txt_Invoices.Text = title;
                translat();
                #endregion
                dg_Invoice.Columns[0].Visibility = Visibility.Collapsed;

                hidDisplayColumns();
                await refreshInvoices();
                Txb_search_TextChanged(null, null);

                
                    HelpClass.EndAwait(grid_ucInvoice);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_ucInvoice);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void translat()
        {
            //txt_Invoices.Text = AppSettings.resourcemanager.GetString("trInvoices");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            col_num.Header = AppSettings.resourcemanager.GetString("trCharp");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_user.Header = AppSettings.resourcemanager.GetString("trUser");
            col_count.Header = AppSettings.resourcemanager.GetString("trCount");
            col_total.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_type.Header = AppSettings.resourcemanager.GetString("trType");

            #region translate agent column
            string[] invTypeArray = new string[] { "tsd", "ssd" };
            var invTypes = invTypeArray.ToList();
            var inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_agent.Header = AppSettings.resourcemanager.GetString("trCustomer");
            else
                col_agent.Header = AppSettings.resourcemanager.GetString("trVendor");

            #endregion

            txt_countTitle.Text = AppSettings.resourcemanager.GetString("trCount") + ":";

            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");

            if (page == "storageMov" && icon == "orders") // import
                col_branch.Header = AppSettings.resourcemanager.GetString("trToBranch");
            else if (page == "storageMov" && icon == "ordersWait") // export
                col_branch.Header = AppSettings.resourcemanager.GetString("trFromBranch");
        }

        private void hidDisplayColumns()
        {
            #region hide Total column in grid if invoice is import/export order/purchase order/ spending request order/Food Beverages Consumption

            string[] invTypeArray = new string[] { "imd", "exd", "im", "ex", "exw", "pod", "po", "srd", "sr", "srw", "src", "fbc" };
            var invTypes = invTypeArray.ToList();           
            var inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_total.Visibility = Visibility.Collapsed; //make total column unvisible
            #endregion

            #region display branch & user columns in grid if invoice is sales order and purchase orders
            invTypeArray = new string[] { "or" };
            invTypes = invTypeArray.ToList();
            invTypeL = invoiceType.Split(',').ToList();
            inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
            {
                col_agent.Header = AppSettings.resourcemanager.GetString("trCustomer");
                col_agent.Visibility = Visibility.Visible;
                if (fromOrder == false)
                {
                    col_branch.Visibility = Visibility.Visible; //make branch column visible
                    col_user.Visibility = Visibility.Visible; //make user column visible
                }
                //dg_Invoice.Columns[7].Visibility = Visibility.Visible; //make user column visible
            }
            #endregion

            #region display branch, vendor & user columns in grid if invoice is  purchase orders
            if (invoiceType == "po" && fromOrder == false)
            {
                col_agent.Header = AppSettings.resourcemanager.GetString("trVendor");
                col_branch.Visibility = Visibility.Visible; //make branch column visible
                col_user.Visibility = Visibility.Visible; //make user column visible
                col_agent.Visibility = Visibility.Visible;
            }
            #endregion

            #region display branch if invoice is export or import process
            invTypeArray = new string[] { "exw", "im", "ex" };
            invTypes = invTypeArray.ToList();
            inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_branch.Visibility = Visibility.Visible; //make branch column unvisible
            #endregion

            #region display customer if invoice is take away or self-service
            invTypeArray = new string[] { "tsd", "ssd" };
            invTypes = invTypeArray.ToList();
            inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_agent.Visibility = Visibility.Visible; //make branch column unvisible
            #endregion
        }
        private async Task refreshInvoices()
        {
            #region purchase invoice
            if (page == "purchases")
            {
                if (icon == "invoices" )
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
                else if (icon == "orders")
                    FillCombo.invoices = await FillCombo.invoice.getUnHandeldOrders(invoiceType, branchCreatorId, branchId, duration, userId);
                else if (icon == "drafts")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
            }
            #endregion
            #region purchase order
            else if (page == "purchaseOrders")
            {
                if (icon == "invoices" )
                    FillCombo.invoices = await FillCombo.invoice.getUnHandeldOrders(invoiceType, branchCreatorId, branchId, duration, userId);

                else if (icon == "drafts")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
            }
            #endregion
            #region storage invoice
            else if (page == "storageInv")
            {
                if (icon == "purInvoices" || icon == "retInvoice" )
                    FillCombo.invoices = await FillCombo.invoice.getBranchInvoices(invoiceType, 0, branchId);

                else if (icon == "drafts")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
            }
            #endregion
            #region storage movements
            else if (page == "storageMov")
            {
                if (icon == "drafts")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
                else if(icon == "orders" )
                    FillCombo.invoices = await FillCombo.invoice.getExportImportInvoices(invoiceType, branchId);
                else if(icon == "ordersWait")
                    FillCombo.invoices =await FillCombo.invoice.getExportInvoices(invoiceType, branchId);
            }
            #endregion
            #region spending Request
            else if (page == "spendingRequest")
            {
                if (icon == "drafts")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
                if (icon == "spendingRequest")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
            }
            #endregion
            #region spending order in storage
            else if (page == "spendingOrder")
            {
                if (icon == "waitingOrders")
                    FillCombo.invoices = await FillCombo.invoice.getBranchInvoices(invoiceType, branchCreatorId, branchId);
                else if (icon == "draftOrders")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration);
            }
            #endregion
            #region consumption in kitchen
            else if (page == "consumption")
            {
                if (icon == "invoices")
                    FillCombo.invoices = await FillCombo.invoice.getBranchInvoices(invoiceType, branchCreatorId, branchId,duration);
            }
            #endregion
            #region take away
            else if (page == "takeAway")
            {
                if (icon == "drafts")
                    FillCombo.invoices = await FillCombo.invoice.GetInvoicesByCreator(invoiceType, userId, duration,hours);
            }
            #endregion
            //if (condition == "orders")
            //{
            //    invoices = await invoice.getUnHandeldOrders(invoiceType,branchCreatorId, branchId,duration,userId);
            //}
            //else if(condition == "return")
            //    invoices = await invoice.getInvoicesToReturn(invoiceType, userId);
            //else if(condition == "admin")
            //    invoices = await invoice.GetInvoicesForAdmin(invoiceType, duration);
            //else
            //{
            //    if (userId != 0 && (invoiceStatus == "" || invoiceStatus == null))/// to display draft invoices
            //        invoices = await invoice.GetInvoicesByCreator(invoiceType, userId, duration);
            //    else if (branchId != 0 && branchCreatorId != 0) // to get invoices to make return from it
            //        invoices = await invoice.getBranchInvoices(invoiceType, branchCreatorId, branchId);
            //    else if (branchCreatorId != 0)
            //        invoices = await invoice.getBranchInvoices(invoiceType, branchCreatorId);
            //    else if (invoiceStatus != "" && branchId != 0) // get return invoice in storage
            //        invoices = await invoice.getBranchInvoices(invoiceType, branchCreatorId, branchId);
            //    else if (branchId != 0) // get export/ import orders
            //        invoices = await invoice.GetOrderByType(invoiceType, branchId);
            //    else if (invoiceStatus != "" && userId != 0) // get sales invoices to get deliver accept on it
            //        invoices = await invoice.getDeliverOrders(invoiceType, invoiceStatus, userId);
            //    else
            //        invoices = await invoice.GetInvoicesByType(invoiceType, branchId);
            //}


        }
        private void Dg_Invoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                invoice = dg_Invoice.SelectedItem as Invoice;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Dg_Invoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            { 
                Btn_select_Click(null,null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
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
        private async void deleteRowFromInvoiceItems(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    
            //        HelpClass.StartAwait(grid_ucInvoice);
            //    for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            //        if (vis is DataGridRow)
            //        {
            //            #region
            //            Window.GetWindow(this).Opacity = 0.2;
            //            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
            //            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
            //            w.ShowDialog();
            //            Window.GetWindow(this).Opacity = 1;
            //            #endregion
            //            if (w.isOk)
            //            {
            //                Invoice row = (Invoice)dg_Invoice.SelectedItems[0];
            //                int res = 0;
            //                if (row.invType == "or")
            //                    res = await invoice.deleteOrder(row.invoiceId);
            //                else
            //                    res = await invoice.deleteInvoice(row.invoiceId);
            //                if (res > 0)
            //                {
            //                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);
            //                    await refreshInvoices();
            //                    Txb_search_TextChanged(null,null);
            //                }
            //                else
            //                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            //            }
            //        }

            //    
            //        HelpClass.EndAwait(grid_ucInvoice);
            //}
            //catch (Exception ex)
            //{
            //    
            //        HelpClass.EndAwait(grid_ucInvoice);
            //    HelpClass.ExceptionMessage(ex, this);
            //}
        }
    }
}
