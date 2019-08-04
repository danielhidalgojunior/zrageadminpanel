using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using zRageAdminMain.ViewModels.AdminSection;

namespace zRageAdminMain.ViewModels.Converters.AdminSection
{
    public class BoolToStringModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return "Invalid parameter";

            dynamic vm = parameter;

            if (vm is AdminUsersViewModel)
                return (bool)value ? $"Editing user <{vm.DisplayedUser.Name}> of id: {vm.DisplayedUser.Id}" : "Insert mode";
            else
                return (bool)value ? $"Editing group <{vm.DisplayedGroup.GroupName}> of id: {vm.DisplayedGroup.Id}" : "Insert mode";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
