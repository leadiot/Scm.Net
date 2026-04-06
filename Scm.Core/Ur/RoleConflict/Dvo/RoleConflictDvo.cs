using Com.Scm.Dvo;

namespace Com.Scm.Ur.RoleConflict.Dvo;

/// <summary>
/// 角色互斥表
/// </summary>
public class RoleConflictDvo : ScmDataDvo
{
    /// <summary>
    /// 角色A
    /// </summary>
    public long rolea_id { get; set; }

    /// <summary>
    /// 角色A信息
    /// </summary>
    public string rolea_names { get; set; }

    /// <summary>
    /// 角色B
    /// </summary>
    public long roleb_id { get; set; }

    /// <summary>
    /// 角色B信息
    /// </summary>
    public string roleb_names { get; set; }

    /// <summary>
    /// 互斥说明
    /// </summary>
    public string remark { get; set; }
}