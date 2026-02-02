using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Vote;

/// <summary>
/// 投票表
/// </summary>
[SugarTable("scm_vote_header")]
public class VoteHeaderDao : ScmDataDao
{
    /// <summary>
    /// 投票标题
    /// </summary>
    [Required]
    [StringLength(128)]
    [SugarColumn(Length = 128)]
    public string title { get; set; }

    /// <summary>
    /// 投票类型（单选、多选）
    /// </summary>
    [Required]
    public VoteTypeEnums type { get; set; }

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
    /// 防刷规则（IP限制）
    /// </summary>
    [Required]
    public bool swipe_rule { get; set; } = true;

    /// <summary>
    /// 文件地址
    /// </summary>
    [StringLength(128)]
    [SugarColumn(Length = 128, IsNullable = true)]
    public string file_url { get; set; }

    /// <summary>
    /// 规则
    /// </summary>
    [StringLength(1024)]
    [SugarColumn(Length = 1024, IsNullable = true)]
    public string summary { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<VoteDetailDao> details { get; set; }
}