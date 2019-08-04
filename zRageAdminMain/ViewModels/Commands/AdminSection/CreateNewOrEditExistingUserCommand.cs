using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using zRageAdminMain.ViewModels.AdminSection;

namespace zRageAdminMain.ViewModels.Commands.AdminSection
{
    public class CreateNewOrEditExistingUserCommand : ICommand
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

        public CreateNewOrEditExistingUserCommand(AdminUsersViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return true;

            if (parameter == null)
                return false;

            if ((parameter as UserModel).Login == null)
                return false;

            return VM.EditMode ? true : !UserModel.LoginOrIdExists(VM.DisplayedUser);
        }

        public void Execute(object parameter)
        {
            VM.ApplyDefinedGroups();

            if (VM.EditMode)
            {
                UserModel.UpdateOne(VM.DisplayedUser);
            }
            else
            {
                UserModel.InsertOne(VM.DisplayedUser);
            }

            VM.EditMode = false;
            VM.UpdateUsers();
            VM.DisplayedUser.Clear();
            VM.DefinedGroups.Clear();
            VM.UpdateAvaliableGroups();
        }
    }
}
