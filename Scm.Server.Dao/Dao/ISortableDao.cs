namespace Com.Scm.Dao
{
    public interface ISortableDao : IUpdateDao
    {
        public long id { get; set; }

        public int od { get; set; }
    }
}
