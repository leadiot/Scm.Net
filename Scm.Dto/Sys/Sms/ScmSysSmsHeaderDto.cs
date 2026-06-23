using Com.Scm.Dto;
using Com.Scm.Utils;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    public class ScmSysSmsHeaderDto : ScmDataDto
    {
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(32)]
        public string phone { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        [Required]
        [StringLength(256)]
        public string body { get; set; }

        /// <summary>
        /// 发送日期
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int color { get; set; }

        public string colors
        {
            get
            {
                return TextUtils.FormatColor(color);
            }
            set
            {
                color = TextUtils.ParseColor(value);
            }
        }
    }
}
