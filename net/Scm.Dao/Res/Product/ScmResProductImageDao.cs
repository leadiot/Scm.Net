using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Product
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_res_product_image")]
    public class ScmResProductImageDao : ScmDataDao
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
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string path { get; set; }
    }
}
