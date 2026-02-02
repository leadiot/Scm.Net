using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// 更新明细
    /// </summary>
    [SugarTable("scm_sys_ver_detail")]
    public class ScmDevVerDetailDao : ScmDataDao
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
        [SugarColumn(Length = 256, IsNullable = true)]
        public string content { get; set; }
    }
}
