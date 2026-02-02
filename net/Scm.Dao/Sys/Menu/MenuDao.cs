using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Menu;

/// <summary>
/// 资源菜单表
/// </summary>
[SugarTable("scm_sys_menu")]
public class MenuDao : ScmDataDao, ISortableDao, IDeleteDao
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
    [Required]
    [StringLength(8)]
    [SugarColumn(Length = 8)]
    public string lang { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string codec { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [Required]
    [StringLength(32)]
    [SugarColumn(Length = 32)]
    public string namec { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    [StringLength(64)]
    [SugarColumn(Length = 64, IsNullable = true)]
    public string icon { get; set; }

    /// <summary>
    /// 父节点
    /// </summary>
    public long pid { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [Required]
    [StringLength(128)]
    [SugarColumn(Length = 128)]
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
    [StringLength(128)]
    [SugarColumn(Length = 128, IsNullable = true)]
    public string active { get; set; }

    /// <summary>
    /// 重定向，暂未使用
    /// </summary>
    [StringLength(256)]
    [SugarColumn(Length = 256, IsNullable = true)]
    public string redirect { get; set; }

    /// <summary>
    /// 文件路径
    /// </summary>
    [StringLength(128)]
    [SugarColumn(Length = 128, IsNullable = true)]
    public string view { get; set; }

    /// <summary>
    /// 文本颜色
    /// </summary>
    [StringLength(32)]
    [SugarColumn(Length = 32, IsNullable = true)]
    public string color { get; set; }

    /// <summary>
    /// 删除状态
    /// </summary>
    public ScmRowDeleteEnum row_delete { get; set; }
}