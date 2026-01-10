using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Adm.Config
{
    /// <summary>
    /// 
    /// </summary>
    [SqlSugar.SugarTable("scm_sys_config_cat")]
    public class AdmConfigCatDao : ScmDataDao, ISystemDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string namec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int od { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmSystemEnum row_system { get; set; } = ScmSystemEnum.No;
    }
}
