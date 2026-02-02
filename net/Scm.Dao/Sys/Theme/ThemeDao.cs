using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Theme
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_theme")]
    public class ThemeDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string theme { get; set; }
    }
}
