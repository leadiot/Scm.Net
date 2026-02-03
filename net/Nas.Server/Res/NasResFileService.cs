using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Nas.Res.Dvo;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Nas.Res
{
    /// <summary>
    /// 文档服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Nas")]
    public class NasResFileService : ApiService
    {
        protected readonly SugarRepository<NasResFileDao> _thisRepository;
        protected readonly ScmContextHolder _jwtHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public NasResFileService(SugarRepository<NasResFileDao> thisRepository, ISqlSugarClient sqlClient, ScmContextHolder scmHolder, IResHolder resHolder)
        {
            _thisRepository = thisRepository;
            _SqlClient = sqlClient;
            _jwtHolder = scmHolder;
            _ResHolder = resHolder;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NasResFileDvo>> GetPagesAsync(SearchRequest request)
        {
            if (!IsValidId(request.dir_id))
            {
                request.dir_id = GetRootDirId();
            }

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.dir_id == request.dir_id)
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.name.Contains(request.key))
                .OrderBy(a => a.type)
                .OrderBy(m => m.id)
                .Select<NasResFileDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        protected virtual long GetRootDirId()
        {
            return NasEnv.DEF_DIR_ID;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<NasResFileDvo>> GetListAsync(SearchRequest request)
        {
            if (!IsValidId(request.dir_id))
            {
                request.dir_id = GetRootDirId();
            }

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.dir_id == request.dir_id && a.row_status == Com.Scm.Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.name.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<NasResFileDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NasResFileDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<NasResFileDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NasResFileDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<NasResFileDvo>()
                .FirstAsync();
        }

        /// <summary>
        /// 下拉列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ResOptionDvo>> GetOptionAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.id)
                .Select(a => new ResOptionDvo { id = a.id, label = a.name, value = a.id })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NasResFileDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<NasResFileDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(NasResFileDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.name == model.name && a.dir_id == model.dir_id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的文档！");
            }

            var parentDao = await _thisRepository.GetByIdAsync(model.dir_id);
            if (parentDao == null || parentDao.type != ScmFileTypeEnum.Dir)
            {
                throw new BusinessException("上级目录不存在！");
            }

            var parentPath = parentDao.path;

            dao = model.Adapt<NasResFileDao>();
            dao.modify_time = TimeUtils.GetUnixTime(true);
            dao.path = NasUtils.CombinePath(parentPath, model.name);

            var result = await _thisRepository.InsertAsync(dao);

            var manager = new NasManager(_SqlClient);
            manager.AddCreateLog(dao, parentDao.user_id);

            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(NasResFileDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.name == model.name && a.dir_id == model.dir_id && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的文档！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的文档！");
            }

            var parentDao = await _thisRepository.GetByIdAsync(model.dir_id);

            var src = dao.path;
            var parentPath = NasUtils.GetParentPath(src);
            dao = model.Adapt(dao);
            dao.modify_time = TimeUtils.GetUnixTime(true);
            dao.path = NasUtils.CombinePath(parentPath, model.name);
            var result = await _thisRepository.UpdateAsync(dao);

            var manager = new NasManager(_SqlClient);
            manager.AddRenameLog(dao, parentDao.user_id, src);

            return result;
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
            var token = _jwtHolder.GetToken();

            var idList = ids.ToListLong();
            var daoList = await _thisRepository.GetListAsync(a => idList.Contains(a.id));
            var manager = new NasManager(_SqlClient);
            manager.AddDeleteLog(daoList, token.user_id);

            return await DeleteRecord(_thisRepository, ids.ToListLong());
        }
    }
}