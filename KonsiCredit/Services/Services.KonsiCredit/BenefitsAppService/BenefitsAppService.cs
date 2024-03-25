using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
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
    private readonly IElasticSearchAppService _elasticSearch;
    private string? ExternalInssUri => _configuration.GetSection("ExternalInssApiUri").Value;

    public BenefitsAppService(IConfiguration configuration, ICachingAppService cache, 
        IElasticSearchAppService elasticSearch)
    {
        _configuration = configuration;
        _cache = cache;
        _elasticSearch = elasticSearch;
    }
    public async Task<UserBenefitsViewModel> GetUserBenefits(string cpf, string token)
    {
        var document = await _cache.GetDocumentAsync(RemoverCaracteresEspeciais(cpf));

        if (string.IsNullOrWhiteSpace(document))
        {
            using HttpClient client = new HttpClient();
            var uri = $"{ExternalInssUri}/inss/consulta-beneficios?cpf={cpf}";
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
                        var result = await _elasticSearch.CreateDocumentAsync(viewModel.data);
                        await _cache.SetDocumentAsync(viewModel.data.cpf, userBenefitsData);
                        return viewModel;
                    }  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        var userBenefitsModel = JsonConvert.DeserializeObject<UserBenefitsViewModel>(document);
        return userBenefitsModel ?? new UserBenefitsViewModel();
    }
    
    
    static string RemoverCaracteresEspeciais(string input)
    {
        var pattern = "[^0-9]";
        var result = Regex.Replace(input, pattern, "");
        return result;
    }
}