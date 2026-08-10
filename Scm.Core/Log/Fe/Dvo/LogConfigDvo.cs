using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Log.Fe.Dvo
{
    public class LogConfigDvo : ScmDvo
    {
        public ScmLogLevelEnum LogLevel { get; set; }

        public bool LogNotify { get; set; }

        public bool LogReport { get; set; }
    }
}
