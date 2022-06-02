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
    class unlimitedEndDate : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null && values[1] != null)
            {
                string sType = (string)values[0];
                DateTime sDate = DateTime.Parse(values[1].ToString());

                if (sType == "o")
                    return AppSettings.resourcemanager.GetString("trUnlimited");
                else
                {
                    DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;
                    DateTime date;
                    if (sDate is DateTime)
                        date = (DateTime)sDate;
                    else return sDate;

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
            }
            else return "";
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
