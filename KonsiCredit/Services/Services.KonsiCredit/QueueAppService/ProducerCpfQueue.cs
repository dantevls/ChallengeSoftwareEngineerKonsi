using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Services.KonsiCredit.BenefitsAppService;

namespace Services.KonsiCredit.QueueAppService;

public class ProducerCpfQueue
{
    private readonly IBenefitsAppService _benefitsAppService;
    public ProducerCpfQueue(IBenefitsAppService benefitsAppService)
    {
        _benefitsAppService = benefitsAppService;
    }

    public async Task EnqueueCpf(IModel channel)
    {
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
            var user = await _benefitsAppService.GetUserBenefits(cpf);
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