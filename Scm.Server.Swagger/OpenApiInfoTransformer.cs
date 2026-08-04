using Com.Scm.Config;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Com.Scm.Swagger
{
    /// <summary>
    /// 用 SwaggerConfig 中的标题/描述/版本回填文档 info，
    /// 覆盖原生 OpenAPI 默认生成的 "{ApplicationName} | {documentName}" 标题
    /// </summary>
    internal sealed class OpenApiInfoTransformer : IOpenApiDocumentTransformer
    {
        private readonly SwaggerConfig _config;
        private readonly ApiInfo _apiDoc;

        public OpenApiInfoTransformer(SwaggerConfig config, ApiInfo apiDoc)
        {
            _config = config;
            _apiDoc = apiDoc;
        }

        public Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            document.Info ??= new OpenApiInfo();

            if (_apiDoc != null)
            {
                if (!string.IsNullOrEmpty(_apiDoc.Title))
                {
                    document.Info.Title = _apiDoc.Title;
                }
                if (!string.IsNullOrEmpty(_apiDoc.Description))
                {
                    document.Info.Description = _apiDoc.Description;
                }
                if (!string.IsNullOrEmpty(_apiDoc.Version))
                {
                    document.Info.Version = _apiDoc.Version;
                }
            }
            else if (_config != null)
            {
                if (!string.IsNullOrEmpty(_config.Title))
                {
                    document.Info.Title = _config.Title;
                }
                if (!string.IsNullOrEmpty(_config.Description))
                {
                    document.Info.Description = _config.Description;
                }
                if (!string.IsNullOrEmpty(_config.Version))
                {
                    document.Info.Version = _config.Version;
                }
            }

            return Task.CompletedTask;
        }
    }
}
