using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Nas.Sync
{
    [SugarTable("nas_log_folder")]
    public class SyncLogFolderDao : ScmDataDao
    {
        public long user_id { get; set; }
        public long folder_id { get; set; }
        public long log_id { get; set; }
    }
}
