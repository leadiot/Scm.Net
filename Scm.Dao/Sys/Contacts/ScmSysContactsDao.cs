using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Contacts
{
    [SugarTable("scm_sys_contacts")]
    public class ScmSysContactsDao : ScmUserDataDao
    {
        /// <summary>
        /// 联系人全名（DISPLAY_NAME）
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string nickname { get; set; }

        /// <summary>
        /// 姓名：名
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string first_name { get; set; }

        /// <summary>
        /// 姓名：中间名
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string middle_name { get; set; }

        /// <summary>
        /// 姓名：姓
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string last_name { get; set; }

        /// <summary>
        /// 姓名前缀（Dr/Prof 等）
        /// </summary>
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string name_prefix { get; set; }

        /// <summary>
        /// 姓名后缀（Jr/Sr 等）
        /// </summary>
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string name_suffix { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string company { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string department { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string title { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        [StringLength(512)]
        [SugarColumn(Length = 512, IsNullable = true)]
        public string website { get; set; }

        /// <summary>
        /// 头像大图 URI
        /// </summary>
        [StringLength(512)]
        [SugarColumn(Length = 512, IsNullable = true)]
        public string photo_uri { get; set; }

        /// <summary>
        /// 头像缩略图 URI
        /// </summary>
        [StringLength(512)]
        [SugarColumn(Length = 512, IsNullable = true)]
        public string photo_thumb_uri { get; set; }

        /// <summary>
        /// 电话列表（JSON 序列化：List<Map<String,String>> phones）
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<Dictionary<string, string>> phones { get; set; }

        /// <summary>
        /// 邮箱列表（JSON 序列化）
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<Dictionary<string, string>> emails { get; set; }

        /// <summary>
        /// 日期列表（生日/农历生日/纪念日等，JSON 序列化）
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<Dictionary<string, string>> dates { get; set; }

        /// <summary>
        /// 地址列表（JSON 序列化）
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<Dictionary<string, string>> addresses { get; set; }

        /// <summary>
        /// 即时消息列表（JSON 序列化）
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<Dictionary<string, string>> im_addresses { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string note { get; set; }

        /// <summary>
        /// 来源应用
        /// </summary>
        [SugarColumn(Length = 16, IsNullable = true)]
        public string source { get; set; }

        /// <summary>
        /// 来源终端（最初是由哪个终端创建的）
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public ScmRowDeleteEnum row_delete { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            CheckNotNull();
        }

        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            CheckNotNull();
        }

        private void CheckNotNull()
        {
            if (dates == null) dates = new List<Dictionary<string, string>>();
            if (phones == null) phones = new List<Dictionary<string, string>>();
            if (emails == null) emails = new List<Dictionary<string, string>>();
            if (addresses == null) addresses = new List<Dictionary<string, string>>();
            if (im_addresses == null) im_addresses = new List<Dictionary<string, string>>();
            if (source == null) source = "";
        }
    }
}