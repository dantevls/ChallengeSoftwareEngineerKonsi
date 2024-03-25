using Microsoft.Extensions.DependencyInjection;
using Nest;
using Services.KonsiCredit.ElasticSearchAppService;

namespace Infra.CrossCutting.ConsumerKonsiCredit.ElasticSearchRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200/")).DefaultIndex("cpf");
        var client = new ElasticClient(settings);
        serviceCollection.AddSingleton(client);
        serviceCollection.AddSingleton<IElasticSearchAppService, ElasticSearchAppService>();

    }
}