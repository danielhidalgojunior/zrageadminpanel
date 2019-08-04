using MongoDBHelper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zRageAdminMain.Models
{
    public class Login : INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
