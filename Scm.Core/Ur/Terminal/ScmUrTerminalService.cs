using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Scm.Ur
{
    /// <summary>
    /// 终端服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class ScmUrTerminalService : ApiService
    {
        private readonly SugarRepository<ScmUrTerminalDao> _thisRepository;
        private readonly IDicService _DicHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmUrTerminalService(SugarRepository<ScmUrTerminalDao> thisRepository, IDicService dicHolder, IResHolder resHolder)
        {
            _thisRepository = thisRepository;
            _DicHolder = dicHolder;
            _ResHolder = resHolder;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmUrTerminalDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.names.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmUrTerminalDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmUrTerminalDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Com.Scm.Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.names.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmUrTerminalDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        private void Prepare(List<ScmUrTerminalDvo> list)
        {
            var dicHeader = _DicHolder.GetDic("client_type");

            foreach (var item in list)
            {
                item.types_name = dicHeader.GetDetailNamec((int)item.types);
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmUrTerminalDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmUrTerminalDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmUrTerminalDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmUrTerminalDvo>()
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
                .Select(a => new ResOptionDvo { id = a.id, label = a.names, value = a.id })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmUrTerminalDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmUrTerminalDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmUrTerminalDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的终端！");
            }

            model.GenIcon();

            dao = model.Adapt<ScmUrTerminalDao>();
            dao.codec = "";
            dao.pass = TextUtils.RandomString(16);
            dao.access_token = "";
            dao.refresh_token = "";

            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmUrTerminalDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的终端！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的终端！");
            }

            dao.names = model.namec;
            dao.namec = model.namec;
            dao.remark = model.remark;

            _ResHolder.Remove(dao.id);

            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            _ResHolder.Clear();

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
            _ResHolder.Clear();

            return await DeleteRecord(_thisRepository, ids.ToListLong());
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<bool> ReleaseAsync(long id)
        {
            var dao = await _thisRepository.GetByIdAsync(id);
            if (dao == null)
            {
                throw new BusinessException("无效的终端代码！");
            }

            _ResHolder.Remove(dao.id);

            dao.binded = ScmBoolEnum.False;
            dao.pass = TextUtils.RandomString(16);
            await _thisRepository.UpdateAsync(dao);

            return true;
        }
    }
}