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
    /// Interaction logic for wd_branchesList.xaml
    /// </summary>
    public partial class wd_branchesList : Window
    {
        public bool isActive;
        public int Id { get; set; }
        public char userOrBranch { get; set; }

        Branch branchModel = new Branch();
        BranchesUsers branchUserModel = new BranchesUsers();
        BranchStore branchStoreModel = new BranchStore();

        List<Branch> allStoresSource = new List<Branch>();
        List<BranchesUsers> selectedStoresByUserSource = new List<BranchesUsers>();
        List<BranchStore> selectedStoresByBranchSource = new List<BranchStore>();

        List<Branch> allStores = new List<Branch>();
        List<BranchesUsers> selectedStoresByUser = new List<BranchesUsers>();
        List<BranchesUserstable> selectedStoresByUserTable = new List<BranchesUserstable>();
        List<BranchStore> selectedStoresByBranch = new List<BranchStore>();
        List<BranchStoretable> selectedStoresByBranchTable = new List<BranchStoretable>();

        Branch branch = new Branch();
        BranchesUsers branchUser = new BranchesUsers();
        BranchStore branchStore = new BranchStore();

        IEnumerable<Branch> storeQuery;

        string searchText = "";

        public string txtStoreSearch;
        public wd_branchesList()
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                
                    HelpClass.StartAwait(grid_branchList);

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
  grid_branchList.FlowDirection = FlowDirection.LeftToRight; }
                else
                {
   grid_branchList.FlowDirection = FlowDirection.RightToLeft; }

                translat();
                #endregion

                allStoresSource = await branchModel.GetAll();////active branch and store 
                allStores.AddRange(allStoresSource.Where(x => x.branchId !=1));
                 //chk user or branch
                var dgtc = dg_selectedStores.Columns[0] as DataGridTextColumn;

                if (userOrBranch == 'u')
                {
                    selectedStoresByUserSource = await branchUserModel.GetBranchesByUserId(Id);
                    selectedStoresByUser.AddRange(selectedStoresByUserSource);
                    //remove selected items from all items
                    foreach (var i in selectedStoresByUser)
                    {
                        branch = allStores.Where(s => s.branchId == i.branchId).FirstOrDefault<Branch>();
                        allStores.Remove(branch);
                    }
                    dgtc.Binding = new System.Windows.Data.Binding("bname");

                    dg_selectedStores.ItemsSource = selectedStoresByUser;
                    dg_selectedStores.SelectedValuePath = "branchId";
                    dg_selectedStores.DisplayMemberPath = "bname";
                }
                else
                {
                    selectedStoresByBranchSource = await branchStoreModel.GetAll();
                    selectedStoresByBranchSource = selectedStoresByBranchSource.Where(s => s.branchId == Id).ToList();
                    selectedStoresByBranch.AddRange(selectedStoresByBranchSource);
                    //remove selected items from all items
                    foreach (var i in selectedStoresByBranch)
                    {
                        branch = allStores.Where(s => s.branchId == i.storeId).FirstOrDefault<Branch>();
                        allStores.Remove(branch);
                    }
               
                    dgtc.Binding = new System.Windows.Data.Binding("sname");

                    dg_selectedStores.ItemsSource = selectedStoresByBranch;
                    dg_selectedStores.SelectedValuePath = "branchId";
                    dg_selectedStores.DisplayMemberPath = "sname";

                    //remove selected branch/store from previous window
                    //branch = allStores.Where(s => s.branchId == Id).FirstOrDefault<Branch>();
                    //allStores.Remove(branch);
                }

                dg_allStores.ItemsSource = allStores;
                dg_allStores.SelectedValuePath = "branchId";
                dg_allStores.DisplayMemberPath = "name";
          
                
                    HelpClass.EndAwait(grid_branchList);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_branchList);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translat()
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            dg_allStores.Columns[0].Header = AppSettings.resourcemanager.GetString("trBranch/Store");
            dg_selectedStores.Columns[0].Header = AppSettings.resourcemanager.GetString("trBranch/Store");

            txt_title.Text = AppSettings.resourcemanager.GetString("trBranchs/Stores");
            txt_stores.Text = AppSettings.resourcemanager.GetString("trBranchs/Stores");
            txt_selectedStores.Text = AppSettings.resourcemanager.GetString("trSelectedBranchs/Stores");

            tt_search.Content = AppSettings.resourcemanager.GetString("trSearch");
            tt_selectAllItem.Content = AppSettings.resourcemanager.GetString("trSelectAllItems");
            tt_unselectAllItem.Content = AppSettings.resourcemanager.GetString("trUnSelectAllItems");
            tt_selectItem.Content = AppSettings.resourcemanager.GetString("trSelectOneItem");
            tt_unselectItem.Content = AppSettings.resourcemanager.GetString("trUnSelectOneItem");
        }
        private void Dg_selectedStores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_unSelectedStore_Click(null, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Dg_allStores_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_selectedStore_Click(null, null);
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
                int x = allStores.Count;
                for (int i = 0; i < x; i++)
                {
                    dg_allStores.SelectedIndex = 0;
                    Btn_selectedStore_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_selectedStore_Click(object sender, RoutedEventArgs e)
        {//select one 
            try
            {
                
                    HelpClass.StartAwait(grid_branchList);

                branch = dg_allStores.SelectedItem as Branch;

            if (branch != null)
            {
                allStores.Remove(branch);
                if(userOrBranch == 'u')
                {
                    BranchesUsers bu = new BranchesUsers();
                    bu.branchsUsersId = 0;
                    bu.branchId = branch.branchId;
                    bu.userId = Id;
                    bu.bname = branch.name;
                    bu.createUserId = MainWindow.userLogin.userId;

                    selectedStoresByUser.Add(bu);
                    
                    dg_selectedStores.ItemsSource = selectedStoresByUser;

                }
                else
                {
                    BranchStore bs = new BranchStore();
                    bs.id = 0;
                    bs.branchId = branch.branchId;
                    bs.storeId = branch.branchId;
                    bs.createUserId = MainWindow.userLogin.userId;
                        bs.sname = branch.name;
                    bs.bname = branch.name;
                    bs.isActive = 1;
                    selectedStoresByBranch.Add(bs);
                    dg_selectedStores.ItemsSource = selectedStoresByBranch;
                }

                dg_allStores.ItemsSource = allStores;

                dg_allStores.Items.Refresh();
                dg_selectedStores.Items.Refresh();
            }
                
                    HelpClass.EndAwait(grid_branchList);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_branchList);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_unSelectedAll_Click(object sender, RoutedEventArgs e)
        {//unselect all
            try
            {
                int x = 0;
                if (userOrBranch == 'u')
                    x = selectedStoresByUser.Count;
                else
                    x = selectedStoresByBranch.Count;

                for (int i = 0; i < x; i++)
                {
                    dg_selectedStores.SelectedIndex = 0;
                    Btn_unSelectedStore_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private  void Btn_unSelectedStore_Click(object sender, RoutedEventArgs e)
        {//unselect one
            try
            {
                
                    HelpClass.StartAwait(grid_branchList);

                if (userOrBranch == 'u')
                {
                    branchUser = dg_selectedStores.SelectedItem as BranchesUsers;

                    if (branchUser != null)
                    {
                        branch = allStoresSource.Where(s => s.branchId == branchUser.branchId.Value).FirstOrDefault();
                        //branch = await branchModel.getBranchById(branchUser.branchId.Value);

                        selectedStoresByUser.Remove(branchUser);

                        dg_selectedStores.ItemsSource = selectedStoresByUser;
                    }
                }
                else
                {
                    branchStore = dg_selectedStores.SelectedItem as BranchStore;

                    if (branchStore != null)
                    {
                        branch = allStoresSource.Where(s => s.branchId == branchStore.storeId.Value).FirstOrDefault();
                        //branch = await branchModel.getBranchById(branchStore.branchId.Value);
                        selectedStoresByBranch.Remove(branchStore);

                        dg_selectedStores.ItemsSource = selectedStoresByBranch;
                    }

                }

                allStores.Add(branch);
                dg_allStores.ItemsSource = allStores;
                dg_allStores.Items.Refresh();
                dg_selectedStores.Items.Refresh();

                
                    HelpClass.EndAwait(grid_branchList);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_branchList);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                
                    HelpClass.StartAwait(grid_branchList);
                int s = 0;
                if (userOrBranch == 'u')
                {
                    foreach(var v in selectedStoresByUser)
                    {
                        BranchesUserstable but = new BranchesUserstable();
                        but.branchsUsersId = v.branchsUsersId;
                        but.branchId = v.branchId;
                        but.userId = v.userId;
                        but.createUserId = v.createUserId;
                        but.userId = v.updateUserId;
                        but.createDate = v.createDate;
                        but.updateDate = v.updateDate;
                        selectedStoresByUserTable.Add(but);
                    }
                    s = await branchUserModel.UpdateBranchByUserId(selectedStoresByUserTable, Id,MainWindow.userLogin.userId);
                }
                else
                {
                    foreach(var v in selectedStoresByBranch)
                    {
                        BranchStoretable bst = new BranchStoretable();
                        bst.id = v.id;
                        bst.branchId = v.branchId;
                        bst.storeId = v.storeId;
                        bst.createDate = v.createDate;
                        bst.updateDate = v.updateDate;
                        bst.createUserId = v.createUserId;
                        bst.updateUserId = v.updateUserId;
                        bst.isActive = 1;
                        selectedStoresByBranchTable.Add(bst);
                    }
                    s = await branchStoreModel.UpdateStoresById(selectedStoresByBranchTable, Id, MainWindow.userLogin.userId);

                }
                isActive = true;
                this.Close();
                
                    HelpClass.EndAwait(grid_branchList);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_branchList);
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

        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                
                    HelpClass.StartAwait(grid_branchList);
                txtStoreSearch = txb_search.Text.ToLower();

                searchText = txb_search.Text;
                storeQuery = allStores.Where(s => s.name.Contains(searchText) );
                dg_allStores.ItemsSource = storeQuery;
                
                    HelpClass.EndAwait(grid_branchList);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_branchList);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {//close
            isActive = false;
            this.Close();
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
        private void Grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                //// Have to do this in the unusual case where the border of the cell gets selected.
                //// and causes a crash 'EditItem is not allowed'
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
