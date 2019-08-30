using Bz2Helper;
using MapHelper;
using MapHelper.Models;
using SourceQueryHandler;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using zRageAdminMain.ViewModels.Commands;

namespace zRageAdminMain.ViewModels
{
    public class MapsListingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MapHandler Mh { get; set; }
        public ObservableCollection<Map> Maps { get; set; }
        private string mapSearch;
        public string MapSearch
        {
            get { return mapSearch; }
            set
            {
                mapSearch = value;
                DisplayOnlyMatches(value);
            }
        }

        public DownloadMapFilesCommand DownloadMapFilesCommand { get; set; }
        public UpdateMapInfoCommand UpdateMapInfoCommand { get; set; }

        public MapsListingViewModel()
        {
            DownloadMapFilesCommand = new DownloadMapFilesCommand(this);
            UpdateMapInfoCommand = new UpdateMapInfoCommand(this);

            Maps = new ObservableCollection<Map>();
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Maps.Add(new Map()
                {
                    FullName = "ze_licciana_pe",
                    CanDownload = true,
                    AlreadyDownloaded = true,
                    DownloadableFiles = new List<MapFile>
                    {
                        new MapFile{ Name = "url1", Size = 535345 },
                        new MapFile{ Name = "url1", Size = 535345 },
                        new MapFile{ Name = "url1", Size = 535345 }
                    }
                });

                Maps.Add(new Map()
                {
                    FullName = "ze_fapescape_p5",
                    CanDownload = false,
                    AlreadyDownloaded = true,
                    DownloadableFiles = new List<MapFile>
                    {
                        new MapFile{ Name = "url1", Size = 535345 },
                        new MapFile{ Name = "url1", Size = 535345 },
                        new MapFile{ Name = "url1", Size = 535345 }
                    }
                });

                Maps.Add(new Map()
                {
                    FullName = "ze_dust_pe",
                    CanDownload = true,
                    AlreadyDownloaded = false,
                    DownloadableFiles = new List<MapFile>
                    {
                        new MapFile{ Name = "url1", Size = 535345 },
                        new MapFile{ Name = "url1", Size = 535345 },
                        new MapFile{ Name = "url1", Size = 535345 }
                    }
                });
            }
            else
            {
                var settings = new MapHandlerSettings
                {
                    FastdLinks = Variables.Settings.FastdLinks.Select(x => x.Url).ToList(),
                    MapsDirectory = Variables.Settings.MapsDirectory
                };

                Mh = new MapHandler(settings);
                ReloadMaps();
            }
        }

        public async Task<IEnumerable<string>> GetMapsFromRcon()
        {
            var response = await ServerManager.SendCommand("listmaps");
            var maps = response.Split('\n');

            return maps.Where(x => !x.Contains(" ") && x.Length > 3); ;
        }

        public void DisplayOnlyMatches(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                foreach (var map in Maps)
                    map.Visible = true;
            }
            else
            {
                foreach (var map in Maps)
                {
                    map.Visible = map.FullName.Contains(str);
                }
            }
        }

        public async void ReloadMaps()
        {
            var rconmaps = await GetMapsFromRcon();
            var maps = Mh.PopulateMaps(rconmaps);
            Maps.Clear();

            foreach (var map in maps)
                Maps.Add(map);            
        }

        public void UpdateMapInfo(Map map)
        {
            var downloadedmaps = Mh.GetMapFilesFromMapsDirectory();

            if (downloadedmaps.Where(x => Path.GetFileNameWithoutExtension(x) == map.FullName).Any())
                map.AlreadyDownloaded = true;

            foreach (var mapfile in map.DownloadableFiles)
            {
                if (Mh.MapFileExists(mapfile))
                    mapfile.Downloaded = true;
                else
                    mapfile.Downloaded = false;

                mapfile.Size = Mh.GetSize(mapfile);
            }
        }

        public void DecompressFiles(List<string> filenames, string location)
        {
            foreach (var file in filenames)
            {
                Bz2Handler.Decompress(file, location);
            }
        }
    }
}
