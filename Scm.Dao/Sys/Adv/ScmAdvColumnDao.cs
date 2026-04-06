using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Adv;

/// <summary>
/// 广告位栏目表 
/// </summary>
[SugarTable("scm_adv_column")]
public class ScmAdvColumnDao : ScmDataDao
{
    /// <summary>
    /// 父编号
    /// </summary>
    [Required]
    public long pid { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    [Required]
    public int layer { get; set; } = 1;

    /// <summary>
    /// 栏目名称
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string namec { get; set; }

    /// <summary>
    /// 栏目类型
    /// </summary>
    [StringLength(50)]
    [SugarColumn(Length = 50, IsNullable = true)]
    public string flag { get; set; }

    /// <summary>
    /// 栏目宽度
    /// </summary>
    [Required]
    public int width { get; set; } = 0;

    /// <summary>
    /// 栏目高度
    /// </summary>
    [Required]
    public int height { get; set; } = 0;

    /// <summary>
    /// 排序
    /// </summary>
    public int od { get; set; }

    /// <summary>
    /// 栏目说明
    /// </summary>
    [StringLength(512)]
    [SugarColumn(Length = 512, IsNullable = true)]
    public string remark { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    [Required]
    public long site_id { get; set; }
}