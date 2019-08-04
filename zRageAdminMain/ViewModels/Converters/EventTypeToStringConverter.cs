using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ToastNoticesManager;

namespace zRageAdminMain.ViewModels.Converters
{
    public class EventTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value as EventType?)
            {
                case EventType.ToastWarning: return "Warning Message";
                case EventType.CommandToServer: return "Command";
                default: return "Invalid";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NoticeType type;
            if (Enum.TryParse((string)value, out type))
            {
                return type;
            }
            else
                throw new Exception("Invalid type");
        }
    }
}
