using MongoDBHelper.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zRageAdminMain.ViewModels
{
    public class MainViewModel
    {
        public UserModel User { get; set; }
        public MainViewModel()
        {
            User = Variables.LoggedUser as UserModel;
        }
    }
}
