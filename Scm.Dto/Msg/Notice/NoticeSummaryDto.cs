namespace Com.Scm.Msg.Notice;

/// <summary>
/// 统计信息
/// </summary>
public class NoticeSummaryDto
{
    /// <summary>
    /// 未读
    /// </summary>
    public int unread { get; set; }

    /// <summary>
    /// 草稿
    /// </summary>
    public int draft { get; set; }

    /// <summary>
    /// 删除
    /// </summary>
    public int deleted { get; set; }

    /// <summary>
    /// 存档
    /// </summary>
    public int archived { get; set; }
}