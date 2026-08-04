using Com.Scm.Computer.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Com.Scm.Computer
{
    /// <summary>
    /// 本机文件服务，提供当前电脑文件的列表、详情、移动、复制、删除等操作
    /// </summary>
    public class ComputerService : ApiService
    {
        public ComputerService()
        {
        }

        #region 文件列表
        /// <summary>
        /// 列出当前计算机的所有逻辑根目录（磁盘分区）
        /// </summary>
        /// <returns></returns>
        public List<FileDvo> GetListRootAsync()
        {
            var fileList = new List<FileDvo>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                var item = new FileDvo
                {
                    name = drive.Name,
                    path = drive.RootDirectory.FullName,
                    is_dir = true,
                    icon = "drive"
                };

                if (drive.IsReady)
                {
                    item.size = drive.TotalSize;
                    item.change_time = TimeUtils.GetUnixTime(drive.RootDirectory.LastWriteTimeUtc);
                }

                fileList.Add(item);
            }

            return fileList;
        }

        /// <summary>
        /// 列出当前用户主目录下的文件与子目录
        /// </summary>
        /// <returns></returns>
        public List<FileDvo> GetListHomeAsync()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return DoListAll(path);
        }

        /// <summary>
        /// 列出当前用户桌面目录下的文件与子目录
        /// </summary>
        /// <returns></returns>
        public List<FileDvo> GetListDesktopAsync()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return DoListAll(path);
        }

        /// <summary>
        /// 获取指定目录的文件列表（目录+文档）
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public List<FileDvo> GetListAsync(string path)
        {
            return DoListAll(path);
        }

        /// <summary>
        /// 获取指定目录的子目录列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public List<FileDvo> GetListDirAsync(string path)
        {
            var info = GetDirectory(path);

            var list = new List<FileDvo>();
            foreach (var dir in SafeGetDirectories(info))
            {
                list.Add(FromDir(dir));
            }

            return list;
        }

        /// <summary>
        /// 获取指定目录的文档列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        public List<FileDvo> GetListDocAsync(string path)
        {
            var info = GetDirectory(path);

            var list = new List<FileDvo>();
            foreach (var file in SafeGetFiles(info))
            {
                list.Add(FromDoc(file));
            }

            return list;
        }
        #endregion

        #region 文件详情
        /// <summary>
        /// 获取文件或目录的详细信息
        /// </summary>
        /// <param name="path">文件或目录路径</param>
        /// <returns></returns>
        public FileDvo GetDetailAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Error("路径不能为空！");
            }

            if (Directory.Exists(path))
            {
                var dir = new DirectoryInfo(path);
                var dvo = FromDir(dir);
                dvo.size = CalcDirSize(dir);
                return dvo;
            }

            if (File.Exists(path))
            {
                var file = new FileInfo(path);
                var dvo = FromDoc(file);
                dvo.hash = CalcFileHash(path);
                return dvo;
            }

            Error($"路径不存在：{path}");
            return null;
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 复制文件或目录
        /// </summary>
        /// <param name="request"></param>
        /// <returns>目标文件信息</returns>
        [HttpPost]
        public FileDvo CopyAsync(FileTransferRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.src) || string.IsNullOrWhiteSpace(request.dst))
            {
                Error("源路径和目标路径不能为空！");
            }

            var src = request.src;
            if (Directory.Exists(src))
            {
                var dst = ResolveDestPath(src, request.dst);
                EnsureNotNested(src, dst);
                CopyDirectory(src, dst, request.overwrite);
                return GetDetailAsync(dst);
            }

            if (File.Exists(src))
            {
                var dst = ResolveDestPath(src, request.dst);
                EnsureParentExists(dst);
                File.Copy(src, dst, request.overwrite);
                return GetDetailAsync(dst);
            }

            Error($"源路径不存在：{src}");
            return null;
        }

        /// <summary>
        /// 移动文件或目录
        /// </summary>
        /// <param name="request"></param>
        /// <returns>目标文件信息</returns>
        [HttpPost]
        public FileDvo MoveAsync(FileTransferRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.src) || string.IsNullOrWhiteSpace(request.dst))
            {
                Error("源路径和目标路径不能为空！");
            }

            var src = request.src;
            if (Directory.Exists(src))
            {
                var dst = ResolveDestPath(src, request.dst);
                EnsureNotNested(src, dst);
                Directory.Move(src, dst);
                return GetDetailAsync(dst);
            }

            if (File.Exists(src))
            {
                var dst = ResolveDestPath(src, request.dst);
                EnsureParentExists(dst);
                File.Move(src, dst, request.overwrite);
                return GetDetailAsync(dst);
            }

            Error($"源路径不存在：{src}");
            return null;
        }

        /// <summary>
        /// 重命名文件或目录
        /// </summary>
        /// <param name="request"></param>
        /// <returns>重命名后的文件信息</returns>
        [HttpPost]
        public FileDvo RenameAsync(FileRenameRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.src) || string.IsNullOrWhiteSpace(request.name))
            {
                Error("源路径和新名称不能为空！");
            }

            var src = request.src;
            var root = Path.GetDirectoryName(src);
            var dst = Path.Combine(root ?? string.Empty, request.name);

            if (Directory.Exists(src))
            {
                Directory.Move(src, dst);
                return GetDetailAsync(dst);
            }

            if (File.Exists(src))
            {
                File.Move(src, dst);
                return GetDetailAsync(dst);
            }

            Error($"源路径不存在：{src}");
            return null;
        }

        /// <summary>
        /// 删除文件或目录（目录将递归删除）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public bool DeleteAsync(FileDeleteRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.path))
            {
                Error("路径不能为空！");
            }

            var path = request.path;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            Error($"路径不存在：{path}");
            return false;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        [HttpPost]
        public FileDvo CreateDirAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Error("路径不能为空！");
            }

            var info = Directory.CreateDirectory(path);
            return FromDir(info);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 列举目录下所有子目录与文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<FileDvo> DoListAll(string path)
        {
            var info = GetDirectory(path);

            var list = new List<FileDvo>();
            foreach (var dir in SafeGetDirectories(info))
            {
                list.Add(FromDir(dir));
            }

            foreach (var file in SafeGetFiles(info))
            {
                list.Add(FromDoc(file));
            }

            return list;
        }

        /// <summary>
        /// 获取目录信息，不存在时抛出异常
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private DirectoryInfo GetDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Error("路径不能为空！");
            }

            var info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                Error($"目录不存在：{path}");
            }

            return info;
        }

        private IEnumerable<DirectoryInfo> SafeGetDirectories(DirectoryInfo info)
        {
            try
            {
                return info.GetDirectories();
            }
            catch
            {
                return Array.Empty<DirectoryInfo>();
            }
        }

        private IEnumerable<FileInfo> SafeGetFiles(DirectoryInfo info)
        {
            try
            {
                return info.GetFiles();
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
                is_dir = true,
                icon = "folder",
                create_time = TimeUtils.GetUnixTime(dir.CreationTimeUtc),
                change_time = TimeUtils.GetUnixTime(dir.LastWriteTimeUtc)
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
                is_dir = false,
                icon = string.IsNullOrWhiteSpace(file.Extension) ? "file" : file.Extension.TrimStart('.').ToLower(),
                size = file.Length,
                create_time = TimeUtils.GetUnixTime(file.CreationTimeUtc),
                change_time = TimeUtils.GetUnixTime(file.LastWriteTimeUtc)
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
        /// 递归复制目录
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="overwrite"></param>
        private void CopyDirectory(string src, string dst, bool overwrite)
        {
            Directory.CreateDirectory(dst);

            foreach (var file in Directory.GetFiles(src))
            {
                var target = Path.Combine(dst, Path.GetFileName(file));
                File.Copy(file, target, overwrite);
            }

            foreach (var dir in Directory.GetDirectories(src))
            {
                var target = Path.Combine(dst, Path.GetFileName(dir));
                CopyDirectory(dir, target, overwrite);
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

            foreach (var file in SafeGetFiles(dir))
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

            foreach (var sub in SafeGetDirectories(dir))
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
                using var stream = File.OpenRead(path);
                var bytes = MD5.HashData(stream);
                return Convert.ToHexString(bytes).ToLower();
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
