using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class UserModel : Entity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<ObjectId> UserGroupsIds { get; set; }
        [BsonIgnore]
        public List<UserGroupModel> UserGroups
        {
            get
            {
                if (UserGroupsIds == null)
                    return new List<UserGroupModel>();

                var filter = Builders<UserGroupModel>.Filter.In(x => x.Id, UserGroupsIds);
                return UserGroupModel.GetAll(filter, true).ToList();
            }
        }
        public DateTime? LastLogin { get; set; }

        public UserModel()
        {
            UserGroupsIds = new List<ObjectId>();
        }

        public UserModel Clone()
        {
            var user = new UserModel
            {
                Id = Id,
                CreatedDate = CreatedDate,
                Name = (string) Name.Clone(),
                Login = (string) Login.Clone(),
                Password = (string) Password.Clone(),
                LastLogin = LastLogin,
                Enabled = Enabled
            };

            if (UserGroups != null)
            {
                foreach (var u in UserGroups)
                {
                    user.UserGroups.Add(u.Clone());
                }
            }

            return user;
        }

        public int? HighestRank
        {
            get
            {
                if (UserGroups != null)
                {
                    if (UserGroups.Count > 0)
                        return UserGroups.Max(x => x.PermissionRank);
                }

                return null;
            }
        }
        public bool GodMode { get; set; }
        public bool Enabled { get; set; }

        public static UserModel TryLogin(string username, string password)
        {
            var filtername = Builders<UserModel>.Filter.Eq(x => x.Login, username);
            var filterpw = Builders<UserModel>.Filter.Eq(x => x.Password, password);
            var filter = Builders<UserModel>.Filter.And(filtername, filterpw);

            return GetOne(filter);
        }
        public static IEnumerable<UserModel> GetAll(FilterDefinition<UserModel> filter = null)
        {
            if (filter == null)
                filter = Builders<UserModel>.Filter.Empty;

            var tasks = Mongo.Get(filter).ToList();

            return tasks;
        }

        public static IEnumerable<UserModel> GetAll(UserGroupModel group)
        {
            var filter = Builders<UserModel>.Filter.AnyEq(x => x.UserGroupsIds, group.Id);
            var tasks = Mongo.Get(filter).ToList();

            return tasks;
        }

        public void Clear()
        {
            Name = null;
            Login = null;
            Password = null;
            if (UserGroups != null)
                UserGroups.Clear();
            Enabled = true;
            CreatedDate = null;
        }

        public bool IsCommandAvaliable(string command)
        {
            if (GodMode)
                return true;

            if (UserGroups == null)
                return false;

            foreach (var group in UserGroups)
            {
                if (group.AvaliableCommands.Where(x => command.Contains(x)).Any())
                    return true;
            }

            return false;
        }

        public bool AreCommandsAvaliable(List<string> commands)
        {
            if (GodMode)
                return true;

            if (UserGroups == null)
                return false;

            foreach (var group in UserGroups)
            {
                foreach (var command in commands)
                {
                    if (group.AvaliableCommands.Where(x => command.Contains(x)).Any())
                        return true;
                }
            }

            return false;
        }

        private List<string> GetPossibleCommandVariations(string command)
        {
            var isSm = command.ToLower().StartsWith("sm_");
            var possiblecmds = new List<string> { command };

            var basecmd = command.Substring(0, 3);
            possiblecmds.Add('/' + (isSm ? basecmd : command));
            possiblecmds.Add('!' + (isSm ? basecmd : command));
            possiblecmds.Add('.' + (isSm ? basecmd : command));

            return possiblecmds;
        }

        public static UserModel GetOne(FilterDefinition<UserModel> filter = null) => Mongo.Get(filter).SingleOrDefault();

        public static bool Exists(FilterDefinition<UserModel> filter = null) => Mongo.Get(filter).Any();

        public static bool LoginOrIdExists(UserModel model)
        {
            var filterlogin = Builders<UserModel>.Filter.Eq(x => x.Login, model.Login);
            var filterid = Builders<UserModel>.Filter.Eq(x => x.Id, model.Id);
            var filter = Builders<UserModel>.Filter.Or(filterlogin, filterid);

            return Mongo.GetCount(filter) > 0;
        }

        public static bool LoginExists(UserModel model)
        {
            var filter = Builders<UserModel>.Filter.Eq(x => x.Login, model.Login);

            return Mongo.GetCount(filter) > 0;
        }

        public static void InsertOne(UserModel entity)
        {
            //UpdateGroupsIds(entity);
            Mongo.InsertOne(entity);
        }

        private static void UpdateGroupsIds(UserModel model)
        {
            var groups = model.UserGroups;

            if (model.UserGroupsIds == null)
                model.UserGroupsIds = new List<ObjectId>();
            else
                model.UserGroupsIds.Clear();

            foreach (var group in groups)
                model.UserGroupsIds.Add(group.Id);
        }

        public static void DeleteOne(FilterDefinition<UserModel> filter) => Mongo.DeleteOne(filter);

        public static bool DeleteOne(UserModel entity) => Mongo.DeleteOne(entity);

        public static bool DeleteMany(IEnumerable<UserModel> entities) => Mongo.DeleteMany(entities);

        public bool RemoveGroupById(ObjectId id)
        {
            var group = UserGroupsIds.Where(x => x == id).SingleOrDefault();

            if (group != null)
            {
                UserGroupsIds.Remove(group);
                UpdateOne(this);

                return true;
            }

            return false;
        }

        public static void UpdateOne(FilterDefinition<UserModel> filter, UserModel entity) => Mongo.UpdateOne(filter, entity);

        public static void UpdateOne(UserModel entity, bool forcegroupupdate = false)
        {
            if (forcegroupupdate)
                UpdateGroupsIds(entity);

            Mongo.UpdateOne(entity);
        }
    }
}
