using Com.Scm.Samples.Mqtt;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Samples.Utils
{
    public static class SamplesServerUtils
    {
        public static void Setup(IServiceCollection services)
        {
            // 注册 MQTT Hosted Service，应用启动时自动运行
            services.AddSingleton<SamplesMqttHostedService>();
            services.AddHostedService(sp => sp.GetRequiredService<SamplesMqttHostedService>());
        }
    }
}
