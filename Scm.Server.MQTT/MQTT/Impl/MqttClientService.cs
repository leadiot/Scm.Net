using MQTTnet;
using MQTTnet.Protocol;
using System.Text;

namespace Com.Scm.MQTT.Impl
{
    /// <summary>
    /// MQTT 客户端服务实现（发布 + 订阅）
    /// </summary>
    public class MqttClientService : IMqttPublisher, IMqttSubscriber, IDisposable
    {
        private readonly MqttConfig _config;
        private readonly IMqttClient _client;
        private MqttMessageReceivedCallback _messageCallback;
        private bool _disposed;

        public MqttClientService(MqttConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _client = new MqttClientFactory().CreateMqttClient();
            _client.ApplicationMessageReceivedAsync += OnApplicationMessageReceivedAsync;
            _client.DisconnectedAsync += OnDisconnectedAsync;
        }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _client.IsConnected;

        /// <summary>
        /// 连接到 Broker
        /// </summary>
        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            var builder = new MqttClientOptionsBuilder()
                .WithTcpServer(_config.Host, _config.Port)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(_config.KeepAlivePeriod))
                .WithCleanSession(_config.CleanSession);

            if (!string.IsNullOrWhiteSpace(_config.ClientId))
            {
                builder = builder.WithClientId(_config.ClientId);
            }

            if (!string.IsNullOrWhiteSpace(_config.UserName))
            {
                builder = builder.WithCredentials(_config.UserName, _config.Password);
            }

            var options = builder.Build();
            await _client.ConnectAsync(options, cancellationToken);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
        {
            if (_client.IsConnected)
            {
                await _client.DisconnectAsync(cancellationToken: cancellationToken);
            }
        }

        /// <inheritdoc />
        public async Task PublishAsync(string topic, string payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            bool retain = false,
            CancellationToken cancellationToken = default)
        {
            var bytes = Encoding.UTF8.GetBytes(payload ?? string.Empty);
            await PublishAsync(topic, bytes, qos, retain, cancellationToken);
        }

        /// <inheritdoc />
        public async Task PublishAsync(string topic, byte[] payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            bool retain = false,
            CancellationToken cancellationToken = default)
        {
            EnsureConnected();
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(qos)
                .WithRetainFlag(retain)
                .Build();

            await _client.PublishAsync(message, cancellationToken);
        }

        /// <inheritdoc />
        public async Task SubscribeAsync(string topic,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            CancellationToken cancellationToken = default)
        {
            EnsureConnected();
            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topic, qos)
                .Build();

            await _client.SubscribeAsync(subscribeOptions, cancellationToken);
        }

        /// <inheritdoc />
        public async Task UnsubscribeAsync(string topic, CancellationToken cancellationToken = default)
        {
            EnsureConnected();
            var unsubscribeOptions = new MqttClientUnsubscribeOptionsBuilder()
                .WithTopicFilter(topic)
                .Build();

            await _client.UnsubscribeAsync(unsubscribeOptions, cancellationToken);
        }

        /// <inheritdoc />
        public void OnMessageReceived(MqttMessageReceivedCallback callback)
        {
            _messageCallback = callback;
        }

        private async Task OnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            if (_messageCallback == null) return;

            var topic = e.ApplicationMessage.Topic;
            var payload = e.ApplicationMessage.ConvertPayloadToString() ?? string.Empty;
            var qos = e.ApplicationMessage.QualityOfServiceLevel;

            try
            {
                await _messageCallback(topic, payload, qos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MQTT] 消息处理异常 Topic={topic}: {ex.Message}");
            }
        }

        private async Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"[MQTT] 连接断开: {e.Reason}");

            if (_config.ReconnectDelay <= 0 || _disposed) return;

            await Task.Delay(TimeSpan.FromSeconds(_config.ReconnectDelay));

            try
            {
                await ConnectAsync();
                Console.WriteLine("[MQTT] 重连成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MQTT] 重连失败: {ex.Message}");
            }
        }

        private void EnsureConnected()
        {
            if (!_client.IsConnected)
            {
                throw new InvalidOperationException("MQTT 客户端未连接，请先调用 ConnectAsync()。");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _client?.Dispose();
            }
            _disposed = true;
        }

        ~MqttClientService()
        {
            Dispose(false);
        }
    }
}
