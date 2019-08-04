using MongoDB.Driver;
using StaticResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class UserGroupModel : Entity, INotifyPropertyChanged
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public int PermissionRank { get; set; }
        public List<string> AvaliableCommands { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Clear()
        {
            Id = new MongoDB.Bson.ObjectId();
            CreatedDate = null;
            GroupCode = "";
            GroupName = "";
            PermissionRank = 0;
            AvaliableCommands.Clear();

        }

        public UserGroupModel()
        {
            AvaliableCommands = new List<string>();
        }

        public UserGroupModel Clone()
        {
            var user = new UserGroupModel
            {
                Id = Id,
                CreatedDate = CreatedDate,
                GroupCode = (string) GroupCode.Clone(),
                GroupName = (string) GroupName.Clone(),
                PermissionRank = PermissionRank
            };

            if (AvaliableCommands != null)
            {
                foreach (var u in AvaliableCommands)
                {
                    user.AvaliableCommands.Add((string) u.Clone());
                }
            }

            return user;
        }

        public static IEnumerable<UserGroupModel> GetAll(FilterDefinition<UserGroupModel> filter = null, bool displayonly = false)
        {
            if (!displayonly || filter == null)
            {
                var loggeduser = (Variables.LoggedUser as UserModel);

                if (filter == null && !loggeduser.GodMode)
                    filter = Builders<UserGroupModel>.Filter.Gte(x => x.PermissionRank, loggeduser.HighestRank);
                else
                    filter = Builders<UserGroupModel>.Filter.Empty;
            }

            var tasks = Mongo.Get(filter).ToList();

            return tasks;
        }

        public static void UpdateOne(UserGroupModel entity) => Mongo.UpdateOne(entity);

        public static void InsertOne(UserGroupModel model) => Mongo.InsertOne(model);

        public static bool DeleteOne(UserGroupModel entity)
        {
            return Mongo.DeleteOne(entity);
        }

        public static bool CodeOrIdExists(UserGroupModel model)
        {
            var filtercode = Builders<UserGroupModel>.Filter.Eq(x => x.GroupCode, model.GroupCode);
            var filterid = Builders<UserGroupModel>.Filter.Eq(x => x.Id, model.Id);
            var filter = Builders<UserGroupModel>.Filter.Or(filtercode, filterid);

            return Mongo.GetCount(filter) > 0;
        }
    }
}
