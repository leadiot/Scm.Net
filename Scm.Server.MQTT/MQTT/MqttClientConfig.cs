using Com.Scm.Config;

namespace Com.Scm.Mqtt
{
    public class MqttClientConfig : ClientConfig
    {
        public const string NAME = "MqttClient";

        public void Prepare(EnvConfig envConfig)
        {
            // 从配置文件读取设置，如果没有则使用默认值
            if (string.IsNullOrWhiteSpace(Host))
            {
                Host = "localhost";
            }
            if (Port <= 0)
            {
                Port = 1883;
            }
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        public static MqttClientConfig Default => new MqttClientConfig
        {
            Enabled = false,
            Host = "localhost",
            Port = 1883,
            ClientId = $"scm-server",
            CleanSession = true,
            KeepAlivePeriod = 60,
            ReconnectDelay = 5
        };
    }
}
