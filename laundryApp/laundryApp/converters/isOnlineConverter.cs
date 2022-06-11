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
    class isOnlineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";
            if (value.ToString().Equals("True"))
                result = AppSettings.resourcemanager.GetString("trOnlineType");
            else
                result = AppSettings.resourcemanager.GetString("trOfflineType");

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
