using Bz2Helper;
using MapHelper;
using MapHelper.Models;
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
    public class DownloadMapFilesCommand : ICommand
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
        public MapsListingViewModel VM { get; set; }

        public DownloadMapFilesCommand(MapsListingViewModel vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
                return true;

            var map = parameter as Map;

            return (map.CanDownload && map.State == MapState.Idle);
        }

        public void Execute(object parameter)
        {
            var map = parameter as Map;
            var mapsInDirectory = VM.Mh.GetMapFilesFromMapsDirectory();

            MessageBoxResult dialogResult = MessageBoxResult.Yes;
            if (mapsInDirectory.Select(x => Path.GetFileNameWithoutExtension(x)).Contains(map.FullName))
            {
                dialogResult = MessageBox.Show($"{map.FullName} already exists in you maps folder. Do you want to replace it?",
                    "Map exists",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
            }

            if (dialogResult == MessageBoxResult.Yes)
            {
                var dh = new DownloadHandler(map, VM.Mh.Fastdl, VM.Mh.Settings.MapsDirectory);
                dh.RegisterCallBack(VM.DecompressFiles);
                dh.DownloadMany(map.DownloadableFiles);
            }
        }
    }
}
