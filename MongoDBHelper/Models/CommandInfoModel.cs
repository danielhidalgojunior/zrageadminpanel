using MongoDB.Driver;
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
        public bool Ignorable { get; set; } = false;

        public CommandInfoModel(string command, string desc, string param = null)
        {
            Command = command;
            Description = desc;
            Parameters = param;
        }

        public CommandInfoModel()
        {

        }

        public void Clear()
        {
            Command = "";
            Parameters = "";
            Description = "";
        }

        public static IEnumerable<CommandInfoModel> GetAll() => Mongo.Get<CommandInfoModel>().ToList();
        public static void InsertOne(CommandInfoModel model) => Mongo.InsertOne(model);

        public CommandInfoModel Clone()
        {
            return new CommandInfoModel((string)Command.Clone(), (string)Description.Clone(), (string)Parameters.Clone());
        }

        public static bool CommandOrIdExists(CommandInfoModel displayedCommand)
        {
            var filtercommand = Builders<CommandInfoModel>.Filter.Eq(x => x.Command, displayedCommand.Command);
            var filterid = Builders<CommandInfoModel>.Filter.Eq(x => x.Id, displayedCommand.Id);
            var filter = Builders<CommandInfoModel>.Filter.Or(filtercommand, filterid);

            return Mongo.GetCount(filter) > 0;
        }

        public static void UpdateOne(CommandInfoModel displayedGroup) => Mongo.UpdateOne(displayedGroup);

        public static void DeleteOne(CommandInfoModel model) => Mongo.DeleteOne(model);
    }
}
