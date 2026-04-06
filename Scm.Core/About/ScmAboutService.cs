using Com.Scm.Config;
using Com.Scm.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.About
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "About")]
    public class ScmAboutService : ApiService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="envConfig"></param>
        public ScmAboutService(EnvConfig envConfig)
        {
            _EnvConfig = envConfig;
        }

        /// <summary>
        /// 获取其它信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public async Task<string> GetInfoAsync(string code, string section)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                code = "Scm.Net";
            }
            if (string.IsNullOrEmpty(section))
            {
                section = "index";
            }

            var file = _EnvConfig.GetDataPath($"about/{code}/{section}.txt");
            if (!System.IO.File.Exists(file))
            {
                file = _EnvConfig.GetDataPath($"about/{section}.txt");
                if (!System.IO.File.Exists(file))
                {
                    file = _EnvConfig.GetDataPath($"about/default.txt");
                }
            }

            return await _EnvConfig.ReadFileAsync(file);
        }
    }
}
