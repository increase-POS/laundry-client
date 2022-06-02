using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace laundryApp.converters
{
    public class ConvertSolidColorBrushToSystemWindowsMedia : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush br =(SolidColorBrush) value;
            return System.Windows.Media.Color.FromArgb(br.Color.A, br.Color.R, br.Color.G, br.Color.B);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch
            {
                return value;
            }
        }
    }
}
