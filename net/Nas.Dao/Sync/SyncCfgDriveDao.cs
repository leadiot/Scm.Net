using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Cfg
{
    /// <summary>
    /// 驱动
    /// </summary>
    [SugarTable("nas_cfg_drive")]
    public class SyncCfgDriveDao : ScmUserDataDao
    {
        /// <summary>
        /// 终端ID
        /// </summary>
        [Required]
        public long terminal_id { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        [Required]
        public long folder_id { get; set; }

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
    }
}