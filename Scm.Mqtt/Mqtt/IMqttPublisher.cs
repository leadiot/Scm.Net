using MQTTnet.Protocol;

namespace Com.Scm.Mqtt
{
    /// <summary>
    /// MQTT 发布者接口
    /// </summary>
    public interface IMqttPublisher
    {
        /// <summary>
        /// 发布消息（字符串内容）
        /// </summary>
        Task PublishAsync(string topic, string payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            bool retain = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 发布消息（字节内容）
        /// </summary>
        Task PublishAsync(string topic, byte[] payload,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            bool retain = false,
            CancellationToken cancellationToken = default);

        bool IsConnected { get; }

        Task ConnectAsync(CancellationToken cancellationToken = default);

        Task DisconnectAsync(CancellationToken cancellationToken = default);
    }
}
