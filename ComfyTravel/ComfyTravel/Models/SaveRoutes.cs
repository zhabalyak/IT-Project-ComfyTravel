using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComfyTravel.Models
{
    [BsonIgnoreExtraElements]
    public class SaveRoutes
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("route")]
        public List<int> Route { get; set; }

        [BsonElement("user_id")]
        public int UserId { get; set; }
    }
}
