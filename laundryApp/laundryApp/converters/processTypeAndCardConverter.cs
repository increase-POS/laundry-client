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
    class processTypeAndCardConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string pType = (string)values[0];
            string cName = (string)values[1];

            switch (pType)
            {
                case "cash": return AppSettings.resourcemanager.GetString("trCash");
                //break;
                case "doc": return AppSettings.resourcemanager.GetString("trDocument");
                //break;
                case "cheque": return AppSettings.resourcemanager.GetString("trCheque");
                //break;
                case "balance": return AppSettings.resourcemanager.GetString("trCredit");
                //break;
                case "card": return cName;
                //break;
                //case "inv": return AppSettings.resourcemanager.GetString("trInv");
                case "inv": return "-";
                case "multiple": return AppSettings.resourcemanager.GetString("trMultiplePayment");

                //break;
                default: return pType;
                    //break;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
