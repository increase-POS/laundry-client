using netoaster;
using laundryApp.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using laundryApp.View.windows;
using Zen.Barcode;

using Microsoft.Reporting.WinForms;

namespace laundryApp.View.catalog.salesItems
{
    /// <summary>
    /// Interaction logic for uc_salesItem.xaml
    /// </summary>
    public partial class uc_salesItem : UserControl
    {
        public uc_salesItem()
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
        private static uc_salesItem _instance;
        public static uc_salesItem Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                    _instance = new uc_salesItem();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public static string categoryName;
        int categoryId;
        string basicsPermission = "basics";
        string dishIngredientsPermission = "dishIngredients";
        byte tgl_itemState;
        string searchText = "";
        public static List<string> requiredControlList;
        public static List<Tag> tagsList;

        List<Unit> units;
        Unit unit = new Unit();
        #region for barcode
        DateTime _lastKeystroke = new DateTime(0);
        static private string _BarcodeStr = "";
        #endregion
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.KeyDown -= HandleKeyPress;
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                MainWindow.mainWindow.KeyDown += HandleKeyPress;
                // for pagination onTop Always
                btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                catigoriesAndItemsView.ucsalesItem = this;
 
                requiredControlList = new List<string> { "code", "name" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                //categoryName = "appetizers";
                if(categoryName == "appetizers")
                {
                    basicsPermission = "appetizers_basics";
                    dishIngredientsPermission = "appetizers_dishIngredients";
                }
                else if(categoryName == "beverages")
                {
                    basicsPermission = "beverages_basics";
                    dishIngredientsPermission = "beverages_dishIngredients";
                }
                else if(categoryName == "fastFood")
                {
                    basicsPermission = "fastFood_basics";
                    dishIngredientsPermission = "fastFood_dishIngredients";
                }
                else if(categoryName == "mainCourses")
                {
                    basicsPermission = "mainCourses_basics";
                    dishIngredientsPermission = "mainCourses_dishIngredients";
                }
                else if(categoryName == "desserts")
                {
                    basicsPermission = "desserts_basics";
                    dishIngredientsPermission = "desserts_dishIngredients";
                }



                categoryId = FillCombo.GetCategoryId(categoryName);
                tagsList = await FillCombo.fillTags(cb_tagId, categoryId);
                Keyboard.Focus(tb_code);
                await RefreshItemsList();
                refreshCatalogTags();
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

            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == categoryName).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == categoryName).FirstOrDefault().translate
               );

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_details, AppSettings.resourcemanager.GetString("trDetailsHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_code, AppSettings.resourcemanager.GetString("trCode") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNote") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_tagId, AppSettings.resourcemanager.GetString("trTag")+"...");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_price, AppSettings.resourcemanager.GetString("trPrice") + "...");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_priceWithService, AppSettings.resourcemanager.GetString("trPriceWithService") + "...");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcode") + "...");
            btn_dishIngredients.Content = AppSettings.resourcemanager.GetString("dishIngredients");

            txt_contentInformatin.Text = AppSettings.resourcemanager.GetString("trMoreInformation");

            btn_add.Content = AppSettings.resourcemanager.GetString("trAdd");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");
            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");



        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh - validateValues
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);

                    item = new Item();
                    if (HelpClass.validate(requiredControlList, this))
                    {
                        Boolean codeAvailable = checkCodeAvailabiltiy();
                        Boolean barcodeCorrect = isBarcodeCorrect(tb_barcode.Text);
                       if (codeAvailable && barcodeCorrect)
                        {
                            item = new Item();
                            item.code = tb_code.Text;
                            item.name = tb_name.Text;
                            item.details = tb_details.Text;
                            item.notes = tb_notes.Text;
                            item.image = "";
                            item.isActive = 1;
                            if (cb_tagId.SelectedIndex != -1)
                                item.tagId = (int)cb_tagId.SelectedValue;
                            item.categoryId = categoryId;
                            item.createUserId = MainWindow.userLogin.userId;
                            item.updateUserId = MainWindow.userLogin.userId;
                            item.type = "SalesNormal";
                            item.taxes = 0;
                            // sale item unit
                            ItemUnit saleItemUnit = new ItemUnit();
                            saleItemUnit.unitId = saleItemUnit.subUnitId = FillCombo.saleUnit.unitId;
                            saleItemUnit.itemId = item.itemId;
                            saleItemUnit.isActive = 1;
                            saleItemUnit.defaultSale = 1;
                            //decimal price = 0;
                            //decimal priceWithServ = 0;
                            //try
                            //{
                            //    price = decimal.Parse(tb_price.Text);
                            //    priceWithServ = decimal.Parse(tb_priceWithService.Text);
                            //}
                            //catch { }
                            //if (priceWithServ != 0 && price == 0)
                            //    price = priceWithServ;
                            //else if (price != 0 && priceWithServ == 0)
                            //    priceWithServ = price;
                            //saleItemUnit.price = price;
                            //saleItemUnit.priceWithService = priceWithServ;
                            saleItemUnit.barcode = tb_barcode.Text;
                            saleItemUnit.unitValue = 1;
                            saleItemUnit.taxes = 0;
                            saleItemUnit.createUserId = MainWindow.userLogin.userId;

                            int res = await item.saveSaleItem(item, saleItemUnit);
                            if (res == -1)// إظهار رسالة الترقية
                                Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpgrade"), animation: ToasterAnimation.FadeIn);

                            else if (res == 0) // an error occure
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                item.itemId = res;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                if (openFileDialog.FileName != "")
                                    await item.uploadImage(openFileDialog.FileName, Md5Encription.MD5Hash("Inc-m" + item.itemId.ToString()), item.itemId);
                               
                                Clear();
                                await RefreshItemsList();                              
                                await Search();
                                await FillCombo.RefreshItemUnit();
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
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                {
                    HelpClass.StartAwait(grid_main);
                    if (item.itemId > 0)
                    {

                        if (HelpClass.validate(requiredControlList, this))
                    {
                        Boolean codeAvailable = checkCodeAvailabiltiy(item.code);
                        Boolean valid = validateValues();
                        if (codeAvailable && valid)
                        {
                            item.code = tb_code.Text;
                            item.name = tb_name.Text;
                            item.details = tb_details.Text;
                            item.notes = tb_notes.Text;
                            item.image = "";
                            item.isActive = 1;
                            if(cb_tagId.SelectedIndex != -1)
                                item.tagId = (int)cb_tagId.SelectedValue;
                            item.categoryId = categoryId;
                            item.updateUserId = MainWindow.userLogin.userId;
                            item.type = "SalesNormal";
                            item.taxes = 0;
                            // sale item unit
                            ItemUnit saleItemUnit = new ItemUnit();
                            saleItemUnit.unitId = saleItemUnit.subUnitId = FillCombo.saleUnit.unitId;
                            saleItemUnit.itemId = item.itemId;
                            //decimal price = 0;
                            //decimal priceWithServ = 0;
                            //try
                            //{
                            //    price = decimal.Parse(tb_price.Text);
                            //    priceWithServ = decimal.Parse(tb_priceWithService.Text);
                            //}
                            //catch { }
                            //if (priceWithServ != 0 && price == 0)
                            //    price = priceWithServ;
                            //else if (price != 0 && priceWithServ == 0)
                            //    priceWithServ = price;
                            //saleItemUnit.price = price;
                            //saleItemUnit.priceWithService = priceWithServ;
                            saleItemUnit.barcode = tb_barcode.Text;
                            saleItemUnit.unitValue = 1;
                            saleItemUnit.taxes = 0;
                            saleItemUnit.createUserId = MainWindow.userLogin.userId;

                            int res = await item.saveSaleItem(item, saleItemUnit);
                            if (res == -1)// إظهار رسالة الترقية
                                Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpgrade"), animation: ToasterAnimation.FadeIn);

                            else if (res == 0) // an error occure
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                item.itemId = res;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                if (openFileDialog.FileName != "")
                                    await item.uploadImage(openFileDialog.FileName, Md5Encription.MD5Hash("Inc-m" + item.itemId.ToString()), item.itemId);

                                Clear();
                                await RefreshItemsList();
                                await Search();
                                await FillCombo.RefreshItemUnit();
                            }
                        }
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
        {
            try
            {//delete
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") || HelpClass.isAdminPermision())
                {
                    HelpClass.StartAwait(grid_main);
                    if (item.itemId != 0)
                    {
                        if ((!item.canDelete) && (item.isActive == 0))
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxActivate");
                    // w.ShowInTaskbar = false;
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                await activate();
                                Clear();
                            }
                        }
                        else
                        {
                            #region
                            Window.GetWindow(this).Opacity = 0.2;
                            wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                            if (item.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!item.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                    // w.ShowInTaskbar = false;
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (item.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!item.canDelete) && (item.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await item.delete(item.itemId, MainWindow.userLogin.userId, item.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    item.itemId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshItemsList();
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
            item.isActive = 1;
            int s = await item.save(item);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshItemsList();
                await Search();
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
                tb_barcode.Text = checkDigit + tb_barcode.Text;
            }
            else if (tb_barcode.Text.Length == 13)
            {
                valid = isBarcodeCorrect(tb_barcode.Text);
            }           
            return valid;
        }
        #endregion
        #region events
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
                if (e.Key.ToString() == "Return" && _BarcodeStr != "")
                {
                    // get item matches barcode
                    if (FillCombo.itemUnitList != null)
                    {
                        var ob = FillCombo.itemUnitList.ToList().Find(c => c.barcode == _BarcodeStr && c.categoryId == categoryId);
                        if (ob != null)
                        {
                            int itemId = (int)ob.itemId;
                            ChangeItemIdEvent(itemId);
                        }
                        else
                        {
                            Clear();
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorItemNotFoundToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    _BarcodeStr = "";
                }
            }
            catch (Exception ex)
            {
                _BarcodeStr = "";
                HelpClass.ExceptionMessage(ex, this);
            }
        }
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
                if (items is null)
                    await RefreshItemsList();
                tgl_itemState = 1;
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
                if (items is null)
                    await RefreshItemsList();
                tgl_itemState = 0;
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

        //private async void Dg_item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        HelpClass.StartAwait(grid_main);
        //        //selection
        //        if (dg_item.SelectedIndex != -1)
        //        {
        //            item = dg_item.SelectedItem as Item;
        //            this.DataContext = item;
        //            if (item != null)
        //            {
        //                await getImg();
        //                #region delete
        //                if (item.canDelete)
        //                    btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
        //                else
        //                {
        //                    if (item.isActive == 0)
        //                        btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
        //                    else
        //                        btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
        //                }
        //                #endregion
        //                HelpClass.getMobile(item.mobile, cb_areaMobile, tb_mobile);
        //                HelpClass.getPhone(item.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
        //                HelpClass.getPhone(item.fax, cb_areaFax, cb_areaFaxLocal, tb_fax);
        //            }
        //        }
        //        HelpClass.clearValidate(requiredControlList, this);
        //        HelpClass.EndAwait(grid_main);
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.EndAwait(grid_main);
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshItemsList();
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

        /*
        async Task Search()
        {
            //search
            if (items is null)
                await RefreshItemsList();
            searchText = tb_search.Text.ToLower();
            itemsQuery = items.Where(s => (s.code.ToLower().Contains(searchText) ||
            s.name.ToLower().Contains(searchText) ||
            s.mobile.ToLower().Contains(searchText)
            ) && s.isActive == tgl_itemState);
            RefreshCustomersView();
        }
        async Task<IEnumerable<Item>> RefreshItemsList()
        {
            items = await item.Get("c");
            return items;
        }
        void RefreshCustomersView()
        {
            dg_item.ItemsSource = itemsQuery;
            txt_count.Text = itemsQuery.Count().ToString();
        }
        */
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            item = new Item();
            item.price = 0;
            generateBarcode();
            this.DataContext = item;   
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            #region image
            HelpClass.clearImg(btn_image);
            openFileDialog.FileName = "";
            #endregion
            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
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
        #region Image
        string imgFileName = "pic/no-image-icon-125x125.png";
        bool isImgPressed = false;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void Btn_image_Click(object sender, RoutedEventArgs e)
        {
            //select image
            try
            {
                HelpClass.StartAwait(grid_main);
                isImgPressed = true;
                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    HelpClass.imageBrush = new ImageBrush();
                    HelpClass.imageBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    btn_image.Background = HelpClass.imageBrush;
                    imgFileName = openFileDialog.FileName;
                }
                else
                { }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async Task getImg()
        {
            if (string.IsNullOrEmpty(item.image))
            {
                HelpClass.clearImg(btn_image);
            }
            else
            {
                byte[] imageBuffer = await item.downloadImage(item.image); // read this as BLOB from your DB

                var bitmapImage = new BitmapImage();
                if (imageBuffer != null)
                {
                    using (var memoryStream = new MemoryStream(imageBuffer))
                    {
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                    }

                    btn_image.Background = new ImageBrush(bitmapImage);
                    // configure trmporary path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = System.IO.Path.Combine(dir, Global.TMPItemsFolder, item.image);
                    openFileDialog.FileName = tmpPath;
                }
                else
                    HelpClass.clearImg(btn_image);
            }
        }
        #endregion
        #region report

        //report  parameters
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        //   SaveFileDialog saveFileDialog = new SaveFileDialog();

        // end report parameters
        public void BuildReport()
        {

            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Catalog\Ar\ArFood.rdlc";
            }
            else
            {
                addpath = @"\Reports\Catalog\En\EnFood.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            clsReports.FoodReport(itemsQuery.ToList(), rep, reppath, paramarr, categoryName);



            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trItems")));

            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {

            //pdf
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
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
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;

                    string pdfpath = "";
                    //
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {

                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
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
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    //Thread t1 = new Thread(() =>
                    //{
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


                    //});
                    //t1.Start();
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
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    //Window.GetWindow(this).Opacity = 0.2;
                    //win_lvc win = new win_lvc(banksQuery, 5);
                    //win.ShowDialog();
                    //Window.GetWindow(this).Opacity = 1;
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




        #endregion
        #region  Cards
        CatigoriesAndItemsView catigoriesAndItemsView = new CatigoriesAndItemsView();
        #region Refrish Y
        Item item = new Item();
        IEnumerable<Item> items;
        IEnumerable<Item> itemsQuery;
        async Task<IEnumerable<Item>> RefreshItemsList()
        {
            items = await item.GetSalesItems();
            if(items != null)
                items = items.Where(x => x.categoryId == categoryId & x.type != "packageItems").ToList();
            return items;
        }
        void RefrishItemsCard(IEnumerable<Item> _items)
        {
            grid_itemContainerCard.Children.Clear();
            catigoriesAndItemsView.gridCatigorieItems = grid_itemContainerCard;
            catigoriesAndItemsView.FN_refrishCatalogItem(_items.ToList(),3,5, "purchase");
        }
        #endregion
        #region Get Id By Click  Y

        public async void ChangeItemIdEvent(int itemId)
        {
            try
            {
               HelpClass.StartAwait(grid_main);
                item = items.Where(x => x.itemId == itemId).FirstOrDefault();
                this.DataContext = item;
                drawBarcode(item.barcode);
                await getImg();
                #region delete
                if (item.canDelete)
                    btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                else
                {
                    if (item.isActive == 0)
                        btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                    else
                        btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                }
                #endregion
                HelpClass.clearValidate(requiredControlList,this);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Search Y


        /// <summary>
        /// Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async Task Search()
        {
            //search
            try
            {
                HelpClass.StartAwait(grid_main);

                if (items is null)
                    await RefreshItemsList();
                searchText = tb_search.Text;
                itemsQuery = items.
                Where(x => (x.code.ToLower().Contains(searchText) ||
                    x.name.ToLower().Contains(searchText) ||
                    x.details.ToLower().Contains(searchText)
                    // || x.categoryName.ToLower().Contains(searchText)
                    ) && x.isActive == tgl_itemState);

                #region search for tag
                if (tagId > 0)
                    itemsQuery = itemsQuery.Where(x => x.tagId == tagId).ToList();
                #endregion
                pageIndex = 1;
                #region


                if (btns is null)
                    btns = new Button[] { btn_firstPage, btn_prevPage, btn_activePage, btn_nextPage, btn_lastPage };
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region Pagination Y
        Pagination pagination = new Pagination();
        Button[] btns;
        public int pageIndex = 1;

        private void Tb_pageNumberSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                itemsQuery = items.ToList();

                if (tb_pageNumberSearch.Text.Equals(""))
                {
                    pageIndex = 1;
                }
                else if (((itemsQuery.Count() - 1) / 9) + 1 < int.Parse(tb_pageNumberSearch.Text))
                {
                    pageIndex = ((itemsQuery.Count() - 1) / 9) + 1;
                }
                else
                {
                    pageIndex = int.Parse(tb_pageNumberSearch.Text);
                }

                #region

                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_firstPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = 1;
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_prevPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_prevPage.Content.ToString());
                #region
                itemsQuery = items.ToList();

                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_activePage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_activePage.Content.ToString());
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_nextPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                pageIndex = int.Parse(btn_nextPage.Content.ToString());
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_lastPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                itemsQuery = items.ToList();
                pageIndex = ((itemsQuery.Count() - 1) / 9) + 1;
                #region
                itemsQuery = items.ToList();
                RefrishItemsCard(pagination.refrishPagination(itemsQuery, pageIndex, btns, 15));
                #endregion

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

        #endregion
        #region tags
        void refreshCatalogTags()
        {
            if (tagsList.Count > 1)
            {
                Tag allTag = new Tag();
                allTag.tagName = AppSettings.resourcemanager.GetString("trAll");
                allTag.tagId = 0;
                tagsList.Add(allTag);
            }
            sp_menuTags.Children.Clear();
            foreach (var item in tagsList)
            {
                #region  
                Button button = new Button();
                button.Content = item.tagName;
                button.Tag = "catalogTags-" + item.tagName;
                button.FontSize = 10;
                button.Height = 25;
                button.Padding = new Thickness(5, 0, 5, 0);
                MaterialDesignThemes.Wpf.ButtonAssist.SetCornerRadius(button, (new CornerRadius(7)));
                button.Margin = new Thickness(5, 0, 5, 0);
                if (item.tagName == AppSettings.resourcemanager.GetString("trAll"))
                {
                    button.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    button.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                }
                else
                {
                    button.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    button.Background = Application.Current.Resources["White"] as SolidColorBrush;
                }
                button.BorderBrush = Application.Current.Resources["MainColor"] as SolidColorBrush;
                button.Click += buttonCatalogTags_Click;


                sp_menuTags.Children.Add(button);
                /////////////////////////////////

                #endregion
            }
        }
        int tagId = 0;
        async void buttonCatalogTags_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string senderTag = (sender as Button).Tag.ToString();
                #region refresh colors
                foreach (var control in tagsList)
                {
                    Button button = FindControls.FindVisualChildren<Button>(this).Where(x => x.Tag != null && x.Tag.ToString() == "catalogTags-" + control.tagName)
                        .FirstOrDefault();
                    if (button.Tag.ToString() == senderTag)
                    {
                        button.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                        button.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        tagId = control.tagId;
                    }
                    else
                    {
                        button.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        button.Background = Application.Current.Resources["White"] as SolidColorBrush;
                    }
                }
                #endregion
                await Search();
            }
            catch { }
        }
        #endregion
        private Boolean checkCodeAvailabiltiy(string oldCode = "")
        {
            string code = tb_code.Text;

            if (code != oldCode && items.ToList().Count > 0)
            {
                var match = items.Where(x => x.code.Contains(code)).FirstOrDefault();

                if ( match != null)
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trDuplicateCodeToolTip"), animation: ToasterAnimation.FadeIn);
                    #region Tooltip_code
                    p_error_code.Visibility = Visibility.Visible;
                    ToolTip toolTip_code = new ToolTip();
                    toolTip_code.Content = AppSettings.resourcemanager.GetString("trDuplicateCodeToolTip");
                    toolTip_code.Style = Application.Current.Resources["ToolTipError"] as Style;
                    p_error_code.ToolTip = toolTip_code;
                    #endregion
                    return false;
                }
                else
                {
                    HelpClass.clearValidate(p_error_code);
                    return true;
                }
            }
            else
            {
                HelpClass.clearValidate(p_error_code);
                return true;
            }
        }
       
        #region barcode
        List<ItemUnit> barcodesList;
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

        #endregion

        private async void Btn_dishIngredients_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //items
                if (item.itemId > 0)
                {

                    if (FillCombo.groupObject.HasPermissionAction(dishIngredientsPermission, FillCombo.groupObjects, "one"))
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        await FillCombo.RefreshItemUnit();
                        wd_dishIngredients w = new wd_dishIngredients();
                        w.itemUnitId = FillCombo.itemUnitList.Where(x => x.itemId == item.itemId).FirstOrDefault().itemUnitId;
                    // w.ShowInTaskbar = false;
                        w.ShowDialog();
                        Window.GetWindow(this).Opacity = 1;

                    }
                    else
                        Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

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
