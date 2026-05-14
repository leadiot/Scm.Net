using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Com.Scm.Configure.Security
{
    /// <summary>
    /// 安全配置服务
    /// 支持从多个来源读取配置，优先级从高到低：
    /// 1. 环境变量
    /// 2. User Secrets (开发环境)
    /// 3. appsettings.{Environment}.json
    /// 4. appsettings.json
    /// </summary>
    public class SecretConfigurationService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public SecretConfigurationService(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        /// <summary>
        /// 获取配置值，优先从环境变量读取
        /// </summary>
        public string GetValue(string key)
        {
            // 环境变量格式：SCM__SECTION__KEY
            var envKey = ConvertToEnvKey(key);
            var envValue = Environment.GetEnvironmentVariable(envKey);
            
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue;
            }

            // 尝试读取配置
            return _configuration[key];
        }

        /// <summary>
        /// 获取配置值，带有默认值
        /// </summary>
        public string GetValue(string key, string defaultValue)
        {
            var value = GetValue(key);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        /// <summary>
        /// 获取整数配置值
        /// </summary>
        public int GetIntValue(string key, int defaultValue = 0)
        {
            var value = GetValue(key);
            if (int.TryParse(value, out var result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取布尔配置值
        /// </summary>
        public bool GetBoolValue(string key, bool defaultValue = false)
        {
            var value = GetValue(key);
            if (bool.TryParse(value, out var result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public string GetConnectionString(string name)
        {
            var envKey = ConvertToEnvKey($"ConnectionStrings:{name}");
            var envValue = Environment.GetEnvironmentVariable(envKey);
            
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue;
            }

            return _configuration.GetConnectionString(name);
        }

        /// <summary>
        /// 获取敏感配置值（从 Secrets 或环境变量）
        /// </summary>
        public string GetSecret(string key)
        {
            // 优先从环境变量读取
            var envKey = ConvertToEnvKey(key);
            var envValue = Environment.GetEnvironmentVariable(envKey);
            
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue;
            }

            // 如果是开发环境，尝试从 User Secrets 读取
            if (_environment.IsDevelopment())
            {
                var secretValue = _configuration[key];
                if (!string.IsNullOrWhiteSpace(secretValue))
                {
                    return secretValue;
                }
            }

            return null;
        }

        /// <summary>
        /// 将配置键转换为环境变量格式
        /// </summary>
        private string ConvertToEnvKey(string key)
        {
            return "SCM__" + key.Replace(":", "__").Replace(".", "__").ToUpperInvariant();
        }

        /// <summary>
        /// 验证敏感配置是否存在
        /// </summary>
        public bool IsSecretSet(string key)
        {
            return !string.IsNullOrWhiteSpace(GetSecret(key));
        }

        /// <summary>
        /// 获取 Email 配置
        /// </summary>
        public EmailSecretConfig GetEmailConfig()
        {
            return new EmailSecretConfig
            {
                SmtpServer = GetValue("Email:SmtpServer"),
                Port = GetIntValue("Email:Port", 465),
                Username = GetSecret("Email:Username"),
                Password = GetSecret("Email:Password")
            };
        }

        /// <summary>
        /// 获取数据库配置
        /// </summary>
        public DatabaseSecretConfig GetDatabaseConfig()
        {
            return new DatabaseSecretConfig
            {
                Type = GetValue("Sql:Type", "Sqlite"),
                ConnectionString = GetSecret("Sql:Text") ?? GetValue("Sql:Text")
            };
        }

        /// <summary>
        /// 获取 Redis 配置
        /// </summary>
        public RedisSecretConfig GetRedisConfig()
        {
            return new RedisSecretConfig
            {
                Type = GetValue("Cache:Type", "Memory"),
                ConnectionString = GetSecret("Cache:Text") ?? GetValue("Cache:Text")
            };
        }

        /// <summary>
        /// 获取 JWT 配置
        /// </summary>
        public JwtSecretConfig GetJwtConfig()
        {
            return new JwtSecretConfig
            {
                SecurityKey = GetSecret("Jwt:Security") ?? GetValue("Jwt:Security"),
                Issuer = GetValue("Jwt:Issuer", "Scm.Net"),
                Audience = GetValue("Jwt:Audience", "scm.net"),
                Expires = GetIntValue("Jwt:Expires", 60)
            };
        }

        /// <summary>
        /// 获取 OIDC 配置
        /// </summary>
        public OidcSecretConfig GetOidcConfig()
        {
            return new OidcSecretConfig
            {
                AppKey = GetSecret("Oidc:app_key"),
                AppSecret = GetSecret("Oidc:app_secret"),
                RedirectUri = GetValue("Oidc:redirect_uri"),
                Scope = GetValue("Oidc:scope", "openid")
            };
        }

        /// <summary>
        /// 获取安全密钥配置
        /// </summary>
        public SecurityKeysConfig GetSecurityKeysConfig()
        {
            return new SecurityKeysConfig
            {
                AppKey = GetSecret("Security:AppKey"),
                AesKey = GetSecret("Security:AesKey"),
                SignKey = GetSecret("Security:SignKey"),
                DesKey = GetSecret("Security:DesKey")
            };
        }
    }

    /// <summary>
    /// Email 敏感配置
    /// </summary>
    public class EmailSecretConfig
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// 数据库敏感配置
    /// </summary>
    public class DatabaseSecretConfig
    {
        public string Type { get; set; }
        public string ConnectionString { get; set; }
    }

    /// <summary>
    /// Redis 敏感配置
    /// </summary>
    public class RedisSecretConfig
    {
        public string Type { get; set; }
        public string ConnectionString { get; set; }
    }

    /// <summary>
    /// JWT 敏感配置
    /// </summary>
    public class JwtSecretConfig
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expires { get; set; }
    }

    /// <summary>
    /// OIDC 敏感配置
    /// </summary>
    public class OidcSecretConfig
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
    }

    /// <summary>
    /// 安全密钥配置
    /// </summary>
    public class SecurityKeysConfig
    {
        public string AppKey { get; set; }
        public string AesKey { get; set; }
        public string SignKey { get; set; }
        public string DesKey { get; set; }
    }
}
