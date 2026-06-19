using Com.Scm.Dto;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers
{
    /// <summary>
    /// 文件信息服务
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "Scm")]
    public class FileController : ApiController
    {
        public FileController()
        {
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        [HttpGet("info/{id}")]
        public async Task<ScmFileDto> Info(string path)
        {
            LogUtils.Debug("获取文件信息：" + path);

            var dto = new ScmFileDto
            {
                //id = dao.id,
                //type = dao.type,
                //kind = dao.kind,
                //dir_id = dao.dir_id,
                //name = dao.name,
                //path = dao.path,
                //hash = dao.hash,
                //size = dao.size,
                //modify_time = dao.modify_time,
                //ver = dao.ver
            };

            return dto;
        }

        #region 文件查看
        /// <summary>
        /// 列出当前计算机的所有逻辑根目录（磁盘分区）
        /// </summary>
        [HttpGet("ListRoot")]
        public async Task<List<ScmFileDto>> ListRoot()
        {
            var list = new List<ScmFileDto>();

            foreach (var drive in DriveInfo.GetDrives())
            {
                list.Add(new ScmFileDto
                {
                    name = drive.Name,                          // "C:\"
                    path = drive.RootDirectory.FullName,        // "C:\"
                    is_dir = true,
                    size = drive.IsReady ? drive.TotalSize : 0,
                });
            }

            return list;
        }

        /// <summary>
        /// 列出当前用户主目录下的文件与子目录
        /// </summary>
        [HttpGet("ListHome")]
        public async Task<List<ScmFileDto>> ListHome()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return await DoListAll(path);
        }

        /// <summary>
        /// 列出当前用户桌面目录下的文件与子目录
        /// </summary>
        [HttpGet("ListDesktop")]
        public async Task<List<ScmFileDto>> ListDesktop()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return await DoListAll(path);
        }

        [HttpGet("ListDocuments")]
        public async Task<List<ScmFileDto>> ListDocuments()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return await DoListAll(path);
        }

        [HttpGet("ListAudio")]
        public async Task<List<ScmFileDto>> ListAudio()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            return await DoListAll(path);
        }

        [HttpGet("ListVideo")]
        public async Task<List<ScmFileDto>> ListVideo()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            return await DoListAll(path);
        }

        [HttpGet("ListImage")]
        public async Task<List<ScmFileDto>> ListImage()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            return await DoListAll(path);
        }

        [HttpGet("ListDownloads")]
        public async Task<List<ScmFileDto>> ListDownloads()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            path = Path.Combine(path, "Downloads");
            return await DoListAll(path);
        }

        /// <summary>
        /// 获取指定目录的文件列表（目录+文档）
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        [HttpGet("ListAll")]
        public async Task<List<ScmFileDto>> ListAll(string path)
        {
            LogUtils.Debug($"获取文件列表：path={path}");

            return await DoListAll(path);
        }

        /// <summary>
        /// 获取指定目录的子目录列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        [HttpGet("ListDir")]
        public async Task<List<ScmFileDto>> ListDir(string path)
        {
            LogUtils.Debug($"获取目录列表：path={path}");
            return await DoListDir(path);
        }

        /// <summary>
        /// 获取指定目录的文档列表
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns></returns>
        [HttpGet("ListDoc")]
        public async Task<List<ScmFileDto>> ListDoc(string path)
        {
            LogUtils.Debug($"获取文档列表：path={path}");
            return await DoListDoc(path);
        }

        /// <summary>
        /// 列举所有
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<List<ScmFileDto>> DoListAll(string path)
        {
            LogUtils.Debug($"获取文件列表：path={path}");

            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                return null;
            }

            var list = new List<ScmFileDto>();

            foreach (var dir in info.GetDirectories())
            {
                list.Add(new ScmFileDto
                {
                    name = dir.Name,
                    path = dir.FullName,
                    is_dir = true,
                    create_time = TimeUtils.GetUnixTime(dir.CreationTimeUtc),
                    change_time = TimeUtils.GetUnixTime(dir.LastWriteTimeUtc),
                });
            }

            foreach (var file in info.GetFiles())
            {
                list.Add(new ScmFileDto
                {
                    name = file.Name,
                    path = file.FullName,
                    size = file.Length,
                    is_dir = false,
                    create_time = TimeUtils.GetUnixTime(file.CreationTimeUtc),
                    change_time = TimeUtils.GetUnixTime(file.LastWriteTimeUtc),
                });
            }

            return list;
        }

        /// <summary>
        /// 列举目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<List<ScmFileDto>> DoListDir(string path)
        {
            LogUtils.Debug($"获取文件列表：path={path}");

            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                return null;
            }

            var list = new List<ScmFileDto>();

            foreach (var dir in info.GetDirectories())
            {
                list.Add(new ScmFileDto
                {
                    name = dir.Name,
                    path = dir.FullName,
                    is_dir = true,
                    create_time = TimeUtils.GetUnixTime(dir.CreationTimeUtc),
                    change_time = TimeUtils.GetUnixTime(dir.LastWriteTimeUtc),
                });
            }

            return list;
        }

        /// <summary>
        /// 列举文档
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<List<ScmFileDto>> DoListDoc(string path)
        {
            LogUtils.Debug($"获取文件列表：path={path}");

            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                return null;
            }

            var list = new List<ScmFileDto>();

            foreach (var file in info.GetFiles())
            {
                list.Add(new ScmFileDto
                {
                    name = file.Name,
                    path = file.FullName,
                    size = file.Length,
                    is_dir = false,
                    create_time = TimeUtils.GetUnixTime(file.CreationTimeUtc),
                    change_time = TimeUtils.GetUnixTime(file.LastWriteTimeUtc),
                });
            }

            return list;
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ScmFileDto> Create(string path)
        {
            var info = new DirectoryInfo(path);
            info.Create();

            var dto = new ScmFileDto();
            dto.name = info.Name;
            dto.path = path;
            dto.create_time = TimeUtils.GetUnixTime(info.CreationTimeUtc);

            return dto;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Copyto(string src, string dst)
        {
            return FileUtils.Copyto(src, dst);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Moveto(string src, string dst)
        {
            return FileUtils.Moveto(src, dst);
        }

        /// <summary>
        /// 更名文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Rename(string src, string dst, bool overwrite)
        {
            return FileUtils.RenameTo(src, dst);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Delete(string path)
        {
            return FileUtils.Delete(path);
        }
        #endregion
    }
}
