using Com.Scm.Dao;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Service
{
    /// <summary>
    /// 
    /// </summary>
    [SqlSugar.SugarTable("scm_res_service")]
    public class ScmResServiceDao : ScmDataDao, IResDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(16)]
        public string codes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(64)]
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        public string barcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(1024)]
        public string description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<ScmResServiceImageDao> images { get; set; }
    }
}
