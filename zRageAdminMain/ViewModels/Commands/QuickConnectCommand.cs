using SourceQueryHandler;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class QuickConnectCommand : ICommand
    {
        public ServerStatusViewModel VM { get; set; }
        public QuickConnectCommand(ServerStatusViewModel vm)
        {
            VM = vm;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Process.Start($"steam://connect/{(Variables.Server as ServerManager).Ip}:{(Variables.Server as ServerManager).Port}");
        }
    }
}
