using MQTTnet.Protocol;

namespace Com.Scm.MQTT
{
    /// <summary>
    /// MQTT 消息接收回调委托
    /// </summary>
    public delegate Task MqttMessageReceivedCallback(string topic, string payload, MqttQualityOfServiceLevel qos);

    /// <summary>
    /// MQTT 订阅者接口
    /// </summary>
    public interface IMqttSubscriber
    {
        /// <summary>
        /// 订阅主题
        /// </summary>
        Task SubscribeAsync(string topic,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 取消订阅主题
        /// </summary>
        Task UnsubscribeAsync(string topic, CancellationToken cancellationToken = default);

        /// <summary>
        /// 注册消息接收回调
        /// </summary>
        void OnMessageReceived(MqttMessageReceivedCallback callback);
    }
}
