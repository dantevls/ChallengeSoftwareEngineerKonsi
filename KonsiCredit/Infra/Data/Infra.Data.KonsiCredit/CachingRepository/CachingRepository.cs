using Microsoft.Extensions.Caching.Distributed;

namespace Infra.Data.KonsiCredit.CachingRepository;

public class CachingRepository : ICachingRepository
{
    private readonly IDistributedCache _distributedCache;
    
    private readonly DistributedCacheEntryOptions _options;
    
    public CachingRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4),
            SlidingExpiration = TimeSpan.MaxValue
        };
    }
    
    public async Task<string> GetDocumentAsync(string id)
    {
        var document = await _distributedCache.GetStringAsync(id);

        return document ?? string.Empty;
    }

    public async Task SetDocumentAsync(string key, string value, DistributedCacheEntryOptions? options)
    {
        await _distributedCache.SetStringAsync(key, value, options ?? _options);
    }
}