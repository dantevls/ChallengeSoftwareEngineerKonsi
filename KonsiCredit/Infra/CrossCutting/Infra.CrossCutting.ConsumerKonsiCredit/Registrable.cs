using Microsoft.Extensions.DependencyInjection;
using Services.KonsiCredit.AuthAppService;
using Services.KonsiCredit.BenefitsAppService;
using Services.KonsiCredit.QueueAppService;

namespace Infra.CrossCutting.ConsumerKonsiCredit;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IBenefitsAppService, BenefitsAppService>();
        serviceCollection.AddSingleton<IProducerQueueAppService, ProducerQueueAppService>();
        serviceCollection.AddSingleton<IAuthAppService, AuthAppService>();
    }
}