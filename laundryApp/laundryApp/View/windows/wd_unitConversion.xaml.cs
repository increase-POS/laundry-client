using System;
using System.Collections.Generic;
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
using netoaster;
using laundryApp.Classes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_unitConversion.xaml
    /// </summary>
    public partial class wd_unitConversion : Window
    {
        public wd_unitConversion()
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
        IEnumerable<Item> items;
        Item item = new Item();
        ItemUnit itemUnit = new ItemUnit();
        ItemLocation ItemLocation = new ItemLocation();
        List<ItemLocation> locations;

        List<ItemUnit> units;
        List<ItemUnit> smallUnits;
        ItemUnit isSmall = null;
        private static string _FromUnit = "";
        private static string _ToUnit = "";
        private static int _ToQuantity = 0;
        private static int _FromQuantity = 0;
        private static int _ConversionQuantity = 0;
        public static List<string> requiredControlList = new List<string>();
        private void Cb_item_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                cb_itemId.ItemsSource = items.Where(x => x.name.Contains(cb_itemId.Text));
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        //private async Task fillParentItemCombo()
        //{
        //    //إنشاء إجراء من API يعيد فقط الحزم
        //    items = await item.GetAllItems();
        //    var listCa = items.Where(x => x.isActive == 1).ToList();
        //    cb_item.ItemsSource = listCa;
        //    cb_item.SelectedValuePath = "itemId";
        //    cb_item.DisplayMemberPath = "name";
        //}

        private async Task setToquantityMessage()
        {
            int quantity = 0;
            int remain = 0;

            if (tb_fromQuantity.Text != "")
                quantity = int.Parse(tb_fromQuantity.Text);
            if (quantity != 0 && cb_fromUnit.SelectedIndex != -1 && cb_toUnit.SelectedIndex != -1)
            {
                isSmall = smallUnits.Find(x => x.itemUnitId == (int)cb_toUnit.SelectedValue);
                if (isSmall != null) // from-unit is bigger than to-unit
                {
                    _ConversionQuantity = await itemUnit.largeToSmallUnitQuan((int)cb_fromUnit.SelectedValue, (int)cb_toUnit.SelectedValue);
                    _ToQuantity = quantity * _ConversionQuantity;
                    _FromUnit = "";
                    _FromQuantity = quantity;

                }
                else
                {
                    _ConversionQuantity = await itemUnit.smallToLargeUnit((int)cb_fromUnit.SelectedValue, (int)cb_toUnit.SelectedValue);

                    if (_ConversionQuantity != 0)
                    {
                        _ToQuantity = quantity / _ConversionQuantity;
                        remain = quantity - (_ToQuantity * _ConversionQuantity); // get remain quantity which cannot be changeed
                    }
                    _FromUnit = remain.ToString() + " " + cb_fromUnit.Text;
                    _FromQuantity = quantity - remain;
                }
                _ToUnit = _ToQuantity.ToString() + " " + cb_toUnit.Text;
            }

            //_ToQuantity = quantity * conversionQuantity;

            //if (cb_fromUnit.SelectedIndex != -1 && isSmall != null)
            //{
            //    _FromUnit = _ToQuantity.ToString() + " " + cb_fromUnit.Text;
            //    txt_toQuantity.Text = _FromUnit;
            //}

            //if (cb_toUnit.SelectedIndex != -1)
            //{
            //    if(isSmall != null) // from large to small
            //        _ToUnit = _ToQuantity.ToString() + " " + units[cb_toUnit.SelectedIndex].mainUnit;
            //    else if(cb_fromUnit.SelectedIndex != -1)
            //        _FromUnit = remain.ToString() + " " + units[cb_fromUnit.SelectedIndex].mainUnit;
            //}
            txt_toQuantity.Text = _FromUnit;
            txt_toQuantityRemainder.Text = _ToUnit;
        }
        private void clearConversionInputs()
        {
            cb_itemId.SelectedIndex = -1;
            cb_fromUnit.SelectedIndex = -1;
            cb_toUnit.SelectedIndex = -1;
            cb_sectionLocation.SelectedIndex = -1;
            tb_fromQuantity.Clear();
            txt_toQuantity.Text = "";
            txt_toQuantityRemainder.Text = "";
            _ToQuantity = 0;
            _ConversionQuantity = 0;
            _FromUnit = "";
            _ToUnit = "";
            isSmall = null;
            HelpClass.clearValidate( p_error_fromQuantity);
        }
        private async void Tb_validateEmptyTextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                //validateEmpty(name, sender);
                var txb = sender as TextBox;
                if ((sender as TextBox).Name == "tb_fromQuantity")
                    HelpClass.InputJustNumber(ref txb);
                checkLocationQuantity();
                await setToquantityMessage();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                //validateEmpty(name, sender);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
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
                
                    HelpClass.StartAwait(grid_main);

                if (e.Key == Key.Return)
                {
                    Btn_save_Click(null, null);
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task<bool> validateInputs()
        {
            bool valid = true;

            int quantity = int.Parse(tb_fromQuantity.Text);
            if (cb_sectionLocation.SelectedIndex == -1)
            {
                int branchQuantity = await ItemLocation.getUnitAmount((int)cb_fromUnit.SelectedValue, MainWindow.branchLogin.branchId);

                if (branchQuantity < quantity)
                {
                    tb_fromQuantity.Text = branchQuantity.ToString();

                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableFromToolTip"), animation: ToasterAnimation.FadeIn);
                    valid = false;
                    return valid;
                }
            }
            if (isSmall == null && _ConversionQuantity > quantity)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorNoEnoughQuantityToolTip"), animation: ToasterAnimation.FadeIn);
                valid = false;
                return valid;
            }
            return valid;
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
                requiredControlList = new List<string> { "fromQuantity", "toUnit", "fromUnit" };
                await FillCombo.FillComboPurchaseItems(cb_itemId);
                
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
            txt_title.Text = AppSettings.resourcemanager.GetString("trUnitConversion");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_itemId, AppSettings.resourcemanager.GetString("trItemHint")); 
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_fromUnit , AppSettings.resourcemanager.GetString("trFromUnitHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_toUnit, AppSettings.resourcemanager.GetString("trToUnitHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_sectionLocation, AppSettings.resourcemanager.GetString("trFromLocationHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_fromQuantity , AppSettings.resourcemanager.GetString("trQuantityHint"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trConvert");
        }
        //private async Task fillItemCombo()
        //{
        //    if (items is null)
        //        await RefrishItems();
        //    cb_item.ItemsSource = items.ToList();
        //    cb_item.SelectedValuePath = "itemId";
        //    cb_item.DisplayMemberPath = "name";
        //}
        //async Task<IEnumerable<Item>> RefrishItems()
        //{
        //    items = await item.GetItemsWichHasUnits();
        //    return items;
        //}
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
        #region save
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (HelpClass.validate(requiredControlList, this))
                {
                    bool valid = await validateInputs();
                    if (valid)
                    {
                        //int fromQuantity = int.Parse(tb_fromQuantity.Text);

                        if (cb_sectionLocation.SelectedIndex != -1)
                        {
                            var locationId = locations.Find(x => x.itemsLocId == (int)cb_sectionLocation.SelectedValue).locationId;
                            int res = await ItemLocation.transferAmountbetweenUnits((int)locationId, (int)cb_sectionLocation.SelectedValue, (int)cb_toUnit.SelectedValue, _FromQuantity, _ToQuantity, MainWindow.userLogin.userId);
                            if (res > 0)
                            {
                                clearConversionInputs();
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                        else
                        {
                            int res = await ItemLocation.unitsConversion(MainWindow.branchLogin.branchId, (int)cb_fromUnit.SelectedValue, (int)cb_toUnit.SelectedValue, _FromQuantity, _ToQuantity, MainWindow.userLogin.userId, isSmall);
                            if (res > 0)
                            {
                                clearConversionInputs();
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                            }
                            else
                                Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
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
        #endregion
        #region events close - selectionChanged
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isActive = false;
                this.Close();
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Cb_item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_itemId.SelectedIndex != -1)
                {
                    units = FillCombo.itemUnitList.Where(x => x.itemId == (int)cb_itemId.SelectedValue).ToList();

                    cb_fromUnit.ItemsSource = units;
                    cb_fromUnit.SelectedValuePath = "itemUnitId";
                    cb_fromUnit.DisplayMemberPath = "unitName";
                    cb_fromUnit.SelectedIndex = 0;

                    cb_toUnit.ItemsSource = units;
                    cb_toUnit.SelectedValuePath = "itemUnitId";
                    cb_toUnit.DisplayMemberPath = "unitName";
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private async void Cb_fromUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                if (cb_fromUnit.SelectedIndex != -1)
                {
                    smallUnits = await itemUnit.getSmallItemUnits((int)cb_itemId.SelectedValue, (int)cb_fromUnit.SelectedValue);

                    string itemUnitStr = cb_fromUnit.SelectedValue.ToString();
                    locations = await ItemLocation.getSpecificItemLocation(itemUnitStr, MainWindow.branchLogin.branchId);

                    cb_sectionLocation.ItemsSource = locations;
                    cb_sectionLocation.SelectedValuePath = "itemsLocId";
                    cb_sectionLocation.DisplayMemberPath = "location";

                    await setToquantityMessage();
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private  async void Cb_toUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                await setToquantityMessage();
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Cb_sectionLocation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                checkLocationQuantity();
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region checkLocationQuantity
        private void checkLocationQuantity()
        {
            if (cb_sectionLocation.SelectedIndex != -1)
            {
                var locationQuantity = locations.Find(x => x.itemsLocId == (int)cb_sectionLocation.SelectedValue).quantity;
                int quantity = 0;
                if (!tb_fromQuantity.Text.Equals(""))
                    quantity = int.Parse(tb_fromQuantity.Text);
                if (locationQuantity < quantity)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableFromToolTip"), animation: ToasterAnimation.FadeIn);
                    tb_fromQuantity.Text = locationQuantity.ToString();

                }
                setToquantityMessage();
            }
        }
        #endregion

    }
}
