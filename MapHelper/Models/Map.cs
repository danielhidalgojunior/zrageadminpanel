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
        public List<MapFile> DownloadableFiles { get; set; }
        public bool Visible { get; set; } = true;
        public MapState State { get; set; } = MapState.Idle;
        public int Progress { get; set; }

        public Map Clone()
        {
            List<MapFile> list = new List<MapFile>();
            foreach (var file in DownloadableFiles)
                list.Add(file.Clone());

            return new Map
            {
                FullName = FullName,
                Size = Size,
                CanDownload = CanDownload,
                AlreadyDownloaded = AlreadyDownloaded,
                DownloadableFiles = list,
                Visible = Visible,
                State = State,
                Progress = Progress
            };
        }

        public void Update(Map map)
        {
            FullName = map.FullName;
            Size = map.Size;
            CanDownload = map.CanDownload;
            AlreadyDownloaded = map.AlreadyDownloaded;
            DownloadableFiles.AddRange(map.DownloadableFiles);
            Visible = map.Visible;
            State = map.State;
            Progress = map.Progress;
        }
    }
}
