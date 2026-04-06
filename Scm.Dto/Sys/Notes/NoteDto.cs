using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Notes
{
    public class NoteDto : ScmDataDto
    {
        public const long SYS_ID = 1000000000000000001;
        /// <summary>
        /// 
        /// </summary>
        public const string FOLDER_NAME = "notes";

        public const int SUMMARY_SIZE = 1024;
        public const int CONTENT_SIZE = 2048;

        /// <summary>
        /// 键
        /// </summary>
        [StringLength(32)]
        public string key { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public NoteTypesEnum types { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(256)]
        public string title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(256)]
        public string sub_title { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 收藏数量
        /// </summary>
        public int fav_qty { get; set; }

        /// <summary>
        /// 留言数量
        /// </summary>
        public int msg_qty { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public int files { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(1024)]
        public string summary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string content { get; set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public int ver { get; set; }

        public bool IsTooLong()
        {
            var tmp = this.content ?? "";
            return tmp.Length > NoteDto.CONTENT_SIZE;
        }

        public string ToDbSummary()
        {
            var tmp = this.content ?? "";
            if (tmp.Length > NoteDto.SUMMARY_SIZE)
            {
                tmp = tmp.Substring(0, NoteDto.SUMMARY_SIZE);
            }
            return tmp;
        }

        public string ToDbContent()
        {
            var tmp = this.content ?? "";
            if (tmp.Length > NoteDto.CONTENT_SIZE)
            {
                tmp = tmp.Substring(0, NoteDto.CONTENT_SIZE);
            }
            return tmp;
        }

        public static string GetFileName(long id)
        {
            return id + ".txt";
        }
    }
}