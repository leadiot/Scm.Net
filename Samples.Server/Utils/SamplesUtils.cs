using Com.Scm.Samples.Book;
using Com.Scm.Samples.Mqtt;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Samples.Utils
{
    public static class SamplesServerUtils
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddScoped<IBookService, SamplesBookService>();
            
            // 注册 MQTT Hosted Service，应用启动时自动运行
            services.AddSingleton<SamplesMqttHostedService>();
            services.AddHostedService<SamplesMqttHostedService>(sp => sp.GetRequiredService<SamplesMqttHostedService>());
        }
    }
}
