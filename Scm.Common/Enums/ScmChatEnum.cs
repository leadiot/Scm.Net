namespace Com.Scm.Enums
{
    public enum ScmChatFriendTypesEnum
    {
        None = 0,
        /// <summary>
        /// 人
        /// </summary>
        Human = 1,
        /// <summary>
        /// 群
        /// </summary>
        Group = 2
    }

    public enum ScmChatGroupTypesEnum
    {
        None = 0,
        /// <summary>
        /// 个人
        /// </summary>
        Single = 1,
        /// <summary>
        /// 群聊
        /// </summary>
        Groups = 2
    }

    public enum ScmChatGroupModesEnums
    {
        None = 0,
        /// <summary>
        /// 内部
        /// </summary>
        Inner = 10,
        /// <summary>
        /// 外部
        /// </summary>
        Outter = 20
    }

    public enum ScmChatMessageTypesEnums
    {
        None = 0,
        /// <summary>
        /// 文本
        /// </summary>
        Text = 10,
        /// <summary>
        /// 图片
        /// </summary>
        Image = 20,
        /// <summary>
        /// 文件
        /// </summary>
        File = 30
    }
}
