namespace Com.Scm
{
    public class ScmServerEnv : ScmEnv
    {
        /// <summary>
        /// 发行日期
        /// TODO: （必需修改）发行版本，发布前需要修改此处
        /// </summary>
        public const string VER_DATE = "2026-04-18";

        /// <summary>
        /// 构建版本
        /// TODO: （必需修改）构建版本，发布前需要修改此处，格式为 YYYYMMDDXX，其中 XX 是当天的第几次构建
        /// </summary>
        public const string VER_CODE = "2026041801";

        /// <summary>
        /// 构建版本
        /// TODO: （必需修改）构建版本，发布前需要修改此处，格式为 X，其中 X 是一个整数，表示构建版本号，通常在每次构建时增加，即使没有功能更新或错误修复
        /// </summary>
        public const int BUILD = 47;

        /// <summary>
        /// 修正版本
        /// TODO: （可选修改）修正版本，发布前需要修改此处，格式为 X，其中 X 是一个整数，表示修正版本号，通常在有错误修复但没有新功能添加时增加
        /// </summary>
        public const int PATCH = 27;

        /// <summary>
        /// 次要版本
        /// TODO: （可选修改）次要版本，发布前需要修改此处，格式为 X，其中 X 是一个整数，表示次要版本号，通常在有新功能添加但保持向后兼容时增加
        /// </summary>
        public const int MINOR = 13;

        /// <summary>
        /// 主要版本
        /// TODO: （可选修改）主要版本，发布前需要修改此处，格式为 X，其中 X 是一个整数，表示主要版本号，通常在有重大功能更新或不兼容的 API 变更时增加
        /// </summary>
        public const int MAJOR = 10;

        /// <summary>
        /// 发行版本
        /// 格式：Scm.Net 10.13.25.45 (Build 2026041501)
        /// </summary>
        public static readonly string VER_INFO = $"{MAJOR}.{MINOR}.{PATCH}.{BUILD} @Build {VER_CODE}";

        public const long USER_ID = 1000000000000001030L;
    }
}
