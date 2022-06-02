using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace laundryApp.converters
{
    class profitToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal d = (decimal)value;
            if (d < 0) return 0;
            else return 1;
            //else       return DependencyProperty.UnsetValue;
            //else return System.Drawing.ColorTranslator.FromHtml("#378500"); 

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
