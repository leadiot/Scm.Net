using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum ScmUserTypesEnum
    {
        None = 0,
    }

    /// <summary>
    /// 角色数据类型
    /// </summary>
    public enum ScmUserDataEnum
    {
        [Description("默认")]
        None = 0,

        /// <summary>
        /// 全部数据
        /// </summary>
        [Description("全部数据")]
        All = 10,

        /// <summary>
        /// 当前用户数据
        /// </summary>
        [Description("当前用户数据")]
        CurrentUser = 20,

        /// <summary>
        /// 指定用户数据
        /// </summary>
        [Description("指定用户数据")]
        SpecifiedUser = 21,

        /// <summary>
        /// 排除用户
        /// </summary>
        [Description("排除用户数据")]
        ExcludeUser = 22,
    }

    public enum ScmUserDataTypesEnum
    {
        None = 0,
        [Description("机构")]
        Unit = 10,
        [Description("用户")]
        User = 20,
        [Description("部门")]
        Organize = 30,
        [Description("岗位")]
        Position = 40,
        [Description("群组")]
        Group = 50,
        [Description("其它")]
        Other = 90,
    }

    /// <summary>
    /// 角色首页类型
    /// </summary>
    public enum ScmUserHomeTypesEnum
    {
        None = 0,
        [Description("工作台")]
        Work,
        [Description("数据台")]
        Data,
        [Description("小组件")]
        Wegdit
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ScmUserVipEnum
    {
        None = 0,
    }

    /// <summary>
    /// token类型
    /// </summary>
    public enum ScmUserTokenTypeEnum
    {
        None,
        /// <summary>
        /// One Time Password
        /// </summary>
        Otp,
    }
}
