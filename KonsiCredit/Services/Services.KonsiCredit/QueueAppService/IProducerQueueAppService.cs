namespace Services.KonsiCredit.QueueAppService;

public interface IProducerQueueAppService
{
    public Task EnqueueCpf();
}