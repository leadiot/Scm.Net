using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Notes
{
    /// <summary>
    /// 记事功能
    /// </summary>
    [SugarTable("scm_sys_note")]
    public class NoteDao : ScmUserDataDao
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
        /// 文章类型
        /// </summary>
        public NoteTypesEnum types { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [Required]
        public long cat_id { get; set; }

        /// <summary>
        /// 标题
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
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string summary { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [StringLength(2048)]
        [SugarColumn(Length = 2048)]
        public string content { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        [Required]
        public int files { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int ver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            this.salt = new Random().Next(10000).ToString("d4");
            this.key = this.id + this.salt;
            this.ver = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            this.ver += 1;
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
