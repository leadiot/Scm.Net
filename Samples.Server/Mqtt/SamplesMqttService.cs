using Com.Scm.Mqtt;
using Com.Scm.Mqtt.Impl;
using Com.Scm.Samples.Mqtt.Dvo;
using Com.Scm.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using MQTTnet.Protocol;

namespace Com.Scm.Samples.Mqtt
{
    /// <summary>
    /// MQTT 示例服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Samples")]
    public class SamplesMqttService : AppService
    {
        private readonly IMqttPublisher _mqttPublisher;
        private readonly IMqttSubscriber _mqttSubscriber;
        private readonly ConcurrentDictionary<string, TemperatureDataDvo> _temperatureData;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplesMqttService(IMqttPublisher mqttPublisher, IMqttSubscriber mqttSubscriber)
        {
            _mqttPublisher = mqttPublisher;
            _mqttSubscriber = mqttSubscriber;
            _temperatureData = new ConcurrentDictionary<string, TemperatureDataDvo>();
            
            // 初始化订阅和回调
            InitializeMqttSubscriptions();
        }

        /// <summary>
        /// 初始化 MQTT 订阅
        /// </summary>
        private void InitializeMqttSubscriptions()
        {
            // 异步执行连接和订阅，避免阻塞启动
            Task.Run(async () =>
            {
                try
                {
                    // 尝试转换为具体实现类型，以便调用 ConnectAsync
                    if (_mqttSubscriber is MqttClientService clientService)
                    {
                        // 首先连接到 Broker
                        await clientService.ConnectAsync();
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[MQTT] 初始化失败: {ex.Message}");
                }
            });
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
        [HttpGet]
        public List<TemperatureDataDvo> GetAllTemperatureData()
        {
            return _temperatureData.Values.ToList();
        }

        /// <summary>
        /// 获取指定设备的最新温度数据
        /// </summary>
        [HttpGet("{deviceId}")]
        public TemperatureDataDvo GetTemperatureData(string deviceId)
        {
            _temperatureData.TryGetValue(deviceId, out var data);
            return data;
        }

        /// <summary>
        /// 手动发送测试消息到指定主题
        /// </summary>
        [HttpPost]
        public async Task<bool> SendTestMessageAsync(string topic, string message)
        {
            await _mqttPublisher.PublishAsync(topic, message, MqttQualityOfServiceLevel.AtLeastOnce);
            return true;
        }
    }
}
