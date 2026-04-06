using Com.Scm.Dto;
using Com.Scm.Enums;

namespace Com.Scm.Ur;

/// <summary>
/// 用户
/// </summary>
public class UserDto : ScmDataDto
{
    /// <summary>
    /// 系统ID
    /// </summary>
    public const long SYS_ID = 1000000000000000001L;
    /// <summary>
    /// 机器人ID
    /// </summary>
    public const long ROBOT_ID = 1000000000000001002L;

    /// <summary>
    /// 
    /// </summary>
    public string codes { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 登录账号
    /// </summary>
    public string names { get; set; }

    /// <summary>
    /// 展示姓名
    /// </summary>
    public string namec { get; set; }

    /// <summary>
    /// 登录密码
    /// </summary>
    public string pass { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string avatar { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string cellphone { get; set; }
    /// <summary>
    /// 固话
    /// </summary>
    public string telephone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string email { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public ScmSexEnum sex { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string remark { get; set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    public long login_time { get; set; }

    /// <summary>
    /// 上次登录时间
    /// </summary>
    public long last_time { get; set; }

    /// <summary>
    /// 登录次数
    /// </summary>
    public int login_count { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<long> organize_list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<long> position_list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<long> group_list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<long> role_list { get; set; }
}