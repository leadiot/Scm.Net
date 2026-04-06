using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Notes.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NoteDvo : ScmDataDvo
    {
        /// <summary>
        /// 键
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public NoteTypesEnum types { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 
        /// </summary>
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
        /// 
        /// </summary>
        public string summary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public int ver { get; set; }
    }
}
