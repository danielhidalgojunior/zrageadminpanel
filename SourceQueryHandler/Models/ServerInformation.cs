using SourceQueryHandler.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceQueryHandler.Models
{
    public class ServerInformation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public List<Player> Players { get; set; }
        public string ServerName { get; set; }
        public string Map { get; set; }
        public int MaxPlayers { get; set; }
        public int OnlinePlayers { get; set; }
        public int Ping { get; set; }
        public bool Responding { get; set; }
        public string PlayerCounter => $"{OnlinePlayers}/{MaxPlayers}";
    }
}
