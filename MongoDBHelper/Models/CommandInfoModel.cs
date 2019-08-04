using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class CommandInfoModel : Entity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Command { get; set; }
        public string Parameters { get; set; }
        public string Description { get; set; }

        public void Clear()
        {
            Command = "";
            Parameters = "";
            Description = "";
        }
    }
}
