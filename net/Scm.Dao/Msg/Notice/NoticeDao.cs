using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Notice;

/// <summary>
/// 邮件
/// </summary>
[SugarTable("scm_msg_notice")]
public class NoticeDao : ScmDataDao
{
    /// <summary>
    /// 发送人编号
    /// </summary>
    [Required]
    public long send_user { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public long send_time { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    [Required]
    [StringLength(256)]
    [SugarColumn(Length = 256)]
    public string title { get; set; }

    /// <summary>
    /// 通知内容
    /// </summary>
    [Required]
    [StringLength(2048)]
    [SugarColumn(Length = 2048)]
    public string content { get; set; }

    /// <summary>
    /// 引用通知
    /// </summary>
    public long ref_id { get; set; }
}