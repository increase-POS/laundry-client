using laundryApp.Classes;
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

namespace laundryApp.View.catalog.salesItems
{
    /// <summary>
    /// Interaction logic for uc_servicePricing.xaml
    /// </summary>
    public partial class uc_servicePricing : UserControl
    {
        public uc_servicePricing()
        {
            InitializeComponent();
        }
        private static uc_servicePricing _instance;
        public static uc_servicePricing Instance
        {
            get
            {
                //if (_instance == null)
                if (_instance is null)
                    _instance = new uc_servicePricing();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        Category category = new Category();
        public static string categoryName;
    }
}
