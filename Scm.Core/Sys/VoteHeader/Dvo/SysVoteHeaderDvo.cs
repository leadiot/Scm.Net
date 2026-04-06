using Com.Scm.Dvo;
using Com.Scm.Sys.Vote;
using Com.Scm.Sys.VoteDetail.Dvo;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.VoteHeader.Dvo;

/// <summary>
/// 投票表
/// </summary>
public class SysVoteHeaderDvo : ScmDataDvo
{
    /// <summary>
    /// 投票标题
    /// </summary>
    [Required]
    [StringLength(90)]
    public string title { get; set; }

    /// <summary>
    /// 投票类型（图文、视频、分组）
    /// </summary>
    [Required]
    public VoteTypeEnums type { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    [Required]
    public DateTime start_time { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    [Required]
    public DateTime end_time { get; set; }

    /// <summary>
    /// 防刷规则（IP限制）
    /// </summary>
    [Required]
    public bool swipe_rule { get; set; } = true;

    /// <summary>
    /// 文件地址
    /// </summary>
    public string file_url { get; set; }

    /// <summary>
    /// 规则说明
    /// </summary>
    public string summary { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<SysVoteDetailDvo> details { get; set; }
}