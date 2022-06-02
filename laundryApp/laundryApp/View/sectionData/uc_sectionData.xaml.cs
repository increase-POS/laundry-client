using laundryApp.Classes;
using laundryApp.View.sectionData.banksData;
using laundryApp.View.sectionData.branchesAndStores;
//using laundryApp.View.sectionData.hallDivide;
using laundryApp.View.sectionData.persons;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace laundryApp.View.sectionData
{
    /// <summary>
    /// Interaction logic for uc_sectionData.xaml
    /// </summary>
    public partial class uc_sectionData : UserControl
    {
        private static uc_sectionData _instance;
        public static uc_sectionData Instance
        {
            get
            {
                if(_instance is null)
                    _instance = new uc_sectionData();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_sectionData()
        {
            try
            {
                InitializeComponent();
            }
            catch
            { }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                await translate();
                #endregion
                permission();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        void permission()
        {
            bool loadWindow = false;
            int counter = 0;
            if (!loadWindow)
                if (!HelpClass.isAdminPermision())
                {
                    foreach (Border border in FindControls.FindVisualChildren<Border>(this))
                    {
                        if (border.Tag != null)
                            if (FillCombo.groupObject.HasPermission(border.Tag.ToString(), FillCombo.groupObjects))
                            {
                                border.Visibility = Visibility.Visible;
                                counter++;
                            }
                            else border.Visibility = Visibility.Collapsed;
                    }
                    if (counter == 1)
                    {
                        foreach (Button button in FindControls.FindVisualChildren<Button>(this))
                        {
                            if (button.Tag != null)
                                if (FillCombo.groupObject.HasPermission(button.Tag.ToString(), FillCombo.groupObjects))
                                {
                                    button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                    loadWindow = true;
                                }
                        }
                    }
                }
        }
        private async Task translate()
        {
            if (FillCombo.objectsList is null || FillCombo.objectsList.Count() == 0)
                await FillCombo.RefreshObjects();
            // Title
            if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate))
                txt_mainTitle.Text = AppSettings.resourcemanager.GetString(
               FillCombo.objectsList.Where(x => x.name == this.Tag.ToString()).FirstOrDefault().translate
               );
            // Icon
            List<Path> InfoPathsList = FindControls.FindVisualChildren<Path>(this)
                .Where(x => x.Name.Contains("Icon") && x.Tag != null).ToList();
            foreach (var item in InfoPathsList)
            {
                if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == item.Tag.ToString()).FirstOrDefault().icon))
                    item.Data = App.Current.Resources[
                FillCombo.objectsList.Where(x => x.name == item.Tag.ToString()).FirstOrDefault().icon
                   ] as Geometry;
            }
            // Info
            List<TextBlock> InfoTextBlocksList = FindControls.FindVisualChildren<TextBlock>(this)
                .Where(x => x.Name.Contains("Info") && x.Tag != null).ToList();
            if (InfoTextBlocksList.Count == 0)
            {
                await Task.Delay(0050);
                await translate();
            }
            foreach (var item in InfoTextBlocksList)
            {
                if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == item.Tag.ToString()).FirstOrDefault().translate))
                    item.Text = AppSettings.resourcemanager.GetString(
                   FillCombo.objectsList.Where(x => x.name == item.Tag.ToString()).FirstOrDefault().translate
                   );
            }
            // Hint
            List<TextBlock> HintTextBlocksList = FindControls.FindVisualChildren<TextBlock>(this)
                .Where(x => x.Name.Contains("Hint") && x.Tag != null).ToList();
            foreach (var item in HintTextBlocksList)
            {
                if (!string.IsNullOrWhiteSpace(FillCombo.objectsList.Where(x => x.name == item.Tag.ToString()).FirstOrDefault().translateHint))
                    item.Text = AppSettings.resourcemanager.GetString(
                   FillCombo.objectsList.Where(x => x.name == item.Tag.ToString()).FirstOrDefault().translateHint
                   );
            }

            // enterButton
            List<TextBlock> enterTextBlocksList = FindControls.FindVisualChildren<TextBlock>(this)
                .Where(x => x.Tag != null).ToList();
            enterTextBlocksList = enterTextBlocksList.Where(x => x.Tag.ToString().Contains("enterButton")).ToList();
            foreach (var item in enterTextBlocksList)
            {
                item.Text = AppSettings.resourcemanager.GetString("enter");
            }
        }
        //private void Btn_hallDivide_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MainWindow.mainWindow.grid_main.Children.Clear();
        //        MainWindow.mainWindow.grid_main.Children.Add(uc_hallDivide.Instance);

        //        Button button = sender as Button;
        //        MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        HelpClass.ExceptionMessage(ex, this);
        //    }
        //}
        private void Btn_residentialSectors_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                MainWindow.mainWindow.grid_main.Children.Add(uc_residentialSectors.Instance);

                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_persons_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                MainWindow.mainWindow.grid_main.Children.Add(uc_persons.Instance);

                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_branchesAndStores_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                MainWindow.mainWindow.grid_main.Children.Add(uc_branchesAndStores.Instance);

                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_banksData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                MainWindow.mainWindow.grid_main.Children.Add(uc_banksData.Instance);

                Button button = sender as Button;
                MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

      
    }
}
