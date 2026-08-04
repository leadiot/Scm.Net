using Com.Scm.Request;

namespace Com.Scm.Samples.Temperature.Rnr
{
    /// <summary>
    /// 温度数据 DTO
    /// </summary>
    public class SamplesTemperatureRequest : ScmRequest
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
