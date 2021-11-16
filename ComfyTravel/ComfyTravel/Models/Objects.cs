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
        public const string Park = "Парк";
        public const string Boulevard = "Бульвар";
        public const string Museum = "Музей";
        public const string Monument = "Памятник";
        public const string Cinema = "Кино";
    }

    public static class TypesOfTransport
    {
        public const string Walk = "w";
        public const string Car = "c";
        public const string Public = "p";
    }
}
