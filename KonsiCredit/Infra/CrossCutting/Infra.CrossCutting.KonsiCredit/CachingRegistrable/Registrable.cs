using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.KonsiCredit.CachingAppService;

namespace Infra.CrossCutting.KonsiCredit.CachingRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICachingAppService, CachingAppService>();
        serviceCollection.AddStackExchangeRedisCache(c =>
        {
            c.InstanceName = "instance:";
            c.Configuration = "redis";
        });
    }
}
