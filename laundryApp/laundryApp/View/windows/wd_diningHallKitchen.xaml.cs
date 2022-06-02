using netoaster;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Threading;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.Collections.Specialized;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_diningHallKitchen.xaml
    /// </summary>
    public partial class wd_diningHallKitchen : Window
    {
        string kitchenPermission = "saleInvoice_invoice";
        public static List<string> requiredControlList;
        public List<BillDetailsSales> invoiceItemsList = new List<BillDetailsSales>();
        public int invoiceId;


        OrderPreparing preparingOrder = new OrderPreparing();
        List<OrderPreparing> orders= new List<OrderPreparing>();
        BillDetailsSales invoiceRow = new BillDetailsSales();
        List<BillDetailsSales> unSentInvoiceItems = new List<BillDetailsSales>();

        List<ItemOrderPreparing> preparingItemsList = new List<ItemOrderPreparing>();
        public wd_diningHallKitchen()
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
                if (e.Key == Key.Return)
                {
                    //Btn_select_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            requiredControlList = new List<string> { "count"};

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
        
            await refreshPreparingOrders();
            fillInvoiceItems();
        }
        private void translate()
        {
            #region dg_invoiceItems
            dg_invoiceItems.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_invoiceItems.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_invoiceItems.Columns[2].Header = AppSettings.resourcemanager.GetString("trCount");
            #endregion

            #region dg_orders
            dg_orders.Columns[1].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_orders.Columns[2].Header = AppSettings.resourcemanager.GetString("trRemainingTime");
            dg_orders.Columns[3].Header = AppSettings.resourcemanager.GetString("trCount");
            dg_orders.Columns[4].Header = AppSettings.resourcemanager.GetString("trStatus");
            #endregion

            txt_title.Text = AppSettings.resourcemanager.GetString("trKitchen");
            txt_invoiceItems.Text = AppSettings.resourcemanager.GetString("invoiceItems");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_count, AppSettings.resourcemanager.GetString("trCountHint"));

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            btn_send.Content = AppSettings.resourcemanager.GetString("trSend");
            btn_sendAll.Content = AppSettings.resourcemanager.GetString("trSendAll");

        }
        #region loading
        void fillInvoiceItems()
        {
            unSentInvoiceItems = new List<BillDetailsSales>();
            int index = 1;
            foreach (BillDetailsSales b in invoiceItemsList)
            {

                int itemCountInOrder = 0;
                try { itemCountInOrder = orders.Where(x => x.itemUnitId == b.itemUnitId).Sum(x => x.quantity); }
                catch { }

                int remainingCount = b.Count - itemCountInOrder;
                if (remainingCount > 0)
                {
                    BillDetailsSales newBillRow = new BillDetailsSales()
                    {
                        itemUnitId = b.itemUnitId,
                        itemName = b.itemName,
                        index = index,
                        Count = remainingCount,
                    };
                    index++;
                    unSentInvoiceItems.Add(newBillRow);
                }              
            }
            dg_invoiceItems.ItemsSource = unSentInvoiceItems;

            if (unSentInvoiceItems.Count == 0)
                btn_sendAll.IsEnabled = false;
            else
                btn_sendAll.IsEnabled = true;
        }
        async Task refreshPreparingOrders()
        {
            orders = await preparingOrder.GetInvoicePreparingOrders(invoiceId);
            dg_orders.ItemsSource = orders;
        }
        #endregion
       

        #region datagris selectionChanged
        private void Dg_invoiceItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection

                if (dg_invoiceItems.SelectedIndex != -1)
                {
                    invoiceRow = new BillDetailsSales();
                    invoiceRow = dg_invoiceItems.SelectedItem as BillDetailsSales;
                    this.DataContext = invoiceRow;

                    tb_count.Text = invoiceRow.Count.ToString();

                    #region enable button
                    btn_send.IsEnabled = true;
                    #endregion
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
        #endregion

        #region events
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
        private void Tb_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int count = 0;
                if(tb_count.Text != "")
                    count = int.Parse(tb_count.Text);

                if(count > invoiceRow.Count )
                {
                    tb_count.Text = invoiceRow.Count.ToString();
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trExceedCount"), animation: ToasterAnimation.FadeIn);
                }
            }
            catch { }
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clear();
            }
            catch { }
        }

        private async void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(kitchenPermission, FillCombo.groupObjects, "send"))
                //{
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        if (int.Parse(tb_count.Text) > 0)
                        {                         
                            await saveOrderPreparing();
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMustBeMoreThanZero"), animation: ToasterAnimation.FadeIn);

                    }
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_sendAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(kitchenPermission, FillCombo.groupObjects, "send"))
                //{                   
                   await saveOrdersPreparing();
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        #region function clear - saveOrderPreparing
        private void clear()
        {
            invoiceRow = new BillDetailsSales();
            this.DataContext = invoiceRow;
            tb_count.Text = "";
            dg_invoiceItems.SelectedIndex = -1;

            btn_send.IsEnabled = false;
        }

        async Task saveOrderPreparing()
        {
            #region order Items
            preparingItemsList = new List<ItemOrderPreparing>();
            ItemOrderPreparing it = new ItemOrderPreparing()
            {
                itemUnitId = invoiceRow.itemUnitId,
                quantity = int.Parse(tb_count.Text),
                notes = tb_notes.Text,
                createUserId = MainWindow.userLogin.userId,
            };
            preparingItemsList.Add(it);
            #endregion
            #region preparing order object
            preparingOrder = new OrderPreparing();
            preparingOrder.invoiceId = invoiceId;
            preparingOrder.notes = tb_notes.Text;
            preparingOrder.orderNum = await preparingOrder.generateOrderNumber("ko",MainWindow.branchLogin.code,MainWindow.branchLogin.branchId);///???
            #endregion
            #region order status object
            orderPreparingStatus statusObject = new orderPreparingStatus();
            statusObject.status = "Listed";
            statusObject.notes = tb_notes.Text;
            statusObject.createUserId = MainWindow.userLogin.userId;
            #endregion
            if (AppSettings.print_kitchen_on_sale == "1")
            {
                //list before save->  orders
                // OrderListbeforesave = orders;
                OrderListBeforesave = await preparingOrder.GetOrdersByInvoiceId(invoiceId);
            }
            int res = await preparingOrder.savePreparingOrder(preparingOrder, preparingItemsList,statusObject);
            if (res > 0)
            {
               
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                #region statusesOfPreparingOrder
                if (AppSettings.statusesOfPreparingOrder == "directlyPrint")
                {
                    #region save status = Preparing
                    statusObject = new orderPreparingStatus();
                    statusObject.orderPreparingId = res;
                    statusObject.status = "Preparing";
                    statusObject.createUserId = MainWindow.userLogin.userId;

                    await preparingOrder.updateOrderStatus(statusObject);
                    #endregion

                    #region save status = Ready
                    statusObject = new orderPreparingStatus();
                    statusObject.orderPreparingId = res;
                    statusObject.status = "Ready";
                    statusObject.createUserId = MainWindow.userLogin.userId;

                    await preparingOrder.updateOrderStatus(statusObject);
                    #endregion

                    #region save status = Done if no shipping
                    statusObject = new orderPreparingStatus();
                    statusObject.orderPreparingId = res;
                    statusObject.status = "Done";
                    statusObject.createUserId = MainWindow.userLogin.userId;

                    await preparingOrder.updateOrderStatus(statusObject);
                    #endregion
                }
                #endregion
                clear();
                await refreshPreparingOrders();
               fillInvoiceItems();
                if (AppSettings.print_kitchen_on_sale == "1")
                {
                    //list after before save->  orders
                    // OrderListAftersave = orders;
                    OrderListAftersave = await preparingOrder.GetOrdersByInvoiceId(invoiceId);

                    prOrderPreparingList = clsreport.newKitchenorderList(OrderListBeforesave, OrderListAftersave);

                  
                    Thread t2 = new Thread(() =>
                    {

                        printInvoiceInkitchen(invoiceId, prOrderPreparingList);
                    });
                    t2.Start();
                }

            }
            else
                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
        }
        async Task saveOrdersPreparing()
        {
            #region order Items

            preparingItemsList = new List<ItemOrderPreparing>();

            foreach (BillDetailsSales b in unSentInvoiceItems)
            {
                ItemOrderPreparing it = new ItemOrderPreparing()
                {
                    itemUnitId = b.itemUnitId,
                    quantity = b.Count,
                    notes = tb_notes.Text,
                    createUserId = MainWindow.userLogin.userId,
                };
                preparingItemsList.Add(it);
            }
            if (preparingItemsList.Count == 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAllItemsListed"), animation: ToasterAnimation.FadeIn);

            else
            {
                #endregion
                #region preparing order object
                preparingOrder = new OrderPreparing();
                preparingOrder.invoiceId = invoiceId;
                preparingOrder.notes = tb_notes.Text;
                //preparingOrder.orderNum = await preparingOrder.generateOrderNumber("ko",MainWindow.branchLogin.code,MainWindow.branchLogin.branchId);///???
                #endregion
                #region order status object
                orderPreparingStatus statusObject = new orderPreparingStatus();
                statusObject.status = "Listed";
                statusObject.notes = tb_notes.Text;
                statusObject.createUserId = MainWindow.userLogin.userId;
                #endregion
                if (AppSettings.print_kitchen_on_sale == "1")
                {
                    //list before save->  orders
                    // OrderListbeforesave = orders;
                    OrderListBeforesave = await preparingOrder.GetOrdersByInvoiceId(invoiceId);
                }
                int res = await preparingOrder.savePreparingOrders(preparingOrder, preparingItemsList, statusObject, MainWindow.branchLogin.branchId, AppSettings.statusesOfPreparingOrder);

                if (res > 0)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                      
                    clear();
                    await refreshPreparingOrders();
                    fillInvoiceItems();
                    if (AppSettings.print_kitchen_on_sale == "1")
                    {
                        //list after before save->  orders
                        // OrderListAftersave = orders;
                        OrderListAftersave = await preparingOrder.GetOrdersByInvoiceId(invoiceId);

                    prOrderPreparingList =  clsreport.newKitchenorderList(OrderListBeforesave, OrderListAftersave);

                        //print

                        // prOrderPreparingList = await preparingOrder.GetOrdersByInvoiceId(prinvoiceId);

                        Thread t2 = new Thread(() =>
                        {
                            //send all
                            printInvoiceInkitchen(invoiceId, prOrderPreparingList);
                        });
                        t2.Start();
                    }
                }
                else
                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            }
        }

        #endregion
        #region report
        ReportCls reportclass = new ReportCls();
        //SaveFileDialog saveFileDialog = new SaveFileDialog();
        List<OrderPreparing> prOrderPreparingList = new List<OrderPreparing>();
        List<OrderPreparing> OrderListBeforesave = new List<OrderPreparing>();
        List<OrderPreparing> OrderListAftersave = new List<OrderPreparing>();
        clsReports clsreport = new clsReports();
        public void printInvoiceInkitchen(int invoiceId, List<OrderPreparing> OrderPreparingList)
        {
            try
            {
                //   prInvoice = new Invoice();

                if (invoiceId > 0)
                {
                    reportsize rs = reportclass.PrintPrepOrder(OrderPreparingList);
                    this.Dispatcher.Invoke(() =>
                    {
                        if (AppSettings.kitchenPaperSize == "A4")
                        {

                            LocalReportExtensions.PrintToPrinterbyNameAndCopy(rs.rep, AppSettings.kitchen_printer_name, short.Parse(AppSettings.kitchen_copy_count));

                        }
                        else
                        {


                            LocalReportExtensions.customPrintToPrinter(rs.rep, AppSettings.kitchen_printer_name, short.Parse(AppSettings.kitchen_copy_count), rs.width, rs.height);

                        }

                    });
                    //foreach (OrderPreparing row in OrderPreparingList)
                    //{
                    //    List<OrderPreparing> templist = new List<OrderPreparing>();
                    //    templist.Add(row);
                    //    printPrepOrder(templist);
                    //}

                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPrintEmptyInvoice"), animation: ToasterAnimation.FadeIn);
                    });
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
                this.Dispatcher.Invoke(() =>
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("notCompleted"), animation: ToasterAnimation.FadeIn);

                });
            }


        }
       
        #endregion
    }
}
