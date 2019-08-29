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
        public Map MapInfo { get; set; }
        public NextMap NextMap { get; set; }
        public DownloadMapCommand DownloadMapCommand { get; set; }

        private readonly DispatcherTimer _verificationLoop;
        public NextMapViewModel()
        {
            MapInfo = new Map();
            NextMap = new NextMap();

            DownloadMapCommand = new DownloadMapCommand(this);

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                NextMap.Map = "ze_potc_p4";
                NextMap.TimeLeft = "3:35";
            }
            else
            {
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
    }
}
