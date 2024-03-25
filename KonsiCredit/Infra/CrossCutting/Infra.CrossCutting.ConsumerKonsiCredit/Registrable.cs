using Microsoft.Extensions.DependencyInjection;
using Services.KonsiCredit.AuthAppService;
using Services.KonsiCredit.BenefitsAppService;
using Services.KonsiCredit.HttpClientHandler;
using Services.KonsiCredit.QueueAppService;
using HttpClientHandler = Services.KonsiCredit.HttpClientHandler.HttpClientHandler;

namespace Infra.CrossCutting.ConsumerKonsiCredit;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IHttpClientHandler, HttpClientHandler>();
        serviceCollection.AddSingleton<IBenefitsAppService, BenefitsAppService>();
        serviceCollection.AddSingleton<IProducerQueueAppService, ProducerQueueAppService>();
        serviceCollection.AddSingleton<IAuthAppService, AuthAppService>();
    }
}