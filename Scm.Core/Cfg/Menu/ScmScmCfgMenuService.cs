using Com.Scm.Adm.Menu;
using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Cfg.Menu
{
    /// <summary>
    /// 用户收藏菜单
    /// </summary>
    [ApiExplorerSettings(GroupName = "Cfg")]
    public class ScmScmCfgMenuService : ApiService
    {
        private readonly SugarRepository<CfgMenuDao> _thisRepository;
        private readonly ScmContextHolder _JwtHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmScmCfgMenuService(SugarRepository<CfgMenuDao> thisRepository, ScmContextHolder jwtHolder)
        {
            _thisRepository = thisRepository;
            _JwtHolder = jwtHolder;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<CfgMenuDto>> GetPagesAsync(ScmSearchPageRequest request)
        {
            return await _thisRepository.AsQueryable()
                .Select<CfgMenuDto>()
                .ToPageAsync(request.page, request.limit);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<CfgMenuDto>> GetListAsync(ScmSearchRequest param)
        {
            var list = await _thisRepository.AsQueryable()
                .OrderByDescending(m => m.id)
                .Select<CfgMenuDto>()
                .ToListAsync();

            return list;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CfgMenuDto> GetAsync(long id)
        {
            var dto = await _thisRepository.AsQueryable()
                .Where(a => a.id == id)
                .Select<CfgMenuDto>()
                .FirstAsync();
            return dto;
        }

        /// <summary>
        /// 获取编辑数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CfgMenuDto> GetEditAsync(long id)
        {
            var dto = await _thisRepository.AsQueryable()
                .Where(a => a.id == id)
                .Select<CfgMenuDto>()
                .FirstAsync();
            return dto;
        }

        /// <summary>
        /// 获取详情数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CfgMenuDvo> GetViewAsync(long id)
        {
            var dto = await _thisRepository.AsQueryable()
                .Where(a => a.id == id)
                .Select<CfgMenuDvo>()
                .FirstAsync();
            return dto;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(CfgMenuDto model)
        {
            //model.ParentId = long.Parse(model.ParentIdList.Last());
            //var upModel = await _thisRepository.GetFirstAsync(m => true, m => m.Sort);
            //model.Sort = upModel.Sort + 1;
            //return await _thisRepository.InsertAsync(model.Adapt<ScmCfgMenuDao>());

            var dao = await _thisRepository.GetFirstAsync(a => a.menu_id == model.menu_id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的菜单！");
            }

            dao = model.Adapt<CfgMenuDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(CfgMenuDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.menu_id == model.menu_id && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的菜单！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的菜单！");
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> SaveAsync(ScmUpdateRequest request)
        {
            var token = _JwtHolder.GetToken();

            var id = request.id;

            var dao = await _thisRepository.GetFirstAsync(a => a.user_id == token.user_id && a.menu_id == id);
            if (dao != null)
            {
                if (dao.row_status != Enums.ScmRowStatusEnum.Enabled)
                {
                    dao.row_status = Enums.ScmRowStatusEnum.Enabled;
                    await _thisRepository.UpdateAsync(dao);
                }
                return true;
            }

            var menuDao = await _thisRepository.Change<AdmMenuDao>().GetByIdAsync(id);
            if (menuDao == null)
            {
                throw new BusinessException("无效的菜单信息！");
            }

            dao = new CfgMenuDao();
            dao.user_id = token.user_id;
            dao.menu_id = id;
            dao.namec = menuDao.namec;
            dao.od = 0;
            dao.qty = 0;

            return await _thisRepository.InsertAsync(dao);
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
        /// 删除,支持多个
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync([FromBody] List<long> ids) =>
            await _thisRepository.DeleteAsync(m => ids.Contains(m.id));
    }
}