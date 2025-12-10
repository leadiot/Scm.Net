namespace Com.Scm.Samples.PoDetail.Dto
{
    public class BatchUpdateRequest : ScmUpdateRequest
    {
        public List<BatchUpdateItem> items { get; set; }
    }

    public class BatchUpdateItem
    {
        public long id { get; set; }
        public int qty { get; set; }
    }
}
