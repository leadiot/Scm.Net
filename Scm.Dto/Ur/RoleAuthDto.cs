using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 授权表
/// </summary>
public class RoleAuthDto : ScmDataDto
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
    //public List<SysMenuApiUrl> api { get; set; } = new();

    /// <summary>
    /// 授权类型1=角色-菜单
    /// </summary>
    [Required]
    public ScmRoleAuthTypesEnum types { get; set; }
}