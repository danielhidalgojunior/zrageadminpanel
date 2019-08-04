using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class AttachConsoleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public ConsoleViewModel VM { get; set; }

        public AttachConsoleCommand(ConsoleViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Console = parameter as RichTextBox;
        }
    }
}
