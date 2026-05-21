using Com.Scm.Config;

namespace Com.Scm.MQTT
{
    public class MqttClientConfig : ClientConfig
    {
        public const string NAME = "MqttClient";

        public void Prepare(EnvConfig envConfig)
        {
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        public static MqttClientConfig Default => new MqttClientConfig
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
