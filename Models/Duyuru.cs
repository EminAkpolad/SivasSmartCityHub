using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    [BsonIgnoreExtraElements]
    public class Duyuru
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string? Id { get; set; }


        public string? KullaniciId { get; set; }
        public required string Baslik { get; set; }
        public required string Icerik { get; set; }

        public string? YayinlananId { get; set; }
        public string? ResimUrl { get; set; }

        public DuyuruKategorisi kategori { get; set; }
        public OnemSeviyesi onem { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime BitisTarihi { get; set; }

        public enum DuyuruKategorisi
        {
            Genel,
            Yemekhane,
            Ulasim,
            Akademik,
            Etkinlik,
        }

        public enum OnemSeviyesi
        {
            Bilgi,
            Uyari,
            Acil,
        }
    }
}
