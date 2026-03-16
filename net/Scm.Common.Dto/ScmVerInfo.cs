using Com.Scm.Dto;
using Com.Scm.Enums;
using System.Text.RegularExpressions;

namespace Com.Scm
{
    public class ScmVerInfo : ScmDto
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        public ScmClientTypeEnum client { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public string ver { get; set; }

        /// <summary>
        /// 主版本号
        /// </summary>
        public int major { get; set; }

        /// <summary>
        /// 次版本号
        /// </summary>
        public int minor { get; set; }

        /// <summary>
        /// 修订号
        /// </summary>
        public int patch { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 构建版本
        /// </summary>
        public string build { get; set; }

        /// <summary>
        /// 最小版本
        /// </summary>
        public string ver_min { get; set; }

        /// <summary>
        /// 最大版本
        /// </summary>
        public string ver_max { get; set; }

        /// <summary>
        /// 是否内测
        /// </summary>
        public bool alpha { get; set; }

        /// <summary>
        /// 是否公测
        /// </summary>
        public bool beta { get; set; }

        /// <summary>
        /// 强制更新
        /// </summary>
        public bool forced { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public bool current { get; set; }

        /// <summary>
        /// 应用路径（首页、下载等）
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 更新事项
        /// </summary>
        public string remark { get; set; }

        public string GetVer()
        {
            return $"{major}.{minor}.{patch}";
        }

        public bool IsMatch(ScmVerInfo info)
        {
            if (info == null)
            {
                return false;
            }

            if (info.major > major)
            {
                return true;
            }
            if (info.major < major)
            {
                return false;
            }

            if (info.minor > minor)
            {
                return true;
            }
            if (info.minor < minor)
            {
                return false;
            }

            return info.patch > patch;
        }

        public bool IsMatch(string newVer)
        {
            return IsMatch(ver, newVer);
        }

        public static bool IsMatch(string oldVer, string newVer)
        {
            if (string.IsNullOrEmpty(oldVer) || string.IsNullOrEmpty(newVer))
            {
                return false;
            }

            var pattern = @"^\d{1,6}(\.\d{1,6}){2}$";
            if (!Regex.IsMatch(oldVer, pattern) || !Regex.IsMatch(newVer, pattern))
            {
                return false;
            }

            var oldArr = oldVer.Split('.');
            var newArr = newVer.Split('.');

            for (var i = 0; i < oldArr.Length; i++)
            {
                var oldTxt = oldArr[i];
                var newTxt = newArr[i];

                var oldInt = int.Parse(oldTxt);
                var newInt = int.Parse(newTxt);

                if (oldInt > newInt)
                {
                    return false;
                }
                if (oldInt < newInt)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
