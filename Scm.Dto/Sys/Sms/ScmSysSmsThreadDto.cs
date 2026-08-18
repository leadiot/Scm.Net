using Com.Scm.Dto;
using Com.Scm.Utils;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    public class ScmSysSmsThreadDto : ScmDataDto
    {
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(64)]
        public string address { get; set; }

        /// <summary>
        /// 会话名称
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

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
