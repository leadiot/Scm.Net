namespace Com.Scm.MQTT
{
    /// <summary>
    /// MQTT 内置 Broker 配置
    /// </summary>
    public class MqttBrokerConfig
    {
        /// <summary>
        /// 监听端口（默认 1883）
        /// </summary>
        public int Port { get; set; } = 1883;

        /// <summary>
        /// 最大待处理消息数（每个客户端，0 表示不限制）
        /// </summary>
        public int MaxConnections { get; set; } = 0;

        /// <summary>
        /// 是否启用身份验证
        /// </summary>
        public bool RequireAuthentication { get; set; } = false;

        /// <summary>
        /// 用户名/密码字典（仅在 RequireAuthentication=true 时生效）
        /// </summary>
        public Dictionary<string, string> Users { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 默认配置（不需要认证，端口 1883）
        /// </summary>
        public static MqttBrokerConfig Default => new MqttBrokerConfig
        {
            Port = 1883,
            RequireAuthentication = false
        };
    }
}
