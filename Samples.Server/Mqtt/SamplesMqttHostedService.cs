using Com.Scm.Mqtt;
using Com.Scm.Mqtt.Impl;
using Com.Scm.Samples.Mqtt.Dvo;
using Microsoft.Extensions.Hosting;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Com.Scm.Samples.Mqtt
{
    /// <summary>
    /// MQTT 示例后台服务 - 应用启动时自动连接和订阅
    /// </summary>
    public class SamplesMqttHostedService : IHostedService, IDisposable
    {
        private readonly IMqttPublisher _mqttPublisher;
        private readonly IMqttSubscriber _mqttSubscriber;
        private readonly ConcurrentDictionary<string, TemperatureDataDvo> _temperatureData;
        private bool _disposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplesMqttHostedService(IMqttPublisher mqttPublisher, IMqttSubscriber mqttSubscriber)
        {
            _mqttPublisher = mqttPublisher;
            _mqttSubscriber = mqttSubscriber;
            _temperatureData = new ConcurrentDictionary<string, TemperatureDataDvo>();
        }

        /// <summary>
        /// 服务启动时执行
        /// </summary>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("[MQTT Hosted Service] 正在初始化...");
            await InitializeMqttSubscriptions(cancellationToken);
        }

        /// <summary>
        /// 服务停止时执行
        /// </summary>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("[MQTT Hosted Service] 正在停止...");
            
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
                    // 首先连接到 Broker
                    await clientService.ConnectAsync(cancellationToken);
                    Console.WriteLine("[MQTT] 客户端连接成功");
                }

                // 订阅温度主题
                await _mqttSubscriber.SubscribeAsync("sensors/temperature/#", MqttQualityOfServiceLevel.AtLeastOnce);
                Console.WriteLine("[MQTT] 已订阅主题：sensors/temperature/#");
                
                // 注册消息接收回调
                _mqttSubscriber.OnMessageReceived(async (topic, payload, qos) =>
                {
                    await HandleMessageAsync(topic, payload, qos);
                });

                Console.WriteLine("[MQTT Hosted Service] 初始化完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MQTT] 初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        private async Task HandleMessageAsync(string topic, string payload, MqttQualityOfServiceLevel qos)
        {
            try
            {
                Console.WriteLine($"[MQTT] 收到消息：Topic={topic}, Payload={payload}");
                
                // 解析温度数据
                var temperatureData = JsonConvert.DeserializeObject<TemperatureDataDvo>(payload);
                if (temperatureData != null)
                {
                    // 保存数据
                    _temperatureData[temperatureData.device_id] = temperatureData;
                    
                    Console.WriteLine($"[MQTT] 接收到温度数据: 设备={temperatureData.device_id}, 温度={temperatureData.temperature}°C");
                    
                    // 发送响应消息
                    await SendResponseAsync(temperatureData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MQTT] 处理消息异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 发送响应消息
        /// </summary>
        private async Task SendResponseAsync(TemperatureDataDvo data)
        {
            // 检查温度是否在正常范围内 (示例: 0-40°C)
            bool isNormal = data.temperature >= 0 && data.temperature <= 40;
            
            var response = new
            {
                device_id = data.device_id,
                status = isNormal ? "ok" : "warning",
                message = isNormal ? "温度正常" : "温度异常",
                timestamp = DateTime.Now
            };
            
            string responseTopic = $"sensors/temperature/{data.device_id}/response";
            string responsePayload = JsonConvert.SerializeObject(response);
            
            try
            {
                await _mqttPublisher.PublishAsync(responseTopic, responsePayload, MqttQualityOfServiceLevel.AtLeastOnce);
                Console.WriteLine($"[MQTT] 已发送响应：Topic={responseTopic}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MQTT] 发送响应失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取所有设备的最新温度数据
        /// </summary>
        public List<TemperatureDataDvo> GetAllTemperatureData()
        {
            return _temperatureData.Values.ToList();
        }

        /// <summary>
        /// 获取指定设备的最新温度数据
        /// </summary>
        public TemperatureDataDvo GetTemperatureData(string deviceId)
        {
            _temperatureData.TryGetValue(deviceId, out var data);
            return data;
        }

        /// <summary>
        /// 手动发送测试消息到指定主题
        /// </summary>
        public async Task<bool> SendTestMessageAsync(string topic, string message)
        {
            await _mqttPublisher.PublishAsync(topic, message, MqttQualityOfServiceLevel.AtLeastOnce);
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
