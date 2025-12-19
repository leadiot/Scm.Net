using Com.Scm.Api.Configure.Filters;
using Com.Scm.Api.Configure.Middleware;
using Com.Scm.Config;
using Com.Scm.Dsa.Dba.Sugar.UnitOfWork.Filters;
using Com.Scm.Email.Config;
using Com.Scm.Extensions;
using Com.Scm.Generator.Config;
using Com.Scm.Hubs;
using Com.Scm.Login.Otp;
using Com.Scm.Mapper;
using Com.Scm.Phone.Config;
using Com.Scm.Quartz;
using Com.Scm.Quartz.Config;
using Com.Scm.Samples;
using Com.Scm.Server;
using Com.Scm.Service;
using Com.Scm.Terminal;
using Com.Scm.Uid.Config;
using Com.Scm.Utils;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace Com.Scm.Api
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AppUtils.Init(builder.Configuration);

            // LOG配置
            Serilog.Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            var services = builder.Services;

            // 环境变量
            var envConfig = AppUtils.GetConfig<EnvConfig>(EnvConfig.NAME) ?? new EnvConfig();
            envConfig.Prepare(builder);
            services.AddSingleton(envConfig);

            // Sql配置
            var sqlConfig = AppUtils.GetConfig<SqlConfig>(SqlConfig.NAME);
            sqlConfig.Prepare(envConfig);
            services.SqlSugarSetup(sqlConfig);

            // Uid配置
            var uidConfig = AppUtils.GetConfig<UidConfig>(UidConfig.NAME);
            UidUtils.InitConfig(uidConfig);

            // 缓存配置
            services.CacheSetup(envConfig);

            // Swagger配置
            var swaggerConfig = AppUtils.GetConfig<SwaggerConfig>(SwaggerConfig.NAME);
            services.SwaggerSetup(swaggerConfig);

            // 数据配置
            var dataConfig = AppUtils.GetConfig<DataConfig>(DataConfig.NAME) ?? new DataConfig();
            dataConfig.Prepare(builder.Environment);
            services.AddSingleton(dataConfig);

            // 安全配置
            var secConfig = AppUtils.GetConfig<SecurityConfig>(SecurityConfig.NAME);
            secConfig.Prepare(builder.Environment);
            services.AddSingleton(secConfig);

            // 代码生成
            var genConfig = AppUtils.GetConfig<GeneratorConfig>(GeneratorConfig.NAME);
            genConfig.Prepare(envConfig);
            services.GeneratorSetup(genConfig);

            // Quartz
            var quartzConfig = AppUtils.GetConfig<QuartzConfig>(QuartzConfig.NAME) ?? new QuartzConfig();
            quartzConfig.Prepare(envConfig);
            services.QuartzSetup(quartzConfig);
            services.AddQuartzClassJobs();

            // EMail
            var emailConfig = AppUtils.GetConfig<EmailConfig>(EmailConfig.NAME) ?? new EmailConfig();
            emailConfig.Prepare(envConfig);
            services.AddSingleton(emailConfig);

            // Phone
            var phoneConfig = AppUtils.GetConfig<PhoneConfig>(PhoneConfig.NAME) ?? new PhoneConfig();
            phoneConfig.Prepare(envConfig);
            services.AddSingleton(phoneConfig);

            // Aiml
            var aimlConfig = AppUtils.GetConfig<AimlConfig>(AimlConfig.NAME) ?? new AimlConfig();
            aimlConfig.Prepare(envConfig);
            services.AddSingleton(aimlConfig);

            // Oidc
            var oidcConfig = AppUtils.GetConfig<OidcConfig>(OidcConfig.NAME) ?? new OidcConfig();
            oidcConfig.Prepare(envConfig);
            services.AddSingleton(oidcConfig);

            // Otp
            var otpConfig = AppUtils.GetConfig<OtpConfig>(OtpConfig.NAME) ?? new OtpConfig();
            otpConfig.Prepare(envConfig);
            services.AddSingleton(otpConfig);

            // Cors
            var corsConfig = AppUtils.GetConfig<CorsConfig>(CorsConfig.NAME);
            if (corsConfig != null)
            {
                corsConfig.Prepare(envConfig);
            }

            services.AddScoped<IUserService, ScmUserService>();
            services.AddScoped<ILogService, ScmLogService>();
            services.AddScoped<IDicService, ScmDicService>();
            services.AddScoped<ICfgService, ScmCfgService>();
            services.AddScoped<ISecService, ScmSecService>();
            services.AddScoped<ICatService, ScmCatService>();
            services.AddScoped<ITagService, ScmTagService>();
            services.AddScoped<IFlowService, ScmFlowService>();
            services.AddScoped<ITerminalHolder, ScmTerminalHolder>();

            // 自定义服务
            SamplesServerUtils.Setup(services);

            // 全局过滤
            services.AddControllers(options =>
            {
                options.Filters.Add<AopActionFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<UnitOfWorkFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            }).NewtonJsonSetup();

            // 接口配置
            var apiConfig = AppUtils.GetConfig<DllConfig>(DllConfig.NAME);
            apiConfig.Prepare(builder.Environment);
            services.RegisterServices(apiConfig);

            // Jwt Config
            services.SetupJwt(envConfig);

            // 跨域访问
            services.CorsSetup(corsConfig);

            // SignalR
            services.AddSignalR();

            // Mapper
            services.AddMapperProfile();

            var app = builder.Build();

            AppUtils.ServiceProvider = app.Services;

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerSetup(swaggerConfig);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!string.IsNullOrEmpty(envConfig.DataUri))
            {
                app.UseFileServer(new FileServerOptions
                {
                    FileProvider = new PhysicalFileProvider(envConfig.DataDir),
                    RequestPath = envConfig.DataUri,
                });
            }

            app.UseRouting();

            // 跨域设置
            if (corsConfig != null)
            {
                if (corsConfig.GlobalCors)
                {
                    app.UseCors(ScmEnv.SCM_CORS);
                }
                else
                {
                    app.UseCors();
                }
            }

            app.Use(next => async context =>
            {
                context.Request.EnableBuffering();
                await next(context);
            });

            // 认证
            app.UseAuthentication();
            // 授权
            app.UseAuthorization();

            // 中间件异常处理
            app.UseMiddleware<ExceptionMiddleware>();
            // Jwt中间件处理
            app.UseMiddleware<JwtMiddleware>();

            app.UseQuartz();

            app.MapControllers().RequireAuthorization();
            app.MapHub<ScmHub>("/scmhub");

            app.Run();
        }
    }
}