using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Adm.Config
{
    /// <summary>
    /// 系统配置
    /// </summary>
    [SqlSugar.SugarTable("scm_sys_config")]
    public class AdmConfigDao : ScmDataDao
    {
        /// <summary>
        /// 用户
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public ScmClientTypeEnum client { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string value { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public ScmDataTypeEnum data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string remark { get; set; }
    }
}
