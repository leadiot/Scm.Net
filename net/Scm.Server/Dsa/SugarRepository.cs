using Com.Scm.Dao;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using Com.Scm.Token;
using Com.Scm.Utils;
using SqlSugar;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Com.Scm.Dsa
{
    public class SugarRepository<T> : SimpleClient<T> where T : class, new()
    {
        private static Type _UserType = typeof(IUserDao);
        private static Type _CreateType = typeof(ICreateDao);
        private static Type _DeleteType = typeof(IDeleteDao);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="holder"></param>
        public SugarRepository(ISqlSugarClient context = null) : base(context)
        {
            //var Context = context;
            //Context = AppUtils.GetService<ISqlSugarClient>();
            if (Context == null)
            {
                return;
            }

            var contextHolder = AppUtils.GetService<ScmContextHolder>();
            var token = contextHolder.GetToken();

            #region 处理数据过滤
            //if (Context.SugarActionType == SugarActionType.Query)
            //{
            var subType = typeof(T);
            // 用户数据
            GenUserLimitFilter(token, subType);
            // 删除数据
            if (CommonUtils.HasImplementedRawGeneric(subType, _DeleteType))
            {
                var subSql = $"it.{nameof(IDeleteDao.row_delete)}=1";
                var lambda = DynamicExpressionParser.ParseLambda([Expression.Parameter(subType, "it")], typeof(bool), subSql, false);
                Context.QueryFilter.Add(new TableFilterItem<T>(subType, lambda, true));
            }
            //}
            #endregion

            // AOP处理
            Context.Aop.DataExecuting = (oldValue, entityInfo) =>
            {
                if (!entityInfo.EntityColumnInfo.IsPrimarykey)
                {
                    return;
                }

                //新增操作
                if (entityInfo.OperationType == DataFilterType.InsertByObject)
                {
                    if (entityInfo.EntityValue is ScmDao)
                    {
                        var newValue = (ScmDao)entityInfo.EntityValue;
                        newValue.PrepareCreate(token.user_id);
                    }

                    return;
                }

                //更新操作
                if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                {
                    if (entityInfo.EntityValue is ScmDao)
                    {
                        var newValue = (ScmDao)entityInfo.EntityValue;
                        newValue.PrepareUpdate(token.user_id);
                    }

                    return;
                }
            };

            //// LOG处理
            //Context.Aop.OnLogExecuting = (s, p) =>
            //{
            //    var sqlValue = string.Empty;
            //    var sql = s;
            //    foreach (var item in p)
            //    {
            //        sql = sql.Replace(item.ParameterName, "'" + item.Value + "'");
            //    }
            //    LogUtils.Debug("Sql脚本：" + sql, "db");
            //};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="subType"></param>
        private void GenUserLimitFilter(ScmToken token, Type subType)
        {
            if (token.data == ScmUserDataEnum.All)
            {
                return;
            }

            if (!CommonUtils.HasImplementedRawGeneric(subType, _UserType))
            {
                return;
            }

            var sql = "it." + nameof(IUserDao.user_id);
            //if (token.data == ScmUserDataEnum.CurrentUser)
            //{
            //    GenLimitFilter(token, subType, sql + "=" + token.user_id);
            //    return;
            //}

            var sub = $"(select suud.data_id from scm_ur_user_data suud where suud.user_id = {token.user_id} and suud.types = {ScmUserDataTypesEnum.User} and suud.row_status = {ScmRowStatusEnum.Enabled})";
            if (token.data == ScmUserDataEnum.SpecifiedUser)
            {
                GenLimitFilter(token, subType, sql + " in " + sub);
                return;
            }

            if (token.data == ScmUserDataEnum.ExcludeUser)
            {
                GenLimitFilter(token, subType, sql + " not in " + sub);
                return;
            }

            GenLimitFilter(token, subType, sql + "=" + token.user_id);
            //GenLimitFilter(token, subType, sql + "=0");
        }

        private void GenLimitFilter(ScmToken token, Type subType, string subSql)
        {
            var lambda = DynamicExpressionParser.ParseLambda([Expression.Parameter(subType, "it")], typeof(bool), subSql, false);
            Context.QueryFilter.Add(new TableFilterItem<T>(subType, lambda, true));
        }

        private bool IsSubIntance(Type parent)
        {
            var entity = Context.EntityMaintenance.GetEntityInfo<T>();
            if (entity == null)
            {
                return false;
            }
            var types = entity.Type;
            if (types == null)
            {
                return false;
            }
            foreach (var type in types.GetInterfaces())
            {
                if (type == parent)
                {
                    return true;
                }
            }
            return false;
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
        public async Task<(List<T>, int totalItem, int totalPage)> GetPageResultAsync(Expression<Func<T, bool>> where, int page, int limit, Expression<Func<T, object>> order, OrderByEnum orderEnum = OrderByEnum.Desc)
        {
            RefAsync<int> totalItems = 0;
            var list = await Context.Queryable<T>()
                .Where(where)
                .OrderBy(order, orderEnum == OrderByEnum.Desc ? OrderByType.Desc : OrderByType.Asc)
                .ToPageListAsync(page, limit, totalItems);
            var sumPage = totalItems != 0 ? totalItems % page == 0 ? totalItems / limit : totalItems / limit + 1 : 0;
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
        public async Task<(List<T>, int totalItem, int totalPage)> GetPageResultAsync(Expression<Func<T, bool>> where, string strWhere, int page, int limit, Expression<Func<T, object>> order, OrderByEnum orderEnum = OrderByEnum.Desc)
        {
            RefAsync<int> totalItems = 0;
            var list = await Context.Queryable<T>()
                .Where(where)
                .WhereIF(!string.IsNullOrEmpty(strWhere), strWhere)
                .OrderBy(order, orderEnum == OrderByEnum.Desc ? OrderByType.Desc : OrderByType.Asc)
                .ToPageListAsync(page, limit, totalItems);
            var sumPage = totalItems != 0 ? totalItems % page == 0 ? totalItems / limit : totalItems / limit + 1 : 0;
            return (list, totalItems, sumPage);
        }

        /// <summary>
        /// 根据条件，获得最新的一条数据
        /// </summary>
        /// <param name="where">拉姆达条件</param>
        /// <param name="order">拉姆达排序</param>
        /// <param name="orderEnum">枚举，1=desc 2=asc</param>
        /// <returns></returns>
        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByEnum orderEnum = OrderByEnum.Desc)
        {
            return await Context.Queryable<T>()
                .Where(where)
                .OrderBy(order, orderEnum == OrderByEnum.Desc ? OrderByType.Desc : OrderByType.Asc)
                .FirstAsync() ?? new T();
        }

        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="where">拉姆达条件</param>
        /// <param name="order">拉姆达排序</param>
        /// <param name="orderEnum">枚举，1=desc 2=asc</param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByEnum orderEnum = OrderByEnum.Desc)
        {
            var query = Context.Queryable<T>()
                .Where(where)
                .OrderBy(order, orderEnum == OrderByEnum.Desc ? OrderByType.Desc : OrderByType.Asc);
            return query.ToList();
        }

        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="where">拉姆达条件</param>
        /// <param name="order">拉姆达排序</param>
        /// <param name="orderEnum">枚举，1=desc 2=asc</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where, Expression<Func<T, object>> order, OrderByEnum orderEnum = OrderByEnum.Desc)
        {
            var query = Context.Queryable<T>()
                .Where(where)
                .OrderBy(order, orderEnum == OrderByEnum.Desc ? OrderByType.Desc : OrderByType.Asc);
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