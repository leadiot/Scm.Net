using RabbitMQ.Client;

namespace Com.Scm.RabbitMQ
{
    public interface IConsumer
    {
        Task ReceiveAsync(string queueName, Func<IChannel, byte[], Task> callback, CancellationToken cancellationToken = default);
    }
}
