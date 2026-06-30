namespace Com.Scm.Config
{
    public class ScalarConfig
    {
        public const string NAME = "Scalar";

        /// <summary>
        /// API 文档标题
        /// </summary>
        public string Title { get; set; } = "Scm.Net API 文档";

        /// <summary>
        /// API 文档描述
        /// </summary>
        public string Description { get; set; } = "系统管理平台 API 接口文档";

        /// <summary>
        /// API 版本
        /// </summary>
        public string Version { get; set; } = "v1.0";

        /// <summary>
        /// 默认语言
        /// </summary>
        public string Language { get; set; } = "zh-CN";

        /// <summary>
        /// Scalar 文档路径
        /// </summary>
        public string ScalarRoute { get; set; } = "/scalar/v1";

        /// <summary>
        /// OpenAPI 文档路径
        /// </summary>
        public string OpenApiRoute { get; set; } = "/openapi/v1.json";

        /// <summary>
        /// 是否在生产环境显示
        /// </summary>
        public bool ShowInProduction { get; set; } = false;

        /// <summary>
        /// API 服务器地址列表
        /// </summary>
        public List<ApiServer> Servers { get; set; } = new List<ApiServer>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> DllXmls { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ApiInfo> ApiDocs { get; set; }

        public void LoadDef()
        {
            Servers = new List<ApiServer>
            {
                new ApiServer { Url = "http://localhost:9999", Description = "本地开发环境" },
                new ApiServer { Url = "http://localhost:5000", Description = "备用开发环境" }
            };
            DllXmls = new List<string>();
            ApiDocs = new List<ApiInfo>();
        }

        public bool HasDocs()
        {
            //return ApiDocs != null && ApiDocs.Count > 0;
            return false;
        }
    }

    /// <summary>
    /// API 服务器配置
    /// </summary>
    public class ApiServer
    {
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public class ApiInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
