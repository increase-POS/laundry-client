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
    class toolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values[0] != null )
            {
                string isCoupon = (string)values[0];
                if (isCoupon == "c")
                {
                   return AppSettings.resourcemanager.GetString("trCoupons");
                }
                else if (isCoupon == "o")
                {
                    return AppSettings.resourcemanager.GetString("trOffers");
                }
                else if (isCoupon == "i")
                {
                    return AppSettings.resourcemanager.GetString("trInvoicesClasses");
                }
                else if (isCoupon == "cs")
                {
                    return AppSettings.resourcemanager.GetString("trCustomers");
                }
                else if (isCoupon == "pdf")
                {
                    return AppSettings.resourcemanager.GetString("trPdf");
                }
                else if (isCoupon == "print")
                {
                    return AppSettings.resourcemanager.GetString("trPrint");
                }
                else if (isCoupon == "excel")
                {
                    return AppSettings.resourcemanager.GetString("trExcel");
                }
                else if (isCoupon == "preview")
                {
                    return AppSettings.resourcemanager.GetString("trPreview");
                }
                else if (isCoupon == "allow")
                {
                    return AppSettings.resourcemanager.GetString("trPrintCount");
                }
                else
                    return "";

            }
            else return "";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
