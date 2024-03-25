using Infra.Data.KonsiCredit.CachingRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.KonsiCredit.CachingRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICachingRepository, CachingRepository>();
        serviceCollection.AddStackExchangeRedisCache(c =>
        {
            c.InstanceName = "instance:";
            c.Configuration = "redis";
        });
    }
}
