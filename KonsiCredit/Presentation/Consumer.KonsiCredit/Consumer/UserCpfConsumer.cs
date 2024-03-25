using System.Text;
using Infra.Data.KonsiCredit.CachingRepository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.KonsiCredit.Consumer;

public class UserCpfConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ICachingRepository _cache;
    private readonly IModel _channel;
    private string? CpfQueue => _configuration.GetSection("CpfQueue").Value;
    
    public UserCpfConsumer(IConfiguration configuration, ICachingRepository cache)
    {
        _configuration = configuration;
        _cache = cache;
        _channel = CreateRabbitMqChannel();
        _channel.QueueDeclare(queue: CpfQueue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _channel.BasicAck(ea.DeliveryTag, false);
            var document = await _cache.GetDocumentAsync(message); 
            if (string.IsNullOrWhiteSpace(document))    
            {
                await _cache.SetDocumentAsync(message, Encoding.UTF8.GetString(body));
            }
        };
         _channel.BasicConsume(queue: CpfQueue,
             autoAck: false,
             consumer: consumer);
        
        return Task.CompletedTask;
    }

    private IModel CreateRabbitMqChannel()
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