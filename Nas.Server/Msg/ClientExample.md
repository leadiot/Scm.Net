# NAS 消息推送客户端示例

## 概述

本示例展示如何在客户端（桌面、移动设备等）使用 SignalR 连接到 NAS 服务端并接收实时消息推送。

## 客户端实现

### 1. 安装依赖

#### .NET 客户端
```bash
dotnet add package Microsoft.AspNetCore.SignalR.Client
```

#### JavaScript 客户端
```bash
npm install @microsoft/signalr
```

### 2. .NET 客户端示例

```csharp
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

public class NasClient
{
    private HubConnection _hubConnection;

    public async Task ConnectAsync(string baseUrl, string accessToken)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/scmhub", options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(accessToken);
            })
            .Build();

        // 注册消息接收处理
        _hubConnection.On<ScmResultResponse<NasMessage>>("ReceiveNasMessage", (response) =>
        {
            var message = response.Data;
            Console.WriteLine($"[消息] {message.Title}: {message.Content}");
            Console.WriteLine($"[路径] {message.Path}");
        });

        // 注册同步状态消息
        _hubConnection.On<ScmResultResponse<NasSyncMessage>>("ReceiveNasSyncStatus", (response) =>
        {
            var syncMessage = response.Data;
            Console.WriteLine($"[同步状态] {syncMessage.FilePath}");
            Console.WriteLine($"[状态] {syncMessage.Status}");
            Console.WriteLine($"[消息] {syncMessage.Message}");
        });

        // 注册文件夹变更消息
        _hubConnection.On<ScmResultResponse<NasFolderMessage>>("ReceiveNasFolderChange", (response) =>
        {
            var folderMessage = response.Data;
            Console.WriteLine($"[文件夹变更] {folderMessage.FolderPath}");
            Console.WriteLine($"[变更类型] {folderMessage.ChangeType}");
        });

        await _hubConnection.StartAsync();
        Console.WriteLine("已连接到 NAS 消息服务");
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }
}
```

### 3. JavaScript 客户端示例

```javascript
import * as signalR from "@microsoft/signalr";

class NasClient {
    constructor() {
        this.connection = null;
    }

    async connect(baseUrl, accessToken) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(`${baseUrl}/scmhub`, {
                accessTokenFactory: () => accessToken
            })
            .build();

        // 注册消息接收处理
        this.connection.on("ReceiveNasMessage", (response) => {
            const message = response.data;
            console.log(`[消息] ${message.title}: ${message.content}`);
            console.log(`[路径] ${message.path}`);
        });

        // 注册同步状态消息
        this.connection.on("ReceiveNasSyncStatus", (response) => {
            const syncMessage = response.data;
            console.log(`[同步状态] ${syncMessage.filePath}`);
            console.log(`[状态] ${syncMessage.status}`);
            console.log(`[消息] ${syncMessage.message}`);
        });

        // 注册文件夹变更消息
        this.connection.on("ReceiveNasFolderChange", (response) => {
            const folderMessage = response.data;
            console.log(`[文件夹变更] ${folderMessage.folderPath}`);
            console.log(`[变更类型] ${folderMessage.changeType}`);
        });

        await this.connection.start();
        console.log("已连接到 NAS 消息服务");
    }

    async disconnect() {
        if (this.connection) {
            await this.connection.stop();
        }
    }
}
```

## 服务端消息触发点

### 1. 文件创建
- 触发点：`NasSyncService.DealCreateDoc`
- 消息类型：`FileOperation`
- 内容：文件创建成功通知

### 2. 文件删除
- 触发点：`NasSyncService.DealDeleteDoc`
- 消息类型：`FileOperation`
- 内容：文件删除成功通知

### 3. 目录创建
- 触发点：`NasSyncService.DealCreateDir`
- 消息类型：`FolderChange`
- 内容：目录创建通知

### 4. 同步状态
- 触发点：可在同步服务中根据需要调用
- 消息类型：`SyncStatus`
- 内容：同步进度和状态

## 消息类型说明

### NasMessageType
- `System` - 系统通知
- `FileOperation` - 文件操作
- `SyncStatus` - 同步状态
- `Error` - 错误提示
- `Warning` - 警告

### NasSyncStatus
- `Syncing` - 同步中
- `Completed` - 同步完成
- `Failed` - 同步失败
- `Paused` - 暂停

### NasFolderChangeType
- `Created` - 创建
- `Modified` - 修改
- `Deleted` - 删除
- `Renamed` - 重命名

## 安全考虑

1. **身份验证**：客户端连接时需要提供有效的 `access_token`
2. **消息加密**：使用 HTTPS 确保消息传输安全
3. **权限控制**：服务端会验证用户权限，确保用户只能接收自己的消息
4. **消息限流**：避免过多消息导致客户端性能问题

## 故障处理

1. **连接断开**：客户端应实现自动重连机制
2. **消息丢失**：关键操作应在服务端持久化，客户端上线后可重新获取
3. **网络延迟**：实现消息队列，确保消息有序送达

## 性能优化

1. **消息批处理**：合并多个小消息为一个批处理消息
2. **消息过滤**：客户端可根据需要订阅特定类型的消息
3. **连接管理**：合理管理连接生命周期，避免资源泄漏

## 总结

通过 SignalR 实现的 NAS 消息推送功能，可以：
- 实时通知客户端文件操作结果
- 提供同步状态的实时反馈
- 支持跨平台客户端（桌面、移动设备等）
- 确保消息的安全可靠传输

客户端只需实现相应的消息处理逻辑，即可接收来自 NAS 服务端的实时通知。
