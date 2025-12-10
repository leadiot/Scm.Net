using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Image.SkiaSharp;
using Com.Scm.Sys.SysSafety;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Scm")]
    public class UploadController : ApiController
    {
        private EnvConfig _Config;
        private ScmSysSafetyService _SafetyService;

        public UploadController(EnvConfig config, ScmSysSafetyService safetyService)
        {
            _Config = config;
            _SafetyService = safetyService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ScmUploadResponse> UploadAsync(ScmUploadRequest request)
        {
            if (request.type == UploadTypeEnum.ByFile)
            {
                return await ByFileAsync(request);
            }

            if (request.type == UploadTypeEnum.ByPart)
            {
                return await ByPartAsync(request);
            }

            if (request.type == UploadTypeEnum.ByHash)
            {
                return await ByHashAsync(request);
            }

            var response = new ScmUploadResponse();
            response.SetFailure("未知的上传类型！");
            return response;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("byfile")]
        public async Task<ScmUploadResponse> ByFileAsync(ScmUploadRequest request)
        {
            var response = new ScmUploadResponse();

            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                response.SetFailure("请选择文件！");
                return response;
            }

            var qty = 0;
            foreach (var file in files)
            {
                var name = file.Name;

                var exts = Path.GetExtension(file.FileName).ToLower();
                if (!IsAcceptExts(exts))
                {
                    response.SetFailure("不支持的文件类型！");
                    return response;
                }

                var dstFile = "";
                using (var stream = System.IO.File.OpenWrite(dstFile))
                {
                    await file.CopyToAsync(stream);
                }
                qty += 1;
            }

            response.SetSuccess(qty, $"成功上传 {qty} 个文件！");
            return response;
        }

        /// <summary>
        /// 分段上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("bypart")]
        public async Task<ScmUploadResponse> ByPartAsync(ScmUploadRequest request)
        {
            var response = new ScmUploadResponse();

            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                response.SetFailure("请选择文件！");
                return response;
            }

            var qty = 0;
            foreach (var file in files)
            {
                var name = file.Name;

                var exts = Path.GetExtension(file.FileName).ToLower();
                if (!IsAcceptExts(exts))
                {
                    response.SetFailure("不支持的文件类型！");
                    return response;
                }

                var dstFile = "";
                using (var stream = System.IO.File.OpenWrite(dstFile))
                {
                    await file.CopyToAsync(stream);
                }
                qty += 1;
            }

            response.SetSuccess(qty, $"成功上传 {qty} 个文件！");
            return response;
        }

        /// <summary>
        /// 摘要上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("byhash")]
        public async Task<ScmUploadResponse> ByHashAsync(ScmUploadRequest request)
        {
            var response = new ScmUploadResponse();

            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                response.SetFailure("请选择文件！");
                return response;
            }

            var qty = 0;
            foreach (var file in files)
            {
                var name = file.Name;

                var exts = Path.GetExtension(file.FileName).ToLower();
                if (!IsAcceptExts(exts))
                {
                    response.SetFailure("不支持的文件类型！");
                    return response;
                }

                var dstFile = "";
                using (var stream = System.IO.File.OpenWrite(dstFile))
                {
                    await file.CopyToAsync(stream);
                }
                qty += 1;
            }

            response.SetSuccess(qty, $"成功上传 {qty} 个文件！");
            return response;
        }

        private bool IsAcceptExts(string exts)
        {
            var safety = _SafetyService.Get();
            if (string.IsNullOrWhiteSpace(safety.UploadWhite))
            {
                return true;
            }

            var arr = safety.UploadWhite.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return arr.Contains(exts);
        }

        [HttpGet("avatar/{file}"), AllowAnonymous, NoJsonResult, NoAuditLog]
        public async Task<IActionResult> AvatarAsync(string file)
        {
            var path = _Config.GetAvatarPath(file);
            if (!System.IO.File.Exists(path))
            {
                var result = new ImageEngine().GenAvatar();
                return File(result.Image, "image/png");
            }

            using (var stream = System.IO.File.OpenRead(path))
            {
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, bytes.Length);
                return File(bytes, "image/png");
            }
        }
    }
}
