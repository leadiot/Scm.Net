using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Vote;

/// <summary>
/// 投票日志
/// </summary>
[SugarTable("scm_vote_result")]
public class VoteResultDao : ScmDataDao
{
    /// <summary>
    /// 投票编号
    /// </summary>
    [Required]
    public long header_id { get; set; } = 0;

    /// <summary>
    /// 投票项编号
    /// </summary>
    [Required]
    public long detail_id { get; set; }

    /// <summary>
    /// 用户编号
    /// </summary>
    [Required]
    public long user_id { get; set; }
}