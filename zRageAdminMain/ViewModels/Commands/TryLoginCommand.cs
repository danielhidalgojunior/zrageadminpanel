using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using zRageAdminMain.Models;
using zRageAdminMain.Views;

namespace zRageAdminMain.ViewModels.Commands
{
    public class TryLoginCommand : ICommand
    {
        public LoginViewModel VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public TryLoginCommand(LoginViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (VM.DoLogin())
            {
                var main = Application.Current.MainWindow;
                var win = new WinMainView(VM);
                win.Show();

                if (main is WinLoginView)
                    main.Close();

                main = win;
            }
            else
            {
                MessageBox.Show("Invalid password.", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
