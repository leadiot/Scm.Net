using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Jwt;
using Com.Scm.Service;
using Com.Scm.Ur.User.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Ur.User
{
    /// <summary>
    /// 管理员表服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Ur")]
    public class ScmUrUserService : ApiService
    {
        private readonly JwtContextHolder _jwtContextHolder;
        private readonly SugarRepository<UserDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="envConfig"></param>
        /// <param name="sqlClient"></param>
        /// <param name="jwtContextHolder"></param>
        /// <param name="thisRepository"></param>
        public ScmUrUserService(
            EnvConfig envConfig,
            ISqlSugarClient sqlClient,
            JwtContextHolder jwtContextHolder,
            SugarRepository<UserDao> thisRepository,
            IUserService userService)
        {
            _EnvConfig = envConfig;
            _SqlClient = sqlClient;
            _jwtContextHolder = jwtContextHolder;
            _thisRepository = thisRepository;
            _UserService = userService;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<BasicUserDvo>> GetPagesAsync(SearchUserRequest request)
        {
            var token = _jwtContextHolder.GetToken();
            SaveSearch(token.user_id, request);

            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), m => m.row_status == request.row_status)
                .WhereIF(IsNormalId(request.organize_id), m => SqlFunc.Subqueryable<UserOrganizeDao>().Where(r => r.user_id == m.id && r.organize_id == request.organize_id).Any())
                .WhereIF(IsNormalId(request.position_id), m => SqlFunc.Subqueryable<UserPositionDao>().Where(r => r.user_id == m.id && r.position_id == request.position_id).Any())
                .WhereIF(IsNormalId(request.group_id), m => SqlFunc.Subqueryable<UserGroupDao>().Where(r => r.user_id == m.id && r.group_id == request.group_id).Any())
                .WhereIF(IsNormalId(request.role_id), m => SqlFunc.Subqueryable<UserRoleDao>().Where(r => r.user_id == m.id && r.role_id == request.role_id).Any())
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key) || m.cellphone.Contains(request.key) || m.email.Contains(request.key) || m.names.Contains(request.key))
                .OrderBy(m => m.id, OrderByType.Asc)
                .Select<BasicUserDvo>()
                .ToPageAsync(request.page, request.limit);
            if (result.Items.Count == 0)
            {
                return result;
            }

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<BasicUserDvo>> GetListAsync(SearchUserRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), m => m.row_status == request.row_status)
                .WhereIF(IsNormalId(request.organize_id), m => SqlFunc.Subqueryable<UserOrganizeDao>().Where(r => r.user_id == m.id && r.organize_id == request.organize_id).Any())
                .WhereIF(IsNormalId(request.position_id), m => SqlFunc.Subqueryable<UserPositionDao>().Where(r => r.user_id == m.id && r.position_id == request.position_id).Any())
                .WhereIF(IsNormalId(request.group_id), m => SqlFunc.Subqueryable<UserGroupDao>().Where(r => r.user_id == m.id && r.group_id == request.group_id).Any())
                .WhereIF(IsNormalId(request.role_id), m => SqlFunc.Subqueryable<UserRoleDao>().Where(r => r.user_id == m.id && r.role_id == request.role_id).Any())
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key) || m.cellphone.Contains(request.key) || m.email.Contains(request.key) || m.names.Contains(request.key))
                .OrderBy(m => m.id, OrderByType.Asc)
                .Select<BasicUserDvo>()
                .ToListAsync();
            if (result.Count == 0)
            {
                return result;
            }

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<SimpleUserDvo>> GetSimpleAsync(SearchUserRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), m => m.row_status == request.row_status)
                .WhereIF(IsNormalId(request.organize_id), m => SqlFunc.Subqueryable<UserOrganizeDao>().Where(r => r.user_id == m.id && r.organize_id == request.organize_id).Any())
                .WhereIF(IsNormalId(request.position_id), m => SqlFunc.Subqueryable<UserPositionDao>().Where(r => r.user_id == m.id && r.position_id == request.position_id).Any())
                .WhereIF(IsNormalId(request.group_id), m => SqlFunc.Subqueryable<UserGroupDao>().Where(r => r.user_id == m.id && r.group_id == request.group_id).Any())
                .WhereIF(IsNormalId(request.role_id), m => SqlFunc.Subqueryable<UserRoleDao>().Where(r => r.user_id == m.id && r.role_id == request.role_id).Any())
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key) || m.cellphone.Contains(request.key) || m.email.Contains(request.key) || m.names.Contains(request.key))
                .OrderBy(m => m.id, OrderByType.Asc)
                .Select<SimpleUserDvo>()
                .ToPageAsync(request.page, request.limit);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ResOptionDvo>> GetOptionAsync()
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id })
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SimpleUserDvo> GetSimpleByIdAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<SimpleUserDvo>()
                .FirstAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserDvo> GetEditAsync(long id)
        {
            var result = await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<UserDvo>()
                .FirstAsync();

            var roleList = await _SqlClient.Queryable<UserRoleDao>()
                .Where(m => m.user_id == id)
                .ToListAsync();
            result.role_list = roleList.Select(m => m.role_id).ToList();

            var groupList = await _thisRepository.Change<UserGroupDao>().GetListAsync(a => a.user_id == id);
            result.group_list = groupList.Select(a => a.group_id).ToList();

            var positionList = await _thisRepository.Change<UserPositionDao>().GetListAsync(a => a.user_id == id);
            result.position_list = positionList.Select(a => a.position_id).ToList();

            var organizeList = await _thisRepository.Change<UserOrganizeDao>().GetListAsync(a => a.user_id == id);
            result.organize_list = organizeList.Select(a => a.organize_id).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserDvo> GetViewAsync(long id)
        {
            var result = await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<UserDvo>()
                .FirstAsync();

            var roleList = await _SqlClient.Queryable<UserRoleDao>()
                .Where(m => m.user_id == id)
                .ToListAsync();
            result.role_list = roleList.Select(m => m.role_id).ToList();

            var groupList = await _thisRepository.Change<UserGroupDao>().GetListAsync(a => a.user_id == id);
            result.group_list = groupList.Select(a => a.group_id).ToList();

            var positionList = await _thisRepository.Change<UserPositionDao>().GetListAsync(a => a.user_id == id);
            result.position_list = positionList.Select(a => a.position_id).ToList();

            var organizeList = await _thisRepository.Change<UserOrganizeDao>().GetListAsync(a => a.user_id == id);
            result.organize_list = organizeList.Select(a => a.organize_id).ToList();

            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddAsync(UserDto model)
        {
            var token = _jwtContextHolder.GetToken();

            var userDao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            if (userDao != null)
            {
                throw new BusinessException($"已存在编码为{model.codec}的用户！");
            }
            userDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
            if (userDao != null)
            {
                throw new BusinessException($"已存在全称为{model.namec}的用户！");
            }

            //根据角色查询互斥内容
            var roleConflict = await _SqlClient.Queryable<RoleConflictDao>().ToListAsync();
            if (roleConflict.Count > 0)
            {
                if (roleConflict.Any(item => model.role_list.Contains(item.rolea_id) && model.role_list.Contains(item.roleb_id)))
                {
                    throw new BusinessException("角色存在互斥关系，无法添加！~");
                }
            }
            //查询角色列表
            var roleList = await _SqlClient.Queryable<RoleDao>()
                .Where(a => model.role_list.Contains(a.id))
                .ToListAsync();
            //查询已授权角色信息
            var userRoleList = await _SqlClient.Queryable<UserRoleDao>()
                .Where(m => model.role_list.Contains(m.role_id))
                .ToListAsync();
            foreach (var item in roleList)
            {
                if (item is { MaxLength: 0 })
                {
                    continue;
                }

                if (item != null && item.MaxLength <= userRoleList.Count(m => m.role_id == item.id))
                {
                    throw new BusinessException("[" + item.namec + "]-已达到角色设置最大边界值！~");
                }
            }

            userDao = model.Adapt<UserDao>();
            userDao.user = model.codec;

            await _thisRepository.InsertAsync(userDao);

            // 角色
            await SaveUserRole(model);

            // 部门更新
            await SaveUserOrganize(model);

            // 岗位更新
            await SaveUserPosition(model);

            // 群组更新
            await SaveUserGroup(model);
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task PassResetAsync(List<long> id, string pass)
        {
            if (string.IsNullOrWhiteSpace(pass))
            {
                pass = _EnvConfig.GetPassword();
            }
            var newPass = SecUtils.Sha256(pass);
            newPass = SecUtils.EncodePass(newPass);
            await _thisRepository.UpdateAsync(m => new UserDao()
            {
                pass = newPass
            }, m => id.Contains(m.id));
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UserDto model)
        {
            var token = _jwtContextHolder.GetToken();

            var userDao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            if (userDao != null)
            {
                throw new BusinessException($"已存在编码为{model.codec}的用户！");
            }
            userDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
            if (userDao != null)
            {
                throw new BusinessException($"已存在全称为{model.codec}的用户！");
            }
            userDao = await _thisRepository.GetByIdAsync(model.id);
            if (userDao == null)
            {
                throw new BusinessException("无效的用户信息！");
            }

            //根据角色查询互斥内容
            var roleConflict = await _SqlClient.Queryable<RoleConflictDao>().ToListAsync();
            if (roleConflict.Count > 0)
            {
                if (roleConflict.Any(item => model.role_list.Contains(item.rolea_id) && model.role_list.Contains(item.roleb_id)))
                {
                    throw new BusinessException("角色存在互斥关系，无法添加！~");
                }
            }

            //查询角色列表
            var roleList = await _SqlClient.Queryable<RoleDao>()
                .Where(a => model.role_list.Contains(a.id))
                .ToListAsync();
            //查询已授权角色信息
            var userRoleList = await _SqlClient.Queryable<UserRoleDao>().Where(m => model.role_list.Contains(m.role_id)).ToListAsync();
            foreach (var roleDao in roleList)
            {
                if (roleDao is { MaxLength: 0 })
                {
                    continue;
                }

                if (roleDao != null && roleDao.MaxLength <= userRoleList.Count(m => m.role_id == roleDao.id))
                {
                    throw new BusinessException("[" + roleDao.namec + "]-已达到角色设置最大边界值！~");
                }
            }

            userDao = CommonUtils.Adapt(model, userDao);
            userDao.user = model.codec;

            await _thisRepository.AsUpdateable(userDao).IgnoreColumns(a => new { a.pass }).ExecuteCommandAsync();

            // 角色
            await SaveUserRole(model);

            // 部门更新
            await SaveUserOrganize(model);

            // 岗位更新
            await SaveUserPosition(model);

            // 群组更新
            await SaveUserGroup(model);

            _UserService.Remove(userDao.id);
        }

        /// <summary>
        /// 保存用户角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task SaveUserRole(UserDto model)
        {
            await _SqlClient.Deleteable<UserRoleDao>().Where(m => m.user_id == model.id).ExecuteCommandAsync();
            if (model.role_list != null)
            {
                var userRoleList = new List<UserRoleDao>();
                foreach (var roleId in model.role_list)
                {
                    var userRoleDao = new UserRoleDao();
                    userRoleDao.user_id = model.id;
                    userRoleDao.role_id = roleId;
                    userRoleList.Add(userRoleDao);
                }
                await _SqlClient.Insertable(userRoleList).ExecuteCommandAsync();
            }
        }

        /// <summary>
        /// 保存用户部门
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task SaveUserOrganize(UserDto model)
        {
            var userOrganizeRepository = _thisRepository.Change<UserOrganizeDao>();
            await userOrganizeRepository.DeleteAsync(a => a.user_id == model.id);
            if (model.organize_list != null)
            {
                var userOrganizeList = new List<UserOrganizeDao>();
                foreach (var organizeId in model.organize_list)
                {
                    var userOrganizeDao = new UserOrganizeDao();
                    userOrganizeDao.user_id = model.id;
                    userOrganizeDao.organize_id = organizeId;
                    userOrganizeList.Add(userOrganizeDao);
                }
                await userOrganizeRepository.InsertRangeAsync(userOrganizeList);
            }
        }

        /// <summary>
        /// 保存用户岗位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task SaveUserPosition(UserDto model)
        {
            var userPositionRepository = _thisRepository.Change<UserPositionDao>();
            await userPositionRepository.DeleteAsync(a => a.user_id == model.id);
            if (model.position_list != null)
            {
                var userPositionList = new List<UserPositionDao>();
                foreach (var positionId in model.position_list)
                {
                    var userPositionDao = new UserPositionDao();
                    userPositionDao.user_id = model.id;
                    userPositionDao.position_id = positionId;
                    userPositionList.Add(userPositionDao);
                }
                await userPositionRepository.InsertRangeAsync(userPositionList);
            }
        }

        /// <summary>
        /// 更新用户群组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task SaveUserGroup(UserDto model)
        {
            var userGroupRepository = _thisRepository.Change<UserGroupDao>();
            await userGroupRepository.DeleteAsync(a => a.user_id == model.id);
            if (model.group_list != null)
            {
                var userGroupList = new List<UserGroupDao>();
                foreach (var groupId in model.group_list)
                {
                    var userGroupDao = new UserGroupDao();
                    userGroupDao.user_id = model.id;
                    userGroupDao.group_id = groupId;
                    userGroupList.Add(userGroupDao);
                }
                await userGroupRepository.InsertRangeAsync(userGroupList);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateUserDataAsync(UserDvo model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }
            dao.data = model.data;
            await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 更新记录状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
        }

        /// <summary>
        /// 删除记录,支持多个
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

            var idList = ids.ToListLong();

            var qty = await _thisRepository.AsUpdateable()
                .SetColumns(a => a.row_delete == ScmDeleteEnum.Yes)
                .Where(a => idList.Contains(a.id))
                .ExecuteCommandAsync();

            foreach (var id in idList)
            {
                _UserService.Remove(id);
            }

            return qty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ScmExportResponse> ExportAllAsync()
        {
            var token = _jwtContextHolder.GetToken();

            var response = new ScmExportResponse();

            SearchUserRequest request = ReadSearch<SearchUserRequest>(token.user_id);
            if (request == null)
            {
                response.SetFailure("请刷新页面后重试！");
                return response;
            }

            var handler = new UserExportHandler(request);
            await SaveExportTask(_thisRepository, handler);

            response.SetSuccess("任务生成成功！");
            return response;
        }
    }
}