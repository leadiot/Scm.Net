using Com.Scm.Dvo;

namespace Com.Scm.Dev.Uid.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmUidDvo : ScmDvo
    {
        /// <summary>
        /// 键
        /// </summary>
        public string k { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public long v { get; set; }

        /// <summary>
        /// 缓存大小
        /// </summary>
        public int c { get; set; } = 1;

        /// <summary>
        /// 缓冲大小
        /// </summary>
        public int b { get; set; } = 0;

        /// <summary>
        /// 数值长度
        /// </summary>
        public int l { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public long t { get; set; }

        /// <summary>
        /// 前置掩码
        /// </summary>
        public string m { get; set; }
        /// <summary>
        /// 后置掩码
        /// </summary>
        public string p { get; set; }
    }
}