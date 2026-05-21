using Com.Scm.Samples.Mqtt.Dvo;
using Com.Scm.Service;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Samples.Mqtt
{
    /// <summary>
    /// MQTT 示例服务 - API 接口层
    /// </summary>
    [ApiExplorerSettings(GroupName = "Samples")]
    public class SamplesMqttService : AppService
    {
        private readonly SamplesMqttHostedService _mqttHostedService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplesMqttService(SamplesMqttHostedService mqttHostedService)
        {
            _mqttHostedService = mqttHostedService;
        }

        /// <summary>
        /// 获取所有设备的最新温度数据
        /// </summary>
        [HttpGet]
        public List<TemperatureDataDvo> GetAllTemperatureData()
        {
            return _mqttHostedService.GetAllTemperatureData();
        }

        /// <summary>
        /// 获取指定设备的最新温度数据
        /// </summary>
        [HttpGet("{deviceId}")]
        public TemperatureDataDvo GetTemperatureData(string deviceId)
        {
            return _mqttHostedService.GetTemperatureData(deviceId);
        }

        /// <summary>
        /// 手动发送测试消息到指定主题
        /// </summary>
        [HttpPost]
        public async Task<bool> SendTestMessageAsync(string topic, string message)
        {
            return await _mqttHostedService.SendTestMessageAsync(topic, message);
        }
    }
}
