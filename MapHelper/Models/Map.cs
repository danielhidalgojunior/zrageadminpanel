using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHelper.Models
{
    public class Map : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string FullName { get; set; }
        public string Size { get; set; }
        public bool CanDownload { get; set; } = true;
        public bool AlreadyDownloaded { get; set; } = false;
        public List<string> DownloadableFiles { get; set; }
        public bool Visible { get; set; } = true;
        public MapState State { get; set; } = MapState.Idle;
        public int Progress { get; set; }
    }
}
