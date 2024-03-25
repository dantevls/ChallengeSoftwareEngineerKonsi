using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Services.KonsiCredit.AuthAppService;
using Services.KonsiCredit.BenefitsAppService;
using static System.Console;

namespace Services.KonsiCredit.QueueAppService;

public class ProducerCpfQueue
{
    private readonly IAuthAppService _auth;
    private readonly IConfiguration _configuration;
    private readonly IBenefitsAppService _benefitsAppService;
    public ProducerCpfQueue(IConfiguration configuration, IAuthAppService auth, IBenefitsAppService benefitsAppService)
    {
        _configuration = configuration;
        _auth = auth;
        _benefitsAppService = benefitsAppService;
    }

    public async Task EnqueueCpf(IModel channel)
    {
        var userRoot = _configuration.GetSection("UserRoot").Value;
        var passRoot = _configuration.GetSection("PassRoot").Value;

        if (string.IsNullOrEmpty(userRoot) || string.IsNullOrEmpty(passRoot)) return;
        
        var token = _auth.GetUserToken(userRoot, passRoot).Result;
        
        if (token == null) return;
        
        var cpfList = new List<string>
        {
            "343.228.350-40",
            "869.230.000-41",
            "568.946.870-30",
            "433.510.120-12",
            "415.022.590-79"
        };

        foreach (var cpf in cpfList )
        {
            var user = await _benefitsAppService.GetUserBenefits(cpf, token);
            if (user == null) return;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(user.data));
            channel.BasicPublish(exchange: string.Empty,
                routingKey: "CpfQueue",
                basicProperties: null,
                body: body);
        }

        channel.QueueDeclare(queue: "CpfQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
}