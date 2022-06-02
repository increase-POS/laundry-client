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
using System.IO;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System.Threading;
using laundryApp.View.purchase;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace laundryApp.View.reports.purchaseReports
{
    /// <summary>
    /// Interaction logic for uc_itemPurchaseReports.xaml
    /// </summary>
    public partial class uc_itemPurchaseReports : UserControl
    {
        private static uc_itemPurchaseReports _instance;
        public static uc_itemPurchaseReports Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_itemPurchaseReports();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_itemPurchaseReports()
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

        //prin & pdf
        private int selectedTab = 0;
        Statistics statisticModel = new Statistics();
        List<ItemTransferInvoice> Items;

        //for combo boxes
        /*************************/
        Branch selectedBranch;
        ItemUnitCombo selectedItem;

        List<Branch> comboBranches;
        List<ItemUnitCombo> itemUnitCombos;

        ObservableCollection<Branch> comboBrachTemp = new ObservableCollection<Branch>();
        ObservableCollection<ItemUnitCombo> comboItemTemp = new ObservableCollection<ItemUnitCombo>();

        ObservableCollection<Branch> dynamicComboBranches;
        ObservableCollection<ItemUnitCombo> dynamicComboItem;

        Branch branchModel = new Branch();
        /*************************/
        // report
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        IEnumerable<ItemTransferInvoice> RepQuery;

        ObservableCollection<int> selectedBranchId = new ObservableCollection<int>();

        ObservableCollection<int> selectedItemId = new ObservableCollection<int>();

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                Items = await statisticModel.GetPuritem((int)MainWindow.branchLogin.branchId, (int)MainWindow.userLogin.userId);

                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                translate();
                #endregion

                comboBranches = await branchModel.GetAllWithoutMain("b");

                itemUnitCombos = statisticModel.GetIUComboList(Items);

                dynamicComboBranches = new ObservableCollection<Branch>(comboBranches);
                dynamicComboItem = new ObservableCollection<ItemUnitCombo>(itemUnitCombos);

                fillComboBranches(cb_collect);
                fillComboItemsBranches(cb_ItemsBranches);
                fillComboItems();

                chk_itemInvoice.IsChecked = true;

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), btn_items.Tag.ToString());

                
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
            tt_items.Content = AppSettings.resourcemanager.GetString("trItems");
            tt_collect.Content = AppSettings.resourcemanager.GetString("trMostPurchased");

            chk_itemInvoice.Content = AppSettings.resourcemanager.GetString("tr_Invoice");
            chk_itemReturn.Content = AppSettings.resourcemanager.GetString("trReturned");
            chk_itemDrafs.Content = AppSettings.resourcemanager.GetString("trDraft");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ItemsBranches, AppSettings.resourcemanager.GetString("trBranchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_Items, AppSettings.resourcemanager.GetString("trItemHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_collect, AppSettings.resourcemanager.GetString("trBranchHint"));

            chk_allcollect.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allBranchesItem.Content = AppSettings.resourcemanager.GetString("trAll");
            chk_allItems.Content = AppSettings.resourcemanager.GetString("trAll");


            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ItemEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_ItemStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_collectEndDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_collectStartDate, AppSettings.resourcemanager.GetString("trStartDateHint"));

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txt_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");

            col_number.Header = AppSettings.resourcemanager.GetString("trNo.");
            col_date.Header = AppSettings.resourcemanager.GetString("trDate");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_item.Header = AppSettings.resourcemanager.GetString("trItem");
            col_unit.Header = AppSettings.resourcemanager.GetString("trUnit");
            col_itQuantity.Header = AppSettings.resourcemanager.GetString("trQTR");
            col_invCount.Header = AppSettings.resourcemanager.GetString("trInvoices");
            col_price.Header = AppSettings.resourcemanager.GetString("trPrice");
            col_total.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_avg.Header = AppSettings.resourcemanager.GetString("trItem") + "/" + AppSettings.resourcemanager.GetString("tr_Invoice");

            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

        }

        private void fillComboBranches(ComboBox cb)
        {
            cb.SelectedValuePath = "branchId";
            cb.DisplayMemberPath = "name";
            cb.ItemsSource = dynamicComboBranches;
        }
        private void fillComboItemsBranches(ComboBox cb)
        {
            cb.SelectedValuePath = "branchId";
            cb.DisplayMemberPath = "name";
            cb.ItemsSource = comboBranches;
        }

        private void fillComboItems()
        {
            cb_Items.SelectedValuePath = "itemUnitId";
            cb_Items.DisplayMemberPath = "itemUnitName";
            cb_Items.ItemsSource = dynamicComboItem;
        }

        public void fillItemsEvent()
        {
            RepQuery = fillList(Items, cb_ItemsBranches, cb_Items, chk_itemInvoice, chk_itemReturn, dp_ItemStartDate, dp_ItemEndDate)
                .Where(j => (selectedItemId.Count != 0 ? selectedItemId.Contains((int)j.ITitemUnitId) : true));

            fillPieChart(cb_Items, selectedItemId);
            fillColumnChart(cb_Items, selectedItemId);
            fillRowChart(cb_Items, selectedItemId);

            dgInvoice.ItemsSource = RepQuery;
            txt_count.Text = dgInvoice.Items.Count.ToString();

        }

        public void fillCollectEvent()
        {
            RepQuery = fillCollectListBranch(Items, dp_collectStartDate, dp_collectEndDate)
                .Where(j => (selectedBranchId.Count != 0 ? selectedBranchId.Contains((int)j.branchCreatorId) : true));

            txt_count.Text = dgInvoice.Items.Count.ToString();
            fillPieChartCollect(cb_collect, selectedBranchId);
            fillColumnChartCollect(cb_collect, selectedBranchId);
            fillRowChartCollect(cb_collect, selectedBranchId);
            dgInvoice.ItemsSource = RepQuery;

        }

        public void fillCollectEventAll()
        {
            RepQuery = fillCollectListAll(Items, dp_collectStartDate, dp_collectEndDate);

            txt_count.Text = dgInvoice.Items.Count.ToString();
            fillPieChartCollect(cb_collect, selectedBranchId);
            fillColumnChartCollect(cb_collect, selectedBranchId);
            fillRowChartCollect(cb_collect, selectedBranchId);
            dgInvoice.ItemsSource = RepQuery;
        }

        private void btn_items_Click(object sender, RoutedEventArgs e)
        {//items
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                selectedTab = 0;
                txt_search.Text = "";

                ReportsHelp.showSelectedStack(grid_stacks, stk_tagsItems);

                path_collect.Fill = Brushes.White;

                bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_items);
                path_items.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                ReportsHelp.showTabControlGrid(grid_father, grid_items);

                ReportsHelp.isEnabledButtons(grid_tabControl, btn_items);

                ReportsHelp.hideAllColumns(dgInvoice);

                col_number.Visibility = Visibility.Visible;
                col_date.Visibility = Visibility.Visible;
                col_branch.Visibility = Visibility.Visible;
                col_item.Visibility = Visibility.Visible;
                col_unit.Visibility = Visibility.Visible;
                col_itQuantity.Visibility = Visibility.Visible;
                col_price.Visibility = Visibility.Visible;
                col_total.Visibility = Visibility.Visible;

                fillItemsEvent();

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_collect_Click(object sender, RoutedEventArgs e)
        {//collect
            try
            {
                
                HelpClass.StartAwait(grid_main);

                selectedTab = 1;
                txt_search.Text = "";

                path_items.Fill = Brushes.White;
                path_collect.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;

                bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);

                ReportsHelp.showSelectedStack(grid_stacks, stk_tagsBranches);
                ReportsHelp.paintTabControlBorder(grid_tabControl, bdr_collect);
                ReportsHelp.showTabControlGrid(grid_father, grid_collect);
                ReportsHelp.isEnabledButtons(grid_tabControl, btn_collect);
                ReportsHelp.hideAllColumns(dgInvoice);

                col_branch.Visibility = Visibility.Visible;
                col_item.Visibility = Visibility.Visible;
                col_unit.Visibility = Visibility.Visible;
                col_itQuantity.Visibility = Visibility.Visible;
                col_invCount.Visibility = Visibility.Visible;
                col_avg.Visibility = Visibility.Visible;

                if (stk_tagsBranches.Children.Count == 0)
                {
                    fillCollectEventAll();
                }
                else
                {
                    fillCollectEvent();
                }

                HelpClass.ReportTabTitle(txt_tabTitle, this.Tag.ToString(), (sender as Button).Tag.ToString());

                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region Items Events
        private void Chip_OnDeleteItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                var currentChip = (Chip)sender;
                stk_tagsItems.Children.Remove(currentChip);
                var m = comboItemTemp.Where(j => j.itemUnitId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                dynamicComboItem.Add(m.FirstOrDefault());
                selectedItemId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                if (selectedItemId.Count == 0)
                {
                    cb_Items.SelectedItem = null;
                }
                fillItemsEvent();
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void cb_Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (cb_Items.SelectedItem != null)
                {
                    if (stk_tagsItems.Children.Count < 5)
                    {
                        selectedItem = cb_Items.SelectedItem as ItemUnitCombo;
                        var b = new MaterialDesignThemes.Wpf.Chip()
                        {
                            Content = selectedItem.itemUnitName,
                            Name = "btn" + selectedItem.itemUnitId.ToString(),
                            IsDeletable = true,
                            Margin = new Thickness(5, 0, 5, 0)
                        };
                        b.DeleteClick += Chip_OnDeleteItemClick;
                        stk_tagsItems.Children.Add(b);
                        comboItemTemp.Add(selectedItem);
                        selectedItemId.Add(selectedItem.itemUnitId);
                        dynamicComboItem.Remove(selectedItem);
                        fillItemsEvent();
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

        private void chk_allItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main); if (cb_Items.IsEnabled == true)
                {
                    cb_Items.SelectedItem = null;
                    cb_Items.IsEnabled = false;
                    for (int i = 0; i < comboItemTemp.Count; i++)
                    {
                        dynamicComboItem.Add(comboItemTemp.Skip(i).FirstOrDefault());
                    }
                    comboItemTemp.Clear();
                    stk_tagsItems.Children.Clear();
                    selectedItemId.Clear();
                }
                else
                {
                    cb_Items.IsEnabled = true;
                }

                fillItemsEvent();
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        //private void chk_itemDrafs_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        
        //            HelpClass.StartAwait(grid_main);
        //        fillItemsEvent();
        //        
        //            HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //            HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        //private void chk_itemDrafs_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        
        //            HelpClass.StartAwait(grid_main); fillItemsEvent();
        //        
        //            HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //            HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        //private void dt_ItemEndTime_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        //{
        //    try
        //    {
        //        
        //            HelpClass.StartAwait(grid_main);

        //        fillItemsEvent();

        //        
        //            HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //            HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        //private void dt_itemStartTime_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        //{
        //    try
        //    {
        //        
        //            HelpClass.StartAwait(grid_main);

        //        fillItemsEvent();

        //        
        //            HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        
        //            HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}

        private void selectionChangedCall(object sender)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                fillItemsEvent();
                
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Chk_allBranchesItem_Checked(object sender, RoutedEventArgs e)
        {

            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_ItemsBranches.SelectedItem = null;
                cb_ItemsBranches.IsEnabled = false;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allBranchesItem_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_ItemsBranches.IsEnabled = true;

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allItems_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_Items.SelectedItem = null;
                cb_Items.IsEnabled = false;

                for (int i = 0; i < comboItemTemp.Count; i++)
                {
                    dynamicComboItem.Add(comboItemTemp.Skip(i).FirstOrDefault());
                }
                comboItemTemp.Clear();
                stk_tagsItems.Children.Clear();
                selectedItemId.Clear();
                fillItemsEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allItems_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_Items.IsEnabled = true;
                fillItemsEvent();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        private void col_selectionChangedCall(object sender)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                fillCollectEvent();

                if (stk_tagsBranches.Children.Count == 0)
                {
                    fillCollectEventAll();
                }

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chip_OnDeleteBranchClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                var currentChip = (Chip)sender;
                stk_tagsBranches.Children.Remove(currentChip);
                var m = comboBrachTemp.Where(j => j.branchId == (Convert.ToInt32(currentChip.Name.Remove(0, 3))));
                dynamicComboBranches.Add(m.FirstOrDefault());
                selectedBranchId.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
                if (selectedBranchId.Count == 0)
                {
                    cb_collect.SelectedItem = null;
                }
                fillCollectEvent();
                if (stk_tagsBranches.Children.Count == 0)
                {
                    fillCollectEventAll();
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_collect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (cb_collect.SelectedItem != null)
                {
                    if (stk_tagsBranches.Children.Count < 5)
                    {
                        selectedBranch = cb_collect.SelectedItem as Branch;
                        var b = new MaterialDesignThemes.Wpf.Chip()
                        {
                            Content = selectedBranch.name,
                            Name = "btn" + selectedBranch.branchId.ToString(),
                            IsDeletable = true,
                            Margin = new Thickness(5, 0, 5, 0)
                        };
                        b.DeleteClick += Chip_OnDeleteBranchClick;
                        stk_tagsBranches.Children.Add(b);
                        comboBrachTemp.Add(selectedBranch);
                        selectedBranchId.Add(selectedBranch.branchId);
                        dynamicComboBranches.Remove(selectedBranch);
                        fillCollectEvent();
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

        private void Chk_allcollect_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_collect.SelectedItem = null;
                cb_collect.IsEnabled = false;
                for (int i = 0; i < comboBrachTemp.Count; i++)
                {
                    dynamicComboBranches.Add(comboBrachTemp.Skip(i).FirstOrDefault());
                }
                comboBrachTemp.Clear();
                stk_tagsBranches.Children.Clear();
                selectedBranchId.Clear();
                fillCollectEvent();
                if (stk_tagsBranches.Children.Count == 0)
                {
                    fillCollectEventAll();
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Chk_allcollect_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                cb_collect.IsEnabled = true;
                fillCollectEvent();
                if (stk_tagsBranches.Children.Count == 0)
                {
                    fillCollectEventAll();
                }

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #region fillLists
        List<ItemTransferInvoice> invLst;
        private List<ItemTransferInvoice> fillList(IEnumerable<ItemTransferInvoice> Invoices, ComboBox cbBranch, ComboBox cbitem, CheckBox chkInvoice, CheckBox chkReturn, DatePicker startDate, DatePicker endDate)
        {
            var selectedBranch = cbBranch.SelectedItem as Branch;
            var selectedItemUnit = cbitem.SelectedItem as ItemUnitCombo;

            var result = Invoices.Where(x => (

           ((chkReturn.IsChecked == true ? (x.invType == "pb") : false) || (chkInvoice.IsChecked == true ? (x.invType == "p") : false))
                      && (cbBranch.SelectedItem != null ? x.branchCreatorId == selectedBranch.branchId : true)
                      && (cb_Items.SelectedItem != null ? x.itemUnitId == selectedItemUnit.itemUnitId : true)
                      && (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
                      && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true)));

            invLst = result.ToList();
            return result.ToList();
        }

        private List<ItemTransferInvoice> fillCollectListBranch(IEnumerable<ItemTransferInvoice> Invoices, DatePicker startDate, DatePicker endDate)
        {

            var temp = Invoices
                .Where(x =>
                 (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
                && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true))
                .GroupBy(obj => new
                {
                    obj.branchCreatorId,
                    obj.ITitemUnitId
                }).Select(obj => new ItemTransferInvoice
                {
                    branchCreatorId = obj.FirstOrDefault().branchCreatorId,
                    branchCreatorName = obj.FirstOrDefault().branchCreatorName,
                    itemUnitId = obj.FirstOrDefault().itemUnitId,
                    ItemUnits = obj.FirstOrDefault().ItemUnits,
                    invoiceId = obj.FirstOrDefault().invoiceId,
                    ITquantity = obj.Sum(x => x.ITquantity),
                    ITitemName = obj.FirstOrDefault().ITitemName,
                    ITitemId = obj.FirstOrDefault().ITitemId,
                    ITunitName = obj.FirstOrDefault().ITunitName,
                    ITunitId = obj.FirstOrDefault().ITunitId,
                    invDate = obj.FirstOrDefault().invDate,
                    itemAvg = obj.Average(x => x.ITquantity),
                    count = obj.Count()
                }).OrderByDescending(obj => obj.ITquantity);

            invLst = temp.ToList();
            return temp.ToList();
        }

        private List<ItemTransferInvoice> fillCollectListAll(IEnumerable<ItemTransferInvoice> Invoices, DatePicker startDate, DatePicker endDate)
        {
            var temp = Invoices
                .Where(x =>
                 (startDate.SelectedDate != null ? x.invDate >= startDate.SelectedDate : true)
                && (endDate.SelectedDate != null ? x.invDate <= endDate.SelectedDate : true))
                .GroupBy(obj => new
                {
                    obj.ITitemUnitId
                }).Select(obj => new ItemTransferInvoice
                {
                    branchCreatorId = obj.FirstOrDefault().branchCreatorId,
                    branchCreatorName = "All Branches",
                    itemUnitId = obj.FirstOrDefault().itemUnitId,
                    ItemUnits = obj.FirstOrDefault().ItemUnits,
                    invoiceId = obj.FirstOrDefault().invoiceId,
                    ITquantity = obj.Sum(x => x.ITquantity),
                    ITitemName = obj.FirstOrDefault().ITitemName,
                    ITitemId = obj.FirstOrDefault().ITitemId,
                    ITunitName = obj.FirstOrDefault().ITunitName,
                    ITunitId = obj.FirstOrDefault().ITunitId,
                    itemAvg = obj.Average(x => x.ITquantity),
                    invDate = obj.FirstOrDefault().invDate,
                    count = obj.Count()
                }).OrderByDescending(obj => obj.ITquantity);

            invLst = temp.ToList();
            return temp.ToList();
        }
        #endregion

        #region General Events


        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string firstTitle = "purchaseItem";
            string secondTitle = "";
            string subTitle = "";
            string Title = "";

            string addpath = "";
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\Ar\ArItem.rdlc";
                    secondTitle = "items";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\Ar\ArItemMostPur.rdlc";
                    secondTitle = "MostPurchased";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }


            }
            else
            {
                //english
                if (selectedTab == 0)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\En\EnItem.rdlc";
                    secondTitle = "items";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }
                else if (selectedTab == 1)
                {
                    addpath = @"\Reports\StatisticReport\Purchase\En\EnItemMostPur.rdlc";
                    secondTitle = "MostPurchased";
                    subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);
                }

            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //  getpuritemcount
            Title = AppSettings.resourcemanagerreport.GetString("trPurchasesReport") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));

         
            clsReports.PurItemStsReport(RepQuery, rep, reppath,paramarr);

            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                #region
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

                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
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
                    // w.ShowInTaskbar = false;
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
        #endregion

        private void fillPieChartCollect(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            List<string> titles = new List<string>();
            List<long> x = new List<long>();
            titles.Clear();
            var temp = invLst;
            if (stk_tagsBranches.Children.Count > 0)
            {
                temp = temp
                 .Where(j => (selectedBranchId.Count != 0 ? selectedBranchId.Contains((int)j.branchCreatorId) : true)).ToList();
            }

            var titleTemp = temp.Select(obj => obj.ITitemUnitName1);
            titles.AddRange(titleTemp);
            x = temp.Select(m => (long)m.ITquantity).ToList();
            int count = x.Count();
            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < count; i++)
            {
                List<long> final = new List<long>();
                List<string> lable = new List<string>();
                if (i < 5)
                {
                    final.Add(x.Max());
                    lable.Add(titles.Skip(i).FirstOrDefault());
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
                  );
                    break;
                }

            }
            chart1.Series = piechartData;
        }

        private void fillColumnChartCollect(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            List<int> x = new List<int>();

            var temp = invLst;
            if (stk_tagsBranches.Children.Count > 0)
            {
                temp = temp
                .Where(j => (selectedBranchId.Count != 0 ? selectedBranchId.Contains((int)j.branchCreatorId) : true)).ToList();
            }

            x = temp.Select(m => m.count).ToList();

            var tempName = temp.OrderByDescending(obj => obj.count).Select(obj => obj.ITitemUnitName1);
            names.AddRange(tempName);

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> cP = new List<int>();

            List<string> titles = new List<string>()
            {
               AppSettings.resourcemanager.GetString("trInvoices")
            };
            int count = x.Count();
            for (int i = 0; i < count; i++)
            {
                if (i < 5)
                {
                    cP.Add(x.Max());
                    x.Remove(x.Max());
                }
                else
                {
                    cP.Add(x.Sum());
                    break;
                }
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());

            }
            axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
            //3 فوق بعض
            columnChartData.Add(
            new ColumnSeries
            {
                Values = cP.AsChartValues(),
                Title = titles[0],
                DataLabels = true,
            });

            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillRowChartCollect(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            int endYear = DateTime.Now.Year;
            int startYear = endYear - 1;
            int startMonth = DateTime.Now.Month;
            int endMonth = startMonth;
            if (dp_collectStartDate.SelectedDate != null && dp_collectEndDate.SelectedDate != null)
            {
                startYear = dp_collectStartDate.SelectedDate.Value.Year;
                endYear = dp_collectEndDate.SelectedDate.Value.Year;
                startMonth = dp_collectStartDate.SelectedDate.Value.Month;
                endMonth = dp_collectEndDate.SelectedDate.Value.Month;
            }
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            List<CashTransferSts> resultList = new List<CashTransferSts>();

            var temp = invLst;

            if (stk_tagsBranches.Children.Count > 0)
            {
                temp = temp
                .Where(j => (selectedBranchId.Count != 0 ? selectedBranchId.Contains((int)j.branchCreatorId) : true)).ToList();
            }

            SeriesCollection rowChartData = new SeriesCollection();
            var tempName = temp.Select(s => s.invDate);
            names.Add("x");

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<long> cash = new List<long>();


            if (endYear - startYear <= 1)
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = startMonth; month <= 12; month++)
                    {
                        var firstOfThisMonth = new DateTime(year, month, 1);
                        var firstOfNextMonth = firstOfThisMonth.AddMonths(1);
                        var drawCash = temp.ToList().Where(c => c.invDate > firstOfThisMonth && c.invDate <= firstOfNextMonth).Sum(obj => (long)obj.ITquantity);
                        cash.Add(drawCash);
                        MyAxis.Labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + "/" + year);

                        if (year == endYear && month == endMonth)
                        {
                            break;
                        }
                        if (month == 12)
                        {
                            startMonth = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int year = startYear; year <= endYear; year++)
                {
                    var firstOfThisYear = new DateTime(year, 1, 1);
                    var firstOfNextMYear = firstOfThisYear.AddYears(1);
                    var drawCash = temp.ToList().Where(c => c.invDate > firstOfThisYear && c.invDate <= firstOfNextMYear).Sum(obj => (long)obj.ITquantity);
                    cash.Add(drawCash);
                    MyAxis.Labels.Add(year.ToString());
                }
            }
            rowChartData.Add(
          new LineSeries
          {
              Values = cash.AsChartValues(),
              Title = AppSettings.resourcemanager.GetString("trQuantity")
          }); ;

            DataContext = this;
            rowChart.Series = rowChartData;
        }

        #region Charts
        private void fillPieChart(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            List<string> titles = new List<string>();
            List<int> x = new List<int>();
            titles.Clear();

            var temp = invLst
             .Where(j => (selectedItemId.Count != 0 ? selectedItemId.Contains((int)j.ITitemUnitId) : true));
            var titleTemp = temp.GroupBy(jj => jj.ITitemUnitId)
             .Select(g => new ItemUnitCombo
             {
                 itemUnitId = (int)g.FirstOrDefault().ITitemUnitId,
                 itemUnitName = g.FirstOrDefault().ITitemName + "-" + g.FirstOrDefault().ITunitName
             }).ToList();

            titles.AddRange(titleTemp.Select(jj => jj.itemUnitName));
            var result = temp.GroupBy(s => s.ITitemUnitId).Select(s => new
            {
                ITitemUnitId = s.Key,
                count = s.Count()
            });
            x = result.Select(m => m.count).ToList();
            int count = x.Count();
            SeriesCollection piechartData = new SeriesCollection();
            for (int i = 0; i < count; i++)
            {
                List<long> final = new List<long>();
                List<string> lable = new List<string>();
                if (i < 5)
                {
                    final.Add(x.Max());
                    lable.Add(titles.Skip(i).FirstOrDefault());
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
                  );
                    break;
                }
            }
            chart1.Series = piechartData;
        }

        private void fillColumnChart(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            axcolumn.Labels = new List<string>();
            List<string> names = new List<string>();
            List<int> x = new List<int>();
            IEnumerable<int> y = null;

            var temp = invLst
                   .Where(j => (selectedItemId.Count != 0 ? selectedItemId.Contains((int)j.ITitemUnitId) : true));
            var result = temp.GroupBy(s => s.ITitemUnitId).Select(s => new
            {
                ITitemUnitId = s.Key,
                countP = s.Where(m => m.invType == "p").Count(),
                countPb = s.Where(m => m.invType == "pb").Count(),

            });
            x = result.Select(m => m.countP).ToList();
            y = result.Select(m => m.countPb);
            var tempName = temp.GroupBy(jj => jj.ITitemUnitId)
             .Select(g => new ItemUnitCombo { itemUnitId = (int)g.FirstOrDefault().ITitemUnitId, itemUnitName = g.FirstOrDefault().ITitemName + "-" + g.FirstOrDefault().ITunitName }).ToList();
            names.AddRange(tempName.Select(nn => nn.itemUnitName));

            List<string> lable = new List<string>();
            SeriesCollection columnChartData = new SeriesCollection();
            List<int> cP = new List<int>();
            List<int> cPb = new List<int>();
            List<int> cD = new List<int>();
            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trSales"),
                AppSettings.resourcemanager.GetString("trReturned"),
                AppSettings.resourcemanager.GetString("trDraft")
            };
            int count = x.Count();
            for (int i = 0; i < count; i++)
            {
                if (i < 5)
                {
                    cP.Add(x.Max());
                    x.Remove(x.Max());
                }
                else
                {
                    cP.Add(x.Sum());
                    break;
                }
                axcolumn.Labels.Add(names.ToList().Skip(i).FirstOrDefault());

            }
            axcolumn.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));

            //3 فوق بعض
            columnChartData.Add(
            new StackedColumnSeries
            {
                Values = cP.AsChartValues(),
                Title = titles[0],
                DataLabels = true,
            });
            columnChartData.Add(
           new StackedColumnSeries
           {
               Values = cPb.AsChartValues(),
               Title = titles[1],
               DataLabels = true,
           });


            DataContext = this;
            cartesianChart.Series = columnChartData;
        }

        private void fillRowChart(ComboBox comboBox, ObservableCollection<int> stackedButton)
        {
            MyAxis.Labels = new List<string>();
            List<string> names = new List<string>();
            IEnumerable<decimal> pTemp = null;
            IEnumerable<decimal> pbTemp = null;
            IEnumerable<decimal> resultTemp = null;

            var temp = invLst.Where(j => (selectedItemId.Count != 0 ? stackedButton.Contains((int)j.ITitemUnitId) : true));
            var result = temp.GroupBy(s => s.ITitemUnitId).Select(s => new
            {
                ITitemUnitId = s.Key,
                totalP = s.Where(x => x.invType == "p").Sum(x => x.totalNet),
                totalPb = s.Where(x => x.invType == "pb").Sum(x => x.totalNet),

            }
         );
            var resultTotal = result.Select(x => new { x.ITitemUnitId, total = x.totalP - x.totalPb }).ToList();
            pTemp = result.Select(x => (decimal)x.totalP);
            pbTemp = result.Select(x => (decimal)x.totalPb);
            resultTemp = result.Select(x => (decimal)x.totalP - (decimal)x.totalPb);
            var tempName = temp.GroupBy(jj => jj.ITitemUnitId)
             .Select(g => new ItemUnitCombo { itemUnitId = (int)g.FirstOrDefault().ITitemUnitId, itemUnitName = g.FirstOrDefault().ITitemName + "-" + g.FirstOrDefault().ITunitName }).ToList();
            names.AddRange(tempName.Select(nn => nn.itemUnitName));

            SeriesCollection rowChartData = new SeriesCollection();
            List<decimal> purchase = new List<decimal>();
            List<decimal> returns = new List<decimal>();
            List<decimal> sub = new List<decimal>();
            List<string> titles = new List<string>()
            {
                AppSettings.resourcemanager.GetString("trNetSales"),
                AppSettings.resourcemanager.GetString("trTotalReturn"),
                AppSettings.resourcemanager.GetString("trTotalSales")
            };

            int xCount = 5;
            if (pTemp.Count() <= 5) xCount = pTemp.Count();
            for (int i = 0; i < xCount; i++)
            {
                purchase.Add(pTemp.ToList().Skip(i).FirstOrDefault());
                returns.Add(pbTemp.ToList().Skip(i).FirstOrDefault());
                sub.Add(resultTemp.ToList().Skip(i).FirstOrDefault());
                MyAxis.Labels.Add(names.ToList().Skip(i).FirstOrDefault());
            }
            if (pTemp.Count() > 5)
            {
                decimal purSum = 0, retSum = 0, subSum = 0;
                for (int i = 5; i < pTemp.Count(); i++)
                {
                    purSum = purSum + pTemp.ToList().Skip(i).FirstOrDefault();
                    retSum = retSum + pbTemp.ToList().Skip(i).FirstOrDefault();
                    subSum = subSum + resultTemp.ToList().Skip(i).FirstOrDefault();
                }
                if (!((purSum == 0) && (retSum == 0) && (subSum == 0)))
                {
                    purchase.Add(purSum);
                    returns.Add(retSum);
                    sub.Add(subSum);
                    MyAxis.Labels.Add(AppSettings.resourcemanager.GetString("trOthers"));
                }
            }
            rowChartData.Add(
            new LineSeries
            {
                Values = purchase.AsChartValues(),
                Title = titles[0]
            });
            rowChartData.Add(
         new LineSeries
         {
             Values = returns.AsChartValues(),
             Title = titles[1]
         });
            rowChartData.Add(
        new LineSeries
        {
            Values = sub.AsChartValues(),
            Title = titles[2]

        });
            DataContext = this;
            rowChart.Series = rowChartData;
        }

        #endregion

        IEnumerable<ItemTransferInvoice> itemTransfers = null;
        IEnumerable<ItemTransferInvoice> query = null;
        private void Txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (selectedTab == 0)
                {
                    itemTransfers = invLst.Where(j => (selectedItemId.Count != 0 ? selectedItemId.Contains((int)j.itemId) : true));

                    query = invLst.Where(s => (s.invNumber.ToLower().Contains(txt_search.Text) ||
                                                s.ITunitName.ToLower().Contains(txt_search.Text) ||
                                                s.ITprice.ToString().ToLower().Contains(txt_search.Text)));
                }
                else if (selectedTab == 1)
                {
                    itemTransfers = invLst.Where(j => (selectedBranchId.Count != 0 ? selectedBranchId.Contains((int)j.branchCreatorId) : true));

                    query = itemTransfers.Where(s => (s.ITitemName.ToLower().Contains(txt_search.Text) ||
                                                      s.ITunitName.ToLower().Contains(txt_search.Text) ||
                                                      s.ITquantity.ToString().ToLower().Contains(txt_search.Text)));
                }

                dgInvoice.ItemsSource = query;

                txt_count.Text = dgInvoice.Items.Count.ToString();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (selectedTab == 0)
                {
                    chk_allBranchesItem.IsChecked = true;
                    chk_allItems.IsChecked = true;
                    chk_itemInvoice.IsChecked = true;
                    chk_itemDrafs.IsChecked = true;
                    chk_itemReturn.IsChecked = true;
                    dp_ItemStartDate.SelectedDate = null;
                    dp_ItemEndDate.SelectedDate = null;

                    for (int i = 0; i < comboItemTemp.Count; i++)
                        dynamicComboItem.Add(comboItemTemp.Skip(i).FirstOrDefault());

                    comboItemTemp.Clear();
                    stk_tagsItems.Children.Clear();
                    selectedItemId.Clear();
                }

                else if (selectedTab == 1)
                {
                    chk_allcollect.IsChecked = true;
                    dp_collectStartDate.SelectedDate = null;
                    dp_collectEndDate.SelectedDate = null;
                    for (int i = 0; i < comboBrachTemp.Count; i++)
                        dynamicComboBranches.Add(comboBrachTemp.Skip(i).FirstOrDefault());

                    comboBrachTemp.Clear();
                    stk_tagsBranches.Children.Clear();
                    selectedBranchId.Clear();
                }

                txt_search.Text = "";

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void dt_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChangedCall(sender);
        }

        private void cb_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChangedCall(sender);
        }

        private void chk_selectionChanged(object sender, RoutedEventArgs e)
        {
            selectionChangedCall(sender);
        }

        private void dt_colselectionChanged(object sender, SelectionChangedEventArgs e)
        {
            col_selectionChangedCall(sender);
        }

        private void t_selectionChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            selectionChangedCall(sender);
        }

       

        Invoice invoice;
        private async void DgInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                invoice = new Invoice();
                if (dgInvoice.SelectedIndex != -1)
                {
                    ItemTransferInvoice item = dgInvoice.SelectedItem as ItemTransferInvoice;
                    if (item.invoiceId > 0)
                    {
                        invoice = await invoice.GetByInvoiceId(item.invoiceId);
                        MainWindow.mainWindow.Btn_purchase_Click(MainWindow.mainWindow.btn_purchase, null);
                        //uc_purchases.Instance.UserControl_Loaded(null, null);
                        //uc_purchases.Instance.btn_payInvoice_Click(uc_purchases.Instance.btn_payInvoice, null);

                        MainWindow.mainWindow.initializationMainTrack("payInvoice");

                        MainWindow.mainWindow.grid_main.Children.Clear();
                        MainWindow.mainWindow.grid_main.Children.Add(uc_payInvoice.Instance);

                        //uc_payInvoice.Instance.UserControl_Loaded(uc_payInvoice.Instance, null);
                        uc_payInvoice._InvoiceType = invoice.invType;
                        uc_payInvoice.Instance.invoice = invoice;
                        uc_payInvoice.isFromReport = true;
                        if (item.archived == 0)
                            uc_payInvoice.archived = false;
                        else
                            uc_payInvoice.archived = true;
                        await uc_payInvoice.Instance.fillInvoiceInputs(invoice);
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
