using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using SmartCityHub.Models;
using SmartCityHub.Services;

public class IlanService
{
    private readonly IMongoCollection<Ilan> _ilanlar;
    private readonly IDistributedCache _cache;
    private readonly string _cacheKey = "TumIlanlar";

    public IlanService(MongoDbService mongoDbService, IDistributedCache cache)
    {
        _ilanlar = mongoDbService.Ilanlar;
        _cache = cache;
    }

    public async Task<List<Ilan>> HepsiniGetir()
    {
        var cachedIlanlar = await _cache.GetStringAsync(_cacheKey);
        if (!string.IsNullOrEmpty(cachedIlanlar))
        {
            return JsonSerializer.Deserialize<List<Ilan>>(cachedIlanlar)!;
        }

        var ilanlar = await _ilanlar.Find(_ => true).ToListAsync();
        var cacheOptions = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) };
        
        await _cache.SetStringAsync(_cacheKey, JsonSerializer.Serialize(ilanlar), cacheOptions);
        return ilanlar;
    }

    public async Task Ekle(Ilan YeniIlan, string kullaniciId)
    {
        YeniIlan.KullaniciId = kullaniciId;
        YeniIlan.Tarih = DateTime.Now;
        YeniIlan.Aktifmi = true;
        await _ilanlar.InsertOneAsync(YeniIlan);
        await _cache.RemoveAsync(_cacheKey);
    }

    public async Task Sil(string id)
    {
        var result = await _ilanlar.DeleteOneAsync(i => i.Id == id);
        if (result.IsAcknowledged && result.DeletedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task Guncelle(string id, Ilan GuncelIlan)
    {
        var result = await _ilanlar.ReplaceOneAsync(i => i.Id == id, GuncelIlan);
        if (result.IsAcknowledged && result.ModifiedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task<Ilan?> GetirById(string id)
    {
        return await _ilanlar.Find(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<Ilan>> KullanicininIlanlariniGetir(string kullaniciId)
    {
        return await _ilanlar.Find(i => i.KullaniciId == kullaniciId).ToListAsync();
    }
}