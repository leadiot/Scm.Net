using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Com.Scm.RabbitMQ.Impl
{
    public class ScmProducer
    {
        private readonly IRabbitMQConnection _connection;

        public ScmProducer(IRabbitMQConnection connection)
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
    }
}
