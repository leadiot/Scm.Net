using Com.Scm.Dto;

namespace Com.Scm.Samples.Mqtt
{
    /// <summary>
    /// 温度数据 DTO
    /// </summary>
    public class TemperatureDataDto : ScmDto
    {
        /// <summary>
        /// 设备 ID
        /// </summary>
        public string device_id { get; set; }

        /// <summary>
        /// 温度值
        /// </summary>
        public float temperature { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public long timestamp { get; set; }
    }
}
