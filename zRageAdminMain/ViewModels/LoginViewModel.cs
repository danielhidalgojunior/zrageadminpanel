using MongoDBHelper.Models;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zRageAdminMain.Models;
using zRageAdminMain.ViewModels.Commands;

namespace zRageAdminMain.ViewModels
{
    public class LoginViewModel
    {
        public TryLoginCommand TryLoginCommand { get; set; }
        public GetFastLoginDataCommand GetFastLoginDataCommand { get; set; }
        public Login Login { get; set; }

        public LoginViewModel()
        {
            Login = new Login();
            TryLoginCommand = new TryLoginCommand(this);
            GetFastLoginDataCommand = new GetFastLoginDataCommand(this);
        }

        public bool DoLogin()
        {
            var user = UserModel.TryLogin(Login.UserName, Login.Password);

            if (user != null)
            {
                user.LastLogin = DateTime.Now;
                Variables.LoggedUser = user;

                UserModel.UpdateOne(user);
            }

            return user != null;
        }
    }
}
