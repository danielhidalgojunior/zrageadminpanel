using MapHelper;
using MapHelper.Models;
using SourceQueryHandler.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace zRageAdminMain.ViewModels.Commands
{
    public class DownloadMapCommand : ICommand
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

        public NextMapViewModel VM { get; set; }
        public DownloadMapCommand(NextMapViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
                return false;

            if (string.IsNullOrEmpty((parameter as NextMap).Map))
                return false;

            return !(parameter as NextMap).Map.Contains("pendente");
        }

        public void Execute(object parameter)
        {
            var map = VM.Mh.GenerateMapByName((parameter as NextMap).Map);
            VM.MapInfo.Update(map);

            if (string.IsNullOrEmpty(VM.MapInfo.FullName))
                return;

            MessageBoxResult dialogResult = MessageBoxResult.Yes;
            if (map.AlreadyDownloaded)
            {
                dialogResult = MessageBox.Show($"{VM.MapInfo.FullName} already exists in you maps folder. Do you want to replace it?",
                    "Map exists",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
            }

            if (dialogResult == MessageBoxResult.Yes)
            {
                var dh = new DownloadHandler(map, VM.Mh.Fastdl, VM.Mh.Settings.MapsDirectory);
                dh.RegisterCallBack(VM.DecompressFiles);
                dh.DownloadMany(map.DownloadableFiles.Select(x => x.Name));
            }
        }
    }
}
