# 配置管理安全化文档

## 概述

本文档介绍 Scm.Net 框架中如何安全地管理敏感配置（如数据库密码、API密钥等），支持以下方式：

1. **User Secrets** - 开发环境使用
2. **环境变量** - 测试/生产环境使用
3. **Azure Key Vault / AWS Secrets Manager** - 企业级密钥管理（可选扩展）

## 配置优先级

配置按以下优先级从高到低读取：

| 优先级 | 来源 | 适用环境 |
|--------|------|----------|
| 1 | 环境变量 | 所有环境 |
| 2 | User Secrets | 开发环境 |
| 3 | appsettings.{Environment}.json | 特定环境 |
| 4 | appsettings.json | 默认配置 |

## 环境变量格式

环境变量使用以下命名约定：

```
SCM__SECTION__KEY=value
```

**示例：**
```bash
# 数据库连接字符串
SCM__SQL__TEXT=Server=localhost;Database=ScmDb;User Id=sa;Password=xxx

# JWT 密钥
SCM__JWT__SECURITY=your_256_bit_secret_key

# 邮件密码
SCM__EMAIL__PASSWORD=your_email_password
```

### 支持的环境变量列表

| 环境变量 | 对应配置 | 说明 |
|----------|----------|------|
| `SCM__SQL__TYPE` | Sql:Type | 数据库类型 |
| `SCM__SQL__TEXT` | Sql:Text | 数据库连接字符串 |
| `SCM__CACHE__TYPE` | Cache:Type | 缓存类型 |
| `SCM__CACHE__TEXT` | Cache:Text | Redis连接字符串 |
| `SCM__JWT__SECURITY` | Jwt:Security | JWT密钥 |
| `SCM__JWT__ISSUER` | Jwt:Issuer | JWT签发者 |
| `SCM__JWT__AUDIENCE` | Jwt:Audience | JWT受众 |
| `SCM__JWT__EXPIRES` | Jwt:Expires | 过期时间(分钟) |
| `SCM__EMAIL__SMTPSERVER` | Email:SmtpServer | SMTP服务器 |
| `SCM__EMAIL__PORT` | Email:Port | SMTP端口 |
| `SCM__EMAIL__USERNAME` | Email:Username | 邮箱账号 |
| `SCM__EMAIL__PASSWORD` | Email:Password | 邮箱密码 |
| `SCM__OIDC__APP_KEY` | Oidc:app_key | OIDC客户端ID |
| `SCM__OIDC__APP_SECRET` | Oidc:app_secret | OIDC客户端密钥 |
| `SCM__SECURITY__APP_KEY` | Security:AppKey | 应用密钥 |
| `SCM__SECURITY__AES_KEY` | Security:AesKey | AES密钥 |
| `SCM__SECURITY__SIGN_KEY` | Security:SignKey | 签名密钥 |

## 开发环境配置

### 使用 User Secrets

**步骤1：初始化 User Secrets**

```bash
cd Scm.Net
dotnet user-secrets init --project Scm.Net
```

**步骤2：设置敏感配置**

```bash
# 设置数据库连接字符串
dotnet user-secrets set "Sql:Text" "Server=localhost;Database=ScmDb;User Id=sa;Password=xxx" --project Scm.Net

# 设置 JWT 密钥
dotnet user-secrets set "Jwt:Security" "your_256_bit_secret_key" --project Scm.Net

# 设置邮箱密码
dotnet user-secrets set "Email:Password" "your_email_password" --project Scm.Net
```

### 使用 .env 文件（可选）

创建 `.env` 文件：

```bash
# 复制示例文件
cp env.example .env
```

编辑 `.env` 文件，填写敏感配置。

## 生产环境配置

### 方法1：使用环境变量

**Linux/macOS（bash）：**

```bash
# 设置环境变量
export SCM__SQL__TEXT="Server=localhost;Database=ScmDb;User Id=sa;Password=xxx"
export SCM__JWT__SECURITY="your_256_bit_secret_key"
export SCM__EMAIL__PASSWORD="your_email_password"

# 启动应用
dotnet Scm.Net.dll
```

**Windows（PowerShell）：**

```powershell
# 设置环境变量
$env:SCM__SQL__TEXT="Server=localhost;Database=ScmDb;User Id=sa;Password=xxx"
$env:SCM__JWT__SECURITY="your_256_bit_secret_key"
$env:SCM__EMAIL__PASSWORD="your_email_password"

# 启动应用
dotnet Scm.Net.dll
```

### 方法2：使用配置文件

创建 `appsettings.production.json` 文件（已被 .gitignore 排除）：

```json
{
  "Sql": {
    "Type": "SqlServer",
    "Text": "Server=localhost;Database=ScmDb;User Id=sa;Password=xxx"
  },
  "Jwt": {
    "Security": "your_256_bit_secret_key",
    "Issuer": "Scm.Net",
    "Audience": "scm.net"
  },
  "Email": {
    "SmtpServer": "smtphz.qiye.163.com",
    "Port": 465,
    "Username": "your_email@example.com",
    "Password": "your_email_password"
  }
}
```

## 使用 SecretConfigurationService

框架提供了 `SecretConfigurationService` 用于安全地读取敏感配置：

```csharp
using Com.Scm.Configure.Security;

public class MyService
{
    private readonly SecretConfigurationService _secretService;

    public MyService(SecretConfigurationService secretService)
    {
        _secretService = secretService;
    }

    public void DoSomething()
    {
        // 获取数据库配置
        var dbConfig = _secretService.GetDatabaseConfig();
        string connectionString = dbConfig.ConnectionString;

        // 获取 JWT 配置
        var jwtConfig = _secretService.GetJwtConfig();
        string securityKey = jwtConfig.SecurityKey;

        // 获取邮箱配置
        var emailConfig = _secretService.GetEmailConfig();
        string password = emailConfig.Password;

        // 获取单个敏感值
        string apiKey = _secretService.GetSecret("Api:Key");
    }
}
```

## 配置安全最佳实践

### 1. 永远不要提交敏感配置到版本控制

确保 `.gitignore` 包含以下文件：
- `.env`
- `.env.local`
- `appsettings.local.json`
- `appsettings.development.json`
- `appsettings.production.json`
- `**/secrets.json`

### 2. 使用强密钥

- **JWT 密钥**：至少 256 位（32 字节）的随机字符串
- **AES 密钥**：128、192 或 256 位
- **数据库密码**：至少 16 位，包含大小写字母、数字和特殊字符

### 3. 定期轮换密钥

- 生产环境中定期更换数据库密码、API密钥等
- 使用密钥管理服务自动轮换密钥

### 4. 限制权限

- 数据库用户只授予必要的权限
- 不要使用 `sa` 或 `root` 用户连接数据库
- 最小化应用服务账户的权限

### 5. 使用 HTTPS

- 生产环境强制使用 HTTPS
- 使用 Let's Encrypt 免费证书

## 扩展：集成云端密钥管理

### Azure Key Vault

**安装 NuGet 包：**

```bash
dotnet add package Azure.Security.KeyVault.Secrets
dotnet add package Azure.Identity
```

**配置：**

```csharp
// Program.cs
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// 添加 Azure Key Vault
var keyVaultUri = new Uri($"https://{builder.Configuration["KeyVault:Name"]}.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(
    keyVaultUri,
    new DefaultAzureCredential());
```

### AWS Secrets Manager

**安装 NuGet 包：**

```bash
dotnet add package AWSSDK.SecretsManager
```

**配置：**

```csharp
// Program.cs
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;

var builder = WebApplication.CreateBuilder(args);

// 添加 AWS Secrets Manager
builder.Services.AddAWSService<IAmazonSecretsManager>();
builder.Services.AddSingleton<ISecretsManagerCache, SecretsManagerCache>();
```

## 验证配置

启动应用后，可以通过以下方式验证配置是否正确加载：

1. **查看日志**：检查应用启动日志中是否包含配置加载信息
2. **API 测试**：调用 API 验证功能是否正常
3. **环境变量检查**：使用以下命令验证环境变量

```bash
# Linux/macOS
echo $SCM__SQL__TEXT

# Windows PowerShell
echo $env:SCM__SQL__TEXT
```

## 故障排除

### 配置未生效

1. 检查环境变量名称是否正确（大小写敏感）
2. 确认配置文件路径正确
3. 检查配置优先级（环境变量 > User Secrets > 配置文件）

### User Secrets 不生效

1. 确认已在正确的项目目录执行 `dotnet user-secrets init`
2. 检查 `secrets.json` 文件是否存在于 `%APPDATA%\Microsoft\UserSecrets\<GUID>\` 目录
3. 确认 `UserSecretsId` 在 `.csproj` 文件中正确配置

### 数据库连接失败

1. 验证连接字符串格式正确
2. 确认数据库服务器可访问
3. 检查数据库用户凭据是否正确
4. 验证数据库用户权限

## 总结

通过使用 `SecretConfigurationService` 和环境变量/User Secrets，您可以安全地管理敏感配置，避免将敏感信息提交到版本控制系统。建议：

- 开发环境使用 **User Secrets**
- 测试环境使用 **环境变量**
- 生产环境使用 **环境变量** 或 **云端密钥管理服务**
