using Com.Scm.Dao;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Config
{
    /// <summary>
    /// 
    /// </summary>
    [SqlSugar.SugarTable("scm_sys_config_cat")]
    public class ConfigCatDao : ScmDataDao, ISystemDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
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
