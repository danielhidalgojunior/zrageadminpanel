using SourceQueryHandler;
using SourceQueryHandler.models;
using SourceQueryHandler.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace zRageAdminMain.ViewModels
{
    public class PlayersBoardViewModel
    {
        public ObservableCollection<Player> OnlineTPlayers { get; set; }
        public ObservableCollection<Player> OnlineCtPlayers { get; set; }
        public ObservableCollection<Player> OnlineSpecPlayers { get; set; }

        private readonly DispatcherTimer _verificationLoop;
        public PlayersBoardViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {

            }
            else
            {
                OnlineTPlayers = new ObservableCollection<Player>();
                OnlineCtPlayers = new ObservableCollection<Player>();
                OnlineSpecPlayers = new ObservableCollection<Player>();

                _verificationLoop = new DispatcherTimer { Interval = new TimeSpan(0, 0, 3) };
                _verificationLoop.Tick += UpdatePlayers;
                _verificationLoop.Start();
            }
        }

        private async void UpdatePlayers(object sender, EventArgs e)
        {
            var statusresponse = await ServerManager.SendCommand("status");
            var players = Player.GetPlayersInfo(ServerManager.Players, statusresponse);
        }
    }
}
