using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Feedback
{
    /// <summary>
    /// 
    /// </summary>
    public class FeedbackDetailDto : ScmDataDto
    {
        /// <summary>
        /// 反馈ID
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [StringLength(1024)]
        public string content { get; set; }
    }
}