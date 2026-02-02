using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Config
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_config_cat")]
    public class ConfigCatDao : ScmDataDao, ISystemDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
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
        public ScmRowSystemEnum row_system { get; set; }
    }
}
