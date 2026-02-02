using Com.Scm.Enums;
using SqlSugar;

namespace Com.Scm.Dao
{
    public interface IStatusDao : IUpdateDao
    {
        public long id { get; set; }

        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowStatusEnum row_status { get; set; }
    }
}
