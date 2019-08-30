using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapHelper.Models
{
    public class MapFile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public long Size { get; set; }
        public bool Downloaded { get; set; }

        public MapFile Clone()
        {
            return new MapFile
            {
                Name = (string)Name.Clone(),
                Size = Size,
                Downloaded = Downloaded
            };
        }
    }
}
