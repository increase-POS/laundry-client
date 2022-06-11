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
    class isLimitedDateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                DateTime date;
                bool isLimited = false;
                if (values[1] is bool)
                    isLimited = (bool)values[1];

                if (isLimited)
                {
                    if (values[0] is DateTime)
                    {
                        date = (DateTime)values[0];
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
                    else return "";
                }
                else
                    return AppSettings.resourcemanager.GetString("trUnlimited");
            }
            else
            {
                return "";
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
