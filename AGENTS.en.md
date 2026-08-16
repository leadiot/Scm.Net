# AGENTS.md — Scm.Net

Compact guide for AI agents working in this .NET 10.0 enterprise backend framework (Chinese: 企业级中后台快速开发框架). Frontend lives in a separate repo (`Scm.Vue`).

---

## Run / Build

| Command | Context |
|---------|---------|
| `cd Scm.Net && dotnet run` | Start dev server (Kestrel `:5000` in Development, `:9999` in Production) |
| `dotnet build Scm.Net.sln` | Build entire 33-project solution |
| `cd Scm.Net && dotnet publish -c Release -o ./Publish` | Production publish |
| `cd Test && dotnet run` | Run the test console app (not a unit-test runner) |
| `cd WinForm && dotnet run` | Launch WinForm test client (MQTT temperature simulator / HTTP test form) |

- **No unit-test framework** is configured. `Test/Test.csproj` is a plain console executable (`<OutputType>Exe</OutputType>`), not xUnit/NUnit/MSTest.
- **No CI/CD**, Docker, `global.json`, `nuget.config`, or lint/format config exists.
- **Scalar** is the API docs UI (Swagger setup is commented out). Available at `/scalar` in Development.

---

## Architecture

### Entry Point
`Scm.Net/Program.cs` is minimal and bootstraps via a fluent pipeline:
```csharp
WebApplication.CreateBuilder(args)
    .ConfigureServices()   // ScmStartup.ConfigureServices()
    .Build()
    .ConfigureMiddleware() // ScmStartup.ConfigureMiddleware()
    .Run();
```
Both stages live in [ScmStartup.cs](file:///d:/workspace/Git/Scm.Net/Scm.Net/Configure/Startup/ScmStartup.cs). `ConfigureServices` wires up every module (Env, Sql, Cache, MQTT, Quartz, Jwt, Scalar, Dynamic API) in a fixed order — **order matters** because later steps depend on earlier singletons (e.g. Filters depend on `ScmContextHolder`).

### Layered Topology
Solution folders mirror runtime layers:

| Folder | Projects | Role |
|--------|----------|------|
| `Scm.App` | `Scm.Net`, `Scm.Core`, `Scm.Dao`, `Scm.Dto` | Web host, business logic, data access, DTOs |
| `Scm.Common` | `Scm.Common`, `Scm.Common.Dto`, `Scm.Common.Log`, `Scm.Common.Os`, `Scm.Common.Excel` | Shared utilities |
| `Scm.Module` | `Scm.Dsa.Dba.Sugar`, `Scm.Dsa.Dfa.Json`, `Scm.Generator`, `Scm.Email`, `Scm.Phone`, `Scm.Mqtt`, `Scm.Ai` | ORM wrapper, JSON helpers, codegen, messaging, AI client |
| `Scm.Server` | `Scm.Server`, `Scm.Server.*` | Server abstractions and infra modules (API, Aiml, Cache, DAO, DVO, MQTT, Quartz, RabbitMQ, Scalar, Service, SignalR) |
| `Samples` | `Samples.Common`, `Samples.Server`, `Samples.Common.Dto`, `Samples.Server.Dao` | Example extension projects |
| `Test` | `Test` | Console scratchpad |
| `WinForm` | `WinForm` | Desktop test clients (MQTT simulator + HTTP form) |

Dependency flow (simplified):
```
Scm.Net (host)
  → Scm.Core (business services)
    → Scm.Server (interfaces, base services)
      → Scm.Dao / Scm.Dto / Scm.Server.Dao / Scm.Server.Dvo
```

### Key Base Classes
- **`AppService`** ([Scm.Server/Service/AppService.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Service/AppService.cs)) — thin base providing `EnvConfig`, `ISqlSugarClient`, `IResHolder`, `ICacheService` and search-cache helpers.
- **`ApiService`** ([Scm.Server/Service/ApiService.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Service/ApiService.cs)) — richer CRUD base: `GetByIdAsync`, `UpdateStatus`, `DeleteRecord`, export/import task helpers.
- New business services typically extend `ApiService` and are auto-exposed as Dynamic Web APIs.

---

## Conventions

### Project Settings (Universal)
Every `.csproj` uses:
- `<TargetFramework>net10.0</TargetFramework>`
- `<ImplicitUsings>enable</ImplicitUsings>`
- `<Nullable>disable</Nullable>`
- `<RootNamespace>Com.Scm</RootNamespace>` (`Samples.*` use `Com.Scm.Samples`)

### Service Auto-Discovery
- Any class name **ending in `Service`** inside a configured assembly is auto-registered in DI as `Scoped` via [DllExtension.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Extensions/DllExtension.cs) (`Assembly.Load(name)` + `Name.EndsWith("Service")`).
- These same classes are automatically exposed as **Dynamic Web API controllers** via `services.AddDynamicWebApi()` (from `Scm.Server.Api`).
- **Do not write explicit Controllers** for standard CRUD; implement a `*Service` class instead. Explicit controllers exist only for non-Dynamic scenarios (e.g. `DbController`, `HbController`).

### Database
- **ORM**: SqlSugarCore (`SqlSugarScope` singleton registered in `ScmStartup.SqlSetup`).
- **Default DB**: SQLite (`Data Source=data/scm.db;`).
- **Design rule**: single-table operations preferred (max 2 tables). Avoid DB-specific SQL features.
- Enum properties map to `INTEGER` in SQLite, `TINYINT` in other engines.
- Schema init: `ScmDbHelper` + `SamplesDbHelper` run `InitDb()` on startup — entity properties drive column types per `DbType` (see the `EntityService` callback in [ScmStartup.cs](file:///d:/workspace/Git/Scm.Net/Scm.Net/Configure/Startup/ScmStartup.cs)).

### DTO Naming
DTO properties use **snake_case** (e.g., `user_name`, `create_time`), not camelCase. JSON serialization uses Newtonsoft.Json (`Microsoft.AspNetCore.Mvc.NewtonsoftJson`).

### Object Mapping
- Uses **Mapster** (not AutoMapper). Register profiles via `MapperRegister : IRegister` and `services.AddMapperProfile()`.
- DVO (`*Dvo`) = view objects returned to frontend; DTO (`*Dto`) = data-transfer contracts; DAO (`*Dao`) = SqlSugar entities mapped to DB tables.

---

## Authentication & Authorization

Three JWT schemes coexist (see [ScmToken.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Token/ScmToken.cs)):

| Scheme | Prefix | Use case |
|--------|--------|----------|
| `Bearer` | `Bearer ` | Standard web user JWT (with refresh via `X-Refresh-Token`) |
| `Operator` | `Operator ` | Operator/session login |
| `Terminal` | `Terminal ` | Bound-device token (Base64 `terminal_id:user_id:time:digest`) |

- [ScmAuthHandler.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Token/ScmAuthHandler.cs) is the abstract dispatcher: it splits the `Authorization` header by scheme and delegates to the matching handler.
- `JwtMiddleware` parses claims and populates `IJwtTokenHolder` **after** `UseAuthentication`/`UseAuthorization`.
- **Token holder uses `AsyncLocal<ScmToken>`** (not `ThreadLocal`) so the current user survives `await` thread switches in ASP.NET Core. Do not revert to `ThreadLocal` — async flows will lose the token.
- Set the token explicitly in background services (`IHostedService`, Quartz jobs) before touching `SugarRepository`, since there is no HTTP context.

---

## MQTT

Two cooperating projects:
- **`Scm.Mqtt`** — portable client/contracts (`BrokerConfig`, `ClientConfig`, `IMqttPublisher`, `IMqttSubscriber`). Safe to reference from any assembly (used by `WinForm` test client).
- **`Scm.Server.MQTT`** — host-side `MqttBrokerService` + `MqttClientService` + DI extensions.

Registration is centralized in `ScmStartup`:
```csharp
services.SetupMqtt(mqttBrokerConfig, mqttClientConfig);
```

Configuration (`appsettings.json`):
```json
"MqttBroker": { "Enabled": true, "Port": 1883, "RequireAuthentication": false },
"MqttClient": { "Enabled": true, "Host": "localhost", "Port": 1883 }
```

**Critical default**: `BrokerConfig.Enabled` defaults to `false`. The Broker only starts when the config section explicitly sets `"Enabled": true`. The client always registers regardless (so it can connect to an external broker).

**Start-order pitfall**: the Broker is an `IHostedService`; the Client (in `SamplesMqttHostedService`) may run before the Broker is listening. Always implement a retry/backoff loop in client-side hosted services.

---

## Quartz (Scheduled Jobs)

- Config: `appsettings.json → Quartz` (`BaseDir`, `JobFile`, `LogsDir`, `DataDir`).
- Two storage modes selected by `QuartzConfig.Type`:
  - `"file"` (default) → `DfQuartzJobService` + `QuartzFileHelper` (jobs defined in `quartz/settings/jobs.json`).
  - anything else → `DbQuartzJobService` (jobs persisted in DB tables).
- **Custom job contract**: implement `ICustomJob` (`string ExecuteService(string parameter)`). All `ICustomJob` implementations across loaded DLLs are auto-registered as Scoped via `AddQuartzClassJobs()` (reflection scan of `*.dll` in `BaseDirectory`).
- Built-in jobs: `ApiClientJob` (HTTP callback), `DllMethodJob` (invoke method by reflection), `VersionCheckJob` (daily version check, see Upgrade section).
- `UseQuartz()` initializes the scheduler on app startup.

---

## Upgrade / Version Check

- `Env.CheckUpgradeUrl` in `appsettings.json` is the version-feed URL returning a `ScmVerInfo` (version + download URL).
- `VersionCheckJob` (Quartz) polls the URL daily; when a newer version is found it writes a row to `scm_sys_upgrade_info` (`ScmUpgradeInfoDao`).
- [UpgradeController](file:///d:/workspace/Git/Scm.Net/Scm.Net/Controllers/UpgradeController.cs) exposes `CheckUpdateAsync` / `HasNewVersionAsync` / `GetVersionHistoryAsync` / `DownloadUpdate` for the frontend prompt + upgrade flow.
- The upgrade launcher is `Upgrade.Net.dll` (in `Libs/net10.0/`); path configurable via `Env.UpgradeFilePath` / `Env.UpgradeJsonPath`.
- **Do not call `Environment.Exit(0)`** to stop the host during upgrade — use `IHostApplicationLifetime.StopApplication()` so hosted services and DB connections shut down gracefully.

---

## AI / RAG Integration

- Config: `appsettings.json → Ai` (default providers: `deepseek`, `qwen`).
- `Scm.Ai` holds request/response DVOs (`AiChatRequest`, `AiChatResponse`, `AiEmbeddingResult`, etc.).
- `Scm.Core/Ai` implements `ScmAiDocService` (knowledge base CRUD) and `AiRagCore` (chunking + retrieval).
- RAG settings under `Ai.Rag`: `DocDir`, `ChunkSize`, `ChunkOverlap`, `TopK`, `MinScore`, `EmbedBatch`.
- **API keys are secrets** — leave `ApiKey` empty in `appsettings.json` and inject via env var `SCM__AI__PROVIDERS__<index>__APIKEY` (double-underscore path, index is the array position).
- See [docs/AiIntegration.md](file:///d:/workspace/Git/Scm.Net/docs/AiIntegration.md) for the full flow.

---

## Configuration & Secrets

### Files
- `Scm.Net/appsettings.json` — committed defaults (Kestrel `:9999`, SQLite, Redis cache, MQTT on, Quartz file mode).
- `Scm.Net/appsettings.Development.json` — dev overrides (Kestrel `:5000`, Scalar on).
- `Scm.Net/Properties/launchSettings.json` — launch profiles.
- `env.example` — sample environment variables (committed).

### Sensitive Values (Never Commit)
The following are **gitignored**:
- `appsettings.local.json`, `appsettings.development.json`, `appsettings.production.json`
- `.env`, `.env.local`, `**/secrets.json`
- `data/`, `upload/`, `images/`, `logs/`, `generator/`

Use **User Secrets** in development:
```bash
cd Scm.Net
dotnet user-secrets init --project Scm.Net
dotnet user-secrets set "Sql:Text" "..." --project Scm.Net
```

Use **environment variables** in production:
```bash
export SCM__SQL__TEXT="..."
export SCM__JWT__SECURITY="..."
export SCM__AI__PROVIDERS__0__APIKEY="..."
```
(Prefix `SCM__`, double-underscore separates sections; array indices are numeric.)

See [docs/SecureConfiguration.md](file:///d:/workspace/Git/Scm.Net/docs/SecureConfiguration.md) for the full secrets strategy.

---

## Precompiled Libraries

`Libs/` contains closed-source DLLs referenced directly by HintPath:
- `Libs/net10.0/` — `Scm.Cache.dll`, `Scm.Uid.dll`, `Scm.Plugin.Image.dll`, `Scm.Aiml.dll`, `Scm.Workflow.dll`, `Upgrade.Net.dll`, plus cache providers (`Scm.Cache.Redis/Memory/Garnet.dll`).
- `Libs/netstandard2.0/` — `Scm.Common.dll`, `Scm.Common.File.dll`, `Scm.Common.Http.dll`, `Scm.Common.Otp.dll`, `Scm.Common.Text.dll`, `Scm.Common.Time.dll`.

Do not delete or expect source code for these. When adding a new `Libs/` reference, also add `<Reference Include="..."><HintPath>...</HintPath></Reference>` to the consuming `.csproj`.

---

## Dynamic API / Plugin Loading

- [DllExtension.cs](file:///d:/workspace/Git/Scm.Net/Scm.Server/Extensions/DllExtension.cs) loads assemblies listed in `DllConfig` (from `appsettings.json → Project:Service`) and auto-registers `*Service` classes as Scoped.
- Default configured service assemblies: `["Scm.Core"]`.
- When adding a new plugin/module DLL, add its assembly name to `Project:Service` in `appsettings.json`. The assembly must be present in the host's `BaseDirectory` at runtime.
- `AddDynamicWebApi()` exposes those services as REST endpoints. API doc grouping is driven by `[ApiExplorerSettings(GroupName = "...")]` on each service — the group code must match an entry in `Scalar.ApiDocs` (e.g. `scm`, `samples`, `test`).

---

## Caching

- Abstraction: `Com.Scm.Cache.ICacheService` (in `Scm.Cache.dll`).
- Provider selected by `Cache.Type` in `appsettings.json`:
  - `"Redis"` (default) — `Scm.Cache.Redis.dll`; `Cache.Text` is the StackExchange Redis connection string.
  - `"Memory"` — `Scm.Cache.Memory.dll`.
  - `"Garnet"` — `Scm.Cache.Garnet.dll`.
- Wired via `services.CacheSetup(envConfig)` in `ScmStartup`. Used by `ApiService` for `SaveSearch`/`ReadSearch` query-condition caching.

---

## SignalR

- Hub endpoint: `app.MapHub<ScmHub>("/scmhub")` (defined in `Scm.Server.SignalR`).
- `JwtMiddleware` ensures authenticated connections carry `ScmToken`; background push should call into `SignalRUtil` from `Scm.Core/Msg`.

---

## Logging

- **Serilog** configured from `appsettings.json → Serilog`.
- Enrichers: `FromLogContext`, `WithMachineName`, `WithThreadId` (use `{ThreadId}` and `{SourceContext}` in `OutputTemplate` for thread/class attribution).
- Sinks: `File` (rolling daily under `Logs/`) and `Console`.
- API: `LogUtils.Info/Debug/Error/Warning` (from `Scm.Common.Log`). Always use `LogUtils` rather than `Console.WriteLine` in production code paths.
- SQL logging: `SqlSugarScope` AOP `OnLogExecuting` logs every SQL with parameter substitution at Debug level (category `"db"`).

---

## Data Directories

Runtime-created directories (all gitignored). Resolved relative to `Env.dataDir` (default `data/`):

| Path | Env key | Contents |
|------|---------|----------|
| `data/` | `dataDir` | SQLite DBs, SQL scripts, fonts, uid.db |
| `data/upload/` | `upload` | Uploaded files |
| `data/images/` | `images` | Image assets |
| `logs/` | `logs` | Serilog rolling logs |
| `generator/` | `Generator.GeneratorDir` | Codegen output |
| `quartz/settings/` | `Quartz.BaseDir` | Quartz job definitions |
| `data/ai/docs/` | `Ai.Rag.DocDir` | RAG knowledge base |

---

## Common Pitfalls

1. **Do not add explicit API Controllers** for domain CRUD. Use `*Service` classes in `Scm.Core` and let the Dynamic Web API system expose them.
2. **Nullable is disabled globally.** Do not assume nullable reference types are enforced.
3. **Test project is not a test suite.** It is a scratch console app. There is no `dotnet test` runner configured.
4. **Frontend is separate.** Do not look for Vue/Vite files here; they live in `Scm.Vue` (another repo).
5. **Rebuild after adding a project reference.** The solution has 33 projects; stale build artifacts can cause missing-assembly errors at runtime.
6. **MQTT Broker default is `Enabled = false`** in `BrokerConfig`. The Broker only starts when the config section explicitly enables it. The Client registers unconditionally — pointing it at an external broker is fine.
7. **MQTT start order**: Client hosted services may run before the Broker is listening. Implement retry/backoff in the client's `StartAsync`/`ExecuteAsync`.
8. **Use `AsyncLocal`, not `ThreadLocal`, for per-request context.** `ThreadLocal<ScmToken>` loses state across `await` thread switches in ASP.NET Core.
9. **`[FromForm]` is required for `multipart/form-data` uploads.** Without it, `path`/`file` fields bind to `null`. For mixed URL+form binding, read fallbacks from `HttpContext.Request.Query[...]`.
10. **`Enabled` defaults are traps.** Many config classes (e.g. `BrokerConfig`) ship with non-zero defaults; always set explicit values in `appsettings.json` rather than relying on the C# default.
11. **Background services have no `HttpContext`.** Quartz jobs and `IHostedService` implementations must call `IJwtTokenHolder.SetToken(...)` themselves before using `SugarRepository`, otherwise data-permission filters operate on an empty token.
12. **Do not call `Environment.Exit()` from request handlers.** Use `IHostApplicationLifetime.StopApplication()` for graceful shutdown during upgrade flows.

---

## Further Reading

- [docs/AiIntegration.md](file:///d:/workspace/Git/Scm.Net/docs/AiIntegration.md) — AI provider + RAG pipeline details.
- [docs/SecureConfiguration.md](file:///d:/workspace/Git/Scm.Net/docs/SecureConfiguration.md) — User Secrets / env-var strategy.
- [docs/UnifiedApiResponse.md](file:///d:/workspace/Git/Scm.Net/docs/UnifiedApiResponse.md) — Response envelope conventions.
