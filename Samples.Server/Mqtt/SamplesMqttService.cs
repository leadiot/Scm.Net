using Com.Scm.Dsa;
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
        private readonly SugarRepository<TemperatureDataDao> _thisRepository;
        private readonly SamplesMqttHostedService _mqttHostedService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SamplesMqttService(SugarRepository<TemperatureDataDao> thisRepository, SamplesMqttHostedService mqttHostedService)
        {
            _thisRepository = thisRepository;
            _mqttHostedService = mqttHostedService;
        }

        /// <summary>
        /// 获取所有设备的最新温度数据
        /// </summary>
        [HttpGet]
        public async Task<List<TemperatureDataDto>> GetAllTemperatureData()
        {
            return await _thisRepository.AsQueryable()
                .OrderBy(a => a.id, SqlSugar.OrderByType.Desc)
                .Take(100)
                .Select<TemperatureDataDto>()
                .ToListAsync();
        }

        /// <summary>
        /// 获取指定设备的最新温度数据
        /// </summary>
        [HttpGet("{deviceId}")]
        public async Task<TemperatureDataDto> GetTemperatureData(string deviceId)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.device_id == deviceId)
                .OrderBy(a => a.id, SqlSugar.OrderByType.Desc)
                .Select<TemperatureDataDto>()
                .FirstAsync();
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
