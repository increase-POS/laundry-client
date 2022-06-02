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
    /// Interaction logic for wd_agentMembership.xaml
    /// </summary>
    public partial class wd_membershipList : Window
    {
        public int membershipID = 0;
        public string membershipType = "";
        string _title = "";

        private static wd_membershipList _instance;
        public static wd_membershipList Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new wd_membershipList();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public wd_membershipList()
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

        #region customer
        List<Agent> allCustomersSource = new List<Agent>();
        List<Agent> allCustomers = new List<Agent>();
        List<Agent> allCustomersQuery = new List<Agent>();
        Agent customer = new Agent();
        List<Agent> selectedCustomersSource = new List<Agent>();
        List<AgentMemberships> selectedCustomers = new List<AgentMemberships>();
        AgentMemberships customerMembership = new AgentMemberships();
        Memberships membership = new Memberships();
        #endregion

        #region coupon
        List<Coupon> allCouponsSource = new List<Coupon>();
        List<Coupon> allCoupons = new List<Coupon>();
        List<Coupon> allCouponsQuery = new List<Coupon>();
        Coupon coupon = new Coupon();
        List<Coupon> selectedCouponsSource = new List<Coupon>();
        List<CouponsMemberships> selectedCoupons = new List<CouponsMemberships>();
        CouponsMemberships couponMembership = new CouponsMemberships();
        #endregion

        # region offer
        List<Offer> allOffersSource = new List<Offer>();
        List<Offer> allOffers = new List<Offer>();
        List<Offer> allOffersQuery = new List<Offer>();
        Offer offer = new Offer();
        List<Offer> selectedOffersSource = new List<Offer>();
        List<MembershipsOffers> selectedOffers = new List<MembershipsOffers>();
        MembershipsOffers offerMembership = new MembershipsOffers();
        #endregion

        #region invoice
        List<InvoicesClass> allInvoicesSource = new List<InvoicesClass>();
        List<InvoicesClass> allInvoices = new List<InvoicesClass>();
        List<InvoicesClass> allInvoicesQuery = new List<InvoicesClass>();
        InvoicesClass invoice = new InvoicesClass();
        List<InvoicesClass> selectedInvoicesSource = new List<InvoicesClass>();
        List<InvoicesClassMemberships> selectedInvoices = new List<InvoicesClassMemberships>();
        InvoicesClassMemberships invoiceMembership = new InvoicesClassMemberships();
        #endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                #region customer
                if (membershipType == "a")
                {
                    #region title
                    _title = "trCustomers";
                    col_customer.Width = new GridLength(1, GridUnitType.Star);
                    col_coupon.Width = new GridLength(0);
                    col_offer.Width = new GridLength(0);
                    col_invoice.Width = new GridLength(0);
                    #endregion

                    allCustomersSource = await customer.GetAgentsActive("c");////active customers
                    allCustomers.AddRange(allCustomersSource);

                    selectedCustomersSource = await customer.GetAgentsByMembershipId(membershipID);

                    foreach (var v in selectedCustomersSource)
                    {
                        customerMembership = new AgentMemberships();
                        customerMembership.agentId = v.agentId;
                        customerMembership.membershipId = membershipID;
                        customerMembership.createUserId = MainWindow.userLogin.userId;
                        customerMembership.updateUserId = MainWindow.userLogin.userId;
                        customerMembership.isActive = 1;
                       
                        selectedCustomers.Add(customerMembership);
                    }

                    //remove selected items from all items
                    foreach (var i in selectedCustomersSource)
                    {
                        customer = allCustomers.Where(s => s.agentId == i.agentId).FirstOrDefault<Agent>();
                        allCustomers.Remove(customer);
                    }

                    dg_selectedItems.ItemsSource = selectedCustomersSource;
                    dg_selectedItems.SelectedValuePath = "agentId";
                    dg_selectedItems.DisplayMemberPath = "name";

                    dg_all.ItemsSource = allCustomers;
                    dg_all.SelectedValuePath = "agentId";
                    dg_all.DisplayMemberPath = "name";
                }
                #endregion

                # region coupon
                if (membershipType == "c")
                {
                    #region title
                    _title = "trCoupons";
                    col_customer.Width = new GridLength(0);
                    col_coupon.Width = new GridLength(1, GridUnitType.Star);
                    col_offer.Width = new GridLength(0);
                    col_invoice.Width = new GridLength(0);
                    #endregion

                    allCouponsSource = await coupon.GetEffictiveCoupons();////active coupons
                    allCoupons.AddRange(allCouponsSource);

                    selectedCouponsSource = await coupon.GetCouponsByMembershipId(membershipID);

                    foreach(var v in selectedCouponsSource)
                    {
                        couponMembership = await couponMembership.GetById(v.couponMembershipId);
                        selectedCoupons.Add(couponMembership);
                    }

                    //remove selected items from all items
                    foreach (var i in selectedCouponsSource)
                    {
                        coupon = allCoupons.Where(s => s.cId == i.cId).FirstOrDefault<Coupon>();
                        allCoupons.Remove(coupon);
                    }

                    dg_selectedItems.ItemsSource = selectedCouponsSource;
                    dg_selectedItems.SelectedValuePath = "cId";
                    dg_selectedItems.DisplayMemberPath = "name";

                    dg_all.ItemsSource = allCoupons;
                    dg_all.SelectedValuePath = "cId";
                    dg_all.DisplayMemberPath = "name";
                }
                #endregion

                #region offer
                if (membershipType == "o")
                {
                    #region title
                    _title = "trOffers";
                    col_customer.Width = new GridLength(0);
                    col_coupon.Width = new GridLength(0);
                    col_offer.Width = new GridLength(1, GridUnitType.Star);
                    col_invoice.Width = new GridLength(0);
                    #endregion

                    allOffersSource = await offer.Get();////active offers
                    allOffersSource = allOffersSource.Where(o => o.isActive == 1).ToList();
                    allOffers.AddRange(allOffersSource);

                    selectedOffersSource = await offer.GetOffersByMembershipId(membershipID);
                    foreach (var v in selectedOffersSource)
                    {
                        offerMembership = await offerMembership.GetById(v.membershipOfferId);
                        selectedOffers.Add(offerMembership);
                    }
                    //remove selected items from all items
                    foreach (var i in selectedOffersSource)
                    {
                        offer = allOffers.Where(s => s.offerId == i.offerId).FirstOrDefault<Offer>();
                        allOffers.Remove(offer);
                    }

                    dg_selectedItems.ItemsSource = selectedOffersSource;
                    dg_selectedItems.SelectedValuePath = "offerId";
                    dg_selectedItems.DisplayMemberPath = "name";

                    dg_all.ItemsSource = allOffers;
                    dg_all.SelectedValuePath = "offerId";
                    dg_all.DisplayMemberPath = "name";
                }
                #endregion

                #region invoice
                if (membershipType == "i")
                {
                    #region title
                    _title = "trInvoicesClasses";
                    col_customer.Width = new GridLength(0);
                    col_coupon.Width = new GridLength(0);
                    col_offer.Width = new GridLength(0);
                    col_invoice.Width = new GridLength(1, GridUnitType.Star);
                    #endregion

                    allInvoicesSource = await invoice.GetAll();////active invoices
                    allInvoicesSource = allInvoicesSource.Where(i => i.isActive == 1).ToList();
                    allInvoices.AddRange(allInvoicesSource);

                    selectedInvoicesSource = await invoice.GetInvclassByMembershipId(membershipID);
                    foreach (var v in selectedInvoicesSource)
                    {
                        invoiceMembership = await invoiceMembership.GetById(v.invClassMemberId);
                        selectedInvoices.Add(invoiceMembership);
                    }
                    //remove selected items from all items
                    foreach (var i in selectedInvoicesSource)
                    {
                        invoice = allInvoices.Where(s => s.invClassId == i.invClassId).FirstOrDefault<InvoicesClass>();
                        allInvoices.Remove(invoice);
                    }

                    dg_selectedItems.ItemsSource = selectedInvoicesSource;
                    dg_selectedItems.SelectedValuePath = "invClassId";
                    dg_selectedItems.DisplayMemberPath = "name";

                    dg_all.ItemsSource = allInvoices;
                    dg_all.SelectedValuePath = "invClassId";
                    dg_all.DisplayMemberPath = "name";
                }
                #endregion

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

            txt_title.Text = AppSettings.resourcemanager.GetString(_title);

            switch (membershipType)
            {
                case "a":
                    txt_selectedItems.Text = AppSettings.resourcemanager.GetString("trSelectedCustomers");
                    break;
                case "c":
                    txt_selectedItems.Text = AppSettings.resourcemanager.GetString("trSelectedCoupons");
                    break;
                case "o":
                    txt_selectedItems.Text = AppSettings.resourcemanager.GetString("trSelectedOffers");
                    break;
                case "i":
                    txt_selectedItems.Text = AppSettings.resourcemanager.GetString("trSelectedInvoicesClasses");
                    break;
            }

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
        {//close
            //isActive = false;
            this.Close();
        }
        string searchText = "";

        public string txtStoreSearch;
        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {//search
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = txb_search.Text;
                switch (membershipType)
                {
                    case "a":
                        allCustomersQuery = allCustomers.Where(s => s.name.Contains(searchText)).ToList();
                        dg_all.ItemsSource = allCustomersQuery;
                        break;
                    case "c":
                        allCouponsQuery = allCoupons.Where(s => s.name.Contains(searchText)).ToList();
                        dg_all.ItemsSource = allCouponsQuery;
                        break;
                    case "o":
                        allOffersQuery = allOffers.Where(s => s.name.Contains(searchText)).ToList();
                        dg_all.ItemsSource = allOffersQuery;
                        break;
                        case "i":
                        allInvoicesQuery = allInvoices.Where(s => s.name.Contains(searchText)).ToList();
                        dg_all.ItemsSource = allInvoicesQuery;
                        break;
                }


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
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

                switch (membershipType)
                {
                    case "a": x = allCustomers.Count; break;
                    case "c": x = allCoupons.Count;   break;
                    case "o": x = allOffers.Count;    break;
                    case "i": x = allInvoices.Count;  break;
                }
                
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
        {//select one
            try
            {
                switch(membershipType)
                {
                    //customer
                    case "a":
                        customer = dg_all.SelectedItem as Agent;
                        if (customer != null)
                        {
                            allCustomers.Remove(customer);
                            selectedCustomersSource.Add(customer);
                            dg_selectedItems.ItemsSource = selectedCustomersSource;

                            AgentMemberships am = new AgentMemberships();
                            am.membershipId = membershipID;
                            am.agentId = customer.agentId;
                            am.notes = "";
                            am.createUserId = MainWindow.userLogin.userId;
                            am.updateUserId = MainWindow.userLogin.userId;
                            am.isActive = 1;

                            selectedCustomers.Add(am);
                        }
                        break;
                    //coupon
                    case "c":
                        coupon = dg_all.SelectedItem as Coupon;
                        if (coupon != null)
                        {
                            allCoupons.Remove(coupon);
                            selectedCouponsSource.Add(coupon);
                            dg_selectedItems.ItemsSource = selectedCouponsSource;

                            CouponsMemberships cm = new CouponsMemberships();
                            cm.couponMembershipId = 0;
                            cm.cId = coupon.cId;
                            cm.membershipId = membershipID;
                            cm.notes = "";
                            cm.createUserId = MainWindow.userLogin.userId;
                            cm.updateUserId = MainWindow.userLogin.userId;

                            selectedCoupons.Add(cm);
                        }
                        break;
                    //offers
                    case "o":
                        offer = dg_all.SelectedItem as Offer;
                        if (offer != null)
                        {
                            allOffers.Remove(offer);
                            selectedOffersSource.Add(offer);
                            dg_selectedItems.ItemsSource = selectedOffersSource;

                            MembershipsOffers om = new MembershipsOffers();
                            om.membershipOfferId = 0;
                            om.membershipId = membershipID;
                            om.offerId = offer.offerId;
                            om.notes = "";
                            om.createUserId = MainWindow.userLogin.userId;
                            om.updateUserId = MainWindow.userLogin.userId;

                            selectedOffers.Add(om);
                        }
                        break;
                    //invoice
                    case "i":
                        invoice = dg_all.SelectedItem as InvoicesClass;

                        if (invoice != null)
                        {
                            allInvoices.Remove(invoice);
                            selectedInvoicesSource.Add(invoice);
                            dg_selectedItems.ItemsSource = selectedInvoicesSource;

                            InvoicesClassMemberships im = new InvoicesClassMemberships();
                            im.invClassMemberId = 0;
                            im.membershipId = membershipID;
                            im.invClassId = invoice.invClassId;
                            im.notes = "";
                            im.createUserId = MainWindow.userLogin.userId;
                            im.updateUserId = MainWindow.userLogin.userId;

                            selectedInvoices.Add(im);
                        }
                        break;
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
                switch (membershipType)
                {
                    //customer
                    case "a":
                        customer = dg_selectedItems.SelectedItem as Agent;

                        if (customer != null)
                        {
                            selectedCustomersSource.Remove(customer);
                            dg_selectedItems.ItemsSource = selectedCustomersSource;

                            allCustomers.Add(customer);
                            dg_all.ItemsSource = allCustomers;

                            customerMembership = selectedCustomers.Where(a => a.agentId == customer.agentId).FirstOrDefault();
                            selectedCustomers.Remove(customerMembership);
                        }
                        break;
                    //coupon
                    case "c":
                        coupon = dg_selectedItems.SelectedItem as Coupon;

                        if (coupon != null)
                        {
                            selectedCouponsSource.Remove(coupon);
                            dg_selectedItems.ItemsSource = selectedCouponsSource;

                            allCoupons.Add(coupon);
                            dg_all.ItemsSource = allCoupons;

                            couponMembership = selectedCoupons.Where(a => a.cId == coupon.cId).FirstOrDefault();
                            selectedCoupons.Remove(couponMembership);
                        }
                        break;
                    //offers
                    case "o":
                        offer = dg_selectedItems.SelectedItem as Offer;

                        if (offer != null)
                        {
                            selectedOffersSource.Remove(offer);
                            dg_selectedItems.ItemsSource = selectedOffersSource;

                            allOffers.Add(offer);
                            dg_all.ItemsSource = allOffers;

                            offerMembership = selectedOffers.Where(a => a.offerId == offer.offerId).FirstOrDefault();
                            selectedOffers.Remove(offerMembership);
                        }
                        break;
                    //invoice
                    case "i":
                        invoice = dg_selectedItems.SelectedItem as InvoicesClass;

                        if (invoice != null)
                        {
                            selectedInvoicesSource.Remove(invoice);
                            dg_selectedItems.ItemsSource = selectedInvoicesSource;

                            allInvoices.Add(invoice);
                            dg_all.ItemsSource = allInvoices;

                            invoiceMembership = selectedInvoices.Where(a => a.invClassId == a.invClassId).FirstOrDefault();
                            selectedInvoices.Remove(invoiceMembership);
                        }
                        break;
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

                switch (membershipType)
                {
                    case "a": x = selectedCustomersSource.Count; break;
                    case "c": x = selectedCouponsSource.Count;   break;
                    case "o": x = selectedOffersSource.Count;    break;
                    case "i": x = selectedInvoicesSource.Count;  break;
                }

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

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                HelpClass.StartAwait(grid_main);
                int s = 0;
                switch (membershipType)
                {
                    //customer
                    case "a":
                        s = await customerMembership.UpdateAgentsByMembershipId(selectedCustomers, membershipID, MainWindow.userLogin.userId);
                        break;
                    //coupon
                    case "c":
                        s = await couponMembership.UpdateCouponsByMembershipId(selectedCoupons, membershipID, MainWindow.userLogin.userId);
                        break;
                    //offers
                    case "o":
                        s = await offerMembership.UpdateOffersByMembershipId(selectedOffers, membershipID, MainWindow.userLogin.userId);
                        break;
                    //invoice
                    case "i":
                        s = await invoiceMembership.UpdateInvclassByMembershipId(selectedInvoices, membershipID, MainWindow.userLogin.userId);
                        break;
                }

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

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
    }
}
