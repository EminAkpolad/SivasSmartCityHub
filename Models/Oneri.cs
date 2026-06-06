using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    public class Oneri
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string GonderenId { get; set; }
        public required string HedefId { get; set; }
        public required string Baslik { get; set; }
        public required string Aciklama { get; set; }
        public string? Icerik { get; set; }

        public string kullaniciId { get; set; }

        [BsonRepresentation(BsonType.Int32)]
        public int Puan { get; set; }

        public OneriKategorisi Kategori { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;

        public enum OneriKategorisi
        {
            Yemekhane,
            Ulasim,
            Kampus,
            SosyalTesis,
            Diger,
        }
    }
}
