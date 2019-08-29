using MapHelper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MapHelper
{
    public class DownloadHandler
    {
        public Map Map { get; set; }
        private string Url;
        private string SavePath;
        private string DecompressPath;
        private int FilesCount { get; set; }
        private int CountNeeded { get; set; }
        private Action<List<string>, string> CallBack { get; set; }
        private List<string> DownloadedFiles { get; set; }

        public DownloadHandler(Map map, string url, string decompresspath, string savepath = null)
        {
            Map = map;
            Url = url;
            DecompressPath = decompresspath;

            if (savepath == null)
            {
                SavePath = Path.GetTempPath();
            }
            else
                SavePath = savepath;
        }

        public void RegisterCallBack(Action<List<string>, string> action)
        {
            CallBack = action;
        }

        public void DownloadMany(IEnumerable<string> files)
        {
            DownloadedFiles = new List<string>();
            Map.State = MapState.Downloading;
            FilesCount = files.Count();
            CountNeeded = 0;

            foreach (var file in files)
            {
                var filenamefull = Path.Combine(SavePath, file);
                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += Wc_DownloadProgressChanged;
                    client.DownloadFileCompleted += Wc_DownloadFileCompleted;

                    string fileurl = $"{Url}/{file}";
                    client.DownloadFileAsync(new Uri(fileurl), filenamefull);

                    DownloadedFiles.Add(filenamefull);
                }
            }
        }

        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            CountNeeded++;
            if (FilesCount == CountNeeded)
            {
                foreach (var file in DownloadedFiles)
                {
                    var fullfilepath = Path.Combine(SavePath, file);
                    if (!File.Exists(fullfilepath))
                        DownloadedFiles.Remove(fullfilepath);
                }

                Task.Run(() =>
                {
                    Map.State = MapState.Decompressing;
                    CallBack(DownloadedFiles, DecompressPath);
                    Map.State = MapState.Idle;
                });
            }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Map.Progress = e.ProgressPercentage;
        }
    }
}
