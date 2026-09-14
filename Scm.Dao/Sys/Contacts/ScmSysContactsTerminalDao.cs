using Com.Scm.Dao.Sync;
using Com.Scm.Dao.Terminal;
using SqlSugar;

namespace Com.Scm.Nas.App
{
    /// <summary>
    /// 设备通讯录表
    /// </summary>
    [SugarTable("scm_sys_contacts_terminal")]
    public class ScmSysContactsTerminalDao : ScmTerminalDataDao, ISyncDao
    {
        /// <summary>
        /// 通讯录ID
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
