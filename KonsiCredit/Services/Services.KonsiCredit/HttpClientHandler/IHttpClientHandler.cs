namespace Services.KonsiCredit.HttpClientHandler;

public interface IHttpClientHandler
{
    Task<HttpResponseMessage> GetAsync(HttpRequestMessage message, string? token = null);

    Task<HttpResponseMessage> PostAsync(HttpRequestMessage message);
}