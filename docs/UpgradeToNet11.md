# Scm.Net 升级到 .NET 11.0 实录

> 升级日期：2026-11-12　｜　应用版本：10.21.50 → 11.0.0（BUILD 100）　｜　SDK：11.0.100

Scm.Net 是一个基于 .NET 的企业级中后台快速开发框架，由 36 个项目、4 个解决方案文件夹（`Scm.App`、`Scm.Common`、`Scm.Module`、`Scm.Server`）以及 `Samples`、`Test`、`WinForm` 组成。本文记录这次从 .NET 10.0 整体迁移到 .NET 11.0 的过程：改了什么、踩了哪些坑、破坏性变更如何修复。

---

## 一、升级策略

.NET 11 与 .NET 10 之间不存在运行时层面的颠覆性变化，真正的工作量来自**同批次升级的第三方库**——它们恰好都在这轮做了大版本重写：

- Quartz.NET 3.x → 4.7.3（调度器 API 整体重做）
- CsvHelper → 34.0.0（写出 API 重写）
- MailKit / MimeKit 4.18.2 → 5.0.0（发送 API 改为请求对象模型）

因此升级被拆成两条并行主线：**框架目标版本**与**依赖版本**。前者机械、可以脚本化；后者需要逐个看懂新 API。

### 全量升级后的目标框架分布

| 项目类型 | 目标框架 | 说明 |
| --- | --- | --- |
| Web 宿主、服务、模块 | `net11.0` | `Scm.Net`、`Scm.Core`、`Scm.Server.*`、`Scm.*` 等 |
| 桌面测试客户端 | `net11.0-windows` | `WinForm` |
| 公共基础库 | `netstandard2.0` | `Scm.Common`、`Scm.Common.Dto`、`Scm.Common.Log`、`Scm.Common.Excel`、`Scm.Common.Os` |

`netstandard2.0` 的公共库**刻意保持不动**。它们要同时被 .NET 11 宿主和可能存在的旧版工具链消费，向上兼容是 .NET 的一贯承诺，没有升级的必要。

### 依赖版本矩阵

| 包 | 升级前 | 升级后 |
| --- | --- | --- |
| Microsoft.AspNetCore.* / Microsoft.Extensions.* | 10.0.14 | 11.0.0 |
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.14 | 11.0.0 |
| Microsoft.AspNetCore.Mvc.NewtonsoftJson | 10.0.14 | 11.0.0 |
| Microsoft.AspNetCore.OpenApi | 10.0.14 | 11.0.0 |
| Swashbuckle.AspNetCore.SwaggerUI | — | 11.0.0 |
| Quartz / Quartz.AspNetCore | 3.20.0 | 4.7.3 |
| CsvHelper | 33.x | 34.0.0 |
| MailKit / MimeKit | 4.18.2 | 5.0.0 |
| MiniExcel | 1.46.0 | 1.47.0 |
| System.Management / System.Text.Encodings.Web | 10.0.14 | 11.0.0 |

宿主项目本身没有代码改动，`Program.cs` 的流式管道保持原样：

```csharp
WebApplication.CreateBuilder(args)
    .ConfigureServices()   // ScmStartup.ConfigureServices()
    .Build()
    .ConfigureMiddleware() // ScmStartup.ConfigureMiddleware()
    .Run();
```

同样，`ScmStartup` 里的模块串联顺序（Env → Sql → Cache → MQTT → Quartz → Jwt → Scalar → Dynamic API）也不需要调整。

---

## 二、预编译库如何处理

`Libs/` 下有一批闭源 DLL（`Scm.Cache.dll`、`Scm.Uid.dll`、`Upgrade.Net.dll` 等），按 `HintPath` 直接引用。这次**没有**把它们改名为 `Libs/net11.0`，而是继续引用 `Libs/net10.0`：

```xml
<Reference Include="Scm.Cache">
  <HintPath>..\Libs\net10.0\Scm.Cache.dll</HintPath>
</Reference>
```

原因是这些 DLL 只用了稳定的 BCL 表面（缓存、UID、HTTP、反射），在 .NET 11 运行时上加载和运行都正常。`Libs/netstandard2.0` 下的那批（`Scm.Common.File.dll`、`Scm.Common.Http.dll`、`Scm.Common.Text.dll` 等）本来就是跨版本兼容的，更无需改动。

> 实践建议：目录名只代表"编译时所用的 TFM"，不代表"运行时只能跑在哪个版本"。真正需要重建的是用到了被移除 API 的库；纯工具类库不必为了目录名好看而重编。

---

## 三、破坏性变更与修复

### 1. Quartz：`Quartz.Spi` 命名空间消失

第一个报错很直白：

```
命名空间"Quartz"中不存在类型或命名空间名"Spi"(是否缺少程序集引用?)
```

Quartz 4.x 是一次彻底重写，原有的扩展点几乎全部搬走：

- `Quartz.Spi` 命名空间被移除，`IJobFactory` 不再位于其中
- `IScheduler` 失去 `JobFactory` 属性
- 列表类查询方法 `GetJobKeys` / `GetTriggersOfJob` / `GetJobGroupNames` 被统一为分页查询
- `IJob.Execute(IJobExecutionContext)` 改为 `ValueTask Execute(IJobExecutionContext, CancellationToken)`
- `ITrigger` 上的 `Name` / `Group` / `JobName` / `JobGroup` 合并为 `Key` 与 `JobKey`

#### 1.1 删除自定义 JobFactory

原实现依赖 `IScheduler.JobFactory` 注入 DI 工厂。4.x 中这个属性已不存在，而 `AddQuartz` 内置的 Microsoft DI 集成本来就会**每次触发创建一个 Scope**，行为与手写工厂一致，因此直接删除：

```csharp
// 升级前
services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
services.AddSingleton<IJobFactory, JobFactory>();

// 升级后：调度器、线程池、DI 感知的 JobFactory 全部由 AddQuartz 装配
services.AddQuartz(q => q.UseInMemoryStore());
```

#### 1.2 查询 API 改为 Query*

```csharp
// 升级前
var groups = await scheduler.GetJobGroupNames();
foreach (var groupName in groups)
{
    foreach (var jobKey in await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)))
    {
        var triggers = await scheduler.GetTriggersOfJob(jobKey);
        ...
    }
}

// 升级后
var jobs = await scheduler.QueryJobs(new JobQuery { Take = PagedQuery.All });
foreach (var job in jobs.Items)
{
    var triggers = await scheduler.QueryTriggers(new TriggerQuery { Job = job.Key, Take = PagedQuery.All });
    foreach (var trigger in triggers.Items)
    {
        var lastTime = trigger.PreviousFireTimeUtc?.LocalDateTime;
        ...
    }
}
```

注意 `QueryJobs` 返回的是 `JobHeader` 而不是完整的 `IJobDetail`——4.x 有意让列表查询不反序列化 `JobDataMap`，这是性能设计，不是功能缺失。

同时，`GroupMatcher` / `NameMatcher` 从 `Quartz.Impl.Matchers` 搬到了 `Quartz` 根命名空间，用过的 `using` 要删掉。

#### 1.3 Cron 表达式校验不再依赖触发器实例

旧代码靠 `CronTriggerImpl.ComputeFirstFireTimeUtc` 判断表达式是否合法，属于"用触发器验证表达式"的绕路写法。新版本直接用 `CronExpression`：

```csharp
var expression = CronExpression.Parse(cronExpression);
DateTimeOffset? date = expression.GetNextValidTimeAfter(DateTimeOffset.Now);
var iscron = date != null;
```

#### 1.4 按名称定位触发器

原代码大量出现这种模式：按分组取所有 `JobKey`，再强转 `ITrigger as CronTriggerImpl` 比较名称。4.x 中 `CronTriggerImpl` 已不是公开契约，改为一次性查询：

```csharp
private async Task<TriggerHeader> FindTrigger(IScheduler scheduler, string groupName, string triggerName)
{
    var triggers = await scheduler.QueryTriggers(new TriggerQuery
    {
        Group = GroupMatcher<TriggerKey>.GroupEquals(groupName),
        Name = NameMatcher<TriggerKey>.NameEquals(triggerName),
        Take = 1
    });
    return triggers.Items.FirstOrDefault();
}
```

暂停、恢复、立即执行、删除四类操作都改为先 `FindTrigger` 再调用对应方法。

#### 1.5 作业实现签名变更

```csharp
// 升级前
public async Task Execute(IJobExecutionContext context)
{
    AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
    var name = trigger.Name;
    var jobName = trigger.JobName;
}

// 升级后
public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
{
    ITrigger trigger = context.Trigger;
    var triggerName = trigger.Key.Name;
    var triggerGroup = trigger.Key.Group;
    var jobName = trigger.JobKey?.Name;
    var jobGroup = trigger.JobKey?.Group;
}
```

两个要点：

1. 不再强转 `JobExecutionContextImpl` / `AbstractTrigger`。这两个都是实现类型，4.x 明确不希望使用者依赖它们。
2. `CancellationToken` 是新的生命周期入口。调度器停机或调用 `Interrupt` 时会通过它通知作业，**所有 `await` 都应该把它传下去**，否则优雅停机就形同虚设。

### 2. CsvHelper 34：`CsvWriter` 构造函数没了

```
"CsvWriter"不包含采用 2 个参数的构造函数
```

CsvHelper 34 重写了写出路径，`new CsvWriter(TextWriter, CultureInfo)` 被移除，`NextRecord()`、`WriteHeader<T>()` 也一并消失，泛型 `WriteRecords<T>` 转而依赖 `[CsvRecord]` 源生成器。

框架里的导出逻辑不想引入源生成器约束，因此改为直接调用底层字段写入 API：

```csharp
// 升级前
using (var writer = new StreamWriter(file))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteHeader<T>();
    csv.NextRecord();
    csv.WriteRecords(records);
}

// 升级后
using (var writer = new StreamWriter(file))
using (var csv = CsvWriter.Create(writer, CsvWriterOptions.Default))
{
    WriteHead<T>(csv);      // 反射取属性名，逐列 WriteField
    csv.EndRecord();        // NextRecord 的替代
    WriteData(csv, records); // 反射取值，逐行 WriteField + EndRecord
}
```

三个映射关系值得记住：

| 旧 API | 新 API |
| --- | --- |
| `new CsvWriter(writer, CultureInfo)` | `CsvWriter.Create(writer, CsvWriterOptions.Default)` |
| `NextRecord()` | `EndRecord()` |
| `WriteHeader<T>()` | 自行 `WriteField(name)` 循环 |

`CsvWriterOptions.Default` 的 `FormatProvider` 默认就是 `InvariantCulture`，所以数字、日期、小数点的输出格式与升级前完全一致——这一点很容易被忽略，一旦换成 `CurrentCulture`，不同服务器上的导出文件就会出现小数点变逗号的差异。

涉及位置：`ScmTaskHandler`、`UserExportHandler`、`SingleTableExportHandler`。

### 3. MailKit 5.0：`MimeMessage` 不再是发送入口

```
参数 1: 无法从"MimeKit.MimeMessage"转换为"MailKit.ISendRequest"
```

MailKit 5.0 用请求对象替代了"直接发消息"的旧重载：

```csharp
// 升级前
string result = client.Send(message);

// 升级后
client.Send(new SendRequest(message));
```

异步版本同理：

```csharp
await client.SendAsync(new SendRequest(message));
```

注意 `SendRequest` 位于 `MailKit` 根命名空间（不在 `MailKit.Net.Smtp`），文件顶部需要补一行 `using MailKit;`。

`SendRequest(MimeMessage)` 会按原有规则从消息头推导发件人与收件人：发件人依次取 Resent-Sender → Resent-From → Sender → From，收件人取 To / Cc / Bcc。因此业务侧构造 `MimeMessage` 的代码（含 HTML 正文的 `TextPart`、`Multipart("mixed")`）一行都不用改。

返回值也从 `string` 变成了 `SendResult`，包含 `ResponseText`、`AcceptedRecipients`、`RejectedRecipients`。相比旧版只能拿到一段状态文本，现在可以精确判断"哪些收件人被拒收"，做重试或告警时更有用。

---

## 四、版本号与发布

版本信息集中在 `ScmServerEnv`：

```csharp
public const string VER_DATE = "2026-11-12";
public const string VER_CODE = "2026111201";
public const int BUILD = 100;
public const int PATCH = 0;
public const int MINOR = 0;
public const int MAJOR = 11;
```

由于目标框架发生了大版本跨越，`MAJOR` 从 10 提到 11、`MINOR` / `PATCH` 归零，最终对外版本为 **11.0.0**。构建号继续累加，便于 `VersionCheckJob` 与升级接口识别新旧。

---

## 五、验证

```bash
dotnet build Scm.Net.sln          # 36 个项目全量编译
cd Scm.Net && dotnet run          # 开发环境监听 :5000
```

验证要点：

1. **全量编译零错误**——分项目单独编译会掩盖引用传递问题，必须走解决方案。
2. **启动后访问 `/scalar`**——确认 OpenAPI 文档分组（`scm`、`samples`、`test`）仍然正常，说明动态 Web API 与 OpenApi 11.0 的集成没有回归。
3. **触发一次定时任务**——确认 Quartz 4.x 的 `AddQuartz` 装配与作业取消令牌链路可用。
4. **导出一次 CSV**——重点看中文、小数、日期列的格式。

---

## 六、经验清单

给准备做同类升级的团队：

1. **先升级框架，再升级库，最后改代码。** 一次性改所有东西，报错会互相淹没；分三步走，每一步的编译错误都可控。
2. **架构变更往往伪装成"命名空间找不到"。** `Quartz.Spi` 报错的背后是整个调度 API 重写，只补 `using` 是无解的。
3. **优先删代码，而不是适配代码。** 自定义 `JobFactory` 之所以能删，是因为官方 DI 集成已经等价实现。升级时先问"这个扩展点还需要吗"。
4. **注意隐含的全局行为差异。** CsvHelper 的 `FormatProvider`、MailKit 的收件人推导规则都属于"编译能过、行为可能变"的坑，要专门验一遍输出。
5. **取消令牌要贯穿到底。** .NET 11 里 `CancellationToken` 已经是作业、HTTP、EF 查询的标配，拿到手就往下传，别再吞掉。
6. **预编译库不必急于重编。** `net10.0` 的目标 DLL 在 .NET 11 进程里通常照常工作，先跑通再决定是否替换。
7. **`netstandard2.0` 项目保持不动。** 它们向上兼容，改目标框架反而会扩大影响面。
