using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class DeleteEventTriggerCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public ConditionalNoticesViewModel VM { get; set; }
        public DeleteEventTriggerCommand(ConditionalNoticesViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            var notice = parameter as EventTriggerModel;

            return VM.Events.Contains(notice);
        }

        public void Execute(object parameter)
        {
            var notice = parameter as EventTriggerModel;

            EventTriggerModel.DeleteOne(notice);

            VM.Events.Remove(notice);
        }
    }
}
