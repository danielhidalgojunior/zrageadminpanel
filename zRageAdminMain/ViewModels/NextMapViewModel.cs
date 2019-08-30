using Bz2Helper;
using MapHelper;
using MapHelper.Models;
using SourceQueryHandler;
using SourceQueryHandler.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using zRageAdminMain.ViewModels.Commands;

namespace zRageAdminMain.ViewModels
{
    public class NextMapViewModel
    {
        public MapHandler Mh { get; set; }
        public NextMap NextMap { get; set; }
        public Map MapInfo { get; set; }
        public DownloadMapCommand DownloadMapCommand { get; set; }

        private readonly DispatcherTimer _verificationLoop;
        public NextMapViewModel()
        {
            NextMap = new NextMap();
            MapInfo = new Map();

            DownloadMapCommand = new DownloadMapCommand(this);

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                NextMap.Map = "ze_potc_p4";
                NextMap.TimeLeft = "3:35";
            }
            else
            {
                var settings = new MapHandlerSettings
                {
                    FastdLinks = Variables.Settings.FastdLinks.Select(x => x.Url).ToList(),
                    MapsDirectory = Variables.Settings.MapsDirectory
                };

                Mh = new MapHandler(settings);

                UpdateData(null, new EventArgs());
                _verificationLoop = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
                _verificationLoop.Tick += UpdateData;
                _verificationLoop.Start();
            }
        }

        private async void UpdateData(object sender, EventArgs eventArgs)
        {
            var nMresponse = await ServerManager.SendCommand("nextmap");
            NextMap.Map = NextMap.ParseNextMapResponse(nMresponse);

            var tlResponse = await ServerManager.SendCommand("timeleft");
            NextMap.TimeLeft = NextMap.ParseTimeLeftResponse(tlResponse);
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
