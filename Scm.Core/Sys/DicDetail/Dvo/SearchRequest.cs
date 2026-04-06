namespace Com.Scm.Sys.DicDetail.Dvo;

/// <summary>
/// 
/// </summary>
public class SearchRequest : ScmSearchPageRequest
{
    /// <summary>
    /// 字典栏目Code
    /// </summary>
    public string codec { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int tag { get; set; }
}