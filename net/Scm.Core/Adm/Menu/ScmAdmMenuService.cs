using Com.Scm.Adm.Menu.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Adm.Menu
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Adm")]
    public class ScmAdmMenuService : IApiService
    {
        private readonly SugarRepository<AdmMenuDao> _thisRepository;
        private readonly ScmContextHolder _jwtContextHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmAdmMenuService(SugarRepository<AdmMenuDao> thisRepository, ScmContextHolder jwtContextHolder)
        {
            _thisRepository = thisRepository;
            _jwtContextHolder = jwtContextHolder;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<AdmMenuDvo>> GetPagesAsync(ScmSearchPageRequest param)
        {
            var query = await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.namec.Contains(param.key))
                .Select<AdmMenuDvo>()
                .ToPageAsync(param.page, param.limit);
            return query;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<AdmMenuDvo>> GetListAsync(ScmSearchPageRequest param)
        {
            var token = _jwtContextHolder.GetToken();

            var list = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.namec.Contains(param.key))
                .OrderBy(a => a.od)
                .Select<AdmMenuDvo>()
                .ToListAsync();
            return list;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResOptionDvo>> OptionAsync()
        {
            return await _thisRepository.AsQueryable()
                .OrderBy(m => m.od)
                .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id })
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AdmMenuDto> GetAsync(long id)
        {
            var model = await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<AdmMenuDto>()
                .FirstAsync();
            return model;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AdmMenuDto model)
        {
            var dao = model.Adapt<AdmMenuDao>();
            var prevDao = await _thisRepository.GetFirstAsync(a => a.pid == model.pid, a => a.od, OrderByEnum.Desc);
            dao.od = prevDao != null ? prevDao.od + 1 : 1;

            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 添加-临时=未命名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AdmMenuDto> AddTempAsync(CreateRequest param)
        {
            var qty = await _thisRepository
                .AsQueryable()
                .CountAsync();

            var dao = new AdmMenuDao()
            {
                namec = param.name,
                pid = param.pid,
                types = ScmMenuTypesEnum.Menu,
                od = qty
            };

            await _thisRepository.InsertAsync(dao);

            return dao.Clone<AdmMenuDto>();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(AdmMenuDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }

            dao = CommonUtils.Adapt(model, dao);
            await _thisRepository
                .AsUpdateable(dao)
                .ExecuteCommandAsync();
        }

        /// <summary>
        /// 拖动节点排序
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task DraggingAsync(ResortRequest request)
        {
            if (request.DragNode == null)
            {
                throw new BusinessException("无效的来源节点");
            }
            var dragDao = await _thisRepository.GetByIdAsync(request.DragNode.Id);
            if (dragDao == null)
            {
                throw new BusinessException("无效的来源节点");
            }

            if (request.DropNode == null)
            {
                throw new BusinessException("无效的目的节点");
            }
            var dropDao = await _thisRepository.GetByIdAsync(request.DropNode.Id);
            if (dropDao == null)
            {
                throw new BusinessException("无效的目的节点");
            }

            List<AdmMenuDao> list;
            var idx = 1;
            if (request.SortType == "before")
            {
                list = await _thisRepository.GetListAsync(a => a.pid == dropDao.pid && a.id != dragDao.id, a => a.od, OrderByEnum.Asc);
                foreach (var item in list)
                {
                    if (item.id == dropDao.id)
                    {
                        dragDao.od = idx++;
                    }
                    item.od = idx++;
                }
                dragDao.pid = dropDao.pid;
            }
            else if (request.SortType == "after")
            {
                list = await _thisRepository.GetListAsync(a => a.pid == dropDao.pid && a.id != dragDao.id, a => a.od, OrderByEnum.Asc);
                foreach (var item in list)
                {
                    item.od = idx++;
                    if (item.id == dropDao.id)
                    {
                        dragDao.od = idx++;
                    }
                }
                dragDao.pid = dropDao.pid;
            }
            else if (request.SortType == "inner")
            {
                list = await _thisRepository.GetListAsync(a => a.pid == dropDao.id && a.id != dragDao.id, a => a.od, OrderByEnum.Asc);
                foreach (var item in list)
                {
                    item.od = idx++;
                }
                dragDao.od = idx++;
                dragDao.pid = dropDao.id;
            }
            else
            {
                throw new BusinessException("无效的排序方式");
            }

            list.Add(dragDao);
            await _thisRepository.UpdateRangeAsync(list);
        }

        /// <summary>
        /// 删除,支持多个
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(string ids)
        {
            return await _thisRepository.DeleteAsync(m => ids.ToListLong().Contains(m.id));
        }
    }
}