namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitBasicRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 机构代码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 机构全称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 机构简称
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string contact { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string cellphone { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string logo { get; set; }
    }
}
