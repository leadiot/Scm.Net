using Com.Scm.Enums;

namespace Com.Scm.Dto
{
    public class ScmDataDto : ScmDto
    {
        public ScmRowStatusEnum row_status { get; set; }

        public long create_user { get; set; }
        public string create_names { get; set; }
        public long create_time { get; set; }

        public long update_user { get; set; }
        public string update_names { get; set; }
        public long update_time { get; set; }

        //public bool enabled { get { return row_status == ScmStatusEnum.Enabled; } }
    }
}