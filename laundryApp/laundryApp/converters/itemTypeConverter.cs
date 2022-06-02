using laundryApp.Classes;
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
    class itemTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = "";
            switch(value)
            {
                case "n" : s = AppSettings.resourcemanager.GetString("trNormal");             break;
                case "d" : s = AppSettings.resourcemanager.GetString("trHaveExpirationDate"); break;
                case "sn": s = AppSettings.resourcemanager.GetString("trHaveSerialNumber");   break;
                case "sr": s = AppSettings.resourcemanager.GetString("trService");            break;
                case "p" : s = AppSettings.resourcemanager.GetString("trPackage");            break;
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
