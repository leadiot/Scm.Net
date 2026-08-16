# AGENTS.md — Scm.Net

面向 AI 助手的精简指南，用于协作开发这个 .NET 10.0 企业级中后台快速开发框架。前端代码在独立仓库（`Scm.Vue`）。

---

## 运行 / 构建

| 命令 | 说明 |
|---------|---------|
| `cd Scm.Net && dotnet run` | 启动开发服务器（开发环境 Kestrel 监听 `:5000`，生产环境监听 `:9999`） |
| `dotnet build Scm.Net.sln` | 构建整个 33 项目的解决方案 |
| `cd Scm.Net && dotnet publish -c Release -o ./Publish` | 生产环境发布 |
| `cd Test && dotnet run` | 运行测试控制台程序（非单元测试框架） |
| `cd WinForm && dotnet run` | 启动 WinForm 测试客户端（MQTT 温度模拟器 / HTTP 测试表单） |

- **未配置单元测试框架**。`Test/Test.csproj` 是一个普通的控制台可执行程序（`<OutputType>Exe</OutputType>`），并非 xUnit/NUnit/MSTest。
- **未配置 CI/CD**、Docker、`global.json`、`nuget.config` 或代码风格/格式化配置。
- **Scalar** 为 API 文档 UI（Swagger 配置已注释掉）。开发环境下可通过 `/scalar` 访问。

---

## 架构

### 程序入口
`Scm.Net/Program.cs` 非常精简，通过流式管道完成引导：
```csharp
WebApplication.CreateBuilder(args)
    .ConfigureServices()   // ScmStartup.ConfigureServices()
    .Build()
    .ConfigureMiddleware() // ScmStartup.ConfigureMiddleware()
    .Run();
```
两个阶段均定义在 [ScmStartup.cs](file:///d:/workspace/Git/Scm.Net/Scm.Net/Configure/Startup/ScmStartup.cs)。`ConfigureServices` 按固定顺序串联所有模块（Env、Sql、Cache、MQTT、Quartz、Jwt、Scalar、Dynamic API）—— **顺序很重要**，因为后面的步骤依赖前面已注册的单例（例如过滤器依赖 `ScmContextHolder`）。

### 分层拓扑
解决方案文件夹对应运行时层次：

| 文件夹 | 项目 | 角色 |
|--------|----------|------|
| `Scm.App` | `Scm.Net`、`Scm.Core`、`Scm.Dao`、`Scm.Dto` | Web 宿主、业务逻辑、数据访问、DTO |
| `Scm.Common` | `Scm.Common`、`Scm.Common.Dto`、`Scm.Common.Log`、`Scm.Common.Os`、`Scm.Common.Excel` | 通用工具 |
| `Scm.Module` | `Scm.Dsa.Dba.Sugar`、`Scm.Dsa.Dfa.Json`、`Scm.Generator`、`Scm.Email`、`Scm.Phone`、`Scm.Mqtt`、`Scm.Ai` | ORM 封装、JSON 助手、代码生成、消息通知、AI 客户端 |
| `Scm.Server` | `Scm.Server`、`Scm.Server.*` | 服务端抽象与基础设施模块（API、Aiml、Cache、DAO、DVO、MQTT、Quartz、RabbitMQ、Scalar、Service、SignalR） |
| `Samples` | `Samples.Common`、`Samples.Server`、`Samples.Common.Dto`、`Samples.Server.Dao` | 示例扩展项目 |
| `Test` | `Test` | 控制台实验台 |
| `WinForm` | `WinForm` | 桌面测试客户端（MQTT 模拟器 + HTTP 表单） |

依赖关系（简化版）：
```
Scm.Net (host)
  → Scm.Core (业务服务)
    → Scm.Server (接口、基础服务)
      → Scm.Dao / Scm.Dto / Scm.Server.Dao / Scm.Server.Dvo
```

### 关键基类
- **`AppService`**（[Scm.Server/Service/AppService.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Service/AppService.cs)）—— 轻量基类，提供 `EnvConfig`、`ISqlSugarClient`、`IResHolder`、`ICacheService` 以及搜索缓存辅助方法。
- **`ApiService`**（[Scm.Server/Service/ApiService.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Service/ApiService.cs)）—— 功能更丰富的 CRUD 基类：`GetByIdAsync`、`UpdateStatus`、`DeleteRecord`、导出/导入任务辅助方法。
- 新增业务服务通常继承自 `ApiService`，并由动态 Web API 系统自动对外暴露。

---

## 约定

### 项目设置（通用）
每个 `.csproj` 均使用：
- `<TargetFramework>net10.0</TargetFramework>`
- `<ImplicitUsings>enable</ImplicitUsings>`
- `<Nullable>disable</Nullable>`
- `<RootNamespace>Com.Scm</RootNamespace>`（`Samples.*` 使用 `Com.Scm.Samples`）

### 服务自动发现
- 配置程序集中所有**名称以 `Service` 结尾**的类，会通过 [DllExtension.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Extensions/DllExtension.cs) 自动以 `Scoped` 方式注册到 DI 容器（`Assembly.Load(name)` + `Name.EndsWith("Service")`）。
- 这些类同时会通过 `services.AddDynamicWebApi()`（来自 `Scm.Server.Api`）自动暴露为**动态 Web API 控制器**。
- **不要为标准 CRUD 编写显式的 Controller**；应直接实现 `*Service` 类。显式控制器仅用于非动态场景（如 `DbController`、`HbController`）。

### 数据库
- **ORM**：SqlSugarCore（在 `ScmStartup.SqlSetup` 中注册 `SqlSugarScope` 单例）。
- **默认数据库**：SQLite（`Data Source=data/scm.db;`）。
- **设计原则**：优先使用单表操作（最多 2 张表）。避免使用数据库特定的 SQL 特性。
- 在 SQLite 中枚举属性映射为 `INTEGER`，在其他引擎中映射为 `TINYINT`。
- 表结构初始化：启动时 `ScmDbHelper` + `SamplesDbHelper` 会执行 `InitDb()`——实体属性按 `DbType` 决定字段类型（参见 [ScmStartup.cs](file:///d:/workspace/Git/Scm.Net/Scm.Net/Configure/Startup/ScmStartup.cs) 中的 `EntityService` 回调）。

### DTO 命名
DTO 属性使用 **snake_case**（例如 `user_name`、`create_time`），而非 camelCase。JSON 序列化使用 Newtonsoft.Json（`Microsoft.AspNetCore.Mvc.NewtonsoftJson`）。

### 对象映射
- 使用 **Mapster**（而非 AutoMapper）。通过 `MapperRegister : IRegister` 注册配置，并调用 `services.AddMapperProfile()` 启用。
- DVO（`*Dvo`）= 返回给前端的视图对象；DTO（`*Dto`）= 数据传输契约；DAO（`*Dao`）= 映射到数据库表的 SqlSugar 实体。

---

## 认证与授权

共存三种 JWT 方案（参见 [ScmToken.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Token/ScmToken.cs)）：

| Scheme | 前缀 | 用途 |
|--------|--------|----------|
| `Bearer` | `Bearer ` | 标准 Web 用户 JWT（支持通过 `X-Refresh-Token` 无感续期） |
| `Operator` | `Operator ` | 操作员/会话登录 |
| `Terminal` | `Terminal ` | 终端绑定令牌（Base64 编码的 `terminal_id:user_id:time:digest`） |

- [ScmAuthHandler.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Token/ScmAuthHandler.cs) 是抽象分发器：它按 scheme 前缀拆分 `Authorization` 头，并交给匹配的处理器处理。
- `JwtMiddleware` 在 `UseAuthentication`/`UseAuthorization` **之后**解析 Claims 并填充 `IJwtTokenHolder`。
- **令牌持有者使用 `AsyncLocal<ScmToken>`**（而非 `ThreadLocal`），因此当前用户上下文在 ASP.NET Core 的 `await` 线程切换后依然保留。切勿改回 `ThreadLocal`——异步执行流程会丢失令牌。
- 在后台服务（`IHostedService`、Quartz 任务）中访问 `SugarRepository` 前必须自行设置令牌，因为不存在 HTTP 上下文。

---

## MQTT

两个相互配合的项目：
- **`Scm.Mqtt`** —— 可移植客户端/契约（`BrokerConfig`、`ClientConfig`、`IMqttPublisher`、`IMqttSubscriber`）。任何程序集都可以安全引用（`WinForm` 测试客户端即如此）。
- **`Scm.Server.MQTT`** —— 宿主侧实现，包括 `MqttBrokerService` + `MqttClientService` + DI 扩展方法。

注册统一在 `ScmStartup` 中完成：
```csharp
services.SetupMqtt(mqttBrokerConfig, mqttClientConfig);
```

配置（`appsettings.json`）：
```json
"MqttBroker": { "Enabled": true, "Port": 1883, "RequireAuthentication": false },
"MqttClient": { "Enabled": true, "Host": "localhost", "Port": 1883 }
```

**关键默认值**：`BrokerConfig.Enabled` 默认值为 `false`。仅当配置节显式设置 `"Enabled": true` 时才启动 Broker。客户端无论如何都会注册（因此也可连接外部 Broker）。

**启动顺序陷阱**：Broker 是一个 `IHostedService`；客户端（在 `SamplesMqttHostedService` 中）可能在 Broker 尚未开始监听前就运行了。客户端侧的 HostedService 务必实现重试/指数退避循环。

---

## Quartz（定时任务）

- 配置：`appsettings.json → Quartz`（`BaseDir`、`JobFile`、`LogsDir`、`DataDir`）。
- 由 `QuartzConfig.Type` 选择两种存储模式之一：
  - `"file"`（默认）→ `DfQuartzJobService` + `QuartzFileHelper`（任务定义于 `quartz/settings/jobs.json`）。
  - 其他任意值 → `DbQuartzJobService`（任务持久化到数据库表）。
- **自定义任务契约**：实现 `ICustomJob`（`string ExecuteService(string parameter)`）。所有加载 DLL 中的 `ICustomJob` 实现会通过 `AddQuartzClassJobs()` 自动以 Scoped 注册（扫描 `BaseDirectory` 下的 `*.dll`）。
- 内置任务：`ApiClientJob`（HTTP 回调）、`DllMethodJob`（反射调用方法）、`VersionCheckJob`（每日版本检查，参见升级章节）。
- `UseQuartz()` 在应用启动时初始化调度器。

---

## 升级 / 版本检测

- `appsettings.json` 中的 `Env.CheckUpgradeUrl` 是版本信息源地址，返回 `ScmVerInfo`（版本号 + 下载地址）。
- `VersionCheckJob`（Quartz 任务）每日轮询该 URL；发现新版本时将一行记录写入 `scm_sys_upgrade_info`（`ScmUpgradeInfoDao`）。
- [UpgradeController](file:///d:/workspace/Git/Scm.Net/Scm.Net/Controllers/UpgradeController.cs) 对外暴露 `CheckUpdateAsync` / `HasNewVersionAsync` / `GetVersionHistoryAsync` / `DownloadUpdate`，供前端提示 + 升级流程使用。
- 升级启动器为 `Upgrade.Net.dll`（位于 `Libs/net10.0/`）；路径可通过 `Env.UpgradeFilePath` / `Env.UpgradeJsonPath` 配置。
- **升级过程中切勿调用 `Environment.Exit(0)`** 来终止宿主——应使用 `IHostApplicationLifetime.StopApplication()`，以保证 HostedService 与数据库连接能优雅关闭。

---

## AI / RAG 集成

- 配置：`appsettings.json → Ai`（默认服务商：`deepseek`、`qwen`）。
- `Scm.Ai` 包含请求/响应 DVO（`AiChatRequest`、`AiChatResponse`、`AiEmbeddingResult` 等）。
- `Scm.Core/Ai` 实现 `ScmAiDocService`（知识库 CRUD）与 `AiRagCore`（分块 + 检索）。
- `Ai.Rag` 下的 RAG 参数：`DocDir`、`ChunkSize`、`ChunkOverlap`、`TopK`、`MinScore`、`EmbedBatch`。
- **API Key 为敏感信息**——在 `appsettings.json` 中将 `ApiKey` 留空，并通过环境变量 `SCM__AI__PROVIDERS__<index>__APIKEY` 注入（双下划线分隔层级，index 为数组下标）。
- 完整流程参见 [docs/AiIntegration.md](file:///d:/workspace/Git/Scm.Net/docs/AiIntegration.md)。

---

## 配置与密钥

### 配置文件
- `Scm.Net/appsettings.json` —— 已提交的默认配置（Kestrel `:9999`、SQLite、Redis 缓存、MQTT 开启、Quartz 文件模式）。
- `Scm.Net/appsettings.Development.json` —— 开发环境覆盖（Kestrel `:5000`、Scalar 开启）。
- `Scm.Net/Properties/launchSettings.json` —— 启动配置。
- `env.example` —— 示例环境变量（已提交）。

### 敏感值（永远不要提交）
以下内容均已加入 **.gitignore**：
- `appsettings.local.json`、`appsettings.development.json`、`appsettings.production.json`
- `.env`、`.env.local`、`**/secrets.json`
- `data/`、`upload/`、`images/`、`logs/`、`generator/`

开发环境使用 **User Secrets**：
```bash
cd Scm.Net
dotnet user-secrets init --project Scm.Net
dotnet user-secrets set "Sql:Text" "..." --project Scm.Net
```

生产环境使用**环境变量**：
```bash
export SCM__SQL__TEXT="..."
export SCM__JWT__SECURITY="..."
export SCM__AI__PROVIDERS__0__APIKEY="..."
```
（前缀 `SCM__`，层级之间用双下划线；数组索引用数字。）

完整密钥策略参见 [docs/SecureConfiguration.md](file:///d:/workspace/Git/Scm.Net/docs/SecureConfiguration.md)。

---

## 预编译库

`Libs/` 目录包含闭源 DLL，通过 HintPath 直接引用：
- `Libs/net10.0/` —— `Scm.Cache.dll`、`Scm.Uid.dll`、`Scm.Plugin.Image.dll`、`Scm.Aiml.dll`、`Scm.Workflow.dll`、`Upgrade.Net.dll`，以及缓存 Provider（`Scm.Cache.Redis/Memory/Garnet.dll`）。
- `Libs/netstandard2.0/` —— `Scm.Common.dll`、`Scm.Common.File.dll`、`Scm.Common.Http.dll`、`Scm.Common.Otp.dll`、`Scm.Common.Text.dll`、`Scm.Common.Time.dll`。

不要删除这些文件，也不要期望获取其源代码。新增 `Libs/` 引用时，也需要在使用的 `.csproj` 中添加 `<Reference Include="..."><HintPath>...</HintPath></Reference>`。

---

## 动态 API / 插件加载

- [DllExtension.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Extensions/DllExtension.cs) 加载 `DllConfig`（来自 `appsettings.json → Project:Service`）中列出的程序集，并自动将 `*Service` 类注册为 Scoped。
- 默认配置的服务程序集：`["Scm.Core"]`。
- 新增插件/模块 DLL 时，将其程序集名称追加到 `appsettings.json` 的 `Project:Service` 中。运行时该 DLL 必须位于宿主的 `BaseDirectory` 下。
- `AddDynamicWebApi()` 将这些服务暴露为 REST 端点。API 文档分组由每个服务上的 `[ApiExplorerSettings(GroupName = "...")]` 决定——group 代码必须与 `Scalar.ApiDocs` 中的条目匹配（如 `scm`、`samples`、`test`）。

---

## 缓存

- 抽象接口：`Com.Scm.Cache.ICacheService`（在 `Scm.Cache.dll` 中）。
- Provider 由 `appsettings.json` 的 `Cache.Type` 选择：
  - `"Redis"`（默认）—— `Scm.Cache.Redis.dll`；`Cache.Text` 为 StackExchange Redis 连接字符串。
  - `"Memory"` —— `Scm.Cache.Memory.dll`。
  - `"Garnet"` —— `Scm.Cache.Garnet.dll`。
- 在 `ScmStartup` 中通过 `services.CacheSetup(envConfig)` 接入。`ApiService` 使用它实现 `SaveSearch`/`ReadSearch` 查询条件缓存。

---

## SignalR

- Hub 端点：`app.MapHub<ScmHub>("/scmhub")`（定义于 `Scm.Server.SignalR`）。
- `JwtMiddleware` 确保已认证的连接携带 `ScmToken`；后台推送应通过 `Scm.Core/Msg` 下的 `SignalRUtil` 调用。

---

## 日志

- **Serilog** 从 `appsettings.json → Serilog` 读取配置。
- Enrichers（增强器）：`FromLogContext`、`WithMachineName`、`WithThreadId`（在 `OutputTemplate` 中使用 `{ThreadId}` 与 `{SourceContext}` 即可追踪线程与来源类）。
- Sinks（输出目标）：`File`（按天滚动，写入 `Logs/` 目录）和 `Console`。
- 调用入口：`LogUtils.Info/Debug/Error/Warning`（来自 `Scm.Common.Log`）。生产代码路径始终使用 `LogUtils`，不要使用 `Console.WriteLine`。
- SQL 日志：在 Debug 级别，`SqlSugarScope` 的 AOP `OnLogExecuting` 会记录每条已参数替换的 SQL（分类为 `"db"`）。

---

## 数据目录

运行时创建的目录（均已加入 .gitignore）。路径相对于 `Env.dataDir`（默认为 `data/`）解析：

| 路径 | Env 配置键 | 内容 |
|------|---------|----------|
| `data/` | `dataDir` | SQLite 数据库、SQL 脚本、字体、uid.db |
| `data/upload/` | `upload` | 上传的文件 |
| `data/images/` | `images` | 图片资源 |
| `logs/` | `logs` | Serilog 滚动日志 |
| `generator/` | `Generator.GeneratorDir` | 代码生成输出 |
| `quartz/settings/` | `Quartz.BaseDir` | Quartz 任务定义 |
| `data/ai/docs/` | `Ai.Rag.DocDir` | RAG 知识库 |

---

## 常见陷阱

1. **不要为领域 CRUD 编写显式的 API Controller**。应在 `Scm.Core` 中写 `*Service` 类，交由动态 Web API 系统暴露。
2. **Nullable 全局禁用**。不要假设可空引用类型会被编译器校验。
3. **Test 项目不是测试套件**。它只是一个控制台实验台，没有配置 `dotnet test` 运行器。
4. **前端代码在独立仓库**。不要在这里找 Vue/Vite 源码；前端项目在另一仓库 `Scm.Vue` 中。
5. **新增项目引用后请重新构建**。整个解决方案共 33 个项目；陈旧的构建产物会导致运行时报找不到程序集的错误。
6. **`BrokerConfig` 中 MQTT Broker 默认为 `Enabled = false`**。仅当配置节显式启用时才会启动 Broker。客户端注册不受限制——可以指向外部 Broker。
7. **MQTT 启动顺序**：客户端 HostedService 可能在 Broker 开始监听前就运行。应在客户端的 `StartAsync`/`ExecuteAsync` 中实现重试/退避机制。
8. **对每请求上下文使用 `AsyncLocal`，而非 `ThreadLocal`**。`ThreadLocal<ScmToken>` 在 ASP.NET Core 中的 `await` 线程切换后会丢失状态。
9. **`multipart/form-data` 上传必须加 `[FromForm]`**。否则 `path`/`file` 等字段会绑定为 `null`。若要同时支持 URL 参数与表单绑定，可在代码中从 `HttpContext.Request.Query[...]` 读取回退值。
10. **`Enabled` 之类的默认值往往是陷阱**。许多配置类（如 `BrokerConfig`）的 C# 默认值并非零值；总是在 `appsettings.json` 中显式设置，不要依赖 C# 默认。
11. **后台服务没有 `HttpContext`**。Quartz 任务与 `IHostedService` 实现在使用 `SugarRepository` 之前，必须自行调用 `IJwtTokenHolder.SetToken(...)`，否则数据权限过滤会基于空令牌执行。
12. **不要从请求处理器中调用 `Environment.Exit()`**。升级流程中应使用 `IHostApplicationLifetime.StopApplication()` 进行优雅关闭。

---

## 延伸阅读

- [docs/AiIntegration.md](file:///d:/workspace/Git/Scm.Net/docs/AiIntegration.md) —— AI 服务商 + RAG 管线详情。
- [docs/SecureConfiguration.md](file:///d:/workspace/Git/Scm.Net/docs/SecureConfiguration.md) —— User Secrets / 环境变量策略。
- [docs/UnifiedApiResponse.md](file:///d:/workspace/Git/Scm.Net/docs/UnifiedApiResponse.md) —— 统一响应封装约定。
