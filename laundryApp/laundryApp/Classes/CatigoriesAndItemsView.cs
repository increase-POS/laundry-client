using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.Classes;
using laundryApp.controlTemplate;
using laundryApp.View;
using laundryApp.View.catalog;
using laundryApp.View.catalog.salesItems;
using laundryApp.View.catalog.rawMaterials;
using laundryApp.View.kitchen;
using laundryApp.View.sales;
using laundryApp.View.storage;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace laundryApp.Classes
{
    public class CatigoriesAndItemsView 
    {

        public uc_receiptInvoice ucdiningHall;
        //public uc_takeAway uctakeAway;
        public uc_itemsRawMaterials ucitemsRawMaterials;
        public uc_salesItem ucsalesItem;
        public uc_package ucpackage;
        //public uc_menuSettings ucmenuSettings;
        public wd_purchaseItems wdPurchaseItems;
        public uc_categorieRawMaterials uccategorie;

        public Grid gridCatigories;
        public Grid gridCatigorieItems;
        private int _idItem;
        private int _idCategory;
        //private int _iddiningHall;
        //private int _iditemsRawMaterials;
        //private int _idsalesItem;

        public int idItem
        {
            get => _idItem; set
            {
                _idItem = value;
                INotifyPropertyChangedIdCatigorieItems();
            }
        }
        public int idCategory
        {
            get => _idCategory; set
            {
                _idCategory = value;
                INotifyPropertyChangedIdCatigorie();
            }
        }
        //public int idwdItems
        //{
        //    get => _iddiningHall; set
        //    {
        //        _iddiningHall = value;
        //        INotifyPropertyChangedIdCatigorieItems();
        //    }
        //}

        private  void INotifyPropertyChangedIdCatigorieItems()
        {
            if (ucdiningHall != null)
            {
                ucdiningHall.ChangeItemIdEvent(idItem);
            }
            //if (uctakeAway != null)
            //{
            //    uctakeAway.ChangeItemIdEvent(idItem);
            //}
            
            if (ucitemsRawMaterials != null)
            {
                ucitemsRawMaterials.ChangeItemIdEvent(idItem);
            }
            if (ucsalesItem != null)
            {
                ucsalesItem.ChangeItemIdEvent(idItem);
            }
            if (ucpackage != null)
            {
                ucpackage.ChangeItemIdEvent(idItem);
            }
            //if (ucmenuSettings != null)
            //{
            //    ucmenuSettings.ChangeItemIdEvent(idItem);
            //}
            if (wdPurchaseItems != null)
            {
                wdPurchaseItems.ChangeItemIdEvent(idItem);
            }
           
        }
        private void INotifyPropertyChangedIdCatigorie()
        {

            if (uccategorie != null)
            {
                uccategorie.ChangeCategoryIdEvent(idCategory);
            }
            if (ucitemsRawMaterials != null)
            {
                ucitemsRawMaterials.ChangeCategoryIdEvent(idCategory);
            }
            if (wdPurchaseItems != null)
            {
                wdPurchaseItems.ChangeCategoryIdEvent(idCategory);
            }
        }


        #region Catalog category


        private int pastCatalogItem_category = -1;
        double gridCatigorieItems_ActualHeight_category = 0;
        double gridCatigorieItems_ActualWidth_category = 0;
        public void FN_refrishCatalogItem_category(List<Category> items, string cardType, int columnCount)
        {
            gridCatigories.Children.Clear();
            if(columnCount != -1)
            {
                gridCatigorieItems_ActualHeight_category = gridCatigories.ActualHeight / 3;
                gridCatigorieItems_ActualWidth_category = gridCatigories.ActualWidth / 5;
            }
            else
            {
                gridCatigorieItems_ActualHeight_category = gridCatigories.ActualHeight;
                gridCatigorieItems_ActualWidth_category = gridCatigories.ActualWidth;
            }
            int row = 0;
            int column = 0;
            foreach (var item in items)
            {

                CardViewItems itemCardView = new CardViewItems();
                itemCardView.category = item;
                itemCardView.cardType = cardType;
                itemCardView.row = row;
                itemCardView.column = column;
                FN_createRectangelCard_category(itemCardView);


                if (column != -1)
                {
                    column++;
                    if (column == columnCount)
                    {
                        column = 0;
                        row++;
                    }
                }
                else
                {
                    column++;
                }
            }
        }


        uc_squareCard FN_createRectangelCard_category(CardViewItems itemCardView, string BorderBrush = "#DFDFDF")
        {
            uc_squareCard uc = new uc_squareCard(itemCardView, gridCatigorieItems_ActualHeight_category, gridCatigorieItems_ActualWidth_category);
            uc.squareCardBorderBrush = BorderBrush;
            uc.Name = "CardName" + itemCardView.category.categoryId;
            Grid.SetRow(uc, itemCardView.row);
            Grid.SetColumn(uc, itemCardView.column);
            gridCatigories.Children.Add(uc);
            uc.MouseDown += this.ucItemMouseDown_category;
            return uc;
        }

        private void ucItemMouseDown_category(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 0)
                doubleClickItem_category(sender);
        }
        private void doubleClickItem_category(object sender)
        {
            try
            {
                uc_squareCard uc = (uc_squareCard)sender;
                uc = gridCatigories.Children.OfType<uc_squareCard>().Where(x => x.Name.ToString() == "CardName" + uc.cardViewitem.category.categoryId).FirstOrDefault();

                uc.squareCardBorderBrush = "#078181";

                if (pastCatalogItem_category != -1 && pastCatalogItem_category != uc.cardViewitem.category.categoryId)
                {
                    var pastUc = new uc_squareCard() { contentId = pastCatalogItem_category };
                    pastUc = gridCatigories.Children.OfType<uc_squareCard>().Where(x => x.Name.ToString() == "CardName" + pastUc.contentId).FirstOrDefault();
                    if (pastUc != null)
                    {
                        pastUc.squareCardBorderBrush = "#DFDFDF";
                    }

                }
                pastCatalogItem_category = uc.cardViewitem.category.categoryId;
                idCategory = uc.cardViewitem.category.categoryId;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        #region Catalog Items


        private int pastCatalogItem = -1;
        double gridCatigorieItems_ActualHeight = 0;
        double gridCatigorieItems_ActualWidth = 0;
        public void FN_refrishCatalogItem(List<Item> items, int rowCount, int columnCount, string cardType)
        {
            gridCatigorieItems.Children.Clear();
            gridCatigorieItems_ActualHeight = gridCatigorieItems.ActualHeight / rowCount;
            gridCatigorieItems_ActualWidth = gridCatigorieItems.ActualWidth / columnCount;
            int row = 0;
            int column = 0;
            foreach (var item in items)
            {

                CardViewItems itemCardView = new CardViewItems();
                itemCardView.item = item;
                itemCardView.cardType = cardType;
                itemCardView.row = row;
                itemCardView.column = column;
                FN_createRectangelCard(itemCardView);


                column++;
                if (column == columnCount)
                {
                    column = 0;
                    row++;
                }
            }
        }


        uc_squareCard FN_createRectangelCard(CardViewItems itemCardView, string BorderBrush = "#DFDFDF")
        {
            uc_squareCard uc = new uc_squareCard(itemCardView, gridCatigorieItems_ActualHeight, gridCatigorieItems_ActualWidth);
            uc.squareCardBorderBrush = BorderBrush;
            uc.Name = "CardName" + itemCardView.item.itemId;
            Grid.SetRow(uc, itemCardView.row);
            Grid.SetColumn(uc, itemCardView.column);
            gridCatigorieItems.Children.Add(uc);
            uc.MouseDown += this.ucItemMouseDown;
            return uc;
        }

        private void ucItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 0)
                doubleClickItem(sender);
        }
        private void doubleClickItem(object sender)
        {
            try
            {
                uc_squareCard uc = (uc_squareCard)sender;
                uc = gridCatigorieItems.Children.OfType<uc_squareCard>().Where(x => x.Name.ToString() == "CardName" + uc.cardViewitem.item.itemId).FirstOrDefault();

                uc.squareCardBorderBrush = "#078181";

                if (pastCatalogItem != -1 && pastCatalogItem != uc.cardViewitem.item.itemId)
                {
                    var pastUc = new uc_squareCard() { contentId = pastCatalogItem };
                    pastUc = gridCatigorieItems.Children.OfType<uc_squareCard>().Where(x => x.Name.ToString() == "CardName" + pastUc.contentId).FirstOrDefault();
                    if (pastUc != null)
                    {
                        pastUc.squareCardBorderBrush = "#DFDFDF";
                    }

                }
                pastCatalogItem = uc.cardViewitem.item.itemId;
                idItem = uc.cardViewitem.item.itemId;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

    }
}
