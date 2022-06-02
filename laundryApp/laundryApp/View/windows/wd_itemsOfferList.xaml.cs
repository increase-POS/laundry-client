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
    /// Interaction logic for wd_itemsOfferList.xaml
    /// </summary>
    public partial class wd_itemsOfferList : Window
    {
        public bool isActive;

        ItemUnitOffer itemUnitOffer = new ItemUnitOffer();
        ItemUnit itemModel = new ItemUnit();
        ItemUnitOffer itemUnitOfferModel = new ItemUnitOffer();
        Offer offerModel = new Offer();

        List<ItemUnit> allItems = new List<ItemUnit>();
        List<ItemUnitOffer> selectedItems = new List<ItemUnitOffer>();

        List<ItemUnit> allItemsSource = new List<ItemUnit>();
        List<ItemUnitOffer> selectedItemsSource = new List<ItemUnitOffer>();

        ItemUnit itemUnit = new ItemUnit();
        Offer offer = new Offer();

        string searchText = "";

        public string txtItemSearch;

        IEnumerable<ItemUnit> itemUnitQuery;

        public int offerId { get; set; }
        public wd_itemsOfferList()
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
                  grid_offerList.FlowDirection = FlowDirection.LeftToRight; }
                else
                {
                 grid_offerList.FlowDirection = FlowDirection.RightToLeft; }

                translat();
                #endregion

                offer = await offerModel.getOfferById(offerId);

                allItemsSource = await itemModel.Getall();
                allItemsSource = allItemsSource.Where(x => x.unitName == "saleUnit").ToList();
                selectedItemsSource = await itemUnitOffer.GetItemsByOfferId(offerId);

                allItems.AddRange(allItemsSource);

                //foreach (var i in allItems)
                //{
                //    i.itemName = i.itemName + "-" + i.unitName;
                //}

                selectedItems.AddRange(selectedItemsSource);
                //foreach (var i in selectedItems)
                //{
                //    i.itemName = i.itemName + "-" + i.unitName;
                //}

                //remove selected items from all items
                foreach (var i in selectedItems)
                {
                    itemUnit = allItemsSource.Where(s => s.itemUnitId == i.iuId).FirstOrDefault<ItemUnit>();
                    allItems.Remove(itemUnit);
                }

                dg_allItems.ItemsSource = allItems;
                dg_allItems.SelectedValuePath = "itemUnitId";
                dg_allItems.DisplayMemberPath = "itemName";

                dg_selectedItems.ItemsSource = selectedItems;
                dg_selectedItems.SelectedValuePath = "itemUnitId";
                dg_selectedItems.DisplayMemberPath = "itemName";

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

            txt_title.Text = AppSettings.resourcemanager.GetString("trOffer");
            txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            txt_selectedItems.Text = AppSettings.resourcemanager.GetString("trSelectedItems");

            tt_search.Content = AppSettings.resourcemanager.GetString("trSearch");
            tt_selectAllItem.Content = AppSettings.resourcemanager.GetString("trSelectAllItems");
            tt_unselectAllItem.Content = AppSettings.resourcemanager.GetString("trUnSelectAllItems");
            tt_selectItem.Content = AppSettings.resourcemanager.GetString("trSelectOneItem");
            tt_unselectItem.Content = AppSettings.resourcemanager.GetString("trUnSelectOneItem");

        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isActive = false;
            this.Close();
        }

        private void Txb_searchitems_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                txtItemSearch = txb_searchitems.Text.ToLower();

                searchText = txb_searchitems.Text;
                itemUnitQuery = allItems.Where(s => s.itemName.Contains(searchText) || s.unitName.Contains(searchText));
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

        private void Btn_selectedItem_Click(object sender, RoutedEventArgs e)
        {//select item
            try
            {
                itemUnit = dg_allItems.SelectedItem as ItemUnit;
                if (itemUnit != null)
                {
                    ItemUnitOffer iUO = new ItemUnitOffer();
                    iUO.ioId = 0;
                    iUO.iuId = itemUnit.itemUnitId;
                    iUO.offerId = offerId;
                    iUO.createUserId = MainWindow.userLogin.userId;
                    iUO.quantity = 1;
                    iUO.offerName = offer.name;
                    iUO.unitName = itemUnit.unitName;
                    iUO.itemName = itemUnit.itemName;
                    iUO.itemId = itemUnit.itemId;
                    iUO.unitId = itemUnit.unitId;

                    allItems.Remove(itemUnit);
                    selectedItems.Add(iUO);

                    dg_allItems.ItemsSource = allItems;
                    dg_selectedItems.ItemsSource = selectedItems;

                    dg_allItems.Items.Refresh();
                    dg_selectedItems.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_unSelectedItem_Click(object sender, RoutedEventArgs e)
        {//unselect item
            try
            {
                itemUnitOffer = dg_selectedItems.SelectedItem as ItemUnitOffer;
                ItemUnit i = new ItemUnit();
                if (itemUnitOffer != null)
                {
                    i = allItemsSource.Where(s => s.itemUnitId == itemUnitOffer.iuId.Value).FirstOrDefault();

                    allItems.Add(i);

                    selectedItems.Remove(itemUnitOffer);

                    dg_allItems.ItemsSource = allItems;
                    dg_selectedItems.ItemsSource = selectedItems;

                    dg_allItems.Items.Refresh();
                    dg_selectedItems.Items.Refresh();
                }
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
                int x = selectedItems.Count;
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

        private void Dg_selectedItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //try
            //{
            //    Btn_unSelectedItem_Click(null, null);
            //}
            //catch (Exception ex)
            //{
            //    SectionData.ExceptionMessage(ex, this);
            //}
        }

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                HelpClass.StartAwait(grid_offerList);

                int s = await itemUnitOfferModel.updategroup(offerId, selectedItems, MainWindow.userLogin.userId);

                isActive = true;
                this.Close();

                HelpClass.EndAwait(grid_offerList);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_offerList);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Dg_allItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void Btn_selectedAll_Click(object sender, RoutedEventArgs e)
        {//select all
            try
            {
                int x = allItems.Count;
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
    }
}
