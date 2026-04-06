using Com.Scm.Dvo;

namespace Com.Scm.Sys.VoteResult.Dvo;

/// <summary>
/// 投票日志
/// </summary>
public class SysVoteResultDvo : ScmDataDvo
{
    /// <summary>
    /// 投票编号
    /// </summary>
    public long header_id { get; set; }

    /// <summary>
    /// 投票项编号
    /// </summary>
    public long detail_id { get; set; }

    /// <summary>
    /// 用户编号
    /// </summary>
    public long user_id { get; set; }
}