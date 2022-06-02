using netoaster;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for wd_selectDiscount.xaml
    /// </summary>
    public partial class wd_selectDiscount : Window
    {
        public wd_selectDiscount()
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
            isOk = false;
            this.Close();
        }
        #region barcode
        public void FindControl(DependencyObject root, List<Control> controls)
        {
            controls.Clear();
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var control = current as Control;
                if (control != null && control.IsTabStop)
                {
                    controls.Add(control);
                }
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    if (child != null)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }
        private async void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_IsFocused)
                {
                    Control c = CheckActiveControl();
                    if (c == null)
                        cb_coupon.Focus();
                    _IsFocused = true;
                }

                HelpClass.StartAwait(grid_main);
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
                else if (e.Key >= Key.A && e.Key <= Key.Z)
                    digit = e.Key.ToString();
                else if (e.Key == Key.OemMinus)
                {
                    digit = "-";
                }
                _BarcodeStr += digit;
                _lastKeystroke = DateTime.Now;
                // process barcode

                if (e.Key.ToString() == "Return" && _BarcodeStr != "")
                {

                    if (_Sender != null)
                    {
                        TextBox tb = _Sender as TextBox;
                        if (tb != null)
                        {
                            if (tb.Name == "tb_discountValue" )// remove barcode from text box
                            {
                                string tbString = tb.Text;
                                string newStr = "";
                                int startIndex = tbString.IndexOf(_BarcodeStr);
                                if (startIndex != -1)
                                    newStr = tbString.Remove(startIndex, _BarcodeStr.Length);

                                tb.Text = newStr;
                            }
                        }
                    }
                    await dealWithBarcode(_BarcodeStr);
                    _BarcodeStr = "";
                    _IsFocused = false;
                    e.Handled = true;
                    cb_discountType.SelectedValue = _SelectedDisType;
                }
                _Sender = null;
                HelpClass.EndAwait(grid_main);
                //if (e.Key == Key.Return)
                //{
                //    Btn_select_Click(btn_select, null);
                //}
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        public Control CheckActiveControl()
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Control c = controls[i];
                if (c.IsFocused)
                {
                    return c;
                }
            }
            return null;
        }
        private async Task dealWithBarcode(string barcode)
        {
            int codeindex = barcode.IndexOf("-");
            string prefix = "";
            if (codeindex >= 0)
                prefix = barcode.Substring(0, codeindex);
            prefix = prefix.ToLower();
            barcode = barcode.ToLower();
            switch (prefix)
            {
                case "cop":// this barcode for coupon
                    {
                        // await fillCouponsList();
                        couponModel = coupons.ToList().Find(c => c.barcode.ToLower() == barcode);
                        var exist = selectedCopouns.Find(c => c.couponId == couponModel.cId);
                        if (couponModel != null && exist == null)
                        {
                            if ((couponModel.invMin != 0 && couponModel.invMax != 0 && _Sum >= couponModel.invMin && _Sum <= couponModel.invMax)
                                || (couponModel.invMax == 0 && _Sum >= couponModel.invMin)
                                || (couponModel.invMax != 0 && couponModel.invMin == 0 && _Sum <= couponModel.invMax))
                            {
                                CouponInvoice ci = new CouponInvoice();
                                ci.couponId = couponModel.cId;
                                ci.discountType = couponModel.discountType;
                                ci.discountValue = couponModel.discountValue;
                                ci.name = couponModel.name;
                                ci.forAgents = couponModel.forAgents;

                                lst_coupons.Items.Add(couponModel.name);
                                
                                selectedCopouns.Add(ci);
                            }

                            else if (couponModel.invMax != 0 && couponModel.invMin != 0)
                            {
                                if (_Sum < couponModel.invMin)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorMinInvToolTip"), animation: ToasterAnimation.FadeIn);
                                else if (_Sum > couponModel.invMax)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorMaxInvToolTip"), animation: ToasterAnimation.FadeIn);
                            }
                            else if (couponModel.invMax == 0)
                            {
                                if (_Sum < couponModel.invMin)
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorMinInvToolTip"), animation: ToasterAnimation.FadeIn);
                            }

                        }
                        else if (exist != null)
                        {
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorCouponExist"), animation: ToasterAnimation.FadeIn);
                        }
                        cb_coupon.SelectedIndex = -1;
                        //cb_coupon.SelectedItem = "";
                        //cb_coupon.Text = "";
                    }
                    break;
            }
        }
        #endregion
        public string userJob;

        public bool isOk { get; set; }
        public int memberShipId { get; set; }
        public decimal manualDiscount = 0;
        public string discountType = "";
        public decimal _Sum = 0;
        public List<CouponInvoice> selectedCopouns = new List<CouponInvoice>();

        Coupon couponModel = new Coupon();
        IEnumerable<Coupon> coupons;
        #region for barcode
        static private string _BarcodeStr = "";
        static private object _Sender;
        bool _IsFocused = false;
        static private string _SelectedDisType = "";
        DateTime _lastKeystroke = new DateTime(0);
        public List<Control> controls;
        #endregion
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
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
                controls = new List<Control>();
                //Walk through the VisualTree
                FindControl(this.grid_main, controls);
                FillCombo.fillDiscountType(cb_discountType);
                await fillCouponsList();
                fillInputs();
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
            txt_title.Text = AppSettings.resourcemanager.GetString("trDiscount");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_discountValue, AppSettings.resourcemanager.GetString("trDiscountValueHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_discountType, AppSettings.resourcemanager.GetString("trDiscountTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_coupon, AppSettings.resourcemanager.GetString("trCoupon") + "...");

            btn_clearCoupon.ToolTip = AppSettings.resourcemanager.GetString("trClear");

            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");


        }
        async Task fillCouponsList()
        {
            coupons = await couponModel.GetEffictiveByMemberShipID(memberShipId);

            foreach (Coupon c in coupons)
                c.name = c.name + "   #" + c.code;

            cb_coupon.DisplayMemberPath = "name";
            cb_coupon.SelectedValuePath = "cId";
            cb_coupon.ItemsSource = coupons;
        }
        private void fillInputs()
        {
            if (manualDiscount != 0)
            {
                tb_discountValue.Text = HelpClass.PercentageDecTostring( manualDiscount);
                cb_discountType.SelectedValue = discountType;
            }
            foreach (var coupon in selectedCopouns)
            {
                lst_coupons.Items.Add(coupon.name + "   #" + coupon.couponCode);
            }
        }
        private void Btn_select_Click(object sender, RoutedEventArgs e) 
        {
            isOk = true;
            if(tb_discountValue.Text != "")
                manualDiscount = decimal.Parse(tb_discountValue.Text);
            if (cb_discountType.SelectedIndex != -1)
                discountType = cb_discountType.SelectedValue.ToString();
            this.Close();
        }
        
        string input;
        decimal _decimal;
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


      
        private async void Cb_coupon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                string s = _BarcodeStr;
                if (cb_coupon.SelectedIndex != -1)
                {
                    couponModel = coupons.ToList().Find(c => c.cId == (int)cb_coupon.SelectedValue);
                    if (couponModel != null)
                    {
                        s = couponModel.barcode;
                        await dealWithBarcode(s);
                    }
                    cb_coupon.SelectedIndex = -1;
                    cb_coupon.SelectedItem = "";
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Btn_clearCoupon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedCopouns.Clear();
                lst_coupons.Items.Clear();
                cb_coupon.SelectedIndex = -1;
                cb_coupon.SelectedItem = "";
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }

        private void Cb_discountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                if (elapsed.TotalMilliseconds > 100 && cb_discountType.SelectedIndex != -1)
                {
                    _SelectedDisType = cb_discountType.SelectedValue.ToString();
                }
                else
                {
                    cb_discountType.SelectedValue = _SelectedDisType;
                }

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
    }
}
