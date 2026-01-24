using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Dic;

/// <summary>
/// 字典信息表
/// </summary>
[SugarTable("scm_sys_dic_detail")]
public class DicDetailDao : ScmDataDao
{
    /// <summary>
    /// 分类编号
    /// </summary>
    [Required]
    public long dic_header_id { get; set; }

    /// <summary>
    /// 键
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string codec { get; set; }
    /// <summary>
    /// 值
    /// </summary>
    [Required]
    public int value { get; set; }

    /// <summary>
    /// 字典值名称
    /// </summary>
    [Required]
    [StringLength(64)]
    [SugarColumn(Length = 64)]
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
    [SugarColumn(Length = 128, IsNullable = true)]
    public string remark { get; set; }

    /// <summary>
    /// 标记 1=默认  2=其他
    /// </summary>
    [Required]
    public int tag { get; set; } = 1;

    /// <summary>
    /// 类别
    /// </summary>
    public int cat { get; set; }
}