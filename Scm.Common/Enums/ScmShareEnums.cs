namespace Com.Scm.Enums
{
    public enum ShareTypesEnums
    {
        None = 0,
        /// <summary>
        /// 私有
        /// </summary>
        Private = 1,
        /// <summary>
        /// 公开
        /// </summary>
        Public = 2,
        /// <summary>
        /// 指定机构
        /// </summary>
        ByUnit = 3,
        /// <summary>
        /// 指定用户
        /// </summary>
        ByUser = 4,
        /// <summary>
        /// 指定角色
        /// </summary>
        ByRole = 5,
        /// <summary>
        /// 指定群组
        /// </summary>
        ByGroup = 6,
        /// <summary>
        /// 指定标签
        /// </summary>
        ByLabel = 7,
    }

    public enum ShareModesEnums
    {
        None = 0,
        /// <summary>
        /// 包含
        /// </summary>
        Include = 1,
        /// <summary>
        /// 排除
        /// </summary>
        Exclude = 2
    }
}
