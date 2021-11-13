using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComfyTravel
{
    [BsonIgnoreExtraElements]
    public class Objects
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("kids")]
        public bool Kids { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("coord")]
        public Tuple<double, double> Coordinates { get; set; }
    }

    public static class TypesOfObjects
    {
        public static readonly string Park = "Парк";
        public static readonly string Boulevard = "Бульвар";
        public static readonly string Museum = "Музей";
        public static readonly string Monument = "Памятник";
        public static readonly string Cinema = "Кино";
    }
}
