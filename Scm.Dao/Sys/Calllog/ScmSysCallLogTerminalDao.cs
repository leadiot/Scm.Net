using Com.Scm.Dao.Sync;
using Com.Scm.Dao.Terminal;
using SqlSugar;

namespace Com.Scm.Nas.App
{
    [SugarTable("scm_sys_calllog_terminal")]
    public class ScmSysCallLogTerminalDao : ScmTerminalDataDao, ISyncDao
    {
        public string number { get; set; }
        public string name { get; set; }
        public long date { get; set; }
        public int type { get; set; }
        public long duration { get; set; }

        public long sync_time { get; set; }
    }
}
