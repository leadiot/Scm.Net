using Com.Scm.Dev.Sql.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Utils;

using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Dev.Sql
{
    /// <summary>
    /// 数据库查询
    /// </summary>
    [ApiExplorerSettings(GroupName = "Dev")]
    public class ScmDevSqlService : ApiService
    {
        private readonly SugarRepository<ScmDevSqlDao> _thisRepository;
        private readonly SugarRepository<ScmDevDbDao> _dbRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="dbRepository"></param>
        public ScmDevSqlService(SugarRepository<ScmDevSqlDao> thisRepository, SugarRepository<ScmDevDbDao> dbRepository)
        {
            _thisRepository = thisRepository;
            _dbRepository = dbRepository;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SqlDvo> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SqlDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SqlDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SqlDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmDevSqlDto model)
        {
            var dao = model.Adapt<ScmDevSqlDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmDevSqlDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }

            dao = model.Adapt(dao);
            await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
            {
                return 0;
            }

            return await DeleteRecord(_thisRepository, ids.ToListLong());
        }

        /// <summary>
        /// 查询所有表
        /// </summary>
        /// <returns></returns>
        public List<DbTableInfo> GetTable(GetTableRequest request)
        {
            using (var client = GetClient(request.db_id))
            {
                if (client == null)
                {
                    throw new BusinessException("系统执行异常：无法建立连接！");
                }

                try
                {
                    client.Open();

                    return GetTable(client, request.key);
                }
                catch (Exception ex)
                {
                    LogUtils.Error(ex);
                    throw new BusinessException("系统执行异常：无法建立连接！");
                }
            }
        }

        private List<DbTableInfo> GetTable(ISqlSugarClient client, string key)
        {
            var list = client.DbMaintenance.GetTableInfoList();
            if (string.IsNullOrEmpty(key))
            {
                return list;
            }

            var filter = new List<DbTableInfo>();
            foreach (var item in list)
            {
                if (item.Name.Contains(key))
                {
                    filter.Add(item);
                }
            }
            return filter;
        }

        /// <summary>
        /// 根据表名查询列信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<DbColumnInfo> GetColumn(GetColumnRequest request)
        {
            using (var client = GetClient(request.db_id))
            {
                if (client == null)
                {
                    throw new BusinessException("系统执行异常：无法建立连接！");
                }

                try
                {
                    client.Open();
                    return client.DbMaintenance.GetColumnInfosByTableName(request.key);
                }
                catch (Exception ex)
                {
                    LogUtils.Error(ex);
                    throw new BusinessException("系统执行异常：无法建立连接！");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<SqlDvo> GetPresql(GetPresqlRequest request)
        {
            var list = _thisRepository.AsQueryable()
                .Where(a => a.db_id == request.db_id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.qty, OrderByType.Desc)
                .Select<SqlDvo>()
                .ToList();
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ExecuteResponse> PostExecuteAsync(ExecuteRequest request)
        {
            var response = new ExecuteResponse();

            try
            {
                using (var client = GetClient(request.db_id))
                {
                    if (client == null)
                    {
                        response.SetFailure("系统执行异常：无法建立连接！");
                        return response;
                    }

                    client.Open();
                    await DoExecute(client, request, response);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex);
                response.SetFailure("系统执行异常：无法建立连接！");
            }

            return response;
        }

        private async Task DoExecute(ISqlSugarClient client, ExecuteRequest request, ExecuteResponse response)
        {
            var sql = request.sql;
            if (string.IsNullOrWhiteSpace(sql))
            {
                response.SetFailure("待执行语句为空！");
                return;
            }
            sql = sql.Trim().TrimEnd(';');

            var lowerSql = sql.ToLower();
            if (lowerSql.StartsWith("select "))
            {
                response.type = ExecuteTypeEnum.Select;
                var qty = 1L;
                if (lowerSql.IndexOf(" from ") > 0)
                {
                    qty = (long)client.Ado.GetScalar(GenCounterSql(sql));

                    if (qty > request.limit)
                    {
                        if (lowerSql.IndexOf(" limit ") < 1)
                        {
                            var start = (request.page - 1) * request.limit;
                            sql += $" limit {start},{request.limit}";
                        }
                    }
                }

                response.Data = await client.Ado.GetDataTableAsync(sql);
                response.qty = 1;
                response.TotalItems = qty;
                response.SetSuccess();
            }
            else if (lowerSql.StartsWith("insert "))
            {
                response.type = ExecuteTypeEnum.Insert;
                response.qty = await client.Ado.ExecuteCommandAsync(sql);
                response.TotalItems = response.qty;
                response.SetSuccess();
            }
            else if (lowerSql.StartsWith("update "))
            {
                response.type = ExecuteTypeEnum.Update;
                response.qty = await client.Ado.ExecuteCommandAsync(sql);
                response.TotalItems = response.qty;
                response.SetSuccess();
            }
            else if (lowerSql.StartsWith("delete "))
            {
                response.type = ExecuteTypeEnum.Delete;
                response.qty = await client.Ado.ExecuteCommandAsync(sql);
                response.TotalItems = response.qty;
                response.SetSuccess();
            }
            else
            {
                response.type = ExecuteTypeEnum.Execute;
                response.qty = await client.Ado.ExecuteCommandAsync(sql);
                response.TotalItems = response.qty;
                response.SetSuccess();
            }
        }

        private string GenCounterSql(string sql)
        {
            var lowerSql = sql.ToLower();
            var idx2 = lowerSql.IndexOf(" from ");
            return "select count(0)" + sql.Substring(idx2);
        }

        private ISqlSugarClient GetClient(long id)
        {
            var dbDao = _dbRepository.GetById(id);
            if (dbDao == null)
            {
                return null;
            }

            var config = new ConnectionConfig();
            config.ConnectionString = $"server={dbDao.host};database={dbDao.schame};uid={dbDao.user};pwd={dbDao.pass};charset={dbDao.charset};SslMode=None;";
            config.DbType = DbType.MySql;
            config.IsAutoCloseConnection = true;
            config.InitKeyType = InitKeyType.Attribute;

            return new SqlSugarClient(config);
        }
    }
}
