using Application.KonsiCredit.UserBenefitsViewModels;

namespace Services.KonsiCredit.ElasticSearchAppService;

public interface IElasticSearchAppService
{
    public Task<string> CreateDocumentAsync(Data document);
    public Task<Data> GetDocumentAsync(int id);
}