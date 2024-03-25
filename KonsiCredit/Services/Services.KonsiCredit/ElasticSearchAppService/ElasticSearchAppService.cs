using Application.KonsiCredit.UserBenefitsViewModels;
using Nest;

namespace Services.KonsiCredit.ElasticSearchAppService;

public class ElasticSearchAppService : IElasticSearchAppService
{
    private readonly ElasticClient _elasticClient;
    
    public ElasticSearchAppService(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<string> CreateDocumentAsync(Data document)
    {
        var response = await _elasticClient.IndexDocumentAsync(document);
        return response.Id;
    }

    public async Task<Data> GetDocumentAsync(int id)
    {
        var response = await _elasticClient.GetAsync(new DocumentPath<Data>(id));
        return response.Source;
    }
}