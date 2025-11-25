using Com.Scm.Dao;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Ver
{
    /// <summary>
    /// 更新明细
    /// </summary>
    [SqlSugar.SugarTable("scm_sys_ver_detail")]
    public class ScmSysVerDetailDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long ver_id { get; set; }

        /// <summary>
        /// 升级类型
        /// </summary>
        public ScmVerDetailTypesEnum types { get; set; }

        /// <summary>
        /// 升级事项
        /// </summary>
        [StringLength(256)]
        public string content { get; set; }
    }
}
