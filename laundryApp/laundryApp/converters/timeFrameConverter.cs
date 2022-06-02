using laundryApp;
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
    public class timeFrameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date;
            if (value is DateTime)
                date = (DateTime)value;
            else return value;

           
            switch (AppSettings.timeFormat)
            {
                case "ShortTimePattern":
                    return date.ToShortTimeString();
                case "LongTimePattern":
                    return date.ToLongTimeString();
                default:
                    return date.ToShortTimeString();
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
