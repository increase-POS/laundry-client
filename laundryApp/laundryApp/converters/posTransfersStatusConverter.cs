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
    class posTransfersStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            byte isConfirm1 = (byte)values[0];
            byte isConfirm2 = (byte)values[1];

            if ((isConfirm1 == 1) && (isConfirm2 == 1))
                return AppSettings.resourcemanager.GetString("trConfirmed");
            else if ((isConfirm1 == 2) || (isConfirm2 == 2))
                return AppSettings.resourcemanager.GetString("trCanceled");
            else
                return AppSettings.resourcemanager.GetString("trWaiting");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
