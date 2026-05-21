namespace Com.Scm.Samples.Mqtt.Dvo
{
    /// <summary>
    /// 温度数据 DTO
    /// </summary>
    public class TemperatureDataDvo
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
        public DateTime timestamp { get; set; }
    }
}
