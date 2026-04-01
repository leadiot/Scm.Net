using Com.Scm.Enums;
using Com.Scm.Request;
using System.Collections.Generic;

namespace Com.Scm
{
    public class ScmChangeStatusRequest : ScmRequest
    {
        public List<long> ids { get; set; }

        public ScmRowStatusEnum status { get; set; }
    }
}
