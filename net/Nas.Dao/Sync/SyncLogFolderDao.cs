using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Sync
{
    [SugarTable("nas_log_folder")]
    public class SyncLogFolderDao : ScmDataDao
    {
        [Required]
        public long user_id { get; set; }

        [Required]
        public long folder_id { get; set; }

        [Required]
        public long log_id { get; set; }
    }
}
