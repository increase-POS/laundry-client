using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using LiveCharts.Helpers;
using laundryApp.View.windows;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using static laundryApp.Classes.Statistics;
using System.Globalization;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using laundryApp.View.storage;
using System.Resources;
using System.Reflection;
using laundryApp.View.storage.movementsOperations;

namespace laundryApp.View.reports.storageReports
{
    /// <summary>
    /// Interaction logic for uc_externalStorageReports.xaml
    /// </summary>
    public partial class uc_externalStorageReports : UserControl
    {

        private static uc_externalStorageReports _instance;
        public static uc_externalStorageReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_externalStorageReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_externalStorageReports()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }

        //List<Storage> storages;

        List<ItemTransferInvoice> itemsTransfer;

        IEnumerable<ItemTransferInvoice> agentsCount;
        IEnumerable<ItemTransferInvoice> invCount;

        Statistics statisticModel = new Statistics();

        private int selectedExternalTab = 0;
        List<ExternalitemCombo> comboExternalItemsItems;
        List<ExternalUnitCombo> comboExternalItemsUnits;
        List<AgentCombo> comboExternalAgentsAgents;
        List<InvTypeCombo> comboExternalInvType;
        List<InvCombo> comboExternalInvoiceInvoice;



        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
               
                HelpClass.StartAwait(grid_main);

                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                itemsTransfer = await statisticModel.GetExternalMov((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);

                comboExternalItemsItems = statisticModel.getExternalItemCombo(itemsTransfer);
                comboExternalItemsUnits = statisticModel.getExternalUnitCombo(itemsTransfer);
                comboExternalAgentsAgents = statisticModel.GetExternalAgentCombos(itemsTransfer);
                comboExternalInvType = statisticModel.GetExternalInvoiceTypeCombos(itemsTransfer);
                comboExternalInvoiceInvoice = statisticModel.GetExternalInvoiceCombos(itemsTransfer);

                fillComboExternalAgentsAgentsType();
                fillComboExternalInvType();
                fillComboExternalInvoiceInvoice();
                fillComboExternalAgentsAgents();

                chk_externalAgentsIn.IsChecked = true;
                chk_externalItemsIn.IsChecked = true;
                chk_externalAgentsOut.IsChecked = true;
                chk_externalItemsOut.IsChecked = true;

                btn_externalItems_Click(btn_item, null);

               
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
            tt_item.Content = AppSettings.resourcemanager.GetString("trItems");
            tt_agent.Content = AppSettings.resourcemanager.GetString("trAgents");
            tt_invoice.Content = AppSettings.resourcemanager.GetString("trInvoices");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_externalItemsBranches, AppSettings.resourcemanager.GetString("trBranch/StoreHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_externalItemsItems, AppSettings.resourcemanager.GetString("trItemHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_externalItemsUnits, AppSettings.resourcemanager.GetString("trUnitHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_externalItemsStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_externalItemsEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));

            chk_externalItemsAllBranches.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_externalItemsAllItems.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_externalItemsAllUnits.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_externalItemsIn.Content = AppSettings.resourcemanager.GetString("trIn");
            chk_externalItemsOut.Content = AppSettings.resourcemanager.GetString("trOut");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_invNumber.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_invTypeNumber.Header = AppSettings.resourcemanager.GetString("trType");
            col_invDate.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_item.Header = AppSettings.resourcemanager.GetString("trItem");
            col_unit.Header = AppSettings.resourcemanager.GetString("trUnit");
            col_agentType.Header = AppSettings.resourcemanager.GetString("trAgentType");
            col_agent.Header = AppSettings.resourcemanager.GetString("trName");
            col_itemUnits.Header = AppSettings.resourcemanager.GetString("trItemUnit");
            col_agentTypeAgent.Header = AppSettings.resourcemanager.GetString("trType")+"-"+ AppSettings.resourcemanager.GetString("trName");
            col_quantity.Header = AppSettings.resourcemanager.GetString("trQTR");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }
        

        public void paintExternlaChilds()
        {
            //bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);

            grid_externalItems.Visibility = Visibility.Hidden;
            grid_externalAgents.Visibility = Visibility.Hidden;
            grid_externalInvoices.Visibility = Visibility.Hidden;


            path_item.Fill = Brushes.White;
            path_agent.Fill = Brushes.White;
            path_invoice.Fill = Brushes.White;

            bdr_agent.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_item.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            bdr_invoice.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush;
        }
        private async void btn_externalItems_Click(object sender, RoutedEventArgs e)
        {//items
            try
            {
               
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                selectedExternalTab = 0;
                txt_search.Text = "";

                paintExternlaChilds();
                grid_externalItems.Visibility = Visibility.Visible;
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_item);
                path_item.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                showSelectedTabColumn();

                await FillCombo.fillComboBranchesAllWithoutMain(cb_externalItemsBranches);

                fillEvents();

               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void btn_externalAgents_Click(object sender, RoutedEventArgs e)
        {//agents
            try
            {
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                selectedExternalTab = 1;
                txt_search.Text = "";

                paintExternlaChilds();
                grid_externalAgents.Visibility = Visibility.Visible;
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_agent);
                path_agent.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                showSelectedTabColumn();

                await FillCombo.fillComboBranchesAllWithoutMain(cb_externalAgentsBranches);

                fillEvents();

               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void btn_externalInvoices_Click(object sender, RoutedEventArgs e)
        {//invoices
            try
            {
               
                HelpClass.StartAwait(grid_main);

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                selectedExternalTab = 2;
                txt_search.Text = "";

                paintExternlaChilds();
                grid_externalInvoices.Visibility = Visibility.Visible;
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_invoice);
                path_invoice.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                showSelectedTabColumn();

                await FillCombo.fillComboBranchesAllWithoutMain(cb_externalInvoicesBranches);

                fillEvents();
               
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void fillComboExternalItemsItems()
        {
            var temp = cb_externalItemsBranches.SelectedItem as Branch;
            cb_externalItemsItems.SelectedValuePath = "ItemId";
            cb_externalItemsItems.DisplayMemberPath = "ItemName";
            if (temp == null)
            {
                cb_externalItemsItems.ItemsSource = comboExternalItemsItems
                    .GroupBy(x => x.ItemId)
                    .Select(g => new ExternalitemCombo
                    {
                        ItemId = g.FirstOrDefault().ItemId,
                        ItemName = g.FirstOrDefault().ItemName,
                        BranchId = g.FirstOrDefault().BranchId
                    }).ToList();
            }
            else
            {
                cb_externalItemsItems.ItemsSource = comboExternalItemsItems
                    .Where(x => x.BranchId == temp.branchId)
                    .GroupBy(x => x.ItemId)
                    .Select(g => new ExternalitemCombo
                    {
                        ItemId = g.FirstOrDefault().ItemId,
                        ItemName = g.FirstOrDefault().ItemName,
                        BranchId = g.FirstOrDefault().BranchId
                    }).ToList();
            }
        }

        private void fillComboExternalItemsUnits()
        {
            var temp = cb_externalItemsItems.SelectedItem as ExternalitemCombo;
            var temp1 = cb_externalItemsBranches.SelectedItem as Branch;

            cb_externalItemsUnits.SelectedValuePath = "UnitId";
            cb_externalItemsUnits.DisplayMemberPath = "UnitName";
            if (temp == null && temp1 == null)
            {
                cb_externalItemsUnits.ItemsSource = comboExternalItemsUnits
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
            else if (temp != null && temp1 == null)
            {
                cb_externalItemsUnits.ItemsSource = comboExternalItemsUnits
                     .Where(x => x.ItemId == temp.ItemId)
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
            else if (temp == null && temp1 != null)
            {
                cb_externalItemsUnits.ItemsSource = comboExternalItemsUnits
                     .Where(x => x.BranchId == temp1.branchId)
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
            else
            {
                cb_externalItemsUnits.ItemsSource = comboExternalItemsUnits
                    .Where(x => x.ItemId == temp.ItemId && x.BranchId == temp1.branchId)
                    .GroupBy(x => x.UnitId)
                    .Select(g => new ExternalUnitCombo
                    {
                        UnitId = g.FirstOrDefault().UnitId,
                        UnitName = g.FirstOrDefault().UnitName,
                        ItemId = g.FirstOrDefault().ItemId
                    });
            }
        }

        private void fillComboExternalAgentsAgentsType()
        {
            var dislist = new[] {
                new { Text = AppSettings.resourcemanager.GetString("trVendor")    , Value = "v" },
                new { Text = AppSettings.resourcemanager.GetString("trCustomer")  , Value = "c" }
                 };
            cb_externalAgentsAgentsType.DisplayMemberPath = "Text";
            cb_externalAgentsAgentsType.SelectedValuePath = "Value";
            cb_externalAgentsAgentsType.ItemsSource = dislist;
            cb_externalAgentsAgentsType.SelectedIndex = 0;
        }

        private void fillComboExternalAgentsAgents()
        {
            var temp = cb_externalAgentsAgentsType.SelectedItem as AgentTypeCombo;
            var temp1 = cb_externalAgentsBranches.SelectedItem as Branch;
            cb_externalAgentsCustomer.SelectedValuePath = "AgentId";
            cb_externalAgentsCustomer.DisplayMemberPath = "AgentName";
            if (temp == null)
            {
                cb_externalAgentsCustomer.ItemsSource = comboExternalAgentsAgents
                    .GroupBy(x => x.AgentId)
                    .Select(g => new AgentCombo
                    {
                        AgentId = g.FirstOrDefault().AgentId,
                        AgentName = g.FirstOrDefault().AgentName,
                        BranchId = g.FirstOrDefault().BranchId,
                        AgentType = g.FirstOrDefault().AgentType
                    }).ToList();
            }
            else if (temp != null && temp1 == null)
            {
                cb_externalAgentsCustomer.ItemsSource = comboExternalAgentsAgents
                        .Where(x => x.AgentType == temp.AgentType)
                    .GroupBy(x => x.AgentId)
                    .Select(g => new AgentCombo
                    {
                        AgentId = g.FirstOrDefault().AgentId,
                        AgentName = g.FirstOrDefault().AgentName,
                        BranchId = g.FirstOrDefault().BranchId,
                        AgentType = g.FirstOrDefault().AgentType
                    }).ToList();
            }
            else if (temp == null && temp1 != null)
            {
                cb_externalAgentsCustomer.ItemsSource = comboExternalAgentsAgents
                        .Where(x => x.BranchId == temp1.branchId)
                    .GroupBy(x => x.AgentId)
                    .Select(g => new AgentCombo
                    {
                        AgentId = g.FirstOrDefault().AgentId,
                        AgentName = g.FirstOrDefault().AgentName,
                        BranchId = g.FirstOrDefault().BranchId,
                        AgentType = g.FirstOrDefault().AgentType
                    }).ToList();
            }
            else
            {
                cb_externalAgentsCustomer.ItemsSource = comboExternalAgentsAgents
                         .Where(x => x.AgentType == temp.AgentType && x.BranchId == temp1.branchId)
                    .GroupBy(x => x.AgentId)
                    .Select(g => new AgentCombo
                    {
                        AgentId = g.FirstOrDefault().AgentId,
                        AgentName = g.FirstOrDefault().AgentName,
                        BranchId = g.FirstOrDefault().BranchId,
                        AgentType = g.FirstOrDefault().AgentType
                    }).ToList();
            }

        }

        private void fillComboExternalInvType()
        {
            var temp = cb_externalInvoicesBranches.SelectedItem as Branch;
            //cb_externalInvoicesInvoiceType.SelectedValuePath = "InvoiceId";
            cb_externalInvoicesInvoiceType.SelectedValuePath = "InvoiceType";
            cb_externalInvoicesInvoiceType.DisplayMemberPath = "InvoiceType";
            if (temp == null)
            {
                var lst = comboExternalInvType
                    .GroupBy(x => x.InvoiceType)
                    .Select(g => new InvTypeCombo
                    {
                        InvoiceType = g.FirstOrDefault().InvoiceType,
                        BranchId = g.FirstOrDefault().BranchId
                    }).ToList();

                foreach (var l in lst)
                {
                    l.InvoiceType = getInvoiceType(l.InvoiceType);
                }
                cb_externalInvoicesInvoiceType.ItemsSource = lst;
            }
            else
            {
                var lst = comboExternalInvType
                    .Where(x => x.BranchId == temp.branchId)
                    .GroupBy(x => x.InvoiceType)
                    .Select(g => new InvTypeCombo
                    {
                        InvoiceType = g.FirstOrDefault().InvoiceType,
                        BranchId = g.FirstOrDefault().BranchId
                    }).ToList();

                foreach (var l in lst)
                {
                    l.InvoiceType = getInvoiceType(l.InvoiceType);
                }
                cb_externalInvoicesInvoiceType.ItemsSource = lst;
            }
        }

        private string getInvoiceType(string s)
        {
            string value = "";
            switch (s)
            {
                //مشتريات 
                case "p":
                    value = AppSettings.resourcemanager.GetString("trPurchaseInvoice");
                    break;
                //فاتورة مشتريات بانتظار الادخال
                case "pw":
                    value = AppSettings.resourcemanager.GetString("trPurchaseInvoiceWaiting");
                    break;
                //مبيعات
                case "s":
                    value = AppSettings.resourcemanager.GetString("trSalesInvoice");
                    break;
                //مرتجع مبيعات
                case "sb":
                    value = AppSettings.resourcemanager.GetString("trSalesReturnInvoice");
                    break;
                //مرتجع مشتريات
                case "pb":
                    value = AppSettings.resourcemanager.GetString("trPurchaseReturnInvoice");
                    break;
                //فاتورة مرتجع مشتريات بانتظار الاخراج
                case "pbw":
                    value = AppSettings.resourcemanager.GetString("trPurchaseReturnInvoiceWaiting");
                    break;
                //مسودة مشتريات 
                case "pd":
                    value = AppSettings.resourcemanager.GetString("trDraftPurchaseBill");
                    break;
                //مسودة مبيعات
                case "sd":
                    value = AppSettings.resourcemanager.GetString("trSalesDraft");
                    break;
                //مسودة مرتجع مبيعات
                case "sbd":
                    value = AppSettings.resourcemanager.GetString("trSalesReturnDraft");
                    break;
                //مسودة مرتجع مشتريات
                case "pbd":
                    value = AppSettings.resourcemanager.GetString("trPurchaseReturnDraft");
                    break;
                // مسودة طلبية مبيعات 
                case "ord":
                    value = AppSettings.resourcemanager.GetString("trSaleOrderDraft");
                    break;
                //   طلبية مبيعات 
                case "or":
                    value = AppSettings.resourcemanager.GetString("trSaleOrder");
                    break;
                case "ors":
                    value = AppSettings.resourcemanager.GetString("trSaleOrder");
                    break;
                // مسودة طلبية شراء 
                case "pod":
                    value = AppSettings.resourcemanager.GetString("trPurchaceOrderDraft");
                    break;
                // طلبية شراء 
                case "po":
                    value = AppSettings.resourcemanager.GetString("trPurchaceOrder");
                    break;
                case "pos":
                    value = AppSettings.resourcemanager.GetString("trPurchaceOrder");
                    break;
                //مسودة عرض 
                case "qd":
                    value = AppSettings.resourcemanager.GetString("trQuotationsDraft");
                    break;
                //فاتورة عرض اسعار
                case "q":
                    value = AppSettings.resourcemanager.GetString("trQuotations");
                    break;
                case "qs":
                    value = AppSettings.resourcemanager.GetString("trQuotations");
                    break;
                //الإتلاف
                case "d":
                    value = AppSettings.resourcemanager.GetString("trDestructive");
                    break;
                //النواقص
                case "sh":
                    value = AppSettings.resourcemanager.GetString("trShortage");
                    break;
                //مسودة  استراد
                case "imd":
                    value = AppSettings.resourcemanager.GetString("trImportDraft");
                    break;
                // استراد
                case "im":
                    value = AppSettings.resourcemanager.GetString("trImport");
                    break;
                // طلب استيراد
                case "imw":
                    value = AppSettings.resourcemanager.GetString("trImportOrder");
                    break;
                //مسودة تصدير
                case "exd":
                    value = AppSettings.resourcemanager.GetString("trExportDraft");
                    break;
                // تصدير
                case "ex":
                    value = AppSettings.resourcemanager.GetString("trExport");
                    break;
                // طلب تصدير
                case "exw":
                    value = AppSettings.resourcemanager.GetString("trExportOrder");
                    break;
                // إدخال مباشر
                case "is":
                    value = AppSettings.resourcemanager.GetString("trDirectEntry");
                    break;
                default: break;
            }

            return value;

        }

        private void fillComboExternalInvoiceInvoice()
        {
            var temp = cb_externalInvoicesInvoiceType.SelectedItem as InvTypeCombo;
            var temp1 = cb_externalInvoicesBranches.SelectedItem as Branch;
            cb_externalInvoicesInvoice.SelectedValuePath = "InvoiceId";
            cb_externalInvoicesInvoice.DisplayMemberPath = "InvoiceNumber";
            if (temp == null && temp1 == null)
            {
                cb_externalInvoicesInvoice.ItemsSource = comboExternalInvoiceInvoice
                    .GroupBy(x => x.InvoiceId)
                    .Select(g => new InvCombo
                    {
                        InvoiceId = g.FirstOrDefault().InvoiceId,
                        InvoiceNumber = g.FirstOrDefault().InvoiceNumber,
                        BranchId = g.FirstOrDefault().BranchId,
                        InvoiceType = g.FirstOrDefault().InvoiceType
                    }).ToList();
            }
            else if (temp != null && temp1 == null)
            {
                cb_externalInvoicesInvoice.ItemsSource = comboExternalInvoiceInvoice
                      .Where(x => getInvoiceType(x.InvoiceType) == temp.InvoiceType)
                    .GroupBy(x => x.InvoiceId)
                    .Select(g => new InvCombo
                    {
                        InvoiceId = g.FirstOrDefault().InvoiceId,
                        InvoiceNumber = g.FirstOrDefault().InvoiceNumber,
                        BranchId = g.FirstOrDefault().BranchId,
                        InvoiceType = g.FirstOrDefault().InvoiceType
                    }).ToList();
            }
            else if (temp == null && temp1 != null)
            {
                cb_externalInvoicesInvoice.ItemsSource = comboExternalInvoiceInvoice
                      .Where(x => x.BranchId == temp1.branchId)
                    .GroupBy(x => x.InvoiceId)
                    .Select(g => new InvCombo
                    {
                        InvoiceId = g.FirstOrDefault().InvoiceId,
                        InvoiceNumber = g.FirstOrDefault().InvoiceNumber,
                        BranchId = g.FirstOrDefault().BranchId,
                        InvoiceType = g.FirstOrDefault().InvoiceType
                    }).ToList();
            }
            else
            {
                cb_externalInvoicesInvoice.ItemsSource = comboExternalInvoiceInvoice
                    .Where(x => x.InvoiceType == temp.InvoiceType && x.BranchId == temp1.branchId)
                     .GroupBy(x => x.InvoiceId)
                    .Select(g => new InvCombo
                    {
                        InvoiceId = g.FirstOrDefault().InvoiceId,
                        InvoiceNumber = g.FirstOrDefault().InvoiceNumber,
                        BranchId = g.FirstOrDefault().BranchId,
                        InvoiceType = g.FirstOrDefault().InvoiceType
                    }).ToList();
            }
        }

        IEnumerable<ItemTransferInvoice> itemTransferInvoicesLst;
        private IEnumerable<ItemTransferInvoice> fillList(IEnumerable<ItemTransferInvoice> itemsTransfer, ComboBox comboBranch, ComboBox comboItem, ComboBox comboUnit, DatePicker startDate, DatePicker endDate, CheckBox chkAllBranches, CheckBox chkAllItems, CheckBox chkAllUnits, CheckBox chkIn, CheckBox chkOut)
        {
            var selectedBranch = comboBranch.SelectedItem as Branch;
            var selectedItem = comboItem.SelectedItem as ExternalitemCombo;
            var selectedUnit = comboUnit.SelectedItem as ExternalUnitCombo;
            var selectedAgentType = comboItem.SelectedItem as AgentTypeCombo;
            var selectedAgent = comboUnit.SelectedItem as AgentCombo;
            var selectedInvoiceType = comboItem.SelectedItem as InvTypeCombo;
            var selectedInvoice = comboUnit.SelectedItem as InvCombo;

            var result = itemsTransfer.Where(x => (
                  (selectedExternalTab == 0 ? (
                            ((chkIn.IsChecked == true ? (x.invType == "p") || (x.invType == "sb") : false)
                            || (chkOut.IsChecked == true ? (x.invType == "s") || (x.invType == "pb") : false))
                          && (comboBranch.SelectedItem != null ? (x.branchId == selectedBranch.branchId) : true)
                          && (comboItem.SelectedItem != null ? (x.itemId == selectedItem.ItemId) : true)
                          && (comboUnit.SelectedItem != null ? (x.unitId == selectedUnit.UnitId) : true)
                          && (dp_externalItemsStartDate.SelectedDate != null ? (x.invDate >= startDate.SelectedDate) : true)
                          && (dp_externalItemsEndDate.SelectedDate != null ? (x.invDate <= endDate.SelectedDate) : true)
                          )
                          : true
                          )
                          && (selectedExternalTab == 1 ? (
                           ((chkIn.IsChecked == true ? (x.invType == "p") || (x.invType == "sb") : false)
                            || (chkOut.IsChecked == true ? (x.invType == "s") || (x.invType == "pb") : false))
                          && (comboBranch.SelectedItem != null ? (x.branchId == selectedBranch.branchId) : true)
                          && (comboItem.SelectedItem != null ? (x.agentType == cb_externalAgentsAgentsType.SelectedValue.ToString()) : true)
                          && (comboUnit.SelectedItem != null ? (x.agentId == selectedAgent.AgentId) : true)
                          && (dp_externalAgentsStartDate.SelectedDate != null ? (x.invDate >= startDate.SelectedDate) : true)
                          && (dp_externalAgentsEndDate.SelectedDate != null ? (x.invDate <= endDate.SelectedDate) : true)
                          )
                          : true
                          )
                          && (selectedExternalTab == 2 ? (
                            (comboBranch.SelectedItem != null ? (x.branchId == selectedBranch.branchId) : true)
                          && (comboItem.SelectedItem != null ? (getInvoiceType(x.invType) == selectedInvoiceType.InvoiceType) : true)
                          && (comboUnit.SelectedItem != null ? (x.invoiceId == selectedInvoice.InvoiceId) : true)
                          && (dp_externalInvoicesStartDate.SelectedDate != null ? (x.invDate >= startDate.SelectedDate) : true)
                          && (dp_externalInvoicesEndDate.SelectedDate != null ? (x.invDate <= endDate.SelectedDate) : true)
                          )
                          : true
                          )
            ));

            itemTransferInvoicesLst = result;
            return result;

        }

        private void fillEvents()
        {
            if (selectedExternalTab == 0)
                temp = fillList(itemsTransfer, cb_externalItemsBranches, cb_externalItemsItems, cb_externalItemsUnits, dp_externalItemsStartDate, dp_externalItemsEndDate, chk_externalItemsAllBranches, chk_externalItemsAllItems, chk_externalItemsAllUnits, chk_externalItemsIn, chk_externalItemsOut);
            else if (selectedExternalTab == 1)
                temp = fillList(itemsTransfer, cb_externalAgentsBranches, cb_externalAgentsAgentsType, cb_externalAgentsCustomer, dp_externalAgentsStartDate, dp_externalAgentsEndDate, chk_externalAgentsAllBranches, chk_externalAgentsAllAgentsType, chk_externalAgentsAllCustomers, chk_externalAgentsIn, chk_externalAgentsOut);
            else if (selectedExternalTab == 2)
                temp = fillList(itemsTransfer, cb_externalInvoicesBranches, cb_externalInvoicesInvoiceType, cb_externalInvoicesInvoice, dp_externalInvoicesStartDate, dp_externalInvoicesEndDate, chk_externalInvoicesAllBranches, chk_externalInvoicesAllInvoicesType, chk_externalInvoicesALlInvoice, null, null);

            dgStock.ItemsSource = temp;
            txt_count.Text = temp.Count().ToString();

            fillExternalColumnChart();
            fillExternalPieChart();
            fillExternalRowChart();
        }

        private void cb_externalItemsBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsItems.IsEnabled = true;
                chk_externalItemsAllItems.IsEnabled = true;
                chk_externalItemsAllItems.IsChecked = true;

                fillComboExternalItemsItems();

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalItemsAllBranches_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsBranches.IsEnabled = false;
                cb_externalItemsBranches.SelectedItem = null;
                cb_externalItemsItems.IsEnabled = true;
                chk_externalItemsAllItems.IsEnabled = true;

                fillComboExternalItemsItems();

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalItemsAllBranches_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsBranches.IsEnabled = true;
                cb_externalItemsItems.IsEnabled = false;
                cb_externalItemsItems.SelectedItem = null;
                chk_externalItemsAllItems.IsEnabled = false;
                chk_externalItemsAllItems.IsChecked = false;

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void cb_externalItemsItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsUnits.IsEnabled = true;
                chk_externalItemsAllUnits.IsEnabled = true;

                fillComboExternalItemsUnits();

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalItemsAllItems_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsItems.IsEnabled = false;
                cb_externalItemsItems.SelectedItem = null;
                cb_externalItemsUnits.IsEnabled = true;
                chk_externalItemsAllUnits.IsEnabled = true;

                fillComboExternalItemsUnits();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalItemsAllItems_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsItems.IsEnabled = true;
                cb_externalItemsUnits.IsEnabled = false;
                cb_externalItemsUnits.SelectedItem = null;
                chk_externalItemsAllUnits.IsEnabled = false;
                chk_externalItemsAllUnits.IsChecked = false;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalItemsAllUnits_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsUnits.IsEnabled = false;
                cb_externalItemsUnits.SelectedItem = null;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalItemsAllUnits_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalItemsUnits.IsEnabled = true;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void cb_externalAgentsBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsAgentsType.IsEnabled = true;
                chk_externalAgentsAllAgentsType.IsEnabled = true;

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void cb_externalAgentsAgentsType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsCustomer.IsEnabled = true;
                chk_externalAgentsAllCustomers.IsEnabled = true;

                fillComboExternalAgentsAgents();

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void chk_externalAgentsAllBranches_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsBranches.IsEnabled = false;
                cb_externalAgentsBranches.SelectedItem = null;
                cb_externalAgentsAgentsType.IsEnabled = true;
                chk_externalAgentsAllAgentsType.IsEnabled = true;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalAgentsAllAgentsType_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsAgentsType.IsEnabled = false;
                cb_externalAgentsAgentsType.SelectedItem = null;
                cb_externalAgentsCustomer.IsEnabled = true;
                chk_externalAgentsAllCustomers.IsEnabled = true;

                fillComboExternalAgentsAgents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalAgentsAllCustomers_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsCustomer.IsEnabled = false;
                cb_externalAgentsCustomer.SelectedItem = null;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalAgentsAllBranches_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsBranches.IsEnabled = true;
                cb_externalAgentsAgentsType.IsEnabled = false;
                cb_externalAgentsAgentsType.SelectedItem = null;
                chk_externalAgentsAllAgentsType.IsEnabled = false;
                chk_externalAgentsAllAgentsType.IsChecked = false;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalAgentsAllAgentsType_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsAgentsType.IsEnabled = true;
                cb_externalAgentsCustomer.IsEnabled = false;
                cb_externalAgentsCustomer.SelectedItem = null;
                chk_externalAgentsAllCustomers.IsEnabled = false;
                chk_externalAgentsAllCustomers.IsChecked = false;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalAgentsAllCustomers_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalAgentsCustomer.IsEnabled = true;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void cb_externalInvoicesBranches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesInvoiceType.IsEnabled = true;
                chk_externalInvoicesAllInvoicesType.IsEnabled = true;

                fillComboExternalInvType();

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalInvoicesAllBranches_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesBranches.IsEnabled = false;
                cb_externalInvoicesBranches.SelectedItem = null;
                cb_externalInvoicesInvoiceType.IsEnabled = true;
                chk_externalInvoicesAllInvoicesType.IsEnabled = true;
                fillComboExternalInvType();


               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalInvoicesAllBranches_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesBranches.IsEnabled = true;
                cb_externalInvoicesInvoiceType.IsEnabled = false;
                cb_externalInvoicesInvoiceType.SelectedItem = null;
                chk_externalInvoicesAllInvoicesType.IsEnabled = false;
                chk_externalInvoicesAllInvoicesType.IsChecked = false;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void cb_externalInvoicesInvoiceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesInvoice.IsEnabled = true;
                chk_externalInvoicesALlInvoice.IsEnabled = true;

                fillComboExternalInvoiceInvoice();

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalInvoicesAllInvoicesType_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesInvoiceType.IsEnabled = false;
                cb_externalInvoicesInvoiceType.SelectedItem = null;
                cb_externalInvoicesInvoice.IsEnabled = true;
                chk_externalInvoicesALlInvoice.IsEnabled = true;

                fillComboExternalInvoiceInvoice();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalInvoicesAllInvoicesType_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesInvoiceType.IsEnabled = true;
                cb_externalInvoicesInvoice.IsEnabled = false;
                cb_externalInvoicesInvoice.SelectedItem = null;
                chk_externalInvoicesALlInvoice.IsEnabled = false;
                chk_externalInvoicesALlInvoice.IsChecked = false;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void chk_externalInvoicesALlInvoice_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesInvoice.IsEnabled = false;
                cb_externalInvoicesInvoice.SelectedItem = null;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void chk_externalInvoicesALlInvoice_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                cb_externalInvoicesInvoice.IsEnabled = true;

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void fillExternalRowChart()
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            List<int> pTemp = new List<int>();
            List<int> sTemp = new List<int>();
            List<int> pbTemp = new List<int>();
            List<int> sbTemp = new List<int>();

            //var temp = itemTransferInvoicesLst;

            var result = itemTransferInvoicesLst.GroupBy(x => new { x.branchId, x.invoiceId }).Select(x => new ItemTransferInvoice
            {
                invType = x.FirstOrDefault().invType,
                branchId = x.FirstOrDefault().branchId
            });

            invCount = result.GroupBy(x => x.branchId).Select(x => new ItemTransferInvoice
            {
                PCount = x.Where(g => g.invType == "p").Count(),
                SCount = x.Where(g => g.invType == "s").Count(),
                PbCount = x.Where(g => g.invType == "pb").Count(),
                SbCount = x.Where(g => g.invType == "sb").Count()
            });

            for (int i = 0; i < agentsCount.Count(); i++)
            {
                pTemp.Add(invCount.ToList().Skip(i).FirstOrDefault().PCount);
                pbTemp.Add(invCount.ToList().Skip(i).FirstOrDefault().PbCount);
                sTemp.Add(invCount.ToList().Skip(i).FirstOrDefault().SCount);
                sbTemp.Add(invCount.ToList().Skip(i).FirstOrDefault().SbCount);
            }
            var tempName = temp.GroupBy(s => new { s.branchId, s.invType }).Select(s => new
            {
                locationName = s.FirstOrDefault().branchName
            });
            names.AddRange(tempName.Select(nn => nn.locationName));

            SeriesCollection rowChartData = new SeriesCollection();
            for (int i = 0; i < pTemp.Count(); i++)
            {
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }

            rowChartData.Add(
          new LineSeries
          {
              Values = pTemp.AsChartValues(),
              Title = AppSettings.resourcemanager.GetString("tr_Purchases")

          }); ;
            rowChartData.Add(
      new LineSeries
      {
          Values = pbTemp.AsChartValues(),
          Title = AppSettings.resourcemanager.GetString("trPurchaseReturnInvoice")
      });
            rowChartData.Add(
      new LineSeries
      {
          Values = sTemp.AsChartValues(),
          Title = AppSettings.resourcemanager.GetString("tr_Sales")
      });
            rowChartData.Add(
      new LineSeries
      {
          Values = sbTemp.AsChartValues(),
          Title = AppSettings.resourcemanager.GetString("trSalesReturnInvoice")

      });
            rowChart.Series = rowChartData;
            DataContext = this;
        }

        private void fillExternalColumnChart()
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();

            //var temp = itemTransferInvoicesLst;
            var res = itemTransferInvoicesLst.GroupBy(x => new { x.branchId, x.agentId }).Select(x => new ItemTransferInvoice
            {
                agentId = x.FirstOrDefault().agentId,
                branchId = x.FirstOrDefault().branchId,
                agentType = x.FirstOrDefault().agentType,
                branchName = x.FirstOrDefault().branchName
            });
            agentsCount = res.GroupBy(x => x.branchId).Select(x => new ItemTransferInvoice
            {
                VenCount = x.Where(g => g.agentType == "v").Count(),
                CusCount = x.Where(g => g.agentType == "c").Count()
            }
            );

            var tempName = res.GroupBy(s => new { s.branchId }).Select(s => new
            {
                itemName = s.FirstOrDefault().branchName,
            });
            names.AddRange(tempName.Select(nn => nn.itemName));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> cP = new List<int>();
            List<int> cPb = new List<int>();

            int xCount = 6;
            if (agentsCount.Count() <= 6) xCount = agentsCount.Count();

            for (int i = 0; i < xCount; i++)
            {
                cP.Add(agentsCount.ToList().Skip(i).FirstOrDefault().VenCount);
                cPb.Add(agentsCount.ToList().Skip(i).FirstOrDefault().CusCount);
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            if (agentsCount.Count() > 6)
            {
                int c = 0, v = 0;
                for (int i = 6; i < agentsCount.Count(); i++)
                {
                    v = v + agentsCount.ToList().Skip(i).FirstOrDefault().VenCount;
                    c = c + agentsCount.ToList().Skip(i).FirstOrDefault().CusCount;
                }
                if (!((c == 0) && (v == 0)))
                {
                    cP.Add(v);
                    cPb.Add(c);
                    axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cP.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("tr_Vendor")
            }); ;
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cPb.AsChartValues(),
                DataLabels = true,
                Title = AppSettings.resourcemanager.GetString("tr_Customer")
            });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillExternalPieChart()
        {
            List<string> titles = new List<string>();
            List<string> titles1 = new List<string>();
            List<decimal> x = new List<decimal>();
            titles.Clear();
            titles1.Clear();

            // var temp = itemTransferInvoicesLst;
            var result = itemTransferInvoicesLst
                .GroupBy(s => new { s.itemId, s.unitId })
                .Select(s => new ItemTransferInvoice
                {
                    itemId = s.FirstOrDefault().itemId,
                    unitId = s.FirstOrDefault().unitId,
                    quantity = s.Sum(g => g.quantity),
                    itemName = s.FirstOrDefault().itemName,
                    unitName = s.FirstOrDefault().unitName,
                }).OrderByDescending(s => s.quantity);
            x = result.Select(m => (decimal)m.quantity).ToList();
            titles = result.Select(m => m.itemName).ToList();
            titles1 = result.Select(m => m.unitName).ToList();
            int count = x.Count();
            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < count; i++)
            {
                List<decimal> final = new List<decimal>();
                List<string> lable = new List<string>();
                if (i < 5)
                {
                    final.Add(x.Max());
                    lable.Add(titles.Skip(i).FirstOrDefault() + titles1.Skip(i).FirstOrDefault());
                    piechartData.Add(
                      new PieSeries
                      {
                          Values = final.AsChartValues(),
                          Title = lable.FirstOrDefault(),
                          DataLabels = true,
                      }
                  );
                    x.Remove(x.Max());
                }
                else
                {
                    final.Add(x.Sum());
                    piechartData.Add(
                      new PieSeries
                      {
                          Values = final.AsChartValues(),
                          Title = AppSettings.resourcemanager.GetString("trOthers"),
                          DataLabels = true,
                      }
                  ); ;
                    break;
                }

            }
            chart1.Series = piechartData;
        }

        private void hideAllColumn()
        {
            col_branch.Visibility = Visibility.Hidden;
            col_item.Visibility = Visibility.Hidden;
            col_unit.Visibility = Visibility.Hidden;
            col_quantity.Visibility = Visibility.Hidden;
            col_itemUnits.Visibility = Visibility.Hidden;
            col_invNumber.Visibility = Visibility.Hidden;
            col_invTypeNumber.Visibility = Visibility.Hidden;
            col_agentType.Visibility = Visibility.Hidden;
            col_agent.Visibility = Visibility.Hidden;
            col_agentTypeAgent.Visibility = Visibility.Hidden;
        }
        private void showSelectedTabColumn()
        {

            hideAllColumn();

            if (selectedExternalTab == 0)
            {
                col_branch.Visibility = Visibility.Visible;
                col_item.Visibility = Visibility.Visible;
                col_unit.Visibility = Visibility.Visible;
                col_quantity.Visibility = Visibility.Visible;
                col_agentTypeAgent.Visibility = Visibility.Visible;
                col_invTypeNumber.Visibility = Visibility.Visible;
                col_invNumber.Visibility = Visibility.Visible;
                col_invDate.Visibility = Visibility.Visible;
            }
            else if (selectedExternalTab == 1)
            {
                col_branch.Visibility = Visibility.Visible;
                col_itemUnits.Visibility = Visibility.Visible;
                col_agent.Visibility = Visibility.Visible;
                col_quantity.Visibility = Visibility.Visible;
                col_agentType.Visibility = Visibility.Visible;
                col_invTypeNumber.Visibility = Visibility.Visible;
                col_invNumber.Visibility = Visibility.Visible;
                col_invDate.Visibility = Visibility.Visible;
            }
            else if (selectedExternalTab == 2)
            {
                col_branch.Visibility = Visibility.Visible;
                col_invNumber.Visibility = Visibility.Visible;
                col_invDate.Visibility = Visibility.Visible;
                col_quantity.Visibility = Visibility.Visible;
                col_agentTypeAgent.Visibility = Visibility.Visible;
                col_itemUnits.Visibility = Visibility.Visible;
            }

        }

        IEnumerable<ItemTransferInvoice> temp = null;
        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                if (selectedExternalTab == 0)
                {
                    temp = itemTransferInvoicesLst

                .Where(s => (s.branchName.Contains(txt_search.Text) ||
           s.itemName.Contains(txt_search.Text) ||
           s.unitName.Contains(txt_search.Text) ||
           s.agentName.Contains(txt_search.Text) ||
           s.agentType.Contains(txt_search.Text) ||
           s.invNumber.Contains(txt_search.Text) ||
           s.invType.Contains(txt_search.Text)
           ));
                    dgStock.ItemsSource = temp;
                    txt_count.Text = temp.Count().ToString();
                    fillExternalPieChart();
                }
                else if (selectedExternalTab == 1)
                {

                    temp = itemTransferInvoicesLst

                  .Where(s => (s.branchName.Contains(txt_search.Text) ||
           s.itemName.Contains(txt_search.Text) ||
           s.unitName.Contains(txt_search.Text) ||
           s.agentName.Contains(txt_search.Text) ||
           s.agentType.Contains(txt_search.Text) ||
           s.invNumber.Contains(txt_search.Text) ||
           s.invType.Contains(txt_search.Text)
           ));
                    dgStock.ItemsSource = temp;
                    txt_count.Text = temp.Count().ToString();
                    fillExternalPieChart();
                }


                else
                {
                    temp = itemTransferInvoicesLst
                .GroupBy(x => new { x.branchId, x.invoiceId })
                             .Select(s => new ItemTransferInvoice
                             {
                                 branchId = s.FirstOrDefault().branchId,
                                 branchName = s.FirstOrDefault().branchName,
                                 AgentTypeAgent = s.FirstOrDefault().AgentTypeAgent,
                                 ItemUnits = s.FirstOrDefault().ItemUnits
                               ,
                                 invNumber = s.FirstOrDefault().invNumber,
                                 invType = s.FirstOrDefault().invType,
                                 quantity = s.FirstOrDefault().quantity
                             })
                .Where(s => (s.branchName.Contains(txt_search.Text) ||
           s.ItemUnits.Contains(txt_search.Text) ||
           s.AgentTypeAgent.Contains(txt_search.Text) ||
           s.invNumber.Contains(txt_search.Text) ||
           s.invType.Contains(txt_search.Text)
           ));
                    dgStock.ItemsSource = temp;
                    txt_count.Text = temp.Count().ToString();
                    fillExternalPieChart();
                }



               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
               
                    HelpClass.StartAwait(grid_main);
                txt_search.Text = "";
                if (selectedExternalTab == 0)
                {
                    cb_externalItemsBranches.SelectedItem = null;
                    cb_externalItemsItems.SelectedItem = null;
                    cb_externalItemsUnits.SelectedItem = null;
                    chk_externalItemsAllBranches.IsChecked = false;
                    chk_externalItemsAllItems.IsChecked = false;
                    chk_externalItemsAllUnits.IsChecked = false;
                    chk_externalItemsIn.IsChecked = true;
                    chk_externalItemsOut.IsChecked = true;
                    dp_externalItemsEndDate.SelectedDate = null;
                    dp_externalItemsStartDate.SelectedDate = null;
                }
                else if (selectedExternalTab == 1)
                {
                    cb_externalAgentsBranches.SelectedItem = null;
                    cb_externalAgentsAgentsType.SelectedItem = null;
                    cb_externalAgentsCustomer.SelectedItem = null;
                    chk_externalAgentsAllBranches.IsChecked = false;
                    chk_externalAgentsAllAgentsType.IsChecked = false;
                    chk_externalAgentsAllCustomers.IsChecked = false;
                    chk_externalAgentsIn.IsChecked = true;
                    chk_externalAgentsOut.IsChecked = true;
                    dp_externalAgentsEndDate.SelectedDate = null;
                    dp_externalAgentsStartDate.SelectedDate = null;
                }

                else if (selectedExternalTab == 2)
                {
                    cb_externalInvoicesBranches.SelectedItem = null;
                    cb_externalInvoicesInvoice.SelectedItem = null;
                    cb_externalInvoicesInvoiceType.SelectedItem = null;
                    chk_externalInvoicesAllBranches.IsChecked = false;
                    chk_externalInvoicesALlInvoice.IsChecked = false;
                    chk_externalInvoicesAllInvoicesType.IsChecked = false;

                    dp_externalInvoicesEndDate.SelectedDate = null;
                    dp_externalInvoicesStartDate.SelectedDate = null;
                }

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();


        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath = "";
            string firstTitle = "external";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (selectedExternalTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Storage\Ar\ArExternalItem.rdlc";
                    secondTitle = "items";

                }
                else if (selectedExternalTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Storage\Ar\ArExternalAgent.rdlc";
                    secondTitle = "customers";
                }
                else if (selectedExternalTab == 2)
                {
                    addpath = @"\Reports\StatisticReport\Storage\Ar\ArExternalInvoice.rdlc";
                    secondTitle = "invoice";
                }
            }
            else
            {
                if (selectedExternalTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Storage\En\EnExternalItem.rdlc";
                    secondTitle = "items";
                }
                else if (selectedExternalTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Storage\En\EnExternalAgent.rdlc";
                    secondTitle = "customers";
                }
                else if (selectedExternalTab == 2)
                {
                    addpath = @"\Reports\StatisticReport\Storage\En\EnExternalInvoice.rdlc";
                    secondTitle = "invoice";
                }
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
            Title = AppSettings.resourcemanagerreport.GetString("trStorageReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));

            //itemTransferInvoiceExternal
            clsReports.itemTransferInvoiceExternal(temp, rep, reppath, paramarr);

            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();


        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                #region
                BuildReport();

                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                #region
                BuildReport();
                LocalReportExtensions.PrintToPrinter(rep);
                #endregion

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                #region
                //Thread t1 = new Thread(() =>
                //  {
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


                //});
                //  t1.Start();

                #endregion

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                #region
                Window.GetWindow(this).Opacity = 0.2;
                string pdfpath = "";



                pdfpath = @"\Thumb\report\temp.pdf";
                pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                BuildReport();

                LocalReportExtensions.ExportToPDF(rep, pdfpath);
                wd_previewPdf w = new wd_previewPdf();
                w.pdfPath = pdfpath;
                if (!string.IsNullOrEmpty(w.pdfPath))
                {
                    w.ShowDialog();
                    w.wb_pdfWebViewer.Dispose();
                }
                Window.GetWindow(this).Opacity = 1;
                #endregion

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void selectionChanged(object sender, RoutedEventArgs e)
        {
            fillEventsCall(sender);
        }

        private void fillEventsCall(object sender)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);

                fillEvents();

               
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
               
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillEventsCall(sender);
        }

      


        Invoice invoice;
        private async void DgStock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
               
                    HelpClass.StartAwait(grid_main);
                invoice = new Invoice();
                if (dgStock.SelectedIndex != -1)
                {
                    ItemTransferInvoice item = dgStock.SelectedItem as ItemTransferInvoice;
                    if (item.invoiceId > 0)
                    {
                        invoice = await invoice.GetByInvoiceId(item.invoiceId);
                        MainWindow.mainWindow.Btn_storage_Click(MainWindow.mainWindow.btn_storage, null);

                        //View.uc_storage.Instance.UserControl_Loaded(null, null);
                        //View.uc_storage.Instance.Btn_itemsExport_Click(View.uc_storage.Instance.btn_importExport, null);

                        MainWindow.mainWindow.grid_main.Children.Clear();
                        MainWindow.mainWindow.grid_main.Children.Add(uc_storageMovements.Instance);

                        MainWindow.mainWindow.initializationMainTrack("storageMovements");
                        //uc_storageMovements.Instance.UserControl_Loaded(uc_storageMovements.Instance, null);
                        uc_storageMovements._ProcessType = invoice.invType;
                        uc_storageMovements.Instance.invoice = invoice;
                        uc_storageMovements.isFromReport = true;
                        await uc_storageMovements.Instance.fillOrderInputs(invoice);
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
    }
}
