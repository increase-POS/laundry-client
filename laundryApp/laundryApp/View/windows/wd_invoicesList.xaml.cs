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
    /// Interaction logic for wd_invoicesList.xaml
    /// </summary>
    public partial class wd_invoicesList : Window
    {
        public wd_invoicesList()
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
        List<Invoice> allInvoicesSource = new List<Invoice>();

        List<Invoice> allInvoices = new List<Invoice>();
        public List<Invoice> selectedInvoices = new List<Invoice>();

        Invoice invoiceModel = new Invoice();
        Invoice invoice = new Invoice();

        public int agentId = 0 , userId = 0, shippingCompanyId = 0;
        public decimal sum = 0;
        public string invType ;
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_invoices);

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_invoices.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_invoices.FlowDirection = FlowDirection.RightToLeft;
                }
                tb_moneyIcon.Text = AppSettings.Currency;
                translat();
                #endregion

                if (agentId != 0)
                    allInvoicesSource = await invoiceModel.getAgentInvoices(MainWindow.branchLogin.branchId , agentId , invType);
                else if(userId != 0)
                    allInvoicesSource = await invoiceModel.getUserInvoices(MainWindow.branchLogin.branchId , userId , invType);
                else if(shippingCompanyId != 0)
                    allInvoicesSource = await invoiceModel.getShipCompanyInvoices(MainWindow.branchLogin.branchId , shippingCompanyId , invType);

                allInvoices.AddRange(allInvoicesSource);
           
                lst_allInvoices.ItemsSource = allInvoices;
                lst_allInvoices.SelectedValuePath = "invNumber";
                lst_allInvoices.DisplayMemberPath = "invoiceId";

                lst_selectedInvoices.ItemsSource = selectedInvoices;
                lst_selectedInvoices.SelectedValuePath = "invNumber";
                lst_selectedInvoices.DisplayMemberPath = "invoiceId";


                HelpClass.EndAwait(grid_invoices);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_invoices);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translat()
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_invoice.Text = AppSettings.resourcemanager.GetString("trInvoices");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
            txt_invoices.Text = AppSettings.resourcemanager.GetString("trInvoices");
            txt_selectedInvoices.Text = AppSettings.resourcemanager.GetString("trSelectedInvoices");

            lst_allInvoices.Columns[0].Header = AppSettings.resourcemanager.GetString("trInvoiceNumber");
            lst_allInvoices.Columns[1].Header = AppSettings.resourcemanager.GetString("trTotal");

            lst_selectedInvoices.Columns[0].Header = AppSettings.resourcemanager.GetString("trInvoiceNumber");
            lst_selectedInvoices.Columns[1].Header = AppSettings.resourcemanager.GetString("trTotal");

            txt_sum.Text = AppSettings.resourcemanager.GetString("trSum");
            tb_moneyIcon.Text = AppSettings.Currency;

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

        private  void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            isActive = true;
            this.Close();
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isActive = false;
            this.Close();
        }

        private void Lst_allInvoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_selectedInvoice_Click(null, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Lst_selectedInvoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            { 
                Btn_unSelectedInvoice_Click(null, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private  void Btn_selectedAll_Click(object sender, RoutedEventArgs e)
        {//select all
            try
            {
                int x = allInvoices.Count;
                for (int i = 0; i < x; i++)
                {
                    lst_allInvoices.SelectedIndex = 0;
                    Btn_selectedInvoice_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_selectedInvoice_Click(object sender, RoutedEventArgs e)
        {//select one
            try
            {
                HelpClass.StartAwait(grid_invoices);

                invoice = lst_allInvoices.SelectedItem as Invoice;
                if (invoice != null)
                {
                    allInvoices.Remove(invoice);

                    selectedInvoices.Add(invoice);

                    lst_allInvoices.ItemsSource = allInvoices;
                    lst_selectedInvoices.ItemsSource = selectedInvoices;

                    lst_allInvoices.Items.Refresh();
                    lst_selectedInvoices.Items.Refresh();

                    decimal x = invoice.deserved ;

                    sum += x;

                    tb_sum.Text = " "+ HelpClass.DecTostring(sum) +" ";
                }
                
                HelpClass.EndAwait(grid_invoices);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_invoices);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_unSelectedInvoice_Click(object sender, RoutedEventArgs e)
        {//unselect one
            try
            {
                HelpClass.StartAwait(grid_invoices);

                invoice = lst_selectedInvoices.SelectedItem as Invoice;

                if (invoice != null)
                {
                    selectedInvoices.Remove(invoice);

                    allInvoices.Add(invoice);

                    lst_allInvoices.ItemsSource = allInvoices;
                    lst_selectedInvoices.ItemsSource = selectedInvoices;

                    lst_allInvoices.Items.Refresh();
                    lst_selectedInvoices.Items.Refresh();

                    decimal x = invoice.deserved;

                    sum -= x;

                    tb_sum.Text = " "+ HelpClass.DecTostring(sum) +" ";
                }
                
                HelpClass.EndAwait(grid_invoices);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_invoices);

                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private  void Btn_unSelectedAll_Click(object sender, RoutedEventArgs e)
        {//unselect all
            try
            {
                int x = selectedInvoices.Count;
                for (int i = 0; i < x; i++)
                {
                    lst_selectedInvoices.SelectedIndex = 0;
                    Btn_unSelectedInvoice_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            { 
                lst_allInvoices.ItemsSource = allInvoices.Where(x => (x.invNumber.ToLower().Contains(txb_search.Text.ToLower())) || 
                                                                     (x.total.ToString().ToLower().Contains(txb_search.Text.ToLower())));
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
