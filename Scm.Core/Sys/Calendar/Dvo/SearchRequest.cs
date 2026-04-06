namespace Com.Scm.Sys.Calendar.Dvo;

/// <summary>
/// 
/// </summary>
public class SearchRequest : ScmSearchRequest
{
    /// <summary>
    /// 
    /// </summary>
    public long user_id { get; set; }
    /// <summary>
    /// 根据类型查询
    /// </summary>
    public long types { get; set; }

    /// <summary>
    /// 根据等级查询
    /// </summary>
    public long level { get; set; }

    /// <summary>
    /// 重要性
    /// </summary>
    public bool important { get; set; }
    /// <summary>
    /// 紧急性
    /// </summary>
    public bool urgent { get; set; }

    /// <summary>
    /// 根据日期查询
    /// </summary>
    public DateTime? date { get; set; }
}