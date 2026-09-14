using Com.Scm.Dao;
using Com.Scm.Dao.Sync;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    /// <summary>
    /// 短信
    /// </summary>
    [SugarTable("scm_sys_sms")]
    public class ScmSysSmsDao : ScmUserDataDao, IDeleteDao, ISyncDao
    {
        public const string FOLDER_NAME = "sms";

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
        [SugarColumn(Length = 64)]
        public string address { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string name { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string body { get; set; }

        /// <summary>
        /// 外部文件数量
        /// </summary>
        public int files { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
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
        /// 来源应用
        /// </summary>
        [SugarColumn(Length = 16, IsNullable = true)]
        public string source { get; set; }

        /// <summary>
        /// 来源终端
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 来源应用ID
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string res_id { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 其它附加参数
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public Dictionary<string, string> os_params { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScmRowDeleteEnum row_delete { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long sync_time { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            if (!ScmUtils.IsNormalId(terminal_id))
            {
                terminal_id = ScmEnv.DEFAULT_ID;
            }

            CheckNotNull();
        }

        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            CheckNotNull();
        }

        private void CheckNotNull()
        {
            sync_time = update_time;

            if (name == null) name = "";
            if (body == null) body = "";
            if (subject == null) subject = "";
            if (source == null) source = "";

            if (body.Length > 256)
            {
                body = body.Substring(0, 256);
                files = 1;
            }
            else
            {
                files = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return id + ".txt";
        }
    }
}
