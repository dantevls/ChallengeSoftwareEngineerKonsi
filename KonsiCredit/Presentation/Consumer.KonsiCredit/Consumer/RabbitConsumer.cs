using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.KonsiCredit.AuthAppService;
using Services.KonsiCredit.BenefitsAppService;
using Services.KonsiCredit.CachingAppService;
using Services.KonsiCredit.QueueAppService;

namespace Consumer.KonsiCredit.Consumer;

public class RabbitConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ICachingAppService _cache;
    private readonly IModel _channel;
    private readonly IAuthAppService _authAppService;
    private readonly IBenefitsAppService _benefitsAppService;
    private string? CpfQueue => _configuration.GetSection("CpfQueue").Value;
    
    public RabbitConsumer(IConfiguration configuration, ICachingAppService cachingAppService, IAuthAppService authAppService, IBenefitsAppService benefitsAppService)
    {
        _configuration = configuration;
        _cache = cachingAppService;
        _authAppService = authAppService;
        _benefitsAppService = benefitsAppService;

        var credentials = _configuration.GetSection("RabbitMQ:Credentials").GetChildren().ToList();
        var factory = new ConnectionFactory
        {
            HostName = credentials?.FirstOrDefault(x => x.Key == "Host")?.Value?.ToString(),
            UserName = credentials?.FirstOrDefault(x => x.Key == "Username")?.Value?.ToString(),
            Password = credentials?.FirstOrDefault(x => x.Key == "Password")?.Value?.ToString(),
        };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();
        _channel.QueueDeclare(queue: CpfQueue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        var producer = new ProducerCpfQueue(_configuration, _authAppService, _benefitsAppService);
        producer.EnqueueCpf(_channel).Wait();
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
}