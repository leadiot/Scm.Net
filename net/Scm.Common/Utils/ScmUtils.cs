using Com.Scm.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Scm.Utils
{
    /// <summary>
    /// Scm.Net公共方法
    /// </summary>
    public class ScmUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsValidId(long id)
        {
            return id > 1000;
        }

        public static bool IsNormalId(long id)
        {
            return id > ScmEnv.DEFAULT_ID;
        }

        public static bool IsValidId(string text)
        {
            return Regex.IsMatch(text, @"^[1-9]\d{18}$");
        }

        public static bool IsValidUser(string text)
        {
            return Regex.IsMatch(text, @"^\w{2,32}(@\w{2,32})?$");
        }

        public static bool IsValidPass(string text)
        {
            return Regex.IsMatch(text, @"^\w{64}$");
        }

        public static bool IsValidCode(string text, int len)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z0-9]{" + len + "}$");
        }

        public static bool IsValidCode(string text, int min, int max)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z0-9]{" + min + "," + max + "}$");
        }

        public static bool IsValidUnitCodes(string text)
        {
            return Regex.IsMatch(text, @"^U\w{7}$");
        }

        /// <summary>
        ///  根据文件类型分配路径
        /// </summary>
        /// <param name="fileExt"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AssigendPath(string fileExt, string path)
        {
            var dataPath = DateTime.Now.ToString("yyyyMMdd");
            if (FileUtils.IsImageFile(fileExt))
                return path + "/upload/images/" + dataPath + "/";
            if (FileUtils.IsAudioFile(fileExt))
                return "/upload/audios/" + dataPath + "/";
            if (FileUtils.IsVideoFile(fileExt))
                return path + "/upload/videos/" + dataPath + "/";
            if (FileUtils.IsOfficeFile(fileExt))
                return "/upload/office/" + dataPath + "/";
            return path + "/upload/others/";
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public static string SmsCode()
        {
            return TextUtils.RandomNumber(6);
        }

        /// <summary>
        /// 将字符串转换为long类型数组
        /// </summary>
        /// <param name="str">如1,2,3,4,5</param>
        /// <param name="c">分隔符</param>
        /// <returns></returns>
        public static List<string> ToList(string str, char c = ',')
        {
            var array = str.Split(c);
            var list = new List<string>();
            list.AddRange(array);
            return list;
        }

        /// <summary>
        /// 将字符串转换为long类型数组
        /// </summary>
        /// <param name="str">如1,2,3,4,5</param>
        /// <returns></returns>
        public static List<long> ToListLong(string str, char c = ',')
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            var array = str.Split(c);
            var list = new List<long>();
            foreach (var item in array)
            {
                if (TextUtils.IsNumberic(item))
                {
                    list.Add(long.Parse(item));
                }
            }
            return list;
        }

        #region 服务路径操作
        /// <summary>
        /// 转换为本机路径
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string ToMachinePath(string uri)
        {
            return uri.Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// 转换为虚拟路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static string ToVirtualPath(string path, string basePath)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(basePath))
            {
                return path;
            }

            if (basePath[basePath.Length - 1] != Path.DirectorySeparatorChar)
            {
                basePath += Path.DirectorySeparatorChar;
            }
            return path.Replace(basePath, "/").Replace(Path.DirectorySeparatorChar, '/');
        }

        #region 目录操作
        public static List<ScmFolderInfo> GetFolders(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                return null;
            }

            return GetFolders(dir, "");
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static List<ScmFolderInfo> GetFolders(DirectoryInfo dir, string baseUri)
        {
            if (dir == null || !dir.Exists)
            {
                return null;
            }

            var list = new List<ScmFolderInfo>();
            foreach (var sub in dir.GetDirectories())
            {
                var folder = new ScmFolderInfo();
                folder.Name = sub.Name;
                folder.Uri = baseUri + "/" + sub.Name;
                folder.Children = GetFolders(sub, folder.Uri);
                list.Add(folder);
            }
            return list;
        }
        #endregion

        #region 文件操作
        /// <summary>
        /// 获得文件目录下的文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static List<ScmFileInfo> GetFiles(string path, ScmDocTypeEnum type, string basePath)
        {
            var list = new List<ScmFileInfo>();
            var folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                foreach (var file in folder.GetFiles())
                {
                    var exts = file.Extension;
                    if (string.IsNullOrWhiteSpace(exts))
                    {
                        continue;
                    }

                    exts = exts.Trim().ToLower();
                    if (type == ScmDocTypeEnum.None)
                    {
                        if (FileUtils.IsByteFile(exts))
                        {
                            list.Add(GetFileInfo(file, ScmDocTypeEnum.Byte, basePath));
                            continue;
                        }
                        if (FileUtils.IsTextFile(exts))
                        {
                            list.Add(GetFileInfo(file, ScmDocTypeEnum.Text, basePath));
                            continue;
                        }
                        if (FileUtils.IsImageFile(exts))
                        {
                            list.Add(GetFileInfo(file, ScmDocTypeEnum.Image, basePath));
                            continue;
                        }
                        if (FileUtils.IsMediaFile(exts))
                        {
                            list.Add(GetFileInfo(file, ScmDocTypeEnum.Media, basePath));
                            continue;
                        }
                        if (FileUtils.IsOfficeFile(exts))
                        {
                            list.Add(GetFileInfo(file, ScmDocTypeEnum.Office, basePath));
                            continue;
                        }

                        list.Add(GetFileInfo(file, type, basePath));
                        continue;
                    }

                    if (type == ScmDocTypeEnum.Byte && FileUtils.IsByteFile(exts))
                    {
                        list.Add(GetFileInfo(file, ScmDocTypeEnum.Byte, basePath));
                        continue;
                    }
                    if (type == ScmDocTypeEnum.Text && FileUtils.IsTextFile(exts))
                    {
                        list.Add(GetFileInfo(file, ScmDocTypeEnum.Text, basePath));
                        continue;
                    }
                    if (type == ScmDocTypeEnum.Image && FileUtils.IsImageFile(exts))
                    {
                        list.Add(GetFileInfo(file, ScmDocTypeEnum.Image, basePath));
                        continue;
                    }
                    if (type == ScmDocTypeEnum.Media && FileUtils.IsMediaFile(exts))
                    {
                        list.Add(GetFileInfo(file, ScmDocTypeEnum.Media, basePath));
                        continue;
                    }
                    if (type == ScmDocTypeEnum.Office && FileUtils.IsOfficeFile(exts))
                    {
                        list.Add(GetFileInfo(file, ScmDocTypeEnum.Office, basePath));
                        continue;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 转换为虚拟路径文件信息
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        private static ScmFileInfo GetFileInfo(FileInfo file, ScmDocTypeEnum type, string basePath)
        {
            var item = new ScmFileInfo();
            item.Type = type;
            item.Name = file.Name;
            //item.FullName = file.FullName;
            item.Extension = file.Extension;
            item.Length = file.Length;
            //item.FileSize = ToFileSize(file.Length);
            item.Uri = ToVirtualPath(file.FullName, basePath);
            item.CreationTime = file.CreationTime;
            item.LastWriteTime = file.LastWriteTime;
            item.LastAccessTime = file.LastAccessTime;
            return item;
        }
        #endregion
        #endregion

        #region 掩码处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string EncodePass(string pass)
        {
            var len1 = 4;
            var len2 = 4;
            var len3 = 4;
            var len = pass.Length >> 1;

            var random = new Random();
            var idx1 = random.Next(len);
            var idx2 = random.Next(len) + len1 + idx1;

            var tmp1 = TextUtils.RandomString(len1, false);
            var tmp2 = TextUtils.RandomString(len2, false);
            var tmp3 = TextUtils.RandomString(len3, false);

            var buf = new StringBuilder(pass);
            buf.Insert(idx1, tmp1).Insert(idx2, tmp2).Append(tmp3).Insert(0, idx1.ToString("x2") + idx2.ToString("x2"));
            return buf.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static string DecodePass(string pass)
        {
            if (pass == null || pass.Length != 80)
            {
                return pass;
            }

            var len1 = 4;
            var len2 = 4;
            var len3 = 4;

            var tmp1 = pass.Substring(0, 2);
            var tmp2 = pass.Substring(2, 2);
            var idx1 = int.Parse(tmp1, NumberStyles.HexNumber);
            var idx2 = int.Parse(tmp2, NumberStyles.HexNumber);

            var buf = new StringBuilder(pass);
            buf.Remove(0, 4).Remove(buf.Length - len3, len3).Remove(idx2, len2).Remove(idx1, len1);
            return buf.ToString();
        }
        #endregion
    }
}
