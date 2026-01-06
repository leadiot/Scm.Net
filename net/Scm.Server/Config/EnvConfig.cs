using Com.Scm.Utils;
using Microsoft.AspNetCore.Builder;

namespace Com.Scm.Config
{
    public partial class EnvConfig
    {
        public const string NAME = "Env";

        /// <summary>
        /// 数据目录物理路径，可以是相对或绝对路径。
        /// </summary>
        public string DataDir { get; set; }
        /// <summary>
        /// 数据目录映射路径
        /// </summary>
        public string DataUri { get; set; }

        /// <summary>
        /// 日志目录，相对于dataDir
        /// </summary>
        public string Logs { get; set; }

        /// <summary>
        /// 临时目录，相对于dataDir
        /// </summary>
        public string Temp { get; set; }
        /// <summary>
        /// 上传目录，相对于dataDir
        /// </summary>
        public string Upload { get; set; }
        /// <summary>
        /// 图像目录，相对于dataDir
        /// </summary>
        public string Images { get; set; }
        /// <summary>
        /// 头像目录，相对于dataDir
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 语言资源路径
        /// </summary>
        public string ResourcesPath { get; set; }
        /// <summary>
        /// 默认语言
        /// </summary>
        public string DefaultCulture { get; set; }
        /// <summary>
        /// 支持语言
        /// </summary>
        public string[] SupportedCultures { get; set; }

        /// <summary>
        /// 默认密码模式，支持Fixed，Random等
        /// </summary>
        public string DefaultPassMode { get; set; }
        /// <summary>
        /// 默认密码
        /// </summary>
        public string DefaultPassWord { get; set; }

        public void Prepare(WebApplicationBuilder builder)
        {
            DataDir = GetPath(builder.Environment.ContentRootPath, DataDir, "data");
            if (DataDir.EndsWith("\\"))
            {
                DataDir = DataDir.Substring(0, DataDir.Length - 1);
            }

            Upload = GetPath(DataDir, Upload, "upload");

            Images = GetPath(DataDir, Images, "images");

            Avatar = GetPath(DataDir, Avatar, "avatar");

            Logs = GetPath(DataDir, Logs, "logs");

            Temp = GetPath(DataDir, Temp, "temp");

            ResourcesPath = "Resources";

            if (DefaultPassMode != "Random")
            {
                DefaultPassMode = "Fixed";
                if (string.IsNullOrWhiteSpace(DefaultPassWord))
                {
                    DefaultPassWord = ScmEnv.DEFAULT_PASS;
                }
            }
        }

        private string GetPath(string root, string path, string def)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = def;
            }
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(root, path);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        #region 系统相关
        public string GetDataPath(string path)
        {
            return Combine(DataDir, path);
        }

        public string GetTempPath(string path)
        {
            return Combine(Temp, path);
        }

        public string GetLogsPath(string path)
        {
            return Combine(Logs, path);
        }

        public string GetUploadPath(string path)
        {
            return Combine(Upload, path);
        }

        public string GetImagesPath(string path)
        {
            return Combine(Images, path);
        }

        public string GetAvatarPath(string path)
        {
            return Combine(Avatar, path);
        }

        private string Combine(string basePath, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return basePath;
            }

            path = ScmUtils.ToMachinePath(path);
            if (path[0] == '\\')
            {
                path = path.Substring(1);
            }

            if (Path.IsPathRooted(path))
            {
                return path;
            }
            return Path.Combine(basePath, path);
        }
        #endregion

        public string ToUri(string path)
        {
            return path.Replace(DataDir, DataUri).Replace("\\", "/");
        }

        #region 文件相关
        public void SaveFile(string path, string name, string content)
        {
            var tmp = GetDataPath(path);
            if (!Directory.Exists(tmp))
            {
                Directory.CreateDirectory(tmp);
            }

            var file = Path.Combine(tmp, name);
            using (var writer = new StreamWriter(file))
            {
                writer.Write(content);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">相对于Data目录的路径</param>
        /// <param name="name">文件名称</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        public async Task SaveFileAsync(string path, string name, string content)
        {
            var tmp = GetDataPath(path);
            if (!Directory.Exists(tmp))
            {
                Directory.CreateDirectory(tmp);
            }

            var file = Path.Combine(tmp, name);
            using (var writer = new StreamWriter(file))
            {
                await writer.WriteAsync(content);
            }
        }

        public string ReadFile(string path, string name)
        {
            var tmp = GetDataPath(path);
            if (!Directory.Exists(tmp))
            {
                return null;
            }

            return ReadFile(Path.Combine(tmp, name));
        }

        public string ReadFile(string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }

            using (var reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }

        public async Task<string> ReadFileAsync(string path, string name)
        {
            var tmp = GetDataPath(path);
            if (!Directory.Exists(tmp))
            {
                return null;
            }

            return await ReadFileAsync(Path.Combine(tmp, name));
        }

        public async Task<string> ReadFileAsync(string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }

            using (var reader = new StreamReader(file))
            {
                return await reader.ReadToEndAsync();
            }
        }
        #endregion

        public string GetPassword()
        {
            if (DefaultPassMode == "Random")
            {
                return TextUtils.RandomString(6);
            }
            if (DefaultPassMode == "Fixed")
            {
                return DefaultPassWord;
            }
            return null;
        }
    }
}
