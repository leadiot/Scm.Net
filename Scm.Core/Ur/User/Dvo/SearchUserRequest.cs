namespace Com.Scm.Ur.User.Dvo;

/// <summary>
/// 
/// </summary>
public class SearchUserRequest : ScmSearchPageRequest
{
    /// <summary>
    /// 
    /// </summary>
    public long organize_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long position_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long group_id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long role_id { get; set; }
}