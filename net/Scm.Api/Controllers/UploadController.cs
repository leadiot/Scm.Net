using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Com.Scm.Api.Controllers
{
    /// <summary>
    /// 文件上传服务
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "Scm")]
    public class UploadController : ApiController
    {
        private EnvConfig _EnvConfig;

        public UploadController(EnvConfig envConfig)
        {
            _EnvConfig = envConfig;
        }

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

            if (file.Length > 1024 * 1024 * 5)
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
    }
}
