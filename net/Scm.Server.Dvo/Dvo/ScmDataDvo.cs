using Com.Scm.Enums;
using Com.Scm.Utils;

namespace Com.Scm.Dvo
{
    /// <summary>
    /// 从服务端返回数据对象
    /// </summary>
    public class ScmDataDvo : ScmDvo
    {
        public ScmRowStatusEnum row_status { get; set; }

        public long update_time { get; set; }
        public long update_user { get; set; }

        public long create_time { get; set; }
        public long create_user { get; set; }

        public string update_times { get { return TimeUtils.FormatDataTime(update_time); } }
        public string update_names { get; set; }
        public string create_times { get { return TimeUtils.FormatDataTime(create_time); } }
        public string create_names { get; set; }
    }
}
