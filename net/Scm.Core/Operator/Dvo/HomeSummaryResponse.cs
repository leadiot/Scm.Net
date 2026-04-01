using Com.Scm.Response;

namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeSummaryResponse : ScmApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<HomeSummary> summaries { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HomeSummary
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string tooltip { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// 增长信息
        /// </summary>
        public HomeSummaryRate rate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HomeSummaryRate
    {
        /// <summary>
        /// 显示标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 显示数值（比例）
        /// </summary>
        public int value { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string units { get; set; }
    }
}
