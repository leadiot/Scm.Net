using Com.Scm.Enums;
using SqlSugar;

namespace Com.Scm.Dao
{
    public interface IDeleteDao : IUpdateDao
    {
        public long id { get; set; }

        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowDeleteEnum row_delete { get; set; }
    }
}
