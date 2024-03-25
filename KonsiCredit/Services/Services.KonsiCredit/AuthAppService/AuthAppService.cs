using System.Net;
using System.Text;
using Application.KonsiCredit.AuthViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Services.KonsiCredit.AuthAppService;

public class AuthAppService : IAuthAppService
{
    private readonly IConfiguration _configuration;
    private string? GetExternalInssUri => _configuration.GetSection("ExternalInssApiUri").Value;

    public AuthAppService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string?> GetUserToken(string username, string password)
    {
        using HttpClient client = new HttpClient();
        
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
            var response = await client.PostAsync(url, httpContent, CancellationToken.None);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokenModel = JsonConvert.DeserializeObject<ResponseTokenViewModel>(result);
                return tokenModel?.data?.token;
            }
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
        
    }

}