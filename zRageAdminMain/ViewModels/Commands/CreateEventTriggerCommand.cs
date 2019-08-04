using MongoDBHelper.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class CreateEventTriggerCommand : ICommand
    {
        public ConditionalNoticesViewModel VM { get; set; }

        public CreateEventTriggerCommand(ConditionalNoticesViewModel vm)
        {
            VM = vm;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(object parameter)
        {
            if (string.IsNullOrEmpty(VM.Trigger.Value))
                return false;

            if (VM.Trigger.Period.Start >= VM.Trigger.Period.End)
                return false;

            if (!(Variables.LoggedUser as UserModel).IsCommandAvaliable(VM.Trigger.Value))
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            VM.CreateEventTrigger();
        }
    }
}
