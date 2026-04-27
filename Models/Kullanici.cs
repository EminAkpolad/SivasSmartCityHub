using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartCityHub.Models
{
    public class Kullanici
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id{get;set;}
        [Required(ErrorMessage ="Kullanici Adi Boş Bırakılamaz")]
        public required string KullaniciAdi{get; set;}

        public required string Email{get;set;}
        public required string Password{get;set;}
        public DateTime CreateAt {get;set;}=DateTime.Now;

        public bool Aktiflik{get;set;}=true;
    }
}