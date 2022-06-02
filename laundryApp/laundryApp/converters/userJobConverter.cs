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
     class userJobConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string s = value.ToString();
                if (FillCombo.UserJobList is null)
                    FillCombo.RefreshUserJobs();
                keyValueString keyValueString = FillCombo.UserJobList.Where(x => x.key == s).FirstOrDefault();

                return keyValueString.value;
            }
            else return "";
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
