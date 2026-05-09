using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Settings
{
    public class MongoDbSettings
    {
        public required string? Baglanti { get; set; } = null;
        public required string? VeriTabani { get; set; } = null;
    }
}
