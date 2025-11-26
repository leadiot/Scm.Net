

# Scm.Net

## 项目介绍
一款基于 **.Net 10.0** 及 **Vue 3.0** 构架、适用于企业中后台管理系统的快速开发框架。   

笔者多年从事供应链系统及企业信息化系统的产品与研发，经常面对异构应用场景需求，在梳理之前多种项目经验的过程中，特开发此项目以期帮助各位同仁位快速搭建一个完整的开发框架，并满足多场景下的异构应用场景需求。  

以下是笔者以及其它伙伴基于此项目开发的产品：**OMS**（订单管理系统）、**WMS**（仓储管理系统）、**TMS**（运输管理系统）、**DMS**（配送管理系统）、**BMS**（计费管理系统）、**YMS**（园区管理系统）、**EAM**（资产管理系统）、**IOT**（物联网管理系统）等。  

当然，此项目还在不断完善的过程中，还存在不少待完善的事项，也欢迎有兴趣的同仁一起交流沟通。  

## 软件架构
1. 采用前后端分离模式；  
2. 后端基于 **.Net 10.0** 开发，可直接编译输出 .Net6/7/8/9/10 的运行时；  
3. 后端仅依赖几个常用的三方类库：**[SqlSugarCore](https://www.donet5.com/Home/Doc)**(ORM工具）、**[Newtonsoft.Json](https://www.newtonsoft.com/json)**（JSON工具）、**[ImageSharp](https://github.com/SixLabors/ImageSharp)**（跨平台图像工具）；  
4. 前端基于 **[Vue 3.0](https://vuejs.org)** 及 **[Element Plus](https://element-plus.gitee.io)** 开发；  
5. 前端仅依赖几个常用的三方组件：**[Axios](https://axios-http.com/)**（HTTP工具），良好支持i18n（多语言）；  
6. 系统无平台依赖，可直接在多平台（**Windows**、**MacOS**、**Linux**、**HarmonyOS**等）开发与运行；  
7. 响应式布局，支持多种设备终端（**电脑**、**平板**、**手机**）等。  

## 设计原则
1. 数据库仅用于 **存储数据** ，除CRUD以外不使用任何依赖特定数据库的特性，项目可平滑迁移到任何支持标准SQL的数据引擎；  
2. 数据库原则上仅允许 **单表操作** ，最多不能同时操作两张表，可以一定程度上进行数据冗余设计，以提升数据引擎效率；  
3. 基于 **Json格式** 的多端数据交互，在保证数据低噪音的前提下提升数据可扩展性；  
4. 适配多场景异构应用需求，DTO（数据传输层）统一使用 **蛇形命名法** ；  

## 主要功能  
1. 首页自定义风格；  
2. 支持多种登录方式（**账户**、**手机**、**邮件**、**三方**等）；  
3. 支持多种数据引擎（**[MySQL](https://www.mysql.com/)**、**[SQL Server](https://www.microsoft.com/zh-cn/sql-server/)**、**[Oracle](https://www.oracle.com/)**、**[SQLite](https://sqlite.org/)**、**[MariaDB](https://mariadb.org/)**、**[PostgreSQL](https://www.postgresql.org/)**、**[Firebird](https://firebirdsql.org/)**、**[MongoDB](https://www.mongodb.com/)** 等）；  
4. 支持多种缓存机制（**MemoryCache**、**Map**、**Redis**等）；  
5. 支持 **登录日志** 与 **操作日志** ，并记录用户跟进信息（**登录主机**、**操作系统**、**浏览器**、**终端代码**等）；  
6. 支持集成多种大模型（**[DeepSeek](https://www.deepseek.com/)**、**[华为盘古](https://pangu.huaweicloud.com/)**、**[阿里通义千问](https://www.tongyi.com/)**、**[腾讯元宝](https://yuanbao.tencent.com/)**、**[百度文心一言](https://yiyan.baidu.com/)**、**[豆包](https://www.doubao.com/)**、**[ChatGPT](https://chatgpt.com/)**）；  
7. 支持代码自动生成，支持自定义代码模板；  
8. 集成ID生成器，支持雪花ID、序列ID、格式ID等多种生成方式；  
9. 支持多级权限管理：公司管理、部门管理、岗位管理、分组管理、用户管理、角色管理等。  
10. 支持全局数据字典；  
11. 支持全局配置参数；  
12. 支持用户留言与实时反馈；  
13. 支持自定义审批流程；  

## 更新日志：
[更新日志](https://gitee.com/openscm/scm.net/wikis/更新日志)  

## 项目特色
1. 系统提供完善的示例与操作说明；  
2. 系统将不同的功能进行模块化拆分，可以根据需要引入使用；  
3. 前台与后台系统分离，分别为不同的系统（域名可独立）；  
4. 后台系统无需任何二次开发，直接发布即可使用；  
5. 可扩展为多租户、多组织架构应用；  

[查看文档](https://gitee.com/openscm/scm.net/wikis/%E9%A1%B9%E7%9B%AE%E4%BB%8B%E7%BB%8D)

## 浏览器支持

![chrome](https://img.shields.io/badge/chrome->%3D4.5-success.svg?logo=google%20chrome&logoColor=red)
![firefox](https://img.shields.io/badge/firefox->38-success.svg?logo=mozilla%20firefox&logoColor=red)
![edge](https://img.shields.io/badge/edge->%3D12-success.svg?logo=microsoft%20edge&logoColor=blue)
![ie](https://img.shields.io/badge/ie->%3D11-success.svg?logo=internet%20explorer&logoColor=blue)
![Safari](https://img.shields.io/badge/safari->%3D9-success.svg?logo=safari&logoColor=blue)
![Andriod](https://img.shields.io/badge/andriod->%3D4.4-success.svg?logo=android)
![oper](https://img.shields.io/badge/opera->%3D3.0-success.svg?logo=opera&logoColor=red)  

移动端：
![ios](https://img.shields.io/badge/ios-supported-success.svg?logo=apple&logoColor=white)
![Andriod](https://img.shields.io/badge/andriod-suported-success.svg?logo=android)

|                        |  **Chrome**  |  **Firefox**  |  **Safari**  |  **Android Browser & WebView**  |  **Microsoft Edge**  |
| -------                | ---------    | ---------     | ------       | -------------------------       | --------------       |
|  **iOS**               | 支持         | 支持           | 支持         | N/A                             | 支持                 |
|  **Android**           | 支持         | 支持           | N/A          | Android v5.0+ 支持              | 支持                 |

桌面端：
![windows](https://img.shields.io/badge/windows-suported-success.svg?logo=windows)
![macOS](https://img.shields.io/badge/macOS-supported-success.svg?logo=apple&logoColor=white)
![linux](https://img.shields.io/badge/linux-suported-success.svg?logo=linux&logoColor=white)

|             | **Chrome**    | **Firefox**   | **Internet Explorer** | **Microsoft Edge** | **Opera**     | **Safari**    |
| -------     | ---------     | ---------     | -----------------     | --------------     | ---------     | ------------- |
| **Windows** | 支持          | 支持          | 支持（IE10+）          | 支持                | 支持           | 支持          |
| **MacOS**   | 支持          | 支持          | N/A                   | N/A                | 支持           | 支持          |
| **Linux**   | 支持          | 支持          | N/A                   | N/A                | N/A            | N/A           |

## 开发环境搭建
1. 安装 .Net SDK（10.0或以上）[官方网址](https://dotnet.microsoft.com)
2. 安装 Visual Studio（2026或以上）[官方网址](https://visualstudio.microsoft.com)
3. 安装 MariaDB（10.3或上）[官方网址](https://mariadb.org)
4. 获取项目代码：[Scm.NET](https://gitee.com/openscm/scm.net)
5. 环境搭建教程：[详细说明](https://gitee.com/openscm/scm.net/wikis/%E7%8E%AF%E5%A2%83%E6%90%AD%E5%BB%BA%E6%95%99%E7%A8%8B)
6. 数据库配置：[详细说明](https://gitee.com/openscm/scm.net/wikis/%E6%95%B0%E6%8D%AE%E5%BA%93%E9%85%8D%E7%BD%AE)

## 演示地址  
【登录地址】[点击登录](http://www.c-scm.net)  
【管理账号】  
用户：admin@demo  
密码：123456  
【普通账号】  
用户：user@demo  
密码：123456  

## 常见问题
请点击 [查看文档](https://gitee.com/openscm/scm.net/wikis/%E5%B8%B8%E8%A7%81%E9%97%AE%E9%A2%98) 查看常见问题小节  

## 开源协议
[![Gitee license](https://img.shields.io/github/license/argozhang/bootstrapadmin.svg?logo=git&logoColor=red)](https://gitee.com/openscm/scm.net/blob/master/LICENSE)

## 项目截图
电脑端页面   
![后台首页](https://gitee.com/openscm/scm.net/raw/master/screenshots/pc-home.png)  
![接口日志](https://gitee.com/openscm/scm.net/raw/master/screenshots/pc-logapi.png)  
![在线SQL](https://gitee.com/openscm/scm.net/raw/master/screenshots/pc-sql.png)  
![任务调度](https://gitee.com/openscm/scm.net/raw/master/screenshots/pc-task.png)  
![系统监控](https://gitee.com/openscm/scm.net/raw/master/screenshots/pc-monitor.png)  

手机端页面   
![用户登录](https://gitee.com/openscm/scm.net/raw/master/screenshots/mp-login.jpg)  
![用户首页](https://gitee.com/openscm/scm.net/raw/master/screenshots/mp-home.jpg)  
![系统菜单](https://gitee.com/openscm/scm.net/raw/master/screenshots/mp-menu.jpg)  

更多截图请点击 [查看文档](https://gitee.com/openscm/scm.net/wikis) 查看项目截图小节  

## 特别鸣谢
1. 数据引擎 **[SqlSugar](https://gitee.com/dotnetchina/SqlSugar)**；  
2. 动态API代码借鉴自 **[Panda.DynamicWebApi](https://gitee.com/mirrors/Panda.DynamicWebApi)**；  

## QQ交流群

![QQ](https://img.shields.io/badge/QQ-415872667-green.svg?logo=tencent%20qq&logoColor=red)  
<img src="https://gitee.com/openscm/scm.net/raw/master/qq.jpg" width="30%"/>

## 支持作者

如果这个项目对您有所帮助，并希望能够给予更多的开发人员提供方便，请给予笔者支持，深表感谢。

<img src="https://gitee.com/openscm/scm.net/raw/master/wepay.jpg" width="30%"/>
