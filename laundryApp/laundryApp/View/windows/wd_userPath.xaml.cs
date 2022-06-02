using netoaster;
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
using Group = laundryApp.Classes.Group;
using Object = laundryApp.Classes.Object;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_userPath.xaml
    /// </summary>
    public partial class wd_userPath : Window
    {
        public wd_userPath()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        Classes.Object _object = new Classes.Object();
        IEnumerable<Classes.Object> objects = new List<Classes.Object>();
        IEnumerable<Classes.Object> firstLevel;
        IEnumerable<Classes.Object> secondLevel;
        List<Classes.Object> newlist = new List<Classes.Object>();
        List<Classes.Object> newlist2 = new List<Classes.Object>();
        BrushConverter bc = new BrushConverter();
        UserSetValues userSetValuesModel = new UserSetValues();
        //UserSetValues firstUserSetValue, secondUserSetValue;
        UserSetValues defaulPathUserSetValue;
        SetValues setValuesModel = new SetValues();
        List<SetValues> pathLst = new List<SetValues>();
        //int firstId = 0, secondId = 0;
        int defaulPathId;
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        string _parentObjectName = "";
        Object rootObject = new Object();
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                
                    HelpClass.StartAwait(grid_main);

                #region translate

                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;

                translate();
                #endregion
                await RefreshObjects();

                // fillFirstLevel();



                try
                {
                    rootObject = FillCombo.objectsList.Where(x => x.name == "root").FirstOrDefault();
                    rootObject.translate = "trAll";


                    List<Object> list = Object.findChildrenList("root", FillCombo.objectsList);
                    //list = list.Where(x => x.objectType == "basic" || x.objectType == "basicAlert").ToList();
                    list = list.Where(x => x.objectType == "basic" && x.name != "dashboard").ToList();
                     list = list.Where(x => FillCombo.groupObject.HasPermission(x.name, FillCombo.groupObjects) || HelpClass.isAdminPermision()).ToList();
                   

                    BuildObjectsDesign(list);
                    //initializationMainTrack(button.Tag.ToString());
                    //_parentObjectName = button.Tag.ToString();
                    //RefreshGroupObjectsView();
                }
                catch (Exception ex)
                {
                    HelpClass.ExceptionMessage(ex, this);
                }

                await getUserPath();

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        #region   secondLevel
        void BuildObjectsDesign(List<Object> objectsChildren)
        {
            
            grid_secondLevel.Children.Clear();

            Border border;
            for (int i = 0; i < 3; i++)
            {
                border = new Border();
                Grid.SetColumn(border, (i * 2) + 1);
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
                mainButton.Margin = new Thickness(5, 0, 5, 0);
                mainButton.BorderBrush = null;
                mainButton.Background = null;
                mainButton.Height = Double.NaN;
                mainButton.HorizontalContentAlignment = HorizontalAlignment.Stretch;
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
                path.Name = "path_" + item.name + "object";
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
                itemText.Tag = item.name;
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
                count += 2;
            }
        }
        private async void btn_secondLevelClick(object sender, RoutedEventArgs e)
        {
            try
            {

                //HelpClass.StartAwait(grid_main);
                Button button = sender as Button;

                ////////////////////////
                List<Object> list = Object.findChildrenList(button.Tag.ToString(), FillCombo.objectsList);
                //list = list.Where(x => x.objectType == "basic" || x.objectType == "basicAlert").ToList();
                list = list.Where(x => x.objectType == "basic" && x.name != "dashboard").ToList();
                // filter: have permission;
                    list = list.Where(x => FillCombo.groupObject.HasPermission(x.name, FillCombo.groupObjects) || HelpClass.isAdminPermision()).ToList();

                //string s = "";
                //foreach (var item in list)
                //{
                //    s += item.name + " \n";
                //}


                //////////////
                if (list.Count == 0)
                {
                    await Task.Delay(0100);
                    //change colors
                    List<Path> tabsPathsList = FindControls.FindVisualChildren<Path>(this)
                    .Where(x => x.Name.Contains("object") && x.Tag != null).ToList();

                    foreach (Path path in tabsPathsList)
                    {
                        // do something with tb here
                        if (path.Tag == button.Tag)
                            path.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;
                        else
                            path.Fill = Application.Current.Resources["Grey"] as SolidColorBrush;

                    }
                    List<TextBlock> tabsTextBlocksList = FindControls.FindVisualChildren<TextBlock>(this)
                    .Where(x => x.Name.Contains("object") && x.Tag != null).ToList();
                    foreach (TextBlock textBlock in tabsTextBlocksList)
                    {
                        if (textBlock.Tag == button.Tag)
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
                //RefreshGroupObjectsView();
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
        #region Main Path
        public void initializationMainTrack(string tag)
        {
           

            //sp_mainPath
            sp_mainPath.Children.Clear();

            // add Root
            if(tag != "root")
            sp_mainPath.Children.Add(initializationMainButton(rootObject, false));

            List<Object> _listObjects = new List<Object>();
            _listObjects = FillCombo.objectModel.GetParents(FillCombo.objectsList, tag);
           
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
          
            List<Object> list = Object.findChildrenList(button.Tag.ToString(), FillCombo.objectsList);
            //list = list.Where(x => x.objectType == "basic" || x.objectType == "basicAlert").ToList();
            list = list.Where(x => x.objectType == "basic" && x.name != "dashboard").ToList();
            // filter: have permission;
            list = list.Where(x => FillCombo.groupObject.HasPermission(x.name, FillCombo.groupObjects) || HelpClass.isAdminPermision()).ToList();

            if (list.Count > 0)
                BuildObjectsDesign(list);
            _parentObjectName = button.Tag.ToString();
            //RefreshGroupObjectsView();
        }
        
        #endregion

        async Task RefreshObjects()
        {
            if (FillCombo.objectsList is null)
               await FillCombo.RefreshObjects();
           var objectsLst =  FillCombo.objectsList.ToList();
            //objectsLst = objectsLst.Where(x => x.objectType == "basic" || x.objectType == "basicAlert").ToList();
            objectsLst = objectsLst.Where(x => x.objectType == "basic" && x.name != "dashboard").ToList();

            //objectsLst = objectsLst.Where(x => x.name != "storageStatistic" && x.name != "usersReports" 
            //&& x.name != "purchaseStatistic" && x.name != "accountsStatistic"
            //&& x.name != "medals" && x.name != "membership"&& x.name != "subscriptions").ToList();
            if (!HelpClass.isAdminPermision())
            {
                var list = new List<Classes.Object>();
                foreach (var obj in objectsLst)
                {
                    if (FillCombo.groupObject.HasPermission(obj.name, FillCombo.groupObjects))
                    {
                        list.Add(obj);
                    }
                }
                objects = list;
            }
            else
                objects = objectsLst;

        }
        private async Task getUserPath()
        {
            #region get user path
            UserSetValues uSetValueModel = new UserSetValues();
            List<UserSetValues> lst = await uSetValueModel.GetAll();

            SetValues setValueModel = new SetValues();

            List<SetValues> setVLst = await setValueModel.GetBySetName("user_path");
            //firstId  = setVLst[0].valId;
            //secondId = setVLst[1].valId;
            defaulPathId = setVLst[0].valId;
            //string firstPath = "" , secondPath = "";
            try
            {
                //firstUserSetValue = lst.Where(u => u.valId == firstId && u.userId == MainWindow.userID).FirstOrDefault();
                defaulPathUserSetValue = lst.Where(u => u.valId == defaulPathId && u.userId == MainWindow.userLogin.userId).FirstOrDefault();
                //secondUserSetValue = lst.Where(u => u.valId == secondId && u.userId == MainWindow.userID).FirstOrDefault();

                //initializationMainTrack(defaulPathUserSetValue.notes);

                List<Object> _listObjects = new List<Object>();
                _listObjects = FillCombo.objectModel.GetParents(FillCombo.objectsList, defaulPathUserSetValue.notes);
                foreach (var item in _listObjects)
                {
                    Button button = new Button();
                    button.Tag = item.name;
                    btn_secondLevelClick(button, null);
                }
                //foreach(var o in newlist)
                //{
                //    if (o.name.Equals(HelpClass.translate(firstUserSetValue.note) ))
                //    {
                //        cb_firstLevel.SelectedValue = o.objectId;
                //        break;
                //    }
                //}
                //foreach (var o in newlist2)
                //{
                //    if (o.name.Equals(HelpClass.translate(secondUserSetValue.note)))
                //    {
                //        cb_secondLevel.SelectedValue = o.objectId;
                //        break;
                //    }
                //}
                //cb_firstLevel.SelectedValue = cb_firstLevel.Items.IndexOf(firstUserSetValue.note);
            }
            catch {/* cb_firstLevel.SelectedIndex = -1;*/ }
           
            #endregion
        }

        //private  void fillFirstLevel()
        //{
        //    #region fill FirstLevel
        //    firstLevel = objects.Where(x => string.IsNullOrEmpty(x.parentObjectId.ToString()) && x.objectType == "basic");
        //    newlist = new List<Classes.Object>();

        //    foreach (var row in firstLevel)
        //    {
        //        Classes.Object newrow = new Classes.Object();
        //                newrow.objectId = row.objectId;
        //        newrow.name = HelpClass.translate(row.name);
        //        newrow.parentObjectId = row.parentObjectId;
        //        newlist.Add(newrow);
        //    }
        //    //  firstLevel = objects.Where(x => string.IsNullOrEmpty( x.parentObjectId.ToString()) && x.objectType == "basic" );
        //    cb_firstLevel.DisplayMemberPath = "name";
        //    cb_firstLevel.SelectedValuePath = "objectId";
        //    // cb_firstLevel.ItemsSource = firstLevel;
        //    cb_firstLevel.ItemsSource = newlist.OrderBy(x => x.name);

        //    #endregion


        //}
        //private  void Cb_firstLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        ComboBox combo = sender as ComboBox;
        //        secondLevel = objects.Where(x => x.parentObjectId == (int)cb_firstLevel.SelectedValue);

        //        if (secondLevel.Count() > 0)
        //        {
        //            cb_secondLevel.IsEnabled = true;
                    
        //            #region fill secondLevel

        //            newlist2 = new List<Classes.Object>();
        //            foreach (var row in secondLevel)
        //            {
        //                Classes.Object newrow = new Classes.Object();
        //                newrow.objectId = row.objectId;
        //                newrow.name = HelpClass.translate(row.name);
        //                newrow.parentObjectId = row.parentObjectId;
        //                newlist2.Add(newrow);
        //            }
        //            //secondLevel = objects.Where(x => x.parentObjectId == (int)cb_firstLevel.SelectedValue);
        //            cb_secondLevel.DisplayMemberPath = "name";
        //            cb_secondLevel.SelectedValuePath = "objectId";
        //            //cb_secondLevel.ItemsSource = secondLevel;
        //            cb_secondLevel.ItemsSource = newlist2.OrderBy(x => x.name);

        //            #endregion
        //        }
        //        else
        //            cb_secondLevel.IsEnabled = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trUserPath");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_firstLevel, AppSettings.resourcemanager.GetString("trFirstLevel"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_secondLevel, AppSettings.resourcemanager.GetString("trSecondLevel"));
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        /*
        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                validateEmpty(name, sender);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void validateEmpty(string name, object sender)
        {
            if (name == "ComboBox")
            {
                if ((sender as ComboBox).Name == "cb_firstLevel")
                    HelpClass.validateEmptyComboBox((ComboBox)sender, p_errorFirstLevel, tt_errorFirstLevel, "trFirstPath");
                else if ((sender as ComboBox).Name == "cb_secondLevel")
                    HelpClass.validateEmptyComboBox((ComboBox)sender, p_errorSecondLevel, tt_errorSecondLevel, "trSecondPath");
            }
        }
        */

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save

            //save this in AppSettings.defaultPath
            //MessageBox.Show(_parentObjectName);

            try
            {
                
                    HelpClass.StartAwait(grid_main);

                //#region validate
                //HelpClass.validateEmptyComboBox(cb_firstLevel , p_errorFirstLevel , tt_errorFirstLevel , "trFirstPath");
                //HelpClass.validateEmptyComboBox(cb_secondLevel, p_errorSecondLevel, tt_errorSecondLevel, "trSecondPath");
                //#endregion
                #region save
                //if ((!cb_firstLevel.Text.Equals("")) && (!cb_firstLevel.Text.Equals("")))
                //{
                    //string first = objects.Where(x => x.objectId == (int)cb_firstLevel.SelectedValue).FirstOrDefault().name.ToString();
                    //string second = objects.Where(x => x.objectId == (int)cb_secondLevel.SelectedValue).FirstOrDefault().name.ToString();
               
                    //save first path
                    if(defaulPathUserSetValue == null)
                        defaulPathUserSetValue = new UserSetValues();
                    if(_parentObjectName == "root")
                        _parentObjectName = "";

                    defaulPathUserSetValue.userId = MainWindow.userLogin.userId;
                    defaulPathUserSetValue.valId = defaulPathId;
                    defaulPathUserSetValue.notes = _parentObjectName;
                    defaulPathUserSetValue.createUserId = MainWindow.userLogin.userId;
                    defaulPathUserSetValue.updateUserId = MainWindow.userLogin.userId;
                    int res1 = await userSetValuesModel.Save(defaulPathUserSetValue);

                    ////save second path
                    //if(secondUserSetValue == null)
                    //    secondUserSetValue = new UserSetValues();

                    //secondUserSetValue.userId = MainWindow.userID;
                    //secondUserSetValue.valId = secondId;
                    //secondUserSetValue.note = second;
                    //secondUserSetValue.createUserId = MainWindow.userID;
                    //secondUserSetValue.updateUserId = MainWindow.userID;
                   //int res2 = await userSetValuesModel.Save(secondUserSetValue);
                   /*
                    if ((res1 > 0) && (res2 > 0))
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                        MainWindow.firstPath = first;
                        MainWindow.secondPath = second;
                       // MainWindow.first = res1;
                       //MainWindow.second = res2;
                        await Task.Delay(1500);
                        this.Close();
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                    */
                if (res1 > 0)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    AppSettings.defaultPath = _parentObjectName;
                    await Task.Delay(1500);
                    this.Close();
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                //}
                #endregion

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
            //+ $"Second: {objects.Where(x => x.objectId == (int)cb_secondLevel.SelectedValue).FirstOrDefault().name}");
        }

    }
}
