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
    public class EventTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value as EventType?)
            {
                case EventType.ToastWarning: return Brushes.LightYellow;
                case EventType.CommandToServer: return Brushes.LightBlue;
                default: return Brushes.White;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
