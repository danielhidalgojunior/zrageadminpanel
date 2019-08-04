using MongoDBHelper.Models;
using StaticResources;
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
    public class CreateNewOrEditExistingGroupCommand : ICommand
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

        public AdminPermissionGroupsViewModel VM { get; set; }

        public CreateNewOrEditExistingGroupCommand(AdminPermissionGroupsViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return true;

            if (parameter == null)
                return false;

            var model = (parameter as UserGroupModel);

            if (model.GroupCode == null || model.PermissionRank == 0)
                return false;

            return VM.EditMode ? true : !UserGroupModel.CodeOrIdExists(VM.DisplayedGroup);
        }

        public void Execute(object parameter)
        {
            VM.ApplyDefinedCommands();

            if (!(Variables.LoggedUser as UserModel).AreCommandsAvaliable(VM.DisplayedGroup.AvaliableCommands))
            {
                MessageBox.Show($"You don't have permission to add some of the commands to this group", "Permission denied", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (VM.EditMode)
            {
                UserGroupModel.UpdateOne(VM.DisplayedGroup);
            }
            else
            {
                UserGroupModel.InsertOne(VM.DisplayedGroup);
            }

            VM.EditMode = false;
            VM.UpdateAvaliableGroups();
            VM.DisplayedGroup.Clear();
            VM.CommandsStr = null;
        }
    }
}
