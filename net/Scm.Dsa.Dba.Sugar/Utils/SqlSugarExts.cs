using SqlSugar;
using System.Linq.Expressions;

namespace Com.Scm.Utils
{
    public static class SqlSugarExts
    {
        public static async Task<ScmSearchPageResponse<T>> ToPageAsync<T>(this ISugarQueryable<T> query, int pageIndex, int pageSize, bool isMapper = false)
        {
            RefAsync<int> totalItems = 0;
            var items = await query.ToPageListAsync(pageIndex, pageSize, totalItems);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            return new ScmSearchPageResponse<T>()
            {
                Items = isMapper ? items.Adapt<List<T>>() : items,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Success = true
            };
        }

        public static async Task<List<T>> GetListAsync<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression)
        {
            return await client.Queryable<T>().Where(whereExpression).ToListAsync();
        }

        public static List<T> GetList<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression)
        {
            return client.Queryable<T>().Where(whereExpression).ToList();
        }

        public static async Task<List<T>> GetListAsync<T>(this ISqlSugarClient client)
        {
            return await client.Queryable<T>().ToListAsync();
        }

        public static List<T> GetList<T>(this ISqlSugarClient client)
        {
            return client.Queryable<T>().ToList();
        }

        public static async Task<T> GetFirstAsync<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression)
        {
            return await client.Queryable<T>().Where(whereExpression).FirstAsync();
        }

        public static T GetFirst<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression)
        {
            return client.Queryable<T>().Where(whereExpression).First();
        }

        public static async Task<T> GetByIdAsync<T>(this ISqlSugarClient client, dynamic id)
        {
            return await client.Queryable<T>().InSingleAsync(id);
        }

        public static T GetById<T>(this ISqlSugarClient client, dynamic id)
        {
            return client.Queryable<T>().InSingle(id);
        }

        public static async Task<int> InsertAsync<T>(this ISqlSugarClient client, List<T> insertObjs) where T : class, new()
        {
            return await client.Insertable(insertObjs).ExecuteCommandAsync();
        }

        public static int Insert<T>(this ISqlSugarClient client, List<T> insertObjs) where T : class, new()
        {
            return client.Insertable(insertObjs).ExecuteCommand();
        }

        public static async Task<int> InsertAsync<T>(this ISqlSugarClient client, T insertObj) where T : class, new()
        {
            return await client.Insertable(insertObj).ExecuteCommandAsync();
        }

        public static int Insert<T>(this ISqlSugarClient client, T insertObj) where T : class, new()
        {
            return client.Insertable(insertObj).ExecuteCommand();
        }

        public static async Task<int> UpdateAsync<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression) where T : class, new()
        {
            return await client.Updateable<T>().Where(whereExpression).ExecuteCommandAsync();
        }

        public static int Update<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression) where T : class, new()
        {
            return client.Updateable<T>().Where(whereExpression).ExecuteCommand();
        }

        public static async Task<int> UpdateAsync<T>(this ISqlSugarClient client, T updateObj) where T : class, new()
        {
            return await client.Updateable(updateObj).ExecuteCommandAsync();
        }

        public static int Update<T>(this ISqlSugarClient client, T updateObj) where T : class, new()
        {
            return client.Updateable(updateObj).ExecuteCommand();
        }

        public static async Task<int> DeleteAsync<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression) where T : class, new()
        {
            return await client.Deleteable<T>().Where(whereExpression).ExecuteCommandAsync();
        }

        public static int Delete<T>(this ISqlSugarClient client, Expression<Func<T, bool>> whereExpression) where T : class, new()
        {
            return client.Deleteable<T>().Where(whereExpression).ExecuteCommand();
        }
    }
}
