using Com.Scm.Response;

namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeReportResponse : ScmApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> Titles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<float> Data1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<float> Data2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<float> Data3 { get; set; }
    }
}
