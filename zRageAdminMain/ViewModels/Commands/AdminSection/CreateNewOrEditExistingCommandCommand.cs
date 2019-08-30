using MongoDBHelper.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using zRageAdminMain.ViewModels.AdminSection;

namespace zRageAdminMain.ViewModels.Commands.AdminSection
{
    public class CreateNewOrEditExistingCommandCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public AdminCommandsHelperViewModel VM { get; set; }

        public CreateNewOrEditExistingCommandCommand(AdminCommandsHelperViewModel vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return true;

            if (parameter == null)
                return false;

            var model = (parameter as CommandInfoModel);

            return VM.EditMode ? true : !CommandInfoModel.CommandOrIdExists(VM.DisplayedCommand);
        }

        public void Execute(object parameter)
        {
            if (VM.EditMode)
            {
                CommandInfoModel.UpdateOne(VM.DisplayedCommand);
            }
            else
            {
                CommandInfoModel.InsertOne(VM.DisplayedCommand);
            }

            VM.EditMode = false;
            VM.UpdateCommands();
            VM.DisplayedCommand.Clear();

            Variables.CommandAddedOrUpdated = true;
        }
    }
}
