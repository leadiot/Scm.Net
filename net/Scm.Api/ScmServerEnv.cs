namespace Com.Scm.Api
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
        public const int MINOR = 10;

        /// <summary>
        /// 修订号
        /// </summary>
        public const int PATCH = 20;

        /// <summary>
        /// 构建号
        /// </summary>
        public const string BUILD = "2026033001";

        /// <summary>
        /// 发行日期
        /// </summary>
        public const string RELEASE_DATE = "2026-03-30";

        public static readonly string VERSION = $"{MAJOR}.{MINOR}.{PATCH}";
    }
}
