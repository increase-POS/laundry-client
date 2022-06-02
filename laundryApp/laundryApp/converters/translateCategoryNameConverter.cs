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
    class translateCategoryNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value != "")
            {

                Category category = value as Category;
                string s = category.name;

                switch (s)
                {
                    //sales
                    case "appetizers":
                        s = AppSettings.resourcemanager.GetString("trAppetizers");
                        break;
                    case "beverages":
                        s = AppSettings.resourcemanager.GetString("trBeverages");
                        break;
                    case "fastFood":
                        s = AppSettings.resourcemanager.GetString("trFastFood");
                        break;
                    case "mainCourses":
                        s = AppSettings.resourcemanager.GetString("trMainCourses");
                        break;
                    case "desserts":
                        s = AppSettings.resourcemanager.GetString("trDesserts");
                        break;
                    //purchase
                    case "RawMaterials":
                        s = AppSettings.resourcemanager.GetString("trRawMaterials");
                        break;
                    case "Vegetables":
                        s = AppSettings.resourcemanager.GetString("trVegetables");
                        break;
                    case "Meat":
                        s = AppSettings.resourcemanager.GetString("trMeat");
                        break;
                    case "Drinks":
                        s = AppSettings.resourcemanager.GetString("trDrinks");
                        break;
                }
                return s;
            }
            else return value;
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
