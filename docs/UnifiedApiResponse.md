# 统一 API 响应格式使用文档

## 概述

本文档介绍 Scm.Net 框架中统一 API 响应格式的实现和使用方法。

## 响应格式

所有 API 响应都遵循以下统一格式：

```json
{
  "success": true,
  "code": 200,
  "message": "操作成功",
  "timestamp": 1704067200000,
  "path": "/api/demo/success",
  "data": { }
}
```

### 字段说明

| 字段 | 类型 | 说明 |
|------|------|------|
| `success` | boolean | 是否成功 |
| `code` | int | 状态码 |
| `message` | string | 响应消息 |
| `timestamp` | long | 时间戳（毫秒） |
| `path` | string | 请求路径 |
| `data` | object | 响应数据（可选） |

## 状态码定义

### HTTP 标准状态码

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 400 | 参数错误 |
| 401 | 未授权 |
| 403 | 禁止访问 |
| 404 | 资源不存在 |
| 500 | 服务器内部错误 |

### 业务状态码

| 状态码 | 说明 |
|--------|------|
| 1000 | 业务逻辑错误 |
| 1001 | 数据验证失败 |
| 1002 | 数据不存在 |
| 1003 | 数据已存在 |
| 1004 | 操作失败 |
| 2000 | 第三方服务错误 |

## 使用方法

### 1. 基本响应

```csharp
[HttpGet("demo")]
public string GetDemo()
{
    return "Hello World";
    // 自动包装为：
    // { "success": true, "code": 200, "message": "操作成功", "data": "Hello World" }
}
```

### 2. 返回对象

```csharp
[HttpGet("user")]
public UserDto GetUser()
{
    return new UserDto { Id = 1, Name = "张三" };
    // 自动包装为：
    // { "success": true, "code": 200, "data": { "id": 1, "name": "张三" } }
}
```

### 3. 使用 ApiResult 手动包装

```csharp
[HttpGet("result")]
public ApiResult<UserDto> GetResult()
{
    var user = new UserDto { Id = 1, Name = "张三" };
    return ApiResult<UserDto>.Ok(user, "查询成功");
}
```

### 4. 分页响应

```csharp
[HttpGet("list")]
public ApiResult<PagedResult<UserDto>> GetList(int page = 1, int pageSize = 10)
{
    var users = _userService.GetPage(page, pageSize);
    return ApiResult<UserDto>.OkPaged(users.Items, users.Total, page, pageSize);
}
```

响应结果：
```json
{
  "success": true,
  "code": 200,
  "message": "查询成功",
  "data": {
    "items": [...],
    "total": 100,
    "page": 1,
    "pageSize": 10,
    "totalPages": 10
  }
}
```

## 异常处理

### 抛出业务异常

```csharp
[HttpGet("check")]
public IActionResult CheckUser(int id)
{
    if (id <= 0)
    {
        throw new ValidationException("用户ID必须大于0");
    }
    
    var user = _userService.GetById(id);
    if (user == null)
    {
        throw new NotFoundException("用户不存在");
    }
    
    if (!user.IsActive)
    {
        throw new BusinessException("用户已被禁用");
    }
    
    return Ok(user);
}
```

### 异常类型

| 异常类型 | HTTP 状态码 | 业务码 |
|----------|-------------|--------|
| `ValidationException` | 400 | 1001 |
| `NotFoundException` | 404 | 1002 |
| `UnauthorizedException` | 401 | 401 |
| `ForbiddenException` | 403 | 403 |
| `BusinessException` | 200 | 1000 |
| `ApiException` | 200 | 自定义 |

### 异常响应示例

```json
{
  "success": false,
  "code": 1002,
  "message": "用户不存在",
  "timestamp": 1704067200000,
  "path": "/api/users/999"
}
```

## 配置说明

### 注册服务（Program.cs）

```csharp
// 添加统一响应服务
services.AddUnifiedResponse();

// 添加 API 行为配置
services.AddApiBehavior();
```

### 使用中间件

```csharp
// 统一响应中间件（包装所有 API 响应）
app.UseUnifiedResponse();
```

### 跳过统一响应

如果需要某些 API 不经过统一响应包装，可以使用 `[SkipUnifiedResponse]` 特性：

```csharp
[SkipUnifiedResponse]
[HttpGet("raw")]
public IActionResult GetRawData()
{
    return Content("原始数据");
}
```

## 测试接口

框架提供了演示控制器 `ApiDemoController`，包含以下测试接口：

| 接口 | 说明 |
|------|------|
| `GET /api/demo/success` | 成功响应示例 |
| `GET /api/demo/object` | 返回对象示例 |
| `GET /api/demo/list` | 返回列表示例 |
| `GET /api/demo/result` | 使用 ApiResult 示例 |
| `GET /api/demo/paged` | 分页响应示例 |
| `GET /api/demo/business-error` | 业务异常示例 |
| `GET /api/demo/not-found` | 数据不存在异常示例 |
| `GET /api/demo/validation-error` | 验证异常示例 |
| `GET /api/demo/unauthorized` | 未授权异常示例 |
| `GET /api/demo/forbidden` | 禁止访问异常示例 |
| `GET /api/demo/server-error` | 服务器错误示例 |
| `POST /api/demo/create` | 创建数据示例 |

## 最佳实践

1. **优先使用自动包装**：让中间件自动包装响应，减少样板代码
2. **合理使用异常**：业务逻辑错误使用自定义异常，系统错误让框架处理
3. **提供有意义的错误信息**：异常消息应该清晰描述问题
4. **分页使用 PagedResult**：统一分页数据结构
5. **避免在 Controller 中处理异常**：让全局异常过滤器统一处理

## 注意事项

1. 文件下载接口不会被包装，保持原始行为
2. Swagger 文档路径不会被包装
3. SignalR WebSocket 连接不会被包装
4. 生产环境会隐藏详细的异常堆栈信息
