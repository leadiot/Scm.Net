using SqlSugar;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

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

        // 把 SQL 里的 [xxx] 转成当前数据库的关键字转义符
        public static string EscapeSql(DbType dbType, string sql)
        {
            if (string.IsNullOrEmpty(sql)) return sql;

            // 匹配所有 [任意字符]
            var regex = new Regex(@"\[(.*?)\]");

            string left = "";
            string right = "";

            // 根据数据库类型设置左右转义符
            switch (dbType)
            {
                case DbType.MySql:
                    //case DbType.MariaDB:
                    left = right = "`"; // `table`
                    break;

                case DbType.SqlServer:
                    //case DbType.SqlServer2012:
                    left = "["; right = "]"; // [table]
                    break;

                case DbType.Sqlite:
                case DbType.Oracle:
                case DbType.PostgreSQL:
                    left = right = "\""; // "table"
                    break;

                default:
                    left = "["; right = "]";
                    break;
            }

            // 替换所有 [xxx]
            return regex.Replace(sql, m => left + m.Groups[1].Value + right);
        }

        public static async Task<List<T>> GetListAsync<T>(ISqlSugarClient client, Expression<Func<T, bool>> whereExpression)
        {
            return await client.Queryable<T>().Where(whereExpression).ToListAsync();
        }
    }
}
