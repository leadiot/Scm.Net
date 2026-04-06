using Com.Scm.RabbitMQ.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Com.Scm.RabbitMQ
{
    public static class RabbitMQExtension
    {
        public static void SetupRabbitMQ(this IServiceCollection services, RabbitMQConfig config)
        {
            services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RabbitMQConfig>>().Value;
                var factory = new ConnectionFactory() { HostName = options.Host, Port = options.Port, UserName = options.UserName, Password = options.Password };
                return new RabbitMQConnection(factory);
            });

            services.AddSingleton<RabbitMQService>();
        }
    }
}
