using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace laundryApp.converters
{
    class agentTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            switch (value)
            {
                case "v": return AppSettings.resourcemanager.GetString("trVendor");
                //break;
                case "c": return AppSettings.resourcemanager.GetString("trCustomer");
                //break;
                case "": return "-";
                default: return "";
                    //break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
