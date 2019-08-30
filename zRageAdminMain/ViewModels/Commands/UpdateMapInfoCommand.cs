using MapHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class UpdateMapInfoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MapsListingViewModel VM { get; set; }

        public UpdateMapInfoCommand(MapsListingViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Task.Run(() => VM.UpdateMapInfo(parameter as Map));
        }
    }
}
