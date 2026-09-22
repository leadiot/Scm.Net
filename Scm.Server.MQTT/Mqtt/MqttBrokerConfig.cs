using Com.Scm.Config;

namespace Com.Scm.Mqtt
{
    public class MqttBrokerConfig : BrokerConfig
    {
        public const string NAME = "MqttBroker";

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
