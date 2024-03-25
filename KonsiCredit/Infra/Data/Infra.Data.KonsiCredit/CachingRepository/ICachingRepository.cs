using Microsoft.Extensions.Caching.Distributed;

namespace Infra.Data.KonsiCredit.CachingRepository;

public interface ICachingRepository
{
    Task<string> GetDocumentAsync(string key);
    Task SetDocumentAsync(string key, string value, DistributedCacheEntryOptions? options = null);
}
