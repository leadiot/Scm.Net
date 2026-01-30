using Com.Scm.Enums;

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
    /// 终端类型
    /// </summary>
    public ScmClientTypeEnum client { get; set; }

    /// <summary>
    /// 语言
    /// </summary>
    public string lang { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    public string name { get; set; }
}