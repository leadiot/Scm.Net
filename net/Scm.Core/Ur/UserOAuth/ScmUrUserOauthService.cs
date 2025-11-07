using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Jwt;
using Com.Scm.Log;
using Com.Scm.Operator.Dvo;
using Com.Scm.Operator.Oidc;
using Com.Scm.Service;
using Com.Scm.Ur.UserOAuth.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Ur.UserOAuth
{
    /// <summary>
    /// 三方登录服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Ur")]
    public class ScmUrUserOauthService : ApiService
    {
        private readonly JwtContextHolder _contextHolder;
        private readonly SugarRepository<UserOAuthDao> _thisRepository;
        private readonly SugarRepository<UserDao> _userRepository;
        private readonly SugarRepository<LogOidcDao> _logOAuthRepository;
        private readonly OidcConfig _oidcConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextHolder"></param>
        /// <param name="thisRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="oauthRepository"></param>
        /// <returns></returns>
        public ScmUrUserOauthService(JwtContextHolder contextHolder,
            SugarRepository<UserOAuthDao> thisRepository, SugarRepository<UserDao> userRepository,
            SugarRepository<LogOidcDao> oauthRepository, OidcConfig oidcConfig)
        {
            _contextHolder = contextHolder;
            _thisRepository = thisRepository;
            _userRepository = userRepository;
            _logOAuthRepository = oauthRepository;
            _oidcConfig = oidcConfig;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<UserOAuthDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<UserOAuthDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<UserOAuthDvo>> GetListAsync(ScmSearchRequest request)
        {
            var token = _contextHolder.GetToken();

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.user_id == token.user_id && a.row_status == ScmRowStatusEnum.Enabled)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.od)
                .Select<UserOAuthDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void Prepare(List<UserOAuthDvo> list)
        {
            foreach (var item in list)
            {
                var updateDao = _userRepository.GetById(item.update_user);
                item.update_names = updateDao?.names;

                var createDao = _userRepository.GetById(item.create_user);
                item.create_names = createDao?.names;
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserOAuthDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<UserOAuthDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserOAuthDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<UserOAuthDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserOAuthDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<UserOAuthDvo>()
                .FirstAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(UserOAuthDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在编码为{model.codec}的三方登录！");
            //}

            //if (string.IsNullOrWhiteSpace(model.names))
            //{
            //    model.names = model.namec;
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.names == model.names);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在简称为{model.names}的三方登录！");
            //}

            return await _thisRepository.InsertAsync(model.Adapt<UserOAuthDao>());
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BindResponse> DoBindAsync(BindRequest request)
        {
            var token = _contextHolder.GetToken();
            var response = new BindResponse();

            if (string.IsNullOrWhiteSpace(request.code))
            {
                response.SetFailure(LoginResponse.ERROR_42, "无效的联合登录信息！");
                return null;
            }

            OidcUserInfo oidcUser = null;
            try
            {
                var body = new Dictionary<string, string>()
                {
                    ["grant_type"] = "authorization_code",
                    ["code"] = request.code,
                    ["client_id"] = _oidcConfig.app_key,
                    ["client_secret"] = _oidcConfig.app_secret,
                    ["redirect_uri"] = _oidcConfig.redirect_uri
                };
                var oidcResponse = await HttpUtils.PostFormObjectAsync<OidcAccessTokenResponse>(_oidcConfig.token_uri, body);
                if (oidcResponse == null)
                {
                    response.SetFailure(LoginResponse.ERROR_41, "OIDC服务访问异常，请稍后重试！");
                    return null;
                }
                if (!oidcResponse.IsSuccess())
                {
                    response.SetFailure(LoginResponse.ERROR_44, oidcResponse.GetMessage());
                    return null;
                }

                oidcUser = oidcResponse.User;
                if (oidcUser == null)
                {
                    response.SetFailure(LoginResponse.ERROR_42, "无效的联合登录信息！");
                    return null;
                }

                //var logOAuthDao = new LogOAuthDao();
                //logOAuthDao.provider = "oidc:" + oidcUser.Osp;
                //logOAuthDao.code = request.code;
                //logOAuthDao.state = request.state;
                //logOAuthDao.scope = "openid";
                //logOAuthDao.user = oidcUser.Code;
                //logOAuthDao.name = oidcUser.Name;
                //logOAuthDao.avatar = oidcUser.GetAvatarUrl();
                //logOAuthDao.access_token = oidcResponse.access_token;
                //logOAuthDao.refresh_token = oidcResponse.refresh_token;
                //logOAuthDao.expires_in = oidcResponse.expires_in;
                //await _SqlClient.Insertable(logOAuthDao).ExecuteCommandAsync();
            }
            catch (Exception exp)
            {
                response.SetFailure(LoginResponse.ERROR_41, exp.Message);
                return null;
            }

            var userOAuthListDao = await _thisRepository.GetListAsync(a => a.oauth_id == oidcUser.Code && a.row_status == ScmRowStatusEnum.Enabled);
            if (userOAuthListDao != null && userOAuthListDao.Count > 0)
            {
                if (userOAuthListDao.Count > 1)
                {
                    response.SetFailure(BindResponse.ERROR_03, "联合登录已绑定多个账户，不能重复绑定！");
                    return response;
                }
                if (userOAuthListDao[0].user_id != token.user_id)
                {
                    response.SetFailure(BindResponse.ERROR_04, "联合登录已绑定其它账户，不能重复绑定！");
                    return response;
                }

                response.SetSuccess("联合登录已绑定，请勿重复操作！");
                return response;
            }

            userOAuthListDao = await _thisRepository.GetListAsync(a => a.user_id == token.user_id);

            var userOAuthDao = new UserOAuthDao();
            userOAuthDao.od = userOAuthListDao.Count;
            userOAuthDao.user_id = token.user_id;
            userOAuthDao.provider = request.osp_name;
            userOAuthDao.oauth_id = oidcUser.Code;
            userOAuthDao.user = oidcUser.Code;
            userOAuthDao.name = oidcUser.Name;
            userOAuthDao.avatar = "http://www.oidc.org.cn" + request.avatar;
            await _thisRepository.InsertAsync(userOAuthDao);

            response.SetSuccess("联合登录绑定成功！");
            return response;
        }

        /// <summary>
        /// 解绑
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BindResponse> UnBindAsync(BindRequest request)
        {
            var token = _contextHolder.GetToken();

            var response = new BindResponse();

            var time = TimeUtils.GetUnixTime();

            var result = await _thisRepository.AsUpdateable()
                .SetColumns(a => a.row_status == ScmRowStatusEnum.Disabled)
                .SetColumns(a => a.update_time == time)
                .Where(a => a.id == request.id)
                .ExecuteCommandAsync();

            response.SetSuccess("解绑成功！");
            return response;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UserOAuthDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在编码为{model.codec}的三方登录！");
            //}

            //if (string.IsNullOrWhiteSpace(model.names))
            //{
            //    model.names = model.namec;
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.names == model.names && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在简称为{model.names}的三方登录！");
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