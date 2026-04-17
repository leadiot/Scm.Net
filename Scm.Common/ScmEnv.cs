namespace Com.Scm
{
    public partial class ScmEnv
    {
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
        /// 默认密码
        /// </summary>
        public const string DEFAULT_PASS = "123456";
        /// <summary>
        /// 跨域配置
        /// </summary>
        public const string SCM_CORS = "ScmCors";

        public const string FONT_NAME = "DejaVu Sans";
    }
}
