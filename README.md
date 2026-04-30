
# Scm.Net

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4.svg?logo=dotnet)](https://dotnet.microsoft.com)
[![Vue](https://img.shields.io/badge/Vue-3.0-4FC08D.svg?logo=vuedotjs)](https://vuejs.org)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey.svg)](https://github.com)
[![QQ Group](https://img.shields.io/badge/QQ-415872667-12B7F5.svg?logo=tencentqq)](https://qm.qq.com)

## 项目介绍

一款基于 **.NET 10.0** 及 **Vue 3.0** 构架、适用于企业中后台管理系统的快速开发框架。

笔者多年从事供应链系统及企业信息化系统的产品与研发，经常面对异构应用场景需求，在梳理之前多种项目经验的过程中，特开发此项目以期帮助各位同仁快速搭建一个完整的开发框架，并满足多场景下的异构应用场景需求。

以下是笔者以及其它伙伴基于此项目开发的产品：**OMS**（订单管理系统）、**WMS**（仓储管理系统）、**TMS**（运输管理系统）、**DMS**（配送管理系统）、**BMS**（计费管理系统）、**YMS**（园区管理系统）、**EAM**（资产管理系统）、**IOT**（物联网管理系统）等。

当然，此项目还在不断完善的过程中，还存在不少待完善的事项，也欢迎有兴趣的同仁一起交流沟通。

## 软件架构

1. 采用前后端分离模式；
2. 后端基于 **.NET 10.0** 开发，兼容 .NET 6/7/8/9/10 运行时；
3. 前端基于 **[Vue 3.0](https://vuejs.org)** 及 **[Element Plus](https://element-plus.org)** 开发，支持 i18n 多语言；
4. 系统无平台依赖，可直接在多平台（**Windows**、**macOS**、**Linux**、**HarmonyOS** 等）开发与运行；
5. 响应式布局，支持多种设备终端（**电脑**、**平板**、**手机**）。

### 后端核心依赖

| 依赖库 | 用途 |
| --- | --- |
| [SqlSugarCore](https://www.donet5.com/Home/Doc) | ORM 数据访问 |
| [Newtonsoft.Json](https://www.newtonsoft.com/json) | JSON 序列化 |
| [ImageSharp](https://github.com/SixLabors/ImageSharp) | 跨平台图像处理 |
| [MQTTnet](https://github.com/dotnet/MQTTnet) | MQTT 通讯（客户端 + 内置 Broker） |
| [RabbitMQ.Client](https://www.rabbitmq.com) | RabbitMQ 消息队列 |
| [Quartz.NET](https://www.quartz-scheduler.net) | 定时任务调度 |
| SignalR | 实时通讯 |

## 项目结构

| 项目 | 说明 |
| --- | --- |
| `Scm.Net` | Web API 主入口（Program.cs、Controllers） |
| `Scm.Core` | 核心业务逻辑层 |
| `Scm.Dao` | 数据访问层（DAO） |
| `Scm.Dto` | 数据传输对象层（DTO） |
| `Scm.Common` | 公共枚举、工具类 |
| `Scm.Common.Dto` | 公共 DTO 定义 |
| `Scm.Dsa.Dba.Sugar` | SqlSugar 仓储基类封装 |
| `Scm.Server.Bearer` | JWT Bearer 认证扩展 |
| `Scm.Server.Cache` | 缓存扩展（MemoryCache / Redis） |
| `Scm.Server.Dao` | 服务端 DAO 扩展 |
| `Scm.Server.MQTT` | MQTT 通讯（客户端 + 内置 Broker） |
| `Scm.Server.RabbitMQ` | RabbitMQ 消息队列集成 |
| `Scm.Server.SignalR` | SignalR 实时通讯 |
| `Scm.Server.Quartz` | Quartz 定时任务调度 |
| `Scm.Server.Swagger` | Swagger 文档扩展 |
| `Scm.Generator` | 代码生成器（支持自定义模板） |
| `Scm.Plugin` | 插件扩展基础框架 |
| `Scm.Plugin.Image` | 图像处理插件（条码、水印、验证码等） |
| `Scm.Plugin.Audio` | 音频解析插件 |
| `Scm.Plugin.Video` | 视频转码插件 |
| `Scm.Addon` | 插件加载机制 |
| `Samples.*` | 使用示例工程 |

## 设计原则

1. 数据库仅用于**存储数据**，除 CRUD 以外不使用任何依赖特定数据库的特性，项目可平滑迁移到任何支持标准 SQL 的数据引擎；
2. 数据库原则上仅允许**单表操作**，最多不超过两张表，可以一定程度上进行数据冗余设计，以提升数据引擎效率；
3. 基于 **JSON 格式**的多端数据交互，在保证数据低噪音的前提下提升数据可扩展性；
4. DTO（数据传输层）统一使用**蛇形命名法**，适配多场景异构应用需求。

## 主要功能

1. 首页自定义风格；
2. 支持多种**登录方式**（账户、手机、邮件、三方 OAuth 等）；
3. 支持多种**数据引擎**（[MySQL](https://www.mysql.com/)、[SQL Server](https://www.microsoft.com/zh-cn/sql-server/)、[Oracle](https://www.oracle.com/)、[SQLite](https://sqlite.org/)、[MariaDB](https://mariadb.org/)、[PostgreSQL](https://www.postgresql.org/)、[Firebird](https://firebirdsql.org/)、[MongoDB](https://www.mongodb.com/) 等）；
4. 支持多种**缓存机制**（MemoryCache、Map、Redis 等）；
5. 支持**登录日志**与**操作日志**，并记录用户终端信息（登录主机、操作系统、浏览器、终端代码等）；
6. 支持集成多种**大语言模型**（[DeepSeek](https://www.deepseek.com/)、[华为盘古](https://pangu.huaweicloud.com/)、[阿里通义千问](https://www.tongyi.com/)、[腾讯元宝](https://yuanbao.tencent.com/)、[百度文心一言](https://yiyan.baidu.com/)、[豆包](https://www.doubao.com/)、[ChatGPT](https://chatgpt.com/)）；
7. 支持**代码自动生成**，支持自定义代码模板（实体类、DAO、DTO/VO 等）；
8. 集成 **ID 生成器**，支持雪花 ID、序列 ID、格式 ID 等多种生成方式；
9. 支持**多级权限管理**：公司管理、部门管理、岗位管理、分组管理、用户管理、角色管理；
10. 支持**全局数据字典**；
11. 支持**全局配置参数**；
12. 支持**用户留言**与实时反馈；
13. 支持**自定义审批流程**（流程定义、节点配置、表单绑定、在线审批）；
14. 支持 **MQTT** 轻量级通讯（客户端发布/订阅 + 内置 Broker，适用于 IoT 场景）；
15. 支持 **RabbitMQ** 消息队列（发布者/消费者模式）；
16. 支持 **SignalR** 实时推送通讯；
17. 支持 **Quartz.NET** 定时任务调度；
18. 支持**图像处理插件**（条码生成识别、图片水印、图形验证码、头像裁剪）；
19. 支持**插件扩展机制**（Addon 动态加载）。

## 项目特色

1. 系统提供完善的示例与操作说明；
2. 系统将不同功能进行**模块化拆分**，可以根据需要按需引入；
3. 前台与后台系统分离，分别为不同的系统（域名可独立）；
4. 后台系统无需任何二次开发，直接发布即可使用；
5. 可扩展为**多租户**、**多组织架构**应用；
6. 完整的 Swagger 接口文档支持，开箱即用。

[查看完整文档](wikis/%E9%A1%B9%E7%9B%AE%E4%BB%8B%E7%BB%8D)

## 快速开始

### 1. 环境准备

| 工具 | 版本要求 | 下载地址 |
| --- | --- | --- |
| .NET SDK | ≥ 10.0 | [官网](https://dotnet.microsoft.com) |
| Visual Studio | ≥ 2026 | [官网](https://visualstudio.microsoft.com) |
| MariaDB / MySQL | ≥ 10.3 | [官网](https://mariadb.org) |

### 2. 获取代码

```bash
git clone https://gitee.com/openscm/scm.net.git
```

### 3. 配置数据库

编辑 `Scm.Net/appsettings.json`，修改数据库连接串：

```json
{
  "Sql": {
    "Type": "Sqlite",
    "Text": "Data Source=D:/data/scm.db;"
  },
}
```

系统会默认进行数据库初始化处理。

### 4. 启动后端

```bash
cd Scm.Net
dotnet run
# 或使用 Visual Studio 直接 F5 运行
```

访问 `http://localhost:5000/swagger` 确认接口正常。

### 5. 启动前端

```bash
cd Scm.Vue   # 前端项目目录
npm install
npm run dev
```

详细说明请参考：[环境搭建教程](wikis/%E7%8E%AF%E5%A2%83%E6%90%AD%E5%BB%BA%E6%95%99%E7%A8%8B) | [数据库配置说明](wikis/%E6%95%B0%E6%8D%AE%E5%BA%93%E9%85%8D%E7%BD%AE)

## 演示地址

【登录地址】[点击访问](http://www.c-scm.net)

> 演示账号请访问 [演示说明页](wikis/%E6%BC%94%E7%A4%BA%E8%AF%B4%E6%98%8E) 获取。

## 浏览器支持

支持所有现代浏览器（不支持 IE）：

![chrome](https://img.shields.io/badge/Chrome-≥88-success.svg?logo=googlechrome&logoColor=white)
![firefox](https://img.shields.io/badge/Firefox-≥78-success.svg?logo=firefox&logoColor=white)
![edge](https://img.shields.io/badge/Edge-≥88-success.svg?logo=microsoftedge&logoColor=white)
![safari](https://img.shields.io/badge/Safari-≥14-success.svg?logo=safari&logoColor=white)

桌面端：

|             | **Chrome ≥88** | **Firefox ≥78** | **Edge ≥88** | **Safari ≥14** |
| -------     | :-----------:  | :-------------: | :----------: | :------------: |
| **Windows** | ✅             | ✅              | ✅           | ✅             |
| **macOS**   | ✅             | ✅              | ✅           | ✅             |
| **Linux**   | ✅             | ✅              | ✅           | N/A            |

移动端：

|               | **Chrome** | **Safari** | **Android WebView** |
| -------       | :--------: | :--------: | :-----------------: |
| **iOS**       | ✅         | ✅         | N/A                 |
| **Android**   | ✅         | N/A        | Android 5.0+ ✅     |

## 常见问题

[查看常见问题](wikis/%E5%B8%B8%E8%A7%81%E9%97%AE%E9%A2%98)

## 开源协议

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

本项目基于 **MIT** 协议开源，允许自由使用、修改和分发，商业使用请保留原始版权声明。

## 项目截图

**工作台模式**

![后台首页](screenshots/web-02-home.png)
![用户管理](screenshots/web-03-user.png)
![文件管理](screenshots/web-04-file.png)
![日程管理](screenshots/web-05-calendar.png)
![电子邮件](screenshots/web-06-email.png)
![系统监控](screenshots/web-07-monitor.png)
![在线文档](screenshots/web-08-docs.png)

**云桌面模式**

![用户首页](screenshots/wos-02-desktop-default.png)
![文件管理](screenshots/wos-09-cloud.png)
![记事](screenshots/wos-10-notepad.png)
![图像](screenshots/wos-11-image.png)
![音频](screenshots/wos-12-audio.png)
![待办](screenshots/wos-13-gtd.png)
![终端管理](screenshots/wos-14-terminal.png)

**手机端页面**

![用户登录](screenshots/mp-login.jpg)
![用户首页](screenshots/mp-home.jpg)
![系统菜单](screenshots/mp-menu.jpg)

更多截图请访问 [项目文档](wikis)。

## 特别鸣谢

1. ORM 框架 **[SqlSugar](https://gitee.com/dotnetchina/SqlSugar)**；
2. 动态 API 代码借鉴自 **[Panda.DynamicWebApi](https://gitee.com/mirrors/Panda.DynamicWebApi)**；
3. 感谢所有提交 Issue 和 PR 的社区贡献者。

## 社区与支持

**QQ 交流群**

[![QQ Group](https://img.shields.io/badge/QQ%20群-415872667-12B7F5.svg?logo=tencentqq)](https://qm.qq.com)

<img src="qq.jpg" width="200"/>

**赞赏支持**

如果本项目对您有帮助，欢迎赞赏支持作者持续维护：

<img src="wepay.jpg" width="200"/>
