using Com.Scm.Dto;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Http;
using Com.Scm.Request;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MimeKit;
using System.Security.Cryptography;

namespace Com.Scm.Controllers
{
    /// <summary>
    /// 文件信息服务
    /// 基于整个电脑的文件服务，可以根据需要保留或删除
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "scm")]
    public class ComputerController : ApiController
    {
        public ComputerController()
        {
        }

        #region 文件列表
        /// <summary>
        /// 列出当前计算机的所有逻辑根目录（磁盘分区）
        /// </summary>
        /// <returns></returns>
        [HttpGet("listRoot")]
        public ScmPageResultDto<FileDvo> GetListRootAsync()
        {
            var dvo = new ScmPageResultDto<FileDvo>();

            var fileList = new List<FileDvo>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                var item = new FileDvo
                {
                    name = drive.Name,
                    path = drive.RootDirectory.FullName,
                    type = Enums.ScmFileTypeEnum.Dir,
                    icon = "drive"
                };

                if (drive.IsReady)
                {
                    item.size = drive.TotalSize;
                    item.modify_time = TimeUtils.GetUnixTime(drive.RootDirectory.LastWriteTimeUtc);
                }

                fileList.Add(item);
            }
            dvo.Items = fileList;

            return dvo;
        }

        /// <summary>
        /// 列出用户主目录下的文档
        /// </summary>
        /// <returns></returns>
        [HttpGet("listHome")]
        public ScmPageResultDto<FileDvo> GetListHomeAsync(FileSearchRequest request)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return DoListAll(path, request.key, request.page, request.limit);
        }

        /// <summary>
        /// 获取指定目录下的文件和目录列表（分页）
        /// </summary>
        /// <param name="path">目标目录路径</param>
        /// <param name="key">搜索关键词</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <returns></returns>
        [HttpGet("list")]
        public ScmPageResultDto<FileDvo> GetListAsync(string path, string key, int page, int limit)
        {
            var list = new ScmPageResultDto<FileDvo>();
            if (string.IsNullOrWhiteSpace(path))
            {
                return list;
            }

            path = GetNativePath(path);

            return DoListAll(path, key, page, limit);
        }

        /// <summary>
        /// 获取指定目录的子目录列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="key">搜索关键词</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <returns></returns>
        [HttpGet("listDir")]
        public ScmPageResultDto<FileDvo> GetListDirAsync(FileSearchRequest request)
        {
            var path = request.path;
            var list = new ScmPageResultDto<FileDvo>();
            if (string.IsNullOrWhiteSpace(path))
            {
                return list;
            }

            path = GetNativePath(path);

            return DoListDir(path, request.key, request.page, request.limit);
        }

        /// <summary>
        /// 获取指定目录的文档列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="key">搜索关键词</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <returns></returns>
        [HttpGet("listDoc")]
        public ScmPageResultDto<FileDvo> GetListDocAsync(FileSearchRequest request)
        {
            var path = request.path;
            var list = new ScmPageResultDto<FileDvo>();
            if (string.IsNullOrWhiteSpace(path))
            {
                return list;
            }

            path = GetNativePath(path);

            return DoListDoc(path, request.key, request.page, request.limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("listLib")]
        public ScmPageResultDto<FileDvo> GetListLibAsync()
        {
            var dvo = new ScmPageResultDto<FileDvo>();

            var fileList = new List<FileDvo>();
            fileList.Add(new FileDvo { name = "文档", path = "~documents" });
            fileList.Add(new FileDvo { name = "照片", path = "~pictures" });
            fileList.Add(new FileDvo { name = "音乐", path = "~music" });
            fileList.Add(new FileDvo { name = "视频", path = "~videos" });
            fileList.Add(new FileDvo { name = "下载", path = "~downloads" });
            dvo.Items = fileList;

            return dvo;
        }
        #endregion

        #region 文件详情
        /// <summary>
        /// 获取文件或目录的详细信息
        /// </summary>
        /// <param name="path">文件或目录路径</param>
        /// <returns></returns>
        [HttpGet("info")]
        public FileDvo GetInfoAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Error("路径不能为空！");
            }

            path = GetNativePath(path);
            if (Directory.Exists(path))
            {
                var dir = new DirectoryInfo(path);
                var dvo = FromDir(dir);
                dvo.size = CalcDirSize(dir);
                return dvo;
            }

            if (System.IO.File.Exists(path))
            {
                var doc = new FileInfo(path);
                var dvo = FromDoc(doc);
                dvo.hash = CalcFileHash(path);
                return dvo;
            }

            Error($"路径不存在：{path}");
            return null;
        }
        #endregion

        #region 文件查看
        /// <summary>
        /// 文件预览（自动根据文件类型选择最佳响应方式）
        /// 文本/代码文件：小文件直接返回UTF-8内容，大文件流式输出
        /// 音视频/图片等：流式输出 + HTTP Range 支持（边下边播、进度拖拽）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NoJsonResult]
        [HttpGet("view")]
        public async Task<IActionResult> ViewFileAsync(string path)
        {
            LogUtils.Debug("文件预览：" + path);

            if (string.IsNullOrEmpty(path))
            {
                return Empty;
            }

            var filePath = GetNativePath(path);
            var info = new FileInfo(filePath);
            if (!info.Exists)
            {
                throw new FileNotFoundException("文件不存在！");
            }

            // 其他所有文件（音视频、图片、大文本等）：流式输出 + Range 支持
            var contentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = info.Name,
                FileNameStar = info.Name
            };
            Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

            var contentType = MimeTypes.GetMimeType(filePath);
            return PhysicalFile(filePath, contentType, enableRangeProcessing: true);
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 复制文件或目录
        /// </summary>
        /// <param name="files">源文件或目录路径列表</param>
        /// <param name="path">目标路径</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <returns>目标文件信息</returns>
        [HttpPost("Copy")]
        public bool CopyAsync(FileTransferRequest request)
        {
            if (request == null)
            {
                Error("请求不能为空！");
            }

            var files = request.files;
            var path = request.path;
            var overwrite = request.overwrite;
            if (files == null || files.Count < 1 || string.IsNullOrWhiteSpace(path))
            {
                Error("源路径和目标路径不能为空！");
            }

            foreach (var src in files)
            {
                if (Directory.Exists(src))
                {
                    var dst = ResolveDestPath(src, path);
                    EnsureNotNested(src, dst);
                    FileUtils.CopyDir(src, dst, overwrite);
                    continue;
                }

                if (System.IO.File.Exists(src))
                {
                    var dst = ResolveDestPath(src, path);
                    EnsureParentExists(dst);
                    System.IO.File.Copy(src, dst, overwrite);
                    continue;
                }

                Error($"源路径不存在：{src}");
            }

            return true;
        }

        /// <summary>
        /// 移动文件或目录
        /// </summary>
        /// <param name="files">源文件或目录路径列表</param>
        /// <param name="path">目标路径</param>
        /// <param name="overwrite">是否覆盖</param>
        /// <returns>目标文件信息</returns>
        [HttpPost("Move")]
        public bool MoveAsync(FileTransferRequest request)
        {
            if (request == null)
            {
                Error("请求不能为空！");
            }

            var files = request.files;
            var path = request.path;
            var overwrite = request.overwrite;
            if (files == null || files.Count < 1 || string.IsNullOrWhiteSpace(path))
            {
                Error("源路径和目标路径不能为空！");
            }

            foreach (var src in files)
            {
                if (Directory.Exists(src))
                {
                    var dst = ResolveDestPath(src, path);
                    EnsureNotNested(src, dst);
                    Directory.Move(src, dst);
                    continue;
                }

                if (System.IO.File.Exists(src))
                {
                    var dst = ResolveDestPath(src, path);
                    EnsureParentExists(dst);
                    System.IO.File.Move(src, dst, overwrite);
                    continue;
                }

                Error($"源路径不存在：{src}");
            }

            return true;
        }

        /// <summary>
        /// 重命名文件或目录
        /// </summary>
        /// <param name="file">来源文件路径</param>
        /// <param name="name">新名称</param>
        /// <returns>重命名后的文件信息</returns>
        [HttpPost("Rename")]
        public FileDvo RenameAsync(FileRenameRequest request)
        {
            if (request == null)
            {
                Error("请求不能为空！");
            }

            var file = request.file;
            var name = request.name;

            if (string.IsNullOrWhiteSpace(file) || string.IsNullOrWhiteSpace(name))
            {
                Error("源路径和新名称不能为空！");
            }

            var root = Path.GetDirectoryName(file);
            var dst = Path.Combine(root ?? string.Empty, name);

            if (Directory.Exists(file))
            {
                Directory.Move(file, dst);
                return GetInfoAsync(dst);
            }

            if (System.IO.File.Exists(file))
            {
                System.IO.File.Move(file, dst);
                return GetInfoAsync(dst);
            }

            Error($"源路径不存在：{file}");
            return null;
        }

        /// <summary>
        /// 删除文件或目录（目录将递归删除）
        /// </summary>
        /// <param name="files">待删除的文件或目录路径列表</param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public bool DeleteAsync(FileDeleteRequest request)
        {
            if (request == null)
            {
                Error("请求不能为空！");
            }

            var files = request.files;
            if (files == null || files.Count < 1)
            {
                Error("路径不能为空！");
            }

            foreach (var path in files)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return true;
                }

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }

                Error($"路径不存在：{path}");
            }

            return false;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <param name="name">目录名称</param>
        /// <returns></returns>
        [HttpPost("CreateDir")]
        public FileDvo CreateDirAsync(FileCreateRequest request)
        {
            var path = GetNativePath(request.path);
            if (string.IsNullOrWhiteSpace(path))
            {
                Error("路径不能为空！");
            }

            if (!Directory.Exists(path))
            {
                Error("路径不存在：" + path);
            }

            var name = request.name;
            if (string.IsNullOrWhiteSpace(name))
            {
                Error("名称不能为空！");
            }

            var dir = Path.Combine(path, name);
            if (Directory.Exists(dir))
            {
                Error("目录已存在！");
            }

            var info = Directory.CreateDirectory(dir);
            return FromDir(info);
        }
        #endregion

        #region 上传下载
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public async Task<ScmUploadResponse> UploadFileAsync(IFormFile file, [FromForm] string path)
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

            var name = file.FileName;
            if (string.IsNullOrWhiteSpace(name))
            {
                LogUtils.Debug("无效的文件名称！");
                response.SetFailure("无效的文件名称！");
                return response;
            }

            var dstPath = GetNativePath(path);
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

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [NoJsonResult]
        [HttpGet("download")]
        public async Task<IActionResult> DownloadFileAsync(string path)
        {
            LogUtils.Debug("文件下载：" + path);

            if (string.IsNullOrWhiteSpace(path))
            {
                return NotFound();
            }

            var filePath = GetNativePath(path);

            // 校验文件是否存在
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // 获取文件的MIME类型
            var contentType = HttpContentType.APPLICATION_OCTET_STREAM;

            // Response.Headers.Append($"Content-Disposition", $"attachment; filename=\"{FileUtils.GetFileName(filePath)}\"");

            // 返回文件流（第三个参数是下载时显示的文件名）
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath), enableRangeProcessing: true);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 列举目录下所有子目录与文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private ScmPageResultDto<FileDvo> DoListAll(string path, string key, int page, int limit)
        {
            var result = new ScmPageResultDto<FileDvo>();
            if (string.IsNullOrEmpty(path))
            {
                return result;
            }

            var info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                return result;
            }

            page -= 1;
            if (page < 0)
            {
                page = 0;
            }

            var from = page * limit;
            var to = from + limit;

            var list = new List<FileDvo>();

            var dirList = SafeGetDirectories(info, key);
            var dirQty = dirList.Length;
            var end = dirQty > to ? to : dirQty;
            while (from < end)
            {
                list.Add(FromDir(dirList[from]));
                from += 1;
            }

            var docList = SafeGetFiles(info, key);
            var docQty = docList.Length;
            if (from < to)
            {
                from -= dirQty;
                to -= dirQty;
                end = docQty > to ? to : docQty;

                while (from < end)
                {
                    list.Add(FromDoc(docList[from]));
                    from += 1;
                }
            }

            var qty = dirQty + docQty;
            result.TotalItems = qty;
            result.TotalPages = (qty - 1 + limit) / limit;
            result.Items = list;

            return result;
        }

        /// <summary>
        /// 列出目录下所有子目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private ScmPageResultDto<FileDvo> DoListDir(string path, string key, int page, int limit)
        {
            var result = new ScmPageResultDto<FileDvo>();
            if (string.IsNullOrEmpty(path))
            {
                return result;
            }

            var info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                return result;
            }

            page -= 1;
            if (page < 0)
            {
                page = 0;
            }

            var from = page * limit;
            var to = from + limit;

            var list = new List<FileDvo>();

            var dirList = SafeGetDirectories(info, key);
            var dirQty = dirList.Length;
            var end = dirQty > to ? to : dirQty;
            while (from < end)
            {
                list.Add(FromDir(dirList[from]));
                from += 1;
            }

            var qty = dirQty;
            result.TotalItems = qty;
            result.TotalPages = (qty - 1 + limit) / limit;
            result.Items = list;

            return result;
        }

        /// <summary>
        /// 列出目录下所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        private ScmPageResultDto<FileDvo> DoListDoc(string path, string key, int page, int limit)
        {
            var result = new ScmPageResultDto<FileDvo>();
            if (string.IsNullOrEmpty(path))
            {
                return result;
            }

            var info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                return result;
            }

            page -= 1;
            if (page < 0)
            {
                page = 0;
            }

            var from = page * limit;
            var to = from + limit;

            var list = new List<FileDvo>();

            var docList = SafeGetFiles(info, key);
            var docQty = docList.Length;
            var end = docQty > to ? to : docQty;
            while (from < end)
            {
                list.Add(FromDoc(docList[from]));
                from += 1;
            }

            var qty = docQty;
            result.TotalItems = qty;
            result.TotalPages = (qty - 1 + limit) / limit;
            result.Items = list;

            return result;
        }

        /// <summary>
        /// 安全获取目录列表
        /// </summary>
        /// <param name="info"></param>
        /// <param name="key"></param>
        /// <param name="hasHidden"></param>
        /// <returns></returns>
        private DirectoryInfo[] SafeGetDirectories(DirectoryInfo info, string key, bool hasHidden = false)
        {
            try
            {
                var list = string.IsNullOrWhiteSpace(key) ? info.GetDirectories() : info.GetDirectories(key);
                if (!hasHidden)
                {
                    list = list.Where(a => !a.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
                }
                return list;
            }
            catch
            {
                return Array.Empty<DirectoryInfo>();
            }
        }

        /// <summary>
        /// 安全获取文件列表
        /// </summary>
        /// <param name="info"></param>
        /// <param name="key"></param>
        /// <param name="hasHidden"></param>
        /// <returns></returns>
        private FileInfo[] SafeGetFiles(DirectoryInfo info, string key, bool hasHidden = false)
        {
            try
            {
                var list = string.IsNullOrWhiteSpace(key) ? info.GetFiles() : info.GetFiles(key);
                if (!hasHidden)
                {
                    list = list.Where(a => !a.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
                }
                return list;
            }
            catch
            {
                return Array.Empty<FileInfo>();
            }
        }

        /// <summary>
        /// 目录信息转换为视图对象
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private FileDvo FromDir(DirectoryInfo dir)
        {
            return new FileDvo
            {
                name = dir.Name,
                path = dir.FullName,
                type = Enums.ScmFileTypeEnum.Dir,
                icon = "folder",
                create_time = TimeUtils.GetUnixTime(dir.CreationTimeUtc),
                modify_time = TimeUtils.GetUnixTime(dir.LastWriteTimeUtc)
            };
        }

        /// <summary>
        /// 文件信息转换为视图对象
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private FileDvo FromDoc(FileInfo file)
        {
            return new FileDvo
            {
                name = file.Name,
                path = file.FullName,
                type = Enums.ScmFileTypeEnum.Doc,
                icon = string.IsNullOrWhiteSpace(file.Extension) ? "file" : file.Extension.TrimStart('.').ToLower(),
                size = file.Length,
                create_time = TimeUtils.GetUnixTime(file.CreationTimeUtc),
                modify_time = TimeUtils.GetUnixTime(file.LastWriteTimeUtc)
            };
        }

        /// <summary>
        /// 解析目标路径：若目标为已存在的目录，则拼接源名称
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        private string ResolveDestPath(string src, string dst)
        {
            if (Directory.Exists(dst))
            {
                var name = Path.GetFileName(src.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                return Path.Combine(dst, name);
            }

            return dst;
        }

        /// <summary>
        /// 确保目标的父目录存在
        /// </summary>
        /// <param name="dst"></param>
        private void EnsureParentExists(string dst)
        {
            var parent = Path.GetDirectoryName(dst);
            if (!string.IsNullOrWhiteSpace(parent) && !Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }
        }

        /// <summary>
        /// 防止把目录移动/复制到自身或其子目录
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        private void EnsureNotNested(string src, string dst)
        {
            var srcFull = Path.GetFullPath(src).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var dstFull = Path.GetFullPath(dst).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (string.Equals(srcFull, dstFull, StringComparison.OrdinalIgnoreCase))
            {
                Error("目标路径与源路径相同！");
            }

            if (dstFull.StartsWith(srcFull + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            {
                Error("不能将目录移动或复制到其自身或其子目录内！");
            }
        }

        /// <summary>
        /// 递归计算目录大小
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private long CalcDirSize(DirectoryInfo dir)
        {
            long size = 0;

            foreach (var file in SafeGetFiles(dir, null))
            {
                try
                {
                    size += file.Length;
                }
                catch
                {
                    // 忽略无法访问的文件
                }
            }

            foreach (var sub in SafeGetDirectories(dir, null))
            {
                size += CalcDirSize(sub);
            }

            return size;
        }

        /// <summary>
        /// 计算文件MD5摘要
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string CalcFileHash(string path)
        {
            try
            {
                using var stream = System.IO.File.OpenRead(path);
                var bytes = MD5.HashData(stream);
                return Convert.ToHexString(bytes).ToLower();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 将路径中的特殊标记转换为本地系统路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetNativePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            path = path.Trim();

            if (path[0] != '~')
            {
                return path;
            }

            var tag = path.Split('/')[0];
            var pre = "";
            switch (tag.ToLower())
            {
                case "~desk":
                case "~desktop":
                    pre = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    break;
                case "~home":
                case "~personal":
                    pre = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    break;
                case "~document":
                case "~documents":
                    pre = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    break;
                case "~image":
                case "~images":
                case "~pictures":
                    pre = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    break;
                case "~audio":
                case "~audios":
                case "~music":
                    pre = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                    break;
                case "~video":
                case "~videos":
                    pre = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                    break;
                case "~download":
                case "~downloads":
                    pre = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    break;
                default:
                    return path;
            }

            return path.Replace(tag, pre);
        }
    }

    public class FileCreateRequest : ScmRequest
    {
        public string path { get; set; }

        public string name { get; set; }
    }

    /// <summary>
    /// 文件删除请求
    /// </summary>
    public class FileDeleteRequest : ScmRequest
    {
        /// <summary>
        /// 要删除的文件或目录路径
        /// </summary>
        public List<string> files { get; set; }
    }

    /// <summary>
    /// 文件重命名请求
    /// </summary>
    public class FileRenameRequest : ScmRequest
    {
        /// <summary>
        /// 源文件或目录路径
        /// </summary>
        public string file { get; set; }

        /// <summary>
        /// 新的名称（不含路径）
        /// </summary>
        public string name { get; set; }
    }

    public class FileSearchRequest : ScmSearchPageRequest
    {
        public string path { get; set; }
    }

    /// <summary>
    /// 文件复制/移动请求
    /// </summary>
    public class FileTransferRequest : ScmRequest
    {
        /// <summary>
        /// 源文件或目录路径
        /// </summary>
        public List<string> files { get; set; }

        /// <summary>
        /// 目标目录路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 是否覆盖已存在的目标
        /// </summary>
        public bool overwrite { get; set; }
    }

    public class FileDvo : ScmDvo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 完整路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 文件图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 是否是目录
        /// </summary>
        public ScmFileTypeEnum type { get; set; }

        #region 文件属性
        /// <summary>
        /// 文件摘要（MD5）
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long size { get; set; }
        #endregion
    }
}
