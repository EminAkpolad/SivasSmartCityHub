using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace SmartCityHub.Models
{
    public class Ilan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public required string Baslik { get; set; }
        public required string Aciklama { get; set; }
        public decimal Fiyat { get; set; }
        public IlanTipi Tip { get; set; }
        public Kategori kategori { get; set; }
        public required string KullaniciId { get; set; }
        public required string ResimUrl { get; set; }
        public DateTime Tarih { get; set; }=DateTime.Now;

        public bool Aktifmi { get; set; }=true;

        public enum IlanTipi
        {
            Satilik, Kiralik,
        }

        public enum Kategori
        {
            Kitap, Elektronik, EvAletleri
        }
    }
}