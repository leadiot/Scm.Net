using Com.Scm.Dao;
using Com.Scm.Dao.Unit;
using Com.Scm.Enum;
using Com.Scm.Jwt;
using Com.Scm.Log;
using Com.Scm.Utils;
using SqlSugar;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Com.Scm.Dsa.Dba.Sugar
{
    public class SugarRepository<T> : SimpleClient<T> where T : class, new()
    {
        private static Type _UnitType = typeof(IUnitDao);
        private static Type _DeleteType = typeof(IDeleteDao);

        public SugarRepository(ISqlSugarClient context = null) : base(context)
        {
            Context = AppUtils.GetService<ISqlSugarClient>();
            if (Context == null)
            {
                return;
            }

            var tenantContextHolder = AppUtils.GetService<JwtContextHolder>();
            var token = tenantContextHolder.GetToken();

            #region 处理租户数据过滤
            var subType = typeof(T);
            if (CommonUtils.HasImplementedRawGeneric(subType, _UnitType))
            {
                var lambda = DynamicExpressionParser.ParseLambda(new[] { Expression.Parameter(subType, "it") }, typeof(bool), nameof(IUnitDao.unit_id) + "=" + token.unit_id, false);
                Context.QueryFilter.Add(new TableFilterItem<T>(subType, lambda, true));
            }
            if (CommonUtils.HasImplementedRawGeneric(subType, _DeleteType))
            {
                var lambda = DynamicExpressionParser.ParseLambda(new[] { Expression.Parameter(subType, "it") }, typeof(bool), nameof(IDeleteDao.row_delete) + "=false", false);
                Context.QueryFilter.Add(new TableFilterItem<T>(subType, lambda, true));
            }
            #endregion

            // AOP处理
            Context.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                //新增操作
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    if (entityInfo.EntityValue is BaseDao)
                    {
                        var newValue = (BaseDao)entityInfo.EntityValue;
                        newValue.PrepareCreate(token.user_id, token.unit_id);
                    }

                    return;
                }

                //更新操作
                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.EntityValue is BaseDao)
                    {
                        var newValue = (BaseDao)entityInfo.EntityValue;
                        newValue.PrepareUpdate(token.user_id, token.unit_id);
                    }

                    return;
                }
            };

            // LOG处理
            Context.Aop.OnLogExecuting = (s, p) =>
            {
                var sqlValue = string.Empty;
                var sql = s;
                foreach (var item in p)
                {
                    sql = sql.Replace(item.ParameterName, "'" + item.Value + "'");
                }
                Logger.Info("Sql脚本：" + sql);
            };
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="page">当前页</param>
        /// <param name="limit">每页条数</param>
        /// <param name="order">排序</param>
        /// <param name="orderEnum">枚举，1=desc 2=asc</param>
        /// <returns>集合、总条数、总页数</returns>
        public async Task<(List<T>, int totalItem, int totalPage)> GetPageResultAsync(Expression<Func<T, bool>> where, int page, int limit, Expression<Func<T, object>> order, OrderEnum orderEnum = OrderEnum.Desc)
        {
            RefAsync<int> totalItems = 0;
            var list = await Context.Queryable<T>()
                .Where(where)
                .OrderBy(order, (int)orderEnum == 1 ? OrderByType.Desc : OrderByType.Asc)
                .ToPageListAsync(page, limit, totalItems);
            var sumPage = totalItems != 0 ? (totalItems % page) == 0 ? (totalItems / limit) : (totalItems / limit) + 1 : 0;
            return (list, totalItems, sumPage);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="where">Expression 条件</param>
        /// <param name="strWhere">string 条件</param>
        /// <param name="page">当前页</param>
        /// <param name="limit">每页条数</param>
        /// <param name="order">排序</param>
        /// <param name="orderEnum">枚举，1=desc 2=asc</param>
        /// <returns>集合、总条数、总页数</returns>
        public async Task<(List<T>, int totalItem, int totalPage)> GetPageResultAsync(Expression<Func<T, bool>> where, string strWhere, int page, int limit, Expression<Func<T, object>> order, OrderEnum orderEnum = OrderEnum.Desc)
        {
            RefAsync<int> totalItems = 0;
            var list = await Context.Queryable<T>()
                .Where(where)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere)
                .OrderBy(order, (int)orderEnum == 1 ? OrderByType.Desc : OrderByType.Asc)
                .ToPageListAsync(page, limit, totalItems);
            var sumPage = totalItems != 0 ? (totalItems % page) == 0 ? (totalItems / limit) : (totalItems / limit) + 1 : 0;
            return (list, totalItems, sumPage);
        }

        /// <summary>
        /// 根据条件，获得最新的一条数据
        /// </summary>
        /// <param name="where">拉姆达条件</param>
        /// <param name="order">拉姆达排序</param>
        /// <param name="orderEnum">枚举，1=desc 2=asc</param>
        /// <returns></returns>
        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderEnum orderEnum = OrderEnum.Desc)
        {
            return await Context.Queryable<T>()
                .Where(where)
                .OrderBy(order, (int)orderEnum == 1 ? OrderByType.Desc : OrderByType.Asc)
                .FirstAsync() ?? new T();
        }

        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="where">拉姆达条件</param>
        /// <param name="order">拉姆达排序</param>
        /// <param name="orderEnum">枚举，1=desc 2=asc</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderEnum orderEnum = OrderEnum.Desc)
        {
            var query = Context.Queryable<T>()
                .Where(where)
                .OrderBy(order, (int)orderEnum == 1 ? OrderByType.Desc : OrderByType.Asc);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="where">拉姆达条件</param>
        /// <param name="order">order by id desc</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, string order)
        {
            var query = Context.Queryable<T>()
                .Where(where)
                .OrderByIF(!string.IsNullOrEmpty(order), order);
            return await query.ToListAsync();
        }

        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="where">拉姆达条件</param>
        /// <param name="expression">拉姆达排序</param>
        /// <returns></returns>
        public async Task<int> SumAsync(Expression<Func<T, bool>> where, Expression<Func<T, int>> expression)
        {
            var query = Context.Queryable<T>()
                .Where(where);
            return await query.SumAsync(expression);
        }

        public async Task<int> StatusAsync(long id)
        {
            var query = Context.Updateable<T>()
                .Where("id=" + id);
            return await query.ExecuteCommandAsync();
        }
    }
}