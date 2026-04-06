using Com.Scm.Dvo;

namespace Com.Scm.Dev.Sql.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public long db_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string sql { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int qty { get; set; }
    }
}