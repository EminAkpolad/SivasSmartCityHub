using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    public class Etkinlik
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string Baslik { get; set; }
        public required string Aciklama { get; set; }

        public required string OlusturanId { get; set; }

        public DateTime TarihSaat { get; set; }
        public string? ResimUrl { get; set; }

        public required string Konum { get; set; }
        public int Kapasite { get; set; }

        public List<string> KatilimciId { get; set; } = new List<string>();

        public EtkinlikKategorisi Kategori { get; set; }

        public enum EtkinlikKategorisi
        {
            Egitim,
            Spor,
            Eglence,
            Yazilim,
            Diger,
        }
    }
}
