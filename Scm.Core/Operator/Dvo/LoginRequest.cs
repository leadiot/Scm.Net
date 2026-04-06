using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Operator.Dvo;

/// <summary>
/// 登录参数
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// 登录类型
    /// </summary>
    public ScmLoginModeEnum mode { get; set; }

    #region ByPass
    /// <summary>
    /// 登录账号
    /// </summary>
    public string user { get; set; }

    /// <summary>
    /// 登录密码
    /// </summary>
    public string pass { get; set; }
    #endregion

    #region ByPhone
    /// <summary>
    /// 
    /// </summary>
    public string phone { get; set; }
    #endregion

    #region ByEmail
    /// <summary>
    /// 
    /// </summary>
    public string email { get; set; }
    #endregion

    #region ByOauth
    /// <summary>
    /// 
    /// </summary>
    public string state { get; set; }
    #endregion

    #region 公共参数
    /// <summary>
    /// 操作时间
    /// </summary>
    public long time { get; set; }

    /// <summary>
    /// 验证码Key
    /// </summary>
    [Required]
    public string key { get; set; }

    /// <summary>
    /// 验证码Value
    /// </summary>
    [Required]
    public string code { get; set; }

    /// <summary>
    /// 是否自动创建
    /// </summary>
    public bool auto { get; set; }
    #endregion
}