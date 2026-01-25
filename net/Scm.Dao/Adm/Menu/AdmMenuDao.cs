using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Adm.Menu;

/// <summary>
/// 资源菜单表
/// </summary>
[SugarTable("scm_sys_menu")]
public class AdmMenuDao : ScmDataDao, ISortableDao, IDeleteDao
{
    /// <summary>
    /// 终端类型
    /// </summary>
    [Required]
    public ScmClientTypeEnum client { get; set; }

    /// <summary>
    /// 菜单类型
    /// </summary>
    [Required]
    public ScmMenuTypesEnum types { get; set; }

    /// <summary>
    /// 语言
    /// </summary>
    public string lang { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    public string codec { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [Required]
    [StringLength(30)]
    public string namec { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string icon { get; set; }

    /// <summary>
    /// 父节点
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public string uri { get; set; }

    /// <summary>
    /// 菜单层级
    /// </summary>
    [Required]
    public int layer { get; set; } = 1;

    /// <summary>
    /// 排序
    /// </summary>
    public int od { get; set; } = 1;

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool visible { get; set; } = true;

    /// <summary>
    /// 是否使能
    /// </summary>
    public bool enabled { get; set; } = true;

    /// <summary>
    /// 是否全屏
    /// </summary>
    public bool fullpage { get; set; } = false;

    /// <summary>
    /// 是否缓存
    /// </summary>
    public bool keepAlive { get; set; } = true;

    /// <summary>
    /// 高亮，暂未使用
    /// </summary>
    public string active { get; set; }

    /// <summary>
    /// 重定向，暂未使用
    /// </summary>
    public string redirect { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    public string view { get; set; }

    /// <summary>
    /// 文本颜色
    /// </summary>
    public string color { get; set; }

    /// <summary>
    /// 删除状态
    /// </summary>
    public ScmRowDeleteEnum row_delete { get; set; }

    public override void PrepareCreate(long userId)
    {
        base.PrepareCreate(userId);
        row_delete = ScmRowDeleteEnum.No;
    }
}