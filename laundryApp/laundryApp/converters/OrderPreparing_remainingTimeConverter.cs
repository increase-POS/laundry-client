using laundryApp.Classes;
using laundryApp.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace laundryApp.converters
{
    class OrderPreparing_remainingTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

           
            decimal remainingTime = 0;
            OrderPreparing orderPreparing = (OrderPreparing) value ;
                DateTime dt;
                if (orderPreparing.preparingStatusDate == null)
                    dt = DateTime.Now;
                else
                    dt = orderPreparing.preparingStatusDate.Value;

            remainingTime =  OrderPreparing.calculateRemainingTime(dt,
                orderPreparing.preparingTime.Value, orderPreparing.status);


                return HelpClass.decimalToTime(remainingTime);

            }
            catch
            {
                return "";

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
