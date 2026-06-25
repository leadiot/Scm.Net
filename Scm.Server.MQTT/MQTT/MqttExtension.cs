using Com.Scm.Mqtt.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Com.Scm.Mqtt
{
    /// <summary>
    /// MQTT 依赖注入扩展方法
    /// </summary>
    public static class MqttExtension
    {
        /// <summary>
        /// 注册 MQTT 内置 Broker 服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <param name="config">Broker 配置</param>
        public static IServiceCollection SetupMqttBroker(this IServiceCollection services, MqttBrokerConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            if (config.Enabled)
            {
                services.AddSingleton<BrokerConfig>(config);
                //services.AddSingleton<MqttBrokerService>();
                services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<MqttBrokerService>());
            }

            return services;
        }

        /// <summary>
        /// 注册 MQTT 客户端服务（发布 + 订阅）
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <param name="config">客户端配置</param>
        public static IServiceCollection SetupMqttClient(this IServiceCollection services, MqttClientConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            services.AddSingleton<ClientConfig>(config);
            //services.AddSingleton<MqttClientService>();
            services.AddSingleton<IMqttPublisher>(sp => sp.GetRequiredService<MqttClientService>());
            services.AddSingleton<IMqttSubscriber>(sp => sp.GetRequiredService<MqttClientService>());
            //services.AddSingleton<MqttService>();

            return services;
        }

        /// <summary>
        /// 同时注册 MQTT Broker + 客户端（全内置模式）
        /// </summary>
        public static IServiceCollection SetupMqtt(this IServiceCollection services,
            MqttBrokerConfig brokerConfig = null,
            MqttClientConfig clientConfig = null)
        {
            brokerConfig ??= MqttBrokerConfig.Default;
            clientConfig ??= MqttClientConfig.Default;

            // 根据配置决定是否启动内置 Broker
            if (brokerConfig.Enabled)
            {
                // 客户端自动连接本地 Broker
                clientConfig.Host = "localhost";
                clientConfig.Port = brokerConfig.Port;
                services.SetupMqttBroker(brokerConfig);
            }

            services.SetupMqttClient(clientConfig);

            return services;
        }
    }
}
