using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Server;

namespace Com.Scm.MQTT.Impl
{
    /// <summary>
    /// MQTT Broker（内置服务端）
    /// 用于不依赖外部 Broker 的轻量级场景
    /// </summary>
    public class MqttBrokerService : IHostedService, IDisposable
    {
        private readonly MqttBrokerConfig _config;
        private MqttServer _server;
        private bool _disposed;

        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event Func<string, Task> ClientConnected;

        /// <summary>
        /// 客户端断开事件
        /// </summary>
        public event Func<string, Task> ClientDisconnected;

        /// <summary>
        /// 消息拦截事件（返回 false 则丢弃消息）
        /// </summary>
        public event Func<string, string, string, Task<bool>> MessageIntercepted;

        public MqttBrokerService(MqttBrokerConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// 启动 Broker
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(_config.Port);

            if (_config.MaxConnections > 0)
            {
                optionsBuilder = optionsBuilder.WithMaxPendingMessagesPerClient(_config.MaxConnections);
            }

            var options = optionsBuilder.Build();
            _server = new MqttServerFactory().CreateMqttServer(options);

            _server.ClientConnectedAsync += OnClientConnectedAsync;
            _server.ClientDisconnectedAsync += OnClientDisconnectedAsync;
            _server.InterceptingPublishAsync += OnInterceptingPublishAsync;

            if (_config.RequireAuthentication)
            {
                _server.ValidatingConnectionAsync += OnValidatingConnectionAsync;
            }

            await _server.StartAsync();
            Console.WriteLine($"[MQTT Broker] 启动成功，端口：{_config.Port}");
        }

        /// <summary>
        /// 停止 Broker
        /// </summary>
        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (_server != null)
            {
                await _server.StopAsync();
                Console.WriteLine("[MQTT Broker] 已停止");
            }
        }

        /// <summary>
        /// 获取当前连接的客户端列表
        /// </summary>
        public async Task<IList<MqttClientStatus>> GetConnectedClientsAsync()
        {
            if (_server == null) return new List<MqttClientStatus>();
            return await _server.GetClientsAsync();
        }

        /// <summary>
        /// 主动向客户端推送消息（服务端注入）
        /// </summary>
        public async Task InjectMessageAsync(string topic, string payload,
            MQTTnet.Protocol.MqttQualityOfServiceLevel qos = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce,
            bool retain = false)
        {
            if (_server == null) throw new InvalidOperationException("Broker 尚未启动");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(qos)
                .WithRetainFlag(retain)
                .Build();

            await _server.InjectApplicationMessage(new InjectedMqttApplicationMessage(message));
        }

        private Task OnClientConnectedAsync(ClientConnectedEventArgs e)
        {
            Console.WriteLine($"[MQTT Broker] 客户端已连接: {e.ClientId}");
            return ClientConnected?.Invoke(e.ClientId) ?? Task.CompletedTask;
        }

        private Task OnClientDisconnectedAsync(ClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"[MQTT Broker] 客户端已断开: {e.ClientId}，原因: {e.DisconnectType}");
            return ClientDisconnected?.Invoke(e.ClientId) ?? Task.CompletedTask;
        }

        private async Task OnInterceptingPublishAsync(InterceptingPublishEventArgs e)
        {
            Console.WriteLine($"[MQTT Broker] 消息 [{e.ClientId}] -> Topic={e.ApplicationMessage.Topic}");
            if (MessageIntercepted != null)
            {
                var payload = e.ApplicationMessage.ConvertPayloadToString() ?? string.Empty;
                var allow = await MessageIntercepted(e.ClientId, e.ApplicationMessage.Topic, payload);
                e.ProcessPublish = allow;
            }
        }

        private Task OnValidatingConnectionAsync(ValidatingConnectionEventArgs e)
        {
            if (_config.Users == null || _config.Users.Count == 0) return Task.CompletedTask;

            if (_config.Users.TryGetValue(e.UserName ?? string.Empty, out var pwd))
            {
                if (pwd == (e.Password ?? string.Empty))
                {
                    e.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.Success;
                    return Task.CompletedTask;
                }
            }

            e.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
            return Task.CompletedTask;
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
                _server?.Dispose();
            }
            _disposed = true;
        }

        ~MqttBrokerService()
        {
            Dispose(false);
        }
    }
}
