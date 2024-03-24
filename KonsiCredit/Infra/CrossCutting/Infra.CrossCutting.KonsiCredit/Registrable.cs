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
        //     serviceCollection.AddSingleton(sg =>
    //         {
    //             var service = sg.GetService<IRabbitMQAppService>();
    //             var factory = 
    //                 
    //         }
    //        
    //         );
     }
}