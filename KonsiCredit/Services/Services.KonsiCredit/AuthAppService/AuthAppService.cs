using System.Net;
using System.Text;
using Application.KonsiCredit.AuthViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.KonsiCredit.HttpClientHandler;

namespace Services.KonsiCredit.AuthAppService;

public class AuthAppService : IAuthAppService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientHandler _httpClientHandler;
    private string? GetExternalInssUri => _configuration.GetSection("ExternalInssApiUri").Value;

    public AuthAppService(IConfiguration configuration, IHttpClientHandler httpClientHandler)
    {
        _configuration = configuration;
        _httpClientHandler = httpClientHandler;
    }

    public async Task<string?> GetUserToken(string username, string password)
    {
        var url = GetExternalInssUri + "/token";
        var body = new UserViewModel
        {
            username = username,
            password = password
        };

        try
        {
            var content = JsonConvert.SerializeObject(body);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = httpContent
            };
            var response = await _httpClientHandler.PostAsync(message);
            
            if (response.StatusCode != HttpStatusCode.OK) return null;
            
            var result = await response.Content.ReadAsStringAsync();
            var tokenModel = JsonConvert.DeserializeObject<ResponseTokenViewModel>(result);
            return tokenModel?.data?.token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
        
    }

}