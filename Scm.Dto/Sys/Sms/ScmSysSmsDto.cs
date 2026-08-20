using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    public class ScmSysSmsDto : ScmDataDto
    {
        /// <summary>
        /// 会话 ID
        /// </summary>
        public long thread_id { get; set; }

        /// <summary>
        /// 短信类型
        /// </summary>
        public ScmSmsTypeEnum type { get; set; }

        /// <summary>
        /// 短信协议
        /// </summary>
        public ScmSmsProtocolEnum protocol { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [StringLength(64)]
        public string address { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

        /// <summary>
        /// 短信内容，此处不做限制，超长则存入文件
        /// </summary>
        [Required]
        public string body { get; set; }

        /// <summary>
        /// 外部文件数量
        /// </summary>
        public int files { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [StringLength(128)]
        public string subject { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int color { get; set; }

        /// <summary>
        /// 接收/发送时间戳（毫秒）
        /// </summary>
        public long date { get; set; }

        /// <summary>
        /// 送达日期
        /// </summary>
        public long delivery_date { get; set; }

        /// <summary>
        /// 是否已读：0 未读 1 已读
        /// </summary>
        public ScmSmsReadEnum read { get; set; }

        /// <summary>
        /// 联系人ID
        /// </summary>
        public long contacts_id { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 来源应用
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 来源终端
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScmRowDeleteEnum row_delete { get; set; }
    }
}
