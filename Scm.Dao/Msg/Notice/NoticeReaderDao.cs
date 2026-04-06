using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Notice;

/// <summary>
/// 收件人（是否已读）
/// </summary>
[SugarTable("scm_msg_notice_reader")]
public class NoticeReaderDao : ScmDataDao
{
    /// <summary>
    /// 通知编号
    /// </summary>
    [Required]
    public long notice_id { get; set; }

    /// <summary>
    /// 用户编号
    /// </summary>
    [Required]
    public long user_id { get; set; }

    /// <summary>
    /// 是否归档
    /// </summary>
    public bool is_arc { get; set; }
    /// <summary>
    /// 是否删除
    /// </summary>
    public bool is_del { get; set; }
    /// <summary>
    /// 未读标识
    /// </summary>
    public bool unread { get; set; }
    /// <summary>
    /// 读取次数(统计)
    /// </summary>
    [Required]
    public int read_qty { get; set; }

    /// <summary>
    /// 回复次数
    /// </summary>
    public int reply_qty { get; set; }
}