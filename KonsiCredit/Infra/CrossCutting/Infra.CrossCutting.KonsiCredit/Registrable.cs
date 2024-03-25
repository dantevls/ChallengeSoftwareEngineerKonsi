using Microsoft.Extensions.DependencyInjection;
using Services.KonsiCredit.AuthAppService;
using Services.KonsiCredit.BenefitsAppService;

namespace Infra.CrossCutting.KonsiCredit;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthAppService, AuthAppService>();  
        serviceCollection.AddScoped<IBenefitsAppService, BenefitsAppService>();
    }
}