using Infra.Data.KonsiCredit.CachingRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.ConsumerKonsiCredit.CachingRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICachingRepository, CachingRepository>();
        serviceCollection.AddStackExchangeRedisCache(c =>
        {
            c.InstanceName = "instance:";
            c.Configuration = "redis";
        });
    }
}