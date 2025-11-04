namespace Com.Scm.Adm.Menu.Dvo;

/// <summary>
/// 
/// </summary>
public class CreateRequest : ScmUpdateRequest
{
    /// <summary>
    /// 父级
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string name { get; set; }
}