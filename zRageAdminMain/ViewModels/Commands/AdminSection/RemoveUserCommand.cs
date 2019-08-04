using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using zRageAdminMain.ViewModels.AdminSection;

namespace zRageAdminMain.ViewModels.Commands.AdminSection
{
    public class RemoveUserCommand : ICommand
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

        public RemoveUserCommand(AdminUsersViewModel vm)
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

            if (MessageBox.Show($"Are you sure about erasing all data from the user {user.Name} of rank {user.HighestRank}?",
                "Deleting user",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Warning,
                 MessageBoxResult.No) != MessageBoxResult.Yes)
                return;

            if (user.Id == VM.DisplayedUser.Id)
            {
                VM.DisplayedUser.Clear();
                VM.DefinedGroups.Clear();
                VM.EditMode = false;
            }

            UserModel.DeleteOne(user);
            VM.UpdateUsers();

            VM.UpdateAvaliableGroups();
        }
    }
}
