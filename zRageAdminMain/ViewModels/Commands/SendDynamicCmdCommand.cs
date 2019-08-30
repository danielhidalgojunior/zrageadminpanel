using MapHelper.Models;
using SourceQueryHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class SendDynamicCmdCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (parameter == null)
                return;

            var map = (Map)(parameter as object[])[0];
            var cmd = (string)(parameter as object[])[1];

            string fullcmd = $"{cmd} {map.FullName}";

            MessageBoxResult result;
            result = MessageBox.Show("Are you sure about sending this command to the server? \n\n" + fullcmd, 
                "Caution", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var response = await ServerManager.SendCommand(fullcmd);

                MessageBox.Show("Command response:\n\n " + response, 
                    "Command sent", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);
            }
        }
    }
}
