using Microsoft.Extensions.DependencyInjection;
using Services.KonsiCredit.CachingAppService;

namespace Infra.CrossCutting.KonsiCredit.CachingRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICachingAppService, CachingAppService>();
        serviceCollection.AddStackExchangeRedisCache(c =>
        {
            c.InstanceName = "instance:";
            c.Configuration = "redis";
        });
    }
}
