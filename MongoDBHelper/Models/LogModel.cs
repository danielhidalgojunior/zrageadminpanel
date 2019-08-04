using MongoDB.Bson;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class LogModel : Entity
    {
        public static string[] CommandIgnore = {"status", "sm_players", "nextmap", "timeleft"};
        public string Command { get; set; }
        public ObjectId UserId { get; set; }
        public string UserName { get; set; }

        public LogModel(string command)
        {
            UserId = (Variables.LoggedUser as UserModel).Id;
            UserName = (Variables.LoggedUser as UserModel).Name;
        }

        private bool canIgnore()
        {
            return CommandIgnore.Contains(Command.ToLower());
        }
        public static void InsertOne(LogModel model)
        {
            Mongo.InsertOne(model);
        }
        public void Save()
        {
            if (!canIgnore())
                InsertOne(this);
        }
    }
}
