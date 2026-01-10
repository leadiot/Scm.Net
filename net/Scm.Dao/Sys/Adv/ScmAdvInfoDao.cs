using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Adv;

/// <summary>
/// 广告位信息表 
/// </summary>
[SugarTable("scm_adv_info")]
public class ScmAdvInfoDao : ScmDao
{
    /// <summary>
    /// 栏目Id
    /// </summary>
    [Required]
    public long ColumnId { get; set; }

    /// <summary>
    /// 广告名称
    /// </summary>
    [Required]
    [StringLength(128)]
    [SugarColumn(Length = 128)]
    public string Title { get; set; }

    /// <summary>
    /// 广告位类型
    /// </summary>
    [Required]
    public int Types { get; set; } = 1;

    /// <summary>
    /// 状态
    /// </summary>
    [Required]
    public bool Status { get; set; } = true;

    /// <summary>
    /// 广告位图片
    /// </summary>
    [SugarColumn(Length = 128)]
    public string ImgUrl { get; set; }

    /// <summary>
    /// 连接地址
    /// </summary>
    [SugarColumn(Length = 128)]
    public string LinkUrl { get; set; }

    /// <summary>
    /// 跳转方式
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string Target { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(Length = 512)]
    public string Summary { get; set; }

    /// <summary>
    /// 广告位代码
    /// </summary>
    [SugarColumn(Length = 512)]
    public string Codes { get; set; }

    /// <summary>
    /// 是否开启时间限制
    /// </summary>
    [Required]
    public bool IsTimeLimit { get; set; } = false;

    /// <summary>
    /// 开始时间
    /// </summary>
    public long BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public long EndTime { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int Sort { get; set; } = 1;

    /// <summary>
    /// 广告点击率
    /// </summary>
    [Required]
    public int Hits { get; set; } = 0;
}