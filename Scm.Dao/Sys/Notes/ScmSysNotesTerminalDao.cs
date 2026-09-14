using Com.Scm.Dao.Sync;
using Com.Scm.Dao.Terminal;
using SqlSugar;

namespace Com.Scm.Nas.App
{
    /// <summary>
    /// 设备便签表
    /// </summary>
    [SugarTable("scm_sys_notes_terminal")]
    public class ScmSysNotesTerminalDao : ScmTerminalDataDao, ISyncDao
    {
        /// <summary>
        /// 便签ID
        /// </summary>
        public long sys_id { get; set; }

        /// <summary>
        /// 来源应用ID
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string res_id { get; set; }

        /// <summary>
        /// 其它附加参数
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public Dictionary<string, string> os_params { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long sync_time { get; set; }
    }
}
