namespace Com.Scm
{
    public class ScmServerEnv
    {
        /// <summary>
        /// 主版本号
        /// </summary>
        public const int MAJOR = 10;

        /// <summary>
        /// 次版本号
        /// </summary>
        public const int MINOR = 11;

        /// <summary>
        /// 修订号
        /// </summary>
        public const int PATCH = 21;

        /// <summary>
        /// 构建号
        /// </summary>
        public const string BUILD = "2026040601";

        /// <summary>
        /// 发行日期
        /// </summary>
        public const string RELEASE_DATE = "2026-04-06";

        public static readonly string VERSION = $"{MAJOR}.{MINOR}.{PATCH}";
    }
}
