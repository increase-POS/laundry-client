using netoaster;
using laundryApp.Classes;
using laundryApp.View.windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Group = laundryApp.Classes.Group;
using Object = laundryApp.Classes.Object;

namespace laundryApp.View.settings
{
    /// <summary>
    /// Interaction logic for uc_permissions.xaml
    /// </summary>
    public partial class uc_permissions : UserControl
    {
        public uc_permissions()
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
        private static uc_permissions _instance;
        public static uc_permissions Instance
        {
            get
            {
                //if (_instance == null)
                if(_instance is null)
                _instance = new uc_permissions();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        string basicsPermission = "permissions_basics";
        string usersPermission = "permissions_users";
        string _parentObjectName = "";
        Object objectModel = new Object();
        List<Object> listObjects = new List<Object>();
        Group group = new Group();
        IEnumerable<Group> groupsQuery;
        IEnumerable<Group> groups;
        byte tgl_groupState;
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
                requiredControlList = new List<string> { "name" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();
                //btn_tabs_Click(btn_home,null);
                Keyboard.Focus(tb_name);
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

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_name, AppSettings.resourcemanager.GetString("trNameHint"));
            btn_refresh.ToolTip = AppSettings.resourcemanager.GetString("trRefresh");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            txt_active.Text = AppSettings.resourcemanager.GetString("trActive_");
            btn_usersList.Content = AppSettings.resourcemanager.GetString("trUsers");
            btn_save.Content = AppSettings.resourcemanager.GetString("trUpdate");



            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));

            dg_group.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            dg_group.Columns[1].Header = AppSettings.resourcemanager.GetString("trNote");
            
            dg_permissions.Columns[0].Header = AppSettings.resourcemanager.GetString("trPermission");
            dg_permissions.Columns[1].Header = AppSettings.resourcemanager.GetString("trShow");
            dg_permissions.Columns[2].Header = AppSettings.resourcemanager.GetString("trAdd");
            dg_permissions.Columns[3].Header = AppSettings.resourcemanager.GetString("trUpdate");
            dg_permissions.Columns[4].Header = AppSettings.resourcemanager.GetString("trDelete");
            dg_permissions.Columns[5].Header = AppSettings.resourcemanager.GetString("trReports");


            txt_title.Text = AppSettings.resourcemanager.GetString("trPermission");
            txt_groupDetails.Text = AppSettings.resourcemanager.GetString("trDetails");
            txt_groups.Text = AppSettings.resourcemanager.GetString("trGroups");


        }
        
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add") )
                {
                    HelpClass.StartAwait(grid_main);

                    group = new Group();
                    if (HelpClass.validate(requiredControlList, this))
                    {

                        bool isGroupExist = await chkDuplicateGroup();
                        if (isGroupExist)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopGroupExist"), animation: ToasterAnimation.FadeIn);
                            #region Tooltip_name
                            p_error_name.Visibility = Visibility.Visible;
                            ToolTip toolTip_name = new ToolTip();
                            toolTip_name.Content = AppSettings.resourcemanager.GetString("trDuplicateCodeToolTip");
                            toolTip_name.Style = Application.Current.Resources["ToolTipError"] as Style;
                            p_error_name.ToolTip = toolTip_name;
                            #endregion
                        }
                        else
                        {
                            group.name = tb_name.Text;
                            group.createUserId = MainWindow.userLogin.userId;
                            group.updateUserId = MainWindow.userLogin.userId;
                            group.notes = tb_notes.Text;
                            group.isActive = 1;

                            int s = await group.Save(group);
                            if (s <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                group.groupId = s;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);



                                Clear();

                                await RefreshGroupsList();
                                await RefreshGroupObjectList();
                                await Search();
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
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") )
                {
                    HelpClass.StartAwait(grid_main);
                    if (group.groupId > 0)
                    {
                        if (HelpClass.validate(requiredControlList, this))
                    {
                        bool isGroupExist = await chkDuplicateGroup();
                        if (isGroupExist)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopGroupExist"), animation: ToasterAnimation.FadeIn);
                            #region Tooltip_name
                            p_error_name.Visibility = Visibility.Visible;
                            ToolTip toolTip_name = new ToolTip();
                            toolTip_name.Content = AppSettings.resourcemanager.GetString("trDuplicateCodeToolTip");
                            toolTip_name.Style = Application.Current.Resources["ToolTipError"] as Style;
                            p_error_name.ToolTip = toolTip_name;
                            #endregion
                        }
                        else
                        {
                            group.name = tb_name.Text;
                            group.updateUserId = MainWindow.userLogin.userId;
                            group.notes = tb_notes.Text;

                            int s = await group.Save(group);
                            if (s <= 0)
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                group.groupId = s;
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await RefreshGroupsList();
                                await Search();
                                await RefreshGroupObjectList();
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
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete") )
                {
                    HelpClass.StartAwait(grid_main);
                    if (group.groupId != 0)
                    {
                        if ((!group.canDelete) && (group.isActive == 0))
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
                            if (group.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");
                            if (!group.canDelete)
                                w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDeactivate");
                            w.ShowDialog();
                            Window.GetWindow(this).Opacity = 1;
                            #endregion
                            if (w.isOk)
                            {
                                string popupContent = "";
                                if (group.canDelete) popupContent = AppSettings.resourcemanager.GetString("trPopDelete");
                                if ((!group.canDelete) && (group.isActive == 1)) popupContent = AppSettings.resourcemanager.GetString("trPopInActive");

                                int s = await group.Delete(group.groupId, MainWindow.userLogin.userId, group.canDelete);
                                if (s < 0)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    group.groupId = 0;
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);
                                    await RefreshGroupsList();
                                    await Search();
                                    await RefreshGroupObjectList();
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
            group.isActive = 1;
            int s = await group.Save(group);
            if (s <= 0)
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
            else
            {
                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopActive"), animation: ToasterAnimation.FadeIn);
                await RefreshGroupsList();
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
                if (groups is null)
                    await RefreshGroupsList();
                tgl_groupState = 1;
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
                if (groups is null)
                    await RefreshGroupsList();
                tgl_groupState = 0;
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
        private void Dg_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_group.SelectedIndex != -1)
                {
                    group = dg_group.SelectedItem as Group;
                    this.DataContext = group;
                    if (group != null)
                    {
                        #region delete
                        if (group.canDelete)
                            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
                        else
                        {
                            if (group.isActive == 0)
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trActive");
                            else
                                btn_delete.Content = AppSettings.resourcemanager.GetString("trInActive");
                        }
                        #endregion
                    }
                }
                RefreshGroupObjectsView();
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
                await RefreshGroupsList();
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
            if (groups is null)
                await RefreshGroupsList();
            searchText = tb_search.Text.ToLower();
            groupsQuery = groups.Where(s => (s.name.ToLower().Contains(searchText)
            ) && s.isActive == tgl_groupState);
            RefreshGroupsView();
        }
        async Task<IEnumerable<Group>> RefreshGroupsList()
        {
            groups = await group.GetAll();
            return groups;
        }
        void RefreshGroupsView()
        {
            dg_group.ItemsSource = groupsQuery;
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new Group();
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");



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


        private async Task<bool> chkDuplicateGroup()
        {
            bool b = false;


            List<Group> groups = await group.GetAll();
            Group group1 = new Group();

            for (int i = 0; i < groups.Count(); i++)
            {
                group1 = groups[i];
                if ((group1.name.Equals(tb_name.Text.Trim())) &&
                    (group1.groupId != group.groupId))
                { b = true; break; }
            }

            return b;
        }
        #region tabs
        private void btn_tabs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                paint(button.Tag.ToString());
                //isEnabledButtons(button.Tag.ToString());
                //grid_home.Visibility = Visibility.Visible;
                //btn_home.Opacity = 1;
                List<Object> list =   Object.findChildrenList(button.Tag.ToString(), FillCombo.objectsList);
                list = list.Where(x => x.objectType == "basic" || x.objectType == "basicAlert").ToList();
                //string s = "";
                //foreach (var item in list)
                //{
                //    s += item.name+" \n";
                //}
                BuildObjectsDesign(list);
                initializationMainTrack(button.Tag.ToString());
                _parentObjectName = button.Tag.ToString();
                RefreshGroupObjectsView();
                //MessageBox.Show(s);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        //private void isEnabledButtons(string tag)
        //{
        //    // btn
        //    List<Button> tabsButtonsList = FindControls.FindVisualChildren<Button>(this)
        //        .Where(x =>  x.Tag != null).ToList();
        //    foreach (var item in tabsButtonsList)
        //    {
        //        if (item.Tag.ToString() == tag)
        //        {
        //            item.IsEnabled = false;
        //            item.Opacity = 1;
        //        }
        //        else
        //            item.IsEnabled = true;
        //    }
        //    //btn_home.IsEnabled = true;
        //    //btn_catalog.IsEnabled = true;
        //    //btn_store.IsEnabled = true;
        //    //btn_sale.IsEnabled = true;
        //    //btn_purchase.IsEnabled = true;
        //    //btn_sectionData.IsEnabled = true;
        //    //btn_reports.IsEnabled = true;
        //    //btn_settings.IsEnabled = true;
        //    //btn_alerts.IsEnabled = true;
        //    //btn_account.IsEnabled = true;
        //}
        public void paint(string tag)
        {
            bdrMain.RenderTransform = Animations.borderAnimation(50, bdrMain, true);

            // bdr
            List<Border> tabsBordersList = FindControls.FindVisualChildren<Border>(this)
                .Where(x => x.Tag != null).ToList();
            foreach (var item in tabsBordersList)
            {
                if (item.Tag.ToString() == tag)
                    item.Background = Application.Current.Resources["White"] as SolidColorBrush;
                else
                    item.Background = Application.Current.Resources["SecondColor"] as SolidColorBrush; 
            }
            // path
            List<Path> tabsPathsList = FindControls.FindVisualChildren<Path>(this)
                .Where(x => x.Tag != null).ToList();
            foreach (var item in tabsPathsList)
            {
                if (item.Tag.ToString() == tag)
                    item.Fill = Application.Current.Resources["SecondColor"] as SolidColorBrush;
                else
                    item.Fill = Application.Current.Resources["White"] as SolidColorBrush;
            }
           

            /*
            grid_home.Visibility = Visibility.Hidden;
            grid_catalog.Visibility = Visibility.Hidden;
            grid_store.Visibility = Visibility.Hidden;
            grid_purchase.Visibility = Visibility.Hidden;
            grid_kitchin.Visibility = Visibility.Hidden;
            grid_sales.Visibility = Visibility.Hidden;
            grid_charts.Visibility = Visibility.Hidden;
            grid_data.Visibility = Visibility.Hidden;
            grid_settings.Visibility = Visibility.Hidden;
            grid_alerts.Visibility = Visibility.Hidden;
            grid_account.Visibility = Visibility.Hidden;
            */
        }
        #endregion
        #region   secondLevel
        void BuildObjectsDesign(List<Object> objectsChildren)
        {
            grid_secondLevel.Children.Clear();

            Border border;
            for (int i = 0; i < 3; i++)
            {
                border = new Border();
                Grid.SetColumn(border, (i*2)+1);
                Grid.SetRowSpan(border, 3);
                border.BorderBrush = Application.Current.Resources["veryLightGrey"] as SolidColorBrush;
                border.BorderThickness = new Thickness(1);
                border.Margin = new Thickness(5, 10, 5, 10);
                grid_secondLevel.Children.Add(border);
            }

            int count = 0;
            int div, mod;
            foreach (var item in objectsChildren)
            {
                
                #region button
                Button mainButton = new Button();
                mainButton.Name = "btn_" + item.name + "object";
                mainButton.Tag = item.name;
                mainButton.Padding = new Thickness(0);
                mainButton.Margin = new Thickness(5,0, 5, 0);
                mainButton.BorderBrush = null;
                mainButton.Background = null;
                mainButton.Height = Double.NaN;
                mainButton.HorizontalContentAlignment =HorizontalAlignment.Stretch;
                div = count / 8;
                mod = count % 8; //remainder 
                Grid.SetRow(mainButton, div);
                Grid.SetColumn(mainButton, mod);
                mainButton.Click += btn_secondLevelClick;

                #region StackPanel Container
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;

                /////////////////////////////////////////////////////
                #region Path table
                Path path = new Path();
                path.Name = "path_"+ item.name + "object"; 
                path.Tag = item.name; 
                // if(count == 0)
                //    path.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
                //else
                    path.Fill = Application.Current.Resources["Grey"] as SolidColorBrush;
                path.Width = 25;
                path.Height = 25;
                path.FlowDirection = FlowDirection.LeftToRight;
                path.Margin = new Thickness(0, 0, 15, 0);
                path.Data = App.Current.Resources[item.icon] as Geometry;
                path.Stretch = Stretch.Fill;
                sp.Children.Add(path);
                #endregion
                #region   name
                var itemText = new TextBlock();
                itemText.Name = "txt_" + item.name + "object"; 
                itemText.Tag = item.name ; 
                // translate
                if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == item.name).FirstOrDefault().translate))
                    itemText.Text = AppSettings.resourcemanager.GetString(
                   FillCombo.objectsList.Where(x => x.name == item.name).FirstOrDefault().translate
                   );
                itemText.Margin = new Thickness(5, 5, 5, 2.5);
                itemText.VerticalAlignment = VerticalAlignment.Bottom;
                itemText.FontSize = 14;
                //if(count == 0)
                //    itemText.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                //else
                    itemText.Foreground = Application.Current.Resources["Grey"] as SolidColorBrush;

                sp.Children.Add(itemText);
                #endregion

                mainButton.Content = sp;
                #endregion
                grid_secondLevel.Children.Add(mainButton);
                #endregion
                count+=2;
            }
        }
        private void btn_secondLevelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                
                //HelpClass.StartAwait(grid_main);
                Button button = sender as Button;

                ////////////////////////
                List<Object> list = Object.findChildrenList(button.Tag.ToString(), FillCombo.objectsList);
                list = list.Where(x => x.objectType == "basic" || x.objectType == "basicAlert").ToList();
                string s = "";
                foreach (var item in list)
                {
                    s += item.name + " \n";
                }


                //////////////
                if(list.Count == 0)
                {
                    //change colors
                List<Path> tabsPathsList = FindControls.FindVisualChildren<Path>(this)
                .Where(x => x.Name.Contains("object") && x.Tag != null).ToList();
                foreach (Path path in tabsPathsList)
                {
                    // do something with tb here
                    if (path.Tag ==  button.Tag )
                        path.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    else
                        path.Fill = Application.Current.Resources["Grey"] as SolidColorBrush;

                }
                List<TextBlock> tabsTextBlocksList = FindControls.FindVisualChildren<TextBlock>(this)
                .Where(x => x.Name.Contains("object") && x.Tag != null).ToList();
                foreach (TextBlock textBlock in tabsTextBlocksList)
                {
                    if (textBlock.Tag ==  button.Tag  )
                        textBlock.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    else
                        textBlock.Foreground = Application.Current.Resources["Grey"] as SolidColorBrush;
                }

                }
                else
                {
                    // Initialize buttons
                    BuildObjectsDesign(list);
                }



                
                ///////////////////////
                _parentObjectName = button.Tag.ToString();
                //MessageBox.Show(_parentObjectName);
                RefreshGroupObjectsView();
                initializationMainTrack(button.Tag.ToString());
                //HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                //HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
        #region groupObjects
        GroupObject groupObject = new GroupObject();
        IEnumerable<GroupObject> groupObjectsQuery;
        IEnumerable<GroupObject> groupObjects;
        async void RefreshGroupObjectsView()
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                if (groupObjects is null)
                    await RefreshGroupObjectList();
                groupObjectsQuery = groupObjects.Where(s => s.groupId == group.groupId
                && (s.objectType == "one" || s.objectType == "all" || s.objectType == "alert" )
                && s.parentObjectName == _parentObjectName);
                dg_permissions.ItemsSource = groupObjectsQuery;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
       
        /*
        private async void Btn_refreshGroupObjects_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    SectionData.StartAwait(grid_main);
                await RefreshGroupObjectList();
                Tb_search_TextChanged(null, null);
                
                    SectionData.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    SectionData.EndAwait(grid_main);
                SectionData.ExceptionMessage(ex, this);
            }
        }
        */
        async Task<IEnumerable<GroupObject>> RefreshGroupObjectList()
        {
            groupObjects = await groupObject.GetAll();
            return groupObjects;
        }
        #endregion
        private void Btn_usersList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(usersPermission, FillCombo.groupObjects, "one") )
                {
                    if (group.groupId > 0)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_usersList w = new wd_usersList();
                        w.groupId = group.groupId;

                        w.ShowDialog();

                        Window.GetWindow(this).Opacity = 1;
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

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update") )
                {
                    int s = 0;
                    foreach (var item in groupObjectsQuery)
                    {
                        s = await groupObject.Save(item);
                    }
                    if (!s.Equals(0))
                    {
                        //addObjects(int.Parse(s));
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                        Btn_clear_Click(null, null);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

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
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.S:
                        //handle S key
                        Btn_save_Click(btn_save, null);
                        break;

                }
            }
        }
        #region Main Path
        public void initializationMainTrack(string tag)
        {
            //sp_mainPath
            sp_mainPath.Children.Clear();
            List<Object> _listObjects = new List<Object>();
            _listObjects = objectModel.GetParents(FillCombo.objectsList, tag);
            int counter = 1;
            bool isLast = false;
            foreach (var item in _listObjects)
            {
                if (counter == _listObjects.Count)
                    isLast = true;
                else
                    isLast = false;
                sp_mainPath.Children.Add(initializationMainButton(item, isLast));
                counter++;
            }
        }
        Button initializationMainButton(Object _object, bool isLast)
        {
            Button button = new Button();
            button.Content = ">" + AppSettings.resourcemanager.GetString(_object.translate);
            button.Tag = _object.name;
            button.Click += MainButton_Click;
            button.Background = null;
            button.Margin = new Thickness(5, 0, 0, 0);
            button.BorderThickness = new Thickness(0);
            button.Padding = new Thickness(0);
            button.FontSize = 16;
            if (isLast)
                button.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
            else
                button.Foreground = Application.Current.Resources["Grey"] as SolidColorBrush;
            return button;
        }
        void MainButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            initializationMainTrack(button.Tag.ToString());
            /*
            loadPath(button.Tag.ToString());
            */
            List<Object> list = Object.findChildrenList(button.Tag.ToString(), FillCombo.objectsList);
            list = list.Where(x => x.objectType == "basic" || x.objectType == "basicAlert").ToList();
            if(list.Count > 0)
            BuildObjectsDesign(list);
            _parentObjectName = button.Tag.ToString();
            RefreshGroupObjectsView();
        }
        /*
        void loadPath(string tag)
        {
            grid_main.Children.Clear();
            switch (tag)
            {
                //2
                //case "home":
                //    grid_main.Children.Add(uc_home.Instance);
                //    break;
                default:
                    return;
            }
        }
        */
        #endregion
    }
}
