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
    public class notificationsContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return MainWindow.GetUntilOrEmpty(value.ToString(), ":")
                          + " : " +
                         AppSettings.resourcemanager.GetString(value.ToString().Substring(value.ToString().LastIndexOf(':') + 1)); ;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
