using Com.Scm.Config;
using Com.Scm.Http;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers
{
    /// <summary>
    /// SCM系统文件服务
    /// 基于Scm Data目录的文件上传和下载服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "scm")]
    public class ScmFileController : ApiController
    {
        private EnvConfig _EnvConfig;
        private SecConfig _SecConfig;

        public ScmFileController(EnvConfig envConfig, SecConfig secConfig)
        {
            _EnvConfig = envConfig;
            _SecConfig = secConfig;
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="path">要下载的文件名（含扩展名）</param>
        /// <returns>文件流</returns>
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string path)
        {
            LogUtils.Debug("文件下载：" + path);

            if (string.IsNullOrWhiteSpace(path))
            {
                return NotFound();
            }

            // 校验路径是否为绝对路径，防止越权访问
            if (Path.IsPathRooted(path))
            {
                return NotFound();
            }

            // 1. 定义文件存储的根路径
            var filePath = _EnvConfig.GetDataPath(path);

            // 2. 校验文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // 3. 获取文件的MIME类型
            var contentType = HttpContentType.APPLICATION_OCTET_STREAM;

            // Response.Headers.Append($"Content-Disposition", $"attachment; filename=\"{FileUtils.GetFileName(filePath)}\"");

            // 4. 返回文件流（第三个参数是下载时显示的文件名）
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath), enableRangeProcessing: true);
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file">要上传的文件</param>
        /// <param name="path">上传到的目录</param>
        /// <param name="name">上传后的文件名</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ScmUploadResponse> UploadFileAsync(IFormFile file, string path, string name)
        {
            var response = new ScmUploadResponse();

            if (file == null)
            {
                LogUtils.Debug("上传文件为空！");
                response.SetFailure("上传文件为空！");
                return response;
            }

            if (file.Length > ScmEnv.MAX_FILE_SIZE)
            {
                LogUtils.Debug("无效的内容过大！");
                response.SetFailure("无效的内容过大！");
                return response;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = file.FileName;
                if (string.IsNullOrWhiteSpace(name))
                {
                    LogUtils.Debug("无效的文件名称！");
                    response.SetFailure("无效的文件名称！");
                    return response;
                }
            }

            var exts = Path.GetExtension(file.FileName);
            if (!IsAcceptExts(exts))
            {
                response.SetFailure("不支持的文件类型！");
                return response;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                path = "Upload";
            }

            // 校验路径是否为绝对路径，防止越权访问
            if (Path.IsPathRooted(path))
            {
                response.SetFailure("不支持的文件路径！");
                return response;
            }

            var dstPath = _EnvConfig.GetDataPath(path);
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
            }

            var dstFile = Path.Combine(dstPath, name);
            using (var stream = System.IO.File.OpenWrite(dstFile))
            {
                await file.CopyToAsync(stream);
            }

            LogUtils.Debug("上传文件成功：" + name);
            response.SetSuccess($"文件上传成功！");
            return response;
        }

        private bool IsAcceptExts(string exts)
        {
            if (string.IsNullOrWhiteSpace(exts))
            {
                return false;
            }

            exts = exts.Trim().ToLower();
            var extsList = _SecConfig.UploadWhite.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var ext in extsList)
            {
                if (ext == exts)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
