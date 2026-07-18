using Com.Scm.Enums;

namespace Com.Scm.Dao
{
    public interface IStatusDao : IUpdateDao
    {
        public long id { get; set; }

        public ScmRowStatusEnum row_status { get; set; }
    }
}
