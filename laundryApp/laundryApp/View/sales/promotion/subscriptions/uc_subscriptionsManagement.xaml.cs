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

namespace laundryApp.View.sales.promotion.subscriptions
{
    /// <summary>
    /// Interaction logic for uc_subscriptionsManagement.xaml
    /// </summary>
    public partial class uc_subscriptionsManagement : UserControl
    {
        private static uc_subscriptionsManagement _instance;
        public static uc_subscriptionsManagement Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_subscriptionsManagement();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_subscriptionsManagement()
        {
            InitializeComponent();
        }
    }
}
