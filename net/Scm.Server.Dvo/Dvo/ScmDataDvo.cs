using Com.Scm.Enums;

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
        public string update_names { get; set; }

        public long create_time { get; set; }
        public long create_user { get; set; }
        public string create_names { get; set; }
    }
}
