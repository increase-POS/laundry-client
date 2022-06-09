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

namespace laundryApp.View.sales.promotion.points
{
    /// <summary>
    /// Interaction logic for uc_customersPoints.xaml
    /// </summary>
    public partial class uc_customersPoints : UserControl
    {
        private static uc_customersPoints _instance;
        public static uc_customersPoints Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new uc_customersPoints();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public uc_customersPoints()
        {
            InitializeComponent();
        }
    }
}
