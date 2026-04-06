using Com.Scm.Dvo;

namespace Com.Scm.Ur.Position.Dvo
{
    public class PositionDvo : ScmDataDvo
    {
        /// <summary>
        /// 岗位编码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int od { get; set; } = 1;

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; }
    }
}
