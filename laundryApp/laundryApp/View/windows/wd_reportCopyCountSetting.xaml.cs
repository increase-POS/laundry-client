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
using System.Windows.Shapes;

namespace laundryApp.View.windows
{
    /// <summary>
    /// Interaction logic for wd_reportCopyCountSetting.xaml
    /// </summary>
    public partial class wd_reportCopyCountSetting : Window
    {

        public wd_reportCopyCountSetting()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
        }
        BrushConverter bc = new BrushConverter();

        //print

        string sale_copy_count;
        string pur_copy_count;
        string rep_copy_count;
        string directentry_copy_count;
        string kitchen_copy_count;
        SetValues setvalueModel = new SetValues();
        List<SetValues> printList = new List<SetValues>();

        public SetValues sale_copy_countrow = new SetValues();
        public SetValues pur_copy_countrow = new SetValues();
        public SetValues rep_copy_countrow = new SetValues();
        public SetValues directentry_copy_countrow = new SetValues();
        public SetValues kitchen_copy_countrow = new SetValues();

        async Task refreshWindow()
        {
            printList = await setvalueModel.GetBySetvalNote("print");

            sale_copy_countrow = printList.Where(X => X.name == "sale_copy_count").FirstOrDefault();
            sale_copy_count = sale_copy_countrow.value;
            pur_copy_countrow = printList.Where(X => X.name == "pur_copy_count").FirstOrDefault();
            pur_copy_count = pur_copy_countrow.value;
            rep_copy_countrow = printList.Where(X => X.name == "rep_copy_count").FirstOrDefault();
            rep_copy_count = rep_copy_countrow.value;
            directentry_copy_countrow = printList.Where(X => X.name == "directentry_copy_count").FirstOrDefault();
            directentry_copy_count = directentry_copy_countrow.value;
            kitchen_copy_countrow = printList.Where(X => X.name == "kitchen_copy_count").FirstOrDefault();
            kitchen_copy_count = kitchen_copy_countrow.value;

            tb_purCopyCount.Text = pur_copy_count;
            tb_saleCopyCount.Text = sale_copy_count;
            tb_repPrintCount.Text = rep_copy_count;
            tb_directEntryCount.Text = directentry_copy_count;
            tb_kitchenCount.Text = kitchen_copy_count;
        }

        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public static List<string> requiredControlList;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            
            try
            {
                
                    HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "purCopyCount", "saleCopyCount", "repPrintCount", "directEntryCount", "kitchenCount" };

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

                //
                await refreshWindow();


                
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
           
            txt_title.Text = AppSettings.resourcemanager.GetString("trCopyCount");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_purCopyCount, AppSettings.resourcemanager.GetString("trPurchasesCopyCount"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_saleCopyCount, AppSettings.resourcemanager.GetString("trSalesCopyCount"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_repPrintCount, AppSettings.resourcemanager.GetString("trReportsCopyCount"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_directEntryCount, AppSettings.resourcemanager.GetString("trDirectEntry"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_kitchenCount, AppSettings.resourcemanager.GetString("trKitchen"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
             

        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    //Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this); }
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    HelpClass.StartAwait(grid_main);


                int msg;
            sale_copy_countrow.value = (string)tb_saleCopyCount.Text;
            pur_copy_countrow.value = (string)tb_purCopyCount.Text;
            rep_copy_countrow.value = (string)tb_repPrintCount.Text;
            directentry_copy_countrow.value = (string)tb_directEntryCount.Text;
            kitchen_copy_countrow.value = (string)tb_kitchenCount.Text;
            if (int.Parse(sale_copy_countrow.value) <=0 || int.Parse(pur_copy_countrow.value) <= 0|| int.Parse(rep_copy_countrow.value)<=0)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMustBeMoreThanZero"), animation: ToasterAnimation.FadeIn);
            }
            else
            {
            
                msg = await setvalueModel.Save(sale_copy_countrow);
                msg = await setvalueModel.Save(pur_copy_countrow);
                msg = await setvalueModel.Save(rep_copy_countrow);
                msg = await setvalueModel.Save(kitchen_copy_countrow);
                await refreshWindow();
                await MainWindow.Getprintparameter();
                if (msg > 0)
                {
                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopSave"), animation: ToasterAnimation.FadeIn);
                    await Task.Delay(1500);
                    this.Close();
                }
                else
                {
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
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

        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    string name = sender.GetType().Name;
            //    validateEmpty(name, sender);
            //}
            //catch (Exception ex)
            //{
            //    HelpClass.ExceptionMessage(ex, this);
            //}

            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }

        }
         
        private void Tb_count_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                {
                    return;
                }



                if (textBox.Name == "tb_purCopyCount" || textBox.Name == "tb_saleCopyCount" || textBox.Name == "tb_repPrintCount" || textBox.Name == "tb_directEntryCount" || textBox.Name == "tb_kitchenCount")
                    HelpClass.InputJustNumber(ref textBox);

                //if (textBox.Name == "tb_purCopyCount")
                //{
                //    if (int.TryParse(textBox.Text, out _numPurCopyCount))
                //        numPurCopyCount = int.Parse(textBox.Text);
                //}
                //else if (textBox.Name == "tb_saleCopyCount")
                //{
                //    if (int.TryParse(textBox.Text, out _numSaleCopyCount))
                //        numSaleCopyCount = int.Parse(textBox.Text);
                //}
                //else if (textBox.Name == "tb_repPrintCount")
                //{
                //    if (int.TryParse(textBox.Text, out _numRepPrintCount))
                //        numRepPrintCount = int.Parse(textBox.Text);
                //}

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Tb_count_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        private void Tb_PreventSpaces(object sender, KeyEventArgs e)
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
        #region NumericCount

      

        private int _numValue_purCopyCount = 1;
        public int numValue_purCopyCount
        {
            get {
                if (int.TryParse(tb_purCopyCount.Text, out _numValue_purCopyCount))
                    _numValue_purCopyCount = int.Parse(tb_purCopyCount.Text);
                return _numValue_purCopyCount; }
            set
            {
                _numValue_purCopyCount = value;
                tb_purCopyCount.Text = value.ToString();
            }
        }


        private int _numValue_saleCopyCount = 1;
        public int numValue_saleCopyCount
        {
            get {
                if (int.TryParse(tb_saleCopyCount.Text, out _numValue_saleCopyCount))
                    _numValue_saleCopyCount = int.Parse(tb_saleCopyCount.Text);
                return _numValue_saleCopyCount; }
            set
            {
                _numValue_saleCopyCount = value;
                tb_saleCopyCount.Text = value.ToString();
            }
        }


        private int _numValue_repPrintCount = 1;
        public int numValue_repPrintCount
        {
            get {
                if (int.TryParse(tb_repPrintCount.Text, out _numValue_repPrintCount))
                    _numValue_repPrintCount = int.Parse(tb_repPrintCount.Text);
                return _numValue_repPrintCount; }
            set
            {
                _numValue_repPrintCount = value;
                tb_repPrintCount.Text = value.ToString();
            }
        }

        private int _numValue_directEntryCount = 1;
        public int numValue_directEntryCount
        {
            get
            {
                if (int.TryParse(tb_directEntryCount.Text, out _numValue_directEntryCount))
                    _numValue_directEntryCount = int.Parse(tb_directEntryCount.Text);
                return _numValue_directEntryCount;
            }
            set
            {
                _numValue_directEntryCount = value;
                tb_directEntryCount.Text = value.ToString();
            }
        }
        
        private int _numValue_kitchenCount = 1;
        public int numValue_kitchenCount
        {
            get
            {
                if (int.TryParse(tb_kitchenCount.Text, out _numValue_kitchenCount))
                    _numValue_kitchenCount = int.Parse(tb_kitchenCount.Text);
                return _numValue_kitchenCount;
            }
            set
            {
                _numValue_kitchenCount = value;
                tb_kitchenCount.Text = value.ToString();
            }
        }

        private void Btn_countDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Button button = sender as Button;

                if (button.Tag.ToString() == "purCopyCount" && numValue_purCopyCount > 0)
                    numValue_purCopyCount--;
                else if (button.Tag.ToString() == "saleCopyCount" && numValue_saleCopyCount > 0)
                    numValue_saleCopyCount--;
                else if (button.Tag.ToString() == "repPrintCount" && numValue_repPrintCount > 0)
                    numValue_repPrintCount--;
                else if (button.Tag.ToString() == "directEntryCount" && numValue_directEntryCount > 0)
                    numValue_directEntryCount--;
                else if (button.Tag.ToString() == "kitchenCount" && numValue_kitchenCount > 0)
                    numValue_kitchenCount--;

                HelpClass.validate(requiredControlList, this);

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
                if (button.Tag.ToString() == "purCopyCount")
                    numValue_purCopyCount++;
                else if (button.Tag.ToString() == "saleCopyCount")
                    numValue_saleCopyCount++;
                else if (button.Tag.ToString() == "repPrintCount")
                    numValue_repPrintCount++;
                else if (button.Tag.ToString() == "directEntryCount")
                    numValue_directEntryCount++;
                else if (button.Tag.ToString() == "kitchenCount")
                    numValue_kitchenCount++;

                HelpClass.validate(requiredControlList, this);

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        #endregion
    }
}
