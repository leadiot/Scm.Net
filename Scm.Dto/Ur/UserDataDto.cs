using Com.Scm.Dto;
using Com.Scm.Enums;

namespace Com.Scm.Ur
{
    public class UserDataDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmUserDataTypesEnum types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long data_id { get; set; }
    }
}
