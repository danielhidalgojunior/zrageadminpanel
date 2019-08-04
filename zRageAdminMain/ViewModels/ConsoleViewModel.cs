using MongoDBHelper.Models;
using SourceQueryHandler;
using System;
using System.Collections.Generic;
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
        public List<CommandInfoModel> AllCommands { get; set; }
        public RichTextBox Console { get; set; }
        public SendCmdCommand SendCmdCommand { get; set; }
        public AttachConsoleCommand AttachConsoleCommand { get; set; }
        public ConsoleViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            AttachConsoleCommand = new AttachConsoleCommand(this);
            SendCmdCommand = new SendCmdCommand(this);

            LoadCommands();
        }

        public void LoadCommands()
        {
            AllCommands = new List<CommandInfoModel>
            {
                new CommandInfoModel { Command = "sm_ban", Description = "bans player" },
                new CommandInfoModel { Command = "sm_kick", Description = "kicks player" },
                new CommandInfoModel { Command = "sm_slap", Description = "slaps player" },
                new CommandInfoModel { Command = "sm_mute", Description = "mute player" }
            };
        }
    }
}
