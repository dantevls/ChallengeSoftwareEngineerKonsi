using Infra.Data.KonsiCredit.CachingRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.KonsiCredit.CachingRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var host = configuration.GetSection("RedisHost").Value;
        serviceCollection.AddScoped<ICachingRepository, CachingRepository>();
        serviceCollection.AddStackExchangeRedisCache(c =>
        {
            c.InstanceName = "instance:";
            c.Configuration = host;
        });
    }
}
