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
    class closingDescriptonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OpenClosOperatinModel s = value as OpenClosOperatinModel;
            string name = "";
            switch (s.side)
            {
                case "bnd": break;
                case "v": name = AppSettings.resourcemanager.GetString("trVendor"); break;
                case "c": name = AppSettings.resourcemanager.GetString("trCustomer"); break;
                case "u": name = AppSettings.resourcemanager.GetString("trUser"); break;
                case "s": name = AppSettings.resourcemanager.GetString("trSalary"); break;
                case "e": name = AppSettings.resourcemanager.GetString("trGeneralExpenses"); break;
                case "m":
                    if (s.transType == "d")
                        name = AppSettings.resourcemanager.GetString("trAdministrativeDeposit");
                    if (s.transType == "p")
                        name = AppSettings.resourcemanager.GetString("trAdministrativePull");
                    break;
                case "sh": name = AppSettings.resourcemanager.GetString("trShippingCompany"); break;
                default: break;
            }

            if (!string.IsNullOrEmpty(s.agentName))
                name = name + " " + s.agentName;
            else if (!string.IsNullOrEmpty(s.usersName) && !string.IsNullOrEmpty(s.usersLName))
                name = name + " " + s.usersName + " " + s.usersLName;
            else if (!string.IsNullOrEmpty(s.shippingCompanyName))
                name = name + " " + s.shippingCompanyName;
            else if ((s.side != "e") && (s.side != "m"))
                name = name + " " + AppSettings.resourcemanager.GetString("trUnKnown");

            if (s.transType.Equals("p"))
            {
                if ((s.side.Equals("bn")) || (s.side.Equals("p")))
                {
                    return AppSettings.resourcemanager.GetString("trPull") + " " +
                           AppSettings.resourcemanager.GetString("trFrom") + " " +
                           name;//receive
                }
                else if ((!s.side.Equals("bn")) || (!s.side.Equals("p")))
                {
                    return AppSettings.resourcemanager.GetString("trPayment") + " " +
                           AppSettings.resourcemanager.GetString("trTo") + " " +
                           name;//دفع
                }
                else return "";
            }
            else if (s.transType.Equals("d"))
            {
                if ((s.side.Equals("bn")) || (s.side.Equals("p")))
                {
                    return AppSettings.resourcemanager.GetString("trDeposit") + " " +
                           AppSettings.resourcemanager.GetString("trTo") + " " +
                           name;
                }
                else if ((!s.side.Equals("bn")) || (!s.side.Equals("p")))
                {
                    return AppSettings.resourcemanager.GetString("trReceiptOperation") + " " +
                           AppSettings.resourcemanager.GetString("trFrom") + " " +
                           name;//قبض
                }
                else return "";
            }
            else return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #region old
        //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        //{
        //    //string s = value as string;
        //    string transType = (string)values[0];
        //    string side = (string)values[1];

        //   if (transType.Equals("p"))
        //    {
        //        if ((side.Equals("bn")) || (side.Equals("p")))
        //        {
        //            return AppSettings.resourcemanager.GetString("trReceiptOperation")+" "+ 
        //                   AppSettings.resourcemanager.GetString("trFrom") + " " + 
        //                   side;//receive
        //        }
        //        else if ((!side.Equals("bn")) || (!side.Equals("p")))
        //        {
        //            return AppSettings.resourcemanager.GetString("trPayment")+" "+
        //                   AppSettings.resourcemanager.GetString("trTo") + " " + 
        //                   side;//دفع
        //        }
        //        else return "";
        //    }
        //    else if (transType.Equals("d"))
        //    {
        //        if ((side.Equals("bn")) || (side.Equals("p")))
        //        {
        //            return AppSettings.resourcemanager.GetString("trDeposit")+" "+
        //                   AppSettings.resourcemanager.GetString("trTo") + " " + 
        //                   side;
        //        }
        //        else if ((!side.Equals("bn")) || (!side.Equals("p")))
        //        {
        //            return AppSettings.resourcemanager.GetString("trReceive")+" "+
        //                   AppSettings.resourcemanager.GetString("trFrom") + " " + 
        //                   side;//قبض
        //        }
        //        else return "";
        //    }
        //    else return "";
        //}

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}
