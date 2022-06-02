using laundryApp;
using laundryApp.ApiClasses;
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
    /// Interaction logic for wd_usersList.xaml
    /// </summary>
    public partial class wd_usersList : Window
    {
        public wd_usersList()
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

        List<User> allUsersSource = new List<User>();
        List<User> selectedUsersSource = new List<User>();

        List<User> allUsers = new List<User>();
        List<User> selectedUsers = new List<User>();

        public int groupId = 0;

        User userModel = new User();
        User user = new User();

        Group groupModel = new Group();
        /// <summary>
        /// Selcted User if selectedUsers Have User At the beginning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                
                    HelpClass.StartAwait(grid_main);
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

                allUsersSource = await userModel.GetUsersActive();
                allUsersSource = allUsersSource.Where(x => x.isAdmin != true).ToList();
                selectedUsersSource = await groupModel.GetUsersByGroupId(groupId);
                //foreach (var u in selectedUsersSource)
                //{
                //    u.fullName = u.name + " " + u.lastname;
                //}
                allUsers.AddRange(allUsersSource);
                selectedUsers.AddRange(selectedUsersSource);

                //remove selected users from all users
                foreach (var su in selectedUsers)
                {
                    for (int i = 0; i < allUsers.Count; i++)
                        if (su.userId == allUsers[i].userId)
                        {
                            allUsers.Remove(allUsers[i]);
                        }
                }

                lst_allUsers.ItemsSource = allUsers;
                lst_allUsers.SelectedValuePath = "fullName";
                lst_allUsers.DisplayMemberPath = "userId";

                lst_selectedUsers.ItemsSource = selectedUsers;
                lst_selectedUsers.SelectedValuePath = "fullName";
                lst_selectedUsers.DisplayMemberPath = "userId";
                
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
            txt_user.Text = AppSettings.resourcemanager.GetString("trUsers");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            lst_allUsers.Columns[0].Header = AppSettings.resourcemanager.GetString("trUser");
            lst_selectedUsers.Columns[0].Header = AppSettings.resourcemanager.GetString("trUser");

            txt_users.Text = AppSettings.resourcemanager.GetString("trUsers");
            txt_selectedUsers.Text = AppSettings.resourcemanager.GetString("trSelectedUsers");

            tt_selectAllItem.Content = AppSettings.resourcemanager.GetString("trSelectAllItems");
            tt_unselectAllItem.Content = AppSettings.resourcemanager.GetString("trUnSelectAllItems");
            tt_selectItem.Content = AppSettings.resourcemanager.GetString("trSelectOneItem");
            tt_unselectItem.Content = AppSettings.resourcemanager.GetString("trUnSelectOneItem");

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
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                //get selcted ids
                List<int> userIds = new List<int>();
                foreach (var u in selectedUsers)
                    userIds.Add(u.userId);

                await groupModel.UpdateGroupIdInUsers(groupId, userIds, MainWindow.userLogin.userId);

                isActive = true;
                this.Close();
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isActive = false;
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Lst_allUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            try
            {
                
                    HelpClass.StartAwait(grid_main);

                Btn_selectedUser_Click(null, null);
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Lst_selectedUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                Btn_unSelectedUser_Click(null, null);
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_selectedAll_Click(object sender, RoutedEventArgs e)
        {//select all
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                int x = allUsers.Count;
                for (int i = 0; i < x; i++)
                {
                    lst_allUsers.SelectedIndex = 0;
                    Btn_selectedUser_Click(null, null);
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_selectedUser_Click(object sender, RoutedEventArgs e)
        {//select one
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                user = lst_allUsers.SelectedItem as User;
                if (user != null)
                {
                    allUsers.Remove(user);
                    selectedUsers.Add(user);

                    lst_allUsers.ItemsSource = allUsers;
                    lst_selectedUsers.ItemsSource = selectedUsers;

                    lst_allUsers.Items.Refresh();
                    lst_selectedUsers.Items.Refresh();
                }

                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }


        private void Btn_unSelectedUser_Click(object sender, RoutedEventArgs e)
        {//unselect one
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                user = lst_selectedUsers.SelectedItem as User;
                if (user != null)
                {
                    selectedUsers.Remove(user);

                    allUsers.Add(user);

                    lst_allUsers.ItemsSource = allUsers;
                    lst_selectedUsers.ItemsSource = selectedUsers;

                    lst_allUsers.Items.Refresh();
                    lst_selectedUsers.Items.Refresh();
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_unSelectedAll_Click(object sender, RoutedEventArgs e)
        {//unselect all
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                int x = selectedUsers.Count;
                for (int i = 0; i < x; i++)
                {
                    lst_selectedUsers.SelectedIndex = 0;
                    Btn_unSelectedUser_Click(null, null);
                }
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                lst_allUsers.ItemsSource = allUsers.Where(x => (x.fullName.ToLower().Contains(txb_search.Text.ToLower())) && x.isActive == 1);
                
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
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
            try
            {
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
