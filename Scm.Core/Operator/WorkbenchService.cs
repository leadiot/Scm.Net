using Com.Scm.Dr.Web;
using Com.Scm.Enums;
using Com.Scm.Operator.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Operator;

/// <summary>
/// 工作台
/// </summary>
[ApiExplorerSettings(GroupName = "Scm")]
public class WorkbenchService : IApiService
{
    private readonly ISqlSugarClient _sqlClient;
    public WorkbenchService(ISqlSugarClient sqlClient)
    {
        _sqlClient = sqlClient;
    }

    /// <summary>
    /// 获得资源使情况
    /// </summary>
    /// <returns></returns>
    public DeviceUse GetResourceUse()
    {
        return DeviceUtils.GetInstance().GetSystemRateInfo();
    }

    /// <summary>
    /// 获得系统信息
    /// </summary>
    /// <returns></returns>
    public dynamic GetSystemInfo()
    {
        return DeviceUtils.GetInstance().GetSystemInfo();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public dynamic GetServerInfo()
    {
        return new Dictionary<string, string>();
    }

    /// <summary>
    /// 获取摘要信息
    /// </summary>
    public HomeSummaryResponse GetSummaryAsync()
    {
        var response = new HomeSummaryResponse();

        var summaries = new List<HomeSummary>();
        summaries.Add(CreateSummary("今日总单量", "今日进入系统的所有订单数量", 10000, 100));
        summaries.Add(CreateSummary("今日待发货", "今日进入单量中待发货数量", 10000, 100));
        summaries.Add(CreateSummary("月度活跃用户", "本月所有活跃用户数量", 10000, 100));
        summaries.Add(CreateSummary("月度新增用户", "本月所有新增用户数量", 10000, 100));
        response.summaries = summaries;

        return response;
    }

    private HomeSummary CreateSummary(string title, string tooltip, int max, int min)
    {
        var random = new Random();

        var summary = new HomeSummary();
        summary.title = title;
        summary.tooltip = tooltip;
        summary.value = random.Next(0, max) + min;
        var rate = new HomeSummaryRate();
        rate.title = "环比增长";
        rate.value = random.Next(0, 50) - 25;
        rate.units = "%";
        summary.rate = rate;

        return summary;
    }

    /// <summary>
    /// 获取报表
    /// </summary>
    public async Task<HomeReportResponse> GetReportAsync()
    {
        var response = new HomeReportResponse();

        response.Titles = new List<string>();
        response.Data1 = new List<float>();
        response.Data2 = new List<float>();
        response.Data3 = new List<float>();

        var now = DateTime.Now;
        //var start = now.AddMonths(-1);
        var start = now.AddDays(-14);
        var tmp = start;
        while (tmp <= now)
        {
            response.Titles.Add(TimeUtils.FormatDate(tmp));
            tmp = tmp.AddDays(1);
        }

        //tmp = start;
        var list = await GetListAsync(start, now);
        foreach (var item in list)
        {
            response.Data1.Add(item.pv);

            response.Data2.Add(item.uv);
        }

        return response;
    }

    public async Task<List<ScmDrWebDailyDao>> GetListAsync(DateTime start, DateTime end)
    {
        var days = (int)(end - start).TotalDays;
        var day = TimeUtils.FormatDate(start);

        var daoList = await _sqlClient.Queryable<ScmDrWebDailyDao>()
            .Where(a => a.row_status == ScmRowStatusEnum.Enabled && a.day.CompareTo(day) >= 0)
            //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
            //.OrderBy(m => m.day)
            .ToListAsync();

        if (daoList.Count < days)
        {
            var random = new Random();
            var appendList = new List<ScmDrWebDailyDao>();
            while (start <= end)
            {
                var tmp = TimeUtils.FormatDate(start);
                var dao = daoList.FirstOrDefault(a => a.day == tmp);
                if (dao == null)
                {
                    var pv = random.Next(10, 10000);
                    var uv = random.Next(1, pv);
                    dao = new ScmDrWebDailyDao
                    {
                        day = tmp,
                        pv = pv,
                        uv = uv,
                    };
                    appendList.Add(dao);
                }
                start = start.AddDays(1);
            }
            await _sqlClient.Insertable(appendList).ExecuteCommandAsync();
        }

        var result = await _sqlClient.Queryable<ScmDrWebDailyDao>()
            .Where(a => a.row_status == ScmRowStatusEnum.Enabled && a.day.CompareTo(day) >= 0)
            //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
            .OrderBy(m => m.day)
            .ToListAsync();

        return result;
    }

    /// <summary>
    /// 获取待办
    /// </summary>
    public HomeTodoResponse GetTodoAsync()
    {
        var response = new HomeTodoResponse();
        var list = new List<HomeTodo>();
        list.Add(new HomeTodo());
        list.Add(new HomeTodo());
        list.Add(new HomeTodo());
        list.Add(new HomeTodo());
        list.Add(new HomeTodo());

        var items = new Dictionary<int, List<HomeTodo>>();
        items.Add(1, list);

        response.items = items;
        return response;
    }
}