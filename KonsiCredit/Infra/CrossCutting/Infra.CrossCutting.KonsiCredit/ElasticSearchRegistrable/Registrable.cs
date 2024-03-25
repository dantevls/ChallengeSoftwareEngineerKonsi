using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Services.KonsiCredit.ElasticSearchAppService;


namespace Infra.CrossCutting.KonsiCredit.ElasticSearchRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var host = configuration.GetSection("ElasticSearchHost").Value;
        var settings = new ConnectionSettings(new Uri(host)).DefaultIndex("cpf");
        var client = new ElasticClient(settings);
        serviceCollection.AddSingleton(client);
        serviceCollection.AddScoped<IElasticSearchAppService, ElasticSearchAppService>();

    }
}