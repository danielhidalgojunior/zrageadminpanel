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
    public class LoadUserForEditionCommand : ICommand
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

        public LoadUserForEditionCommand(AdminUsersViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var user = parameter as UserModel;
            VM.DisplayedUser = user.Clone();

            VM.DefinedGroups.Clear();
            foreach (var group in user.UserGroups)
            {
                VM.DefinedGroups.Add(group);
            }

            VM.UpdateAvaliableGroups();
            VM.EditMode = false;
            VM.EditMode = true;
        }
    }
}
