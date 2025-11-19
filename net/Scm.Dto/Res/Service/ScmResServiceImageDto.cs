using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmResServiceImageDto : ScmDataDto
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
