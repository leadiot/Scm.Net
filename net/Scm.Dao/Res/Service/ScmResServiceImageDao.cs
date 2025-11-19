using Com.Scm.Dao;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Service
{
    /// <summary>
    /// 
    /// </summary>
    [SqlSugar.SugarTable("scm_res_service_image")]
    public class ScmResServiceImageDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long spu_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        public string path { get; set; }
    }
}
