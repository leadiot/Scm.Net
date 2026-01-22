using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Http;
using Com.Scm.Nas.Sync;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Api.Controllers
{
    /// <summary>
    /// 文件下载服务
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "Scm")]
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
        /// 下载服务器上的物理文件
        /// </summary>
        /// <param name="path">要下载的文件名（含扩展名）</param>
        /// <returns>文件流</returns>
        [HttpGet("File/{id}")]
        [HttpGet("Small/{id}")]
        [NoJsonResult]
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

            // 1. 定义文件存储的根路径
            var filePath = _EnvConfig.GetDataPath("/Nas" + docDao.path);

            // 2. 校验文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return Empty;
            }

            // 3. 获取文件的MIME类型
            var ext = FileUtils.GetExtension(filePath);
            var contentType = MimeUtils.GetMimeType(ext);

            // 4. 返回文件流（第三个参数是下载时显示的文件名）
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }

        [HttpGet("Large")]
        [NoJsonResult]
        public async Task<IActionResult> DownloadLargeFile(string path)
        {
            LogUtils.Debug("大文件下载：" + path);

            // 1. 定义文件存储的根路径
            var filePath = _EnvConfig.GetUploadPath(path);

            // 2. 校验文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("文件不存在，请检查文件名是否正确");
            }

            // 3. 获取文件的MIME类型
            var contentType = HttpContentType.APPLICATION_OCTET_STREAM;

            var fileInfo = new FileInfo(filePath);
            var fileLength = fileInfo.Length;
            var rangeHeader = Request.Headers.Range.ToString();

            // 处理断点续传
            if (!string.IsNullOrEmpty(rangeHeader))
            {
                var range = rangeHeader.Replace("bytes=", "").Split('-');
                var start = long.Parse(range[0]);
                var end = range[1] == "" ? fileLength - 1 : long.Parse(range[1]);
                var length = end - start + 1;

                Response.StatusCode = StatusCodes.Status206PartialContent;
                Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{fileLength}");
                Response.Headers.Append("Content-Length", length.ToString());

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    stream.Seek(start, SeekOrigin.Begin);
                    var buffer = new byte[length];
                    await stream.ReadExactlyAsync(buffer, 0, (int)length);
                    return File(buffer, contentType, Path.GetFileName(filePath));
                }
            }

            // 正常下载
            Response.Headers.Append("Content-Length", fileLength.ToString());
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }
    }
}
