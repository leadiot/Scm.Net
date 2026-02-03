using Com.Scm.Adm.Files.Dvo;
using Com.Scm.Config;
using Com.Scm.Filters;
using Com.Scm.Image.SkiaSharp;
using Com.Scm.Sys.SysSafety;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Adm.Files;

/// <summary>
/// 
/// </summary>
[ApiExplorerSettings(GroupName = "Adm")]
public class ScmAdmFileService : IApiService
{
    private readonly EnvConfig _envConfig;
    private readonly ScmSysSafetyService _safetyService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="envConfig"></param>
    /// <param name="safetyService"></param>
    /// <param name="httpContextAccessor"></param>
    public ScmAdmFileService(EnvConfig envConfig, ScmSysSafetyService safetyService, IHttpContextAccessor httpContextAccessor)
    {
        _envConfig = envConfig;
        _safetyService = safetyService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 查询目录-树形结构
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public List<ScmFolderInfo> GetFolders(ListFileRequest request)
    {
        var basePath = _envConfig.GetDataPath(request.path);

        var root = new ScmFolderInfo() { Name = "根目录", Uri = "/" };
        root.Children = ScmUtils.GetFolders(basePath);

        return new List<ScmFolderInfo> { root };
    }

    /// <summary>
    /// 根据目录查询文件
    /// </summary>
    /// <returns></returns>
    public List<ScmFileInfo> GetFiles(ListFileRequest request)
    {
        var basePath = _envConfig.GetDataPath(request.path);
        return ScmUtils.GetFiles(basePath, request.kind, _envConfig.DataDir);
    }

    #region 文件上传
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <returns></returns>
    public async Task<ScmUploadResponse> Upload([FromForm] ScmUploadRequest request)
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
    /// <returns></returns>
    public async Task<ScmUploadResponse> ByFileAsync([FromForm] ScmUploadRequest request)
    {
        var response = new ScmUploadResponse();

        var files = _httpContextAccessor.HttpContext?.Request.Form.Files;
        if (files.Count == 0)
        {
            response.SetFailure("上传文件不能为空！");
            return response;
        }

        var dstPath = _envConfig.GetDataPath(request.path);
        FileUtils.CreateDir(dstPath);

        var qty = 0;
        var now = DateTime.Now.ToFileTimeUtc();
        foreach (var file in files)
        {
            var name = file.Name;

            var exts = Path.GetExtension(file.FileName).ToLower();
            if (!IsAcceptExts(exts))
            {
                response.AddResult(new ScmUploadResult { file = name, message = "不支持的文件扩展名：" + exts });
                continue;
            }

            var dstFile = Path.Combine(dstPath, now++ + exts);
            using (var stream = File.OpenWrite(dstFile))
            {
                await file.CopyToAsync(stream);
            }
            response.AddResult(new ScmUploadResult { file = name, path = _envConfig.ToUri(dstFile), success = true });

            qty += 1;
        }

        response.SetSuccess();
        return response;
    }

    /// <summary>
    /// 分段上传
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ScmUploadResponse> ByPartAsync(ScmUploadRequest request)
    {
        var response = new ScmUploadResponse();

        var files = _httpContextAccessor.HttpContext?.Request.Form.Files;
        if (files.Count == 0)
        {
            response.SetFailure("上传文件不能为空！");
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
            using (var stream = File.OpenWrite(dstFile))
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
    public async Task<ScmUploadResponse> ByHashAsync(ScmUploadRequest request)
    {
        var response = new ScmUploadResponse();

        var files = _httpContextAccessor.HttpContext?.Request.Form.Files;
        if (files.Count == 0)
        {
            response.SetFailure("上传文件不能为空！");
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
            using (var stream = File.OpenWrite(dstFile))
            {
                await file.CopyToAsync(stream);
            }
            qty += 1;
        }

        response.SetSuccess(qty, $"成功上传 {qty} 个文件！");
        return response;
    }
    #endregion

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <returns></returns>
    public void DeleteFile(DeleteRequest request)
    {
        var path = request.path;
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        var basePath = _envConfig.GetDataPath(path);
        FileUtils.DeleteDoc(basePath);
    }

    /// <summary>
    /// 删除目录及目录下文件
    /// </summary>
    /// <returns></returns>
    public void DeleteFolder(DeleteRequest request)
    {
        var path = request.path;
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        var basePath = _envConfig.GetDataPath(path);
        FileUtils.DeleteDir(basePath);
    }

    private bool IsAcceptExts(string exts)
    {
        var safety = _safetyService.Get();
        if (string.IsNullOrWhiteSpace(safety.UploadWhite))
        {
            return true;
        }

        var arr = safety.UploadWhite.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        return arr.Contains(exts);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous, NoJsonResult, NoAuditLog]
    public async Task<IActionResult> ViewAsync(string file)
    {
        var path = _envConfig.GetDataPath(file);
        if (!File.Exists(path))
        {
            var result = new ImageEngine().GenAvatar();
            return new FileContentResult(result.Image, "image/png");
        }

        var exts = Path.GetExtension(path);
        using (var stream = File.OpenRead(path))
        {
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            return new FileContentResult(bytes, FileUtils.GetMimeByExt(exts));
        }
    }
}