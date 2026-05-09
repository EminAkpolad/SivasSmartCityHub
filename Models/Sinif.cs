using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    public class Sinif
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        public required string Kod { get; set; }
        public required string Ad { get; set; }
        public required string Bolge { get; set; }

        public int Kapasite { get; set; }
        public int Kat { get; set; }

        public List<string> Ozellikler = new List<string>();

        public bool DolumuBosmu { get; set; } = true;
    }
}
