using SourceQueryHandler;
using SourceQueryHandler.models;
using SourceQueryHandler.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using zRageAdminMain.ViewModels.Commands;

namespace zRageAdminMain.ViewModels
{
    public class ServerStatusViewModel
    {
        public ServerInformation Info { get; set; }
        public QuickConnectCommand QuickConnectCommand { get; set; }

        private readonly DispatcherTimer _verificationLoop;
        public ServerStatusViewModel()
        {
            Info = new ServerInformation();

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Info.ServerName = "Zombie Rage: SERVIDOR UAU LEGAL";
                Info.Ping = 32;
                Info.Map = "ze_titanic_v2";
                Info.MaxPlayers = 37;
                Info.OnlinePlayers = 36;
                Info.Responding = true;
            }
            else
            {
                UpdateData(null, new EventArgs());
                _verificationLoop = new DispatcherTimer { Interval = new TimeSpan(0, 0, 5) };
                _verificationLoop.Tick += UpdateData;
                _verificationLoop.Start();
            }

            QuickConnectCommand = new QuickConnectCommand(this);
        }

        private void UpdateData(object sender, EventArgs e)
        {
            if (Application.Current.MainWindow?.WindowState == null || Application.Current.MainWindow?.WindowState == WindowState.Minimized)
                return;

            var i = ServerManager.Info;

            if (i != null)
            {
                Info.Responding = true;
                Info.ServerName = i.Name;
                Info.Map = i.Map;
                Info.MaxPlayers = i.MaxPlayers;
                Info.OnlinePlayers = i.Players;
                Info.Ping = (int)i.Ping;

                //var status = await App.Server.SendCommand("status");
                //Info.Players = Player.GetPlayersInfo(App.Server.Players, status);
            }
            else
                Info.Responding = false;
        }
    }
}
