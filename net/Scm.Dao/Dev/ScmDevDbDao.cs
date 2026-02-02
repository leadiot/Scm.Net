using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// 数据库
    /// </summary>
    [SugarTable("scm_dev_db")]
    public class ScmDevDbDao : ScmUserDataDao
    {
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string namec { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int port { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string schame { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string user { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string pass { get; set; }

        /// <summary>
        /// 字符集
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string charset { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
    }
}