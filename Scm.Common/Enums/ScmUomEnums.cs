namespace Com.Scm.Enums
{
    public enum ScmUomTypesEnum
    {
        None = 0,
        /// <summary>
        /// 基础单位
        /// </summary>
        Basic = 1,
        /// <summary>
        /// 复合单位
        /// </summary>
        Compose = 2
    }

    public enum ScmUomModesEnum
    {
        None = 0,
        /// <summary>
        /// 计数单位
        /// </summary>
        Numberic = 1,
        /// <summary>
        /// 长度单位
        /// </summary>
        Length = 2,
        /// <summary>
        /// 重量单位
        /// </summary>
        Weight = 3,
        /// <summary>
        /// 体积单位
        /// </summary>
        Volume = 4,
        /// <summary>
        /// 时间单位
        /// </summary>
        Time = 5,
        /// <summary>
        /// 币制单位
        /// </summary>
        Currency = 6
    }

    public enum ScmUomKindsEnum
    {
        None = 0,
        /// <summary>
        /// 国际单位
        /// </summary>
        International = 1,
        /// <summary>
        /// 市制单位
        /// </summary>
        Metric = 2,
        /// <summary>
        /// 英制单位
        /// </summary>
        Imperial = 3
    }
}
