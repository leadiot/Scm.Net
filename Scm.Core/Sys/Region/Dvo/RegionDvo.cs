using Com.Scm.Dvo;

namespace Com.Scm.Sys.Region.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class RegionDvo : ScmDvo
    {
        /// <summary>
        /// 城市编号
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string namef { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string postcode { get; set; }

        /// <summary>
        /// 所属上级
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int lv { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string lng { get; set; }

        /// <summary>
        /// 维度
        /// </summary>
        public string lat { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int od { get; set; }
    }
}
