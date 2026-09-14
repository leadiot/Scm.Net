namespace Com.Scm.Dao.Sync
{
    public interface ISyncDao : IDeleteDao
    {
        public long sync_time { get; set; }
    }
}
