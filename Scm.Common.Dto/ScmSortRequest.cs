namespace Com.Scm
{
    /// <summary>
    /// 上下移动参数
    /// </summary>
    public class ScmSortRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 排序方式 0=向上  1=向下
        /// </summary>
        public SortTypeEnum type { get; set; }
    }

    public enum SortTypeEnum
    {
        Na,
        Up,
        Down,
        Left,
        Right
    }
}