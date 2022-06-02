using netoaster;
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

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_residentialSectorsList.xaml
    /// </summary>
    public partial class wd_residentialSectorsList : Window
    {
        ResidentialSectors residentialSector = new ResidentialSectors();
        List<ResidentialSectors> allSectorsSource = new List<ResidentialSectors>();
        List<ResidentialSectors> allSectors = new List<ResidentialSectors>();
        List<ResidentialSectors> allSectorsQuery = new List<ResidentialSectors>();
        List<ResidentialSectors> selectedSectorsSource = new List<ResidentialSectors>();

        ResidentialSectorsUsers residentialSectorsUserModel = new ResidentialSectorsUsers();
        ResidentialSectorsUsers residentialSectorsUser = new ResidentialSectorsUsers();
        List<ResidentialSectorsUsers> selectedSectors = new List<ResidentialSectorsUsers>();
        public int driverId = 0;

        public wd_residentialSectorsList()
        {
            InitializeComponent();
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

                
                allSectorsSource = await residentialSector.Get(); ////active sectors
                allSectorsSource = allSectorsSource.Where(r => r.isActive == 1).ToList();
                allSectors.AddRange(allSectorsSource);

                selectedSectorsSource = await residentialSector.GetResSectorsByUserId(driverId);

                //foreach (var v in selectedSectorsSource)
                //{
                //    residentialSectorsUser = await residentialSectorsUserModel.GetById(v.residentSecId);
                //    selectedSectors.Add(residentialSectorsUser);
                //}

                //remove selected items from all items
                foreach (var i in selectedSectorsSource)
                {
                    residentialSector = allSectors.Where(s => s.residentSecId == i.residentSecId).FirstOrDefault<ResidentialSectors>();
                    allSectors.Remove(residentialSector);
                }

                dg_selectedItems.ItemsSource = selectedSectorsSource;
                dg_selectedItems.SelectedValuePath = "residentSecId";
                dg_selectedItems.DisplayMemberPath = "name";

                dg_all.ItemsSource = allSectors;
                dg_all.SelectedValuePath = "residentSecId";
                dg_all.DisplayMemberPath = "name";

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
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
            
            txt_title.Text = AppSettings.resourcemanager.GetString("trResidentialSectors");
            txt_selectedItems.Text = AppSettings.resourcemanager.GetString("selectedResidentialSectors");

            txt_All.Text = AppSettings.resourcemanager.GetString("trAll") + " " + txt_title.Text;

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

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        string searchText = "";
        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = txb_search.Text;
                    
                allSectorsQuery = allSectors.Where(s => s.name.Contains(searchText)).ToList();
                dg_all.ItemsSource = allSectorsQuery;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Dg_all_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_selectedOne_Click(null, null);
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

        private void Btn_selectedAll_Click(object sender, RoutedEventArgs e)
        {//select all
            try
            {
                int x = 0;

               
                x = allSectors.Count;

                for (int i = 0; i < x; i++)
                {
                    dg_all.SelectedIndex = 0;
                    Btn_selectedOne_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_selectedOne_Click(object sender, RoutedEventArgs e)
        {   //select one
            try
            {
                residentialSector = dg_all.SelectedItem as ResidentialSectors;
                if (residentialSector != null)
                {
                    allSectors.Remove(residentialSector);
                    selectedSectorsSource.Add(residentialSector);
                    dg_selectedItems.ItemsSource = selectedSectorsSource;

                    //ResidentialSectorsUsers rs = new ResidentialSectorsUsers();
                    //rs.residentSecId = residentialSector.residentSecId;
                    //rs.userId = driverId;
                    //rs.notes = "";
                    //rs.createUserId = MainWindow.userLogin.userId;
                    //rs.updateUserId = MainWindow.userLogin.userId;

                    //selectedSectors.Add(rs);
                }
               
                dg_all.Items.Refresh();
                dg_selectedItems.Items.Refresh();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }

        private void Btn_unSelectedOne_Click(object sender, RoutedEventArgs e)
        {//unselect one
            try
            {
                residentialSector = dg_selectedItems.SelectedItem as ResidentialSectors;

                if (residentialSector != null)
                {
                    selectedSectorsSource.Remove(residentialSector);
                    dg_selectedItems.ItemsSource = selectedSectorsSource;

                    allSectors.Add(residentialSector);
                    dg_all.ItemsSource = allSectors;

                    //residentialSectorsUser = selectedSectors.Where(a => a.residentSecId == residentialSector.residentSecId).FirstOrDefault();
                    //selectedSectors.Remove(residentialSectorsUser);
                }
               
                dg_all.Items.Refresh();
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
              
                x = selectedSectorsSource.Count;

                for (int i = 0; i < x; i++)
                {
                    dg_selectedItems.SelectedIndex = 0;
                    Btn_unSelectedOne_Click(null, null);
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
                Btn_unSelectedOne_Click(null, null);
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
                HelpClass.StartAwait(grid_main);
                int s = 0;
                foreach (var item in selectedSectorsSource)
                {
                    ResidentialSectorsUsers rs = new ResidentialSectorsUsers();
                    rs.residentSecId = item.residentSecId;
                    rs.userId = driverId;
                    rs.notes = "";
                    rs.createUserId = MainWindow.userLogin.userId;
                    rs.updateUserId = MainWindow.userLogin.userId;

                    selectedSectors.Add(rs);
                }
                s = await residentialSectorsUser.UpdateResSectorsByUserId(selectedSectors, driverId, MainWindow.userLogin.userId);

                if (s <= 0)
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                else
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                    this.Close();
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
