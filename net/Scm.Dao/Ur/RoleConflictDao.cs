using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 角色互斥表
/// </summary>
[SugarTable("scm_ur_role_conflict")]
public class RoleConflictDao : ScmDataDao
{
    /// <summary>
    /// 角色A
    /// </summary>
    [Required]
    public long rolea_id { get; set; }

    /// <summary>
    /// 角色B
    /// </summary>
    [Required]
    public long roleb_id { get; set; }

    /// <summary>
    /// 互斥说明
    /// </summary>
    [Required]
    [StringLength(256)]
    [SugarColumn(Length = 256)]
    public string remark { get; set; }
}