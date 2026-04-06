namespace Com.Scm.RabbitMQ
{
    public interface IProducer
    {
        Task SendAsync(string exchange, string routingKey, object message, bool mandatory = false, CancellationToken cancellationToken = default);
    }
}
