using RabbitMQ.Client;

namespace Com.Scm.RabbitMQ
{
    public interface IRabbitMQConnection : IDisposable
    {
        Task<IChannel> CreateChannelAsync();
    }
}
