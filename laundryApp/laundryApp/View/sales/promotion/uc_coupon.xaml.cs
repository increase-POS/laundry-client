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
    /// Interaction logic for uc_coupon.xaml
    /// </summary>
    public partial class uc_coupon : UserControl
    {
        public uc_coupon()
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
        private static uc_coupon _instance;
        public static uc_coupon Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_coupon();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string basicsPermission = "coupon_basics";
        Coupon coupon = new Coupon();
        IEnumerable<Coupon> couponsQuery;
        IEnumerable<Coupon> coupons;
        byte tgl_couponState;
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

                requiredControlList = new List<string> { "name", "code", "discountType", "discountValue" };

                FillCombo.fillDiscountType(cb_discountType);
                FillCombo.fillAvailabilityType(cb_forAgents);

                //fill membership combo

                img_barcode.Source = null;

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

                await RefreshCouponsList();
                await Search();
                
                Keyboard.Focus(tb_code);

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

            //txt_title.Text = AppSettings.resourcemanager.GetString("trCoupon");
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_code, AppSettings.resourcemanager.GetString("trCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcode")+"...");
            txt_isActiveCoupon.Text = AppSettings.resourcemanager.GetString("trActive");
            txt_contentInformatin.Text = AppSettings.resourcemanager.GetString("trDetails");

            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_membershipId, AppSettings.resourcemanager.GetString("trMembership")+"...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_discountType, AppSettings.resourcemanager.GetString("trTypeDiscountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_discountValue, AppSettings.resourcemanager.GetString("trDiscountValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_startDate, AppSettings.resourcemanager.GetString("trStartDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_endDate, AppSettings.resourcemanager.GetString("trEndDateHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_invMin, AppSettings.resourcemanager.GetString("trMinimumInvoiceValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_invMax, AppSettings.resourcemanager.GetString("trMaximumInvoiceValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_quantity, AppSettings.resourcemanager.GetString("trQuantityHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");

            dg_coupon.Columns[0].Header = AppSettings.resourcemanager.GetString("trCode");
            dg_coupon.Columns[1].Header = AppSettings.resourcemanager.GetString("trName");
            dg_coupon.Columns[2].Header = AppSettings.resourcemanager.GetString("trValue");
            dg_coupon.Columns[3].Header = AppSettings.resourcemanager.GetString("trState");
            dg_coupon.Columns[4].Header = AppSettings.resourcemanager.GetString("trQuantity");
            dg_coupon.Columns[5].Header = AppSettings.resourcemanager.GetString("trRemainQuantity");
            dg_coupon.Columns[6].Header = AppSettings.resourcemanager.GetString("trvalidity");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            btn_printBarcode.Content = AppSettings.resourcemanager.GetString("trPrintBarcode");

        }

        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") )
                {
                    HelpClass.StartAwait(grid_main);
                    coupon = new Coupon();
                    if (HelpClass.validate(requiredControlList, this) )
                    {
                        #region validate
                        bool isCodeExist = false;
                        Coupon existCoupon = await coupon.Existcode(tb_code.Text);
                        if (existCoupon != null) isCodeExist = true;

                        bool isEndDateSmaller = false;
                        if ((dp_startDate.SelectedDate != null) && (dp_endDate.SelectedDate != null))
                            if (dp_endDate.SelectedDate < dp_startDate.SelectedDate) isEndDateSmaller = true;

                        bool isMaxInvoiceValueSmaller = false;
                        try
                        {
                            if (!decimal.Parse(tb_invMax.Text).Equals(0))
                                if (decimal.Parse(tb_invMax.Text) < decimal.Parse(tb_invMin.Text)) isMaxInvoiceValueSmaller = true;
                        }
                        catch { }
                        if ((isCodeExist) || (isEndDateSmaller) || (isMaxInvoiceValueSmaller))
                        {
                            if (isCodeExist)
                                HelpClass.SetValidate(p_error_code , "trDuplicateCodeToolTip");
                            if (isEndDateSmaller)
                            {
                                HelpClass.SetValidate(p_error_startDate, "trErrorEndDateSmallerToolTip");
                                HelpClass.SetValidate(p_error_endDate, "trErrorEndDateSmallerToolTip");
                            }
                            if (isMaxInvoiceValueSmaller)
                            {
                                HelpClass.SetValidate(p_error_invMin, "trErrorMaxInvoiceSmallerToolTip");
                                HelpClass.SetValidate(p_error_invMax, "trErrorMaxInvoiceSmallerToolTip");
                            }
                           
                        }
                        #endregion
                        else
                        {
                            #region add
                            coupon = new Coupon();

                            coupon.code = tb_code.Text;
                            coupon.name = tb_name.Text;
                            coupon.forAgents = cb_forAgents.SelectedValue.ToString();
                            coupon.notes = tb_notes.Text;
                            coupon.barcode = tb_barcode.Text;
                            coupon.isActive = Convert.ToByte(tgl_isActiveCoupon.IsChecked);
                            coupon.discountType = Convert.ToByte(cb_discountType.SelectedValue);
                            coupon.discountValue = decimal.Parse(tb_discountValue.Text);

                            if (dp_startDate.SelectedDate != null)
                                coupon.startDate = DateTime.Parse(dp_startDate.Text);
                            if (dp_endDate.SelectedDate != null)
                                coupon.endDate = DateTime.Parse(dp_endDate.Text);
                            if (!tb_invMin.Text.Equals(""))
                                coupon.invMin = decimal.Parse(tb_invMin.Text);
                            else
                                coupon.invMin = 0;
                            if (!tb_invMax.Text.Equals(""))
                                coupon.invMax = decimal.Parse(tb_invMax.Text);
                            else
                                coupon.invMax = 0;
                            if (string.IsNullOrWhiteSpace(tb_quantity.Text))
                                coupon.quantity = 0;
                            else
                                coupon.quantity = Int32.Parse(tb_quantity.Text);
                            coupon.remainQ = coupon.quantity;
                            //coupon.membershipId = 0;///????????????
                            coupon.createUserId = MainWindow.userLogin.userId;

                            int s = await coupon.save(coupon);

                            if (s > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                Clear();

                                await RefreshCouponsList();
                                await Search();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        #endregion
                        }

                    }
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

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") )
                {
                    HelpClass.StartAwait(grid_main);
                    if (coupon.cId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this))
                    {
                        #region validate
                        bool isCodeExist = false;
                        Coupon existCoupon = await coupon.Existcode(tb_code.Text);
                        if ((existCoupon != null) && (existCoupon.cId != coupon.cId))
                            isCodeExist = true;

                        bool isEndDateSmaller = false;
                        if ((dp_startDate.SelectedDate != null) && (dp_endDate.SelectedDate != null))
                            if (dp_endDate.SelectedDate < dp_startDate.SelectedDate) isEndDateSmaller = true;

                        bool isMaxInvoiceValueSmaller = false;
                        try
                        {
                            if (!decimal.Parse(tb_invMax.Text).Equals(0))
                                if (decimal.Parse(tb_invMax.Text) < decimal.Parse(tb_invMin.Text)) isMaxInvoiceValueSmaller = true;
                        }
                        catch { }
                        if ((isCodeExist) || (isEndDateSmaller) || (isMaxInvoiceValueSmaller))
                        {
                            if (isCodeExist)
                                HelpClass.SetValidate(p_error_code, "trDuplicateCodeToolTip");
                            if (isEndDateSmaller)
                            {
                                HelpClass.SetValidate(p_error_startDate, "trErrorEndDateSmallerToolTip");
                                HelpClass.SetValidate(p_error_endDate, "trErrorEndDateSmallerToolTip");
                            }
                            if (isMaxInvoiceValueSmaller)
                            {
                                HelpClass.SetValidate(p_error_invMin, "trErrorMaxInvoiceSmallerToolTip");
                                HelpClass.SetValidate(p_error_invMax, "trErrorMaxInvoiceSmallerToolTip");
                            }
                            #endregion
                    }
                    else
                    {
                        #region update
                        coupon.code = tb_code.Text;
                        coupon.name = tb_name.Text;
                        coupon.forAgents = cb_forAgents.SelectedValue.ToString();
                        coupon.notes = tb_notes.Text;
                        coupon.barcode = tb_barcode.Text;
                        coupon.isActive = Convert.ToByte(tgl_isActiveCoupon.IsChecked);
                        coupon.discountType = Convert.ToByte(cb_discountType.SelectedValue);
                        coupon.discountValue = decimal.Parse(tb_discountValue.Text);

                        if (dp_startDate.SelectedDate != null)
                            coupon.startDate = DateTime.Parse(dp_startDate.Text);
                        if (dp_endDate.SelectedDate != null)
                            coupon.endDate = DateTime.Parse(dp_endDate.Text);
                        if (!tb_invMin.Text.Equals(""))
                            coupon.invMin = decimal.Parse(tb_invMin.Text);
                        else
                            coupon.invMin = 0;
                        if (!tb_invMax.Text.Equals(""))
                            coupon.invMax = decimal.Parse(tb_invMax.Text);
                        else
                            coupon.invMax = 0;
                        if (string.IsNullOrWhiteSpace(tb_quantity.Text))
                            coupon.quantity = 0;
                        else
                            coupon.quantity = Int32.Parse(tb_quantity.Text);
                        coupon.remainQ = coupon.quantity;
                        //coupon.membershipId = 0;///????????????
                        coupon.updateUserId = MainWindow.userLogin.userId;

                        int s = await coupon.save(coupon);

                        if (s > 0)
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);

                            await Search();
                        }
                        else
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        
                        }
                        #endregion
                    }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);


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

        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") )
                {
                    HelpClass.StartAwait(grid_main);
                    if (coupon.cId != 0)
                    {
                        if ((!coupon.canDelete) && (coupon.isActive == 0))
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
                            if (coupon.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!coupon.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (coupon.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!coupon.canDelete) && (coupon.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await coupon.delete(coupon.cId, MainWindow.userLogin.userId, coupon.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    coupon.cId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshCouponsList();
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
        
        private async Task activate()
        {//activate
            coupon.isActive = 1;
            int s = await coupon.save(coupon);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshCouponsList();
                await Search();
            }
        }
        
        #endregion

        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                await Search();
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
                if (coupons is null)
                    await RefreshCouponsList();

                tgl_couponState = 1;

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

                if (coupons is null)
                    await RefreshCouponsList();

                tgl_couponState = 0;

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
        private async void Dg_coupon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_coupon.SelectedIndex != -1)
                {
                    coupon = dg_coupon.SelectedItem as Coupon;
                    this.DataContext = coupon;
                    if (coupon != null)
                    {
                        tb_barcode.Text = coupon.barcode;

                        tb_discountValue.IsEnabled = false;
                        cb_discountType.IsEnabled = false;

                        HelpClass.drawBarcode(coupon.barcode , img_barcode);

                        #region delete
                        if (coupon.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (coupon.isActive == 0)
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
                tb_search.Text = "";
                await RefreshCouponsList();
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
            if (coupons is null)
                await RefreshCouponsList();

            searchText = tb_search.Text.ToLower();
            couponsQuery = coupons;
            couponsQuery = coupons.Where(s => 
            (
            s.code.ToLower().Contains(searchText) 
            ||
            s.name.ToLower().Contains(searchText) 
            ||
            s.discountType.ToString().ToLower().Contains(searchText)
            ||
            s.discountValue.ToString().ToLower().Contains(searchText)
            ) 
            && s.isActive == tgl_couponState
            );
            RefreshCustomersView();
        }
        async Task<IEnumerable<Coupon>> RefreshCouponsList()
        {
            coupons = await coupon.Get();
            return coupons;
        }
        void RefreshCustomersView()
        {
            dg_coupon.ItemsSource = couponsQuery;
            txt_count.Text = couponsQuery.Count().ToString();
        }
        
        #endregion

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            coupon = new Coupon();
            coupon.forAgents = "pb";
            this.DataContext = coupon;
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            tb_discountValue.IsEnabled = true;
            cb_discountType.IsEnabled = true;

            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only  digits
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
        {
            try
            {
                //only english and digits
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
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
                string name = sender.GetType().Name;
                var txb = sender as TextBox;
                if ((sender as TextBox).Name == "tb_code")
                {
                    tb_barcode.Text = genBarCode(tb_code.Text);
                    HelpClass.drawBarcode(tb_barcode.Text, img_barcode);
                }
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
        private string genBarCode(string code)
        {
            string s = "cop-" + code;
            return s;
        }
        #endregion

        #region reports
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void Btn_printBarcode_Click(object sender, RoutedEventArgs e)
        {//print barcode
            if (tb_barcode.Text != null && tb_barcode.Text != "")
            {
                buildbarcodereport();
                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
            }
            else
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBarcodeEmpty"), animation: ToasterAnimation.FadeIn);
            }
        }

        public void buildbarcodereport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();

            addpath = @"\Reports\Sale\En\coupExport.rdlc";

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();

            clsReports.couponExportReport(rep, reppath, paramarr, tb_barcode.Text);

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
                    Thread t1 = new Thread(() =>
                    {
                        pdfPurCoupon();
                    });
                    t1.Start();
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
                    Thread t1 = new Thread(() =>
                    {
                        printPurCoupon();
                    });
                    t1.Start();
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
                    win_lvcSales win = new win_lvcSales(couponsQuery, 1);
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

        public void BuildReport()
        {
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string firstTitle = "promotion";
            string secondTitle = "coupon";
            string subTitle = "";
            string Title = "";

            string addpath = "";
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {

                addpath = @"\Reports\Sale\Ar\ArCoupon.rdlc";
                //   secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);

            }
            else
            {
                //english

                addpath = @"\Reports\Sale\En\EnCoupon.rdlc";
                //  secondTitle = "items";
                subTitle = clsReports.ReportTabTitle(firstTitle, secondTitle);


            }

            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportCls.checkLang();
            //  getpuritemcount
            Title = AppSettings.resourcemanagerreport.GetString("trSales") + " / " + subTitle;
            paramarr.Add(new ReportParameter("trTitle", Title));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));


            clsReports.couponReport(couponsQuery, rep, reppath, paramarr);

            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        #endregion

    }
}
