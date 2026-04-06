using Com.Scm.Request;

namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class DateThemeRequest : ScmRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDate()
        {
            if (date != null)
            {
                return date;
            }

            return DateTime.Now.ToString(ScmEnv.FORMAT_DATE);
        }
    }
}
