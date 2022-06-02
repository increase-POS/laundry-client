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
using Microsoft.Reporting.WinForms;

namespace laundryApp.View.sectionData.persons
{
    /// <summary>
    /// Interaction logic for uc_users.xaml
    /// </summary>
    public partial class uc_users : UserControl
    {
        public uc_users()
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
        private static uc_users _instance;
        public static uc_users Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_users();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string basicsPermission = "users_basics";
        string storesPermission = "users_stores";
        string permissionPermission = "users_permission";
        User user = new User();
        IEnumerable<User> usersQuery;
        IEnumerable<User> users;
        byte tgl_userState;
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
                requiredControlList = new List<string> { "name", "lastname", "mobile", "job", "username" };

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

                btn_stores.IsEnabled = false;
                await FillCombo.fillCountries(cb_areaMobile);
                await FillCombo.fillCountries(cb_areaPhone);
                FillCombo.FillUserJob(cb_job);

                #region permission
                await FillCombo.FillComboGroups_withDefault(cb_groupId);
                if (FillCombo.groupObject.HasPermissionAction(permissionPermission, FillCombo.groupObjects, "one"))
                    bdr_groupId.Visibility = Visibility.Visible;
                else
                    bdr_groupId.Visibility = Visibility.Collapsed;

                #endregion
                    //await fillJobCombo();
                Keyboard.Focus(tb_name);
                await RefreshUsersList();
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
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_title.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );

            //txt_title.Text = AppSettings.resourcemanager.GetString("trUser");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trUserInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_lastname, AppSettings.resourcemanager.GetString("trLastNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_email, AppSettings.resourcemanager.GetString("trEmailHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pb_password, AppSettings.resourcemanager.GetString("trPasswordHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_mobile, AppSettings.resourcemanager.GetString("trMobileHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_phone, AppSettings.resourcemanager.GetString("trPhoneHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_address, AppSettings.resourcemanager.GetString("trAdressHint"));
            txt_workInformation.Text = AppSettings.resourcemanager.GetString("trWorkInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_job, AppSettings.resourcemanager.GetString("trJobHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_workHours, AppSettings.resourcemanager.GetString("trWorkHoursHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            txt_loginInformation.Text = AppSettings.resourcemanager.GetString("trLoginInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_username, AppSettings.resourcemanager.GetString("trUserNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_password, AppSettings.resourcemanager.GetString("trPasswordHint"));
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");
            btn_stores.Content = AppSettings.resourcemanager.GetString("trBranchs/Stores");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_groupId, AppSettings.resourcemanager.GetString("trPermission"));

            dg_user.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            dg_user.Columns[1].Header = AppSettings.resourcemanager.GetString("trJob");
            dg_user.Columns[2].Header = AppSettings.resourcemanager.GetString("trWorkHours");
            dg_user.Columns[3].Header = AppSettings.resourcemanager.GetString("trNote");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            tt_refresh.Content = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_pieChart.Content = AppSettings.resourcemanager.GetString("trPieChart");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");

            txt_contactInformation.Text = AppSettings.resourcemanager.GetString("trContactInformation");
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        { //add
            try
            {
                HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") )
                {
                    //chk duplicate userName
                    bool duplicateUserName = false;
                    duplicateUserName = await chkIfUserNameIsExists(tb_username.Text, 0);
                    //chk duplicate userFullName
                    bool duplicateUserFullName = true;
                    if (duplicateUserName)
                    {
                        duplicateUserFullName = await chkIfUserFullNameIsExists(tb_name.Text, tb_lastname.Text, 0);
                    }
                    //chk password length
                    bool passLength = false;
                    passLength = chkPasswordLength(pb_password.Password);

                    user = new User();
                    if (HelpClass.validate(requiredControlList, this) && duplicateUserName && duplicateUserFullName && passLength && HelpClass.IsValidEmail(this))
                    {

                            user.username = tb_username.Text;
                            user.password = Md5Encription.MD5Hash("Inc-m" + pb_password.Password);
                            user.name = tb_name.Text;
                            user.lastname = tb_lastname.Text;
                            user.job = cb_job.SelectedValue.ToString();
                            user.workHours = tb_workHours.Text;
                            user.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text; ;
                            if (!tb_phone.Text.Equals(""))
                                user.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                            user.email = tb_email.Text;
                            user.address = tb_address.Text;
                            user.balance = 0;
                            user.balanceType = 0;
                            user.isActive = 1;
                            user.createUserId = MainWindow.userLogin.userId;
                            user.updateUserId = MainWindow.userLogin.userId;
                            user.notes = tb_notes.Text;
                            user.driverIsAvailable = 0;

                        if (cb_groupId.SelectedIndex != -1 && FillCombo.groupObject.HasPermissionAction(permissionPermission, FillCombo.groupObjects, "one"))
                            user.groupId = (int)cb_groupId.SelectedValue;
                            //user.role = "";
                            //user.details = "";

                            int s = await user.save(user);
                        if (s <= 0)
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            if (isImgPressed)
                            {
                                int userId = s;
                                string b = await user.uploadImage(imgFileName,
                                    Md5Encription.MD5Hash("Inc-m" + userId.ToString()), userId);
                                user.image = b;
                                isImgPressed = false;
                            }

                            Clear();
                            await RefreshUsersList();
                            await Search();
                            FillCombo.usersList = users.ToList();
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
                HelpClass.StartAwait(grid_main);
                if (user.userId > 0)
                {
                    if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                    {
                        //chk duplicate userName
                        bool duplicateUserName = false;
                        duplicateUserName = await chkIfUserNameIsExists(tb_username.Text, user.userId);
                        //chk duplicate userFullName
                        bool duplicateUserFullName = true;
                        if (duplicateUserName)
                        {
                            duplicateUserFullName = await chkIfUserFullNameIsExists(tb_name.Text, tb_lastname.Text, user.userId);
                        }
                        if (HelpClass.validate(requiredControlList, this) && duplicateUserName && duplicateUserFullName && HelpClass.IsValidEmail(this))
                        {
                            user.username = tb_username.Text;
                            //user.password = Md5Encription.MD5Hash("Inc-m" + pb_password.Password);
                            user.name = tb_name.Text;
                            user.lastname = tb_lastname.Text;
                            user.job = cb_job.SelectedValue.ToString();
                            user.workHours = tb_workHours.Text;
                            user.mobile = cb_areaMobile.Text + "-" + tb_mobile.Text; ;
                            if (!tb_phone.Text.Equals(""))
                                user.phone = cb_areaPhone.Text + "-" + cb_areaPhoneLocal.Text + "-" + tb_phone.Text;
                            user.email = tb_email.Text;
                            user.address = tb_address.Text;
                            user.updateUserId = MainWindow.userLogin.userId;
                            user.notes = tb_notes.Text;
                            if (cb_groupId.SelectedIndex != -1 && FillCombo.groupObject.HasPermissionAction(permissionPermission, FillCombo.groupObjects, "one"))
                                user.groupId = (int)cb_groupId.SelectedValue;
                            //user.role = "";
                            //user.details = "";

                            int s = await user.save(user);
                            if (s <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await Search();
                                FillCombo.usersList = users.ToList();
                                if (MainWindow.userLogin.userId == s)
                                    MainWindow.userLogin =  user;
                                if (isImgPressed)
                                {
                                    int userId = s;
                                    string b = await user.uploadImage(imgFileName, Md5Encription.MD5Hash("Inc-m" + userId.ToString()), userId);
                                    user.image = b;
                                    isImgPressed = false;
                                    if (!b.Equals(""))
                                    {
                                        getImg();
                                    }
                                    else
                                    {
                                        HelpClass.clearImg(btn_image);
                                    }
                                }
                            }
                        }
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
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") )
                {
                    HelpClass.StartAwait(grid_main);
                    if (user.userId != 0)
                    {
                        if ((!user.canDelete) && (user.isActive == 0))
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
                            if (user.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!user.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (user.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!user.canDelete) && (user.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await user.delete(user.userId, MainWindow.userLogin.userId, user.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    user.userId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                    await RefreshUsersList();
                                    await Search();
                                    Clear();
                                    FillCombo.usersList = users.ToList();
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
            user.isActive = 1;
            int s = await user.save(user);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshUsersList();
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
                if (users is null)
                    await RefreshUsersList();
                tgl_userState = 1;
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
                if (users is null)
                    await RefreshUsersList();
                tgl_userState = 0;
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
        private async void Dg_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_user.SelectedIndex != -1)
                {
                    user = dg_user.SelectedItem as User;
                    this.DataContext = user;
                    if (user != null)
                    {
                    btn_stores.IsEnabled = true;
                        grid_userNameLabel.Visibility = Visibility.Visible;
                        grid_userNameInput.Visibility = Visibility.Collapsed;

                        getImg();
                        #region delete
                        if (user.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (user.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                        HelpClass.getMobile(user.mobile, cb_areaMobile, tb_mobile);
                        HelpClass.getPhone(user.phone, cb_areaPhone, cb_areaPhoneLocal, tb_phone);
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
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);

                tb_search.Text = "";
                searchText = "";
                await RefreshUsersList();
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
            if (users is null)
                await RefreshUsersList();
            searchText = tb_search.Text.ToLower();
            usersQuery = users.Where(s => (s.name.ToLower().Contains(searchText) ||
            s.job.ToLower().Contains(searchText) 
            ) && s.isActive == tgl_userState);
            RefreshCustomersView();
        }
        async Task<IEnumerable<User>> RefreshUsersList()
        {
            users = await user.Get();
            users = users.Where(x => x.isAdmin != true);
            return users;
        }
        void RefreshCustomersView()
        {
            dg_user.ItemsSource = usersQuery;
            txt_count.Text = usersQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            user = new User();
            user.workHours = "0";
            this.DataContext = user;
            numValue_workHours = 1;
            pb_password.Password = "";
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            grid_userNameLabel.Visibility = Visibility.Collapsed;
            grid_userNameInput.Visibility = Visibility.Visible;

            #region mobile-Phone-fax-email
            brd_areaPhoneLocal.Visibility =  Visibility.Collapsed;
            cb_areaMobile.SelectedIndex = -1;
            cb_areaPhone.SelectedIndex = -1;
            cb_areaPhoneLocal.SelectedIndex = -1;
            tb_mobile.Clear();
            tb_phone.Clear();
            tb_email.Clear();
            #endregion
            #region image
            HelpClass.clearImg(btn_image);
            #endregion


            // last 
            HelpClass.clearValidate(requiredControlList, this);
            p_error_email.Visibility = Visibility.Collapsed;
            btn_stores.IsEnabled = false;
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
                if ((sender as TextBox).Name == "tb_workHours")
                {
                    HelpClass.InputJustNumber(ref tb_workHours);
                    if (tb_workHours != null)
                        if (!int.TryParse(tb_workHours.Text, out _numValue_workHours))
                            tb_workHours.Text = _numValue_workHours.ToString();
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
        #region Phone
        int? countryid;
        private async void Cb_areaPhone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                if (cb_areaPhone.SelectedValue != null)
                {
                    if (cb_areaPhone.SelectedIndex >= 0)
                    {
                        countryid = int.Parse(cb_areaPhone.SelectedValue.ToString());
                        await FillCombo.fillCountriesLocal(cb_areaPhoneLocal, (int)countryid, brd_areaPhoneLocal);
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
            try
            {
                HelpClass.StartAwait(grid_image, "forImage");
                if (string.IsNullOrEmpty(user.image))
                {
                    HelpClass.clearImg(btn_image);
                }
                else
                {
                    byte[] imageBuffer = await user.downloadImage(user.image); // read this as BLOB from your DB

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
                        string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                        string tmpPath = System.IO.Path.Combine(dir, Global.TMPUsersFolder);
                        tmpPath = System.IO.Path.Combine(tmpPath, user.image);
                        openFileDialog.FileName = tmpPath;
                    }
                    else
                        HelpClass.clearImg(btn_image);
                }
                HelpClass.EndAwait(grid_image, "forImage");
            }
            catch
            {
                HelpClass.EndAwait(grid_image, "forImage");
            }
        }
        #endregion
     
        

        private void Cb_job_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                ComboBox cbm = sender as ComboBox;
                HelpClass.searchInComboBox(cbm);
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }

        #region Password
        private void ValidateEmpty_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
                p_error_password.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void P_showPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                tb_password.Text = pb_password.Password;
                tb_password.Visibility = Visibility.Visible;
                pb_password.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        private void P_showPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                tb_password.Visibility = Visibility.Collapsed;
                pb_password.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        private bool chkPasswordLength(string password)
        {
            bool isValid = true;

            if (password.Length < 6)
                isValid = false;

            if (!isValid)
            {
                p_error_password.Visibility = Visibility.Visible;
                #region Tooltip
                ToolTip toolTip = new ToolTip();
                toolTip.Content = AppSettings.resourcemanager.GetString("trErrorPasswordLengthToolTip");
                toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
                p_error_password.ToolTip = toolTip;
                #endregion
            }

            return isValid;
        }
        #endregion
        /*
        private async Task fillJobCombo()
        {
            if (users == null)
              await RefreshUsersList();
            usersQuery = users.Where(s => s.isActive == 1);

            //List<User> userList = new List<User>();
            //userList.AddRange(usersQuery.ToList());
            //for (int i = 0; i < userList.Count(); i++)
            //    if (!cb_job.Items.Contains(userList[i].job))
            //        cb_job.Items.Add(userList[i].job);

            var userList = users.GroupBy(x => x.job);
            foreach (var item in userList)
            {
                cb_job.Items.Add(item.Key);
            }

        }
        */
        private async Task<bool> chkIfUserNameIsExists(string username, int uId)
        {
            bool isValid = true;
            if (users == null)
                await RefreshUsersList();
            if (users.Any(i => i.username == username && i.userId != uId))
                isValid = false;
            if (!isValid)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorDuplicateUserNameToolTip"), animation: ToasterAnimation.FadeIn);
            return isValid;
        }
        private async Task<bool> chkIfUserFullNameIsExists(string name , string lastName, int uId)
        {
            bool isValid = true;
            if (users == null)
                await RefreshUsersList();
            if (users.Any(i => i.name == name && i.lastname == lastName && i.userId != uId))
                isValid = false;
            if (!isValid)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorDuplicateUserFullNameToolTip"), animation: ToasterAnimation.FadeIn);
            return isValid;
        }
        private async void Btn_stores_Click(object sender, RoutedEventArgs e)
        {
            //stores
            try
            {
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(storesPermission, FillCombo.groupObjects, "one"))
                {
                    Window.GetWindow(this).Opacity = 0.2;

                    wd_branchesList w = new wd_branchesList();
                    w.Id = user.userId;
                    w.userOrBranch = 'u';
                    w.ShowDialog();
                    await FillCombo.RefreshByBranchandUser();
                    Window.GetWindow(this).Opacity = 1;
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

        #region report
        //report  parameters
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        //  SaveFileDialog saveFileDialog = new SaveFileDialog();

        // end report parameters
        public void BuildReport()
        {

            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\SectionData\persons\Ar\ArUsers.rdlc";
            }
            else
            {
                addpath = @"\Reports\SectionData\persons\En\EnUsers.rdlc";
            }
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            clsReports.UserReport(usersQuery, rep, reppath, paramarr);
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

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
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

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
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

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report") )
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
        private void Btn_pieChart_Click(object sender, RoutedEventArgs e)
        {//pie
            try
            {
                HelpClass.StartAwait(grid_main);

                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    win_lvc win = new win_lvc(usersQuery, 3);
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
        #endregion
        #region NumericCount



        private int _numValue_workHours = 1;
        public int numValue_workHours
        {
            get
            {
                if (int.TryParse(tb_workHours.Text, out _numValue_workHours))
                    _numValue_workHours = int.Parse(tb_workHours.Text);
                return _numValue_workHours;
            }
            set
            {
                _numValue_workHours = value;
                tb_workHours.Text = value.ToString();
            }
        }




        private void Btn_countDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;

                if (button.Tag.ToString() == "workHours" && numValue_workHours > 0)
                    numValue_workHours--;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_countUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                if (button.Tag.ToString() == "workHours")
                    numValue_workHours++;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion

    }
}
