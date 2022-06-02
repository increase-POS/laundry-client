using netoaster;
using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for wd_diningHallTables.xaml
    /// </summary>
    /// 
    public partial class wd_diningHallTables : Window
    {
        string basicsPermission = "reservationsUpdate_basics";
        public Invoice invoice { get; set; }
        public bool isOk { get; set; }
        TablesReservation nextReservation = new TablesReservation();
        public List<Tables> selectedTables = new List<Tables>();
        List<Tables> gridTables = new List<Tables>();
        public List<Tables> reservationTables = new List<Tables>();
        Tables table = new Tables();
        int _PersonsCount = 0;
        public wd_diningHallTables() 
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
                    //Btn_select_Click(null, null);
                }
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
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                } 
                translate();

                txt_totalCurrency.Text = AppSettings.Currency;
                FillCombo.FillTablesStatus(cb_searchStatus);

                #region loading
                loadingList = new List<keyValueBool>();
                bool isDone = true;
                loadingList.Add(new keyValueBool { key = "loading_tables", value = false });
                loadingList.Add(new keyValueBool { key = "loading_reservations", value = false });
                loadingList.Add(new keyValueBool { key = "loading_fillSectionCombo", value = false });

                loading_tables();
                loading_reservations();
                loading_fillSectionCombo();
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
        
                Search();
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
            #region set text
            txt_title.Text = AppSettings.resourcemanager.GetString("trTables");
            txt_details.Text = AppSettings.resourcemanager.GetString("trDetails");
            txt_nextReservation.Text = AppSettings.resourcemanager.GetString("trNextReservation");
            txt_tableName.Text = AppSettings.resourcemanager.GetString("trTableName");
            txt_tableStatus.Text = AppSettings.resourcemanager.GetString("trTableStatus");
            tb_tableAvailable.Text = AppSettings.resourcemanager.GetString("trTableIsAvailable");
            txt_invoiceCode.Text = AppSettings.resourcemanager.GetString("trInvoiceCode");
            txt_startTime.Text = AppSettings.resourcemanager.GetString("trStartTime");
            txt_endTime.Text = AppSettings.resourcemanager.GetString("trExpectedEndTime");
            txt_invCustomer.Text = AppSettings.resourcemanager.GetString("trCustomer");
            txt_memberShip.Text = AppSettings.resourcemanager.GetString("trMembership");
            txt_waiter.Text = AppSettings.resourcemanager.GetString("trWaiter");
            txt_total.Text = AppSettings.resourcemanager.GetString("trTotal");
            txt_preparingOrders.Text = AppSettings.resourcemanager.GetString("trPreparingOrders");
            txt_date.Text = AppSettings.resourcemanager.GetString("trDate");
            txt_reservStartTime.Text = AppSettings.resourcemanager.GetString("trStartTime");
            txt_reservEndTime.Text = AppSettings.resourcemanager.GetString("trEndTime");
            txt_personsCount.Text = AppSettings.resourcemanager.GetString("trPersonsCount");

            txt_statusEmpty.Text = AppSettings.resourcemanager.GetString("trEmpty");
            txt_statusOpen.Text = AppSettings.resourcemanager.GetString("trOpened");
            txt_statusReservated.Text = AppSettings.resourcemanager.GetString("trReserved");

            txt_tablesContainer.Text = AppSettings.resourcemanager.GetString("trTables");
            txt_reservationsContainer.Text = AppSettings.resourcemanager.GetString("trReservations");

            txt_table.Text = AppSettings.resourcemanager.GetString("trTable");
            #endregion

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_searchSection, AppSettings.resourcemanager.GetString("trSectionHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_searchStatus, AppSettings.resourcemanager.GetString("trStatusHint"));


            #region tooltip
            btn_refresh.ToolTip = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            btn_colse.ToolTip = AppSettings.resourcemanager.GetString("trClose");

            #endregion

             btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");
            txt_mergeTable.Text = AppSettings.resourcemanager.GetString("trMergeTable");
            txt_confirmOpen.Text = AppSettings.resourcemanager.GetString("trConfirmAndOpen");
            txt_cancleOpen.Text = AppSettings.resourcemanager.GetString("trCancleAndOpen");
            btn_open2.Content = AppSettings.resourcemanager.GetString("trOpen");
            btn_open1.Content = AppSettings.resourcemanager.GetString("trOpen");
            txt_mergeInvTable.Text = AppSettings.resourcemanager.GetString("trMergeTable");
            txt_changeTable.Text = AppSettings.resourcemanager.GetString("trChangeTable");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            dg_reservation.Columns[0].Header = AppSettings.resourcemanager.GetString("trCode");
            dg_reservation.Columns[1].Header = AppSettings.resourcemanager.GetString("trDate");
            dg_reservation.Columns[2].Header = AppSettings.resourcemanager.GetString("time");
            dg_reservation.Columns[3].Header = AppSettings.resourcemanager.GetString("trCount");
            dg_reservation.Columns[4].Header = AppSettings.resourcemanager.GetString("trCustomer");
            dg_reservation.Columns[5].Header = AppSettings.resourcemanager.GetString("trStatus");

        }
        #region loading
        List<keyValueBool> loadingList;
        async Task loading_tables()
        {
            try
            {
                await refreshTablesList();
            }
            catch { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_tables"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        async Task loading_reservations()
        {
            //try
            {
                await refreshReservationsList();
            }
            //catch { }
            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_reservations"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        async Task loading_fillSectionCombo()
        {

            await FillCombo.FillComboHallSectionsWithDefault(cb_searchSection);

            foreach (var item in loadingList)
            {
                if (item.key.Equals("loading_fillSectionCombo"))
                {
                    item.value = true;
                    break;
                }
            }
        }

        #endregion
        #region events
        private void Cb_search_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Refresh & Search
        int sectionId = 0;
         void Search()
        {
            tablesQuery = tablesList;
            string textSearch = tb_search.Text;

            if(textSearch != "")
                tablesQuery = tablesQuery.Where(x => x.name == textSearch).ToList();
            #region section search
            if (cb_searchSection.SelectedIndex > 0)
            {
                sectionId = (int)cb_searchSection.SelectedValue;
                tablesQuery = tablesQuery.Where(s => s.sectionId == sectionId).ToList();
            }
            else
                sectionId = 0;
            #endregion

            #region status search
            if (cb_searchStatus.SelectedIndex > 0)
                tablesQuery = tablesQuery.Where(s => s.status == cb_searchStatus.SelectedValue.ToString()).ToList();
            #endregion
            BuildTablesDesign();
        }
        async Task refreshTablesList()
        {
            tablesList = await FillCombo.table.GetTablesForDinning(MainWindow.branchLogin.branchId,DateTime.Now.ToString()); 
        }
        async Task refreshReservationsList()
        {
            reservationsList = await reservation.Get(MainWindow.branchLogin.branchId);
            //reservationsList = reservationsList.Where(x => DateTime.Parse( x.reservationDate.ToString().Split(' ')[0]) >= DateTime.Parse(DateTime.Now.ToString().Split(' ')[0])).ToList();
            dg_reservation.ItemsSource = reservationsList;
        }
        #endregion      
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
            this.Close();
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            // yasin
            sv_detailsContainer.Visibility = Visibility.Collapsed;
            brd_detailsContainer.Visibility = Visibility.Collapsed;
            ////
            grid_emptyTableButtons.Visibility = Visibility.Collapsed;
            grid_openTableButtons.Visibility = Visibility.Collapsed;
            grid_reservatedTableButtons.Visibility = Visibility.Collapsed;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        #region table
        List<Tables> tablesList = new List<Tables>();
        List<Tables> tablesQuery = new List<Tables>();
        int tableId = 0;
        IEnumerable<TablesReservation> reservationsList;
        TablesReservation reservation = new TablesReservation();
        void BuildTablesDesign()
        {
            wp_tablesContainer.Children.Clear();

            foreach (var item in tablesQuery)
            {
                #region button
                Button tableButton = new Button();
                tableButton.Tag = item.name;
                tableButton.Margin = new Thickness(5);
                tableButton.Padding = new Thickness(0);
                tableButton.Background = null;
                tableButton.BorderBrush = null;

                if (item.personsCount <= 2)
                {
                    tableButton.Height = 90;
                    tableButton.Width = 80;
                } 
                else if (item.personsCount == 3)
                {
                    tableButton.Height = 130;
                    tableButton.Width = 90;
                }
                 else if (item.personsCount == 4)
                {
                    tableButton.Height = 140;
                    tableButton.Width = 100;
                }
                 else if (item.personsCount == 5)
                {
                    tableButton.Height = 150;
                    tableButton.Width = 110;
                }
                 else if (item.personsCount == 6)
                {
                    tableButton.Height = 160;
                    tableButton.Width = 120;
                }
                 else if (item.personsCount == 7)
                {
                    tableButton.Height = 170;
                    tableButton.Width = 130;
                }
                 else if (item.personsCount == 8)
                {
                    tableButton.Height = 180;
                    tableButton.Width = 140;
                }
                 else if (item.personsCount == 9)
                {
                    tableButton.Height = 190;
                    tableButton.Width = 150;
                }
                else if (item.personsCount > 9)
                {
                    tableButton.Height = 200;
                    tableButton.Width = 160;
                }
                
                tableButton.Click += tableButton_Click;

                #region Grid Container
                Grid gridContainer = new Grid();
                int rowCount = 3;
                RowDefinition[] rd = new RowDefinition[rowCount];
                for (int i = 0; i < rowCount; i++)
                {
                    rd[i] = new RowDefinition();
                }
                rd[0].Height = new GridLength(1, GridUnitType.Star);
                rd[1].Height = new GridLength(20, GridUnitType.Pixel);
                rd[2].Height = new GridLength(20, GridUnitType.Pixel);
                for (int i = 0; i < rowCount; i++)
                {
                    gridContainer.RowDefinitions.Add(rd[i]);
                }
                /////////////////////////////////////////////////////
                #region Path table
                Path pathTable = new Path();
                pathTable.Stretch = Stretch.Fill;
                pathTable.FlowDirection = FlowDirection.LeftToRight;
                pathTable.Margin = new Thickness(5);

                if (item.status == "opened" || item.status == "openedReserved")
                    pathTable.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
                else if (item.status == "reserved") 
                    pathTable.Fill = Application.Current.Resources["BlueTables"] as SolidColorBrush;
                else
                    pathTable.Fill = Application.Current.Resources["GreenTables"] as SolidColorBrush;

                if (item.personsCount <= 2)
                pathTable.Data = App.Current.Resources["tablePersons2"] as Geometry;
                else if (item.personsCount == 3)
                pathTable.Data = App.Current.Resources["tablePersons3"] as Geometry;
                else if (item.personsCount == 4)
                pathTable.Data = App.Current.Resources["tablePersons4"] as Geometry;
                else if (item.personsCount == 5)
                pathTable.Data = App.Current.Resources["tablePersons5"] as Geometry;
                else if (item.personsCount == 6)
                pathTable.Data = App.Current.Resources["tablePersons6"] as Geometry;
                else if (item.personsCount == 7)
                pathTable.Data = App.Current.Resources["tablePersons7"] as Geometry;
                else if (item.personsCount == 8)
                pathTable.Data = App.Current.Resources["tablePersons8"] as Geometry;
                else if (item.personsCount == 9)
                pathTable.Data = App.Current.Resources["tablePersons9"] as Geometry;
                else if (item.personsCount > 9)
                pathTable.Data = App.Current.Resources["tablePersons9Plus"] as Geometry;

                gridContainer.Children.Add(pathTable);
                #endregion
                #region   personCount 
                if (item.personsCount > 9)
                {
                    var itemPersonCountText = new TextBlock();
                    itemPersonCountText.Text = item.personsCount.ToString();
                    itemPersonCountText.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    itemPersonCountText.FontSize = 32;
                    itemPersonCountText.VerticalAlignment = VerticalAlignment.Center;
                    itemPersonCountText.HorizontalAlignment = HorizontalAlignment.Center;
                    gridContainer.Children.Add(itemPersonCountText);
                }
                #endregion
                #region   name
                var itemNameText = new TextBlock();
                itemNameText.Text = item.name;
                itemNameText.VerticalAlignment = VerticalAlignment.Center;
                itemNameText.HorizontalAlignment = HorizontalAlignment.Center;
                itemNameText.Foreground = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                Grid.SetRow(itemNameText, 1);
                gridContainer.Children.Add(itemNameText);
                #endregion
                #region   status
                var itemStatusText = new TextBlock();

                if (item.status == "opened" || item.status == "openedReserved")
                    itemStatusText.Text = AppSettings.resourcemanager.GetString("trOpened");
                else if (item.status == "reserved")
                    itemStatusText.Text = AppSettings.resourcemanager.GetString("trReserved");
                else
                    itemStatusText.Text = AppSettings.resourcemanager.GetString("trEmpty");
                //itemStatusText.Text = item.status;
                itemStatusText.VerticalAlignment = VerticalAlignment.Center;
                itemStatusText.HorizontalAlignment = HorizontalAlignment.Center;
                itemStatusText.Foreground = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                Grid.SetRow(itemStatusText, 2);
                gridContainer.Children.Add(itemStatusText);
                #endregion
                tableButton.Content = gridContainer;
                
                #endregion
                wp_tablesContainer.Children.Add(tableButton);
                #endregion
            }
         }
        async void tableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                var button = sender as Button;
                string tableName = button.Tag.ToString();
                tableId = tablesList.Where(x => x.name == tableName).Select(x => x.tableId).FirstOrDefault();
                await showDetails();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async Task showDetails()
        {

            // yasin
            sv_detailsContainer.Visibility = Visibility.Visible;
            brd_detailsContainer.Visibility = Visibility.Visible;
            ////
            ///
            selectedTables = new List<Tables>();           

            table = tablesList.Where(x => x.tableId == tableId).FirstOrDefault();
            if(table != null)
            {
                tb_tableName.Text = table.name;
                tb_tableStatus.Text = AppSettings.resourcemanager.GetString(table.status);
           
            switch (table.status)
            {
                case "empty":
                    grid_emptyTableDetails.Visibility = Visibility.Visible;
                    grid_emptyTableButtons.Visibility = Visibility.Visible;
                    dp_details.Visibility = Visibility.Visible;

                    grid_openTableDetails.Visibility = Visibility.Collapsed;
                    grid_tables.Visibility = Visibility.Collapsed;
                    grid_openTableButtons.Visibility = Visibility.Collapsed;

                    break;
                case "opened":
                case "openedReserved":
                    invoice = await FillCombo.table.GetTableInvoice(table.tableId);

                    selectedTables = invoice.tables;
                   
                    #region view - display
                    grid_openTableDetails.Visibility = Visibility.Visible;
                    grid_openTableButtons.Visibility = Visibility.Visible;
                    dp_details.Visibility = Visibility.Visible;

                    grid_emptyTableButtons.Visibility = Visibility.Collapsed;
                    grid_emptyTableDetails.Visibility = Visibility.Collapsed;

                    grid_reservatedTableButtons.Visibility = Visibility.Collapsed;
                    dp_reservatedTableTitle.Visibility = Visibility.Collapsed;
                    grid_reservatedTableDetails.Visibility = Visibility.Collapsed;

                    gridTables = selectedTables.Where(x => x.tableId != table.tableId).ToList();
                        if (gridTables.Count > 0)
                        {
                            btn_changeTable.Visibility = Visibility.Collapsed; // change table only if invoice for one table
                            grid_tables.Visibility = Visibility.Visible;
                            dg_tables.ItemsSource = null;
                            dg_tables.ItemsSource = gridTables;
                        }
                        else
                        {
                            grid_tables.Visibility = Visibility.Collapsed;
                            btn_changeTable.Visibility = Visibility.Visible;
                        }
                        #endregion

                        tb_InvoiceCode.Text = invoice.invNumber;
                    tb_startTime.Text = invoice.invDate.ToString().Split(' ')[1];
                    TimeSpan startTime = TimeSpan.Parse(invoice.invDate.ToString().Split(' ')[1]);
                    TimeSpan timeStaying = TimeSpan.FromHours(AppSettings.time_staying);
                    tb_endTime.Text = startTime.Add(timeStaying).ToString();
                    tb_customerName.Text = invoice.agentName;
                    tb_total.Text = invoice.totalNet.ToString();
                    break;
                case "reserved":
                    grid_emptyTableDetails.Visibility = Visibility.Collapsed;
                    dp_details.Visibility = Visibility.Collapsed;
                    grid_emptyTableButtons.Visibility = Visibility.Collapsed;

                    break;
            }

            #region next reservation

            nextReservation = new TablesReservation();
            foreach (var res in reservationsList)
            {
                var found = res.tables.Where(x => x.tableId == tableId).FirstOrDefault();
                if (found != null)
                {
                    nextReservation = res;
                    break;
                }
            }
            if (nextReservation.reservationId != 0)
            {             
                grid_emptyTableButtons.Visibility = Visibility.Collapsed;

                grid_reservatedTableDetails.Visibility = Visibility.Visible;
                dp_reservatedTableTitle.Visibility = Visibility.Visible;

                tb_reservCode.Text = nextReservation.code;
                tb_date.Text = nextReservation.reservationDate.ToString().Split(' ')[0];
                tb_reservStartTime.Text = nextReservation.reservationTime.ToString().Split(' ')[1];
                tb_reservEndTime.Text = nextReservation.endTime.ToString().Split(' ')[1];
                tb_personsCount.Text = nextReservation.personsCount.ToString();
                tb_reservCustomerName.Text = nextReservation.customerName;

                if (table.status != "open" && table.status != "openedReserved")
                {
                    grid_reservatedTableButtons.Visibility = Visibility.Visible;

                    grid_openTableDetails.Visibility = Visibility.Collapsed;
                    grid_openTableButtons.Visibility = Visibility.Collapsed;
                    dp_details.Visibility = Visibility.Collapsed;
                    grid_tables.Visibility = Visibility.Collapsed;
                    //if (nextReservation.tables.Count > 0)
                    //{
                    //    grid_tables.Visibility = Visibility.Visible;
                    //    dg_tables.ItemsSource = nextReservation.tables;
                    //}

                }
            }
            else
            {
                dp_reservatedTableTitle.Visibility = Visibility.Collapsed;
                grid_reservatedTableDetails.Visibility = Visibility.Collapsed;
                grid_reservatedTableButtons.Visibility = Visibility.Collapsed;
            }

                #endregion
            }
        }

        #endregion

        private async  void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await refreshTablesList();
                Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #region container

        private void Btn_tablesContainer_Click(object sender, RoutedEventArgs e)
        {
            sp_tablesStatus.Visibility = Visibility.Visible;
            wp_tablesContainer.Visibility = Visibility.Visible;
            path_tablesContainer.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
            txt_tablesContainer.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;

            grid_reservationsContainer.Visibility = Visibility.Collapsed;
            path_reservationsContainer.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            txt_reservationsContainer.Foreground = Application.Current.Resources["SecondColor"] as SolidColorBrush;
        }

        private void Btn_reservationsContainer_Click(object sender, RoutedEventArgs e)
        {
            sp_tablesStatus.Visibility = Visibility.Collapsed;
            wp_tablesContainer.Visibility = Visibility.Collapsed;
            path_tablesContainer.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;
            txt_tablesContainer.Foreground = Application.Current.Resources["SecondColor"] as SolidColorBrush;

            grid_reservationsContainer.Visibility = Visibility.Visible;
            path_reservationsContainer.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
            txt_reservationsContainer.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
        }

        #endregion
      
        #region Button In DataGrid

       async void cancelRowinDatagridTable(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                #region Accept
                grid_ucInvoice.Opacity = 0.2;
                wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxContinue");
                w.ShowDialog();
                grid_ucInvoice.Opacity = 1;
                #endregion
                if (w.isOk)
                {
                    for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                        if (vis is DataGridRow)
                        {
                            Tables row = (Tables)dg_tables.SelectedItems[0];
                            selectedTables.Remove(row);
                            int res = await FillCombo.invoice.updateInvoiceTables(invoice.invoiceId,selectedTables,invoice.reservationId);
                            if (res > 0)
                            {                              
                                gridTables = selectedTables.Where(x => x.tableId != table.tableId).ToList();
                                if (gridTables.Count > 0)
                                {
                                    dg_tables.ItemsSource = null;
                                    dg_tables.ItemsSource = gridTables;
                                }
                                else
                                    grid_tables.Visibility = Visibility.Collapsed;

                                await refreshTablesList();
                                Search();
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

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

        async void confirmRowinDatagridReservation(object sender, RoutedEventArgs e)
        {
            try
            {
                //delete
                if (dg_reservation.SelectedIndex != -1)
                {
                    reservation = dg_reservation.SelectedItem as TablesReservation;
                    //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "confirm"))
                    //{
                        HelpClass.StartAwait(grid_main);
                        #region Accept
                        grid_ucInvoice.Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxContinue");
                        w.ShowDialog();
                        grid_ucInvoice.Opacity = 1;
                        #endregion
                        if (w.isOk)
                        {
                            int res = await reservation.updateReservationStatus(reservation.reservationId, "confirm", MainWindow.userLogin.userId);
                            if (res > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("popConfirmed"), animation: ToasterAnimation.FadeIn);
                            res = await openInvoiceForReserve(reservation);

                            await refreshReservationsList();
                                await refreshTablesList();
                                Search();
                                await showDetails();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                        HelpClass.EndAwait(grid_main);
                    //}
                    //else
                    //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        async void cancelRowinDatagridReservation(object sender, RoutedEventArgs e)
        {
            try
            {
                //delete
                if (dg_reservation.SelectedIndex != -1)
                {
                    reservation = dg_reservation.SelectedItem as TablesReservation;
                    //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete"))
                    //{
                        HelpClass.StartAwait(grid_main);
                        #region Accept
                        grid_ucInvoice.Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxContinue");
                        w.ShowDialog();
                        grid_ucInvoice.Opacity = 1;
                        #endregion
                        if (w.isOk)
                        {
                            int res = await reservation.updateReservationStatus(reservation.reservationId, "cancle", MainWindow.userLogin.userId);
                            if (res > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("popCanceled"), animation: ToasterAnimation.FadeIn);
                                await refreshReservationsList();
                                await refreshTablesList();
                                Search();
                                await showDetails();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                        HelpClass.EndAwait(grid_main);
                    //}

                    //else
                    //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                }
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        #endregion

        #region opened table buttons
        private async void Btn_mergeInvTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "merge"))
                //{
                    HelpClass.StartAwait(grid_main);
                    grid_ucInvoice.Opacity = 0.2;

                    wd_tablesList w = new wd_tablesList();
                    w.page = "dinningHall-invoice";
                    w.selectedTables = selectedTables.ToList();
                    w.invDate = (DateTime)invoice.invDate;

                    w.ShowDialog();

                if (w.DialogResult == true)
                {

                    if (w.selectedTables.Count > 0)
                    {
                        selectedTables = w.selectedTables;
                        #region validate table availability 
                        DateTime invDate = DateTime.Parse(invoice.invDate.ToString().Split(' ')[0]);
                        DateTime startTime = DateTime.Parse(invoice.invDate.ToString().Split(' ')[1]);
                        TimeSpan timeStaying = TimeSpan.FromHours(AppSettings.time_staying);
                        DateTime endTime = startTime.Add(timeStaying);
                        bool valid = await validateReservationTime(invDate, startTime, endTime, selectedTables, invoice.invoiceId);
                        #endregion
                        if (valid)
                        {
                            //save
                            int res = await FillCombo.invoice.updateInvoiceTables(invoice.invoiceId, selectedTables,invoice.reservationId);
                            if (res > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await refreshTablesList();
                                Search();
                                await showDetails();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("oneTableAtLeast"), animation: ToasterAnimation.FadeIn);

                }

                grid_ucInvoice.Opacity = 1;
                    HelpClass.EndAwait(grid_main);
                //}
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_changeTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "change"))
                //{
                    HelpClass.StartAwait(grid_main);
                    grid_ucInvoice.Opacity = 0.2;

                    wd_selectTable w = new wd_selectTable();

                    w.ShowDialog();

                if (w.isOk == true)
                {
                    int newTableId = w.table.tableId;

                    #region validate table availability 
                    List<Tables> newTables = new List<Tables>();
                    newTables.Add(w.table);
                    DateTime invDate = DateTime.Parse(invoice.invDate.ToString().Split(' ')[0]);
                    DateTime startTime = DateTime.Parse(invoice.invDate.ToString().Split(' ')[1]);
                    TimeSpan timeStaying = TimeSpan.FromHours(AppSettings.time_staying);
                    DateTime endTime = startTime.Add(timeStaying);
                    bool valid = await validateReservationTime(invDate, startTime, endTime, newTables, invoice.invoiceId);
                    #endregion
                    if (valid)
                    {
                        //save
                        int res = await FillCombo.invoice.updateInvoiceTables(invoice.invoiceId, newTables,invoice.reservationId);
                        if (res > 0)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                            await refreshTablesList();
                            Search();
                            await showDetails();
                        }
                        else
                            Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                    //}
                }
                grid_ucInvoice.Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region reservation Buttons
        private async void Btn_mergeTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                //{
                    HelpClass.StartAwait(grid_main);
                    grid_ucInvoice.Opacity = 0.2;

                    wd_tablesList w = new wd_tablesList();
                    w.page = "dinningHall-reserve";
                    w.selectedTables = nextReservation.tables.ToList();
                    w.ShowDialog();

                if (w.DialogResult == true)
                {
                    _PersonsCount = 0;
                    foreach (var table in w.selectedTables)
                        _PersonsCount += table.personsCount;
                    if (w.selectedTables.Count > 0)
                    {
                        selectedTables = w.selectedTables;
                        #region validate table availability 
                        DateTime reservationDate = (DateTime)nextReservation.reservationDate;
                        DateTime startTime = (DateTime)nextReservation.reservationTime;
                        DateTime endTime = (DateTime)nextReservation.endTime;
                        bool valid = await validateReservationTime(reservationDate, startTime, endTime, selectedTables);
                        #endregion
                        if (valid)
                        {
                            //save
                            int res = await reservation.updateReservation(nextReservation, selectedTables);
                            if (res > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await refreshReservationsList();
                                await refreshTablesList();
                                Search();
                                await showDetails();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("oneTableAtLeast"), animation: ToasterAnimation.FadeIn);

                }
                grid_ucInvoice.Opacity = 1;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        async Task<Boolean> validateReservationTime(DateTime date, DateTime startTime, DateTime endTime,List<Tables> tables,int invoiceId =0)
        {
            bool valid = true;
            string message = "";
            
            foreach (Tables tb in tables)
            {
                int notReserved = await FillCombo.table.checkTableAvailabiltiy(tb.tableId, MainWindow.branchLogin.branchId,
                                                         date.ToString(),
                                                         startTime.ToString(),
                                                         endTime.ToString(), nextReservation.reservationId,invoiceId);

                if (notReserved == 0)
                {
                    valid = false;
                    if (message == "")
                        message += tb.name;
                    else
                        message += ", " + tb.name;
                }
                message += " ";
                if (!valid)
                    Toaster.ShowWarning(Window.GetWindow(this), message: message + AppSettings.resourcemanager.GetString("trReserved"), animation: ToasterAnimation.FadeIn);
            }
            return valid;
        }
        private async void Btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                //{
                    HelpClass.StartAwait(grid_main);
                    #region Accept
                    grid_ucInvoice.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxContinue");
                    w.ShowDialog();
                    grid_ucInvoice.Opacity = 1;
                    #endregion
                    if (w.isOk)
                    {
                        int res = await reservation.updateReservationStatus(nextReservation.reservationId, "confirm", MainWindow.userLogin.userId);
                        if (res > 0)
                        {
                            res = await openInvoiceForReserve(nextReservation);
                            if (res > 0)
                            {
                                //Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("popConfirmed"), animation: ToasterAnimation.FadeIn);
                                
                                isOk = true;
                                this.Close();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                        }
                        else
                            Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                    HelpClass.EndAwait(grid_main);
                //}
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_cancle_Click(object sender, RoutedEventArgs e)
        {
            try
            {//delete
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete"))
                //{
                    HelpClass.StartAwait(grid_main);
                    #region Accept
                    grid_ucInvoice.Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxContinue");
                    w.ShowDialog();
                    grid_ucInvoice.Opacity = 1;
                    #endregion
                    if (w.isOk)
                    {
                        int res = await reservation.updateReservationStatus(nextReservation.reservationId, "cancle", MainWindow.userLogin.userId);
                        if (res > 0)
                        {
                            res = await openEmptyInvoice();
                            if (res > 0)
                            {
                                //Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("popCanceled"), animation: ToasterAnimation.FadeIn);
                                
                                isOk = true;
                                this.Close();
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                          
                        }
                        else
                            Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    }
                    HelpClass.EndAwait(grid_main);
                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion
        #region open
        private async Task<int> openEmptyInvoice()
        {
            #region invoice object
            invoice = new Invoice();

            invoice.invNumber = await invoice.generateDialyInvNumber("s,sd,ssd,ss,tsd,ts",  MainWindow.branchLogin.branchId); 
            invoice.invType = "sd";

            if (AppSettings.invoiceTax_bool == true)
                invoice.tax = (decimal)AppSettings.invoiceTax_decimal;
            else
                invoice.tax = 0;

            invoice.branchCreatorId = MainWindow.branchLogin.branchId;
            invoice.posId = MainWindow.posLogin.posId;
            invoice.branchId = MainWindow.branchLogin.branchId;
            invoice.createUserId = MainWindow.userLogin.userId;
            #endregion
            #region table object
            List<Tables> invTables = new List<Tables>();
            invTables.Add(table);
            selectedTables = invTables;
            #endregion
            int res = await FillCombo.invoice.saveInvoiceWithTables(invoice, invTables);
            invoice.invoiceId = res;
            return res;
        }
        private async Task<int> openInvoiceForReserve(TablesReservation reserv)
        {
            #region invoice object
            invoice = new Invoice();

            invoice.invNumber = await invoice.generateDialyInvNumber("s,sd,ssd,ss,tsd,ts", MainWindow.branchLogin.branchId);
            invoice.invType = "sd";

            if (AppSettings.invoiceTax_bool == true)
                invoice.tax = (decimal)AppSettings.invoiceTax_decimal;
            else
                invoice.tax = 0;

            invoice.agentId = reserv.customerId;
            invoice.reservationId = reserv.reservationId;
            invoice.branchCreatorId = MainWindow.branchLogin.branchId;
            invoice.posId = MainWindow.posLogin.posId;
            invoice.branchId = MainWindow.branchLogin.branchId;
            invoice.createUserId = MainWindow.userLogin.userId;
            #endregion
            #region table object
            List<Tables> invTables = new List<Tables>();
            invTables.AddRange(reserv.tables);
            selectedTables = invTables;
            #endregion
            int res = await FillCombo.invoice.saveInvoiceWithTables(invoice, invTables);
            invoice.invoiceId = res;
            return res;
        }
        private async void Btn_open_Click(object sender, RoutedEventArgs e)
        {
            try
            {//delete
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "open"))
                //{
                    HelpClass.StartAwait(grid_main);
                    int res = await openEmptyInvoice();
                    if (res > 0)
                    {
                        //Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                      
                        isOk = true;
                        this.Close();
                    }
                    else
                        Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);

                //}
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            isOk = true;
            this.Close();
        }



        #endregion

      
    }
}
