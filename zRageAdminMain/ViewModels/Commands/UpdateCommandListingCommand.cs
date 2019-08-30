using StaticResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class UpdateCommandListingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public ConsoleViewModel VM { get; set; }
        public UpdateCommandListingCommand(ConsoleViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return Variables.CommandAddedOrUpdated;
        }

        public void Execute(object parameter)
        {
            Variables.CommandAddedOrUpdated = false;
            Task.Run(VM.LoadCommands);
        }
    }
}
