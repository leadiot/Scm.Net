using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Cfg
{
    /// <summary>
    /// 驱动
    /// </summary>
    [SugarTable("nas_cfg_folder")]
    public class SyncCfgFolderDao : ScmDataDao
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public long user_id { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        [Required]
        public long terminal_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        /// <summary>
        /// 远端路径
        /// </summary>
        [StringLength(2048)]
        public string path { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
        public long res_id { get; set; }
    }
}