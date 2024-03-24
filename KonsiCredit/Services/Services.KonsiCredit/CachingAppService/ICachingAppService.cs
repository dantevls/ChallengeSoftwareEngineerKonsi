using Microsoft.Extensions.Caching.Distributed;

namespace Services.KonsiCredit.CachingAppService;

public interface ICachingAppService
{
    Task<string> GetDocumentAsync(string key);
    Task SetDocumentAsync(string key, string value, DistributedCacheEntryOptions? options = null);
}