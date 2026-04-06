using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Vote
{
    public class VoteDetailDto : ScmDataDto
    {
        /// <summary>
        /// 投票编号
        /// </summary>
        [Required]
        public long header_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(128)]
        public string title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        public string remark { get; set; }

        /// <summary>
        /// 投票数量
        /// </summary>
        public int count { get; set; }
    }
}
