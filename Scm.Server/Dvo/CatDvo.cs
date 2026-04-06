namespace Com.Scm.Dvo
{
    /// <summary>
    /// 类别
    /// </summary>
    public class CatDvo : ScmDataDvo
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public long app { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int lv { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string image { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long pid { get { return _pid; } set { _pid = value; } }
        private long _pid;

        /// <summary>
        /// 顶级ID
        /// </summary>
        public long tid { get; set; }

        /// <summary>
        /// 引用数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 引用ID
        /// </summary>
        public long ref_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string uri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ParentId { get { return _pid; } set { _pid = value; } }

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
    }
}
