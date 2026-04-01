using Com.Scm.Response;

namespace Com.Scm.Dev.Sql.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecuteResponse : ScmApiDataResponse<System.Data.DataTable>
    {
        /// <summary>
        /// 执行类型
        /// </summary>
        public ExecuteTypeEnum type { get; set; }

        /// <summary>
        /// 执行成功数量（语句）
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPages { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long TotalItems { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ExecuteTypeEnum
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Select = 1,
        /// <summary>
        /// 
        /// </summary>
        Insert = 2,
        /// <summary>
        /// 
        /// </summary>
        Update = 3,
        /// <summary>
        /// 
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 
        /// </summary>
        Execute = 5
    }
}
