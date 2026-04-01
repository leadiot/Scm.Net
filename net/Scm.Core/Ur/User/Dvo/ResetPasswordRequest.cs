using Com.Scm.Request;

namespace Com.Scm.Ur.User.Dvo;

/// <summary>
/// 
/// </summary>
public class ResetPasswordRequest : ScmRequest
{
    /// <summary>
    /// 
    /// </summary>
    public List<long> ids { get; set; }
}