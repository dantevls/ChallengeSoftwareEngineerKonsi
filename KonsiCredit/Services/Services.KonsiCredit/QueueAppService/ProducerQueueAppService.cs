using System.Text;
using Application.KonsiCredit.UserBenefitsViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Services.KonsiCredit.BenefitsAppService;

namespace Services.KonsiCredit.QueueAppService;

public class ProducerQueueAppService : IProducerQueueAppService
{
    private readonly IConfiguration _configuration;
    private readonly IModel _channel;
    private readonly IBenefitsAppService _benefitsAppService;
    private string? CpfQueue => _configuration.GetSection("CpfQueue").Value;

    public ProducerQueueAppService(IConfiguration configuration, 
        IBenefitsAppService benefitsAppService)
    {
        _configuration = configuration;
        _benefitsAppService = benefitsAppService;
        _channel = CreateRabbitMqChannel();
        _channel.QueueDeclare(queue: CpfQueue,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public async Task EnqueueCpf()
    {
        var cpfList = new List<string>
        {
            "343.228.350-40",
            "869.230.000-41",
            "568.946.870-30",
            "433.510.120-12",
            "415.022.590-79",
            "415.022.590-79",
            "568.946.870-30",
            "869.230.000-41"
        };

        foreach (var cpf in cpfList)
        {
            var user = await _benefitsAppService.GetUserBenefits(cpf);
            if (user == new UserBenefitsViewModel()) return;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user.data));
            _channel.BasicPublish(exchange: string.Empty,
                routingKey: "CpfQueue",
                basicProperties: null,
                body: body);
        }

        _channel.QueueDeclare(queue: "CpfQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
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