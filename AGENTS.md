# AGENTS.md — Scm.Net

Compact guide for AI agents working in this .NET 10.0 enterprise backend framework (Chinese: 企业级中后台快速开发框架). Frontend lives in a separate repo (`Scm.Vue`).

---

## Run / Build

| Command | Context |
|---------|---------|
| `cd Scm.Net && dotnet run` | Start dev server on `https://localhost:5000` (Swagger enabled) |
| `dotnet build Scm.Net.sln` | Build entire 32-project solution |
| `cd Scm.Net && dotnet publish -c Release -o ./Publish` | Production publish |
| `cd Test && dotnet run` | Run the test console app (not a unit-test runner) |

- **No unit-test framework** is configured. `Test/Test.csproj` is a plain console executable (`<OutputType>Exe</OutputType>`), not xUnit/NUnit/MSTest.
- **No CI/CD**, Docker, `global.json`, `nuget.config`, or lint/format config exists.

---

## Architecture

### Entry Point
`Scm.Net/Program.cs` bootstraps via `WebApplicationBuilder` → `ScmStartup.ConfigureServices()` → `ScmStartup.ConfigureMiddleware()` in `Scm.Net/Configure/Startup/ScmStartup.cs`.

### Layered Topology
Solution folders mirror runtime layers:

| Folder | Projects | Role |
|--------|----------|------|
| `Scm.App` | `Scm.Net`, `Scm.Core`, `Scm.Dao`, `Scm.Dto` | Web host, business logic, data access, DTOs |
| `Scm.Common` | `Scm.Common`, `Scm.Common.Dto`, `Scm.Common.Log`, `Scm.Common.Os`, `Scm.Common.Excel` | Shared utilities |
| `Scm.Module` | `Scm.Dsa.Dba.Sugar`, `Scm.Dsa.Dfa.Json`, `Scm.Generator`, `Scm.Email`, `Scm.Phone` | ORM wrapper, JSON helpers, codegen, messaging |
| `Scm.Server` | `Scm.Server`, `Scm.Server.*` | Server abstractions and infra modules (API, Bearer, Cache, DAO, DVO, MQTT, Quartz, RabbitMQ, Service, SignalR, Swagger, Aiml) |
| `Samples` | `Samples.Common`, `Samples.Server`, etc. | Example extension projects |
| `Test` | `Test` | Console scratchpad |

Dependency flow (simplified):
```
Scm.Net (host)
  → Scm.Core (business services)
    → Scm.Server (interfaces, base services)
      → Scm.Dao / Scm.Dto / Scm.Server.Dao / Scm.Server.Dvo
```

---

## Conventions

### Project Settings (Universal)
Every `.csproj` uses:
- `<TargetFramework>net10.0</TargetFramework>`
- `<ImplicitUsings>enable</ImplicitUsings>`
- `<Nullable>disable</Nullable>`
- `<RootNamespace>Com.Scm</RootNamespace>` (`Samples.*` use `Com.Scm.Samples`)

### Service Auto-Discovery
- Any class name **ending in `Service`** inside a configured assembly is auto-registered in DI as `Scoped`.
- These same classes are automatically exposed as **Dynamic Web API controllers** via `Scm.Server.Api`.
- **Do not write explicit Controllers** for standard CRUD; implement a `*Service` class instead.

### Database
- **ORM**: SqlSugarCore (`SqlSugarScope` singleton registered in `ScmStartup.SqlSetup`).
- **Default DB**: SQLite (`Data Source=data/scm.db;`).
- **Design rule**: single-table operations preferred (max 2 tables). Avoid DB-specific SQL features.
- Enum properties map to `INTEGER` in SQLite, `TINYINT` in other engines.

### DTO Naming
DTO properties use **snake_case** (e.g., `user_name`, `create_time`), not camelCase.

---

## Configuration & Secrets

### Files
- `Scm.Net/appsettings.json` — committed defaults (Kestrel on `:9999`, SQLite, Redis cache).
- `Scm.Net/appsettings.Development.json` — dev overrides (Kestrel on `:5000`, Swagger on).
- `Scm.Net/Properties/launchSettings.json` — launch profiles.

### Sensitive Values (Never Commit)
The following are **gitignored**:
- `appsettings.local.json`, `appsettings.development.json`, `appsettings.production.json`
- `.env`, `.env.local`, `**/secrets.json`

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
```
(Prefix `SCM__`, double-underscore separates sections.)

---

## Precompiled Libraries

`Libs/` contains closed-source DLLs referenced directly by HintPath:
- `Libs/net10.0/` — `Scm.Cache.dll`, `Scm.Uid.dll`, `Scm.Plugin.Image.dll`, etc.
- `Libs/netstandard2.0/` — `Scm.Common.File.dll`, `Scm.Common.Http.dll`, etc.

Do not delete or expect source code for these.

---

## Dynamic API / Plugin Loading

- `Scm.Server/Extensions/DllExtension.cs` loads assemblies listed in `DllConfig` (from `appsettings.json` → `Project:Service`) and auto-registers `*Service` classes.
- Default configured service assemblies: `["Scm.Core"]`.
- When adding a new plugin/module DLL, add its assembly name to `Project:Service` in `appsettings.json`.

---

## Data Directories

Runtime-created directories (all gitignored):
- `data/` — SQLite DBs, SQL migration scripts, fonts
- `upload/` — uploaded files
- `images/` — image assets
- `logs/` — Serilog rolling logs
- `generator/` — codegen output
- `quartz/settings/` — Quartz job definitions

---

## Common Pitfalls

1. **Do not add explicit API Controllers** for domain CRUD. Use `*Service` classes in `Scm.Core` and let the Dynamic Web API system expose them.
2. **Nullable is disabled globally.** Do not assume nullable reference types are enforced.
3. **Test project is not a test suite.** It is a scratch console app. There is no `dotnet test` runner configured.
4. **Frontend is separate.** Do not look for Vue/Vite files here; they live in `Scm.Vue` (another repo).
5. **Rebuild after adding a project reference.** The solution has 32 projects; stale build artifacts can cause missing-assembly errors at runtime.
