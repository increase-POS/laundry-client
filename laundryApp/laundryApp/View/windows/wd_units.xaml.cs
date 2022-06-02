using netoaster;
using laundryApp;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zen.Barcode;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_units.xaml
    /// </summary>
    public partial class wd_units : Window
    {
        #region for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        static private object _Sender;
        #endregion
        public wd_units()
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
        public bool isOpend = false;
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
            this.Close();
        }
        private async void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 150)
                {
                    _BarcodeStr = "";
                }

                string digit = "";
                // record keystroke & timestamp 
                if (e.Key >= Key.D0 && e.Key <= Key.D9)
                {
                    //digit pressed!
                    digit = e.Key.ToString().Substring(1);
                    // = "1" when D1 is pressed
                }
                else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                {
                    digit = e.Key.ToString().Substring(6); // = "1" when NumPad1 is pressed

                }
                _BarcodeStr += digit;
                _lastKeystroke = DateTime.Now;

                // process barcode 

                //if (e.Key.ToString() == "Return" && _BarcodeStr.Length > 0)

               // if (_BarcodeStr.Length == 13)
                if (e.Key.ToString() == "Return" && _BarcodeStr != "")
                {                 
                    // get item matches barcode
                    if (FillCombo.itemUnitList != null)
                    {
                       itemUnit = FillCombo.itemUnitList.ToList().Find(c => c.barcode == tb_barcode.Text && c.itemId == item.itemId);
                        if (itemUnit == null)
                        {
                            itemUnit = new ItemUnit();
                            if (_Sender != null)
                            {
                                TextBox tb = _Sender as TextBox;
                                if (tb != null)
                                {
                                    if (tb.Name == "tb_unitValue" || tb.Name == "tb_price")// remove barcode from text box
                                    {
                                        string tbString = tb.Text;
                                        string newStr = "";
                                        int startIndex = tbString.IndexOf(_BarcodeStr);
                                        if (startIndex != -1)
                                            newStr = tbString.Remove(startIndex, _BarcodeStr.Length);

                                        if (newStr != "")
                                        {
                                            if (tb.Name == "tb_unitValue")
                                                itemUnit.unitValue = int.Parse(newStr);
                                            else
                                                itemUnit.price = int.Parse(newStr);
                                        }
                                    }
                                }
                            }
                           itemUnit.barcode = _BarcodeStr;
                        }
                        await fillItemUnit();
                    }
                    drawBarcode(tb_barcode.Text);
                    _BarcodeStr = "";
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        public Item item;
        ItemUnit itemUnit = new ItemUnit();
        IEnumerable<ItemUnit> itemUnitsQuery;
        IEnumerable<ItemUnit> itemUnits;
        public List<Unit> units;
        byte tgl_itemUnitState;
        string searchText = "";
        public static List<string> requiredControlList;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "unitId", "unitValue", "subUnitId", "barcode" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                Keyboard.Focus(tb_barcode);
                FillCombo.FillUnits(cb_unitId);
                await RefreshItemUnitsList();
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
        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trUnits");
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            btn_add.Content = AppSettings.resourcemanager.GetString("trAdd");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");
            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            ///////////////////////////Barcode
            dg_itemUnit.Columns[0].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_itemUnit.Columns[1].Header = AppSettings.resourcemanager.GetString("trCountUnit");
           // dg_itemUnit.Columns[2].Header = AppSettings.resourcemanager.GetString("trSmallUnit");
            dg_itemUnit.Columns[2].Header = AppSettings.resourcemanager.GetString("trPrice");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_unitId, AppSettings.resourcemanager.GetString("trSelectUnitHint"));
            txt_isDefaultPurchases.Text = AppSettings.resourcemanager.GetString("trIsDefaultPurchases");
            //tb_isDefaultSales.Text = AppSettings.resourcemanager.GetString("trIsDefaultSales");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_unitValue, AppSettings.resourcemanager.GetString("trCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_subUnitId, AppSettings.resourcemanager.GetString("trUnitHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_price, AppSettings.resourcemanager.GetString("trPriceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));
 
             
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                HelpClass.StartAwait(grid_main);


                itemUnit = new ItemUnit();
                if (HelpClass.validate(requiredControlList, this))
                {
                    if (tb_barcode.Text.Length == 12 || tb_barcode.Text.Length == 13)
                    {
                        bool valid = validateValues();                       
                        if (valid == true)
                        {
                            // check barcode value if assigned to any item
                            if (!checkBarcodeValidity(tb_barcode.Text))
                            {
                                #region Tooltip_code
                                p_error_barcode.Visibility = Visibility.Visible;
                                ToolTip toolTip_barcode = new ToolTip();
                                toolTip_barcode.Content = AppSettings.resourcemanager.GetString("trErrorDuplicateBarcodeToolTip");
                                toolTip_barcode.Style = Application.Current.Resources["ToolTipError"] as Style;
                                p_error_barcode.ToolTip = toolTip_barcode;
                                #endregion
                            }                          
                            else //barcode is available
                                {

                                //unit
                                Nullable<int> unitId = null;
                                if (cb_unitId.SelectedIndex != -1)
                                    unitId = (int) cb_unitId.SelectedValue;

                                //count
                                int unitValue = int.Parse(tb_unitValue.Text);
                                //smallUnitId
                                Nullable<int> smallUnitId = (int)cb_subUnitId.SelectedValue;
                                //defaultBurchase
                                short defaultBurchase = 0;
                                if (tbtn_isDefaultPurchases.IsChecked == true)
                                    defaultBurchase = 1;
                                //price
                                //decimal price = 0;
                                //try
                                //{
                                //    price = decimal.Parse(tb_price.Text);
                                //}
                                //catch { }
                                //barcode
                                string barcode = tb_barcode.Text;
                                /////////////////////////////////////
                                itemUnit.itemUnitId = 0;
                                itemUnit.itemId = item.itemId;
                                itemUnit.unitId = unitId;
                                itemUnit.unitValue = unitValue;
                                itemUnit.subUnitId = smallUnitId;
                                itemUnit.defaultPurchase = defaultBurchase;
                                //itemUnit.defaultSale = defaultSale;
                                //itemUnit.price = price;
                                itemUnit.barcode = barcode;
                                itemUnit.createUserId = MainWindow.userLogin.userId;
                                itemUnit.updateUserId = MainWindow.userLogin.userId;
                                int res = await itemUnit.saveItemUnit(itemUnit);
                                if (res <= 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                    await FillCombo.RefreshItemUnit();
                                    Clear();
                                    await RefreshItemUnitsList();
                                    await Search();
                                }
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopErrorBarcodeLength"), animation: ToasterAnimation.FadeIn);
                }
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
                    HelpClass.StartAwait(grid_main);
                if (HelpClass.validate(requiredControlList, this))
                {
                    if (tb_barcode.Text.Length == 12 || tb_barcode.Text.Length == 13)
                    {
                        bool valid = validateValues();
                        if (valid == true)
                        {

                            // check barcode value if assigned to any item
                            if (!checkBarcodeValidity(tb_barcode.Text) && itemUnit.barcode != tb_barcode.Text)
                            {
                                #region Tooltip_code
                                p_error_barcode.Visibility = Visibility.Visible;
                                ToolTip toolTip_barcode = new ToolTip();
                                toolTip_barcode.Content = AppSettings.resourcemanager.GetString("trErrorDuplicateBarcodeToolTip");
                                toolTip_barcode.Style = Application.Current.Resources["ToolTipError"] as Style;
                                p_error_barcode.ToolTip = toolTip_barcode;
                                #endregion
                            }                        
                            else //barcode is available
                            {

                                //unit
                                Nullable<int> unitId = null;
                                if (cb_unitId.SelectedIndex != -1)
                                    unitId = (int) cb_unitId.SelectedValue;

                                //count
                                int unitValue = int.Parse(tb_unitValue.Text);
                                //smallUnitId
                                Nullable<int> smallUnitId = (int)cb_subUnitId.SelectedValue;
                                //defaultBurchase
                                short defaultBurchase = 0;
                                if (tbtn_isDefaultPurchases.IsChecked == true)
                                    defaultBurchase = 1;
                                //defaultSale
                                //short defaultSale = 0;
                                //if (tbtn_isDefaultSales.IsChecked == true)
                                //    defaultSale = 1;
                                //price
                                //decimal price = 0;
                                //try
                                //{
                                //   price = decimal.Parse(tb_price.Text);
                                //}
                                //catch { }
                                //barcode
                                string barcode = tb_barcode.Text;
                                /////////////////////////////////////
                                itemUnit.itemId = item.itemId;
                                itemUnit.unitId = unitId;
                                itemUnit.unitValue = unitValue;
                                itemUnit.subUnitId = smallUnitId;
                                itemUnit.defaultPurchase = defaultBurchase;
                                //itemUnit.defaultSale = defaultSale;
                                //itemUnit.price = price;
                                itemUnit.barcode = barcode;
                                itemUnit.updateUserId = MainWindow.userLogin.userId;
                                int res = await itemUnit.saveItemUnit(itemUnit);
                                if (res <= 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                    await FillCombo.RefreshItemUnit();
                                    Clear();
                                    await RefreshItemUnitsList();
                                    await Search();
                                }
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopErrorBarcodeLength"), animation: ToasterAnimation.FadeIn);
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {//delete
                    HelpClass.StartAwait(grid_main);
                    if (itemUnit.itemUnitId != 0)
                    {
                        if ((!itemUnit.canDelete) && (itemUnit.isActive == 0))
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
                            if (itemUnit.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!itemUnit.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (itemUnit.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!itemUnit.canDelete) && (itemUnit.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await itemUnit.Delete(itemUnit.itemUnitId, MainWindow.userLogin.userId, itemUnit.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshItemUnitsList();
                                    await Search();
                                    Clear();
                                }
                            }
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
        private async Task activate()
        {//activate
            itemUnit.isActive = 1;
            int s = await itemUnit.saveItemUnit(itemUnit);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshItemUnitsList();
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
                if (itemUnits is null)
                    await RefreshItemUnitsList();
                tgl_itemUnitState = 1;
                await Search();
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
                if (itemUnits is null)
                    await RefreshItemUnitsList();
                tgl_itemUnitState = 0;
                await Search();
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
        private void Tb_unitValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            _Sender = sender;
        }
        private void Tb_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            _Sender = sender;
        }
        private async void Dg_itemUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_itemUnit.SelectedIndex != -1)
                {
                    itemUnit = dg_itemUnit.SelectedItem as ItemUnit;
                    await fillItemUnit();
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
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshItemUnitsList();
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        int _int = 0;
        private async void Cb_unitId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_unitId.SelectedValue != null)
                {
                    if (int.TryParse(cb_unitId.SelectedValue.ToString(), out _int))
                    {
                        await FillCombo.FillSmallUnits(cb_subUnitId, (int)cb_unitId.SelectedValue, item.itemId);
                        cb_subUnitId.SelectedValue = itemUnit.subUnitId;
                        if (itemUnit.itemUnitId == 0)
                            generateBarcode();
                    }
                }
                else
                {
                    await FillCombo.FillSmallUnits(cb_subUnitId, -1, item.itemId);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_subUnitId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_unitId.SelectedIndex != -1 && cb_subUnitId.SelectedIndex != -1)
                {
                    if ((int)cb_unitId.SelectedValue == (int)cb_subUnitId.SelectedValue)
                        tb_unitValue.Text = "1";
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Refresh & Search
        async Task Search()
        {
            //search
            if (itemUnits is null)
                await RefreshItemUnitsList();
            RefreshItemUnitsView();
        }
        async Task<IEnumerable<ItemUnit>> RefreshItemUnitsList()
        {
            await FillCombo.RefreshItemUnit();
            itemUnits = FillCombo.itemUnitList;
            itemUnits = itemUnits.Where(x => x.itemId == item.itemId);
            return itemUnits;
        }
        void RefreshItemUnitsView()
        {
            itemUnitsQuery = itemUnits;
            dg_itemUnit.ItemsSource = itemUnitsQuery;
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            itemUnit = new ItemUnit();
            itemUnit.unitValue = 0;
            itemUnit.price = 0;
            this.DataContext = itemUnit;
            cb_subUnitId.SelectedIndex = -1;
            tb_barcode.Clear();
            tb_unitValue.Text = "0";
            img_barcode.Source = null;
            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {


                //only  digits
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
        static private int _InternalCounter = 0;
        private Boolean checkBarcodeValidity(string barcode)
        {
            if (FillCombo.itemUnitList != null)
            {
                var exist = FillCombo.itemUnitList.Where(x => x.barcode == barcode && x.itemUnitId != itemUnit.itemUnitId).FirstOrDefault();
                if (exist != null)
                    return false;
                else
                    return true;
            }
            return true;
        }
        private void Tb_barcode_KeyDown(object sender, KeyEventArgs e)
        {
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
        }
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
        private async void generateBarcode()
        {
            string barcodeString = "";
            barcodeString = generateRandomBarcode();
            if (FillCombo.itemUnitList!= null)
            {
                if (! checkBarcodeValidity(barcodeString))
                    barcodeString = generateRandomBarcode();
            }
           tb_barcode.Text = barcodeString;
            HelpClass.validateEmpty( "trErrorEmptyBarcodeToolTip",p_error_barcode );
            drawBarcode(barcodeString);
        }

        #endregion
        #region functions
        private async Task fillItemUnit()
        {
            this.DataContext = itemUnit;
            await FillCombo.FillSmallUnits(cb_subUnitId, (int)itemUnit.unitId, item.itemId);
            cb_subUnitId.SelectedValue = (int)itemUnit.subUnitId;
            tb_unitValue.Text = itemUnit.unitValue.ToString();
            tb_barcode.Text = itemUnit.barcode;
            if (itemUnit != null)
            {
                #region delete
                if (itemUnit.canDelete)
                    btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                else
                {
                    if (itemUnit.isActive == 0)
                        btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                    else
                        btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                }
                #endregion
                drawBarcode(itemUnit.barcode);
            }
           
        }

        private bool validateValues()
        {
            bool valid = true;
            char[] barcodeData;
            char checkDigit;
            if (tb_barcode.Text.Length == 12)// generate checksum didit
            {
                barcodeData = tb_barcode.Text.ToCharArray();
                checkDigit = Mod10CheckDigit(barcodeData);
                itemUnit.barcode = checkDigit + tb_barcode.Text;
                this.DataContext = itemUnit;
               // tb_barcode.Text = checkDigit + tb_barcode.Text;
            }
            else if (tb_barcode.Text.Length == 13)
            {
                valid = isBarcodeCorrect(tb_barcode.Text);
                //char cd = tb_barcode.Text[0];
                //string barCode = tb_barcode.Text.Substring(1);
                //barcodeData = barCode.ToCharArray();
                //checkDigit = Mod10CheckDigit(barcodeData);
                //if (checkDigit != cd)
                //{
                //    valid = false;
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorBarcodeToolTip"), animation: ToasterAnimation.FadeIn);
                //}
            }
            if(tb_unitValue.Text.Equals("0"))
            {
                valid = false;
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMustBeMoreThanZero"), animation: ToasterAnimation.FadeIn);
            }
            return valid;
        }
        #endregion


    }
}
