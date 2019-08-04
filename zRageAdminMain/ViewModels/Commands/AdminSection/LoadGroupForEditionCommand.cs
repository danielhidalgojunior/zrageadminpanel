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
    public class LoadGroupForEditionCommand : ICommand
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

        public LoadGroupForEditionCommand(AdminPermissionGroupsViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var group = parameter as UserGroupModel;
            VM.DisplayedGroup = group.Clone();

            StringBuilder sb = new StringBuilder();
            foreach (var cmd in group.AvaliableCommands)
            {
                sb.Append($"{cmd}\n");
            }

            VM.CommandsStr = sb.ToString();
            VM.EditMode = false;
            VM.EditMode = true;
        }
    }
}
