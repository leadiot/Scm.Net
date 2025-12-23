using Com.Scm.Dsa;
using Com.Scm.Msg.Comment;
using Com.Scm.Msg.CommentDetail.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Msg.CommentDetail
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Msg")]
    public class ScmMsgCommentDetailService : ApiService
    {
        private readonly SugarRepository<CommentDetailDao> _thisRepository;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userService"></param>
        public ScmMsgCommentDetailService(SugarRepository<CommentDetailDao> thisRepository, IUserHolder userService)
        {
            _thisRepository = thisRepository;
            _UserService = userService;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<CommentDetailDvo>> GetPagesAsync(SearchRequest request)
        {
            var query = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .OrderBy(m => m.id)
                .Select<CommentDetailDvo>()
                .ToPageAsync(request.page, request.limit);
            return query;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommentDetailDvo>> GetListAsync(SearchRequest request)
        {
            var list = await _thisRepository.AsQueryable()
                .Where(a => a.comment_id == request.commentId && a.row_status == request.row_status)
                .OrderBy(a => a.id)
                .Select<CommentDetailDvo>()
                .ToListAsync();
            return list;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommentDetailDvo>> GetCommentAsync(GetCommentRequest request)
        {
            List<CommentDetailDvo> list;

            var headerDao = await _thisRepository.Change<CommentHeaderDao>()
                .GetFirstAsync(a => a.codec == request.codec && a.row_status == Enums.ScmRowStatusEnum.Enabled);
            if (headerDao == null)
            {
                list = new List<CommentDetailDvo>();
            }
            else
            {
                list = await _thisRepository.AsQueryable()
                    .Where(a => a.comment_id == headerDao.id && a.row_status == request.row_status)
                    .OrderBy(a => a.id)
                    .Select<CommentDetailDvo>()
                    .ToListAsync();
            }

            Prepare(list);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void Prepare(List<CommentDetailDvo> list)
        {
            foreach (var item in list)
            {
                item.update_names = _UserService.GetUserNames(item.update_user);

                var createDao = _UserService.GetUser(item.create_user);
                if (createDao != null)
                {
                    item.create_names = createDao.names;
                    item.namec = createDao.namec;
                    item.avatar = createDao.avatar;
                }
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CommentDetailDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<CommentDetailDto>();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CommentDetailDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<CommentDetailDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CommentDetailDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<CommentDetailDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(CommentDetailDto model)
        {
            return await _thisRepository.InsertAsync(model.Adapt<CommentDetailDao>());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(CommentDetailDto model)
        {
            return await _thisRepository.UpdateAsync(model.Adapt<CommentDetailDao>());
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="request">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest request)
        {
            return await UpdateStatus(_thisRepository, request.ids, request.status);
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