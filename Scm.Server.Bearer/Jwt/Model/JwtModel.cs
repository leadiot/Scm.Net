namespace Com.Scm.Jwt.Model;

/// <summary>
/// 
/// </summary>
public class JwtModel
{
    public const string Name = "JwtAuth";

    /// <summary>
    /// 
    /// </summary>
    public string Security { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// 失效时间，单位：分钟
    /// </summary>
    public int WebExp { get; set; }
}