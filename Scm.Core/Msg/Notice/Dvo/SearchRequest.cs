using Com.Scm.Enums;

namespace Com.Scm.Msg.Notice.Dvo;

/// <summary>
/// 
/// </summary>
public class SearchRequest : ScmSearchPageRequest
{
    /// <summary>
    /// 目录类别
    /// </summary>
    public SearchFolderEnum folder { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public SearchStatusEnum status { get; set; }
    /// <summary>
    /// 阅读状态
    /// </summary>
    public NoticeReadedEnum readed { get; set; }
}

/// <summary>
/// 搞件目录
/// </summary>
public enum SearchFolderEnum
{
    /// <summary>
    /// 
    /// </summary>
    None = 0,
    /// <summary>
    /// 收件箱
    /// </summary>
    Inbox,
    /// <summary>
    /// 发件箱
    /// </summary>
    Outbox
}

/// <summary>
/// 
/// </summary>
public enum SearchStatusEnum
{
    /// <summary>
    /// 
    /// </summary>
    None = 0,
    /// <summary>
    /// 草稿
    /// </summary>
    Draft = 1,
    /// <summary>
    /// 删除
    /// </summary>
    Droped = 2,
    /// <summary>
    /// 归档
    /// </summary>
    Archived = 3
}
