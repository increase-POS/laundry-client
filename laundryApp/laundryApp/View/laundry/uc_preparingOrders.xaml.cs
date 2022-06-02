using netoaster;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace laundryApp.View.kitchen
{
    /// <summary>
    /// Interaction logic for uc_preparingOrders.xaml
    /// </summary>
    public partial class uc_preparingOrders : UserControl
    {
        public uc_preparingOrders()
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
        private static uc_preparingOrders _instance;
        public static uc_preparingOrders Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_preparingOrders();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string updatePermission = "preparingOrders_update";
        OrderPreparing preparingOrder = new OrderPreparing();
        List<OrderPreparing> orders = new List<OrderPreparing>();
        List<OrderPreparing> ordersQuery = new List<OrderPreparing>();

        string searchText = "";
        public static DispatcherTimer timer;



        public static List<string> requiredControlList;
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (timer != null)
                timer.Stop();
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "preparingTime" };

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_orders", value = false });
                loadingList.Add(new keyValueBool { key = "loading_salesItems", value = false });

                loading_orders();
                loading_salesItems();
                do
                {
                    isDone = true;
                    foreach (var item in loadingList)
                    {
                        if (item.value == false)
                        {
                            isDone = false;
                            break;
                        }
                    }
                    if (!isDone)
                    {
                        await Task.Delay(0500);
                    }
                }
                while (!isDone);
                #endregion
               
                FillCombo.FillInvoiceTypeWithDefault(cb_searchInvType);
                //FillCombo.FillPreparingOrderStatusWithDefault(cb_searchStatus);

                await Search();
                #region 
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(5);
                timer.Tick += timer_Tick;
                timer.Start();
                #endregion
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
            #region dg_orders
            col_orderNum.Header = AppSettings.resourcemanager.GetString("trOrderCharp");
            dg_orders.Columns[2].Header = AppSettings.resourcemanager.GetString("trInvoiceCharp");
            dg_orders.Columns[3].Header = AppSettings.resourcemanager.GetString("trRemainingTime");
            dg_orders.Columns[4].Header = AppSettings.resourcemanager.GetString("trStatus");
            col_orderType.Header = AppSettings.resourcemanager.GetString("trType");
            col_table.Header = AppSettings.resourcemanager.GetString("trTable");
            #endregion

            //txt_title.Text = AppSettings.resourcemanager.GetString("trOrder");
            txt_details.Text = AppSettings.resourcemanager.GetString("trDetails");
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_minute.Text = AppSettings.resourcemanager.GetString("trMinute");
            txt_tablesTitle.Text = AppSettings.resourcemanager.GetString("trTables");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_preparingTime, AppSettings.resourcemanager.GetString("trPreparingTime") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_searchStatus, AppSettings.resourcemanager.GetString("trStatusHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_searchCatalog, AppSettings.resourcemanager.GetString("trCategorie") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_searchInvType, AppSettings.resourcemanager.GetString("typesOfService") + "...");

            chk_allForDelivery.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_listed.Content = AppSettings.resourcemanager.GetString("trListed");
            chk_preparing.Content = AppSettings.resourcemanager.GetString("trPreparing");
            chk_ready.Content = AppSettings.resourcemanager.GetString("trReady");

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
            //btn_updatePreparingTime.Content = AppSettings.resourcemanager.GetString("trPreparingTime");
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.S:
                        //handle S key
                        Btn_save_Click(btn_save, null);
                        break;

                }
            }
        }
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (dg_orders.Items.Count > 0)
                {
                    //var firstCol = dg_orders.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                    //if (firstCol != null || dg_orders?.Items != null)
                    //    foreach (var item in dg_orders.Items)
                    //    {
                    //        var chBx = firstCol.GetCellContent(item) as CheckBox;
                    //        if (chBx == null)
                    //        {
                    //            continue;
                    //        }
                    //        chBx.IsChecked = false;
                    //    }

                    dg_orders.Items.Refresh();
                    //var firstCol = dg_orders.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                    //if (firstCol != null || dg_orders?.Items != null)
                    //    foreach (var item in dg_orders.Items)
                    //    {
                    //        OrderPreparing orderPreparing = item as OrderPreparing;
                    //        if (selectedOrders.Where(x => x.orderPreparingId == orderPreparing.orderPreparingId).Count() > 0)
                    //        {
                    //            var chBx = firstCol.GetCellContent(item) as CheckBox;
                    //            chBx.IsChecked = true;
                    //        }
                    //    }
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #region loading
        List<keyValueBool> loadingList;
        async void loading_orders()
        {
            //get orders
            try
            {
                await refreshPreparingOrders();
            }
            catch
            {

            }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_orders"))
                {
                    item.value = true;
                    break;
                }
            }
        }
       async void loading_salesItems()
        {
            try
            {
                //await FillCombo.FillComboSalesItemsWithDefault(cb_searchCatalog);
                await FillCombo.FillCategorySale(cb_searchCatalog);
                cb_searchCatalog.SelectedIndex = 0;
            }
            catch { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_salesItems"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        #endregion
        #region Add - Update - Delete  - Tgl - Clear - DG_SelectionChanged - refresh - Btn_updatePreparingTime
        private async void Btn_updatePreparingTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "one"))
                {
                    var selectedOrders = ordersQuery.Where(x => x.IsChecked == true).ToList();
                    if (selectedOrders.Count == 0)// one row is clicked
                        selectedOrders.Add(preparingOrder);
                    decimal preparingTime = decimal.Parse(tb_preparingTime.Text);
                    int res = await preparingOrder.EditPreparingOrdersPrepTime(selectedOrders, preparingTime, MainWindow.userLogin.userId);
                    
                    if(res > 0)
                    {
                        await refreshPreparingOrders();
                        await Search();
                    }
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
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        { //add
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(updatePermission, FillCombo.groupObjects, "one"))
                {
                    var selectedOrders = ordersQuery.Where(x => x.IsChecked == true).ToList();
                        if (HelpClass.validate(requiredControlList, this)  || selectedOrders.Count > 0)
                    {
                        await saveOrderPreparing();
                    }
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
        async Task saveOrderPreparing()
        {
            var selectedOrders = ordersQuery.Where(x => x.IsChecked == true).ToList();
            if (selectedOrders.Count == 0 && preparingOrder.invoiceId != null )// one row is clicked
                selectedOrders.Add(preparingOrder);

            if (selectedOrders.Count > 0)
            {

                #region order status object
                orderPreparingStatus statusObject = new orderPreparingStatus();
                statusObject.notes = tb_notes.Text;
                statusObject.createUserId = MainWindow.userLogin.userId;
                #endregion

                int res = 0;
                string status = "";

                if (selectedOrders.Count == 1)
                    status = preparingOrder.status;
                else if(chk_ready.IsChecked.Value)
                    status = "Ready";
                else if (chk_preparing.IsChecked.Value)
                    status = "Preparing";
                else if (chk_listed.IsChecked.Value)
                    status = "Listed";
                switch (status)
                {
                    case "Listed":

                        statusObject.status = "Preparing";

                        foreach(OrderPreparing or in selectedOrders)
                        {
                            or.notes = tb_notes.Text;
                            or.updateUserId = MainWindow.userLogin.userId;
                        }
                        res = await preparingOrder.editPreparingOrdersAndStatus(selectedOrders, statusObject);
                        break;
                    case "Preparing":
                        statusObject.status = "Ready";

                    res = await preparingOrder.updateListOrdersStatus(selectedOrders, statusObject);
                    break;
                    case "Ready":
                        statusObject.status = "Done";

                    res = await preparingOrder.updateListOrdersStatus(selectedOrders, statusObject);
                    break;
                }
                if (res > 0)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                    clear();
                    await refreshPreparingOrders();
                    await Search();
                }

                else
                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            }
        }

        private void clear()
        {
            try
            {
                preparingOrder = new OrderPreparing();
                this.DataContext = preparingOrder;
                dg_orders.SelectedIndex = -1;
                itemsList = new List<ItemOrderPreparing>();
                BuildOrderItemsDesign();
                btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
                btn_save.IsEnabled = false;
            }
            catch { }
        }



        #endregion
        #region events
        //checkboxColumn yasin
        private void FieldDataGridCheckedHeader(object sender, RoutedEventArgs e)
        {
            try
            {
                //selectedOrders.Clear();
                var chkSelectAll = sender as CheckBox;
                if (chk_allForDelivery.IsChecked == true)
                {
                    chkSelectAll.IsChecked = false;
                }
                else
                {
                    if(dg_orders.Items.Count > 0)
                    {
                        //var firstCol = dg_orders.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                        //var statusCol = dg_orders.Columns[1] as DataGridTextColumn;
                        //if (chkSelectAll == null || firstCol == null || dg_orders?.Items == null || dg_orders.Items.Count == 0)
                        //{
                        //    return;
                        //}

                        var item0 = dg_orders.Items[0] as OrderPreparing;

                        #region refreshSaveBtnText
                        //bdr_cbDeliveryCompany.Visibility = Visibility.Collapsed;

                        if (item0.status.Equals("Listed"))
                        {
                            btn_save.Content = AppSettings.resourcemanager.GetString("trPreparing");
                            btn_save.IsEnabled = true;
                            //bdr_cbDeliveryMan.Visibility = Visibility.Visible;
                            //bdr_tbDeliveryMan.Visibility = Visibility.Collapsed;
                        }
                        else if (item0.status.Equals("Preparing"))
                        {
                            btn_save.Content = AppSettings.resourcemanager.GetString("trReady");
                            btn_save.IsEnabled = true;
                            //bdr_cbDeliveryMan.Visibility = Visibility.Visible;
                            //bdr_tbDeliveryMan.Visibility = Visibility.Collapsed;
                        }
                        else if (item0.status.Equals("Ready"))
                        {
                            btn_save.Content = AppSettings.resourcemanager.GetString("trDone");
                            btn_save.IsEnabled = true;
                            //bdr_cbDeliveryMan.Visibility = Visibility.Collapsed;
                            //bdr_tbDeliveryMan.Visibility = Visibility.Visible;
                        }
                        #endregion

                        foreach (var item in dg_orders.Items)
                        {
                            // stop code yasmin
                            /*
                            var chBx = firstCol.GetCellContent(item) as CheckBox;
                            if (chBx == null)
                            {
                                continue;
                            }
                            */

                            var txt = item as OrderPreparing;
                            if (txt == null)
                            {
                                continue;
                            }
                            if (txt.status.Equals(item0.status))
                            {

                                // stop code yasmin
                                //chBx.IsChecked = chkSelectAll.IsChecked;

                                txt.IsChecked = chkSelectAll.IsChecked.Value;
                                //if (item0.status == "InTheWay")
                                //    requiredControlList = new List<string>();
                                //else
                                //{
                                //    if (item0.shipUserId == null)
                                //        requiredControlList = new List<string> { "companyId" };
                                //    else
                                //        requiredControlList = new List<string> { "userId" };
                                //}
                                //selectedOrders.Add(txt);
                            }
                        }
                    }
                    dg_orders.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void FieldDataGridUncheckedHeader(object sender, RoutedEventArgs e)
        {
            var chkSelectAll = sender as CheckBox;
            if (chk_allForDelivery.IsChecked == true)
            {
                chkSelectAll.IsChecked = false;
            }
            else
            {
                // stop code yasmin
                /*
                var firstCol = dg_orders.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                if (chkSelectAll == null || firstCol == null || dg_orders?.Items == null || dg_orders.Items.Count == 0)
                {
                    return;
                }
               
            foreach (var item in dg_orders.Items)
            {

                var chBx = firstCol.GetCellContent(item) as CheckBox;
                if (chBx == null)
                {
                    continue;
                }
                chBx.IsChecked = chkSelectAll.IsChecked;

            }
                */

                foreach (var item in ordersQuery)
                {
                    item.IsChecked = chkSelectAll.IsChecked.Value;
                }
                dg_orders.Items.Refresh();
            }
        }

        //List<OrderPreparing> selectedOrders = new List<OrderPreparing>();
        private void FieldDataGridChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //CheckBox cb = sender as CheckBox;
                //if (chk_allForDelivery.IsChecked == true)
                //{
                //    selectedOrders.Clear();
                //}
                //else
                //{
                if (chk_allForDelivery.IsChecked != true)
                {
                OrderPreparing selectedOrder = dg_orders.SelectedItem as OrderPreparing;
                }

                //selectedOrders.Add(selectedOrder);
                //}
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void FieldDataGridUnchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (chk_allForDelivery.IsChecked != true)
                {
                    //selectedOrders.Clear();
                    //var index = dg_orders.SelectedIndex;
                    OrderPreparing selectedOrder = dg_orders.SelectedItem as OrderPreparing;
                    //selectedOrders.Remove(selectedOrder);
                    selectedOrder.IsChecked = false;
                }
                //else
                //{
                   
                //}

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }



        private async void Cb_search_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Dg_orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_orders.SelectedIndex != -1)
                {
                    preparingOrder = new OrderPreparing();
                    preparingOrder = dg_orders.SelectedItem as OrderPreparing;

                    this.DataContext = preparingOrder;

                    if (preparingOrder != null)
                    {
                        // stop code yasmin
                        /*
                        if (chk_allForDelivery.IsChecked.Value)
                        {
                            selectedOrders.Clear();

                            if (dg_orders.Items.Count > 1)
                            {
                                var firstCol = dg_orders.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                                if (firstCol != null || dg_orders?.Items != null)
                                    foreach (var item in dg_orders.Items)
                                    {
                                        var chBx = firstCol.GetCellContent(item) as CheckBox;
                                        if (chBx == null)
                                        {
                                            continue;
                                        }
                                        // stop code yasmin
                                        //chBx.IsChecked = false;
                                        preparingOrder.IsChecked =false;
                                    }
                            }
                        }
                        CheckBox checkboxColumn = (dg_orders.Columns[0].GetCellContent(dg_orders.SelectedItem) as CheckBox);
                        checkboxColumn.IsChecked = !checkboxColumn.IsChecked;
                        */
                        if (chk_allForDelivery.IsChecked.Value)
                        {
                            preparingOrder.IsChecked = false;
                        }
                        else
                        {
                            preparingOrder.IsChecked = !preparingOrder.IsChecked;
                            dg_orders.Items.Refresh();
                        }
                        

                        //if (selectedOrders.Count > 0)
                        //{
                        //    if (selectedOrders[0].shipUserId == null)
                        //        requiredControlList = new List<string> { "companyId" };
                        //    else
                        //        requiredControlList = new List<string> { "userId" };
                        //}

                        itemsList = preparingOrder.items;
                        BuildOrderItemsDesign();

                        inputEditable(preparingOrder.status);

                        //btn_save.IsEnabled = true;
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
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
               
                await refreshPreparingOrders();
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
        async Task refreshPreparingOrders()
        {
            List<string> statusLst = new List<string>() { "Listed", "Preparing", "Ready" };

            int duration = 24;
            orders = await preparingOrder.GetKitchenPreparingOrders(MainWindow.branchLogin.branchId, "",duration);
            orders = orders.Where(x=>statusLst.Contains(x.status)).ToList();
            orders = orders.Where(x => x.status != "Ready" || (x.status == "Ready" && x.shippingCompanyId == null)).ToList();
        }
        async Task Search()
        {
            //search
            if (orders is null)
                await refreshPreparingOrders();
            ordersQuery = orders.ToList();

            searchText = tb_search.Text.ToLower();
            ordersQuery = ordersQuery.Where(s => s.orderNum.ToLower().Contains(searchText) ).ToList();

            #region seacrch in catalog
            if (cb_searchCatalog.SelectedIndex > 0)
                ordersQuery = ordersQuery.Where(c => c.items.Where(p => p.categoryId == (int)cb_searchCatalog.SelectedValue).Any()).ToList();
            #endregion 
            #region seacrch status
            //if (cb_searchStatus.SelectedIndex >0)
            //    ordersQuery = ordersQuery.Where(c => c.status == cb_searchStatus.SelectedValue.ToString()).ToList();

            if (chk_allForDelivery.IsChecked == true)
            {
                ordersQuery = ordersQuery.Where(c => c.status.Contains("")).ToList();
            }
            else if (chk_listed.IsChecked == true)
            {
                ordersQuery = ordersQuery.Where(c => c.status == "Listed").ToList();
            }
            else if (chk_preparing.IsChecked == true)
            {
                ordersQuery = ordersQuery.Where(c => c.status == "Preparing").ToList();
            }
            else if (chk_ready.IsChecked == true)
            {
                ordersQuery = ordersQuery.Where(c => c.status == "Ready").ToList();

            }
            #endregion

            #region search invoice type
            if (cb_searchInvType.SelectedIndex > 0)
            {
                List<string> invoiceTypes;

                if (cb_searchInvType.SelectedValue.ToString() == "diningHall")
                    invoiceTypes = new List<string>(){"s","sd" };

                else if(cb_searchInvType.SelectedValue.ToString() == "takeAway")
                    invoiceTypes = new List<string>() { "ts" };

                else
                    invoiceTypes = new List<string>() { "ss" }; // self service

                ordersQuery = ordersQuery.Where(c => invoiceTypes.Contains( c.invType)).ToList();
            }
            #endregion
            dg_orders.ItemsSource = ordersQuery;
        }

        private async void search_Checking(object sender, RoutedEventArgs e)
        {
            try
            {
                //selectedOrders.Clear();
                

                CheckBox cb = sender as CheckBox;
                if (cb.IsChecked == true)
                {
                    if (cb.Name == "chk_allForDelivery")
                    {
                        chk_listed.IsChecked = false;
                        chk_preparing.IsChecked = false;
                        chk_ready.IsChecked = false;
                        col_chk.Visibility = Visibility.Collapsed;
                        inputEditable("Listed");
                    }
                    else if (cb.Name == "chk_listed")
                    {
                        chk_allForDelivery.IsChecked = false;
                        chk_preparing.IsChecked = false;
                        chk_ready.IsChecked = false;
                        col_chk.Visibility = Visibility.Visible;
                        inputEditable("Listed");
                    }
                    else if (cb.Name == "chk_preparing")
                    {
                        chk_allForDelivery.IsChecked = false;
                        chk_listed.IsChecked = false;
                        chk_ready.IsChecked = false;
                        col_chk.Visibility = Visibility.Visible;
                        inputEditable("Preparing");
                    }
                    else if (cb.Name == "chk_ready")
                    {
                        chk_allForDelivery.IsChecked = false;
                        chk_listed.IsChecked = false;
                        chk_preparing.IsChecked = false;
                        col_chk.Visibility = Visibility.Visible;
                        inputEditable("Ready");
                    }
                }
                HelpClass.StartAwait(grid_main);

                //Clear();
                //selectedOrders.Clear();
                foreach (var item in dg_orders.Items)
                {
                    OrderPreparing orderSelected = item as OrderPreparing;
                    orderSelected.IsChecked = false;
                }
                await Search();

                HelpClass.EndAwait(grid_main);
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
                    if (cb.Name == "chk_allForDelivery")
                        chk_allForDelivery.IsChecked = true;
                    else if (cb.Name == "chk_listed")
                        chk_listed.IsChecked = true;
                    else if (cb.Name == "chk_preparing")
                        chk_preparing.IsChecked = true;
                    else if (cb.Name == "chk_ready")
                        chk_ready.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
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
        #region report
        /*
        // report
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
                addpath = @"\Reports\Sale\Ar\PackageReport.rdlc";
            }
            else
                addpath = @"\Reports\Sale\En\PackageReport.rdlc";
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            clsReports.packageReport(itemsQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
        }
        public void pdfpackage()
        {

            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
            });
        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {

                
                    SectionData.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    /////////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        pdfpackage();
                    });
                    t1.Start();
                    //////////////////////////////////////
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        public void printpackage()
        {
            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, MainWindow.rep_printer_name, short.Parse(MainWindow.rep_print_count));
            });
        }
        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    /////////////////////////////////////
                    Thread t1 = new Thread(() =>
                    {
                        printpackage();
                    });
                    t1.Start();
                    //////////////////////////////////////

                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvcCatalog win = new win_lvcCatalog(itemsQuery, 3);
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    /////////////////////
                    string pdfpath = "";
                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);
                    BuildReport();
                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    ///////////////////
                    wd_previewPdf w = new wd_previewPdf();
                    w.pdfPath = pdfpath;
                    if (!string.IsNullOrEmpty(w.pdfPath))
                    {
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();
                    }
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        public void ExcelPackage()
        {

            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                saveFileDialog.Filter = "EXCEL|*.xls;";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToExcel(rep, filepath);
                }
            });
        }
        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
                
                    SectionData.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") || SectionData.isAdminPermision())
                {
                    Thread t1 = new Thread(() =>
                    {
                        ExcelPackage();

                    });
                    t1.Start();
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        */
        #endregion
        #region items
   
        List<ItemOrderPreparing> itemsList = new List<ItemOrderPreparing>();
        void BuildOrderItemsDesign()
        {
            sp_items.Children.Clear();
            foreach (var item in itemsList)
            {
                #region Grid Container
                Grid gridContainer = new Grid();
                int colCount = 3;
                ColumnDefinition[] cd = new ColumnDefinition[colCount];
                for (int i = 0; i < colCount; i++)
                {
                    cd[i] = new ColumnDefinition();
                }
                cd[0].Width = new GridLength(1, GridUnitType.Auto);
                cd[1].Width = new GridLength(1, GridUnitType.Star);
                cd[2].Width = new GridLength(1, GridUnitType.Auto);
                for (int i = 0; i < colCount; i++)
                {
                    gridContainer.ColumnDefinitions.Add(cd[i]);
                }
                /////////////////////////////////////////////////////
                #region   sequence
                var itemSequenceText = new TextBlock();
                itemSequenceText.Text = item.sequence + ".";
                itemSequenceText.Margin = new Thickness(5);
                itemSequenceText.Foreground = Application.Current.Resources["ThickGrey"] as SolidColorBrush;
                itemSequenceText.FontWeight = FontWeights.SemiBold;
                itemSequenceText.VerticalAlignment = VerticalAlignment.Center;
                itemSequenceText.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(itemSequenceText, 0);
               
                gridContainer.Children.Add(itemSequenceText);
                #endregion
                #region   name
                var itemNameText = new TextBlock();
                itemNameText.Text = item.itemName;
                itemNameText.Margin = new Thickness(5);
                itemNameText.Foreground = Application.Current.Resources["ThickGrey"] as SolidColorBrush;
                //itemNameText.FontWeight = FontWeights.SemiBold;
                itemNameText.VerticalAlignment = VerticalAlignment.Center;
                itemNameText.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(itemNameText, 1);

                gridContainer.Children.Add(itemNameText);
                #endregion
                #region   count
                var itemCountText = new TextBlock();
                itemCountText.Text = item.quantity.ToString();
                itemCountText.Margin = new Thickness(5, 5 ,10 , 5);
                itemCountText.Foreground = Application.Current.Resources["ThickGrey"] as SolidColorBrush;
                //itemCountText.FontWeight = FontWeights.SemiBold;
                itemCountText.VerticalAlignment = VerticalAlignment.Center;
                itemCountText.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetColumn(itemCountText, 2);

                gridContainer.Children.Add(itemCountText);
                #endregion
                #endregion
                sp_items.Children.Add(gridContainer);
            }
        }



        #endregion

        #region inputEditable
        private void inputEditable(string status)
        {
            switch(status)
            {
                case "Listed":
                    gd_preparingTime.Visibility = Visibility.Visible;
                    btn_save.Content = AppSettings.resourcemanager.GetString("trPreparing");
                    btn_save.IsEnabled = true;
                    break;
                case "Preparing":
                    gd_preparingTime.Visibility = Visibility.Collapsed;
                    btn_save.Content = AppSettings.resourcemanager.GetString("trReady");
                    btn_save.IsEnabled = true;
                    break;
                case "Ready":
                    gd_preparingTime.Visibility = Visibility.Collapsed;
                    btn_save.Content = AppSettings.resourcemanager.GetString("trDone");

                    if(preparingOrder.shippingCompanyId != null) // order is take away (make done in delivery managment)
                        btn_save.IsEnabled = false;
                    else
                        btn_save.IsEnabled = true;

                    break;
                case "Done":
                    gd_preparingTime.Visibility = Visibility.Collapsed;
                    btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
                    btn_save.IsEnabled = false;
                    break;
            }
        }

        #endregion

       
    }
}
