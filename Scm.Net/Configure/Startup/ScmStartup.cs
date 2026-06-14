using Com.Scm.Config;
using Com.Scm.Configure.Filters;
using Com.Scm.Configure.Middleware;
using Com.Scm.Dsa;
using Com.Scm.Dsa.Dba.Sugar.UnitOfWork.Filters;
using Com.Scm.Email.Config;
using Com.Scm.Extensions;
using Com.Scm.Generator.Config;
using Com.Scm.Helper;
using Com.Scm.Holder;
using Com.Scm.Hubs;
using Com.Scm.Image.ImageSharp;
using Com.Scm.Login.Otp;
using Com.Scm.Mapper;
using Com.Scm.Mqtt;
using Com.Scm.Phone.Config;
using Com.Scm.Quartz;
using Com.Scm.Quartz.Config;
using Com.Scm.Samples.Helper;
using Com.Scm.Samples.Utils;
using Com.Scm.Server;
using Com.Scm.Service;
using Com.Scm.Uid.Config;
using Com.Scm.Utils;
using Microsoft.Extensions.FileProviders;
using Serilog;
using SqlSugar;

namespace Com.Scm.Configure.Startup
{
    public static class ScmStartup
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            AppUtils.Init(builder.Configuration);

            // LOG配置
            Serilog.Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            LogUtils.Info("正在启动系统...");

            var services = builder.Services;

            // 环境变量
            LogUtils.Info("正在进行环境配置...");
            var envConfig = AppUtils.GetConfig<EnvConfig>(EnvConfig.NAME) ?? new EnvConfig();
            envConfig.Prepare(builder);
            services.AddSingleton(envConfig);

            // 单位配置
            RenameFile(envConfig, "unit-origin.json", "unit.json");

            // Uid配置
            LogUtils.Info("正在进行Uid配置...");
            RenameFile(envConfig, "uid-origin.db", "uid.db");
            var uidConfig = AppUtils.GetConfig<UidConfig>(UidConfig.NAME);
            UidUtils.InitConfig(uidConfig);

            // Sql配置
            LogUtils.Info("正在进行Sql配置...");
            RenameFile(envConfig, "scm-origin.db", "scm.db");
            var sqlConfig = AppUtils.GetConfig<SqlConfig>(SqlConfig.NAME) ?? new SqlConfig();
            sqlConfig.Prepare(envConfig);
            SqlSetup(services, envConfig, sqlConfig);

            // Swagger配置
            LogUtils.Info("正在进行Swagger配置...");
            var swaggerConfig = AppUtils.GetConfig<SwaggerConfig>(SwaggerConfig.NAME);
            services.SwaggerSetup(swaggerConfig);

            // 字体配置
            LogUtils.Info("正在进行字体配置...");
            FontSetup(services, envConfig);

            // 缓存配置
            LogUtils.Info("正在进行缓存配置...");
            services.CacheSetup(envConfig);

            // 数据配置
            //var dataConfig = AppUtils.GetConfig<DataConfig>(DataConfig.NAME) ?? new DataConfig();
            //dataConfig.Prepare(builder.Environment);
            //services.AddSingleton(dataConfig);

            // 安全配置
            LogUtils.Info("正在进行安全配置...");
            var secConfig = AppUtils.GetConfig<SecurityConfig>(SecurityConfig.NAME);
            secConfig.Prepare(builder.Environment);
            services.AddSingleton(secConfig);

            // 代码生成
            LogUtils.Info("正在进行代码生成配置...");
            var genConfig = AppUtils.GetConfig<GeneratorConfig>(GeneratorConfig.NAME);
            genConfig.Prepare(envConfig);
            services.GeneratorSetup(genConfig);

            // EMail
            LogUtils.Info("正在进行Email配置...");
            var emailConfig = AppUtils.GetConfig<EmailConfig>(EmailConfig.NAME) ?? new EmailConfig();
            emailConfig.Prepare(envConfig);
            services.AddSingleton(emailConfig);

            // Phone
            LogUtils.Info("正在进行Phone配置...");
            var phoneConfig = AppUtils.GetConfig<PhoneConfig>(PhoneConfig.NAME) ?? new PhoneConfig();
            phoneConfig.Prepare(envConfig);
            services.AddSingleton(phoneConfig);

            // Aiml
            LogUtils.Info("正在进行AIML配置...");
            var aimlConfig = AppUtils.GetConfig<AimlConfig>(AimlConfig.NAME) ?? new AimlConfig();
            aimlConfig.Prepare(envConfig);
            services.AddSingleton(aimlConfig);

            // Oidc
            LogUtils.Info("正在进行OIDC配置...");
            var oidcConfig = AppUtils.GetConfig<OidcConfig>(OidcConfig.NAME) ?? new OidcConfig();
            oidcConfig.Prepare(envConfig);
            services.AddSingleton(oidcConfig);

            // Otp
            LogUtils.Info("正在进行OTP配置...");
            var otpConfig = AppUtils.GetConfig<OtpConfig>(OtpConfig.NAME) ?? new OtpConfig();
            otpConfig.Prepare(envConfig);
            services.AddSingleton(otpConfig);

            // Cors
            LogUtils.Info("正在进行CORS配置...");
            var corsConfig = AppUtils.GetConfig<CorsConfig>(CorsConfig.NAME);
            if (corsConfig != null)
            {
                corsConfig.Prepare(envConfig);
                services.CorsSetup(corsConfig);
            }

            LogUtils.Info("正在进行服务配置...");
            services.AddScoped<IResHolder, ScmResHolder>();
            services.AddScoped<ILogService, ScmLogService>();
            services.AddScoped<IDicService, ScmDicService>();
            services.AddScoped<ICfgService, ScmCfgService>();
            services.AddScoped<ISecService, ScmSecService>();
            services.AddScoped<ICatService, ScmCatService>();
            services.AddScoped<ITagService, ScmTagService>();
            services.AddScoped<IFlowService, ScmFlowService>();

            // MQTT Broker（必须在 SamplesServerUtils.Setup 之前，确保 Broker 先启动）
            LogUtils.Info("正在进行MQTT配置...");
            var mqttBrokerConfig = AppUtils.GetConfig<MqttBrokerConfig>(MqttBrokerConfig.NAME) ?? new MqttBrokerConfig();
            mqttBrokerConfig.Prepare(envConfig);

            // MQTT Client（依赖于 MQTT Broker 配置）
            var mqttClientConfig = AppUtils.GetConfig<MqttClientConfig>(MqttClientConfig.NAME) ?? new MqttClientConfig();
            mqttClientConfig.Prepare(envConfig);
            services.SetupMqtt(mqttBrokerConfig, mqttClientConfig);

            // 自定义服务
            SamplesServerUtils.Setup(services);

            //// NAS 消息服务
            //services.AddNasMessageService();

            // Quartz
            LogUtils.Info("正在进行Quartz配置...");
            var quartzConfig = AppUtils.GetConfig<QuartzConfig>(QuartzConfig.NAME) ?? new QuartzConfig();
            quartzConfig.Prepare(envConfig);
            services.QuartzSetup(quartzConfig);
            services.AddQuartzClassJobs();

            // Mapper
            LogUtils.Info("正在进行Mapper配置...");
            services.AddMapperProfile();

            // SignalR
            LogUtils.Info("正在进行SignalR配置...");
            services.AddSignalR();

            // Jwt Config（必须先注册，Filters 和 Service 依赖 ScmContextHolder）
            LogUtils.Info("正在进行身份认证配置...");
            services.SetupJwt(envConfig);

            // 安全配置服务（支持环境变量和 User Secrets）
            LogUtils.Info("正在进行安全认证配置...");
            services.AddSecureConfiguration();

            // 全局过滤（依赖 ScmContextHolder，必须在 SetupJwt 之后）
            services.AddControllers(options =>
            {
                options.Filters.Add<AopActionFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<UnitOfWorkFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            }).NewtonJsonSetup();

            // 统一响应和异常处理
            //services.AddUnifiedResponse();
            //services.AddApiBehavior();

            // 接口配置（依赖 AddControllers，必须在 MVC 注册之后）
            LogUtils.Info("正在进行接口配置...");
            var apiConfig = AppUtils.GetConfig<DllConfig>(DllConfig.NAME);
            apiConfig.Prepare(builder.Environment);
            services.RegisterServices(apiConfig);

            return builder;
        }

        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            var envConfig = app.Services.GetRequiredService<EnvConfig>();
            var swaggerConfig = AppUtils.GetConfig<SwaggerConfig>(SwaggerConfig.NAME);
            var corsConfig = AppUtils.GetConfig<CorsConfig>(CorsConfig.NAME);

            AppUtils.ServiceProvider = app.Services;

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerSetup(swaggerConfig);
            }

            LogUtils.Info("正在进行静态文件设置...");
            app.UseHttpsRedirection();
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" }
            });
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
            LogUtils.Info("正在进行跨域设置...");
            if (corsConfig != null)
            {
                if (corsConfig.GlobalCors)
                {
                    app.UseCors(ScmEnv.CORS_NAME);
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

            // 异常处理：放在认证/授权之前，确保能捕获整个业务管道的异常
            app.UseMiddleware<ExceptionMiddleware>();

            // 认证：验证 Authorization 头的令牌（Bearer / Api / App 三种方案）
            app.UseAuthentication();
            // Jwt 中间件：解析 Claims 注入 ScmContextHolder，必须在 UseAuthentication 之后
            app.UseMiddleware<JwtMiddleware>();
            // 授权：基于已认证用户检查权限策略
            app.UseAuthorization();

            // 统一响应中间件（包装所有 API 响应）
            //app.UseUnifiedResponse();

            app.UseQuartz();

            app.MapControllers().RequireAuthorization();
            app.MapHub<ScmHub>("/scmhub");

            LogUtils.Info("正在进行启动服务...");
            var kestrelConfig = AppUtils.GetConfig<KestrelConfig>(KestrelConfig.NAME);
            if (kestrelConfig != null)
            {
                var url = kestrelConfig.Endpoints?.Http?.Url;
                if (url == null)
                {
                    url = "http://*:9999";
                }
                url = url.Replace("*", "localhost");
                LogUtils.Info("系统启动完成，您可以通过以下地址访问系统：" + url);
            }
            else
            {
                LogUtils.Info("系统启动完成！");
            }

            LogUtils.Info("===========");

            app.Run();

            return app;
        }

        private static void RenameFile(EnvConfig envConfig, string src, string dst)
        {
            var dstFile = envConfig.GetDataPath(dst);
            if (File.Exists(dstFile))
            {
                return;
            }

            var srcFile = envConfig.GetDataPath(src);
            if (!File.Exists(srcFile))
            {
                return;
            }

            File.Copy(srcFile, dstFile);
        }

        private static void SqlSetup(IServiceCollection services, EnvConfig envConfig, SqlConfig sqlConfig)
        {
            //LogUtils.Info("正在初始化数据库...");

            var dbType = SqlSugarUtils.GetDbType(sqlConfig.Type);
            var sugarScope = new SqlSugarScope(new ConnectionConfig()
            {
                ConnectionString = sqlConfig.Text,
                DbType = dbType,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (c, p) =>
                    {
                        if (c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            p.IsNullable = true;
                        }

                        if (dbType == DbType.MySql)
                        {
                            if (c.PropertyType.IsEnum)
                            {
                                p.DataType = "TINYINT";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                            else if (c.PropertyType == typeof(bool))
                            {
                                p.DataType = "TINYINT(1)";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                            else if (c.PropertyType == typeof(int))
                            {
                                p.DataType = "INT";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                            else if (c.PropertyType == typeof(long))
                            {
                                p.DataType = "BIGINT";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                        }
                        else if (dbType == DbType.Sqlite)
                        {
                            if (c.PropertyType.IsEnum)
                            {
                                p.DataType = "INTEGER";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                            else if (c.PropertyType == typeof(bool))
                            {
                                p.DataType = "INTEGER";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                            else if (c.PropertyType == typeof(int))
                            {
                                p.DataType = "INTEGER";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                            else if (c.PropertyType == typeof(long))
                            {
                                p.DataType = "INTEGER";
                                p.IsNullable = false;
                                p.DefaultValue = "0";
                            }
                        }
                        else if (dbType == DbType.PostgreSQL)
                        {
                            if (c.PropertyType.IsEnum)
                            {
                                p.DataType = "INT";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(bool))
                            {
                                p.DataType = "BOOLEAN";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(int))
                            {
                                p.DataType = "INT";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(long))
                            {
                                p.DataType = "BIGINT";
                                p.IsNullable = false;
                            }
                        }
                        else if (dbType == DbType.Oracle)
                        {
                            if (c.PropertyType.IsEnum)
                            {
                                p.DataType = "Number";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(bool))
                            {
                                p.DataType = "Number(1)";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(int))
                            {
                                p.DataType = "Number(10)";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(long))
                            {
                                p.DataType = "Number(19)";
                                p.IsNullable = false;
                            }
                        }
                        else
                        {
                            if (c.PropertyType.IsEnum)
                            {
                                p.DataType = "TINYINT";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(bool))
                            {
                                p.DataType = "TINYINT(1)";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(int))
                            {
                                p.DataType = "INT";
                                p.IsNullable = false;
                            }
                            else if (c.PropertyType == typeof(long))
                            {
                                p.DataType = "BIGINT";
                                p.IsNullable = false;
                            }
                        }
                    }
                }
            },
            db =>
            {
                //每次Sql执行前事件
                db.Aop.OnLogExecuting = (s, p) =>
                {
                    //var sqlValue = string.Empty;
                    var sql = s;
                    foreach (var item in p)
                    {
                        sql = sql.Replace(item.ParameterName, "'" + item.Value + "'");
                    }
                    //LogUtils.Debug("Sql脚本：" + sql, "db");
                };
            });

            var sqlDir = envConfig.GetDataPath("sql");
            IModelHelper dbHelper = new ScmDbHelper();
            dbHelper.Init(sugarScope, sqlDir);
            dbHelper.InitDb();

            dbHelper = new SamplesDbHelper();
            dbHelper.Init(sugarScope, sqlDir);
            dbHelper.InitDb();

            // 单例注册SqlSugar
            services.AddSingleton<ISqlSugarClient>(sugarScope);
            //注册仓储
            services.AddScoped(typeof(SugarRepository<>));
        }

        private static void FontSetup(IServiceCollection services, EnvConfig envConfig)
        {
            LogUtils.Info("字体目录：" + envConfig.Fonts);
            ImageEngine.LoadFont(envConfig.Fonts);
            LogUtils.Info("默认字体：" + envConfig.DefaultFont);
            ImageEngine.SetDefaultFontName(envConfig.DefaultFont);
        }
    }
}
