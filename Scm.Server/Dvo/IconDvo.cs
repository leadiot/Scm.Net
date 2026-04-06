namespace Com.Scm.Dvo
{
    public class IconDvo : ScmDvo
    {
        /// <summary>
        /// 图标集合，vue,sc,ms
        /// </summary>
        public string set { get; set; }
        public long set_id { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string cat { get; set; }
        public long cat_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string desc { get; set; }

        /// <summary>
        /// 笔画类型，both,line,fill
        /// </summary>
        public string type { get; set; }
    }
}
