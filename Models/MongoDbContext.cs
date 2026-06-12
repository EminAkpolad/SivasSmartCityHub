using MongoDB.Driver;
using SmartCityHub.Models;

namespace SmartCityHub.Models
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDbSettings:Baglanti").Value;
            var databaseName = configuration.GetSection("MongoDbSettings:VeriTabani").Value;
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Duyuru> Duyurular => _database.GetCollection<Duyuru>("Duyuru");
        public IMongoCollection<Ilan> Ilanlar=>_database.GetCollection<Ilan>("Ilan");
    }
}