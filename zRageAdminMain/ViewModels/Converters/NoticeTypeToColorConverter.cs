using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ToastNoticesManager;

namespace zRageAdminMain.ViewModels.Converters
{
    public class NoticeTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (value as NoticeType?).ToString();
            var color = App.GetHashColor(type);

            return new SolidColorBrush(color);
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
