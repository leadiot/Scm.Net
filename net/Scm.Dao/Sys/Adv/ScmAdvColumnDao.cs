using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Adv;

/// <summary>
/// 广告位栏目表 
/// </summary>
[SugarTable("scm_adv_column")]
public class ScmAdvColumnDao : ScmDao
{
    /// <summary>
    /// 父编号
    /// </summary>
    [Required]
    public long ParentId { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    [Required]
    public int Layer { get; set; } = 1;

    /// <summary>
    /// 栏目名称
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string Name { get; set; }

    /// <summary>
    /// 栏目类型
    /// </summary>
    [SugarColumn(Length = 50)]
    public string Flag { get; set; }

    /// <summary>
    /// 栏目宽度
    /// </summary>
    [Required]
    public int Width { get; set; } = 0;

    /// <summary>
    /// 栏目高度
    /// </summary>
    [Required]
    public int Height { get; set; } = 0;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 栏目状态
    /// </summary>
    [Required]
    public bool Status { get; set; } = true;

    /// <summary>
    /// 栏目说明
    /// </summary>
    [SugarColumn(Length = 512)]
    public string Summary { get; set; }

    /// <summary>
    /// 站点ID
    /// </summary>
    [Required]
    public long SiteId { get; set; }

    /// <summary>
    /// 添加时间
    /// </summary>
    public DateTime CreateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 添加人
    /// </summary>
    public string CreateUser { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 修改人
    /// </summary>
    public string UpdateUser { get; set; }


}