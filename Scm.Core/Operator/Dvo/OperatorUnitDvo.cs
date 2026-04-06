namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class OperatorUnitDvo
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
        /// 
        /// </summary>
        public long prov_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long city_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long area_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long town_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string street { get; set; }

        public void LoadDef()
        {
            codec = "Demo";
            namec = "演示机构";
            names = "演示机构有限公司";
        }
    }
}
