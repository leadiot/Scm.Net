namespace Com.Scm.Log.Api.Dvo;

/// <summary>
/// 15日 图表
/// </summary>
public class SysLogChartResponse
{
    /// <summary>
    /// 日期
    /// </summary>
    public List<string> Time { get; set; } = new();

    /// <summary>
    /// 数量
    /// </summary>
    public List<List<int>> Count { get; set; } = new();
}