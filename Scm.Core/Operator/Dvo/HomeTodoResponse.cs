using Com.Scm.Response;

namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeTodoResponse : ScmApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, List<HomeTodo>> items { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HomeTodo
    {
        /// <summary>
        /// 
        /// </summary>
        public string types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int handle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string create_names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long create_time { get; set; }
    }
}
