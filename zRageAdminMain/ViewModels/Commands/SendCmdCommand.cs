using MongoDBHelper.Models;
using SourceQueryHandler;
using StaticResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class SendCmdCommand : ICommand
    {
        public ConsoleViewModel VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SendCmdCommand(ConsoleViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (parameter == null)
                return;

            var inputcontrol =  parameter as AutoCompleteBox;
            if (inputcontrol.Text == null)
                return;

            var command = inputcontrol.Text;

            var response = await ServerManager.SendCommand(command);

            VM.Console.AppendText(Environment.NewLine + response);
            VM.Console.ScrollToEnd();

            inputcontrol.Text = "";
        }
    }
}
