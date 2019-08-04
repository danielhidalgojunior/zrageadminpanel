using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using zRageAdminMain.Models;

namespace zRageAdminMain.ViewModels.Commands
{
    public class GetFastLoginDataCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public LoginViewModel VM { get; set; }

        public GetFastLoginDataCommand(LoginViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            //return false;
            return File.Exists("Config/fastlogin.json");
        }

        public void Execute(object parameter)
        {
            if (!File.Exists("Config/fastlogin.json"))
                return;

            var jsontext = File.ReadAllText("Config/fastlogin.json");

            try
            {
                var login = JsonConvert.DeserializeObject<Login>(jsontext);
                VM.Login = login;
            }
            catch (Exception) { }
            
            if (VM.Login != null)
                VM.TryLoginCommand.Execute(null);
        }
    }
}
