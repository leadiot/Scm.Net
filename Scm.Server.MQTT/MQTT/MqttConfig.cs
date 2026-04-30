namespace Com.Scm.MQTT
{
    /// <summary>
    /// MQTT 配置
    /// </summary>
    public class MqttConfig
    {
        /// <summary>
        /// Broker 服务器地址
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// Broker 端口号（默认 1883）
        /// </summary>
        public int Port { get; set; } = 1883;

        /// <summary>
        /// 客户端标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否清除会话（默认 true）
        /// </summary>
        public bool CleanSession { get; set; } = true;

        /// <summary>
        /// 保活间隔（秒，默认 60）
        /// </summary>
        public int KeepAlivePeriod { get; set; } = 60;

        /// <summary>
        /// 断线后自动重连间隔（秒，默认 5，设为 0 表示不重连）
        /// </summary>
        public int ReconnectDelay { get; set; } = 5;

        /// <summary>
        /// 默认配置
        /// </summary>
        public static MqttConfig Default => new MqttConfig
        {
            Host = "localhost",
            Port = 1883,
            ClientId = $"scm-{Guid.NewGuid():N}",
            CleanSession = true,
            KeepAlivePeriod = 60,
            ReconnectDelay = 5
        };
    }
}
