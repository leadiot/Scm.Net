using Com.Scm.Config;
using Com.Scm.Filters;
using Com.Scm.Image.SkiaSharp;
using Com.Scm.Sys.Files.Dvo;
using Com.Scm.Sys.SysSafety;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Files;

/// <summary>
/// 
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysFileService : IApiService
{
    private readonly EnvConfig _envConfig;
    private readonly ScmContextHolder _contextHolder;
    private readonly ScmSysSafetyService _safetyService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="envConfig"></param>
    /// <param name="contextHolder"></param>
    /// <param name="safetyService"></param>
    /// <param name="httpContextAccessor"></param>
    public ScmSysFileService(EnvConfig envConfig,
        ScmContextHolder contextHolder,
        ScmSysSafetyService safetyService,
        IHttpContextAccessor httpContextAccessor)
    {
        _envConfig = envConfig;
        _contextHolder = contextHolder;
        _safetyService = safetyService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// 查询目录-树形结构
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public List<ScmDirInfo> GetFolders(ListFileRequest request)
    {
        var token = _contextHolder.GetToken();

        var root = new ScmDirInfo() { Name = "根目录", Uri = "/" };

        var basePath = _envConfig.GetDataPath(request.path);
        root.Children = ScmUtils.GetFolders(basePath);

        return new List<ScmDirInfo> { root };
    }

    /// <summary>
    /// 根据目录查询文件
    /// </summary>
    /// <returns></returns>
    public List<ScmDocInfo> GetFiles(ListFileRequest request)
    {
        var basePath = _envConfig.GetDataPath(request.path);

        return ScmUtils.GetFiles(basePath, request.kind, _envConfig.DataDir);
    }

    #region 文件上传
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <returns></returns>
    [HttpPost]
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
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public async Task<ScmUploadResult> UploadFile(IFormFile file, string path)
    {
        var result = new ScmUploadResult();

        var dstPath = _envConfig.GetDataPath(path);
        FileUtils.CreateDir(dstPath);

        var name = file.Name;

        var exts = Path.GetExtension(file.FileName).ToLower();
        if (!IsAcceptExts(exts))
        {
            result.name = name;
            result.message = "不支持的文件扩展名：" + exts;
            return result;
        }

        var dstFile = Path.Combine(dstPath, DateTime.Now.ToFileTimeUtc() + exts);
        using (var stream = System.IO.File.OpenWrite(dstFile))
        {
            await file.CopyToAsync(stream);
        }
        result.name = name;
        result.path = _envConfig.ToUri(dstFile);
        result.success = true;

        return result;
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
                response.AddResult(new ScmUploadResult { name = name, message = "不支持的文件扩展名：" + exts });
                continue;
            }

            var dstFile = Path.Combine(dstPath, (now++) + exts);
            using (var stream = System.IO.File.OpenWrite(dstFile))
            {
                await file.CopyToAsync(stream);
            }
            response.AddResult(new ScmUploadResult { name = name, path = _envConfig.ToUri(dstFile), success = true });

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
            using (var stream = System.IO.File.OpenWrite(dstFile))
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
    public void DeleteFile(string path)
    {
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
    public void DeleteFolder(string path)
    {
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
        if (!System.IO.File.Exists(path))
        {
            var result = new ImageEngine().GenAvatar();
            return new FileContentResult(result.Image, "image/png");
        }

        var exts = Path.GetExtension(path);
        using (var stream = System.IO.File.OpenRead(path))
        {
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            return new FileContentResult(bytes, FileUtils.GetMimeByExt(exts));
        }
    }

    /// <summary>
    /// 用户头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpGet("{file}"), AllowAnonymous, NoJsonResult, NoAuditLog]
    public async Task<IActionResult> AvatarAsync(string file)
    {
        var path = _envConfig.GetAvatarPath(file);
        if (!System.IO.File.Exists(path))
        {
            var result = new ImageEngine().GenAvatar();
            return new FileContentResult(result.Image, "image/png");
        }

        using (var stream = System.IO.File.OpenRead(path))
        {
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, bytes.Length);
            return new FileContentResult(bytes, "image/png");
        }
    }
}