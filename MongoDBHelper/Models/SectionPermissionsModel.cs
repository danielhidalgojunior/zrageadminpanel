using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class SectionPermissionsModel : Entity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string SettingsSectionName { get; set; }
        public Dictionary<string, int> RankedPermissions { get; set; }
    }
}
