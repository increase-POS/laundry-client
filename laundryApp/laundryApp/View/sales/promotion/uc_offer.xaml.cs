using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace laundryApp.View.sales.promotion
{
    /// <summary>
    /// Interaction logic for uc_offer.xaml
    /// </summary>
    public partial class uc_offer : UserControl
    {
        public uc_offer()
        {
            try
            {
                InitializeComponent();
                if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1440)
                {
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;
                }
                else if (System.Windows.SystemParameters.PrimaryScreenWidth >= 1360)
                {
                    txt_add_Icon.Visibility = Visibility.Collapsed;
                    txt_update_Icon.Visibility = Visibility.Collapsed;
                    txt_delete_Icon.Visibility = Visibility.Collapsed;
                    txt_deleteButton.Visibility = Visibility.Visible;
                    txt_addButton.Visibility = Visibility.Visible;
                    txt_updateButton.Visibility = Visibility.Visible;

                }
                else
                {
                    txt_deleteButton.Visibility = Visibility.Collapsed;
                    txt_addButton.Visibility = Visibility.Collapsed;
                    txt_updateButton.Visibility = Visibility.Collapsed;
                    txt_add_Icon.Visibility = Visibility.Visible;
                    txt_update_Icon.Visibility = Visibility.Visible;
                    txt_delete_Icon.Visibility = Visibility.Visible;

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private static uc_offer _instance;
        public static uc_offer Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_offer();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string basicsPermission = "offer_basics";
        string itemsPermission = "offer_items";

        ItemUnit itemUnitsModel = new ItemUnit();
        IEnumerable<ItemUnit> itemUnits;
        Offer offer = new Offer();
        IEnumerable<Offer> offersQuery;
        IEnumerable<Offer> offers;
        byte tgl_offerState;

        string searchText = "";

        public static List<string> requiredControlList;
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

                requiredControlList = new List<string> { "code" , "name", "discountType" , "discountValue" , "startDate" , "endDate"};

                FillCombo.fillDiscountType(cb_discountType);
                FillCombo.fillAvailabilityType(cb_forAgents);

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                #endregion

                await RefreshOffersList();
                await Search();

                //Keyboard.Focus(tb_code);

                Clear();

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
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            ///////////////////////////////////////------OFFER------///////////////////////////////
            //txt_title.Text = AppSettings.resourcemanager.GetString("trOffer");
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_code, AppSettings.resourcemanager.GetString("trCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
            txt_isActiveOffer.Text = AppSettings.resourcemanager.GetString("trActive");
            txt_contentInformatin.Text = AppSettings.resourcemanager.GetString("trDetails");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_discountType, AppSettings.resourcemanager.GetString("trTypeDiscountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_discountValue, AppSettings.resourcemanager.GetString("trDiscountValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_endDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            TextBox tbStart = (TextBox)tp_startTime.Template.FindName("PART_TextBox", tp_startTime);
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tbStart, AppSettings.resourcemanager.GetString("trStartTimeHint"));
            TextBox tbEnd = (TextBox)tp_endTime.Template.FindName("PART_TextBox", tp_endTime);
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tbEnd, AppSettings.resourcemanager.GetString("trEndTimeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");

            dg_offer.Columns[0].Header = AppSettings.resourcemanager.GetString("trCode");
            dg_offer.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_offer.Columns[2].Header = AppSettings.resourcemanager.GetString("trValue");
            dg_offer.Columns[3].Header = AppSettings.resourcemanager.GetString("trState");
            dg_offer.Columns[4].Header = AppSettings.resourcemanager.GetString("trStartDate");
            dg_offer.Columns[5].Header = AppSettings.resourcemanager.GetString("trEndDate");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            btn_items.Content = AppSettings.resourcemanager.GetString("trItems");


        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add"))
                {
                    HelpClass.StartAwait(grid_main);

                    #region validate
                    bool isCodeExist = await HelpClass.isCodeExist(tb_code.Text ,"","Offer" , 0);

                    bool isEndDateSmaller = false;
                    if (dp_endDate.SelectedDate < dp_startDate.SelectedDate) isEndDateSmaller = true;

                    if (HelpClass.validate(requiredControlList, this))
                    {
                        if ((isCodeExist) || (isEndDateSmaller))
                        {
                            if (isCodeExist)
                                HelpClass.SetValidate(p_error_code, "trDuplicateCodeToolTip");

                            if (isEndDateSmaller)
                            {
                                HelpClass.SetValidate(p_error_startDate, "trErrorEndDateSmallerToolTip");
                                HelpClass.SetValidate(p_error_endDate, "trErrorEndDateSmallerToolTip");
                            }
                        }
                        #endregion

                        else
                        {
                            #region add
                            string startDateStr = dp_startDate.SelectedDate.Value.ToShortDateString();
                            string startTimeStr = "00:00 AM";
                            if (tp_startTime.Text != null)
                                startTimeStr = tp_startTime.SelectedTime.Value.ToShortTimeString();
                            DateTime startDateTime = DateTime.Parse(startDateStr + " " + startTimeStr);

                            string endDateStr = dp_endDate.SelectedDate.Value.ToShortDateString();
                            string endTimeStr = "00:00 AM";
                            if (tp_endTime.Text != null)
                                endTimeStr = tp_endTime.SelectedTime.Value.ToShortTimeString();
                            DateTime endDateTime = DateTime.Parse(endDateStr + " " + endTimeStr);

                            offer = new Offer();

                            offer.name = tb_name.Text;
                            offer.code = tb_code.Text;
                            offer.forAgents = cb_forAgents.SelectedValue.ToString();
                            offer.isActive = Convert.ToByte(tgl_isActiveOffer.IsChecked);
                            offer.discountType = Convert.ToString(cb_discountType.SelectedValue);
                            offer.discountValue = decimal.Parse(tb_discountValue.Text);
                            if (dp_startDate.SelectedDate != null)
                                offer.startDate = startDateTime;
                            if (dp_endDate.SelectedDate != null)
                                offer.endDate = endDateTime;
                            offer.createUserId = MainWindow.userLogin.userId;
                            offer.notes = tb_notes.Text;

                            int s = await offer.save(offer);

                            if (s > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                Clear();

                                await RefreshOffersList();
                                await Search();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            #endregion
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                {
                    if (offer.offerId > 0)
                    {
                        HelpClass.StartAwait(grid_main);

                        #region validate
                        bool isCodeExist = await HelpClass.isCodeExist(tb_code.Text, "", "Offer", offer.offerId);

                        bool isEndDateSmaller = false;
                        if (dp_endDate.SelectedDate < dp_startDate.SelectedDate) isEndDateSmaller = true;

                        if (HelpClass.validate(requiredControlList, this))
                        {
                            if ((isCodeExist) || (isEndDateSmaller))
                            {
                                if (isCodeExist)
                                    HelpClass.SetValidate(p_error_code, "trDuplicateCodeToolTip");

                                if (isEndDateSmaller)
                                {
                                    HelpClass.SetValidate(p_error_startDate, "trErrorEndDateSmallerToolTip");
                                    HelpClass.SetValidate(p_error_endDate, "trErrorEndDateSmallerToolTip");
                                }
                            }
                            #endregion

                            else
                            {
                                #region update
                                string startDateStr = dp_startDate.SelectedDate.Value.ToShortDateString();
                                string startTimeStr = "00:00 AM";
                                if (tp_startTime.Text != null)
                                    startTimeStr = tp_startTime.SelectedTime.Value.ToShortTimeString();
                                DateTime startDateTime = DateTime.Parse(startDateStr + " " + startTimeStr);

                                string endDateStr = dp_endDate.SelectedDate.Value.ToShortDateString();
                                string endTimeStr = "00:00 AM";
                                if (tp_endTime.Text != null)
                                    endTimeStr = tp_endTime.SelectedTime.Value.ToShortTimeString();
                                DateTime endDateTime = DateTime.Parse(endDateStr + " " + endTimeStr);

                                offer.name = tb_name.Text;
                                offer.code = tb_code.Text;
                                offer.forAgents = cb_forAgents.SelectedValue.ToString();
                                offer.isActive = Convert.ToByte(tgl_isActiveOffer.IsChecked);
                                offer.discountType = Convert.ToString(cb_discountType.SelectedValue);
                                offer.discountValue = decimal.Parse(tb_discountValue.Text);
                                if (dp_startDate.SelectedDate != null)
                                    offer.startDate = startDateTime;
                                if (dp_endDate.SelectedDate != null)
                                    offer.endDate = endDateTime;
                                offer.updateUserId = MainWindow.userLogin.userId;
                                offer.notes = tb_notes.Text;

                                int s = await offer.save(offer);

                                if (s > 0)
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                                    await RefreshOffersList();
                                    await Search();
                                }
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                #endregion
                            }
                        }
                    HelpClass.EndAwait(grid_main);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("selectOfferFirst"), animation: ToasterAnimation.FadeIn);

                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete"))
                {
                    HelpClass.StartAwait(grid_main);
                    if (offer.offerId != 0)
                    {
                        if ((!offer.canDelete) && (offer.isActive == 0))
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxActivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                                await activate();
                        }
                        else
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            if (offer.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!offer.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (offer.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!offer.canDelete) && (offer.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await offer.delete(offer.offerId, MainWindow.userLogin.userId, offer.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    offer.offerId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshOffersList();
                                    await Search();
                                    Clear();
                                }
                            }
                        }
                    }
                    HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Btn_items_Click(object sender, RoutedEventArgs e)
        {//items
            try
            {
                if(offer.offerId > 0)
                {

                HelpClass.StartAwait(grid_main);
               
                if (FillCombo.groupObject.HasPermissionAction(itemsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                {
                    //HelpClass.clearValidate(tb_code, p_errorCode);

                    itemUnits = await itemUnitsModel.Getall();

                    Window.GetWindow(this).Opacity = 0.2;

                    wd_itemsOfferList w = new wd_itemsOfferList();

                    w.offerId = offer.offerId;
                    w.ShowDialog();
                    if (w.isActive)
                    {

                    }

                    Window.GetWindow(this).Opacity = 1;
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("selectOfferFirst"), animation: ToasterAnimation.FadeIn);

            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async Task activate()
        {//activate
            offer.isActive = 1;
            int s = await offer.save(offer);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshOffersList();
                await Search();
            }
        }

        #endregion

        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                await Search();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Tgl_isActive_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (offers is null)
                    await RefreshOffersList();

                tgl_offerState = 1;
                await Search();
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Tgl_isActive_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                if (offers is null)
                    await RefreshOffersList();

                tgl_offerState = 0;
                await Search();

                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Dg_offer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_offer.SelectedIndex != -1)
                {
                    offer = dg_offer.SelectedItem as Offer;
                    this.DataContext = offer;
                    if (offer != null)
                    {
                        #region delete
                        if (offer.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (offer.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                    }
                }
                HelpClass.clearValidate(requiredControlList, this);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {//refresh
            try
            {
                HelpClass.StartAwait(grid_main);

                searchText = "";
                tb_search.Text = "";
                await RefreshOffersList();
                await Search();

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
        
        async Task Search()
        {
            //search
            if (offers is null)
                await RefreshOffersList();

            searchText = tb_search.Text.ToLower();
           
            offersQuery = offers.Where(s => (
            s.code.ToLower().Contains(searchText) 
            ||
            s.name.ToLower().Contains(searchText)
            ) 
            && 
            s.isActive == tgl_offerState);

            RefreshOffersView();
        }
        async Task<IEnumerable<Offer>> RefreshOffersList()
        {
            offers = await offer.Get();
            return offers;
        }
        void RefreshOffersView()
        {
            dg_offer.ItemsSource = offersQuery;
            txt_count.Text = offersQuery.Count().ToString();
        }
        
        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {

            offer = new Offer();
            offer.forAgents = "pb";
            this.DataContext = offer;
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }

        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        { //only  digits
            try
            {
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
                else if (textBox.Tag.ToString() == "decimal")
                {
                    input = e.Text;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only english and digits
            try
            {
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private decimal _numValue = 0;

        public decimal NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                tb_discountValue.Text = value.ToString();
            }
        }

        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #endregion

        #region barcode
        /*List<ItemUnit> barcodesList;
        static private int _InternalCounter = 0;
        private Boolean checkBarcodeValidity(string barcode)
        {
            if (FillCombo.itemUnitList != null)
            {
                var exist = FillCombo.itemUnitList.Where(x => x.barcode == barcode && x.itemId != item.itemId).FirstOrDefault();
                if (exist != null)
                    return false;
                else
                    return true;
            }
            return true;
        }
        */
       

        private void Tb_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            try
            {
                TextBox tb = (TextBox)sender;
                string barCode = tb_barcode.Text;
                if (e.Key == Key.Return && barCode.Length == 13)
                {
                    if (isBarcodeCorrect(barCode) == false)
                    {
                        item.barcode = "";
                        this.DataContext = item;
                    }
                    else
                        drawBarcode(barCode);
                }
                else if (barCode.Length == 13 || barCode.Length == 12)
                    drawBarcode(barCode);
                else
                    drawBarcode("");
            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this);
            }
            */
        }

        /*
        private bool isBarcodeCorrect(string barCode)
        {
            char checkDigit;
            char[] barcodeData;

            char cd = barCode[0];
            barCode = barCode.Substring(1);
            barcodeData = barCode.ToCharArray();
            checkDigit = Mod10CheckDigit(barcodeData);

            if (checkDigit != cd)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorBarcodeToolTip"), animation: ToasterAnimation.FadeIn);
                return false;
            }
            else
                return true;
        }
        public static char Mod10CheckDigit(char[] data)
        {
            // Start the checksum calculation from the right most position.
            int factor = 3;
            int weight = 0;
            int length = data.Length;

            for (int i = 0; i <= length - 1; i++)
            {
                weight += (data[i] - '0') * factor;
                factor = (factor == 3) ? 1 : 3;
            }

            return (char)(((10 - (weight % 10)) % 10) + '0');

        }
        private void drawBarcode(string barcodeStr)
        {
            try
            {
                // configur check sum metrics
                BarcodeSymbology s = BarcodeSymbology.CodeEan13;

                BarcodeDraw drawObject = BarcodeDrawFactory.GetSymbology(s);

                BarcodeMetrics barcodeMetrics = drawObject.GetDefaultMetrics(60);
                barcodeMetrics.Scale = 2;

                if (barcodeStr != "")
                {
                    if (barcodeStr.Length == 13)
                        barcodeStr = barcodeStr.Substring(1);//remove check sum from barcode string
                    var barcodeImage = drawObject.Draw(barcodeStr, barcodeMetrics);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();

                        img_barcode.Source = ImageProcess.ByteToImage(imageBytes);
                    }
                }
                else
                    img_barcode.Source = null;
            }
            catch { img_barcode.Source = null; }

        }
        private async void generateBarcode()
        {
            string barcodeString = "";
            barcodeString = generateRandomBarcode();
            if (FillCombo.itemUnitList != null)
            {
                if (!checkBarcodeValidity(barcodeString))
                    barcodeString = generateRandomBarcode();
            }
            item.barcode = barcodeString;
            this.DataContext = item;
            //tb_barcode.Text = barcodeString;
            HelpClass.validateEmpty("trErrorEmptyBarcodeToolTip", p_error_barcode);
            drawBarcode(tb_barcode.Text);
        }
        static public string generateRandomBarcode()
        {
            var now = DateTime.Now;

            var days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
            var seconds = (int)(now - DateTime.Today).TotalSeconds;

            var counter = _InternalCounter++ % 100;
            string randomBarcode = days.ToString("00000") + seconds.ToString("00000") + counter.ToString("00");
            char[] barcodeData = randomBarcode.ToCharArray();
            char checkDigit = Mod10CheckDigit(barcodeData);
            return checkDigit + randomBarcode;

        }
        */
        #endregion

        #region reports
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string firstTitle = "promotion";
            string secondTitle = "offers";
            string subTitle = "";
            string Title = "";

            string addpath = "";
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {

                addpath = @"\Reports\Sale\Ar\ArOffer.rdlc";
                //   secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            }
            else
            {
                //english

                addpath = @"\Reports\Sale\En\EnOffer.rdlc";
                //  secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);


            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //  getpuritemcount
            Title = AppSettings.resourcemanagerreport.GetString("trSales") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));


            clsReports.OfferReport(offersQuery.ToList(), rep, reppath, paramarr);

            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {//pdf
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    /////////////////////////////////////
                    //Thread t1 = new Thread(() =>
                    //{
                        pdfPurCoupon();
                    //});
                    //t1.Start();
                    //////////////////////////////////////
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {//print
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    /////////////////////////////////////
                    //Thread t1 = new Thread(() =>
                    //{
                        printPurCoupon();
                    //});
                    //t1.Start();
                    //////////////////////////////////////

                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvcSales win = new win_lvcSales(offersQuery, 2);
                    win.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {//excel
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();
                    this.Dispatcher.Invoke(() =>
                    {
                        saveFileDialog.Filter = "EXCEL|*.xls;";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filepath = saveFileDialog.FileName;
                            LocalReportExtensions.ExportToExcel(rep, filepath);
                        }
                    });


                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {//preview
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    string pdfpath = "";



                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    BuildReport();

                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    wd_previewPdf w = new wd_previewPdf();
                    w.pdfPath = pdfpath;
                    if (!string.IsNullOrEmpty(w.pdfPath))
                    {
                        w.ShowDialog();
                        w.wb_pdfWebViewer.Dispose();
                    }
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                }
                else
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public async void pdfPurCoupon()
        {
            BuildReport();
            this.Dispatcher.Invoke(() =>
            {
                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
            });
        }

        public async void printPurCoupon()
        {
            BuildReport();
            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, short.Parse(AppSettings.rep_print_count));
            });
        }

   
        #endregion

    }
}
