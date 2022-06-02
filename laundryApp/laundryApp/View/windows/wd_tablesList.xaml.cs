using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
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
    /// Interaction logic for wd_tablesList.xaml
    /// </summary>
    public partial class wd_tablesList : Window
    {
        public int sectionId = 0 ;
        public bool isActive;
        public string page;
        
        HallSection hallSection = new  HallSection();
        HallSection hallSectionModel = new HallSection();
        IEnumerable<Tables> allTableSource;
         IEnumerable<Tables> selectedTablesSource;
        List<Tables> allTables = new List<Tables>();
        List<Tables> tablesQuery = new List<Tables>();
        public List<Tables> selectedTables = new List<Tables>();
        Tables tableModel = new Tables();
        Tables table = new Tables();

        #region date parameters to get table satatus
        public DateTime invDate;
        #endregion
        
        string searchText = "";

        public wd_tablesList()
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
                HelpClass.StartAwait(grid_tableList);

                #region translate
                if (AppSettings.lang.Equals("en"))
                { 
                       grid_tableList.FlowDirection = FlowDirection.LeftToRight; }
                else
                { 
                  grid_tableList.FlowDirection = FlowDirection.RightToLeft; }

                translat();
                #endregion

                #region invoice tbale
                if(page == "dinningHall-reserve" || page == "dinningHall-invoice")
                {
                    TimeSpan startTime = TimeSpan.Parse(invDate.ToString().Split(' ')[1]);
                    TimeSpan timeStaying = TimeSpan.FromHours(AppSettings.time_staying);
                    TimeSpan endTime = startTime.Add(timeStaying);
                    allTableSource = await FillCombo.table.GetTablesForDinning(MainWindow.branchLogin.branchId, DateTime.Now.ToString());
                    allTableSource = allTableSource.Where(x => x.status != "opened" && x.status != "openedReserved");
                }
                #endregion
                #region reservation Tables
                else if (page == "reservationUpdate")
                {
                    allTableSource = await tableModel.Get(MainWindow.branchLogin.branchId, 0);
                }
                #endregion

                #region section Tables
                else
                {
                    hallSection = await hallSectionModel.GetById(sectionId);
                    allTableSource = await tableModel.Get(hallSection.branchId.Value, 0);
                    selectedTablesSource = await tableModel.Get(hallSection.branchId.Value, sectionId);
                    selectedTables.AddRange(selectedTablesSource.ToList());
                }
                #endregion

                #region
               
                allTables.AddRange(allTableSource.ToList());
               
                //remove selected items from all items
                foreach (var i in selectedTables)
                {
                    table = allTableSource.Where(s => s.tableId == i.tableId).FirstOrDefault<Tables>();
                    allTables.Remove(table);
                }

                dg_allItems.ItemsSource = allTables;
                dg_allItems.SelectedValuePath = "tableId";
                dg_allItems.DisplayMemberPath = "name";

                dg_selectedItems.ItemsSource = selectedTables;
                dg_selectedItems.SelectedValuePath = "tableId";
                dg_selectedItems.DisplayMemberPath = "name";
                #endregion

                HelpClass.EndAwait(grid_tableList); 
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_tableList);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translat()
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_searchitems, AppSettings.resourcemanager.GetString("trSearchHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            dg_allItems.Columns[0].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_selectedItems.Columns[0].Header = AppSettings.resourcemanager.GetString("trItem");

            txt_title.Text = AppSettings.resourcemanager.GetString("trTables");
            txt_items.Text = AppSettings.resourcemanager.GetString("trTables");
            txt_selectedItems.Text = AppSettings.resourcemanager.GetString("trSelectedTables");

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
        {//close
            DialogResult = false;
            this.Close();
        }

        private void Txb_searchitems_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                searchText = txb_searchitems.Text.ToLower();
                tablesQuery = allTables.Where(s => s.name.Contains(searchText)).ToList();
                dg_allItems.ItemsSource = tablesQuery;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //// Have to do this in the unusual case where the border of the cell gets selected.
            //// and causes a crash 'EditItem is not allowed'
            //e.Cancel = true;
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
                int x = allTables.Count();
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
                table = dg_allItems.SelectedItem as Tables;
                if (table != null)
                {
                    table.sectionId = sectionId;

                    allTables.Remove(table);
                    selectedTables.Add(table);

                    dg_allItems.ItemsSource = allTables;
                    dg_selectedItems.ItemsSource = selectedTables;

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
        {//unselect one
            try
            {
                table = dg_selectedItems.SelectedItem as Tables;

                table.sectionId = 0;

                allTables.Add(table);

                selectedTables.Remove(table);

                dg_allItems.ItemsSource = allTables;

                dg_allItems.Items.Refresh();
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
                x = selectedTables.Count();
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
            try
            {
                Btn_unSelectedItem_Click(null, null);
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
                HelpClass.StartAwait(grid_tableList);
                if (page == "reservationUpdate")
                {

                }
                #region section tables
                else
                {
                    await tableModel.AddTablesToSection(selectedTables.ToList(), sectionId, MainWindow.userLogin.userId);
                }
                #endregion
                DialogResult = true;
                this.Close();

                HelpClass.EndAwait(grid_tableList);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_tableList);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
