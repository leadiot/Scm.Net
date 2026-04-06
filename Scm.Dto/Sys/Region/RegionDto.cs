using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Region;

/// <summary>
/// 城市表
/// </summary>
public class RegionDto : ScmDataDto
{
    public const long SYS_ID = 1000000000000000001L;
    public const long DEF_PID = 1000000000000000002L;
    public const long CHINA_ID = 1000000000000000101L;

    [StringLength(32)]
    public string names { get; set; }

    /// <summary>
    /// 城市名称
    /// </summary>
    [Required]
    [StringLength(64)]
    public string namef { get; set; }

    /// <summary>
    /// 所属上级
    /// </summary>
    [Required]
    public long ParentId { get; set; }

    /// <summary>
    /// 所属上级组
    /// </summary>
    public List<string> ParentIdList { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    [Required]
    public int lv { get; set; } = 1;

    /// <summary>
    /// 城市编号
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public string lng { get; set; }

    /// <summary>
    /// 维度
    /// </summary>
    public string lat { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int od { get; set; } = 1;
}