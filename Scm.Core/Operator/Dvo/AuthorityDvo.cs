using System.Text.Json.Serialization;

namespace Com.Scm.Operator.Dvo;

/// <summary>
/// 返回给前端的权限菜单
/// ElementPlus专用
/// </summary>
public class AuthorityDvo
{
    /// <summary>
    /// 菜单ID
    /// </summary>
    public long id { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    public string path { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 组件
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string component { get; set; }

    /// <summary>
    /// 重定向
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string redirect { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    public AuthorityMeta meta { get; set; }

    //public bool alwaysShow { get; set; } = true;

    /// <summary>
    /// 子级
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<AuthorityDvo> children { get; set; }
}

/// <summary>
/// 
/// </summary>
public class AuthorityMeta
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string title { get; set; }

    /// <summary>
    /// 图表
    /// </summary>
    public string icon { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool affix { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string type { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool? hidden { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonIgnore]
    public bool? dynamicNewTab { get; set; }

    /// <summary>
    /// 是否全屏
    /// </summary>
    public bool? fullpage { get; set; }

    /// <summary>
    /// 是否关闭
    /// </summary>
    [JsonIgnore]
    public bool noClosable { get; set; }

    /// <summary>
    /// 不缓存
    /// </summary>
    [JsonIgnore]
    public bool keepAlive { get; set; } = true;

    /// <summary>
    /// 按钮权限
    /// </summary>
    [JsonIgnore]
    public string[] roles { get; set; }
}