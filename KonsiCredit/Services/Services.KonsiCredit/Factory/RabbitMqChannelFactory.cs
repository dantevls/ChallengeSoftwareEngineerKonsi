using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace Services.KonsiCredit.Factory;

public class RabbitMqChannelFactory : IRabbitMqChannelFactory
{
    private readonly IConfiguration _configuration;
    public RabbitMqChannelFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public IModel CreateChannel()
    {
        var credentials = _configuration.GetSection("RabbitMQ:Credentials").GetChildren().ToList();
        var factory = new ConnectionFactory
        {
            HostName = credentials?.FirstOrDefault(x => x.Key == "Host")?.Value?.ToString(),
            UserName = credentials?.FirstOrDefault(x => x.Key == "Username")?.Value?.ToString(),
            Password = credentials?.FirstOrDefault(x => x.Key == "Password")?.Value?.ToString(),
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        return channel;
    }
}