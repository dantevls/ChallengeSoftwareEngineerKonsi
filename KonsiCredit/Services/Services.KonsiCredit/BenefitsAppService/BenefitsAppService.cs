using System.Net;
using System.Text.RegularExpressions;
using Application.KonsiCredit.UserBenefitsViewModels;
using Infra.Data.KonsiCredit.CachingRepository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.KonsiCredit.AuthAppService;
using Services.KonsiCredit.ElasticSearchAppService;
using Services.KonsiCredit.HttpClientHandler;

namespace Services.KonsiCredit.BenefitsAppService;

public class BenefitsAppService : IBenefitsAppService
{
    private readonly IConfiguration _configuration;
    private readonly ICachingRepository _cache;
    private readonly IElasticSearchAppService _elasticSearch;
    private readonly IAuthAppService _authAppService;
    private readonly IHttpClientHandler _httpClientHandler;

    public BenefitsAppService(IConfiguration configuration, ICachingRepository cache, 
        IElasticSearchAppService elasticSearch, IAuthAppService authAppService, IHttpClientHandler httpClientHandler)
    {
        _configuration = configuration;
        _cache = cache;
        _elasticSearch = elasticSearch;
        _authAppService = authAppService;
        _httpClientHandler = httpClientHandler;
    }
    public async Task<UserBenefitsViewModel> GetUserBenefits(string cpf)
    {
        var formattedCpf = RemoverCaracteresEspeciais(cpf);
        if (string.IsNullOrEmpty(formattedCpf)) return new UserBenefitsViewModel();
        
        var document = await _cache.GetDocumentAsync(RemoverCaracteresEspeciais(formattedCpf));

        if (string.IsNullOrWhiteSpace(document))
        {
            var externalInssUri = _configuration.GetSection("ExternalInssApiUri").Value;
            var uri = $"{externalInssUri}/inss/consulta-beneficios?cpf={cpf}";
            try
            {
                var userRoot = _configuration.GetSection("UserRoot").Value;
                var passRoot = _configuration.GetSection("PassRoot").Value;
                if (userRoot == null || passRoot == null) return new UserBenefitsViewModel();
                var token = await _authAppService.GetUserToken(userRoot, passRoot);

                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                var response = await _httpClientHandler.GetAsync(request, token ?? null);
            
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var userBenefitsData = await response.Content.ReadAsStringAsync();
                    var viewModel = JsonConvert.DeserializeObject<UserBenefitsViewModel>(userBenefitsData);
                    if (viewModel?.data.cpf != null)
                    {
                        await _elasticSearch.CreateDocumentAsync(viewModel.data);
                        await _cache.SetDocumentAsync(viewModel.data.cpf, userBenefitsData);
                        return viewModel;
                    }  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
        }
        
        var userBenefitsModel = JsonConvert.DeserializeObject<UserBenefitsViewModel>(document);
        
        if (userBenefitsModel == null) return new UserBenefitsViewModel();
        await _elasticSearch.CreateDocumentAsync(userBenefitsModel.data);
        return userBenefitsModel;

    }
    
    
    static string RemoverCaracteresEspeciais(string input)
    {
        var pattern = "[^0-9]";
        var result = Regex.Replace(input, pattern, "");
        return result.Length != 11 ? string.Empty : result;
    }
}