using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace laundryApp.converters
{
    class discountTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string x="";
            if (value != null)
            {
                x = value.ToString();
            }

            if (x == "1")
            {
                value = "قيمة";
            }
            else if (x == "2")
            {
                value = "نسبة مئوية";
            }
            else return null;
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
