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
    class processTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            switch (value)
            {
                case "cash": return AppSettings.resourcemanager.GetString("trCash");
                    //break;
                case "doc": return AppSettings.resourcemanager.GetString("trDocument");
                    //break;
                case "cheque": return AppSettings.resourcemanager.GetString("trCheque");
                    //break;
                case "balance": return AppSettings.resourcemanager.GetString("trCredit");
                //break;
                case "card": return AppSettings.resourcemanager.GetString("trAnotherPaymentMethods");
                //break;
                case "inv": return AppSettings.resourcemanager.GetString("trInv");
                //break;
                default: return s;
                    //break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
