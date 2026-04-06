using Microsoft.AspNetCore.Hosting;

namespace Com.Scm
{
    /// <summary>
    /// 项目基本配置
    /// </summary>
    public class DllConfig
    {
        public const string NAME = "Project";

        /// <summary>
        /// 项目依赖DLL
        /// </summary>
        public string[] Service { get; set; }
        /// <summary>
        /// 项目根目录
        /// </summary>
        public string Root { get; set; }

        public void Prepare(IWebHostEnvironment environment)
        {
            Root = environment.ContentRootPath;
        }
    }
}
