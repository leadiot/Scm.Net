using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Http;
using Com.Scm.Nas.Sync;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Api.Controllers
{
    public class DownloadController : ApiController
    {
        private EnvConfig _EnvConfig;
        private ISqlSugarClient _SqlClient;

        public DownloadController(EnvConfig envConfig, ISqlSugarClient sqlClient)
        {
            _EnvConfig = envConfig;
            _SqlClient = sqlClient;
        }

        /// <summary>
        /// 小文件下载
        /// </summary>
        /// <param name="path">要下载的文件名（含扩展名）</param>
        /// <returns>文件流</returns>
        [NoJsonResult]
        [HttpGet("ds/{id}")]
        public async Task<IActionResult> DownloadSmallFile(long id)
        {
            LogUtils.Debug("小文件下载：" + id);

            var docDao = await _SqlClient.Queryable<SyncResFileDao>()
                .Where(a => a.id == id)
                .FirstAsync();
            if (docDao == null)
            {
                return Empty;
            }

            var userDao = await _SqlClient.Queryable<UserDao>()
                .Where(a => a.id == docDao.user_id)
                .FirstAsync();
            if (userDao == null)
            {
                return Empty;
            }

            // 1. 定义文件存储的根路径
            var filePath = _EnvConfig.GetDataPath($"/Nas/{userDao.codes}" + docDao.path);

            // 2. 校验文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return Empty;
            }

            // 3. 获取文件的MIME类型
            var contentType = HttpContentType.APPLICATION_OCTET_STREAM;

            // Response.Headers.Append($"Content-Disposition", $"attachment; filename=\"{FileUtils.GetFileName(filePath)}\"");

            // 4. 返回文件流（第三个参数是下载时显示的文件名）
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }
    }
}
