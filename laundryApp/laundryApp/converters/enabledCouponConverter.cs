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
    class enabledCouponConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Coupon c = value as Coupon;
            string state = "";

            if ((c.isActive == 1) && ((c.endDate > DateTime.Now)||(c.endDate == null)) && ((c.quantity == 0) || (c.quantity > 0 && c.remainQ != 0)))
                state = AppSettings.resourcemanager.GetString("trValid");
            else
                state = AppSettings.resourcemanager.GetString("trExpired");

            return state;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
