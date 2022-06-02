using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
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
    /// Interaction logic for wd_selectTable.xaml
    /// </summary>
    public partial class wd_selectTable : Window
    {
        public wd_selectTable()
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
            isOk = false;
            this.Close();
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_select_Click(btn_select, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public Tables table;
        List<Tables> tables = new List<Tables>();
        //public int tableId;
        public bool isOk { get; set; }
        public static List<string> requiredControlList = new List<string>();
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "tableId" };

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                await fillTableCombo();
                //cb_tableId.SelectedValue = tableId;

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
             txt_title.Text = AppSettings.resourcemanager.GetString("trTable");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_tableId, AppSettings.resourcemanager.GetString("trTableHint"));
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");
        }

        async Task fillTableCombo()
        {
            tables = await FillCombo.table.GetTablesForDinning(MainWindow.branchLogin.branchId, DateTime.Now.ToString());
            tables = tables.Where(x => x.status != "opened" && x.status != "openedReserved").ToList();

            cb_tableId.SelectedValuePath = "tableId";
            cb_tableId.DisplayMemberPath = "name";
            cb_tableId.ItemsSource = tables;
        }
        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            // if have id return true

            if (HelpClass.validate(requiredControlList, this))
            {
                isOk = true;

                int tableId = (int)cb_tableId.SelectedValue;
                table = tables.Where(x => x.tableId == tableId).FirstOrDefault();
                this.Close();
            }
        }
    }
}
