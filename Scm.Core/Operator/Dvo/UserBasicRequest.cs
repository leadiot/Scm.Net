using Com.Scm.Enums;

namespace Com.Scm.Operator.Dvo;

/// <summary>
/// 个人信息基本修改
/// </summary>
public class UserBasicRequest : ScmUpdateRequest
{
    /// <summary>
    /// 用户全称
    /// </summary>
    public string namec { get; set; }

    /// <summary>
    /// 用户简称
    /// </summary>
    public string names { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public ScmSexEnum sex { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string cellphone { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string email { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string remark { get; set; }
}