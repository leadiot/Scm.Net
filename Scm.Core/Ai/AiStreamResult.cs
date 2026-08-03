using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Ai
{
    /// <summary>
    /// SSE流式输出结果，将AI服务的流式响应透传给客户端
    /// </summary>
    public class AiStreamResult : IActionResult
    {
        private readonly HttpResponseMessage _response;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response">AI服务的原始流式响应</param>
        public AiStreamResult(HttpResponseMessage response)
        {
            _response = response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var http = context.HttpContext;
            http.Response.ContentType = "text/event-stream";
            http.Response.Headers.CacheControl = "no-cache";
            http.Response.Headers.Connection = "keep-alive";

            try
            {
                using (_response)
                {
                    using var stream = await _response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    while (!http.RequestAborted.IsCancellationRequested)
                    {
                        var line = await reader.ReadLineAsync(http.RequestAborted);
                        if (line == null)
                        {
                            break;
                        }

                        // 跳过空行，按SSE事件格式逐条透传
                        if (line.Length > 0)
                        {
                            await http.Response.WriteAsync(line + "\n\n", http.RequestAborted);
                            await http.Response.Body.FlushAsync(http.RequestAborted);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 客户端中断请求，忽略
            }
        }
    }
}
