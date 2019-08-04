using MongoDBHelper;
using MongoDBHelper.Models;
using SourceQueryHandler;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace zRageAdminMain
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Mongo.Initialize(JsonConfigurator.GetConnectionStringByConfigFile("config/db.json"));

            var auth = RconModel.GetOne("zRage");
            Variables.Server = new ServerManager(auth);
        }

        public static Color GetHashColor(string text)
        {
            Random r;
            var color = new Color();
            var str = text;
            var bytes = Encoding.ASCII.GetBytes(str);
            int seed = 0;

            bytes.ToList().ForEach(x => seed += x + text.Length);

            r = new Random(seed);

            color.R = (byte)r.Next(90, 255);
            color.G = (byte)r.Next(90, 255);
            color.B = (byte)r.Next(90, 255);
            color.A = 255;

            return color;
        }
    }
}
