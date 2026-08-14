using Com.Scm.Ai.Config;
using Com.Scm.Ai.Dvo;
using Com.Scm.Exceptions;
using Com.Scm.Utils;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Com.Scm.Ai.Provider
{
    /// <summary>
    /// OpenAI兼容协议客户端，适用于 DeepSeek、通义千问（DashScope兼容模式）等服务
    /// </summary>
    public class OpenAiCompatibleClient
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _http;
        private readonly AiProviderConfig _config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public OpenAiCompatibleClient(AiProviderConfig config)
        {
            _config = config;
            _http = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(config.Timeout > 0 ? config.Timeout : 300)
            };
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.ApiKey);
        }

        /// <summary>
        /// 对话（非流式）
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public async Task<AiChatResult> ChatAsync(List<AiChatMessage> messages, string model, float temperature = 0.7f)
        {
            var body = BuildChatBody(messages, model, temperature, false);

            var response = await PostAsync("chat/completions", body);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new BusinessException($"AI对话失败：{response.StatusCode}，{json}");
            }

            var result = JsonSerializer.Deserialize<AiChatCompletion>(json, _jsonOptions);
            var content = result?.choices?.FirstOrDefault()?.message?.content;

            return new AiChatResult()
            {
                content = content,
                model = result?.model
            };
        }

        /// <summary>
        /// 对话（流式SSE），返回原始响应流，由调用方负责读取与释放
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ChatStreamAsync(List<AiChatMessage> messages, string model, float temperature = 0.7f)
        {
            var body = BuildChatBody(messages, model, temperature, true);

            var request = new HttpRequestMessage(HttpMethod.Post, BuildUrl("chat/completions"))
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                response.Dispose();
                throw new BusinessException($"AI对话失败：{response.StatusCode}，{json}");
            }

            return response;
        }

        /// <summary>
        /// 文本向量化
        /// </summary>
        /// <param name="texts"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<float[]>> EmbedAsync(IReadOnlyList<string> texts, string model)
        {
            var payload = new
            {
                model = model,
                input = texts
            };
            var body = JsonSerializer.Serialize(payload);

            var response = await PostAsync("embeddings", body);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new BusinessException($"AI向量化失败：{response.StatusCode}，{json}");
            }

            var result = JsonSerializer.Deserialize<AiEmbeddingResult>(json, _jsonOptions);
            if (result?.data == null)
            {
                throw new BusinessException("AI向量化返回数据为空！");
            }

            return result.data
                .OrderBy(a => a.index)
                .Select(a => a.embedding)
                .ToList();
        }

        private string BuildChatBody(List<AiChatMessage> messages, string model, float temperature, bool stream)
        {
            var payload = new
            {
                model = model,
                messages = messages.Select(a => new { role = a.role, content = a.content }),
                temperature = temperature,
                stream = stream
            };

            return JsonSerializer.Serialize(payload);
        }

        private async Task<HttpResponseMessage> PostAsync(string path, string body)
        {
            if (!_config.IsValid())
            {
                throw new BusinessException("AI服务未配置，请先配置 Ai 节点的 BaseUrl 与 ApiKey！");
            }

            var request = new HttpRequestMessage(HttpMethod.Post, BuildUrl(path))
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            try
            {
                return await _http.SendAsync(request);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex);
                throw new BusinessException("AI服务请求异常：" + ex.Message);
            }
        }

        private string BuildUrl(string path)
        {
            var baseUrl = _config.BaseUrl.TrimEnd('/');
            return $"{baseUrl}/{path}";
        }
    }
}
