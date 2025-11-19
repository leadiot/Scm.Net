using Com.Scm.Config;
using Com.Scm.Dev;
using Com.Scm.Dev.App.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.App
{
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysAppService : ApiService
    {
        private readonly SugarRepository<ScmDevAppDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userRepository"></param>
        public ScmSysAppService(SugarRepository<ScmDevAppDao> thisRepository, EnvConfig envConfig)
        {
            _thisRepository = thisRepository;
            _EnvConfig = envConfig;
        }

        /// <summary>
        /// 获取应用的基本信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{code}"), AllowAnonymous]
        public async Task<AppDvo> GetAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = "";
            }
            code = code.ToLower();

            var dvo = await _thisRepository
                .AsQueryable()
                .Where(a => a.code == code && a.row_status == ScmRowStatusEnum.Enabled)
                .Select<AppDvo>()
                .FirstAsync();
            if (dvo == null)
            {
                dvo = new AppDvo();
            }

            return dvo;
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

            var file = _EnvConfig.GetDataPath($"About/{code}/{section}.txt");
            if (!System.IO.File.Exists(file))
            {
                file = _EnvConfig.GetDataPath($"About/{section}.txt");
                if (!System.IO.File.Exists(file))
                {
                    file = _EnvConfig.GetDataPath($"About/default.txt");
                }
            }

            return await _EnvConfig.ReadFileAsync(file);
        }
    }
}
