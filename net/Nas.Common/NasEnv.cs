namespace Com.Scm.Nas
{
    public class NasEnv
    {
        /// <summary>
        /// 服务路径（公有）
        /// </summary>
        public const string SERVER_URL = "https://api.c-scm.net";

        /// <summary>
        /// 无端路径（私有）
        /// </summary>
        public const string REMOTE_URL = "https://api.c-scm.net";

        /// <summary>
        /// 虚拟路径标识
        /// </summary>
        public const string VirtualTag = "nas:/";

        /// <summary>
        /// 根目录
        /// </summary>
        public const string RootTag = "@/";

        /// <summary>
        /// 用户目录
        /// </summary>
        public const string UserTag = "~/";

        /// <summary>
        /// 上传临时文件后缀
        /// </summary>
        public const string TempFileExt = ".nas";

        /// <summary>
        /// 默认目录ID
        /// </summary>
        public const long DEF_DIR_ID = 1000000000000000001L;

        /// <summary>
        /// Nas数据目录
        /// </summary>
        public const string DEF_NAS_DIR = "Nas";

        /// <summary>
        /// 当前系统路径分隔符
        /// </summary>
        public static readonly char DirSeparator = System.IO.Path.DirectorySeparatorChar;

        /// <summary>
        /// 网络路径分隔符
        /// </summary>
        public static readonly char WebSeparator = '/';

        /// <summary>
        /// 块文件大小
        /// </summary>
        public const long MAX_CHUNK_SIZE = 1024 * 1024 * 5;

        #region 服务端接口
        /// <summary>
        /// 环境初始化
        /// </summary>
        public const string InitUrl = "/NasSync/Init";

        /// <summary>
        /// 小文件下载
        /// </summary>
        public const string DsUrl = "/Nas/Ds/";
        public const string DlUrl = "/Nas/Dl/";
        /// <summary>
        /// 小文件预览
        /// </summary>
        public const string VsUrl = "/Nas/Vs/";
        public const string VlUrl = "/Nas/Vl/";

        /// <summary>
        /// 文件上传路径
        /// </summary>
        public const string FileUploadUrl = "/Upload/File";
        /// <summary>
        /// 分块上传路径
        /// </summary>
        public const string ChunkUploadUrl = "/Upload/Chunk";
        /// <summary>
        /// 分块上传检测
        /// </summary>
        public const string CheckUploadUrl = "/Upload/Check";
        /// <summary>
        /// 分块合并
        /// </summary>
        public const string MergeUploadUrl = "/Upload/Merge";

        /// <summary>
        /// 文件下载路径
        /// </summary>
        public const string FileDownloadUrl = "/Download/file";

        /// <summary>
        /// 获取目录路径
        /// </summary>
        public const string DirUrl = "/NasSync/Dir";
        /// <summary>
        /// 获取文档路径
        /// </summary>
        public const string DocUrl = "/NasSync/Doc";
        /// <summary>
        /// 获取日志路径
        /// </summary>
        public const string LogUrl = "/NasSync/Log";

        /// <summary>
        /// 获取文件列表
        /// </summary>
        public const string FileUrl = "/NasSync/File";

        /// <summary>
        /// 同步操作路径
        /// </summary>
        public const string SyncUrl = "/NasSync/Sync";

        /// <summary>
        /// 获取驱动列表
        /// </summary>
        public const string ListFolderUrl = "/NasSync/Folder";
        /// <summary>
        /// 更新驱动路径
        /// </summary>
        public const string SaveFolderUrl = "/NasSync/Folder";
        #endregion

        #region 系统专用目录
        /// <summary>
        /// 最近
        /// </summary>
        public const string NodeRecent = "Recent";
        /// <summary>
        /// 常用
        /// </summary>
        public const string NodeUsually = "Usually";
        /// <summary>
        /// 收藏
        /// </summary>
        public const string NodeFavorites = "Favorites";
        /// <summary>
        /// 下载
        /// </summary>
        public const string NodeDownloads = "Downloads";
        /// <summary>
        /// 设备
        /// </summary>
        public const string NodeDevices = "Devices";
        /// <summary>
        /// 私密
        /// </summary>
        public const string NodeSecret = "Secret";
        /// <summary>
        /// 共享
        /// </summary>
        public const string NodePublic = "Public";
        /// <summary>
        /// 标签
        /// </summary>
        public const string NodeTags = "Tags";
        /// <summary>
        /// 文档
        /// </summary>
        public const string NodeDocs = "Docs";
        /// <summary>
        /// 应用
        /// </summary>
        public const string NodeApps = "Apps";
        /// <summary>
        /// 回收站
        /// </summary>
        public const string NodeTrash = "Trash";
        #endregion

        #region 系统专用目录
        /// <summary>
        /// 最近
        /// </summary>
        public const string PathRecent = "/" + NodeRecent;
        /// <summary>
        /// 常用
        /// </summary>
        public const string PathUsually = "/" + NodeUsually;
        /// <summary>
        /// 收藏
        /// </summary>
        public const string PathFavorites = "/" + NodeFavorites;
        /// <summary>
        /// 设备
        /// </summary>
        public const string PathDevices = "/" + NodeDevices;
        /// <summary>
        /// 下载
        /// </summary>
        public const string PathDownloads = "/" + NodeDownloads;
        /// <summary>
        /// 私密
        /// </summary>
        public const string PathSecret = "/" + NodeSecret;
        /// <summary>
        /// 共享
        /// </summary>
        public const string PathPublic = "/" + NodePublic;
        /// <summary>
        /// 标签
        /// </summary>
        public const string PathTags = "/" + NodeTags;
        /// <summary>
        /// 文档
        /// </summary>
        public const string PathDocs = "/" + NodeDocs;
        /// <summary>
        /// 应用
        /// </summary>
        public const string PathApps = "/" + NodeApps;
        /// <summary>
        /// 回收站
        /// </summary>
        public const string PathTrash = "/" + NodeTrash;
        #endregion
    }
}
