using RabbitMQ.Client;

namespace Services.KonsiCredit.Factory;

public interface IRabbitMqChannelFactory
{
    IModel CreateChannel();
}