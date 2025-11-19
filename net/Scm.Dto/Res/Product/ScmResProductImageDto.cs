using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmResProductImageDto : ScmDataDto
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
