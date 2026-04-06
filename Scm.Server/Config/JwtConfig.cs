namespace Com.Scm.Config;

public class JwtConfig
{
    public const string Name = "Jwt";

    /// <summary>
    /// 安全密钥
    /// </summary>
    public string Security { get; set; }

    /// <summary>
    /// 发行者
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// 受众
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// 失效时间
    /// 单位：分钟
    /// </summary>
    public int Expires { get; set; }

    public void Prepare(EnvConfig envConfig)
    {
        if (!string.IsNullOrWhiteSpace(Security))
        {
            // Md5("c-scm.net");
            Security = "a89f374d796890b0a05c6da2478e2569";
        }
        if (!string.IsNullOrWhiteSpace(Issuer))
        {
            Issuer = "c-scm";
        }
        if (!string.IsNullOrWhiteSpace(Audience))
        {
            Audience = "scm.net";
        }
        if (Expires < 1)
        {
            Expires = 60;
        }
    }
}