using SqlSugar;
using System.Linq.Expressions;

namespace Com.Scm.Utils
{
    public class SqlSugarUtils
    {
        public static DbType GetDbType(string type)
        {
            if (type == null)
            {
                return DbType.MySql;
            }

            switch (type.ToLower())
            {
                case "sqlite":
                    return DbType.Sqlite;
                case "sqlserver":
                    return DbType.SqlServer;
                case "oracle":
                    return DbType.Oracle;
                default:
                    return DbType.MySql;
            }
        }

        public static async Task<List<T>> GetListAsync<T>(ISqlSugarClient client, Expression<Func<T, bool>> whereExpression)
        {
            return await client.Queryable<T>().Where(whereExpression).ToListAsync();
        }
    }
}
