using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm
{
    [SugarTable("scm_test")]
    public class ScmTestDao : ScmDao
    {
        [StringLength(32)]
        public string key { get; set; }

        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string value { get; set; }

        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string remark { get; set; }
    }
}
