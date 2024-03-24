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
            Username = username,
            Password = password
        };

        try
        {
            var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync(url, content);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

}