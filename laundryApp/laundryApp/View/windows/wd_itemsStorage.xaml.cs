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
    /// Interaction logic for wd_itemsStorage.xaml
    /// </summary>
    public partial class wd_itemsStorage : Window
    {
        List<Item> kitchenItems = new List<Item>();
        List<Item> itemsQuery = new List<Item>();
        int categoryId = 0;
        public string txtItemSearch;
        public wd_itemsStorage()
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
                //if (e.Key == Key.Return)
                //{
                //    Btn_select_Click(null, null);
                //}
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

                HelpClass.StartAwait(grid_main);

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translat();
                #endregion
                await refreshKitchenItems();
                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void translat()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trItemsStorage");
            txt_countTitle.Text = AppSettings.resourcemanager.GetString("trCount:");

            dg_items.Columns[0].Header = AppSettings.resourcemanager.GetString("trItemUnit");
            dg_items.Columns[1].Header = AppSettings.resourcemanager.GetString("trQuantity");
            dg_items.Columns[2].Header = AppSettings.resourcemanager.GetString("trExpireDate");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

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

        private async void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await Search();
            }
            catch { }
        }

        private void Dg_items_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Dg_items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
   
        async Task Search()
        {
            //search
            if (kitchenItems is null)
                await refreshKitchenItems();

            txtItemSearch = txb_search.Text.ToLower();

            itemsQuery = kitchenItems;
            itemsQuery = itemsQuery.Where(x => (x.code.ToLower().Contains(txtItemSearch) ||
                x.name.ToLower().Contains(txtItemSearch) ||
                x.details.ToLower().Contains(txtItemSearch)
                )).ToList();

            txt_count.Text = itemsQuery.Count.ToString();
            RefreshItemView();
        }
        private async Task refreshKitchenItems()
        {
            kitchenItems = await FillCombo.item.GetKitchenItemsWithUnits(MainWindow.branchLogin.branchId,categoryId);
        }
        void RefreshItemView()
        {
            dg_items.ItemsSource = itemsQuery;
        }

    }
}
