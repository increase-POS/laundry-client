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
    class depositorNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CashTransfer s = value as CashTransfer;
            string name = "";
            switch (s.side)
            {
                case "bnd":break;
                case "v": name = AppSettings.resourcemanager.GetString("trVendor"); break;
                case "c": name = AppSettings.resourcemanager.GetString("trCustomer"); break;
                case "u": name = AppSettings.resourcemanager.GetString("trUser"); break;
                case "s": name = AppSettings.resourcemanager.GetString("trSalary"); break;
                case "e": name = AppSettings.resourcemanager.GetString("trGeneralExpenses"); break;
                case "m":
                    if(s.transType=="d")
                        name = AppSettings.resourcemanager.GetString("trAdministrativeDeposit");
                    if(s.transType == "p")
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

            return name;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    

}
