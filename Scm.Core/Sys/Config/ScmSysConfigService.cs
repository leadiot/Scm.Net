using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Service;
using Com.Scm.Sys.Config.Dvo;
using Com.Scm.Token;
using Com.Scm.Ur;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Config
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysConfigService : ApiService
    {
        private readonly SugarRepository<ConfigDao> _thisRepository;
        private readonly ScmContextHolder _jwtHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="jwtHolder"></param>
        public ScmSysConfigService(SugarRepository<ConfigDao> thisRepository, ScmContextHolder jwtHolder)
        {
            _thisRepository = thisRepository;
            _jwtHolder = jwtHolder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ConfigDto>> GetListAsync(SearchRequest request)
        {
            var user = _jwtHolder.GetToken();

            return await _thisRepository
                .AsQueryable()
                .Where(a => (a.user_id == user.user_id || a.user_id == UserDto.SYS_ID) && a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(request.client != ScmClientTypeEnum.None, a => a.client == request.client)
                .Select<ConfigDto>()
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public async Task<ConfigDto> GetAsync(string key)
        {
            var token = _jwtHolder.GetToken();
            var userId = token.user_id;

            return await _thisRepository.AsQueryable()
                .Where(a => a.key == key && a.row_status == ScmRowStatusEnum.Enabled)
                .Where(a => a.user_id == userId || a.user_id == UserDto.SYS_ID)
                .OrderBy(a => a.user_id, SqlSugar.OrderByType.Desc)
                .Select<ConfigDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="client"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<bool> SaveAsync(long userId, ScmClientTypeEnum client, string key, string value, ScmDataTypeEnum data)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.user_id == userId && a.key == key);
            if (dao == null)
            {
                dao = new ConfigDao();
                dao.user_id = userId;
                dao.client = client;
                dao.key = key;
                dao.value = value ?? "";
                dao.data = data;
                return await _thisRepository.InsertAsync(dao);
            }

            dao.client = client;
            dao.key = key;
            dao.value = value ?? "";
            dao.data = data;
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> PostAsync(List<ConfigDto> items)
        {
            var user = _jwtHolder.GetToken();

            foreach (var item in items)
            {
                await SaveAsync(user.user_id, item.client, item.key, item.value, item.data);
            }

            return true;
        }
    }
}
