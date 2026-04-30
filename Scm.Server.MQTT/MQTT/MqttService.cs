using Com.Scm.MQTT.Impl;
using MQTTnet.Protocol;

namespace Com.Scm.MQTT
{
    /// <summary>
    /// MQTT 综合服务（封装客户端，统一管理连接、发布、订阅）
    /// </summary>
    public class MqttService : IDisposable
    {
        private readonly MqttClientService _clientService;
        private bool _disposed;

        public MqttService(MqttConfig config)
        {
            _clientService = new MqttClientService(config);
        }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected => _clientService.IsConnected;

        /// <summary>
        /// 连接到 Broker
        /// </summary>
        public Task ConnectAsync(CancellationToken cancellationToken = default)
            => _clientService.ConnectAsync(cancellationToken);

        /// <summary>
        /// 断开连接
        /// </summary>
        public Task DisconnectAsync(CancellationToken cancellationToken = default)
            => _clientService.DisconnectAsync(cancellationToken);

        /// <summary>
        /// 发布字符串消息
        /// </summary>
        public Task PublishAsync(string topic, string payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            bool retain = false,
            CancellationToken cancellationToken = default)
            => _clientService.PublishAsync(topic, payload, qos, retain, cancellationToken);

        /// <summary>
        /// 发布字节消息
        /// </summary>
        public Task PublishAsync(string topic, byte[] payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            bool retain = false,
            CancellationToken cancellationToken = default)
            => _clientService.PublishAsync(topic, payload, qos, retain, cancellationToken);

        /// <summary>
        /// 订阅主题
        /// </summary>
        public Task SubscribeAsync(string topic,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            CancellationToken cancellationToken = default)
            => _clientService.SubscribeAsync(topic, qos, cancellationToken);

        /// <summary>
        /// 取消订阅主题
        /// </summary>
        public Task UnsubscribeAsync(string topic, CancellationToken cancellationToken = default)
            => _clientService.UnsubscribeAsync(topic, cancellationToken);

        /// <summary>
        /// 注册消息接收回调
        /// </summary>
        public void OnMessageReceived(MqttMessageReceivedCallback callback)
            => _clientService.OnMessageReceived(callback);

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
                _clientService?.Dispose();
            }
            _disposed = true;
        }

        ~MqttService()
        {
            Dispose(false);
        }
    }
}
