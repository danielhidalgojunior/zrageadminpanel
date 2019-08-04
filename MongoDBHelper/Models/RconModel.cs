using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class RconModel : Entity
    {
        public string ServerName { get; set; }
        public string Ip { get; set; }
        public ushort Port { get; set; }
        public string RconPassword { get; set; }
        public string FastDlUrl { get; set; }

        public static RconModel GetOne(string servername)
        {
            var filter = Builders<RconModel>.Filter.Eq(x => x.ServerName, servername);
            return Mongo.Get(filter).SingleOrDefault();
        }
        public static RconModel GetOne(FilterDefinition<RconModel> filter = null) => Mongo.Get(filter).SingleOrDefault();
        public static bool Exists(FilterDefinition<RconModel> filter = null) => Mongo.Get(filter).Any();
    }
}
