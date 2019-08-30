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
    public class RemoveCommandCommand : ICommand
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

        public AdminCommandsHelperViewModel VM { get; set; }

        public RemoveCommandCommand(AdminCommandsHelperViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            var command = parameter as CommandInfoModel;

            if (command.Id == VM.DisplayedCommand.Id)
            {
                VM.DisplayedCommand.Clear();
                VM.EditMode = false;
            }

            CommandInfoModel.DeleteOne(command);
            VM.UpdateCommands();
        }
    }
}
