using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Com.Scm.RabbitMQ
{
    public class RabbitMQService
    {
        private readonly IRabbitMQConnection _connection;

        public RabbitMQService(IRabbitMQConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task SendAsync(string exchange, string routingKey, object message, bool mandatory = false, CancellationToken cancellationToken = default)
        {
            try
            {
                using var channel = await _connection.CreateChannelAsync();
                var mesjson = JsonSerializer.Serialize(message);
                Console.WriteLine("发送消息：" + mesjson);
                var body = Encoding.UTF8.GetBytes(mesjson);
                var properties = new BasicProperties
                {
                    Persistent = true // 设置消息持久化
                };
                await channel.BasicPublishAsync(exchange, routingKey, false, properties, body, cancellationToken);

            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"Operation was canceled: {ex.Message}");
                //throw; // Re-throw if you want to propagate the cancellation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                //throw; // Re-throw if you want to propagate the error
            }
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
