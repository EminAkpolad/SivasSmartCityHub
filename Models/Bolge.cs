using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    public class Bolge
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id{get;set;}
        public required string Isim{get;set;}
        public required string Kategori{get;set;}
        public required string Aciklama{get;set;}
        public required string Adres{get;set;}
        public bool IndirimVarmi{get;set;}
        public double KullaniciPuani{get;set;}
    }
}