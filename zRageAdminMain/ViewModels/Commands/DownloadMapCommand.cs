using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class DownloadMapCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public NextMapViewModel VM { get; set; }
        public DownloadMapCommand(NextMapViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
        }
    }
}
