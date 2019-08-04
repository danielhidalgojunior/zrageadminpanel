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
    public class RemoveGroupCommand : ICommand
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

        public RemoveGroupCommand(AdminPermissionGroupsViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            var group = parameter as UserGroupModel;

            if (MessageBox.Show($"Are you sure about erasing all data from the group {group.GroupName} <{group.GroupCode}> of rank {group.PermissionRank}?",
                "Deleting group",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Warning,
                 MessageBoxResult.No) != MessageBoxResult.Yes)
                return;

            var usersusing = UserModel.GetAll(group);
            if (usersusing.Any())
            {
                if (MessageBox.Show($"This group is being used by {usersusing.Count()} user{(usersusing.Count() > 0 ? "s" : "")}. Are you sure about deleting it and removing from every user?",
                "Group is in use",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Warning,
                 MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    foreach (var user in usersusing)
                    {
                        user.RemoveGroupById(group.Id);
                    }
                }
            }

            if (group.Id == VM.DisplayedGroup.Id)
            {
                VM.DisplayedGroup.Clear();
                VM.CommandsStr = null;
                VM.EditMode = false;
            }

            UserGroupModel.DeleteOne(group);
            VM.UpdateAvaliableGroups();
        }
    }
}
