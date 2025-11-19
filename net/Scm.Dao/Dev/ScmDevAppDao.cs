using Com.Scm.Dao;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// App信息
    /// </summary>
    [SqlSugar.SugarTable("scm_sys_app")]
    public class ScmDevAppDao : ScmDataDao
    {
        /// <summary>
        /// 应用类型
        /// </summary>
        [Required]
        public int types { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        [StringLength(16)]
        public string code { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

        /// <summary>
        /// 应用简介
        /// </summary>
        [StringLength(1024)]
        public string content { get; set; }
    }
}
