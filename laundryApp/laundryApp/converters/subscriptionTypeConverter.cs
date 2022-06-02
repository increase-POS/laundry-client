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
    class subscriptionTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            switch (value)
            {
                case "f": return AppSettings.resourcemanager.GetString("trFree");

                case "m": return AppSettings.resourcemanager.GetString("trMonthly");

                case "y": return AppSettings.resourcemanager.GetString("trYearly");

                case "o": return AppSettings.resourcemanager.GetString("trOnce");

                default: return AppSettings.resourcemanager.GetString("");

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
