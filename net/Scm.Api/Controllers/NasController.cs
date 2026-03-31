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
using Microsoft.Net.Http.Headers;
using MimeKit;
using SqlSugar;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Scm.Api.Controllers
{
    /// <summary>
    /// 文件下载服务
    /// /Api/Nas/ds/id，小文件下载
    /// /Api/Nas/dl/id，大文件下载
    /// /Api/Nas/vs/id，文件查看
    /// /Api/Nas/info，获取文件信息
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

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("info/{id}")]
        public async Task<ScmDownloadResponse> FileInfo(long id)
        {
            var response = new ScmDownloadResponse();

            LogUtils.Debug("获取文件信息：" + id);

            var docDao = await _SqlClient.Queryable<SyncResFileDao>()
                .Where(a => a.id == id)
                .FirstAsync();
            if (docDao == null)
            {
                response.SetFailure("文件不存在！");
                return response;
            }

            var userDao = await _SqlClient.Queryable<UserDao>()
                .Where(a => a.id == docDao.user_id)
                .FirstAsync();
            if (userDao == null)
            {
                response.SetFailure("文件不存在！");
                return response;
            }

            // 1. 定义文件存储的根路径
            var filePath = _EnvConfig.GetDataPath($"/Nas/{userDao.codes}" + docDao.path);

            // 2. 校验文件是否存在
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                response.SetFailure("文件不存在！");
                return response;
            }

            response.name = docDao.name;
            response.size = fileInfo.Length;
            response.hash = docDao.hash;

            response.SetSuccess();
            return response;
        }

        #region 文件查看
        /// <summary>
        /// 查看文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NoJsonResult]
        [HttpGet("vs/{id}")]
        public async Task<IActionResult> ViewSmallFile(long id)
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

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length >= NasEnv.MAX_CHUNK_SIZE)
            {
                return Content("文件过大，无法预览！");
            }

            if (docDao.kind == Enums.ScmFileKindEnum.Text || docDao.kind == Enums.ScmFileKindEnum.Code)
            {
                // 3. 关键：设置响应头（编码+不下载+预览）
                Response.Headers.Append("Content-Encoding", Encoding.UTF8.WebName);

                return Content(FileUtils.ReadText(filePath));
            }

            var contentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = docDao.name,
                FileNameStar = docDao.name
            };
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            // 5. 获取文件的MIME类型
            var contentType = MimeTypes.GetMimeType(filePath);

            return PhysicalFile(filePath, contentType);
        }
        #endregion

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
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length >= NasEnv.MAX_CHUNK_SIZE)
            {
                return Content("文件过大，无法下载！");
            }

            // 3. 获取文件的MIME类型
            var contentType = HttpContentType.APPLICATION_OCTET_STREAM;

            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = docDao.name,
                FileNameStar = docDao.name
            };
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            // Response.Headers.Append("Content-Length", fileLength.ToString());
            return PhysicalFile(filePath, contentType, docDao.name);

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
                return Empty;
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
                var end = string.IsNullOrEmpty(range[1]) ? fileLength : long.Parse(range[1]);
                if (end > fileLength)
                {
                    end = fileLength;
                }
                var length = end - start;

                Response.StatusCode = StatusCodes.Status206PartialContent;
                Response.Headers.Append("Content-Length", length.ToString());
                Response.Headers.Append("Content-Range", $"bytes {start}-{end}/{fileLength}");
                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = docDao.name,
                    FileNameStar = docDao.name
                };
                Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
                //Response.Headers.Append("Content-Disposition", $"attachment; filename={docDao.name}");
                Response.Headers.Append("Accept-Ranges", "bytes");

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    stream.Seek(start, SeekOrigin.Begin);
                    var buffer = new byte[length];
                    await stream.ReadExactlyAsync(buffer, 0, (int)length);
                    return File(buffer, contentType, docDao.name);
                }
            }

            // 正常下载
            Response.Headers.Append("Content-Length", fileLength.ToString());
            return PhysicalFile(filePath, contentType, docDao.name);
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
                LogUtils.Debug("文件的内容过大！");
                response.SetFailure("文件的内容过大！");
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

            LogUtils.Debug("文件上传成功：" + name);
            response.SetSuccess($"文件上传成功！");
            return response;
        }
        #endregion

        #region 大文件上传
        /// <summary>
        /// 分块上传，
        /// 上传文件名称必须满足：64位哈希值.分块序号（如：1.chunk）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("chunk")]
        public async Task<ScmUploadResponse> UploadChunkAsync(ScmUploadRequest request)
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
                LogUtils.Debug("文件的内容过大！");
                response.SetFailure("文件的内容过大！");
                return response;
            }

            var hash = request.hash ?? "";
            if (!Regex.IsMatch(hash, @"^\w{64}$"))
            {
                LogUtils.Debug("无效的文件摘要！");
                response.SetFailure("无效的文件摘要！");
                return response;
            }

            var name = (request.file_name ?? file.FileName).ToLower();
            if (!Regex.IsMatch(name, @"^\d+[.]chunk$"))
            {
                LogUtils.Debug("无效的文件名称！");
                response.SetFailure("无效的文件名称！");
                return response;
            }

            var dstPath = _EnvConfig.GetTempPath(hash);
            FileUtils.CreateDir(dstPath);
            var dstFile = FileUtils.Combine(dstPath, name);
            using (var stream = System.IO.File.OpenWrite(dstFile))
            {
                await file.CopyToAsync(stream);
            }

            LogUtils.Debug("文件上传成功：" + name);
            response.SetSuccess($"文件上传成功！");
            return response;
        }

        /// <summary>
        /// 上传校验
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("check/{hash}")]
        public async Task<ScmUploadResponse> UploadCheckAsync(string hash)
        {
            var response = new ScmUploadResponse();

            if (hash == null || !Regex.IsMatch(hash, @"^\w{64}$"))
            {
                LogUtils.Debug("无效的文件摘要！");
                response.SetFailure("无效的文件摘要！");
                return response;
            }

            var list = new List<ScmUploadResult>();

            var dstPath = _EnvConfig.GetTempPath(hash);
            if (FileUtils.ExistsDir(dstPath))
            {
                var files = Directory.GetFiles(dstPath, "*.chunk").OrderBy(a => a).ToList();
                foreach (var file in files)
                {
                    list.Add(new ScmUploadResult
                    {
                        name = FileUtils.GetFileName(file)
                    });
                }
            }

            response.results = list;
            response.SetSuccess();

            return response;
        }

        /// <summary>
        /// 文件合并
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("merge/{hash}")]
        public async Task<ScmUploadResponse> UploadMergeAsync(string hash)
        {
            var response = new ScmUploadResponse();

            if (hash == null || !Regex.IsMatch(hash, @"^\w{64}$"))
            {
                LogUtils.Debug("无效的文件摘要！");
                response.SetFailure("无效的文件摘要！");
                return response;
            }

            var dstPath = _EnvConfig.GetTempPath(hash);
            if (!FileUtils.ExistsDir(dstPath))
            {
                LogUtils.Debug("分块目录不存在：" + hash);
                response.SetSuccess($"分块目录不存在！");
                return response;
            }

            var name = hash + ".nas";

            var files = Directory.GetFiles(dstPath, "*.chunk").OrderBy(a => a).ToList();
            var dstFile = _EnvConfig.GetTempPath(name);
            using (var stream = System.IO.File.OpenWrite(dstFile))
            {
                foreach (var file in files)
                {
                    using (var srcStream = System.IO.File.OpenRead(file))
                    {
                        await srcStream.CopyToAsync(stream);
                    }
                }
            }

            // 删除分块文件
            FileUtils.DeleteDir(dstPath, true);

            LogUtils.Debug("文件合并完成：" + name);
            response.SetSuccess($"文件合并完成！");
            return response;
        }
        #endregion
        #endregion
    }
}
