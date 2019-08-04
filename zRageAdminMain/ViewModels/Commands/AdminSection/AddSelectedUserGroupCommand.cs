using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using zRageAdminMain.ViewModels.AdminSection;

namespace zRageAdminMain.ViewModels.Commands.AdminSection
{
    public class AddSelectedUserGroupCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public AdminUsersViewModel VM { get; set; }

        public AddSelectedUserGroupCommand(AdminUsersViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return parameter != null && VM.AvaliableGroups.Any();
        }

        public void Execute(object parameter)
        {
            var ug = parameter as UserGroupModel;
            VM.AvaliableGroups.Remove(ug);
            VM.DefinedGroups.Add(ug);
        }
    }
}
