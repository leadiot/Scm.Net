namespace Com.Scm
{
    public partial class ScmEnv
    {
        /// <summary>
        /// 应用代码
        /// </summary>
        public const string APP_CODE = "Scm.Net";

        /// <summary>
        /// 发行
        /// </summary>
        public const string ISSUER = "Scm.Net";

        /// <summary>
        /// 受众
        /// </summary>
        public const string AUDIENCE = "scm.net";

        /// <summary>
        /// 默认日期格式
        /// </summary>
        public const string FORMAT_DATE = "yyyy-MM-dd";
        /// <summary>
        /// 默认时间格式
        /// </summary>
        public const string FORMAT_TIME = "HH:mm:ss";
        /// <summary>
        /// 默认日期时间格式
        /// </summary>
        public static readonly string FORMAT_DATETIME = FORMAT_DATE + ' ' + FORMAT_TIME;

        /// <summary>
        /// 默认索引
        /// </summary>
        public const long DEFAULT_ID = 1000000000000000001L;

        /// <summary>
        /// 文件大小
        /// </summary>
        public const long MAX_FILE_SIZE = 10L * 1024 * 1024;

        /// <summary>
        /// 默认密码
        /// </summary>
        public const string DEFAULT_PASS = "123456";

        /// <summary>
        /// 跨域配置
        /// </summary>
        public const string CORS_NAME = "ScmCors";

        /// <summary>
        /// 默认字体名称常量，用于指定文本渲染时的字体。
        /// </summary>
        /// <remarks>编译时常量，应在整个代码库中保持不变以保证一致性。</remarks>
        public const string FONT_NAME = "Open Sans";

        /// <summary>
        /// 网络路径分隔符
        /// </summary>
        public static readonly char WebSeparator = '/';

        /// <summary>
        /// 系统路径分隔符
        /// </summary>
        public static readonly char DirSeparator = System.IO.Path.DirectorySeparatorChar;
    }
}
