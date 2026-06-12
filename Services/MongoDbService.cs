using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using MongoDB.Driver;
using SmartCityHub.Models;
using SmartCityHub.Settings;

namespace SmartCityHub.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.Baglanti);
            _database = client.GetDatabase(settings.Value.VeriTabani);
        }

        public IMongoCollection<Bolge> Bolgeler => _database.GetCollection<Bolge>("Bolge");
        public IMongoCollection<Duyuru> Duyurular => _database.GetCollection<Duyuru>("Duyuru");
        public IMongoCollection<Ilan> Ilanlar => _database.GetCollection<Ilan>("Ilan");
        public IMongoCollection<Kullanici> Kullanicilar =>
            _database.GetCollection<Kullanici>("Kullanici");
        public IMongoCollection<OgrenciProfil> OgrenciProfilleri =>
            _database.GetCollection<OgrenciProfil>("OgrenciProfil");
        public IMongoCollection<Oneri> Oneriler => _database.GetCollection<Oneri>("Oneri");
        public IMongoCollection<Sinif> Siniflar => _database.GetCollection<Sinif>("Sinif");
    }
}
