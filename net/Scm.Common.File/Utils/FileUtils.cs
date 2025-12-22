using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Scm.Utils
{
    public class FileUtils
    {
        #region 文件操作
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool CreateDoc(string fileName)
        {
            using (var stream = File.Create(fileName))
            {
            }
            return true;
        }

        /// <summary>
        /// 是否空文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsEmptyDoc(string path)
        {
            var info = new FileInfo(path);
            return info.Length < 1;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static long GetDocSize(string file)
        {
            var info = new FileInfo(file);
            if (!info.Exists)
            {
                return 0;
            }

            return info.Length;
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        private static bool CopyDoc(string src, string dst)
        {
            var dir = Path.GetDirectoryName(dst);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.Copy(src, dst, true);
            return true;
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        private static bool MoveDoc(string src, string dst)
        {
            if (File.Exists(dst))
            {
                File.Delete(dst);
            }

            var dir = Path.GetDirectoryName(dst);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.Move(src, dst);
            return true;
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void DeleteDoc(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// 获取重复后的文件名，返回格式：xxxx (nnnn).ext
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileName(string path, string file)
        {
            var idx = 0;
            var name = Path.GetFileNameWithoutExtension(file);
            var exts = Path.GetExtension(file);
            var tmp = "";
            while (idx < 1000000)
            {
                idx += 1;
                tmp = Path.Combine(path, $"{name} ({idx}){exts}");
                if (!File.Exists(tmp))
                {
                    break;
                }
            }

            return tmp;
        }
        #endregion

        #region 目录操作
        public static int CopyDir(string srcPath, string dstPath)
        {
            if (!Directory.Exists(srcPath))
            {
                return 0;
            }

            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
            }

            var qty = 0;
            foreach (var srcDir in Directory.GetDirectories(srcPath))
            {
                qty += CopyDir(srcDir, Path.Combine(dstPath, Path.GetFileName(srcDir)));
            }

            foreach (var srcDoc in Directory.GetFiles(srcPath))
            {
                CopyDoc(srcDoc, Path.Combine(dstPath, Path.GetFileName(srcDoc)));
                qty += 1;
            }

            return qty;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="folderPath">目录的绝对路径</param>
        public static void CreateDir(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        /// <summary>
        /// 是否空目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsEmptyDir(string path)
        {
            var info = new DirectoryInfo(path);
            return info.GetFiles().Length < 1 && info.GetDirectories().Length < 1;
        }

        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">文件的绝对路径</param>
        /// <param name="recursive">是否级联删除</param>
        public static void DeleteDir(string directoryPath, bool recursive = true)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, recursive);
            }
        }

        /// <summary>
        /// 获取重复后的文件名，返回格式：xxxx (nnnn).ext
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetDirName(string path, string file)
        {
            var idx = 0;
            var tmp = "";
            while (idx < 1000000)
            {
                idx += 1;
                tmp = Path.Combine(path, $"{file} ({idx})");
                if (!Directory.Exists(tmp))
                {
                    break;
                }
            }

            return tmp;
        }
        #endregion

        #region 文件或目录操作
        /// <summary>
        /// 复制文件或目录
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static bool Copyto(string src, string dst)
        {
            if (File.Exists(src))
            {
                return CopyDoc(src, dst);
            }

            if (!Directory.Exists(src))
            {
                return false;
            }

            if (!Directory.Exists(dst))
            {
                Directory.CreateDirectory(dst);
            }

            var dirs = Directory.GetDirectories(src);
            foreach (var dir in dirs)
            {
                var tmp = Path.Combine(dst, Path.GetFileName(dir));
                Copyto(dir, tmp);
            }

            var files = Directory.GetFiles(src);
            foreach (var file in files)
            {
                var tmp = Path.Combine(dst, Path.GetFileName(file));
                if (!CopyDoc(file, tmp))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 移动文件或目录
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static bool Moveto(string src, string dst)
        {
            if (File.Exists(src))
            {
                return MoveDoc(src, dst);
            }

            if (!Directory.Exists(src))
            {
                return false;
            }

            if (!Directory.Exists(dst))
            {
                Directory.CreateDirectory(dst);
            }

            Directory.Move(src, dst);

            var dirs = Directory.GetDirectories(src);
            foreach (var dir in dirs)
            {
                var tmp = Path.Combine(dst, Path.GetFileName(dir));
                Moveto(dir, tmp);
            }

            var files = Directory.GetFiles(src);
            foreach (var file in files)
            {
                var tmp = Path.Combine(dst, Path.GetFileName(file));
                if (!MoveDoc(file, tmp))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 更名文件或目录
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="dstFile"></param>
        public static bool RenameTo(string srcFile, string dstFile)
        {
            Delete(dstFile);

            if (File.Exists(srcFile))
            {
                new FileInfo(srcFile).MoveTo(dstFile);
                return true;
            }

            if (Directory.Exists(srcFile))
            {
                new DirectoryInfo(srcFile).MoveTo(dstFile);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除文件或目录
        /// </summary>
        /// <param name="path"></param>
        public static bool Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                return true;
            }

            return false;
        }
        #endregion

        #region 文件类型判断
        #region 根据文件后缀判断文件类型
        /// <summary>
        /// 字节文件后缀
        /// </summary>
        private static List<string> ByteExts = new List<string> { ".bin", ".dat" };
        /// <summary>
        /// 字符文件后缀
        /// </summary>
        private static List<string> TextExts = new List<string> { ".txt", ".log", ".css", ".html", ".htm", ".js", ".xml", ".json", ".cs", ".java", ".py" };
        /// <summary>
        /// 代码文件后缀
        /// </summary>
        private static List<string> CodeExts = new List<string> { ".css", ".html", ".htm", ".js", ".xml", ".json", ".cs", ".java", ".py" };
        /// <summary>
        /// 图像文件后缀
        /// </summary>
        private static List<string> ImageExts = new List<string> { ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".webp", ".tiff" };
        /// <summary>
        /// 音频文件后缀
        /// </summary>
        private static List<string> AudioExts = new List<string> { ".mp3", ".avi", ".wma" };
        /// <summary>
        /// 视频文件后缀
        /// </summary>
        private static List<string> VedioExts = new List<string> { ".rmvb", ".mkv", ".ts", ".wmv", ".rm", ".mp4", ".flv", ".mpeg", ".mov", ".3gp", ".mpg", ".webm" };
        /// <summary>
        /// 媒体文件后缀
        /// </summary>
        private static List<string> MeidaExts = new List<string> { };
        /// <summary>
        /// 办公文件后缀
        /// </summary>
        private static List<string> OfficeExts = new List<string> { ".xls", ".xlsx", ".doc", ".docx", ".ppt", ".pptx", ".pdf", ".csv" };
        /// <summary>
        /// 压缩文件后缀
        /// </summary>
        private static List<string> ArchiveExts = new List<string> { ".zip", ".rar", ".7z", ".gzip", ".tar", ".bzip", ".bzip2" };

        /// <summary>
        /// 是否字节文件
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsByteFile(string fileExt)
        {
            return IsFileFormat(ByteExts, fileExt);
        }

        /// <summary>
        /// 是否文本文件
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsTextFile(string fileExt)
        {
            return IsFileFormat(TextExts, fileExt);
        }

        /// <summary>
        /// 是否图像文件
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsImageFile(string fileExt)
        {
            return IsFileFormat(ImageExts, fileExt);
        }

        /// <summary>
        /// 是否为音频
        /// </summary>
        /// <param name="fileExt">文件扩展名</param>
        /// <returns></returns>
        public static bool IsAudioFile(string fileExt)
        {
            return IsFileFormat(AudioExts, fileExt);
        }

        public static bool IsVideoFile(string fileExt)
        {
            return IsFileFormat(AudioExts, fileExt);
        }

        /// <summary>
        /// 是否媒体文件
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsMediaFile(string fileExt)
        {
            return IsAudioFile(fileExt) || IsVideoFile(fileExt);
        }

        /// <summary>
        /// 是否办公文件
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsOfficeFile(string fileExt)
        {
            return IsFileFormat(OfficeExts, fileExt);
        }

        /// <summary>
        /// 是否归档文件
        /// </summary>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsArchiveFile(string fileExt)
        {
            return IsFileFormat(ArchiveExts, fileExt);
        }

        /// <summary>
        /// 是否为指定类型的文件
        /// </summary>
        /// <param name="extList"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsFileFormat(List<string> extList, string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt))
            {
                return false;
            }

            if (fileExt[0] != '.')
            {
                fileExt = '.' + fileExt;
            }

            return extList.Contains(fileExt.ToLower());
        }
        #endregion

        #region 根据文件签名判断文件类型
        /// <summary>
        /// 从文件流判断文件类型
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static FileType GetFileType(Stream stream)
        {
            stream.Position = 0;

            var len = 160;
            var bytes = new byte[160];
            if (stream.Length < len)
            {
                len = (int)stream.Length;
            }
            stream.Read(bytes, 0, len);

            var fileSigns = InitFileTypes();
            foreach (var fileSign in fileSigns)
            {
                foreach (var sign in fileSign.sign)
                {
                    if (IsMatch(bytes, sign, fileSign.offset))
                    {
                        return fileSign.type;
                    }
                }
            }

            return FileType.UnKnown;
        }

        /// <summary>
        /// 判断文件签名是否相同
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileSigns"></param>
        /// <returns></returns>
        public static bool IsMath(Stream stream, List<FileSign> fileSigns)
        {
            stream.Position = 0;

            var len = 160;
            var bytes = new byte[160];
            if (stream.Length < len)
            {
                len = (int)stream.Length;
            }
            stream.Read(bytes, 0, len);

            foreach (var fileSign in fileSigns)
            {
                foreach (var sign in fileSign.sign)
                {
                    if (IsMatch(bytes, sign, fileSign.offset))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断数组是否相同
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="srcIndex"></param>
        /// <returns></returns>
        public static bool IsMatch(byte[] src, byte[] dst, int srcIndex)
        {
            if (srcIndex >= src.Length - dst.Length - 1)
            {
                return false;
            }

            for (int i = 0; i < dst.Length; i++)
            {
                if (src[srcIndex + i] != dst[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 文件签名列表
        /// </summary>
        /// <returns></returns>
        private static List<FileSign> InitFileTypes()
        {
            var list = new List<FileSign>();
            // 文档类
            list.Add(FileSign.Doc);
            list.Add(FileSign.Docx);
            list.Add(FileSign.Xls);
            list.Add(FileSign.Xlsx);
            list.Add(FileSign.Csv);
            list.Add(FileSign.Pdf);

            // 图像类
            list.Add(FileSign.Bmp);
            list.Add(FileSign.Gif);
            list.Add(FileSign.Jpg);
            list.Add(FileSign.Png);
            list.Add(FileSign.Wmp);
            list.Add(FileSign.Webp);

            // 媒体类
            list.Add(FileSign.Avi);
            list.Add(FileSign.Flv);
            list.Add(FileSign.Mp4);
            list.Add(FileSign.M4v);
            list.Add(FileSign.Mkv);
            list.Add(FileSign.Mov);
            list.Add(FileSign.WebM);
            list.Add(FileSign.Wmv);

            // 归档类
            list.Add(FileSign.Zip);

            return list;
        }
        #endregion

        /// <summary>
        /// 获取文件MIME类型
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetMimeByExt(string ext)
        {
            var sub = ext;
            if (sub[0] == '.')
            {
                sub = sub.Substring(1);
            }
            if (IsImageFile(ext))
            {
                return "image/" + sub;
            }
            if (IsTextFile(ext))
            {
                return "plain/text";
            }

            return "";
        }
        #endregion

        #region 文本文件读写
        /// <summary>
        /// 同步读取文本内容
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content">文件不存在时返回的默认内容</param>
        /// <returns></returns>
        public static string ReadText(string file, string content = null)
        {
            if (!File.Exists(file))
            {
                return content;
            }

            using (var stream = new StreamReader(file))
            {
                return stream.ReadToEnd();
            }
        }

        /// <summary>
        /// 异步读取文本内容
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content">文件不存在时返回的默认内容</param>
        /// <returns></returns>
        public static async Task<string> ReadTextAsync(string file, string content = null)
        {
            if (!File.Exists(file))
            {
                return content;
            }

            using (var stream = new StreamReader(file))
            {
                return await stream.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 同步写入文本内容
        /// </summary>
        /// <param name="file">文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="appendToLast"></param>
        public static bool WriteText(string file, string content, bool appendToLast = false)
        {
            using (var stream = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }

                var bytes = Encoding.Default.GetBytes(content);
                stream.Write(bytes, 0, bytes.Length);
            }

            return true;
        }

        /// <summary>
        /// 异步写入文本内容
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        /// <param name="appendToLast"></param>
        /// <returns></returns>
        public static async Task<bool> WriteTextAsync(string file, string content, bool appendToLast = false)
        {
            using (var stream = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write))
            {
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }

                var bytes = Encoding.Default.GetBytes(content);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }

            return true;
        }
        #endregion

        #region 文件内容操作
        /// <summary>
        /// 转换文件大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ToFileSize(long size)
        {
            var units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            int i = 0;
            while (size > 1024)
            {
                size = size >> 10;
                i++;
            }
            return size + units[i];
        }

        /// <summary>
        /// 转换为HTML的Base64表示
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ToHtmlBase64(string file)
        {
            var pre = "";
            var ext = Path.GetExtension(file)?.ToLower();
            if (ext == ".jpg" || ext == ".jpeg")
            {
                pre = "data:image/jpeg;base64,";
            }
            else if (ext == ".png")
            {
                pre = "data:image/png;base64,";
            }
            else if (ext == ".aac")
            {
                pre = "data:audio/aac;base64,";
            }
            else if (ext == ".mp3")
            {
                pre = "data:audio/mpeg;base64,";
            }
            else if (ext == ".mp4")
            {
                pre = "data:video/mp4;base64,";
            }
            else if (ext == ".wav")
            {
                pre = "data:audio/x-wav;base64,";
            }
            using (var stream = File.OpenRead(file))
            {
                var bytes = new byte[stream.Length];
                var len = stream.Read(bytes, 0, bytes.Length);
                return pre + Convert.ToBase64String(bytes, 0, len);
            }
        }

        /// <summary>
        /// 获取文件摘要
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string Md5(Stream stream)
        {
            return GetDocHash(stream, "MD5");
        }

        /// <summary>
        /// 获取文件摘要
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string Md5(string file)
        {
            return GetDocHash(file, "MD5");
        }

        /// <summary>
        /// 获取文件摘要
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string Sha(Stream stream)
        {
            return GetDocHash(stream, "SHA256");
        }

        /// <summary>
        /// 获取文件摘要
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string Sha(string file)
        {
            return GetDocHash(file, "SHA256");
        }

        /// <summary>
        /// 获取文件摘要
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cipher"></param>
        /// <returns></returns>
        public static string GetDocHash(string file, string cipher)
        {
            var alg = System.Security.Cryptography.HashAlgorithm.Create(cipher);
            using (var stream = File.OpenRead(file))
            {
                var result = alg.ComputeHash(stream);
                return TextUtils.ToHexString(result);
            }
        }

        /// <summary>
        /// 获取文件摘要
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="cipher"></param>
        /// <returns></returns>
        public static string GetDocHash(Stream stream, string cipher)
        {

            var alg = System.Security.Cryptography.HashAlgorithm.Create(cipher);

            stream.Seek(0, SeekOrigin.Begin);
            var result = alg.ComputeHash(stream);
            return TextUtils.ToHexString(result);
        }
        #endregion

        /// <summary>
        /// 文件路径合并
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }

        /// <summary>
        /// 获取文件名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            return System.IO.Path.GetFileName(path);
        }

        public static string GetExtension(string path)
        {
            return System.IO.Path.GetExtension(path);
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// 获取目录路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDir(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }
    }
}
