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
    class preparingOrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case "Listed":    return AppSettings.resourcemanager.GetString("trListed");
                case "Preparing": return AppSettings.resourcemanager.GetString("trPreparing");
                case "Ready":     return AppSettings.resourcemanager.GetString("trReady");
                case "Collected": return AppSettings.resourcemanager.GetString("withDelivery");
                case "InTheWay": return AppSettings.resourcemanager.GetString("onTheWay");
                case "Done":      return AppSettings.resourcemanager.GetString("trDone"); // gived to customer
                default:          return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
