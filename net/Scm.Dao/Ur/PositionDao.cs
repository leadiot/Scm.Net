using Com.Scm.Dao;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 岗位表
/// </summary>
[SugarTable("scm_ur_position")]
public class PositionDao : ScmDataDao
{
    /// <summary>
    /// 
    /// </summary>
    public string codes { get; set; }

    /// <summary>
    /// 岗位编码
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string names { get; set; }

    /// <summary>
    /// 岗位名称
    /// </summary>
    [Required]
    [StringLength(32)]
    public string namec { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; } = 1;

    /// <summary>
    /// 备注信息
    /// </summary>
    public string remark { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    public override void PrepareCreate(long userId)
    {
        base.PrepareCreate(userId);

        codes = UidUtils.NextCodes("scm_ur_position");
    }
}