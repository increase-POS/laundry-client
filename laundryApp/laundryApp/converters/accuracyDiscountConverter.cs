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
    class accuracyDiscountConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

            if (values != null && values[0] != null && values[1] != null)
            {
                string type = values[0].ToString();
                decimal value = (decimal)values[1];

                decimal num = decimal.Parse(value.ToString());
                string s = num.ToString();

                switch (AppSettings.accuracy)
                {
                    case "0":
                        s = string.Format("{0:F0}", num);
                        break;
                    case "1":
                        s = string.Format("{0:F1}", num);
                        break;
                    case "2":
                        s = string.Format("{0:F2}", num);
                        break;
                    case "3":
                        s = string.Format("{0:F3}", num);
                        break;
                    default:
                        s = string.Format("{0:F1}", num);
                        break;
                }

                string sdc = string.Format("{0:G29}", decimal.Parse(s));


                if (type == "2")
                    return sdc + "%";
                else
                    return s;

            }
            else return "";
            }
            catch
            {

                return values;
                //return "Hello";
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
