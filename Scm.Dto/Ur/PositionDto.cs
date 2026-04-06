using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 岗位表
/// </summary>
public class PositionDto : ScmDataDto
{
    /// <summary>
    /// 
    /// </summary>
    public const long SYS_ID = 1000000000000000001L;

    /// <summary>
    /// 岗位编码
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 岗位名称
    /// </summary>
    [Required]
    [StringLength(32)]
    public string namec { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; } = 1;

    /// <summary>
    /// 备注信息
    /// </summary>
    public string remark { get; set; }
}