using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Contacts
{
    public class ScmSysContactsDto : ScmDataDto
    {
        /// <summary>
        /// 联系人全名（DISPLAY_NAME）
        /// </summary>
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(64)]
        public string nickname { get; set; }

        /// <summary>
        /// 姓名：名
        /// </summary>
        [StringLength(64)]
        public string first_name { get; set; }

        /// <summary>
        /// 姓名：中间名
        /// </summary>
        [StringLength(64)]
        public string middle_name { get; set; }

        /// <summary>
        /// 姓名：姓
        /// </summary>
        [StringLength(64)]
        public string last_name { get; set; }

        /// <summary>
        /// 姓名前缀（Dr/Prof 等）
        /// </summary>
        [StringLength(16)]
        public string name_prefix { get; set; }

        /// <summary>
        /// 姓名后缀（Jr/Sr 等）
        /// </summary>
        [StringLength(16)]
        public string name_suffix { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        [StringLength(128)]
        public string company { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [StringLength(128)]
        public string department { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [StringLength(128)]
        public string title { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        [StringLength(512)]
        public string website { get; set; }

        /// <summary>
        /// 头像大图 URI
        /// </summary>
        [StringLength(512)]
        public string photo_uri { get; set; }

        /// <summary>
        /// 头像缩略图 URI
        /// </summary>
        [StringLength(512)]
        public string photo_thumb_uri { get; set; }

        /// <summary>
        /// 电话列表（JSON 序列化：List<Map<String,String>> phones）
        /// </summary>
        public List<Dictionary<string, string>> phones { get; set; }

        /// <summary>
        /// 邮箱列表（JSON 序列化）
        /// </summary>
        public List<Dictionary<string, string>> emails { get; set; }

        /// <summary>
        /// 日期列表（生日/农历生日/纪念日等，JSON 序列化）
        /// </summary>
        public List<Dictionary<string, string>> dates { get; set; }

        /// <summary>
        /// 地址列表（JSON 序列化）
        /// </summary>
        public List<Dictionary<string, string>> addresses { get; set; }

        /// <summary>
        /// 即时消息列表（JSON 序列化）
        /// </summary>
        public List<Dictionary<string, string>> im_addresses { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        public string note { get; set; }
    }
}
