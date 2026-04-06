using Com.Scm.Dvo;

namespace Com.Scm.Dev.Db.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class DbDvo : ScmDataDvo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public string schame { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 字符集
        /// </summary>
        public string charset { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
    }
}