using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Sys.Calendar
{
    /// <summary>
    /// 参与人列表
    /// </summary>
    [SugarTable("scm_sys_calendar_user")]
    public class CalendarUserDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long calendar_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }
    }
}
