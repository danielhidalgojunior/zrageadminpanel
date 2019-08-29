using MapHelper.Models;
using SourceQueryHandler;
using StaticResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MapHelper
{
    public class MapHandler
    {
        public MapHandlerSettings Settings { get; set; }
        public string Fastdl { get; set; }
        public IEnumerable<string> RconMaps { get; set; }
        public IEnumerable<string> DownloadedMaps { get; set; }
        public IEnumerable<Map> Maps { get; set; }

        private readonly string[] MapFilesVariations = { ".bsp", ".nav" };

        public MapHandler(MapHandlerSettings settings)
        {
            Fastdl = FindAvaliableFastdl(settings.FastdLinks);
            Settings = settings;

            DownloadedMaps = GetMapFilesFromMapsDirectory();
        }

        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        private string FindAvaliableFastdl(List<string> fastdLinks)
        {
            var ping = new Ping();

            foreach (var item in fastdLinks)
            {
                Uri myUri = new Uri(item);
                string host = myUri.Host;

                IPStatus status = IPStatus.TimedOut;
                try
                {
                    status = ping.Send(host).Status;
                }
                catch (Exception) { }

                if (status == IPStatus.Success)
                    return item;
            }

            return null;
        }

        public IEnumerable<Map> PopulateMaps(IEnumerable<string> maps)
        {
            var files = DownloadedMaps;
            List<Map> auxmaps = new List<Map>();

            foreach (var map in maps)
            {
                var mapobj = new Map();
                mapobj.FullName = (string)map.Clone();
                mapobj.DownloadableFiles = GetMapFilesByName(map).ToList();

                if (files.Contains(map + ".bsp"))
                {
                    mapobj.CanDownload = true;
                    mapobj.AlreadyDownloaded = true;
                }

                auxmaps.Add(mapobj);
            }

            return auxmaps;
        }

        public IEnumerable<string> GetMapFilesFromMapsDirectory()
        {
            return Directory.GetFiles(Settings.MapsDirectory, "*.bsp").Select(x => Path.GetFileName(x));
        }

        public IEnumerable<string> GetMapFilesByName(string mapname)
        {
            return MapFilesVariations.Select(x => string.Concat(mapname, x, ".bz2"));
        }

        public string GetMapUrl(string map, bool IsBz2 = true)
        {
            var fileFormat = IsBz2 ? ".bsp.bz2" : ".bsp";

            return string.Concat(CombineUri(Fastdl, map), fileFormat);
        }

        public static string CombineUri(params string[] uriParts)
        {
            string uri = string.Empty;
            if (uriParts != null && uriParts.Length > 0)
            {
                char[] trims = new char[] { '\\', '/' };
                uri = (uriParts[0] ?? string.Empty).TrimEnd(trims);
                for (int i = 1; i < uriParts.Length; i++)
                {
                    uri = string.Format("{0}/{1}", uri.TrimEnd(trims), (uriParts[i] ?? string.Empty).TrimStart(trims));
                }
            }
            return uri;
        }

        public static string GetMapNameWithoutExtensions(string map)
        {
            return map.Replace(".bsp.bz2", "").Replace(".bsp", "");
        }

        public string GetMapSize(Map map)
        {
            return GetFileSize(GetMapUrl(map.FullName));
        }

        public static string GetFileSize(string remotefile)
        {
            var webRequest = WebRequest.Create(remotefile);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {
                var fileSize = webResponse.Headers.Get("Content-Length");
                var fileSizeInBytes = Convert.ToInt32(fileSize);
                return BytesToString(fileSizeInBytes);
            }
        }

        private IEnumerable<string> OnlyMaps(IEnumerable<string> list, string prefix = null)
        {
            List<string> aux = new List<string>();

            foreach (var map in list)
            {
                if (map.EndsWith(".bsp.bz2") || map.EndsWith(".bsp"))
                {
                    if (prefix != null)
                    {
                        if (map.StartsWith(prefix))
                            aux.Add(map);
                    }
                    else
                        aux.Add(map);
                }
                else
                    continue;
            }

            return aux;
        }

        public static string GetGameModeByName(string mapname)
        {
            return mapname.Split('_')[0].ToUpper();
        }

        public static bool IsEscape(string map)
        {
            return map.ToLower().StartsWith("ze_");
        }

        public static bool IsTTT(string map)
        {
            return map.ToLower().StartsWith("ttt_");
        }

        public static bool IsBarricade(string map)
        {
            return map.ToLower().StartsWith("zm_");
        }

        public static bool IsZombieBhop(string map)
        {
            return map.ToLower().StartsWith("zb_");
        }
    }
}
