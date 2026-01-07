using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Region;

/// <summary>
/// 区域表
/// </summary>
[SugarTable("scm_sys_region")]
public class RegionDao : ScmDataDao, ISortableDao
{
    /// <summary>
    /// 城市编号
    /// </summary>
    public string codes { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 区域简称
    /// </summary>
    [Required]
    [StringLength(64)]
    public string names { get; set; }

    /// <summary>
    /// 区域全称
    /// </summary>
    public string namef { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string namee { get; set; }

    /// <summary>
    /// 所属上级
    /// </summary>
    [Required]
    public long pid { get; set; }

    /// <summary>
    /// 层级
    /// </summary>
    [Required]
    public int lv { get; set; }

    /// <summary>
    /// 经度
    /// </summary>
    public string lng { get; set; }

    /// <summary>
    /// 维度
    /// </summary>
    public string lat { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string postcode { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int od { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public ScmRowStatusEnum p_status { get; set; }
}