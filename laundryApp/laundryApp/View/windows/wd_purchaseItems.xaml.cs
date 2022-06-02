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
using MaterialDesignThemes.Wpf;
using netoaster;
using laundryApp.Classes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_items.xaml
    /// </summary>
    public partial class wd_purchaseItems : Window
    {
        public wd_purchaseItems()
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
        public List<int> selectedItems { get; set; }
        List<Item> items;
        List<Item> itemsQuery;
        //Category category = new Category();

        //List<int> categoryIds = new List<int>();
        //Boolean all = true;
        //List<string> categoryNames = new List<string>();
        public byte tglCategoryState = 1;
        public byte tglItemState = 1;
        public string txtItemSearch;
        CatigoriesAndItemsView catigoriesAndItemsView = new CatigoriesAndItemsView();

        public bool isActive;
        public string CardType = "";
        #region loading
        public class loadingThread
        {
            public string name { get; set; }
            public bool value { get; set; }
        }
        List<keyValueBool> loadingList;
       async void loading_RefrishItems()
        {
            try
            {
               await RefrishItems();
            }
            catch (Exception)
            { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_RefrishItems"))
                {
                    item.value = true;
                    break;
                }
            }
        }
     


        #endregion
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                grid_main.Opacity = 1;

                // for pagination onTop Always
                btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                catigoriesAndItemsView.wdPurchaseItems = this;

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
                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_RefrishItems", value = false });


                loading_RefrishItems();
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
                await RefrishCategoriesCard();
                Txb_searchitems_TextChanged(txb_searchitems, null);
               
                
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
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_searchitems, AppSettings.resourcemanager.GetString("trSearchHint"));
            btn_add.Content = AppSettings.resourcemanager.GetString("trAdd");
        }
        
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isActive = false;
            this.Close();
        }

        #region Categor and Item
        #region Refrish Y
   
      
    
        async Task<IEnumerable<Item>> RefrishItems()
        {
            selectedItems = new List<int>();
            // int branchId = MainWindow.branchLogin.branchId;
            if (CardType == "consumption")
            {
                items = await FillCombo.item.GetKitchenItems(category.categoryId, MainWindow.branchLogin.branchId);
            }
            else //purchase: payInvoice - purchase Order - spending request - items movement
            {
                if (FillCombo.purchaseItems == null)
                    await FillCombo.RefreshPurchaseItems();
                
                items = FillCombo.purchaseItems.Where(x => x.itemUnitId != null).ToList();
            }
            //if (CardType.Equals("sales"))
            //{
            //    defaultSale = 1;
            //    defaultPurchase = 0;
            //    items = await itemModel.GetSaleOrPurItems(category.categoryId, defaultSale, defaultPurchase, branchId);
            //    FillCombo.salesItems = items.ToList();
            //}
            //else if (CardType.Equals("purchase"))
            //{

            //}
            //else if (CardType.Equals("order"))
            //{
            //    defaultPurchase = 0;
            //    defaultSale = 0;
            //    items = await itemModel.GetSaleOrPurItems(category.categoryId, defaultSale, defaultPurchase, branchId);
            //    FillCombo.salesItems = items.ToList();
            //}
            //else if (CardType.Equals("movement"))
            //{
            //    defaultPurchase = -1;
            //    defaultSale = -1;
            //    items = await itemModel.GetSaleOrPurItems(category.categoryId, defaultSale, defaultPurchase, branchId);
            //    FillCombo.salesItems = items.ToList();
            //}

            //if (defaultSale == 1)
            //    MainWindow.InvoiceGlobalSaleUnitsList = await itemUnitModel.GetForSale();
            //else
            //    MainWindow.InvoiceGlobalItemUnitsList = await itemUnitModel.Getall();
            return items;
        }

        //void RefrishItemsDatagrid(IEnumerable<Item> _items)
        //{
        //    dg_items.ItemsSource = _items;
        //}


        void RefrishItemsCard(IEnumerable<Item> _items)
        {
            grid_itemContainerCard.Children.Clear();
            catigoriesAndItemsView.gridCatigorieItems = grid_itemContainerCard;
            catigoriesAndItemsView.FN_refrishCatalogItem(_items.ToList(), 2, 3, "purchase");
        }
        #endregion
        #region Get Id By Click  Y
        private void Chip_OnDeleteClick(object sender, RoutedEventArgs e)
        {
            var currentChip = (Chip)sender;
            lst_items.Children.Remove(currentChip);
            selectedItems.Remove(Convert.ToInt32(currentChip.Name.Remove(0, 3)));
        }
        public void ChangeItemIdEvent(int itemId)
        {
            int isExist = -1;
            if (selectedItems != null)
                isExist = selectedItems.IndexOf(itemId);
            var item = items.ToList().Find(x => x.itemId == itemId);
            if (isExist == -1)
            {
                var b = new MaterialDesignThemes.Wpf.Chip()
                {
                    Content = item.name,
                    Name = "btn" + item.itemId,
                    IsDeletable = true,
                    Margin = new Thickness(5, 5, 5, 5)
                };
                b.DeleteClick += Chip_OnDeleteClick;
                lst_items.Children.Add(b);
                selectedItems.Add(itemId);
            }
        }

        #endregion
      
        #region Search Y


        private async void Txb_searchitems_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (FillCombo.purchaseItems is null)
                    await FillCombo.RefreshPurchaseItems();
                txtItemSearch = txb_searchitems.Text.ToLower();
                pageIndex = 1;

                #region
                itemsQuery = items;
                //if (categoryIds.Count > 0 && all == false)
                //{
                //    itemsQuery = itemsQuery.Where(x => x.categoryId != null).ToList().Where(x => categoryIds.Contains((int)x.categoryId)).ToList();
                //}

                itemsQuery = itemsQuery.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                ) && x.categoryId.ToString().Contains(categoryIdSearch)).ToList();

                if (btns is null)
                    btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns,6));
                #endregion
                //RefrishItemsDatagrid(itemsQuery);

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion
        #region Pagination Y
        Pagination pagination = new Pagination();
        Button[] btns;
        public int pageIndex = 1;

        private void Tb_pageNumberSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                itemsQuery = items.Where(x => x.isActive == tglItemState).ToList();

                if (tb_pageNumberSearch.Text.Equals(""))
                {
                    pageIndex = 1;
                }
                else if (((itemsQuery.Count() - 1) / 9) + 1 < int.Parse(tb_pageNumberSearch.Text))
                {
                    pageIndex = ((itemsQuery.Count() - 1) / 9) + 1;
                }
                else
                {
                    pageIndex = int.Parse(tb_pageNumberSearch.Text);
                }

                #region
                itemsQuery = items.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                ) && x.isActive == tglItemState).ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns,6));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_firstPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = 1;
                #region
                itemsQuery = items.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                ) && x.isActive == tglItemState).ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 6));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_prevPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_prevPage.Content.ToString());
                #region
                itemsQuery = items.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                ) && x.isActive == tglItemState).ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 6));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_activePage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_activePage.Content.ToString());
                #region
                itemsQuery = items.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                ) && x.isActive == tglItemState).ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 6));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_nextPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_nextPage.Content.ToString());
                #region
                itemsQuery = items.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                ) && x.isActive == tglItemState).ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 6));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_lastPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                itemsQuery = items.Where(x => x.isActive == 1).ToList();
                pageIndex = ((itemsQuery.Count() - 1) / 9) + 1;
                #region
                itemsQuery = items.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                ) && x.isActive == tglItemState).ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 6));
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
      

        #endregion

        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                await FillCombo.RefreshPurchaseItems();
               FillCombo.RefreshItemUnit();
                RefrishItems();
                Txb_searchitems_TextChanged(null, null);
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
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

        private void Btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (selectedItems.Count > 0)
                {
                    isActive = true;
                    this.Close();
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        /*
        private void categories_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Tag.ToString() == "allCategories")
            {
                chb_rawMaterials.IsChecked =
                chb_vegetables.IsChecked =
                chb_meat.IsChecked =
                chb_drinks.IsChecked = false;

                chb_rawMaterials.IsEnabled =
                chb_vegetables.IsEnabled =
                chb_meat.IsEnabled =
                chb_drinks.IsEnabled = false;

                categoryIds = new List<int>();
                all = true;
            }
            else
            {
                all = false;
                categoryIds.Add(FillCombo.GetCategoryId(checkBox.Tag.ToString()));
            }
            Txb_searchitems_TextChanged(null, null);
        }
        private void categories_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Tag.ToString() == "allCategories")
            {
                chb_rawMaterials.IsChecked =
                chb_vegetables.IsChecked =
                chb_meat.IsChecked =
                chb_drinks.IsChecked = false;

                chb_rawMaterials.IsEnabled =
                chb_vegetables.IsEnabled =
                chb_meat.IsEnabled =
                chb_drinks.IsEnabled = true;

                categoryIds = new List<int>();
                all = true;
            }
            else
            {                
                categoryIds.Remove(FillCombo.GetCategoryId(checkBox.Tag.ToString()));
            }
            Txb_searchitems_TextChanged(null, null);
        }
        */
        #region category
        List<Category> categories;
        List<Category> categoriesQuery;
        Category category = new Category();
        Category categoryModel = new Category();
        string categoryIdSearch = "";

        async Task<IEnumerable<Category>> RefreshCategorysList()
        {
            categories = await categoryModel.Get();
            categories = categories.Where(x => x.type == "p").ToList();
            return categories;
        }
        async Task RefrishCategoriesCard()
        {
            if (categories is null)
                await RefreshCategorysList();
            categoriesQuery = categories.Where(x => x.isActive == 1).ToList();
            catigoriesAndItemsView.gridCatigories = grid_categoryCards;
            generateCoulmnCategoriesGrid(categoriesQuery.Count());
            catigoriesAndItemsView.FN_refrishCatalogItem_category(categoriesQuery.ToList(), "Category", -1);
        }
        void generateCoulmnCategoriesGrid(int column)
        {

            #region
            grid_categoryCards.ColumnDefinitions.Clear();
            ColumnDefinition[] cd = new ColumnDefinition[column];
            for (int i = 0; i < column; i++)
            {
                cd[i] = new ColumnDefinition();
                cd[i].Width = new GridLength(1, GridUnitType.Auto);
                grid_categoryCards.ColumnDefinitions.Add(cd[i]);
            }
            #endregion

        }
        public  void ChangeCategoryIdEvent(int categoryId)
        {
            category = categories.ToList().Find(c => c.categoryId == categoryId);
            categoryIdSearch = categoryId.ToString();
            //await RefreshItemsList();
            //await Search();
            Txb_searchitems_TextChanged(txb_searchitems, null);

        }
        private async void Btn_getAllCategory_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                HelpClass.StartAwait(grid_main);
                categoryIdSearch = "";
                await RefrishCategoriesCard();
                //await RefreshItemsList();
                //await Search();
                Txb_searchitems_TextChanged(txb_searchitems, null);


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
