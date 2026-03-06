using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Cfg
{
    /// <summary>
    /// 驱动
    /// </summary>
    [SugarTable("nas_cfg_folder")]
    public class NasCfgFolderDao : ScmUserDataDao
    {
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
        [SugarColumn(Length = 256)]
        public string name { get; set; }

        /// <summary>
        /// 远端节点
        /// </summary>
        public NasNodeEnums node { get; set; }

        /// <summary>
        /// 远端路径
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string path { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
        public long res_id { get; set; }
    }
}