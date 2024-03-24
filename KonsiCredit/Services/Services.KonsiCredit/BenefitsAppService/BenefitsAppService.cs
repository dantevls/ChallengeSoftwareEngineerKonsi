using System.Net;
using System.Net.Http.Headers;
using Application.KonsiCredit.UserBenefitsViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.KonsiCredit.CachingAppService;
using Services.KonsiCredit.ElasticSearchAppService;

namespace Services.KonsiCredit.BenefitsAppService;

public class BenefitsAppService : IBenefitsAppService
{
    private readonly IConfiguration _configuration;
    private readonly ICachingAppService _cache;
    private IElasticSearchAppService<Data> _elasticSearch;
    private string? ExternalInssUri => _configuration.GetSection("ExternalInssApiUri").Value;

    public BenefitsAppService(IConfiguration configuration, ICachingAppService cache, IElasticSearchAppService<Data> elasticSearch)
    {
        _configuration = configuration;
        _cache = cache;
        _elasticSearch = elasticSearch;
    }
    public async Task<UserBenefitsViewModel> GetUserBenefits(string cpf, string token)
    {
        var document = await _cache.GetDocumentAsync(cpf);

        if (!string.IsNullOrWhiteSpace(document)) return new UserBenefitsViewModel();
        
        using HttpClient client = new HttpClient();
        var uri = $"{ExternalInssUri}/consulta-beneficios?cpf={cpf}";
        try
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(uri);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var userBenefitsData = await response.Content.ReadAsStringAsync();
                var viewModel = JsonConvert.DeserializeObject<UserBenefitsViewModel>(userBenefitsData);
                if (viewModel != null)
                {
                    await _cache.SetDocumentAsync(viewModel.Data.Cpf, userBenefitsData);
                    return viewModel;
                }  
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return new UserBenefitsViewModel();
        
    }
}