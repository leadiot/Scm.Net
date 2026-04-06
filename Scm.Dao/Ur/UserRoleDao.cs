using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 用户角色关系表
/// </summary>
[SugarTable("scm_ur_user_role")]
public class UserRoleDao : ScmDataDao
{
    /// <summary>
    /// 用户编号
    /// </summary>
    [Required]
    public long user_id { get; set; }

    /// <summary>
    /// 角色编号
    /// </summary>
    [Required]
    public long role_id { get; set; }
}