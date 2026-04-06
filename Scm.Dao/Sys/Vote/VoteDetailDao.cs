using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Vote;

/// <summary>
/// 投票项
/// </summary>
[SugarTable("scm_vote_detail")]
public class VoteDetailDao : ScmDataDao
{
    /// <summary>
    /// 投票编号
    /// </summary>
    [Required]
    public long header_id { get; set; } = 0;

    /// <summary>
    /// 投票项标题
    /// </summary>
    [Required]
    [StringLength(128)]
    [SugarColumn(Length = 128)]
    public string title { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [StringLength(256)]
    [SugarColumn(Length = 256, IsNullable = true)]
    public string remark { get; set; }

    /// <summary>
    /// 投票数量
    /// </summary>
    [Required]
    public int count { get; set; } = 0;
}