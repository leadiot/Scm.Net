using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Calendar;

/// <summary>
/// 日程表
/// </summary>
[SugarTable("scm_sys_calendar")]
public class CalendarDao : ScmDataDao
{
    /// <summary>
    /// 日程标题
    /// </summary>
    [Required]
    [StringLength(255)]
    public string title { get; set; }

    /// <summary>
    /// 日程类型
    /// </summary>
    [Required]
    public int types { get; set; }

    /// <summary>
    /// 日程等级
    /// </summary>
    [Required]
    public int level { get; set; }

    /// <summary>
    /// 重要程度
    /// </summary>
    public int important { get; set; }

    /// <summary>
    /// 紧急程度
    /// </summary>
    public int urgent { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [Required]
    public long start_time { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [Required]
    public long end_time { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public CalendarRemindEnum remind_type { get; set; }

    /// <summary>
    /// 提醒设置
    /// </summary>
    public int remind_time { get; set; }

    /// <summary>
    /// 重复方式
    /// </summary>
    public CalendarRepeatEnum repeat_type { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int repeat_time { get; set; }
}