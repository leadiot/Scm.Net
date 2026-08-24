using Com.Scm.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "scm")]
    public class AboutController : ApiController
    {
        private EnvConfig _EnvConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="envConfig"></param>
        public AboutController(EnvConfig envConfig)
        {
            _EnvConfig = envConfig;
        }

        /// <summary>
        /// 获取其它信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        [HttpGet("info")]
        public async Task<string> GetInfoAsync(string code, string section)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                code = ScmServerEnv.APP_CODE;
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
