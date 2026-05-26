using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using SmartCityHub.Models;
using SmartCityHub.Services;

public class OneriService
{
    private readonly IMongoCollection<Oneri> _oneriler;
    private readonly IDistributedCache _cache;

    private readonly string _cacheKey="TumOneriler";

    public OneriService(MongoDbService mongoDbService,IDistributedCache cache)
    {
        _oneriler=mongoDbService.Oneriler;
        _cache=cache;
    }

    public async Task<List<Oneri>> HepsiniGetir()
    {
        var cachedOneriler=await _cache.GetStringAsync(_cacheKey);

        if (!string.IsNullOrEmpty(cachedOneriler))
        {
            return JsonSerializer.Deserialize<List<Oneri>>(cachedOneriler);
        }

        var oneriler=await _oneriler.Find(_=>true).ToListAsync();

        var cacheOptions=new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow=TimeSpan.FromMinutes(10)
        };

        var JsonOneriler=JsonSerializer.Serialize(oneriler);
        await _cache.SetStringAsync(_cacheKey,JsonOneriler,cacheOptions);
        return oneriler;
    }

    public async Task Ekle(Oneri YeniOneri)
    {
        YeniOneri.Tarih=DateTime.Now;
        await _oneriler.InsertOneAsync(YeniOneri);
        await _cache.RemoveAsync(_cacheKey);
    }

    public async Task Sil(string id)
    {
        var result=await _oneriler.DeleteOneAsync(i=>i.Id==id);
        if(result.IsAcknowledged && result.DeletedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task Guncelle(Oneri GuncelOneri)
    {
        var result= await _oneriler.ReplaceOneAsync(i=>i.Id==GuncelOneri.Id,GuncelOneri);
        if(result.IsAcknowledged && result.MatchedCount > 0)
        {
            await _cache.RemoveAsync(_cacheKey);
        }
    }

    public async Task<Oneri?> GetirById(string id)
    {
        return await _oneriler.Find(i=>i.Id==id).FirstOrDefaultAsync();
    }
}