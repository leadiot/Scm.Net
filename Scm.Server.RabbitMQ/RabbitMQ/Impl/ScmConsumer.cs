using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Com.Scm.RabbitMQ.Impl
{
    public class ScmConsumer
    {
        private readonly IRabbitMQConnection _connection;

        public ScmConsumer(IRabbitMQConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task ReceiveAsync(string queueName, Func<IChannel, byte[], Task> callback, CancellationToken cancellationToken = default)
        {
            var channel = await _connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                try
                {
                    // 直接传递 model 和 body 给 callback，不需要转换
                    await callback(channel, body);
                }
                finally
                {
                    //await channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                }
            };
            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
            // Prevent the method from returning immediately
            await Task.Delay(-1, cancellationToken);
        }
    }
}
