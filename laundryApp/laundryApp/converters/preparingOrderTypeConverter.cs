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
    class preparingOrderTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "sd":
                case "s":
                    return AppSettings.resourcemanager.GetString("trDiningHall");
                case "ts": return AppSettings.resourcemanager.GetString("trTakeAway");
                case "ss": return AppSettings.resourcemanager.GetString("trSelfService");
                default: return "";
            }
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
