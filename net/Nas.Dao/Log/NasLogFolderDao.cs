using Com.Scm.Dao.User;
using SqlSugar;

namespace Com.Scm.Nas.Log
{
    [SugarTable("nas_log_folder")]
    public class NasLogFolderDao : ScmUserDataDao
    {
        public long folder_id { get; set; }


        public long log_id { get; set; }
    }
}
