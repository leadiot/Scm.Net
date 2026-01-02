using Com.Scm.Config;
using Com.Scm.Dao;
using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Sys.Tasks;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.Service
{
    /// <summary>
    /// Api服务
    /// </summary>
    public class ApiService : IApiService
    {
        protected EnvConfig _EnvConfig;
        protected ISqlSugarClient _SqlClient;
        protected IUserHolder _UserHolder;
        protected Com.Scm.Cache.ICacheService _CacheService;

        #region Search缓存
        /// <summary>
        /// 保存查询条件
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        protected void SaveSearch(long userId, ScmSearchRequest request)
        {
            if (_CacheService == null)
            {
                return;
            }

            var key = userId + request.GetType().FullName;
            _CacheService.SetCache(key, request);
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected T ReadSearch<T>(long userId) where T : ScmSearchRequest, new()
        {
            if (_CacheService == null)
            {
                return default(T);
            }

            var key = userId + typeof(T).FullName;
            return _CacheService.GetCache<T>(key);
        }

        /// <summary>
        /// 移除查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected bool RemoveSearch<T>(long userId)
        {
            if (_CacheService == null)
            {
                return false;
            }

            var key = userId + typeof(T).FullName;
            _CacheService.RemoveCache(key);
            return true;
        }
        #endregion

        #region 读取Dao
        protected async Task<T> GetByIdAsync<T>(long id) where T : ScmDao
        {
            return await _SqlClient.Queryable<T>().FirstAsync(a => a.id == id);
        }

        protected T GetById<T>(long id) where T : ScmDao
        {
            return _SqlClient.Queryable<T>().First(a => a.id == id);
        }
        #endregion

        #region 记录操作
        /// <summary>
        /// 更新数据状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cient"></param>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected async Task<int> UpdateStatus<T>(SugarRepository<T> cient, List<long> ids, ScmRowStatusEnum status) where T : class, IStatusDao, new()
        {
            var updateable = cient.AsUpdateable()
                .Where(a => ids.Contains(a.id));
            if (status == ScmRowStatusEnum.Reverse)
            {
                updateable = updateable.SetColumns(a => a.row_status == (3 - a.row_status));
            }
            else
            {
                updateable = updateable.SetColumns(a => a.row_status == status);
            }
            return await updateable.ExecuteCommandAsync();
        }

        protected async Task<int> UpdateStatus<T>(ISqlSugarClient client, List<long> ids, ScmRowStatusEnum status) where T : class, IStatusDao, new()
        {
            var updateable = client.Updateable<T>()
                .Where(a => ids.Contains(a.id));
            if (status == ScmRowStatusEnum.Reverse)
            {
                updateable = updateable.SetColumns(a => a.row_status == (3 - a.row_status));
            }
            else
            {
                updateable = updateable.SetColumns(a => a.row_status == status);
            }
            return await updateable.ExecuteCommandAsync();
        }

        /// <summary>
        /// 物理删除记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="ids"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        protected async Task<int> DeleteRecord<T>(SugarRepository<T> client, List<long> ids, bool delete = true) where T : ScmDao, new()
        {
            //if (CommonUtils.HasImplementedRawGeneric(typeof(T), typeof(IDeleteDao)))
            //{
            //    return await sugar.AsUpdateable()
            //        .Where(a => ids.Contains(a.id))
            //        .SetColumns(a => a.row_delete == delete)
            //        .ExecuteCommandAsync();
            //}

            return await client.AsDeleteable().Where(m => ids.Contains(m.id)).ExecuteCommandAsync();
        }

        /// <summary>
        /// 物理删除记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="ids"></param>
        /// <param name="delete"></param>
        /// <returns></returns>
        protected async Task<int> DeleteRecord<T>(ISqlSugarClient client, List<long> ids, bool delete = true) where T : ScmDao, new()
        {
            //if (typeof(T) is IDeleteDao)
            //{

            //}
            //if (CommonUtils.HasImplementedRawGeneric(typeof(T), typeof(IDeleteDao)))
            //{
            //    return await sugar.AsUpdateable()
            //        .Where(a => ids.Contains(a.id))
            //        .SetColumns(a => a.row_delete == delete)
            //        .ExecuteCommandAsync();
            //}

            return await client.Deleteable<T>().Where(m => ids.Contains(m.id)).ExecuteCommandAsync();
        }
        #endregion

        #region 导入导出
        /// <summary>
        /// 保存导出任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugar"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        protected async Task<TaskDao> SaveExportTask<T>(SugarRepository<T> sugar, ITaskHandler handler) where T : class, new()
        {
            var exportRespository = sugar.Change<TaskDao>();

            var json = handler.Json;
            var code = SecUtils.Md5(json);

            var dao = await exportRespository.GetFirstAsync(a => a.codec == code && a.handle == ScmHandleEnum.Todo);
            if (dao != null)
            {
                return dao;
            }

            dao = new TaskDao();
            dao.types = TaskTypesEnum.Export;
            dao.names = handler.Name;
            dao.codec = code;
            dao.clazz = handler.GetType().FullName;
            dao.json = json;
            dao.file = "";
            dao.handle = ScmHandleEnum.Todo;
            dao.result = ScmResultEnum.None;
            dao.need_time_f = handler.FromTime();
            dao.need_time_t = handler.ToTime();
            await exportRespository.InsertAsync(dao);

            return dao;
        }

        /// <summary>
        /// 保存导入任务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugar"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        protected async Task SaveImportTask<T>(SugarRepository<T> sugar, ITaskHandler handler) where T : class, new()
        {
            var exportRespository = sugar.Change<TaskDao>();

            var json = handler.ToJsonString();
            var code = SecUtils.Md5(json);

            var dao = await exportRespository.GetFirstAsync(a => a.codec == code);
            if (dao != null)
            {
                return;
            }

            dao = new TaskDao();
            dao.types = TaskTypesEnum.Import;
            dao.codec = code;
            dao.clazz = handler.GetType().FullName;
            dao.json = json;
            dao.handle = ScmHandleEnum.Todo;
            dao.result = ScmResultEnum.None;
            await exportRespository.InsertAsync(dao);
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 是否为可用的ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool IsValidId(long id)
        {
            return id > 1000;
        }

        /// <summary>
        /// 是否为有效的ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool IsNormalId(long id)
        {
            return id > ScmEnv.DEFAULT_ID;
        }

        /// <summary>
        /// 是否为合适的整数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool IsValidInt(long id)
        {
            return id > 0;
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="BusinessException"></exception>
        protected void Error(string message)
        {
            throw new BusinessException(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dvo"></param>
        protected void Prepare(ScmDataDvo dvo)
        {
            dvo.update_names = _UserHolder.GetUserNames(dvo.update_user);
            dvo.create_names = _UserHolder.GetUserNames(dvo.create_user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        protected void Prepare<T>(List<T> list) where T : ScmDataDvo
        {
            foreach (var item in list)
            {
                item.update_names = _UserHolder.GetUserNames(item.update_user);
                item.create_names = _UserHolder.GetUserNames(item.create_user);
            }
        }
        #endregion
    }
}
