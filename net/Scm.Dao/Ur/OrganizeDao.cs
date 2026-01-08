using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur;

/// <summary>
/// 组织机构表
/// </summary>
[SugarTable("scm_ur_organize")]
public class OrganizeDao : ScmDataDao, ISortableDao, IDeleteDao, IResDao
{
    /// <summary>
    /// 
    /// </summary>
    public string codes { get; set; }

    /// <summary>
    /// 机构编码
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [StringLength(32)]
    public string names { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [Required]
    [StringLength(32)]
    public string namec { get; set; }

    /// <summary>
    /// 父节点
    /// </summary>
    [Required]
    public long pid { get; set; }

    /// <summary>
    /// 部门层级
    /// </summary>
    [Required]
    public int lv { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int od { get; set; }

    /// <summary>
    /// 主管人员
    /// </summary>
    public long owner_id { get; set; }

    /// <summary>
    /// 主管人员(冗余)
    /// </summary>
    public string owner_names { get; set; }

    /// <summary>
    /// 删除状态
    /// </summary>
    public ScmDeleteEnum row_delete { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    public override void PrepareCreate(long userId)
    {
        base.PrepareCreate(userId);

        codes = UidUtils.NextCodes("scm_ur_organize");
        if (string.IsNullOrEmpty(names))
        {
            names = namec;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    public override void PrepareUpdate(long userId)
    {
        base.PrepareUpdate(userId);
        if (string.IsNullOrEmpty(names))
        {
            names = namec;
        }
    }
}