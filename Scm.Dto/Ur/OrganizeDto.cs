using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 组织机构表
/// </summary>
public class OrganizeDto : ScmDataDto
{
    /// <summary>
    /// 
    /// </summary>
    public const long SYS_ID = 1000000000000000001L;

    /// <summary>
    /// 父节点
    /// </summary>
    [Required]
    public long pid { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [Required]
    [StringLength(32)]
    public string namec { get; set; }

    /// <summary>
    /// 机构编码
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 部门层级
    /// </summary>
    [Required]
    public int lv { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; }

    /// <summary>
    /// 主管人员
    /// </summary>
    public long owner_id { get; set; }

    /// <summary>
    /// 主管人员(冗余)
    /// </summary>
    public string owner_names { get; set; }
}