namespace Com.Scm.Log.Api.Dvo;

/// <summary>
/// 
/// </summary>
public class SearchRequest : ScmSearchPageRequest
{
    /// <summary>
    /// 
    /// </summary>
    public int Level { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
    public int Type { get; set; } = 0;
}