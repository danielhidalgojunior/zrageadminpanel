using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace zRageAdminMain.ViewModels.Converters
{
    public class TimeLeftToStringOrMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tl = (string)value;

            try
            {
                if (tl.ToList().Where(x => char.IsNumber(x)).Any())
                    return tl;
                else
                {
                    return "Last round";
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
