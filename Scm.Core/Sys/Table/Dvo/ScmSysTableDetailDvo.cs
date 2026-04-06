using Com.Scm.Dvo;

namespace Com.Scm.Sys.Table.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysTableDetailDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 显示标题
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// 数据字段
        /// </summary>
        public string prop { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool hide { get; set; }

        /// <summary>
        /// 列宽
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 对齐
        /// </summary>
        public string align { get; set; }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool sortable { get; set; }
    }
}