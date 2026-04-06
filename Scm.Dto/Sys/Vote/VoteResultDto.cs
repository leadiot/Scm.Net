using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Vote
{
    public class VoteResultDto : ScmDataDto
    {
        /// <summary>
        /// 投票编号
        /// </summary>
        [Required]
        public long header_id { get; set; }

        /// <summary>
        /// 投票项编号
        /// </summary>
        [Required]
        public long detail_id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        public long user_id { get; set; }
    }
}
