using laundryApp.Classes;
using laundryApp.View.windows;
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
using static laundryApp.View.windows.wd_setupFirstPos;

namespace laundryApp.View.setup
{
    /// <summary>
    /// Interaction logic for uc_serverConfig.xaml
    /// </summary>
    public partial class uc_serverConfig : UserControl
    {
        public uc_serverConfig()
        {
            InitializeComponent();
        }

        private static uc_serverConfig _instance ;
        public static uc_serverConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new uc_serverConfig();
                return _instance;
            }
        }
        public string serverUri { get; set; }
        public string activationkey { get; set; }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //serverUri = activationkey = "";
        }
        private void Tb_validateEmptyTextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                validateEmpty(name, sender);

                TextBox textBox = sender as TextBox;
                if (textBox.Name.Equals("tb_serverUri"))
                {
                    serverUri = tb_serverUri.Text;

                }
                else if (textBox.Name.Equals("tb_activationkey"))
                {
                    activationkey = tb_activationkey.Text;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this);
            }
        }
        private void validateEmpty(string name, object sender)
        {
            if (name == "TextBox")
            {
                if ((sender as TextBox).Name == "tb_serverUri")
                   validateEmptyTextBox(tb_serverUri, p_errorServerUri, tt_errorServerUri, "trEmptyError");
                else if ((sender as TextBox).Name == "tb_activationkey")
                    validateEmptyTextBox(tb_activationkey, p_errorActivationkey, tt_errorActivationkey, "trEmptyError");
            }
            else if (name == "ComboBox")
            {
                //if ((sender as ComboBox).Name == "cb_paymentProcessType")
                //    HelpClass.validateEmptyComboBox((ComboBox)sender, p_errorpaymentProcessType, tt_errorpaymentProcessType, "trErrorEmptyPaymentTypeToolTip");
                //else if ((sender as ComboBox).Name == "cb_card")
                //    HelpClass.validateEmptyComboBox((ComboBox)sender, p_errorCard, tt_errorCard, "trEmptyCardTooltip");
            }
        }
        public static BrushConverter bc = new BrushConverter();
        public static bool validateEmptyTextBox(TextBox tb, Path p_error, ToolTip tt_error, string tr)
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                p_error.Visibility = Visibility.Visible;
                //tt_error.Content = wd_setupFirstPos.resourcemanager.GetString(tr);
                tt_error.Content = "This field cann't be empty";
                //tb.Background = (Brush)bc.ConvertFrom("#15FF0000");
                isValid = false;
            }
            else
            {
                //tb.Background = (Brush)bc.ConvertFrom("#f8f8f8");
                p_error.Visibility = Visibility.Collapsed;
            }
            return isValid;
        }
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

       
    }
}
