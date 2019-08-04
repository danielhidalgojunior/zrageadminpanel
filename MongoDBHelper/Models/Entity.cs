using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBHelper.Models
{
    public abstract class Entity
    {
        [BsonId]
        public ObjectId Id { set; get; } = ObjectId.GenerateNewId();
        public DateTime? CreatedDate { get; set; }
    }
}
