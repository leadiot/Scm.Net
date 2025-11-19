using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// SQL
    /// </summary>
    [SugarTable("scm_dev_sql")]
    public class ScmDevSqlDao : ScmUserDataDao
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public long db_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(1024)]
        public string sql { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int qty { get; set; }
    }
}