using Com.Scm.Dvo;

namespace Com.Scm.Sys.VoteDetail.Dvo;

/// <summary>
/// 投票项
/// </summary>
public class SysVoteDetailDvo : ScmDataDvo
{
    /// <summary>
    /// 投票编号
    /// </summary>
    public long header_id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string remark { get; set; }

    /// <summary>
    /// 投票数量
    /// </summary>
    public int count { get; set; }
}