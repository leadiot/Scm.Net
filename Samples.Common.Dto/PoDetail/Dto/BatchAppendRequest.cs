namespace Com.Scm.Samples.PoDetail.Dto
{
    public class BatchAppendRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 更新内容
        /// </summary>
        public List<long> items { get; set; }
        /// <summary>
        /// 追加数量
        /// </summary>
        public int qty { get; set; } = 1;
    }
}
