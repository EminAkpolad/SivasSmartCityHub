using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using SmartCityHub.Models;
using SmartCityHub.Services;

public class DuyuruService
{
    private readonly IMongoCollection<Duyuru> _duyurular;
    private readonly IDistributedCache _cache;

    private readonly string _cacheKey = "TumDuyurular";

    public DuyuruService(MongoDbService mongoDbService, IDistributedCache cache)
    {
        _duyurular = mongoDbService.Duyurular;
        _cache = cache;
    }

    public async Task<List<Duyuru>> HepsiniGetir()
    {
        var cachedDuyurular = await _cache.GetStringAsync(_cacheKey);

        if (!string.IsNullOrEmpty(cachedDuyurular))
        {
            return JsonSerializer.Deserialize<List<Duyuru>>(cachedDuyurular);
        }

        var duyurular = await _duyurular.Find(_ => true).ToListAsync();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
        };

        var JsonDuyurular = JsonSerializer.Serialize(duyurular);

        await _cache.SetStringAsync(_cacheKey, JsonDuyurular, cacheOptions);
        return duyurular;
    }

    public async Task Ekle(Duyuru YeniDuyuru)
    {
        YeniDuyuru.OlusturmaTarihi = DateTime.Now;

        await _duyurular.InsertOneAsync(YeniDuyuru);
        await _cache.RemoveAsync(_cacheKey);
    }

    public async Task Sil(string id)
    {
        var result = await _duyurular.DeleteOneAsync(i => i.Id == id);

        if (result.IsAcknowledged && result.DeletedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task Guncelle(Duyuru GuncelDuyuru)
    {
        var result = await _duyurular.ReplaceOneAsync(i => i.Id == GuncelDuyuru.Id, GuncelDuyuru);

        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task<Duyuru?> GetirById(string id)
    {
        return await _duyurular.Find(i => i.Id == id).FirstOrDefaultAsync();
    }
}
