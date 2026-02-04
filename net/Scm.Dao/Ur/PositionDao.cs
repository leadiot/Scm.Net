using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 岗位表
/// </summary>
[SugarTable("scm_ur_position")]
public class PositionDao : ScmDataDao, IDeleteDao
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [StringLength(16)]
    [SugarColumn(Length = 16)]
    public string codes { get; set; }

    /// <summary>
    /// 岗位编码
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
    /// 岗位名称
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string namec { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; } = 1;

    /// <summary>
    /// 备注信息
    /// </summary>
    [StringLength(256)]
    [SugarColumn(Length = 256, IsNullable = true)]
    public string remark { get; set; }

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
        codes = UidUtils.NextCodes("scm_ur_position");
    }
}