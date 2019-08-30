using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDBHelper.Models;
using StaticResources;

namespace MongoDBHelper
{
    public static class Mongo
    {
        private static MongoClient Client { get; set; }
        private static IMongoDatabase Db { get; set; }

        public static void Initialize(JsonConfigurator configurator)
        {
            try
            {
                Client = new MongoClient(configurator.ConnectionString);
                Db = Client.GetDatabase(configurator.DatabaseName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static IMongoCollection<T> GetCollection<T>() where T : Entity
        {
            return Db.GetCollection<T>(typeof(T).Name.NormalizeCollectionName());
        }

        public static IAsyncCursor<T> Get<T>(FilterDefinition<T> filter = null) where T : Entity
        {
            var collection = Db.GetCollection<T>(typeof(T).Name.NormalizeCollectionName());

            if (filter == null)
                return collection.FindAsync(FilterDefinition<T>.Empty).Result;
            else
                return collection.FindAsync(filter).Result;
        }

        public static long GetCount<T>(FilterDefinition<T> filter = null) where T : Entity
        {
            var collection = Db.GetCollection<T>(typeof(T).Name.NormalizeCollectionName());

            if (filter == null)
                return collection.CountDocumentsAsync(FilterDefinition<T>.Empty).Result;
            else
                return collection.CountDocumentsAsync(filter).Result;
        }

        public static void InsertOne<T>(T entity) where T : Entity
        {
            entity.CreatedByUser = (Variables.LoggedUser as UserModel).Name;
            entity.CreatedDate = DateTime.Now;
            GetCollection<T>().InsertOneAsync(entity).Wait();
        }

        public static void InsertMany<T>(params T[] entities) where T : Entity
        {
            foreach (var ent in entities)
            {
                ent.CreatedByUser = (Variables.LoggedUser as UserModel).Name;
                ent.CreatedDate = DateTime.Now;
            }

            GetCollection<T>().InsertManyAsync(entities).Wait();
        }

        public static void DeleteOne<T>(FilterDefinition<T> filter) where T : Entity
        {
            GetCollection<T>().DeleteOneAsync(filter).Wait();
        }

        public static bool DeleteOne<T>(T entity) where T : Entity
        {
            var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
            GetCollection<T>().DeleteOneAsync(filter).Wait();

            return !GetCollection<T>().FindAsync(filter).Result.Any();
        }

        public static bool DeleteMany<T>(FilterDefinition<T> filter) where T : Entity
        {
            GetCollection<T>().DeleteManyAsync(filter).Wait();

            return !GetCollection<T>().FindAsync(filter).Result.Any();
        }

        public static bool DeleteMany<T>(IEnumerable<T> entities) where T : Entity
        {
            var filter = Builders<T>.Filter.In(x => x, entities);
            GetCollection<T>().DeleteManyAsync(filter).Wait();

            return !GetCollection<T>().FindAsync(filter).Result.Any();
        }

        public static void UpdateOne<T>(FilterDefinition<T> filter, T entity) where T : Entity
        {
            entity.LastModifiedByUser = (Variables.LoggedUser as UserModel).Name;
            entity.CreatedByUser = (Variables.LoggedUser as UserModel).Name;
            entity.ModifiedDate = DateTime.Now;
            var options = new UpdateOptions { IsUpsert = true };
            GetCollection<T>().ReplaceOneAsync(filter, entity, options).Wait();
        }

        public static void UpdateOne<T>(T entity) where T : Entity
        {
            entity.LastModifiedByUser = (Variables.LoggedUser as UserModel).Name;
            entity.CreatedByUser = (Variables.LoggedUser as UserModel).Name;
            entity.ModifiedDate = DateTime.Now;
            var options = new UpdateOptions { IsUpsert = true };
            var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
            GetCollection<T>().ReplaceOneAsync(filter, entity, options).Wait();
        }

        public static bool UpdateField<T>(FilterDefinition<T> filter, UpdateDefinition<T> update) where T : Entity
        {
            return GetCollection<T>().UpdateOneAsync(filter, update).Result.IsAcknowledged;
        }
    }
}
