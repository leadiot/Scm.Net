using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmResProductDto : ScmDataDto
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
        /// 默认图像
        /// </summary>
        [StringLength(32)]
        public string image { get; set; }

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
        public List<ScmResProductImageDto> images { get; set; }
    }
}
