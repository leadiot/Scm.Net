using Com.Scm.Cfg.UserTheme;
using Com.Scm.Response;

namespace Com.Scm.Operator.Dvo;

/// <summary>
/// 
/// </summary>
public class LoginResponse : ScmApiResponse
{
    /// <summary>
    /// 
    /// </summary>
    public string accessToken { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public OperatorInfo userInfo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserThemeDto userTheme { get; set; }

    /// <summary>
    /// 无效的登录用户信息
    /// </summary>
    public const int ERROR_01 = 1;
    /// <summary>
    /// 无效的登录机构信息
    /// </summary>
    public const int ERROR_02 = 1;
    /// <summary>
    /// 登录账户不存在
    /// </summary>
    public const int ERROR_03 = 1;
    /// <summary>
    /// 账号被冻结，请联系管理员！
    /// </summary>
    public const int ERROR_04 = 2;
    /// <summary>
    /// 限制登录时间
    /// </summary>
    public const int ERROR_05 = 3;
    /// <summary>
    /// 不支持的登录方式
    /// </summary>
    public const int ERROR_06 = 6;

    #region 密码登录
    /// <summary>
    /// 验证码错误
    /// </summary>
    public const int ERROR_11 = 11;
    /// <summary>
    /// 无效的登录用户
    /// </summary>
    public const int ERROR_12 = 12;
    /// <summary>
    /// 无效的登录密码
    /// </summary>
    public const int ERROR_13 = 13;
    /// <summary>
    /// 账号密码输入错误
    /// </summary>
    public const int ERROR_14 = 14;
    #endregion

    #region 手机登录
    /// <summary>
    /// 无效的手机号码
    /// </summary>
    public const int ERROR_21 = 21;
    /// <summary>
    /// 无效的验证码
    /// </summary>
    public const int ERROR_22 = 22;
    /// <summary>
    /// 
    /// </summary>
    public const int ERROR_23 = 23;
    #endregion

    #region 邮件登录
    /// <summary>
    /// 无效的电子邮件
    /// </summary>
    public const int ERROR_31 = 31;
    /// <summary>
    /// 无效的验证码
    /// </summary>
    public const int ERROR_32 = 32;
    #endregion

    #region 联合登录
    /// <summary>
    /// OIDC服务访问异常，请稍后重试！
    /// </summary>
    public const int ERROR_41 = 41;
    /// <summary>
    /// 无效的联合登录信息！
    /// </summary>
    public const int ERROR_42 = 42;
    /// <summary>
    /// 联合登录授权已过期，请重新授权！
    /// </summary>
    public const int ERROR_43 = 43;
    /// <summary>
    /// 联合登录异常
    /// </summary>
    public const int ERROR_44 = 44;
    /// <summary>
    /// 联合登录不存在关联账户！
    /// </summary>
    public const int ERROR_45 = 45;
    /// <summary>
    /// 联合登录存在多个关联账户！
    /// </summary>
    public const int ERROR_46 = 46;
    #endregion
}