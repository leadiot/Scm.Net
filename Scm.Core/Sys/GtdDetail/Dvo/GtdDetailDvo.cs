using Com.Scm.Dvo;
using Com.Scm.Sys.Enums;

namespace Com.Scm.Sys.GtdDetail.Dvo
{
    public class GtdDetailDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public ScmGtdPriorityEnum priority { get; set; }

        /// <summary>
        /// 提醒标识
        /// </summary>
        public ScmGtdRemindEnum remind { get; set; }

        /// <summary>
        /// 提示方式
        /// </summary>
        public ScmGtdNoticeEnum notice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScmGtdHandleEnum handle { get; set; }
    }
}
