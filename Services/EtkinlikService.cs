using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using SmartCityHub.Models;
using SmartCityHub.Services;

public class EtkinlikService
{
    private readonly IMongoCollection<Etkinlik> _etkinlikler;
    private readonly IDistributedCache _cache;

    private readonly string _cacheKey="TumEtkinlikler";

    public EtkinlikService(MongoDbService mongoDbService,IDistributedCache cache)
    {
        _etkinlikler=mongoDbService.Etkinlikler;
        _cache=cache;
    }

    public async Task<List<Etkinlik>> HepsiniGetir()
    {
        var cachedEtkinlikler=await _cache.GetStringAsync(_cacheKey);
        if (!string.IsNullOrEmpty(cachedEtkinlikler))
        {
            return JsonSerializer.Deserialize<List<Etkinlik>>(cachedEtkinlikler);
        }
        var etkinlikler=await _etkinlikler.Find(_=>true).ToListAsync();

        var cacheOptions=new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow=TimeSpan.FromMinutes(10)
        };
        var JsonEtkinlikler=JsonSerializer.Serialize(etkinlikler);
        await _cache.SetStringAsync(_cacheKey,JsonEtkinlikler,cacheOptions);
        return etkinlikler;
    }

    public async Task Ekle(Etkinlik YeniEtkinlik ,string kullaniciId)
    {
        YeniEtkinlik.kullaniciId=kullaniciId;
        YeniEtkinlik.TarihSaat=DateTime.Now;
        await _etkinlikler.InsertOneAsync(YeniEtkinlik);
        await _cache.RemoveAsync(_cacheKey);
    }

    public async Task Sil(string id)
    {
        var result=await _etkinlikler.DeleteOneAsync(i=>i.Id==id);
        if(result.IsAcknowledged && result.DeletedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task Guncelle(Etkinlik GuncelEtkinlik)
    {
        var result=await _etkinlikler.ReplaceOneAsync(i=>i.Id==GuncelEtkinlik.Id,GuncelEtkinlik);
        if(result.IsAcknowledged && result.MatchedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task<Etkinlik?> GetirById(string id)
    {
        return await _etkinlikler.Find(i=>i.Id==id).FirstOrDefaultAsync();
    }
}