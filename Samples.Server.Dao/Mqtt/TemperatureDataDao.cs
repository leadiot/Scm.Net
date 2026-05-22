using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Samples.Mqtt
{
    /// <summary>
    /// 温度数据 DAO
    /// </summary>
    [SugarTable("samples_temperature_data")]
    public class TemperatureDataDao : ScmDao
    {
        /// <summary>
        /// 设备 ID
        /// </summary>
        [SugarColumn(Length = 16)]
        public string device_id { get; set; }

        /// <summary>
        /// 温度值
        /// </summary>
        public float temperature { get; set; }

        /// <summary>
        /// 记录时间（Unix 毫秒戳）
        /// </summary>
        public long timestamp { get; set; }
    }
}
