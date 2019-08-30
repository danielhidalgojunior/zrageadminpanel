using MongoDBHelper.Models;
using SourceQueryHandler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using zRageAdminMain.ViewModels.Commands;

namespace zRageAdminMain.ViewModels
{
    public class ConsoleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Command { get; set; }
        public ObservableCollection<CommandInfoModel> AllCommands { get; set; }
        public RichTextBox Console { get; set; }
        public SendCmdCommand SendCmdCommand { get; set; }
        public AttachConsoleCommand AttachConsoleCommand { get; set; }
        public UpdateCommandListingCommand UpdateCommandListingCommand { get; set; }
        public ConsoleViewModel()
        {
            AllCommands = new ObservableCollection<CommandInfoModel>();

            AttachConsoleCommand = new AttachConsoleCommand(this);
            SendCmdCommand = new SendCmdCommand(this);
            UpdateCommandListingCommand = new UpdateCommandListingCommand(this);

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;


            LoadCommands();
        }

        public void LoadCommands()
        {
            var commands = CommandInfoModel.GetAll();

            Application.Current.Dispatcher.Invoke(delegate
            {
                AllCommands.Clear();
            });
            
            foreach (var command in commands)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    AllCommands.Add(command);
                });
            }
        }
    }
}
