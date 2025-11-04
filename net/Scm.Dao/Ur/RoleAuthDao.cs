using Com.Scm.Adm.Menu;
using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 角色权限关系表
/// </summary>
[SugarTable("scm_ur_role_auth")]
public class RoleAuthDao : ScmDataDao
{
    /// <summary>
    /// 角色编号
    /// </summary>
    [Required]
    public long role_id { get; set; }

    /// <summary>
    /// 显示排序
    /// </summary>
    public int od { get; set; }

    /// <summary>
    /// 菜单编号
    /// </summary>
    public long auth_id { get; set; }

    /// <summary>
    /// 接口权限
    /// </summary>
    [SugarColumn(IsJson = true)]
    public List<SysMenuApiUrl> api { get; set; } = new();

    /// <summary>
    /// 授权类型1=角色-菜单
    /// 暂时没有使用
    /// </summary>
    [Required]
    public ScmRoleAuthTypesEnum types { get; set; }
}