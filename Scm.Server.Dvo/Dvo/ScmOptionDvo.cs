namespace Com.Scm.Dvo
{
    public class ScmOptionDvo<T> : ScmDvo
    {
        /// <summary>
        /// 选项标签
        /// </summary>
        public string label { get; set; }
        /// <summary>
        /// 选项的值
        /// </summary>
        public T value { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool disabled { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TextOptionDvo : ScmOptionDvo<string>
    {
    }

    /// <summary>
    /// 应用资源
    /// </summary>
    public class ResOptionDvo : ScmOptionDvo<long>
    {
        /// <summary>
        /// 类别
        /// </summary>
        public int cat { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public long parentId { get; set; }
    }

    /// <summary>
    /// 数据字典
    /// </summary>
    public class DicOptionDvo : ScmOptionDvo<int>
    {
        /// <summary>
        /// 类别
        /// </summary>
        public int cat { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        public string codec { get; set; }
    }
}
