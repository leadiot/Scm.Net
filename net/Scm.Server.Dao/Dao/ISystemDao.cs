using Com.Scm.Enums;

namespace Com.Scm.Dao
{
    public interface ISystemDao : IUpdateDao
    {
        public long id { get; set; }

        public ScmRowSystemEnum row_system { get; set; }
    }
}
