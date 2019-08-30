using MongoDB.Bson;
using MongoDB.Driver;
using StaticResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public class EventTriggerModel : Entity, INotifyPropertyChanged
    {
        public ObjectId User { get; set; }
        public NoticeType? Notice { get; set; }
        public EventType? Event { get; set; }
        public DateRange Period { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; }

        public EventTriggerModel()
        {
            Period = new DateRange();

            if ((Variables.LoggedUser as UserModel) != null)
                User = (Variables.LoggedUser as UserModel).Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static void InsertOne(EventTriggerModel model) => Mongo.InsertOne(model);
        public static IEnumerable<EventTriggerModel> GetAll(FilterDefinition<EventTriggerModel> filter = null)
        {
            if (filter == null)
                filter = Builders<EventTriggerModel>.Filter.Eq(x => x.User, (Variables.LoggedUser as UserModel).Id);

            var tasks = Mongo.Get(filter).ToList();

            return tasks;
        }

        public static void DeleteOne(EventTriggerModel model)
        {
            if (model.User != (Variables.LoggedUser as UserModel).Id)
                return;

            Mongo.DeleteOne(model);
        }

        public bool Disable()
        {
            if (User != (Variables.LoggedUser as EventTriggerModel).Id)
                return false;

            if (!Enabled)
                return false;

            var filter = Builders<EventTriggerModel>.Filter.Eq(x => x.Id, Id);
            var update = Builders<EventTriggerModel>.Update.Set(x => x.Enabled, false);

            // preguiça de botar no method do mongo, vai aqui mesmo por enquanto
            LastModifiedByUser = (Variables.LoggedUser as UserModel).Name;
            CreatedByUser = (Variables.LoggedUser as UserModel).Name;
            ModifiedDate = DateTime.Now;

            return Mongo.UpdateField(filter, update);
        }
    }
}
