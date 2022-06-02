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
    class invoicePayStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            switch (s)
            {
                case "payed": return AppSettings.resourcemanager.GetString("trPaid_");
                
                case "unpayed": return AppSettings.resourcemanager.GetString("trCredit");

                case "partpayed": return AppSettings.resourcemanager.GetString("trPartialPay");

                default: return "";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
