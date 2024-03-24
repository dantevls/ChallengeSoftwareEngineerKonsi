namespace Services.KonsiCredit.ElasticSearchAppService;

public interface IElasticSearchAppService<T>
{
    public Task<string> CreateDocumentAsync<T>(T document) where T : class;
    public Task<T> GetDocumentAsync(int id);
}