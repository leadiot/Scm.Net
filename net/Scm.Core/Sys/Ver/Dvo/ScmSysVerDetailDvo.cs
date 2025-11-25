using Com.Scm.Dvo;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Ver.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysVerDetailDvo : ScmDataDvo
    {
        /// <summary>
        /// 升级类型
        /// </summary>
        public ScmVerDetailTypesEnum types { get; set; }

        /// <summary>
        /// 升级事项
        /// </summary>
        [StringLength(256)]
        public string content { get; set; }
    }
}
