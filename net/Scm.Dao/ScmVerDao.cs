using Com.Scm.Dao;

namespace Com.Scm
{
    [SqlSugar.SugarTable("scm_ver")]
    public class ScmVerDao : ScmDao
    {
        public const int VER_MAJOR = 10;
        public const int VER_MINOR = 0;
        public const int VER_PATCH = 0;
        public const int VER_BUILD = 0;

        /// <summary>
        /// 关键字
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 主版本
        /// </summary>
        public int major { get; set; }
        /// <summary>
        /// 子版本
        /// </summary>
        public int minor { get; set; }
        /// <summary>
        /// 修正版本
        /// </summary>
        public int patch { get; set; }
        /// <summary>
        /// 构建版本
        /// </summary>
        public int build { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public long update_time { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }
    }
}
