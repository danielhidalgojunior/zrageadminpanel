using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticResources
{
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [JsonProperty("getMapsWithRcon")]
        public bool GetMapsWithRcon { get; set; }
        [JsonProperty("fastdlLinks")]
        public List<FastdlUrl> FastdLinks { get; set; }
        [JsonProperty("mapsDirectory")]
        public string MapsDirectory { get; set; }
    }

    public class FastdlUrl : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
