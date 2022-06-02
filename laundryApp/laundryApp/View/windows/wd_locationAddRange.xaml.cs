using netoaster;
using laundryApp;
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
using System.Windows.Shapes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_locationAddRange.xaml
    /// </summary>
    public partial class wd_locationAddRange : Window
    {
        public wd_locationAddRange()
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
        public bool isOpend = false;
        List<Location> locations = new List<Location>();
        List<Location> AllLocations = new List<Location>();
        Location location = new Location();
        //BrushConverter bc = new BrushConverter();
        Regex regex = new Regex("^[a-zA-Z0-9_]*$");
        Regex regexNumber = new Regex("^[0-9]");
        Regex regexAlpha = new Regex("^[A-Za-z]+$");
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        bool validate(Location location = null)
        {
            bool isValid = true;
            //chk empty x
           validateEmptyTextBox(tb_fromX, p_error_fromX );
            //chk empty Y
           validateEmptyTextBox(tb_fromY, p_error_fromY );
            //chk empty Z
           validateEmptyTextBox(tb_fromZ, p_error_fromZ );

            //chk empty x
           validateEmptyTextBox(tb_toX, p_error_toX );
            //chk empty Y                           
           validateEmptyTextBox(tb_toY, p_error_toY );
            //chk empty Z                           
           validateEmptyTextBox(tb_toZ, p_error_toZ );

            /////////////////////////////////
            if (regexAlpha.IsMatch(tb_fromX.Text) && regexAlpha.IsMatch(tb_toX.Text) ||
                regexNumber.IsMatch(tb_fromX.Text) && regexNumber.IsMatch(tb_toX.Text))
            {
                if (regexAlpha.IsMatch(tb_fromX.Text))
                {
                    if (char.Parse(tb_fromX.Text.ToString().ToUpper()) > char.Parse(tb_toX.Text.ToString().ToUpper()))
                    {
                        SetError(tb_fromX, p_error_fromX,   "trValidLocationToBigger");
                        SetError(tb_toX, p_error_toX,   "trValidLocationToBigger");
                        isValid = false;
                    }
                }
                else
                {
                    if (int.Parse(tb_fromX.Text.ToString().ToUpper()) > int.Parse(tb_toX.Text.ToString().ToUpper()))
                    {
                        SetError(tb_fromX, p_error_fromX,   "trValidLocationToBigger");
                        SetError(tb_toX, p_error_toX,   "trValidLocationToBigger");
                        isValid = false;
                    }
                }
            }
            ///////
            if (regexAlpha.IsMatch(tb_fromY.Text) && regexAlpha.IsMatch(tb_toY.Text) ||
                regexNumber.IsMatch(tb_fromY.Text) && regexNumber.IsMatch(tb_toY.Text))
            {
                if (regexAlpha.IsMatch(tb_fromY.Text))
                {
                    if (char.Parse(tb_fromY.Text.ToString().ToUpper()) > char.Parse(tb_toY.Text.ToString().ToUpper()))
                    {
                        SetError(tb_fromY, p_error_fromY,  "trValidLocationToBigger");
                        SetError(tb_toY, p_error_toY,  "trValidLocationToBigger");
                        isValid = false;
                    }
                }
                else
                {
                    if (int.Parse(tb_fromY.Text.ToString().ToUpper()) > int.Parse(tb_toY.Text.ToString().ToUpper()))
                    {
                        SetError(tb_fromY, p_error_fromY,   "trValidLocationToBigger");
                        SetError(tb_toY, p_error_toY,   "trValidLocationToBigger");
                        isValid = false;
                    }
                }
            }
            ///////
            if (regexAlpha.IsMatch(tb_fromZ.Text) && regexAlpha.IsMatch(tb_toZ.Text) ||
                regexNumber.IsMatch(tb_fromZ.Text) && regexNumber.IsMatch(tb_toZ.Text))
            {
                if (regexAlpha.IsMatch(tb_fromZ.Text))
                {
                    if (char.Parse(tb_fromZ.Text.ToString().ToUpper()) > char.Parse(tb_toZ.Text.ToString().ToUpper()))
                    {
                        SetError(tb_fromZ, p_error_fromZ,   "trValidLocationToBigger");
                        SetError(tb_toZ, p_error_toZ,   "trValidLocationToBigger");
                        isValid = false;
                    }
                }
                else
                {
                    if (int.Parse(tb_fromZ.Text.ToString().ToUpper()) > int.Parse(tb_toZ.Text.ToString().ToUpper()))
                    {
                        SetError(tb_fromZ, p_error_fromZ,   "trValidLocationToBigger");
                        SetError(tb_toZ, p_error_toZ,   "trValidLocationToBigger");
                        isValid = false;
                    }
                }
            }
            /////////////////////////////////

            /////////////////////////////////
            if (regexAlpha.IsMatch(tb_fromX.Text) && !regexAlpha.IsMatch(tb_toX.Text) ||
                regexNumber.IsMatch(tb_fromX.Text) && !regexNumber.IsMatch(tb_toX.Text))
            {
                SetError(tb_fromX, p_error_fromX,  "trValidLocationMatch");
                SetError(tb_toX, p_error_toX,   "trValidLocationMatch");
                isValid = false;
            }
            if (regexAlpha.IsMatch(tb_fromY.Text) && !regexAlpha.IsMatch(tb_toY.Text) ||
                regexNumber.IsMatch(tb_fromY.Text) && !regexNumber.IsMatch(tb_toY.Text))
            {
                SetError(tb_fromY, p_error_fromY,  "trValidLocationMatch");
                SetError(tb_toY, p_error_toY,   "trValidLocationMatch");
                isValid = false;
            }
            if (regexAlpha.IsMatch(tb_fromZ.Text) && !regexAlpha.IsMatch(tb_toZ.Text) ||
                regexNumber.IsMatch(tb_fromZ.Text) && !regexNumber.IsMatch(tb_toZ.Text))
            {
                SetError(tb_fromZ, p_error_fromZ,   "trValidLocationMatch");
                SetError(tb_toZ, p_error_toZ,   "trValidLocationMatch");
                isValid = false;
            }
            /////////////////////////////////

            if ((tb_fromX.Text.Equals("")) || (tb_fromX.Text.Equals("")) || (tb_fromZ.Text.Equals("")) ||
                (tb_toX.Text.Equals("")) || (tb_toX.Text.Equals("")) || (tb_toZ.Text.Equals("")))
             isValid = false;

            return isValid;
        }
        
        void generateLocationListX(Location location)
        {
            #region x
            if (regexAlpha.IsMatch(tb_fromX.Text)  && !regexNumber.IsMatch(tb_fromX.Text))
            {
                for (char x = char.Parse(tb_fromX.Text.ToString().ToUpper());
                  x <= char.Parse(tb_toX.Text.ToString().ToUpper()); x++)

                {
                    location.x = x.ToString();
                    generateLocationListY(location);
                }
            }
            else
            {
                for (int x = int.Parse(tb_fromX.Text.ToString());
                 x <= int.Parse(tb_toX.Text.ToString()); x++)
                {
                    location.x = x.ToString();
                    generateLocationListY(location);
                }
            }
            #endregion  
        }
        void generateLocationListY(Location location)
        {
            #region y
            if (regexAlpha.IsMatch(tb_fromY.Text) &&  !regexNumber.IsMatch(tb_fromY.Text))
            {
                for (char y = char.Parse(tb_fromY.Text.ToString().ToUpper());
                  y <= char.Parse(tb_toY.Text.ToString().ToUpper()); y++)

                {
                    location.y = y.ToString();
                    generateLocationListZ(location);
                }
            }
            else
            {
                for (int y = int.Parse(tb_fromY.Text.ToString());
                  y <= int.Parse(tb_toY.Text.ToString()); y++)
                {
                    location.y = y.ToString();
                    generateLocationListZ(location);
                }
            }
            #endregion  
        }
        void generateLocationListZ(Location location)
        {
            #region z
            if (regexAlpha.IsMatch(tb_fromZ.Text) && !regexNumber.IsMatch(tb_fromZ.Text))
            {
                for (char z = char.Parse(tb_fromZ.Text.ToString().ToUpper());
                  z <= char.Parse(tb_toX.Text.ToString().ToUpper()); z++)

                {
                    location.z = z.ToString();
                    Location l = new Location();
                    l.x = location.x;
                    l.y = location.y;
                    l.z = location.z;
                    locations.Add(l);
                }
            }
            else
            {
                for (int z = int.Parse(tb_fromZ.Text.ToString());
                  z <= int.Parse(tb_toZ.Text.ToString()); z++)
                {
                    location.z = z.ToString();
                    Location l = new Location();
                    l.x = location.x;
                    l.y = location.y;
                    l.z = location.z;
                    locations.Add(l);
                }
            }

            #endregion z
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
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {//save
            try
            {
                
                    HelpClass.StartAwait(grid_locationRange);
                
                if (validate(location))
                {
                    int s = 0;
                    generateLocationListX(location);
                    foreach (var item in locations)
                    {
                        if (AllLocations.Where(x => x.name == item.name && x.branchId == MainWindow.branchLogin.branchId).Count() == 0)
                        {
                            item.createUserId = MainWindow.userLogin.userId;
                            item.updateUserId = MainWindow.userLogin.userId;
                            item.notes = "";
                            item.isActive = 1;
                            item.sectionId = null;
                            item.branchId = MainWindow.branchLogin.branchId;

                            s = await location.save(item);
                        }
                    }

                    if (!s.Equals("-1"))
                    {
                        Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                        Btn_clear_Click(null, null);
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                }

                
                    HelpClass.EndAwait(grid_locationRange);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_locationRange);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {//clear
            try
            {
                tb_fromX.Clear();
                tb_fromY.Clear();
                tb_fromZ.Clear();

                p_error_fromX.Visibility = Visibility.Collapsed;
                p_error_fromY.Visibility = Visibility.Collapsed;
                p_error_fromZ.Visibility = Visibility.Collapsed;

                tb_fromX.Background = (Brush)bc.ConvertFrom("#f8f8f8");
                tb_fromY.Background = (Brush)bc.ConvertFrom("#f8f8f8");
                tb_fromZ.Background = (Brush)bc.ConvertFrom("#f8f8f8");
                /////////////////////////////////////////////
                tb_toX.Clear();
                tb_toY.Clear();
                tb_toZ.Clear();

                p_error_toX.Visibility = Visibility.Collapsed;
                p_error_toY.Visibility = Visibility.Collapsed;
                p_error_toZ.Visibility = Visibility.Collapsed;

                tb_toX.Background = (Brush)bc.ConvertFrom("#f8f8f8");
                tb_toY.Background = (Brush)bc.ConvertFrom("#f8f8f8");
                tb_toZ.Background = (Brush)bc.ConvertFrom("#f8f8f8");
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex,this);
            }
        }
        private void validationControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }
        
        private void validationControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                bool isValid = false;

                if (regex.IsMatch(e.Text))
                {
                    if (regexNumber.IsMatch(e.Text) && !(regexAlpha.IsMatch((sender as TextBox).Text)))
                    {
                        e.Handled = false;
                        isValid = true;
                    }
                    else if (regexAlpha.IsMatch(e.Text) && (sender as TextBox).Text.Count() == 0 && !regexNumber.IsMatch((sender as TextBox).Text))
                    {
                        e.Handled = false;
                        isValid = true;
                    }
                    else if (regexAlpha.IsMatch(e.Text) && (sender as TextBox).Text.Count() == 1 && !regexNumber.IsMatch((sender as TextBox).Text))
                    {
                        (sender as TextBox).Text = "";
                        e.Handled = false;
                        isValid = true;
                    }
                }
                if (!isValid)
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validationControl_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as Control).Name == "tb_fromX")
                   validateEmptyTextBox(tb_fromX, p_error_fromX);
                else if ((sender as Control).Name == "tb_fromY")  
                   validateEmptyTextBox(tb_fromY, p_error_fromY);
                else if ((sender as Control).Name == "tb_fromZ")  
                   validateEmptyTextBox(tb_fromZ, p_error_fromZ);

                else if ((sender as Control).Name == "tb_toX")
                   validateEmptyTextBox(tb_toX, p_error_toX);
                else if ((sender as Control).Name == "tb_toY")
                   validateEmptyTextBox(tb_toY, p_error_toY);
                else if ((sender as Control).Name == "tb_toZ")
                   validateEmptyTextBox(tb_toZ, p_error_toZ);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validationTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((sender as Control).Name == "tb_fromX")
                   validateEmptyTextBox(tb_fromX, p_error_fromX );
                else if ((sender as Control).Name == "tb_fromY")
                   validateEmptyTextBox(tb_fromY, p_error_fromY );
                else if ((sender as Control).Name == "tb_fromZ")
                   validateEmptyTextBox(tb_fromZ, p_error_fromZ );
                ////////////////////////////////////
                else if ((sender as Control).Name == "tb_toX")
                   validateEmptyTextBox(tb_toX, p_error_toX );
                else if ((sender as Control).Name == "tb_toY")
                   validateEmptyTextBox(tb_toY, p_error_toY );
                else if ((sender as Control).Name == "tb_toZ")
                   validateEmptyTextBox(tb_toZ, p_error_toZ );
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                
                    HelpClass.StartAwait(grid_locationRange);

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_locationRange.FlowDirection = FlowDirection.LeftToRight; }
                else
                {
                    grid_locationRange.FlowDirection = FlowDirection.RightToLeft; }

                translate();
                #endregion

                AllLocations = await location.Get();

                
                    HelpClass.EndAwait(grid_locationRange);
            }
            catch (Exception ex)
            {
                
                    HelpClass.EndAwait(grid_locationRange);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trLocation");
            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_fromX, AppSettings.resourcemanager.GetString("trFromXHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_fromY, AppSettings.resourcemanager.GetString("trFromYHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_fromZ, AppSettings.resourcemanager.GetString("trFromZHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_toX, AppSettings.resourcemanager.GetString("trToXHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_toY, AppSettings.resourcemanager.GetString("trToYHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_toZ, AppSettings.resourcemanager.GetString("trToZHint"));

            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }



        public static BrushConverter bc = new BrushConverter();
        public static void SetError(Control c, Path p_error,  string tr)
        {
            p_error.Visibility = Visibility.Visible;
            #region Tooltip
            ToolTip toolTip = new ToolTip();
            toolTip.Content = AppSettings.resourcemanager.GetString(tr);
            toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
            p_error.ToolTip = toolTip;
            #endregion
        }
        public static bool validateEmptyTextBox(TextBox tb, Path p_error)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                p_error.Visibility = Visibility.Visible;
                #region Tooltip
                ToolTip toolTip = new ToolTip();
                toolTip.Content = AppSettings.resourcemanager.GetString("trIsRequired");
                toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
                p_error.ToolTip = toolTip;
                #endregion
                isValid = false;
            }
            else
            {
                p_error.Visibility = Visibility.Collapsed;
            }
            return isValid;
        }

    }
}
