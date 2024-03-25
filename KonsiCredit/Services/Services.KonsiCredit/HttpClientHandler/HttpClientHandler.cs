using System.Net.Http.Headers;

namespace Services.KonsiCredit.HttpClientHandler;

public class HttpClientHandler : IHttpClientHandler
{
    private readonly HttpClient _httpClient;

    public HttpClientHandler()
    {
        _httpClient = new HttpClient();
    }
    public async Task<HttpResponseMessage> GetAsync(HttpRequestMessage message, string? token)
    {
        if (!string.IsNullOrEmpty(token)) _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(message.RequestUri);
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage message)
    {
        var response = await _httpClient.PostAsync(message.RequestUri, message.Content);
        return response;
    }
}