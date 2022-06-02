using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data;
using System.Resources;
using System.Reflection;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_locationsList.xaml
    /// </summary>
    public partial class wd_locationsList : Window
    {
        public wd_locationsList()
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

       
        public bool isActive;

        public int sectionId { get; set; }
        Classes.Section section = new Classes.Section();
        Classes.Section sectionModel = new Classes.Section();

        List<Location> allLocationsSource = new List<Location>();
        public List<Location> selectedLocationsSource = new List<Location>();

        List<Location> allLocations = new List<Location>();
        public List<Location> selectedLocations = new List<Location>();

        Location locationModel = new Location();
        Location location = new Location();

        /// <summary>
        /// Selcted Locations if selectedLocations Have Locations At the beginning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                
                    HelpClass.StartAwait(grid_locations);

                #region translate
                if (AppSettings.lang.Equals("en"))
                { 
  grid_locations.FlowDirection = FlowDirection.LeftToRight; }
                else
                { 
  grid_locations.FlowDirection = FlowDirection.RightToLeft; }

                translat();
                #endregion

                section = await sectionModel.getById(sectionId);

                allLocationsSource = await locationModel.Get();
                allLocationsSource = allLocationsSource.Where(x => x.branchId == section.branchId && x.isFreeZone != 1 && x.isKitchen != 1 ).ToList();

                var query = allLocationsSource.Where(i => i.sectionId == sectionId && i.isFreeZone != 1 && i.isKitchen != 1 && i.isActive == 1);
                selectedLocationsSource = query.ToList();

                allLocations.AddRange(allLocationsSource);
                selectedLocations.AddRange(selectedLocationsSource);

                //remove selected locations from all locations
                foreach (var i in selectedLocations)
                {
                    allLocations.Remove(i);
                }
                /////////////////////////////////////////////////
                foreach (var i in allLocations)
                {
                    i.x = i.x.Trim() + i.y.Trim() + i.z.Trim();
                }

                foreach (var i in selectedLocations)
                {
                    i.x = i.x.Trim() + i.y.Trim() + i.z.Trim();
                }

                lst_allLocations.ItemsSource = allLocations;
                lst_allLocations.SelectedValuePath = "x";
                lst_allLocations.DisplayMemberPath = "locationId";

                lst_selectedLocations.ItemsSource = selectedLocations;
                lst_selectedLocations.SelectedValuePath = "x";
                lst_selectedLocations.DisplayMemberPath = "locationId";
                HelpClass.EndAwait(grid_mainGrid);

                
                    HelpClass.EndAwait(grid_locations);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_locations);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translat()
        {
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            lst_allLocations.Columns[0].Header = AppSettings.resourcemanager.GetString("trLocation");
            lst_selectedLocations.Columns[0].Header = AppSettings.resourcemanager.GetString("trSelectedLocations");

            txt_locations.Text = AppSettings.resourcemanager.GetString("trLocation");
            txt_location.Text = AppSettings.resourcemanager.GetString("trLocation");
            txt_selectedLocations.Text = AppSettings.resourcemanager.GetString("trSelectedLocations");
            tt_searchX.Content = AppSettings.resourcemanager.GetString("trX");
            tt_searchY.Content = AppSettings.resourcemanager.GetString("trY");
            tt_searchZ.Content = AppSettings.resourcemanager.GetString("trZ");

            tt_selectAllItem.Content = AppSettings.resourcemanager.GetString("trSelectAllItems");
            tt_unselectAllItem.Content = AppSettings.resourcemanager.GetString("trUnSelectAllItems");
            tt_selectItem.Content = AppSettings.resourcemanager.GetString("trSelectOneItem");
            tt_unselectItem.Content = AppSettings.resourcemanager.GetString("trUnSelectOneItem");

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
                HelpClass.ExceptionMessage(ex,this);
            }
        }
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                
                    HelpClass.StartAwait(grid_locations);

                int s = await location.saveLocationsSection(selectedLocations , sectionId, MainWindow.userLogin.userId);

                isActive = true;
                this.Close();

                
                    HelpClass.EndAwait(grid_locations);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_locations);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isActive = false;
            this.Close();
        }

        private void Lst_allLocations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_selectedLocation_Click(null, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Lst_selectedLocations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_unSelectedLocation_Click(null, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private   void Btn_selectedAll_Click(object sender, RoutedEventArgs e)
        {//select all
            try
            {
                int x = allLocations.Count;
                for (int i = 0; i < x; i++)
                {
                    lst_allLocations.SelectedIndex = 0;
                    Btn_selectedLocation_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_selectedLocation_Click(object sender, RoutedEventArgs e)
        {//select one
            try
            {
                location = lst_allLocations.SelectedItem as Location;
                if (location != null)
                {
                    allLocations.Remove(location);
                    selectedLocations.Add(location);

                    lst_allLocations.ItemsSource = allLocations;
                    lst_selectedLocations.ItemsSource = selectedLocations;

                    lst_allLocations.Items.Refresh();
                    lst_selectedLocations.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_unSelectedLocation_Click(object sender, RoutedEventArgs e)
        {//unselect one
            try
            {
                location = lst_selectedLocations.SelectedItem as Location;
                if (location != null)
                {
                    selectedLocations.Remove(location);

                    allLocations.Add(location);

                    lst_allLocations.ItemsSource = allLocations;
                    lst_selectedLocations.ItemsSource = selectedLocations;

                    lst_allLocations.Items.Refresh();
                    lst_selectedLocations.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private   void Btn_unSelectedAll_Click(object sender, RoutedEventArgs e)
        {//unselect all
            try
            {
                int x = selectedLocations.Count;
                for (int i = 0; i < x; i++)
                {
                    lst_selectedLocations.SelectedIndex = 0;
                    Btn_unSelectedLocation_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Txb_searchlocations_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                lst_allLocations.ItemsSource = allLocations.Where(x => (x.x.ToLower().Contains(txb_searchX.Text.ToLower()) &&
                x.y.ToLower().Contains(txb_searchY.Text.ToLower()) &&
                x.z.ToLower().Contains(txb_searchZ.Text.ToLower())
                ) && x.isActive == 1);
            }
            catch (Exception ex)
            {
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

        private void Grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //// Have to do this in the unusual case where the border of the cell gets selected.
            //// and causes a crash 'EditItem is not allowed'
            e.Cancel = true;
        }
    }

}
