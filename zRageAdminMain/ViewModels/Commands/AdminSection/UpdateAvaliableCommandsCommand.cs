using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using zRageAdminMain.ViewModels.AdminSection;

namespace zRageAdminMain.ViewModels.Commands.AdminSection
{
    public class UpdateAvaliableCommandsCommand : ICommand
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

        public object VM { get; set; }

        public UpdateAvaliableCommandsCommand(object vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (VM is AdminCommandsHelperViewModel)
                (VM as AdminCommandsHelperViewModel).UpdateCommands();
            
            if (VM is AdminCommandsHelperViewModel)
                (VM as AdminCommandsHelperViewModel).UpdateCommands();
        }
    }
}
