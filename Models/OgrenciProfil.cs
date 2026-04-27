using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    public class OgrenciProfil
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string KullaniciId{get; set; }
        public required string Bolum{get;set;}
        public required int seviye{get;set;}
        public required List<string> ilgiAlanlari{get;set;}
        public required string Hakkinda{get;set;}
    }
}