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
    /// Interaction logic for wd_transBetweenOpenClose.xaml
    /// </summary>
    public partial class wd_transBetweenOpenClose : Window
    {

        Statistics statisticsModel = new Statistics();
        IEnumerable<OpenClosOperatinModel> cashesQuery;
        public int openCashTransID = 0 , closeCashTransID = 0;

        public wd_transBetweenOpenClose()
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
        {//load
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

                await fillDataGrid();

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        async Task fillDataGrid()
        {
            cashesQuery = await statisticsModel.GetTransBetweenOpenClose(openCashTransID , closeCashTransID);
            cashesQuery = cashesQuery.Where(c => c.transType != "c" && c.transType != "o");
            dg_transfers.ItemsSource = cashesQuery;
        }

        private void translat()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trOperations");
            dg_transfers.Columns[0].Header = AppSettings.resourcemanager.GetString("trNo");
            dg_transfers.Columns[1].Header = AppSettings.resourcemanager.GetString("trDate");
            dg_transfers.Columns[2].Header = AppSettings.resourcemanager.GetString("trDescription");
            dg_transfers.Columns[3].Header = AppSettings.resourcemanager.GetString("trCashTooltip");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            GC.Collect();
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
