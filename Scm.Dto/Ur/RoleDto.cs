using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 角色表
/// </summary>
public class RoleDto : ScmDataDto
{
    /// <summary>
    /// 
    /// </summary>
    public const long SYS_ID = 1000000000000000001L;

    /// <summary>
    /// 
    /// </summary>
    public string names { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [Required]
    [StringLength(32)]
    public string namec { get; set; }

    /// <summary>
    /// 角色父节点
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 角色层级
    /// </summary>
    [Required]
    public int lv { get; set; }

    /// <summary>
    /// 角色设置最大数量 0为不限制
    /// </summary>
    [Required]
    public int MaxLength { get; set; } = 0;

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string remark { get; set; }
}
