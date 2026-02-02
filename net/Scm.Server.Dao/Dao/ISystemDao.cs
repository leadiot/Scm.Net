using Com.Scm.Enums;
using SqlSugar;

namespace Com.Scm.Dao
{
    public interface ISystemDao : IUpdateDao
    {
        public long id { get; set; }

        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowSystemEnum row_system { get; set; }
    }
}
