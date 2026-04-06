using Com.Scm.Enums;
using Com.Scm.Ur;
using Com.Scm.Utils;
using System.Text;

namespace Com.Scm.Token;

public class ScmToken
{
    public const string TokenName = "ScmToken";
    public const string PRE_API = "Api ";
    public const string PRE_APP = "App ";

    public const string ApiToken = "ApiToken";
    public const string AppToken = "AppToken";

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

    public static ScmToken FromAppToken(string token)
    {
        if (token.StartsWith(PRE_APP))
        {
            token = token.Substring(PRE_APP.Length);
        }

        var bytes = Convert.FromBase64String(token);
        token = Encoding.UTF8.GetString(bytes);

        var arr = token.Split(":");
        var nasToken = new ScmToken();
        if (arr.Length == 3)
        {
            var tmp = arr[0];
            if (TextUtils.IsLong(tmp))
            {
                nasToken.terminal_id = long.Parse(tmp);
            }

            tmp = arr[1];
            if (TextUtils.IsLong(tmp))
            {
                nasToken.time = long.Parse(tmp);
            }

            nasToken.digest = arr[2];
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