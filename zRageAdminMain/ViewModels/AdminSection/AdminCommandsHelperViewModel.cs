using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using zRageAdminMain.ViewModels.Commands.AdminSection;

namespace zRageAdminMain.ViewModels.AdminSection
{
    public class AdminCommandsHelperViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<CommandInfoModel> Commands { get; set; }
        public CommandInfoModel DisplayedCommand { get; set; }
        public bool EditMode { get; set; }
        public CreateNewOrEditExistingCommandCommand CreateNewOrEditExistingCommandCommand { get; }
        public CancelAndClearCommandActionCommand CancelAndClearCommandActionCommand { get; }
        public RemoveCommandCommand RemoveCommandCommand { get; }
        public UpdateAvaliableCommandsCommand UpdateAvaliableCommandsCommand { get; }
        public LoadCommandForEditionCommand LoadCommandForEditionCommand { get; }

        public AdminCommandsHelperViewModel()
        {
            CreateNewOrEditExistingCommandCommand = new CreateNewOrEditExistingCommandCommand(this);
            CancelAndClearCommandActionCommand = new CancelAndClearCommandActionCommand(this);
            RemoveCommandCommand = new RemoveCommandCommand(this);
            UpdateAvaliableCommandsCommand = new UpdateAvaliableCommandsCommand(this);
            LoadCommandForEditionCommand = new LoadCommandForEditionCommand(this);

            Commands = new ObservableCollection<CommandInfoModel>();
            DisplayedCommand = new CommandInfoModel();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                #region Design Placeholders
                DisplayedCommand = new CommandInfoModel("sm_ban", "bans player", "<player> <time> <reason>");

                EditMode = true;
                Commands.Add(new CommandInfoModel("sm_ban", "bans player", "<player> <time> <reason>"));
                Commands.Add(new CommandInfoModel("sm_ban", "bans player", "<player> <time> <reason>"));
                Commands.Add(new CommandInfoModel("sm_ban", "bans player", "<player> <time> <reason>"));
                #endregion
            }
            else
            {
                UpdateCommands();
            }
        }

        public void UpdateCommands()
        {
            var commands = CommandInfoModel.GetAll();
            Commands.Clear();

            foreach (var command in commands)
            {
                if (!Commands.Contains(command))
                    Commands.Add(command);
            }
        }
    }
}
