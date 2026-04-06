using Com.Scm.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Server
{
    public static class CorsExtension
    {
        public static void CorsSetup(this IServiceCollection services, CorsConfig corsConfig)
        {
            if (corsConfig == null)
            {
                return;
            }

            services.AddCors(options =>
            {
                options.AddPolicy(name: ScmEnv.SCM_CORS, policy =>
                {
                    if (corsConfig.AllowAnyOrigin)
                    {
                        policy.AllowAnyOrigin();
                    }
                    else
                    {
                        policy.WithOrigins(corsConfig.AllowedOrigins);

                        if (corsConfig.AllowCredentials)
                        {
                            policy.AllowCredentials();
                        }
                    }

                    if (corsConfig.AllowAnyMethod)
                    {
                        policy.AllowAnyMethod();
                    }
                    else
                    {
                        policy.WithMethods(corsConfig.AllowedMethods);
                    }

                    if (corsConfig.AllowAnyHeader)
                    {
                        policy.AllowAnyHeader();
                    }
                    else
                    {
                        policy.WithHeaders(corsConfig.AllowedHeaders);
                    }

                    policy.WithExposedHeaders(corsConfig.ExposedHeaders);

                    policy.SetPreflightMaxAge(TimeSpan.FromHours(corsConfig.PreflightMaxAge));
                });
            });
        }
    }
}
