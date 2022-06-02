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
    /// Interaction logic for wd_itemsUnitList.xaml
    /// </summary>
    public partial class wd_itemsUnitList : Window
    {
        public int itemId = 0 , itemUnitId = 0, storageCostId = 0;
        public bool isActive;
        public string CallerName;//"IUList"

        ItemUnit itemUnit = new ItemUnit();
        List<ItemUnit> allItemUnitsSource = new List<ItemUnit>();
        List<ItemUnit> allItemUnits = new List<ItemUnit>();

        Package package = new Package();
        StorageCost storageCost = new StorageCost();

        List<Package> allIPackagesSource = new List<Package>();
        public List<Package> allPackages = new List<Package>();

        List<ItemUnit> allItemsUnitsSource = new List<ItemUnit>();
        public List<ItemUnit> selectedItemUnits = new List<ItemUnit>();

        public List<ItemUnitUser> selectedItemUnitsUser = new List<ItemUnitUser>();

        //home
        List<ItemUnitUser> selectedItemUnitsHomeSource = new List<ItemUnitUser>();
        public List<ItemUnitUser> selectedItemUnitsHome = new List<ItemUnitUser>();
        ItemUnitUser itemUnitUserModel = new ItemUnitUser();
        ItemUnitUser itemUnitUser = new ItemUnitUser();

        string searchText = "";

        public string txtItemSearch;

        IEnumerable<ItemUnit> itemUnitQuery;

        public wd_itemsUnitList()
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
                    Btn_save_Click(null, null);
                }
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
                
                    HelpClass.StartAwait(grid_offerList);

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_offerList.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_offerList.FlowDirection = FlowDirection.RightToLeft;
                }
                translat();
                #endregion

               
                #region item units for package
                if (CallerName == "package")
                {
                    //bdr_general.Visibility = Visibility.Visible;
                    //bdr_home.Visibility = Visibility.Collapsed;
                    item.Visibility = Visibility.Visible;
                    note.Visibility = Visibility.Collapsed;
                    quantity.Visibility = Visibility.Visible;

                    allItemUnitsSource = FillCombo.itemUnitList.Where(x => x.type == "SalesNormal").ToList();
                    allItemUnits.AddRange(allItemUnitsSource);
                    for (int i = 0; i < allItemUnits.Count; i++)
                    {
                        //remove parent package itemunit
                        if (allItemUnits[i].itemUnitId == itemUnitId)
                        { allItemUnits.Remove(allItemUnits[i]); break; }

                    }
                    allIPackagesSource = await package.GetChildsByParentId(itemUnitId);

                    //remove selected itemunits from source itemunits
                    foreach (var p in allIPackagesSource)
                    {
                        p.itemName = p.notes;
                        for (int i = 0; i < allItemUnits.Count; i++)
                        {
                            //remove saved itemunits
                            if (p.childIUId == allItemUnits[i].itemUnitId)
                            {
                                allItemUnits.Remove(allItemUnits[i]);
                            }
                        }
                    }
                    allPackages.AddRange(allIPackagesSource);
                    foreach (var p in allPackages)
                    {
                        foreach (var iu in allItemUnits)
                            if (p.parentIUId == iu.itemUnitId)
                                p.itemName = iu.itemName + "-" + iu.unitName;
                    }

                    dg_selectedItems.ItemsSource = allPackages;
                    dg_allItems.SelectedValuePath = "packageId";
                    dg_allItems.DisplayMemberPath = "notes";

                }
                #endregion

                #region storageCost
                else if (CallerName == "storageCost")
                {
                    //bdr_general.Visibility = Visibility.Visible;
                    //bdr_home.Visibility = Visibility.Collapsed;
                    item.Visibility = Visibility.Visible;
                    note.Visibility = Visibility.Collapsed;
                    quantity.Visibility = Visibility.Collapsed;

                    allItemUnitsSource = FillCombo.itemUnitList.Select( x => new ItemUnit() { itemId = x.itemId, itemName = x.itemName, unitName = x.unitName,itemUnitId = x.itemUnitId }).ToList();
                    allItemUnits.AddRange(allItemUnitsSource);

                    allItemsUnitsSource = await storageCost.GetStorageCostUnits(storageCostId);
                    selectedItemUnits.AddRange(allItemsUnitsSource);
                    foreach (var iu in allItemUnitsSource)
                            iu.itemName= iu.itemName + "-" + iu.unitName;
                    //remove selected itemunits from source itemunits
                    foreach (var u in allItemsUnitsSource)
                    {
                        for (int i = 0; i < allItemUnits.Count; i++)
                        {
                            //remove saved itemunits
                            if (u.itemUnitId == allItemUnits[i].itemUnitId)
                            {
                                allItemUnits.Remove(allItemUnits[i]);
                            }
                        }
                    }

                    dg_selectedItems.ItemsSource = selectedItemUnits;
                    dg_selectedItems.SelectedValuePath = "itemUnitId";
                    dg_selectedItems.DisplayMemberPath = "itemName";

                }
                #endregion

                #region dashboard
               else if (CallerName.Equals("IUList"))
                {
                    //bdr_general.Visibility = Visibility.Collapsed;
                    //bdr_home.Visibility = Visibility.Visible;

                    item.Visibility = Visibility.Collapsed;
                    note.Visibility = Visibility.Visible;
                    quantity.Visibility = Visibility.Collapsed;

                    //if (FillCombo.itemUnitList == null)
                    //    FillCombo.itemUnitList = await FillCombo.RefreshItemUnit();

                    //allItemUnitsSource = FillCombo.itemUnitList.Where(i => i.type == "PurchaseNormal" || i.type == "PurchaseExpire").ToList();
                    allItemUnitsSource = await itemUnit.GetIU();
                    allItemUnitsSource = allItemUnitsSource.Where(i => i.type == "PurchaseNormal" || i.type == "PurchaseExpire").ToList();
                    allItemUnits.AddRange(allItemUnitsSource);
                   
                    foreach (var iu in allItemUnits)
                    {
                        iu.itemName = iu.itemName + "-" + iu.unitName;
                    }

                    selectedItemUnitsHomeSource = await itemUnitUserModel.GetByUserId(MainWindow.userLogin.userId);

                    //remove selected itemunits from source itemunits
                    foreach (var p in selectedItemUnitsHomeSource)
                    {
                        for (int i = 0; i < allItemUnits.Count; i++)
                        {
                            //remove saved itemunits
                            if (p.itemUnitId == allItemUnits[i].itemUnitId)
                            {
                                allItemUnits.Remove(allItemUnits[i]);
                            }
                        }
                    }
                    selectedItemUnitsHome.AddRange(selectedItemUnitsHomeSource);
                    foreach (var p in selectedItemUnitsHome)
                    {
                        foreach (var iu in allItemUnits)
                            if (p.itemUnitId == iu.itemUnitId)
                                p.notes = iu.itemName + "-" + iu.unitName;
                    }

                    dg_selectedItems.ItemsSource = selectedItemUnitsHome;
                    dg_selectedItems.SelectedValuePath = "id";
                    dg_selectedItems.DisplayMemberPath = "notes";
                }
                #endregion

                dg_allItems.ItemsSource = allItemUnits;
                dg_allItems.SelectedValuePath = "itemUnitId";
                dg_allItems.DisplayMemberPath = "itemName";

                
                    HelpClass.EndAwait(grid_offerList);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_offerList);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translat()
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_searchitems, AppSettings.resourcemanager.GetString("trSearchHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            dg_allItems.Columns[0].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_selectedItems.Columns[0].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_selectedItems.Columns[1].Header = AppSettings.resourcemanager.GetString("trQuantity");

            txt_title.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_selectedItems.Text = AppSettings.resourcemanager.GetString("trSelectedItems");

            tt_search.Content = AppSettings.resourcemanager.GetString("trSearch");
            tt_selectAllItem.Content = AppSettings.resourcemanager.GetString("trSelectAllItems");
            tt_unselectAllItem.Content = AppSettings.resourcemanager.GetString("trUnSelectAllItems");
            tt_selectItem.Content = AppSettings.resourcemanager.GetString("trSelectOneItem");
            tt_unselectItem.Content = AppSettings.resourcemanager.GetString("trUnSelectOneItem");

        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isActive = false;
            DialogResult = false;
            this.Close();
        }

        private void Txb_searchitems_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                txtItemSearch = txb_searchitems.Text.ToLower();

                searchText = txb_searchitems.Text;
                itemUnitQuery = allItemUnits.Where(s => s.itemName.Contains(searchText) || s.unitName.Contains(searchText));
                dg_allItems.ItemsSource = itemUnitQuery;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Dg_allItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_selectedItem_Click(null, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_selectedAll_Click(object sender, RoutedEventArgs e)
        {//select all
            try
            {
                int x = allItemUnits.Count;
                for (int i = 0; i < x; i++)
                {
                    dg_allItems.SelectedIndex = 0;
                    Btn_selectedItem_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_selectedItem_Click(object sender, RoutedEventArgs e)
        {//select one
            try
            {
                itemUnit = dg_allItems.SelectedItem as ItemUnit;
                if (itemUnit != null)
                {
                    if (CallerName == "package")
                    {
                        Package p = new Package();

                        p.parentIUId = itemUnitId;
                        p.childIUId = itemUnit.itemUnitId;
                        p.quantity = 1;
                        p.isActive = 1;
                        p.itemName = itemUnit.itemName;
                        p.createUserId = MainWindow.userLogin.userId;

                        allItemUnits.Remove(itemUnit);
                        allPackages.Add(p);

                        dg_allItems.ItemsSource = allItemUnits;
                        dg_selectedItems.ItemsSource = allPackages;
                    }
                    else if(CallerName == "IUList")
                    {
                        ItemUnitUser iu = new ItemUnitUser();

                        iu.itemUnitId = itemUnit.itemUnitId;
                        iu.userId = MainWindow.userLogin.userId;
                        iu.isActive = 1;
                        iu.notes = itemUnit.itemName;
                        iu.createUserId = MainWindow.userLogin.userId;

                        allItemUnits.Remove(itemUnit);
                        selectedItemUnitsHome.Add(iu);

                        dg_allItems.ItemsSource = allItemUnits;
                        dg_selectedItems.ItemsSource = selectedItemUnitsHome;


                    }
                    else
                    {
                        ItemUnit p = new ItemUnit();

                        p.itemUnitId = itemUnit.itemUnitId;
                        p.itemName = itemUnit.itemName;
                        p.unitName = itemUnit.unitName;
                        p.isActive = 1;
                        p.createUserId = MainWindow.userLogin.userId;

                        allItemUnits.Remove(itemUnit);
                        selectedItemUnits.Add(p);

                        dg_allItems.ItemsSource = allItemUnits;
                        dg_selectedItems.ItemsSource = selectedItemUnits;

                    }

                    dg_allItems.Items.Refresh();
                    dg_selectedItems.Items.Refresh();
                    dg_selectedItems.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_unSelectedItem_Click(object sender, RoutedEventArgs e)
        {//unselect one
            try
            {
                ItemUnit i = new ItemUnit();

               if (CallerName.Equals("package"))
                {
                    package = dg_selectedItems.SelectedItem as Package;
                    if (package != null)
                    {
                        i = allItemUnitsSource.Where(s => s.itemUnitId == package.childIUId.Value).FirstOrDefault();

                        allItemUnits.Add(i);

                        allPackages.Remove(package);

                        dg_selectedItems.ItemsSource = allPackages;
                    }
                }
                else if(CallerName.Equals("IUList"))
                {
                    itemUnitUser = dg_selectedItems.SelectedItem as ItemUnitUser;
                    if (itemUnitUser != null)
                    {
                        i = allItemUnitsSource.Where(s => s.itemUnitId == itemUnitUser.itemUnitId.Value).FirstOrDefault();

                        allItemUnits.Add(i);

                        selectedItemUnitsHome.Remove(itemUnitUser);

                        dg_selectedItems.ItemsSource = selectedItemUnitsHome;
                    }
                }
                else
                {
                    itemUnit = dg_selectedItems.SelectedItem as ItemUnit;
                    if (itemUnit != null)
                    {
                        i = allItemUnitsSource.Where(s => s.itemUnitId == itemUnit.itemUnitId).FirstOrDefault();

                        allItemUnits.Add(i);

                        selectedItemUnits.Remove(itemUnit);

                        dg_selectedItems.ItemsSource = selectedItemUnits;
                    }
                }

                dg_allItems.ItemsSource = allItemUnits;

                dg_allItems.Items.Refresh();
                dg_selectedItems.Items.Refresh();
                dg_selectedItems.Items.Refresh();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_unSelectedAll_Click(object sender, RoutedEventArgs e)
        {//unselect all
            try
            {
                int x = 0;
                if (CallerName.Equals("package"))
                    x = allPackages.Count;
                else
                    x = selectedItemUnits.Count;
                for (int i = 0; i < x; i++)
                {
                    dg_selectedItems.SelectedIndex = 0;
                    Btn_unSelectedItem_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                
                    HelpClass.StartAwait(grid_offerList);

                isActive = true;
                DialogResult = true;
                this.Close();

                
                    HelpClass.EndAwait(grid_offerList);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_offerList);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void Dg_allItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Dg_selectedItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        

        private void Dg_selectedItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //// Have to do this in the unusual case where the border of the cell gets selected.
            //// and causes a crash 'EditItem is not allowed'
            //e.Cancel = true;
        }
    }
}
