using Com.Scm.Enums;
using Com.Scm.Request;

namespace Com.Scm
{
    /// <summary>
    /// 公共查询参数
    /// </summary>
    public class ScmSearchRequest : ScmRequest
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 状态值  1=true  2=false  0=默认
        /// </summary>
        public ScmRowStatusEnum row_status { get; set; } = ScmRowStatusEnum.Enabled;

        /// <summary>
        /// 时间，支持区间
        /// </summary>
        public string times { get; set; }

        /// <summary>
        /// 高级查询Json，支持and or 等
        /// </summary>
        public string query { get; set; }

        public bool IsAllStatus()
        {
            return row_status == ScmRowStatusEnum.Normal;
        }

        public bool IsZeroId()
        {
            return id == 0;
        }
    }
}
