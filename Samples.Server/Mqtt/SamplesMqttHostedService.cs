using Com.Scm.Mqtt;
using Com.Scm.Mqtt.Impl;
using Com.Scm.Response;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.Extensions.Hosting;
using MQTTnet.Protocol;
using SqlSugar;

namespace Com.Scm.Samples.Mqtt
{
    /// <summary>
    /// MQTT 示例后台服务 - 应用启动时自动连接和订阅
    /// </summary>
    public class SamplesMqttHostedService : IHostedService, IDisposable
    {
        private readonly ISqlSugarClient _sqlClient;
        private readonly IMqttPublisher _mqttPublisher;
        private readonly IMqttSubscriber _mqttSubscriber;

        /// <summary>
        /// MQTT 主题常量
        /// </summary>
        private const string TemperatureTopic = "sensors/temperature/device";
        /// <summary>
        /// 用于温度传感器响应消息的主题格式字符串。
        /// </summary>
        /// <remarks>使用时将 {0} 替换为传感器标识符（例如 ID 或名称）。</remarks>
        private const string ResponseTopicFormat = "sensors/temperature/{0}/response";

        private bool _disposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplesMqttHostedService(ISqlSugarClient sqlClient, IMqttPublisher mqttPublisher, IMqttSubscriber mqttSubscriber)
        {
            _sqlClient = sqlClient;
            _mqttPublisher = mqttPublisher;
            _mqttSubscriber = mqttSubscriber;
        }

        /// <summary>
        /// 服务启动时执行
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            LogUtils.Debug("[MQTT Hosted Service] 正在初始化...");
            await InitializeMqttSubscriptions(cancellationToken);
        }

        /// <summary>
        /// 服务停止时执行
        /// </summary>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            LogUtils.Debug("[MQTT Hosted Service] 正在停止...");

            if (_mqttSubscriber is MqttClientService clientService)
            {
                await clientService.DisconnectAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 初始化 MQTT 订阅
        /// </summary>
        private async Task InitializeMqttSubscriptions(CancellationToken cancellationToken)
        {
            try
            {
                // 尝试转换为具体实现类型，以便调用 ConnectAsync
                if (_mqttSubscriber is MqttClientService clientService)
                {
                    // 短暂等待 Broker 完全就绪（HostedService 按顺序启动，但端口绑定可能需要时间）
                    await Task.Delay(100, cancellationToken);

                    // 首先连接到 Broker
                    await clientService.ConnectAsync(cancellationToken);
                    LogUtils.Debug("[MQTT] 客户端连接成功");
                }

                // 订阅温度主题
                await _mqttSubscriber.SubscribeAsync(TemperatureTopic, MqttQualityOfServiceLevel.ExactlyOnce);
                LogUtils.Debug("[MQTT] 已订阅主题：" + TemperatureTopic);

                // 注册消息接收回调
                _mqttSubscriber.OnMessageReceived(HandleMessageAsync);

                LogUtils.Debug("[MQTT Hosted Service] 初始化完成");
            }
            catch (Exception ex)
            {
                LogUtils.Debug($"[MQTT] 初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        private async Task HandleMessageAsync(string topic, string payload, MqttQualityOfServiceLevel qos)
        {
            try
            {
                LogUtils.Debug($"[MQTT] 收到消息：Topic={topic}, Payload={payload}");

                // 解析温度数据
                var temperatureData = payload.AsJsonObject<TemperatureRequest>();
                if (temperatureData != null)
                {
                    var dao = temperatureData.Adapt<TemperatureDataDao>();
                    dao.PrepareCreate(UserDto.SYS_ID);
                    await _sqlClient.InsertAsync(dao);

                    LogUtils.Debug($"[MQTT] 接收到温度数据: 设备={temperatureData.device_id}, 温度={temperatureData.temperature}°C");

                    // 发送响应消息
                    await SendResponseAsync(temperatureData);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Debug($"[MQTT] 处理消息异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送响应消息
        /// </summary>
        private async Task SendResponseAsync(TemperatureRequest data)
        {
            // 检查温度是否在正常范围内 (示例: 0-40°C)
            bool isNormal = data.temperature >= 0 && data.temperature <= 40;

            var result = new TemperatureResult
            {
                device_id = data.device_id,
                status = isNormal ? 0 : 1,
                message = isNormal ? "温度正常" : "温度异常"
            };
            var response = ScmAppResponse.SetSuccess(result);

            string responseTopic = string.Format(ResponseTopicFormat, data.device_id);
            string responsePayload = TextUtils.ToJsonString(response);

            try
            {
                await _mqttPublisher.PublishAsync(responseTopic, responsePayload, MqttQualityOfServiceLevel.AtLeastOnce);
                LogUtils.Debug($"[MQTT] 已发送响应：Topic={responseTopic}");
            }
            catch (Exception ex)
            {
                LogUtils.Debug($"[MQTT] 发送响应失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 手动发送测试消息到指定主题
        /// </summary>
        public async Task<bool> SendTestMessageAsync(string topic, string message)
        {
            await _mqttPublisher.PublishAsync(topic, message, MqttQualityOfServiceLevel.ExactlyOnce);
            return true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
        }

        ~SamplesMqttHostedService()
        {
            Dispose(false);
        }
    }
}
