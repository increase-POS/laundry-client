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
    public class dateFrameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            

            DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;
            DateTime date;
            if (value is DateTime)
                date = (DateTime)value;
            else return value;

            switch (AppSettings.dateFormat)
            {
                case "ShortDatePattern":
                    return date.ToString(@"dd/MM/yyyy");
                case "LongDatePattern":
                    return date.ToString(@"dddd, MMMM d, yyyy");
                case "MonthDayPattern":
                    return date.ToString(@"MMMM dd");
                case "YearMonthPattern":
                    return date.ToString(@"MMMM yyyy");
                default:
                    return date.ToString(@"dd/MM/yyyy");
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
