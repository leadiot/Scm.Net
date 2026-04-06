using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Product
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_res_product")]
    public class ScmResProductDao : ScmDataDao, IResDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string namec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 默认图像
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string barcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<ScmResProductImageDao> images { get; set; }

        public string GetCode()
        {
            return codes;
        }

        public string GetName()
        {
            return names ?? namec;
        }

        public string GetNames()
        {
            return names;
        }

        public string GetNamec()
        {
            return namec;
        }
    }
}
