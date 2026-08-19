using Com.Scm.Dao;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Notes
{
    /// <summary>
    /// 记事功能
    /// </summary>
    [SugarTable("scm_sys_notes")]
    public class ScmSysNotesDao : ScmUserDataDao, IDeleteDao
    {
        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string key { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        [Required]
        [StringLength(4)]
        [SugarColumn(Length = 4)]
        public string salt { get; set; }

        /// <summary>
        /// 来源平台
        /// </summary>
        public ScmClientTypeEnum client { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public NoteTypesEnum types { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [Required]
        public long cat_id { get; set; }

        /// <summary>
        /// 主标题
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string title { get; set; }

        /// <summary>
        /// 子标题
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string sub_title { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int qty { get; set; }

        /// <summary>
        /// 收藏数量
        /// </summary>
        [Required]
        public int fav_qty { get; set; }

        /// <summary>
        /// 留言数量
        /// </summary>
        [Required]
        public int msg_qty { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Required]
        [StringLength(512)]
        [SugarColumn(Length = 512)]
        public string summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(2048)]
        [SugarColumn(Length = 2048)]
        public string content { get; set; }

        /// <summary>
        /// 文件数量（用于判断是否需要读取指定目录的文件数据）
        /// </summary>
        [Required]
        public int files { get; set; }

        /// <summary>
        /// 便签颜色
        /// </summary>
        public int color { get; set; }

        /// <summary>
        /// 修改时间（内部或外部资源的实际修改时间，与update_time不同）
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 来源应用
        /// </summary>
        [SugarColumn(Length = 16, IsNullable = true)]
        public string source { get; set; }

        /// <summary>
        /// 来源终端（首次来源终端）
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public ScmRowDeleteEnum row_delete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            this.salt = new Random().Next(10000).ToString("d4");
            this.key = this.id + this.salt;

            CheckNotNull();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            CheckNotNull();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return id + ".txt";
        }

        public void CheckNotNull()
        {
            if (sub_title == null) sub_title = "";
            if (summary == null) summary = "";
            if (content == null) content = "";
            if (source == null) source = "";

            if (content.Length > 2048)
            {
                content = content.Substring(0, 2048);
                files = 1;
            }
            else
            {
                files = 0;
            }
        }
    }
}
