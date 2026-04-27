using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    public class Sinif
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id{get;set;}
    }
}