using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 角色表
/// </summary>
[SugarTable("scm_ur_role")]
public class RoleDao : ScmDataDao, ISortableDao, ISystemDao, IDeleteDao
{
    /// <summary>
    /// 角色编号
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string codec { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string names { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string namec { get; set; }

    /// <summary>
    /// 角色父节点
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 角色层级
    /// </summary>
    [Required]
    public int lv { get; set; } = 1;

    /// <summary>
    /// 角色设置最大数量 0为不限制
    /// </summary>
    [Required]
    public int max_length { get; set; } = 0;

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; } = 1;

    /// <summary>
    /// 描述
    /// </summary>
    [StringLength(256)]
    [SugarColumn(Length = 256, IsNullable = true)]
    public string remark { get; set; }

    /// <summary>
    /// 系统标识
    /// </summary>
    [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
    public ScmRowSystemEnum row_system { get; set; } = ScmRowSystemEnum.No;

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
    public ScmRowDeleteEnum row_delete { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    public override void PrepareCreate(long userId)
    {
        base.PrepareCreate(userId);

        row_delete = ScmRowDeleteEnum.No;
        if (string.IsNullOrWhiteSpace(names))
        {
            names = namec;
        }
    }
}