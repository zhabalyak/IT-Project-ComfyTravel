using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComfyTravel.Models
{
    [BsonIgnoreExtraElements]
    public class Comments
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("user_id")]
        public int UserId { get; set; }
    }
}
