using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Log
{
    [SugarTable("nas_log_folder")]
    public class NasLogFolderDao : ScmUserDataDao
    {
        [Required]
        public long folder_id { get; set; }

        [Required]
        public long log_id { get; set; }
    }
}
