using Com.Scm.Enums;
using Com.Scm.Ur;
using Com.Scm.Utils;
using System.Text;

namespace Com.Scm.Token;

public class ScmToken
{
    // ── Authorization Scheme 前缀（RFC 7235 标准）──
    // 格式：Authorization: <scheme> <credentials>
    public const string SCHEME_BEARER = "Bearer";
    public const string SCHEME_OPERATOR = "Operator";
    public const string SCHEME_TERMINAL = "Terminal";

    /// <summary>
    /// 会话ID
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public long user_id { get; set; } = UserDto.SYS_ID;
    public string user_codes { get; set; }
    public string user_name { get; set; }

    /// <summary>
    /// 终端
    /// </summary>
    public long terminal_id { get; set; }
    public string terminal_codes { get; set; }
    public string terminal_name { get; set; }

    /// <summary>
    /// 摘要
    /// </summary>
    public string digest { get; set; }

    /// <summary>
    /// 会话时间(UTC Unix时间戳）
    /// </summary>
    public long time { get; set; }

    /// <summary>
    /// 数据权限
    /// </summary>
    public ScmUserDataEnum data { get; set; }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(id);
    }

    public bool IsLogin()
    {
        return user_id > 0;
    }

    /// <summary>
    /// 从 Authorization 头提取 scheme 前缀（如 "Bearer"、"Api"、"App"）
    /// </summary>
    /// <param name="authorizationValue">Authorization 请求头的完整值</param>
    /// <returns>scheme 字符串，若无空格则返回空</returns>
    public static string GetScheme(string authorizationValue)
    {
        if (string.IsNullOrEmpty(authorizationValue))
            return string.Empty;

        var spaceIndex = authorizationValue.IndexOf(' ');
        return spaceIndex > 0 ? authorizationValue[..spaceIndex] : string.Empty;
    }

    /// <summary>
    /// 从 Authorization 头提取 credentials（scheme 之后的部分）
    /// </summary>
    /// <param name="authorizationValue">Authorization 请求头的完整值</param>
    /// <param name="scheme">期望的 scheme 前缀</param>
    /// <returns>credentials 部分，scheme 不匹配时返回 null</returns>
    public static string GetCredentials(string authorizationValue, string scheme)
    {
        if (string.IsNullOrEmpty(authorizationValue))
            return null;

        var prefix = scheme + " ";
        if (authorizationValue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return authorizationValue[prefix.Length..];
        }

        return null;
    }

    /// <summary>
    /// 解析 App 设备令牌（Base64 编码的 terminal_id:time:digest 三元组）
    /// </summary>
    public static ScmToken FromTerminalToken(string token)
    {
        // 兼容旧格式：如果带 "App " 前缀则剥离
        var prefix = SCHEME_TERMINAL + " ";
        if (token.StartsWith(prefix))
        {
            token = token[prefix.Length..];
        }

        var bytes = Convert.FromBase64String(token);
        token = Encoding.UTF8.GetString(bytes);

        var nasToken = new ScmToken();

        // 解析格式：terminal_id:user_id:time:digest
        var arr = token.Split(":");
        if (arr.Length == 4)
        {
            // 终端ID
            var tmp = arr[0];
            if (TextUtils.IsLong(tmp))
            {
                nasToken.terminal_id = long.Parse(tmp);
            }

            // 用户ID
            tmp = arr[1];
            if (TextUtils.IsLong(tmp))
            {
                nasToken.user_id = long.Parse(tmp);
            }

            // 时间戳
            tmp = arr[2];
            if (TextUtils.IsLong(tmp))
            {
                nasToken.time = long.Parse(tmp);
            }

            // 摘要
            nasToken.digest = arr[3];
        }

        return nasToken;
    }

    public bool IsValidAppToken(string token, string appToken)
    {
        var key = $"{terminal_id}:{time}:{token}";
        var hash = TextUtils.Md5(key);
        return hash.Equals(appToken, StringComparison.OrdinalIgnoreCase);
    }
}
