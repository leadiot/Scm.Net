using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Msg.Chat.Group.Dvo;
using Com.Scm.Service;
using Com.Scm.Ur;
using Com.Scm.Ur.User.Dvo;
using Com.Scm.Utils;

using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Msg.Chat.Group
{
    /// <summary>
    /// 群组服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Chat")]
    public class ScmMsgChatGroupService : ApiService
    {
        private readonly SugarRepository<ChatGroupDao> _thisRepository;
        private readonly SugarRepository<ChatGroupUserDao> _groupUserRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="groupUserRepository"></param>
        /// <param name="userRepository"></param>
        /// <returns></returns>
        public ScmMsgChatGroupService(SugarRepository<ChatGroupDao> thisRepository,
            SugarRepository<ChatGroupUserDao> groupUserRepository,
            IResHolder userService)
        {
            _thisRepository = thisRepository;
            _groupUserRepository = groupUserRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ChatGroupDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<ChatGroupDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ChatGroupDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<ChatGroupDvo>()
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
        public async Task<ChatGroupDvo> GetAsync(long id)
        {
            var groupDvo = await _thisRepository.AsQueryable()
                .Where(a => a.id == id)
                .Select<ChatGroupDvo>()
                .FirstAsync();

            if (groupDvo != null)
            {
                var result = await _groupUserRepository.AsQueryable()
                    .Where(a => a.row_status == ScmRowStatusEnum.Enabled && a.group_id == id)
                    .OrderBy(a => a.id)
                    .ToListAsync();

                var userIds = result.Select(a => a.user_id);
                groupDvo.users = await _thisRepository.Change<UserDao>()
                    .AsQueryable()
                    .Where(a => userIds.Contains(a.id))
                    .Select<SimpleUserDvo>()
                    .ToListAsync();
            }

            return groupDvo;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ChatGroupDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ChatGroupDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ChatGroupDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ChatGroupDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 创建群组
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task CreateAsync(CreateGroupRequest request)
        {
            var groupDao = new ChatGroupDao();
            groupDao.types = request.types;
            groupDao.namec = request.namec;
            await _thisRepository.InsertAsync(groupDao);

            var userListDao = new List<ChatGroupUserDao>();
            foreach (var userId in request.users)
            {
                //var namec = "";
                //var friend = "";
                //var user = _userRepository.GetById(userId);
                //if (user == null)
                //{
                //    continue;
                //}
                //else
                //{
                //    namec = user.namec;
                //}

                var groupUser = new ChatGroupUserDao();
                groupUser.group_id = groupDao.id;
                groupUser.user_id = userId;
                userListDao.Add(groupUser);
            }
            await _groupUserRepository.InsertRangeAsync(userListDao);

            groupDao.qty = userListDao.Count;
            await _thisRepository.UpdateAsync(groupDao);
        }

        /// <summary>
        /// 追加人员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AppendAsync(CreateGroupRequest request)
        {
            var groupDao = await _thisRepository.GetByIdAsync(request.id);
            if (groupDao == null)
            {
                return;
            }

            var oldListDao = await _groupUserRepository.GetListAsync(a => a.group_id == request.id);

            var newListDao = new List<ChatGroupUserDao>();
            foreach (var user in request.users)
            {
                var groupUser = oldListDao.Find(a => a.user_id == user);
                if (groupUser != null)
                {
                    continue;
                }

                groupUser = new ChatGroupUserDao();
                groupUser.group_id = groupDao.id;
                groupUser.user_id = user;
                newListDao.Add(groupUser);
            }
            await _groupUserRepository.InsertRangeAsync(newListDao);

            groupDao.qty += newListDao.Count;
            await _thisRepository.UpdateAsync(groupDao);
        }

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task Remove0Async(CreateGroupRequest request)
        {
            var groupDao = await _thisRepository.GetByIdAsync(request.id);
            if (groupDao == null)
            {
                return;
            }

            await _groupUserRepository.DeleteAsync(a => request.users.Contains(a.user_id));

            var list = await _groupUserRepository.GetListAsync(a => a.group_id == request.id);

            groupDao.qty = list.Count;
            await _thisRepository.UpdateAsync(groupDao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ChatGroupDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在编码为{model.codec}的群组！");
            //}

            //if (string.IsNullOrWhiteSpace(model.names))
            //{
            //    model.names = model.namec;
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.names == model.names && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在简称为{model.names}的群组！");
            //}

            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException($"无效的数据信息，更新失败！");
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
            return await DeleteRecord(_thisRepository, ids.ToListLong());
        }
    }
}