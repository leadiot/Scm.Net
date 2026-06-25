using Com.Scm.Config;

namespace Com.Scm.Mqtt
{
    public class MqttBrokerConfig : BrokerConfig
    {
        public const string NAME = "MqttBroker";

        /// <summary>
        /// 是否启用内置 Broker（默认 false）
        /// 设为 false 时仅使用客户端连接外部 Broker
        /// </summary>
        public bool Enabled { get; set; } = true;

        public void Prepare(EnvConfig envConfig)
        {
        }

        /// <summary>
        /// 默认配置（不需要认证，端口 1883）
        /// </summary>
        public static MqttBrokerConfig Default => new MqttBrokerConfig
        {
            Enabled = false,
            Port = 1883,
            RequireAuthentication = false
        };
    }
}
