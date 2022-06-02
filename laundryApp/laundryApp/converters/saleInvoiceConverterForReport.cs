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
    class saleInvoiceConverterForReport : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                //مبيعات
                case "s":
                    value = AppSettings.resourcemanager.GetString("trDiningHallType");
                    break;
                //مسودة مبيعات
                case "sd":
                    value = AppSettings.resourcemanager.GetString("trDiningHallType")+" - "+ AppSettings.resourcemanager.GetString("trDraft");
                    break;
                // طلب خارجي
                case "ts":
                    value = AppSettings.resourcemanager.GetString("trTakeAway");
                    break;
                // مسودة طلب خارجي
                case "tsd":
                    value = AppSettings.resourcemanager.GetString("trTakeAway") + " - " + AppSettings.resourcemanager.GetString("trDraft"); ;
                    break;
                // خدمة ذاتية
                case "ss":
                    value = AppSettings.resourcemanager.GetString("trSelfService");
                    break;
                // خدمة ذاتية مسودة
                case "ssd":
                    value = AppSettings.resourcemanager.GetString("trSelfService") + " - " + AppSettings.resourcemanager.GetString("trDraft"); ;
                    break;
                default: break;
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
