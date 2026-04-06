using Com.Scm.Enums;
using Com.Scm.Response;

namespace Com.Scm.Operator.Dvo;

/// <summary>
/// 返回工作台用户信息
/// </summary>
public class OperatorUserWorkResponse : ScmApiResponse
{
    /// <summary>
    /// 
    /// </summary>
    public long id { get; set; }

    /// <summary>
    /// 用户账户
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 用户简称
    /// </summary>
    public string names { get; set; }

    /// <summary>
    /// 用户全称
    /// </summary>
    public string namec { get; set; }
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
    public string avatar { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ScmSexEnum sex { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> organize_list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> position_list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> group_list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<string> role_list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string lastTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int loginSum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int messageSum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int agencySum { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string remark { get; set; }
}