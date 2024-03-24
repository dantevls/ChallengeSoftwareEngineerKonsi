using Nest;
using Services.KonsiCredit.ElasticSearchAppService;

namespace Services.KonsiCredit.ElasticSearchAppService;

public class ElasticSearchAppService<T> : IElasticSearchAppService<T> where T : class
{

    private readonly ElasticClient _elasticClient;
    
    public ElasticSearchAppService(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<string> CreateDocumentAsync<TDoc>(TDoc document) where TDoc : class
    {
        var response = await _elasticClient.IndexDocumentAsync(document);
        throw new NotImplementedException();
    }

    public async Task<T> GetDocumentAsync(int id)
    {
        var response = await _elasticClient.GetAsync(new DocumentPath<T>(id));
        throw new NotImplementedException();
    }
}