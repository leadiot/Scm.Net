using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Http;
using Com.Scm.Nas;
using Com.Scm.Nas.Sync;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SqlSugar;
using System.Text.RegularExpressions;

namespace Com.Scm.Api.Controllers
{
    /// <summary>
    /// 文件下载服务
    /// /Api/Nas/ds/i，小文件下载
    /// /Api/Nas/dl/id，大文件下载
    /// /Api/Nas/vs/id，文件查看
    /// /Api/Nas/file，小文件上传
    /// /Api/Nas/chunk，分块上传
    /// /Api/Nas/check，上传校验
    /// /Api/Nas/merge，
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "Scm")]
    public class NasController : ApiController
    {
        private EnvConfig _EnvConfig;
        private ISqlSugarClient _SqlClient;

        public NasController(EnvConfig envConfig, ISqlSugarClient sqlClient)
        {
            _EnvConfig = envConfig;
            _SqlClient = sqlClient;
        }

        #region 文件下载
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

        /// <summary>
        /// 大文件下载，支持断点续传
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NoJsonResult]
        [HttpGet("dl/{id}")]
        public async Task<IActionResult> DownloadLargeFile(long id)
        {
            LogUtils.Debug("大文件下载：" + id);

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
        #endregion

        #region 文件上传
        #region 小文件上传
        [HttpPost("file")]
        public async Task<ScmUploadResponse> UploadFileAsync(ScmUploadRequest request)
        {
            var response = new ScmUploadResponse();

            var file = request.file;
            if (file == null)
            {
                LogUtils.Debug("上传文件为空！");
                response.SetFailure("上传文件为空！");
                return response;
            }

            if (file.Length > NasEnv.MAX_CHUNK_SIZE)
            {
                LogUtils.Debug("无效的内容过大！");
                response.SetFailure("无效的内容过大！");
                return response;
            }

            var name = (request.file_name ?? file.FileName).ToLower();
            if (!Regex.IsMatch(name, @"^\w{64}[.]nas$"))
            {
                LogUtils.Debug("无效的文件名称！");
                response.SetFailure("无效的文件名称！");
                return response;
            }

            //var exts = Path.GetExtension(file.FileName).ToLower();
            //if (!IsAcceptExts(exts))
            //{
            //    response.SetFailure("不支持的文件类型！");
            //    return response;
            //}

            var dstFile = _EnvConfig.GetTempPath(name);
            using (var stream = System.IO.File.OpenWrite(dstFile))
            {
                await file.CopyToAsync(stream);
            }

            LogUtils.Debug("上传文件成功：" + name);
            response.SetSuccess($"文件上传成功！");
            return response;
        }
        #endregion

        #region 大文件上传
        /// <summary>
        /// 分块上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("chunk")]
        public async Task<ScmUploadResponse> UploadChunkAsync(ScmUploadRequest request)
        {
            return null;
        }

        /// <summary>
        /// 上传校验
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("check")]
        public async Task<ScmUploadResponse> UploadCheckAsync(ScmUploadRequest request)
        {
            return null;
        }

        /// <summary>
        /// 文件合并
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("merge")]
        public async Task<ScmUploadResponse> UploadMergeAsync(ScmUploadRequest request)
        {
            return null;
        }
        #endregion
        #endregion

        #region 文件查看
        /// <summary>
        /// 查看文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NoJsonResult]
        [HttpGet("vs/{id}")]
        public async Task<IActionResult> ViewFile(long id)
        {
            LogUtils.Debug("文件查看：" + id);

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
            var contentType = MimeTypes.GetMimeType(filePath);
            if (string.IsNullOrWhiteSpace(contentType))
            {
                contentType = HttpContentType.APPLICATION_OCTET_STREAM;
            }

            Response.Headers.Append($"Content-Disposition", $"inline; filename=\"{FileUtils.GetFileName(filePath)}\"");

            // 4. 返回文件流（第三个参数是下载时显示的文件名）
            return PhysicalFile(filePath, contentType);
        }
        #endregion
    }
}
