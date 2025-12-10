using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Samples.Book;
using Com.Scm.Samples.PoDetail.Dao;
using Com.Scm.Samples.PoDetail.Dto;
using Com.Scm.Samples.PoDetail.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Samples.PoDetail
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Samples")]
    public class SamplesPoDetailService : ApiService
    {
        private readonly SugarRepository<SamplesPoDetailDao> _thisRepository;
        private readonly IBookService _BookService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public SamplesPoDetailService(SugarRepository<SamplesPoDetailDao> thisRepository,
            IUserService userService,
            IBookService bookService)
        {
            _thisRepository = thisRepository;
            _UserService = userService;
            _BookService = bookService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<SamplesPoDetailDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.header_id == request.id)
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<SamplesPoDetailDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<SamplesPoDetailDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled && a.header_id == request.id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<SamplesPoDetailDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        private void Prepare(List<SamplesPoDetailDvo> details)
        {
            foreach (var dvo in details)
            {
                Prepare(dvo);

                var bookDao = _BookService.GetDaoById(dvo.book_id);
                if (bookDao != null)
                {
                    dvo.book_codes = bookDao.codes;
                    dvo.book_names = bookDao.names;
                }
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SamplesPoDetailDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SamplesPoDetailDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SamplesPoDetailDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SamplesPoDetailDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SamplesPoDetailDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SamplesPoDetailDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(SamplesPoDetailDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            //if (dao != null)
            //{
            //    throw new BusinessException("已存在相同编码的！");
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
            //if (dao != null)
            //{
            //    throw new BusinessException("已存在相同名称的！");
            //}

            var dao = model.Adapt<SamplesPoDetailDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(SamplesPoDetailDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException("已存在相同编码的！");
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException("已存在相同名称的！");
            //}

            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的！");
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> BatchAppendAsync(BatchAppendRequest request)
        {
            var detailDaoList = await _thisRepository.GetListAsync(a => a.header_id == request.id, a => a.od);
            var updateList = new List<SamplesPoDetailDao>();
            var appendList = new List<SamplesPoDetailDao>();
            var idx = detailDaoList.Count;
            foreach (var id in request.items)
            {
                var detailDao = detailDaoList.FirstOrDefault(a => a.book_id == id);
                if (detailDao != null)
                {
                    // 从逻辑删除恢复时，视为新增
                    detailDao.need_qty = request.qty;
                    detailDao.row_status = Enums.ScmRowStatusEnum.Enabled;
                    updateList.Add(detailDao);
                    continue;
                }

                detailDao = new SamplesPoDetailDao();
                detailDao.header_id = request.id;
                detailDao.book_id = id;
                detailDao.need_qty = request.qty;
                detailDao.od = idx;
                appendList.Add(detailDao);

                idx += 1;
            }

            await _thisRepository.UpdateRangeAsync(updateList);
            await _thisRepository.InsertRangeAsync(appendList);

            return true;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> BatchUpdateAsync(BatchUpdateRequest request)
        {
            var detailDaoList = await _thisRepository.GetListAsync(a => a.header_id == request.id, a => a.od);
            var updateList = new List<SamplesPoDetailDao>();
            foreach (var item in request.items)
            {
                var detailDao = detailDaoList.FirstOrDefault(a => a.id == item.id);
                if (detailDao != null)
                {
                    detailDao.need_qty = item.qty;
                    detailDao.row_status = Enums.ScmRowStatusEnum.Enabled;
                    updateList.Add(detailDao);
                    continue;
                }
            }

            await _thisRepository.UpdateRangeAsync(updateList);

            return true;
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
            return await DeleteRecord(_thisRepository, ids.ToListLong());
        }
    }
}