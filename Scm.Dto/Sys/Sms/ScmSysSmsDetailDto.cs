using Com.Scm.Dto;
using Com.Scm.Enums;
using Com.Scm.Utils;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    public class ScmSysSmsDetailDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long head_id { get; set; }

        [StringLength(32)]
        /// <summary>
        /// 电话号码
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [StringLength(32)]
        public string phone { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

        [Required]
        [StringLength(256)]
        /// <summary>
        /// 短信内容
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 发送日期
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 短信类型
        /// </summary>
        public ScmSmsTypeEnum type { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int color { get; set; }

        /// <summary>
        /// 操作系统相关参数
        /// </summary>
        public Dictionary<string, string> os_params { get; set; }

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
