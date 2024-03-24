using Application.KonsiCredit.UserBenefitsViewModels;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using Services.KonsiCredit.ElasticSearchAppService;


namespace Infra.CrossCutting.KonsiCredit.ElasticSearchRegistrable;

public class Registrable
{
    public static void RegisterServices(IServiceCollection serviceCollection)
    {
        var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200")).DefaultIndex("cpf");
        var client = new ElasticsearchClient(settings);
        serviceCollection.AddSingleton(client);
        serviceCollection.AddScoped<IElasticSearchAppService<Data>, ElasticSearchAppService<Data>>();
    }
}