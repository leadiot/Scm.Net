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
    public long column_id { get; set; }

    /// <summary>
    /// 广告名称
    /// </summary>
    [Required]
    [StringLength(128)]
    [SugarColumn(Length = 128)]
    public string title { get; set; }

    /// <summary>
    /// 广告位类型
    /// </summary>
    [Required]
    public int Types { get; set; } = 1;

    /// <summary>
    /// 广告位图片
    /// </summary>
    [StringLength(128)]
    [SugarColumn(Length = 128, IsNullable = true)]
    public string image_url { get; set; }

    /// <summary>
    /// 连接地址
    /// </summary>
    [StringLength(128)]
    [SugarColumn(Length = 128, IsNullable = true)]
    public string link_url { get; set; }

    /// <summary>
    /// 跳转方式
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string target { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [StringLength(512)]
    [SugarColumn(Length = 512, IsNullable = true)]
    public string remark { get; set; }

    /// <summary>
    /// 广告位代码
    /// </summary>
    [StringLength(512)]
    [SugarColumn(Length = 512, IsNullable = true)]
    public string codes { get; set; }

    /// <summary>
    /// 是否开启时间限制
    /// </summary>
    [Required]
    public bool time_limit { get; set; } = false;

    /// <summary>
    /// 开始时间
    /// </summary>
    public long begin_time { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public long end__time { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; } = 1;

    /// <summary>
    /// 广告点击率
    /// </summary>
    [Required]
    public int hits { get; set; } = 0;
}