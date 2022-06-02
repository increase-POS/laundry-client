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
    class inventoryTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            switch (value)
            {
                case "a": return AppSettings.resourcemanager.GetString("trArchived");
                //break;
                case "n": return AppSettings.resourcemanager.GetString("trSaved");
                //break;
                case "d": return AppSettings.resourcemanager.GetString("trDraft");
                //break;
                default: return value;
                    //break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
