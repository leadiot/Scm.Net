using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Dic;

/// <summary>
/// 字典信息表
/// </summary>
public class DicDetailDto : ScmDataDto
{
    /// <summary>
    /// 分类编号
    /// </summary>
    [Required]
    public long dic_header_id { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [Required]
    public int value { get; set; }

    /// <summary>
    /// 键（可选）
    /// </summary>
    [Required]
    [StringLength(50)]
    public string codec { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    [StringLength(64)]
    public string namec { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; } = 1;

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(128)]
    public string remark { get; set; }

    /// <summary>
    /// 标记 1=默认  2=其他
    /// </summary>
    public int tag { get; set; } = 1;

    /// <summary>
    /// 分类
    /// </summary>
    public int cat { get; set; }
}