using Com.Scm.Enums;
using Com.Scm.Ur;

namespace Com.Scm.Token;

public class ScmToken
{
    public const string TokenName = "Authorization";
    public const string KEY_BASIC = "Basic";
    public const string KEY_BEARER = "Bearer";

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
}