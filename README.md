<p align="center">
  <img src="http://www.c-scm.net/logo.png" alt="Scm.Net Logo" width="120" />
</p>

<h1 align="center">Scm.Net</h1>

<p align="center">
  <a href="https://gitee.com/leadiot/scm.net">
    <img src="https://gitee.com/leadiot/scm.net/badge/star.svg?theme=dark" alt="Gitee Stars" />
  </a>
  <a href="https://dotnet.microsoft.com">
    <img src="https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet" alt=".NET 10" />
  </a>
  <a href="https://vuejs.org/">
    <img src="https://img.shields.io/badge/Vue-3.0-4FC08D?logo=vue.js" alt="Vue 3" />
  </a>
  <a href="https://element-plus.org/">
    <img src="https://img.shields.io/badge/Element%20Plus-2.13-409EFF?logo=element" alt="Element Plus" />
  </a>
  <img src="https://img.shields.io/badge/license-MIT-green" alt="MIT License" />
  <img src="https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey" alt="Cross Platform" />
</p>

<p align="center">
  <b>企业级中后台快速开发框架</b> — 基于 .NET 10.0 + Vue 3.0，前后端分离架构。
</p>

<p align="center">
  前端项目：<a href="https://gitee.com/leadiot/scm.vue">Scm.Vue</a> ｜
  <a href="http://www.c-scm.net">在线演示</a> ｜
  <a href="https://gitee.com/leadiot/scm.net/wikis/%E9%A1%B9%E7%9B%AE%E4%BB%8B%E7%BB%8D">在线文档</a>
</p>

***

## 📖 项目简介

**Scm.Net** 是一套企业级中后台管理系统的快速开发框架，采用前后端分离架构，内置权限管理、代码生成、工作流引擎、实时消息、数据可视化等企业级核心能力。

笔者多年从事供应链系统及企业信息化系统的产品与研发，经常面对异构应用场景需求。在梳理多种项目经验的过程中开发此项目，旨在帮助开发同仁快速搭建一个完整、可扩展的开发框架。

基于本项目已开发的产品包括：

| 产品      | 说明      |
| ------- | ------- |
| **OMS** | 订单管理系统  |
| **WMS** | 仓储管理系统  |
| **TMS** | 运输管理系统  |
| **DMS** | 配送管理系统  |
| **BMS** | 计费管理系统  |
| **YMS** | 园区管理系统  |
| **EAM** | 资产管理系统  |
| **IOT** | 物联网管理系统 |

> 项目仍在不断完善中，欢迎有兴趣的同仁一起交流贡献。

***

## ✨ 核心特性

### 认证与安全

- **多种登录方式** — 账号、手机号、邮箱、OAuth、OIDC、SAML 等第三方联合登录
- **生物识别** — 面部识别、指纹识别、声纹识别接口预留
- **双因素认证** — 基于 TOTP 的 OTP 动态口令
- **数据加密** — 前后端传输参数加密签名（AES/DES）
- **JWT 鉴权** — Bearer Token 认证，自动刷新
- **权限控制** — 公司/部门/岗位/分组/用户/角色六级权限体系

### 系统与框架

- **多数据库支持** — SQLite、MySQL、MariaDB、PostgreSQL、SQL Server、Oracle、Firebird、MongoDB
- **多缓存机制** — MemoryCache、Dictionary、Redis
- **动态 API** — 自动注册服务为 Web API，无需手动编写 Controller
- **代码生成器** — 自动生成实体、DAO、DTO/VO，支持自定义模板
- **工作流引擎** — 可视化流程设计、节点配置、表单绑定、在线审批
- **定时任务** — Quartz.NET 集成，支持动态任务管理
- **实时通信** — SignalR WebSocket 实时推送与在线聊天

### 业务能力

- **MQTT 通信** — 轻量级物联网通信协议，支持内置 Broker
- **RabbitMQ 消息队列** — 发布者/消费者模式集成
- **AI 大语言模型** — 集成 DeepSeek、华为盘古、通义千问、腾讯元宝、百度文心一言、豆包、ChatGPT
- **图像处理** — 条码生成识别、图片水印、图形验证码、头像裁剪
- **数据可视化** — ECharts 图表集成，Dashboard 看板布局
- **文件管理** — 文件上传、导入导出、在线预览
- **ID 生成器** — 雪花 ID、序列 ID、格式 ID 等多种生成方式
- **插件扩展** — Plugin/Addon 动态加载机制

### 平台兼容

- **跨平台运行** — 支持 Windows、macOS、Linux、HarmonyOS
- **响应式布局** — 支持电脑、平板、手机多种终端
- **多租户架构** — 可扩展为多租户、多组织架构应用

***

## 🛠 技术栈

### 后端核心依赖

| 技术                                                               | 版本      | 说明                        |
| ---------------------------------------------------------------- | ------- | ------------------------- |
| [.NET](https://dotnet.microsoft.com)                             | 10.0    | 跨平台运行时，兼容 .NET 6/7/8/9/10 |
| [SqlSugarCore](https://www.donet5.com/Home/Doc)                  | -       | ORM 数据访问框架                |
| [ImageSharp](https://github.com/SixLabors/ImageSharp)            | ^3.1.12 | 跨平台图像处理                   |
| [MQTTnet](https://github.com/dotnet/MQTTnet)                     | -       | MQTT 通讯（客户端 + 内置 Broker）  |
| [RabbitMQ.Client](https://www.rabbitmq.com)                      | -       | RabbitMQ 消息队列             |
| [Quartz.NET](https://www.quartz-scheduler.net)                   | -       | 定时任务调度                    |
| [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr) | -       | 实时 Web 通信                 |
| [Mapster](https://mapster.dev/)                                  | 10.0.7  | 对象映射                      |
| [Serilog](https://serilog.net/)                                  | 4.3.1   | 结构化日志                     |
| [Newtonsoft.Json](https://www.newtonsoft.com/json)               | -       | JSON 序列化                  |
| [JWT Bearer](https://github.com/aspnet/AspNetCore)               | 10.0.8  | 认证授权                      |

### 前端（Scm.Vue）

| 技术                                        | 版本      | 说明                |
| ----------------------------------------- | ------- | ----------------- |
| [Vue](https://vuejs.org/)                 | ^3.5.32 | 渐进式 JavaScript 框架 |
| [Vite](https://vitejs.dev/)               | ^8.0.3  | 下一代前端构建工具         |
| [Element Plus](https://element-plus.org/) | ^2.13.6 | Vue 3 桌面端组件库      |
| [Pinia](https://pinia.vuejs.org/)         | ^3.0.0  | 状态管理              |
| [ECharts](https://echarts.apache.org/)    | ^6.0.0  | 数据可视化             |
| [Axios](https://axios-http.com/)          | ^1.7.0  | HTTP 客户端          |

***

## 🔧 环境要求

| 工具            | 版本要求     | 下载地址                                 |
| ------------- | -------- | ------------------------------------ |
| .NET SDK      | ≥ 10.0   | <https://dotnet.microsoft.com>       |
| Visual Studio | ≥ 2026   | <https://visualstudio.microsoft.com> |
| Node.js       | ≥ 18.0.0 | <https://nodejs.org>                 |

***

## 🚀 快速开始

### 1. 获取代码

```bash
git clone https://gitee.com/leadiot/scm.net.git
```

### 2. 配置数据库

编辑 `Scm.Net/appsettings.json`，修改数据库连接串：

```json
{
  "Sql": {
    "Type": "Sqlite",
    "Text": "Data Source=data/scm.db;"
  }
}
```

> 系统启动时默认自动初始化数据库。

### 3. 启动后端服务

```bash
cd Scm.Net
dotnet run
```

### 4. 启动前端（需先克隆 Scm.Vue）

```bash
git clone https://gitee.com/leadiot/scm.vue.git
cd Scm.Vue
npm install
npm run dev
```

访问 `http://localhost:5000/swagger` 确认后端接口正常，访问 `http://localhost:2800` 进入前端页面。

> 详细说明请参考：[环境搭建教程](https://gitee.com/leadiot/scm.net/wikis/%E7%8E%AF%E5%A2%83%E6%90%AD%E5%BB%BA%E6%95%99%E7%A8%8B) | [数据库配置说明](https://gitee.com/leadiot/scm.net/wikis/%E6%95%B0%E6%8D%AE%E5%BA%93%E9%85%8D%E7%BD%AE)

***

## 🌐 配置文件

### appsettings.json 核心配置

| 配置节点        | 说明                                                        |
| ----------- | --------------------------------------------------------- |
| `Sql`       | 数据库连接（Type 支持 Sqlite、MySQL、PostgreSQL、SqlServer、Oracle 等） |
| `Cache`     | 缓存配置（Type 支持 MemoryCache、Dictionary、Redis）                |
| `Uid`       | ID 生成器配置                                                  |
| `Jwt`       | JWT 认证（Security Key、Issuer、Audience、Expires）              |
| `Kestrel`   | HTTP 监听端点（默认 9999 端口）                                     |
| `Cors`      | 跨域访问配置                                                    |
| `Quartz`    | 定时任务调度配置                                                  |
| `Email`     | 邮件服务（SMTP）                                                |
| `Oidc`      | 第三方联合登录配置                                                 |
| `Otp`       | 动态口令（TOTP）配置                                              |
| `Generator` | 代码生成器配置                                                   |
| `Serilog`   | 结构化日志配置                                                   |

***

## 📁 项目结构

| 项目                    | 说明                                     |
| --------------------- | -------------------------------------- |
| `Scm.Net`             | Web API 主入口（Program.cs、Controllers）    |
| `Scm.Core`            | 核心业务逻辑层                                |
| `Scm.Dao`             | 数据访问层（DAO）                             |
| `Scm.Dto`             | 数据传输对象层（DTO）                           |
| `Scm.Common`          | 公共枚举、工具类                               |
| `Scm.Common.Dto`      | 公共 DTO 定义                              |
| `Scm.Common.Excel`    | Excel 导入导出                             |
| `Scm.Common.Log`      | 日志记录                                   |
| `Scm.Common.Os`       | 操作系统相关工具                               |
| `Scm.Dsa.Dba.Sugar`   | SqlSugar 仓储基类封装                        |
| `Scm.Dsa.Dfa.Json`    | JSON 数据格式转换                            |
| `Scm.Server`          | 服务端核心（接口定义 + 基础服务）                     |
| `Scm.Server.Api`      | 动态 API 注册                              |
| `Scm.Server.Bearer`   | JWT Bearer 认证扩展                        |
| `Scm.Server.Cache`    | 缓存扩展（MemoryCache / Dictionary / Redis） |
| `Scm.Server.Dao`      | 服务端 DAO 扩展                             |
| `Scm.Server.Dvo`      | 服务端 DTO/VO 映射                          |
| `Scm.Server.MQTT`     | MQTT 通讯（客户端 + 内置 Broker）               |
| `Scm.Server.RabbitMQ` | RabbitMQ 消息队列集成                        |
| `Scm.Server.SignalR`  | SignalR 实时通讯                           |
| `Scm.Server.Quartz`   | Quartz 定时任务调度                          |
| `Scm.Server.Swagger`  | Swagger 文档扩展                           |
| `Scm.Server.Aiml`     | AI 大语言模型集成                             |
| `Scm.Server.Service`  | 业务服务扩展注册                               |
| `Scm.Email`           | 邮件发送服务                                 |
| `Scm.Phone`           | 短信发送服务                                 |
| `Scm.Generator`       | 代码生成器（支持自定义模板）                         |
| `Samples.*`           | 使用示例工程                                 |

### 目录结构

```
Scm.Net/
├── Scm.Net/                     # Web API 主项目
│   ├── Controllers/             #   - API 控制器
│   ├── Resources/               #   - 资源文件（logo、字体）
│   ├── data/                    #   - 数据文件（SQLite 数据库、上传文件）
│   ├── Program.cs               #   - 应用入口与启动配置
│   └── appsettings.json         #   - 应用配置
├── Scm.Core/                    # 核心业务逻辑
├── Scm.Dao/                     # 数据访问层
├── Scm.Dto/                     # 数据传输对象
├── Scm.Common/                  # 公共工具类
├── Scm.Common.Dto/              # 公共 DTO
├── Scm.Common.Excel/            # Excel 处理
├── Scm.Common.Log/              # 日志记录
├── Scm.Common.Os/               # 操作系统工具
├── Scm.Dsa.Dba.Sugar/           # SqlSugar 仓储基类
├── Scm.Dsa.Dfa.Json/            # JSON 格式处理
├── Scm.Server/                  # 服务端核心
├── Scm.Server.*/                # 服务端扩展模块
├── Scm.Email/                   # 邮件服务
├── Scm.Phone/                   # 短信服务
├── Scm.Generator/               # 代码生成器
├── Samples.*/                   # 示例项目
├── Libs/                        # 预编译库
├── Test/                        # 测试项目
├── Scm.Net.sln                  # 解决方案文件
└── LICENSE                      # MIT 许可协议
```

***

## 📄 API 接口

| 控制器                   | 功能      |
| --------------------- | ------- |
| `CaptchaController`   | 图形验证码生成 |
| `UploadController`    | 文件上传    |
| `DownloadController`  | 文件下载    |
| `GeneratorController` | 代码生成    |
| `QuartzController`    | 定时任务管理  |
| `HbController`        | 心跳检测    |
| `OnLineController`    | 在线用户管理  |

> 启动后访问 `http://localhost:5000/swagger` 查看完整 API 文档。

***

## 📐 设计原则

1. **数据库仅用于存储数据** — 除 CRUD 外不使用任何依赖特定数据库的特性，可平滑迁移到任何支持标准 SQL 的数据引擎
2. **单表操作为主** — 原则上仅允许单表操作（最多不超过两张表），可通过数据冗余设计提升查询效率
3. **JSON 数据交互** — 多端数据交互基于 JSON 格式，保证低噪音的同时提升可扩展性
4. **蛇形命名 DTO** — DTO 统一使用 snake\_case 命名，适配多场景异构应用需求

***

## 🎨 界面预览（前端 Scm.Vue）

### 工作台模式

| 功能    | 截图                                       |
| ----- | ---------------------------------------- |
| 控制台首页 | ![后台首页](screenshots/web-02-home.png)     |
| 用户管理  | ![用户管理](screenshots/web-03-user.png)     |
| 文件管理  | ![文件管理](screenshots/web-04-file.png)     |
| 日程管理  | ![日程管理](screenshots/web-05-calendar.png) |
| 电子邮件  | ![电子邮件](screenshots/web-06-email.png)    |
| 系统监控  | ![系统监控](screenshots/web-07-monitor.png)  |
| 在线文档  | ![在线文档](screenshots/web-08-docs.png)     |

### 云桌面模式

| 功能    | 截图                                              |
| ----- | ----------------------------------------------- |
| 桌面首页  | ![用户首页](screenshots/wos-02-desktop-default.png) |
| 云文件管理 | ![云文件管理](screenshots/wos-09-cloud.png)          |
| 记事本   | ![记事](screenshots/wos-10-notepad.png)           |
| 图片查看  | ![图像](screenshots/wos-11-image.png)             |
| 音频播放  | ![音频](screenshots/wos-12-audio.png)             |
| 待办事项  | ![待办](screenshots/wos-13-gtd.png)               |
| 终端管理  | ![终端管理](screenshots/wos-14-terminal.png)        |

### 手机端

![用户登录](screenshots/mp-login.jpg) | ![用户首页](screenshots/mp-home.jpg) | ![系统菜单](screenshots/mp-menu.jpg)

> 更多截图请访问 [项目文档](https://gitee.com/leadiot/scm.net/wikis)。

***

## 🧪 演示账号

| 角色  | 登录用户    | 登录口令     |
| --- | ------- | -------- |
| 管理员 | `admin` | `123456` |

> 演示地址：<http://www.c-scm.net>

***

## 🌍 浏览器支持

| 浏览器                                                                            | 最低版本          |
| ------------------------------------------------------------------------------ | ------------- |
| ![Chrome](https://img.shields.io/badge/Chrome->=88-success?logo=google-chrome) | Chrome >= 88  |
| ![Firefox](https://img.shields.io/badge/Firefox->=78-success?logo=firefox)     | Firefox >= 78 |
| ![Safari](https://img.shields.io/badge/Safari->=14-success?logo=safari)        | Safari >= 14  |
| ![Edge](https://img.shields.io/badge/Edge->=88-success?logo=microsoft-edge)    | Edge >= 88    |

桌面端：

| <br />      | **Chrome ≥88** | **Firefox ≥78** | **Edge ≥88** | **Safari ≥14** |
| ----------- | :------------: | :-------------: | :----------: | :------------: |
| **Windows** |        ✅       |        ✅        |       ✅      |        ✅       |
| **macOS**   |        ✅       |        ✅        |       ✅      |        ✅       |
| **Linux**   |        ✅       |        ✅        |       ✅      |       N/A      |

移动端：

| <br />      | **Chrome** | **Safari** | **Android WebView** |
| ----------- | :--------: | :--------: | :-----------------: |
| **iOS**     |      ✅     |      ✅     |         N/A         |
| **Android** |      ✅     |     N/A    |    Android 5.0+ ✅   |

> 不支持 IE 11 及以下版本。

***

## 📦 构建部署

```bash
# 发布后端
cd Scm.Net
dotnet publish -c Release -o ./Publish

# 构建前端（需先克隆 Scm.Vue）
cd ../Scm.Vue
npm run build

# 将前端 dist/ 部署至后端 wwwroot 或独立部署至 Nginx/IIS
```

> **注意**: 生产环境需在 `appsettings.json` 中正确配置 `Kestrel` 监听地址与 `Cors` 跨域设置。

***

## 🔗 相关链接

- [前端项目 Scm.Vue](https://gitee.com/leadiot/scm.vue) — Vue 3 + Vite + Element Plus 前端框架
- [在线文档](https://gitee.com/leadiot/scm.net/wikis/%E9%A1%B9%E7%9B%AE%E4%BB%8B%E7%BB%8D) — 完整开发文档
- [环境搭建教程](https://gitee.com/leadiot/scm.net/wikis/%E7%8E%AF%E5%A2%83%E6%90%AD%E5%BB%BA%E6%95%99%E7%A8%8B) — 从零开始搭建开发环境
- [数据库配置说明](https://gitee.com/leadiot/scm.net/wikis/%E6%95%B0%E6%8D%AE%E5%BA%93%E9%85%8D%E7%BD%AE) — 多数据库引擎配置指南
- [在线演示](http://www.c-scm.net) — 体验系统功能

***

## 📄 开源协议

本项目基于 [MIT License](LICENSE) 开源，允许自由使用、修改和分发，商业使用请保留原始版权声明。

***

## 💬 交流反馈

### QQ 交流群

[![QQ Group](https://img.shields.io/badge/QQ%20群-415872667-12B7F5.svg?logo=tencentqq)](https://qm.qq.com/cgi-bin/qm/qr?k=415872667)

<img src="qqq.jpg" width="300" alt="QQ 群二维码" />

### 特别鸣谢

1. ORM 框架 **[SqlSugar](https://gitee.com/dotnetchina/SqlSugar)**
2. 动态 API 代码借鉴自 **[Panda.DynamicWebApi](https://gitee.com/mirrors/Panda.DynamicWebApi)**
3. 感谢所有提交 Issue 和 PR 的社区贡献者

### 赞赏支持

如果本项目对您有帮助，欢迎赞赏支持作者持续维护：

<img src="wepay.jpg" width="300" alt="赞赏码" />
