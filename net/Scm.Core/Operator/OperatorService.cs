using Com.Scm.Api;
using Com.Scm.Cfg;
using Com.Scm.Cfg.DateTheme;
using Com.Scm.Cfg.UserTheme;
using Com.Scm.Config;
using Com.Scm.DynamicWebApi.Attributes;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Jwt;
using Com.Scm.Log;
using Com.Scm.Msg.Message;
using Com.Scm.Operator.Dvo;
using Com.Scm.Operator.Oidc;
using Com.Scm.Service;
using Com.Scm.Sys.Config;
using Com.Scm.Sys.Menu;
using Com.Scm.Sys.Theme;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Operator;

/// <summary>
/// 操作人服务
/// 
/// 说明：
/// 用户登录时，前端务必确认所属机构信息。
/// 此机构信息可以是用户登录时选择，也可以是前端进行配置。
/// 2025-10-09
/// </summary>
[ApiExplorerSettings(GroupName = "Scm")]
public class OperatorService : ApiService
{
    /// <summary>
    /// 指定机构下，新增用户时，默认参照的用户；
    /// </summary>
    private const string CFG_TEMPLATE_USER = "template_user";
    /// <summary>
    /// 指定机构下，新增用户时，默认分配的角色；
    /// </summary>
    private const string CFG_TEMPLATE_USER_ROLE = "template_user_role";
    /// <summary>
    /// 指定机构下，新增用户时，默认分配的权限；
    /// </summary>
    private const string CFG_TEMPLATE_USER_DATA = "template_user_data";

    private readonly JwtContextHolder _jwtContextHolder;
    private readonly ILogService _logService;
    private readonly ISmsService _smsService;
    private readonly OidcConfig _oidcConfig;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sqlClient"></param>
    /// <param name="envConfig"></param>
    /// <param name="cacheService"></param>
    /// <param name="jwtContextHolder"></param>
    /// <param name="logService"></param>
    /// <param name="smsService"></param>
    public OperatorService(ISqlSugarClient sqlClient
        , EnvConfig envConfig
        , Cache.ICacheService cacheService
        , JwtContextHolder jwtContextHolder
        , ILogService logService
        , ISmsService smsService
        , OidcConfig oidcConfig)
    {
        _SqlClient = sqlClient;
        _EnvConfig = envConfig;
        _CacheService = cacheService;

        _jwtContextHolder = jwtContextHolder;
        _logService = logService;
        _smsService = smsService;
        _oidcConfig = oidcConfig;
    }

    /// <summary>
    /// 登录主题
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public async Task<DateThemeResponse> GetDateThemeAsync(DateThemeRequest request)
    {
        var date = request.GetDate();

        var dateDao = await _SqlClient.Queryable<DateThemeDao>()
            .Where(a => a.dates == date && a.row_status == ScmRowStatusEnum.Enabled)
            .FirstAsync();

        if (dateDao != null)
        {
            var theme1Dao = await _SqlClient.Queryable<ThemeDao>()
                .FirstAsync(a => a.id == dateDao.theme_id);

            if (theme1Dao != null)
            {
                var theme1 = theme1Dao.theme.AsJsonObject<DateThemeResponse>();
                return theme1;
            }

            return new DateThemeResponse();
        }

        var listDao = await _SqlClient.Queryable<ThemeDao>()
            .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
            .ToListAsync();
        if (listDao.Count < 1)
        {
            return new DateThemeResponse();
        }

        var idx = new Random().Next(listDao.Count);
        var themeDao = listDao[idx];
        dateDao = new DateThemeDao();
        dateDao.dates = date;
        dateDao.theme_id = themeDao.id;
        await _SqlClient.Insertable(dateDao).ExecuteCommandAsync();

        var theme2 = themeDao.theme.AsJsonObject<DateThemeResponse>();
        return theme2;
    }

    #region 用户登录
    /// <summary>
    /// 用户登录
    /// 说明：
    /// 所有的用户都会默认从属于某个机构，用户登录时，必需携带机构信息。
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var response = new LoginResponse();

        UserDao userDao = null;

        if (request.mode == ScmLoginModeEnum.ByPass)
        {
            userDao = await LoginByPwdAsync(request, response);
        }
        else if (request.mode == ScmLoginModeEnum.ByPhone || request.mode == ScmLoginModeEnum.ByEmail)
        {
            userDao = await LoginBySmsAsync(request, response);
        }
        else if (request.mode == ScmLoginModeEnum.ByOauth)
        {
            userDao = await LoginByOAuthAsync(request, response);
        }
        else
        {
            LogUtils.Info($"用户登录：无效的登录模式{request.mode}！");
            response.SetFailure(LoginResponse.ERROR_01, $"无效的登录模式！");
            return response;
        }

        if (userDao == null)
        {
            return response;
        }

        response.accessToken = GetToken(userDao, request.code);
        response.userInfo = GetUserInfo(userDao);
        response.userTheme = await GetTheme(userDao);

        #region 登录日志收集
        var userAgent = ServerUtils.GetUserAgent();
        var now = DateTime.Now;
        await _logService.LogAsync(new LogInfo()
        {
            level = ScmLogLevelEnum.Info,
            types = ScmLogTypesEnum.Login,
            module = "Login",
            method = "Login",
            operate_user = userDao.namec,
            operate_date = TimeUtils.FormatDate(now),
            operate_time = TimeUtils.GetUnixTime(now),
            parameters = request.ToJsonString(),
            ip = ServerUtils.GetIp(),
            url = "/api/login",
            browser = ServerUtils.GetBrowser(userAgent),
            agent = userAgent,
        });
        #endregion

        await LogUser(request, userDao, 0, "登录成功！");

        response.SetSuccess();
        return response;
    }

    /// <summary>
    /// 记录登录日志
    /// </summary>
    private async Task LogUser(LoginRequest request, UserDao userDao, int code, string remark)
    {
        var logDao = new LogUserDao();
        logDao.user_id = userDao.id;
        logDao.client = ScmClientTypeEnum.Web;
        logDao.mode = request.mode;
        logDao.time = TimeUtils.GetUnixTime();
        logDao.ip = ServerUtils.GetIp();
        logDao.result = code == 0 ? ScmBoolEnum.True : ScmBoolEnum.False;
        logDao.code = code;
        logDao.remark = remark;
        await _SqlClient.Insertable(logDao).ExecuteCommandAsync();
    }

    /// <summary>
    /// 口令登录
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    [NonDynamicMethod]
    private async Task<UserDao> LoginByPwdAsync(LoginRequest request, LoginResponse response)
    {
        var config = AppUtils.GetConfig<OperatorConfig>("Operator");
        if (config == null || !config.IgnoreCaptcha)
        {
            var code = _CacheService.GetCache(KeyUtils.CAPTCHACODE + request.key);
            if (!string.Equals(code, request.code, StringComparison.CurrentCultureIgnoreCase))
            {
                response.SetFailure(LoginResponse.ERROR_11, "验证码输入错误！");
                return null;
            }
        }

        var user = request.user;
        if (!ScmUtils.IsValidUser(user))
        {
            response.SetFailure(LoginResponse.ERROR_12, "无效的登录用户！");
            return null;
        }
        var pass = request.pass;
        if (!ScmUtils.IsValidPass(request.pass))
        {
            response.SetFailure(LoginResponse.ERROR_13, "无效的登录密码！");
            return null;
        }

        var userDao = await _SqlClient.Queryable<UserDao>()
            .Where(a => a.codec == user && a.row_status == ScmRowStatusEnum.Enabled)
            .FirstAsync();
        // 检测用户信息
        if (userDao == null)
        {
            response.SetFailure(LoginResponse.ERROR_14, "账号密码输入错误！");
            return null;
        }

        // 检测账户状态
        if (!userDao.IsEnabled())
        {
            var msg = "账号被冻结，请联系管理员！";
            response.SetFailure(LoginResponse.ERROR_04, msg);
            await LogUser(request, userDao, LoginResponse.ERROR_04, msg);
            return null;
        }

        var time = TimeUtils.GetUnixTime();

        // 检测是否限制登录
        if (userDao.next_time > time)
        {
            var msg = $"请于 {TimeUtils.FormatDataTime(userDao.next_time)} 后重试！";
            response.SetFailure(LoginResponse.ERROR_05, msg);
            await LogUser(request, userDao, LoginResponse.ERROR_05, msg);
            return null;
        }

        userDao.DecodePass();
        if (pass != SecUtils.Sha256(userDao.pass + "@" + request.time))
        {
            await LoginPassError(userDao, time);
            var msg = "账号密码输入错误！";
            response.SetFailure(LoginResponse.ERROR_14, msg);
            await LogUser(request, userDao, LoginResponse.ERROR_14, msg);
            return null;
        }

        userDao.login_count += 1;
        userDao.last_time = userDao.login_time;
        userDao.login_time = time;
        userDao.error_count = 0;
        userDao.PrepareUpdate(UserDto.SYS_ID);
        await _SqlClient.Updateable(userDao)
            .UpdateColumns(a => new { a.login_count, a.login_time, a.last_time, a.error_count })
            .ExecuteCommandAsync();

        return userDao;
    }

    /// <summary>
    /// 验证码登录
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task<UserDao> LoginBySmsAsync(LoginRequest request, LoginResponse response)
    {
        var code = "";
        if (request.mode == ScmLoginModeEnum.ByPhone)
        {
            if (!TextUtils.IsCellphone(request.phone))
            {
                response.SetFailure(LoginResponse.ERROR_21, "无效的手机号码！");
                return null;
            }
            code = request.phone;
        }
        else if (request.mode == ScmLoginModeEnum.ByEmail)
        {
            if (!TextUtils.IsEmail(request.email))
            {
                response.SetFailure(LoginResponse.ERROR_31, "无效的电子邮件！");
                return null;
            }
            code = request.email;
        }
        else
        {
            response.SetFailure(LoginResponse.ERROR_06, "不支持的登录方式！");
            return null;
        }

        var result = await _smsService.VerifySmsAsync(request.key, request.code);
        if (result.HasError())
        {
            response.SetFailure(LoginResponse.ERROR_22, result.Text);
            return null;
        }

        var userDao = await _SqlClient.Queryable<UserDao>()
            .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
            .WhereIF(request.mode == ScmLoginModeEnum.ByPhone, a => a.cellphone == code)
            .WhereIF(request.mode == ScmLoginModeEnum.ByEmail, a => a.email == code)
            .FirstAsync();

        // 用户已存在
        if (userDao != null)
        {
            // 检测账户状态
            if (!userDao.IsEnabled())
            {
                var msg = "账号被冻结，请联系管理员！";
                response.SetFailure(LoginResponse.ERROR_04, msg);
                await LogUser(request, userDao, LoginResponse.ERROR_04, msg);
                return null;
            }

            var time = TimeUtils.GetUnixTime();

            // 检测是否限制登录
            if (userDao.next_time > time)
            {
                var msg = $"请于 {TimeUtils.FormatDataTime(userDao.next_time)} 后重试！";
                response.SetFailure(LoginResponse.ERROR_05, msg);
                await LogUser(request, userDao, LoginResponse.ERROR_05, msg);
                return null;
            }

            userDao.login_count += 1;
            userDao.last_time = userDao.login_time;
            userDao.login_time = time;
            userDao.error_count = 0;
            await _SqlClient.Updateable(userDao)
                .UpdateColumns(a => new { a.login_count, a.login_time, a.last_time, a.error_count })
                .ExecuteCommandAsync();

            return userDao;
        }

        if (!request.auto)
        {
            LogUtils.Info($"用户登录：无效的用户账户{request.user}！");
            response.SetFailure(LoginResponse.ERROR_01, $"无效的用户账户{request.user}！");
            return null;
        }

        // 创建新用户
        var temp = TimeUtils.GetUnixTime().ToString();
        var name = "user-" + temp;

        userDao = new UserDao();
        userDao.codec = temp;
        userDao.names = name;
        userDao.namec = name;
        //userDao.pass = CryptoUtils.Sha(request.pass);
        userDao.email = request.email;
        userDao.cellphone = request.phone;
        userDao.UseDefaultPass();
        userDao.UseDefaultAvatar();
        var qty = await _SqlClient.Insertable(userDao).ExecuteCommandAsync();
        if (qty < 1)
        {
            LogUtils.Info($"用户注册：用户{userDao.codec}数据库插入异常！");
            response.SetFailure(LoginResponse.ERROR_23, "用户注册失败，请重试！");
            return null;
        }

        await SaveUserRoleFromUser(userDao, response);
        await SaveUserRoleFromRole(userDao, response);

        return userDao;
    }

    /// <summary>
    /// OAuth登录
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task<UserDao> LoginByOAuthAsync(LoginRequest request, LoginResponse response)
    {
        //var state = request.state?.ToLower();
        //if (state != "login")
        //{
        //    response.SetFailure(LoginResponse.ERROR_41, "无效的联合登录信息！");
        //    return null;
        //}
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

            var logOAuthDao = new LogOidcDao();
            logOAuthDao.provider = "oidc:" + oidcUser.Osp;
            logOAuthDao.code = request.code;
            logOAuthDao.state = request.state;
            logOAuthDao.scope = "openid";
            logOAuthDao.user = oidcUser.Code;
            logOAuthDao.name = oidcUser.Name;
            logOAuthDao.avatar = oidcUser.GetAvatarUrl();
            logOAuthDao.access_token = oidcResponse.access_token;
            logOAuthDao.refresh_token = oidcResponse.refresh_token;
            logOAuthDao.expires_in = oidcResponse.expires_in;
            await _SqlClient.Insertable(logOAuthDao).ExecuteCommandAsync();
        }
        catch (Exception exp)
        {
            response.SetFailure(LoginResponse.ERROR_41, exp.Message);
            return null;
        }

        var userOAuthListDao = await _SqlClient.Queryable<UserOAuthDao>()
            .ClearFilter()
            .Where(a => a.oauth_id == oidcUser.Code && a.row_status == ScmRowStatusEnum.Enabled)
            .ToListAsync();
        if (userOAuthListDao == null || userOAuthListDao.Count < 1)
        {
            response.SetFailure(LoginResponse.ERROR_45, "联合登录不存在关联账户！");
            return null;
        }
        if (userOAuthListDao.Count > 1)
        {
            response.SetFailure(LoginResponse.ERROR_46, "联合登录存在多个关联账户！");
            return null;
        }

        var userOAuthDao = userOAuthListDao[0];
        var userDao = await _SqlClient.Queryable<UserDao>()
            .Where(a => a.id == userOAuthDao.user_id)
            .FirstAsync();

        if (userDao != null)
        {
            // 检测账户状态
            if (!userDao.IsEnabled())
            {
                var msg = "账号被冻结，请联系管理员！";
                response.SetFailure(LoginResponse.ERROR_04, msg);
                await LogUser(request, userDao, LoginResponse.ERROR_04, msg);
                return null;
            }

            var time = TimeUtils.GetUnixTime();

            // 检测是否限制登录
            if (userDao.next_time > time)
            {
                var msg = $"请于 {TimeUtils.FormatDataTime(userDao.next_time)} 后重试！";
                response.SetFailure(LoginResponse.ERROR_05, msg);
                await LogUser(request, userDao, LoginResponse.ERROR_05, msg);
                return null;
            }

            userDao.login_count += 1;
            userDao.last_time = userDao.login_time;
            userDao.login_time = time;
            userDao.error_count = 0;
            await _SqlClient.Updateable(userDao)
                .UpdateColumns(a => new { a.login_count, a.login_time, a.last_time, a.error_count })
                .ExecuteCommandAsync();

            return userDao;
        }

        // 用户不存在

        if (!request.auto)
        {
            response.SetFailure(LoginResponse.ERROR_03, "不存在已注册账户！");
            return null;
        }

        // TODO 自动注册

        return userDao;
    }

    /// <summary>
    /// API测试
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    public async Task<LoginResponse> GetSwaggerAsync(string user, string pass)
    {
        if (string.IsNullOrEmpty(user))
        {
            throw new BusinessException("登录用户不能为空！");
        }
        if (string.IsNullOrEmpty(pass))
        {
            throw new BusinessException("登录口令不能为空！");
        }

        pass = SecUtils.Sha256(pass);
        var time = TimeUtils.GetUnixTime();
        pass = SecUtils.Sha256(pass += "@" + time);
        return await LoginAsync(new LoginRequest { mode = ScmLoginModeEnum.ByPass, user = user, pass = pass, time = time });
    }
    #endregion

    #region 用户登出
    /// <summary>
    /// 退出
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ScmApiResponse Logout()
    {
        _jwtContextHolder.Clear();
        return ServerUtils.Success();
    }
    #endregion

    #region 机构信息
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public OperatorUnitDvo GetUnitInfo()
    {
        OperatorUnitDvo dvo = null;

        var file = _EnvConfig.GetDataPath("unit.json");
        if (File.Exists(file))
        {
            var json = FileUtils.ReadText(file);
            if (!string.IsNullOrEmpty(json))
            {
                dvo = json.AsJsonObject<OperatorUnitDvo>();
            }
        }

        return dvo;
    }

    /// <summary>
    /// 工作台机构信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public OperatorUnitWorkResponse GetUnitWorkAsync()
    {
        OperatorUnitWorkResponse dvo = null;

        var file = _EnvConfig.GetDataPath("unit.json");
        if (File.Exists(file))
        {
            var json = FileUtils.ReadText(file);
            if (!string.IsNullOrEmpty(json))
            {
                dvo = json.AsJsonObject<OperatorUnitWorkResponse>();
            }
        }

        return dvo;
    }

    /// <summary>
    /// 修改机构基本信息
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public void UpdateUnitBasicAsync(UnitBasicRequest model)
    {
        var file = _EnvConfig.GetDataPath("unit.json");

        var json = model.ToJsonString();
        using (var stream = File.OpenWrite(file))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }
    }
    #endregion

    #region 用户信息
    /// <summary>
    /// 用户主题
    /// </summary>
    /// <returns></returns>
    public async Task<UserThemeDto> GetUserThemeAsync()
    {
        var userId = _jwtContextHolder.GetToken().user_id;

        var themeDto = await _SqlClient.Queryable<UserThemeDao>()
            .Where(a => a.user_id == userId && a.row_status == ScmRowStatusEnum.Enabled)
            .Select<UserThemeDto>()
            .FirstAsync();

        return themeDto;
    }

    /// <summary>
    /// 查询登录人信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<OperatorInfo> GetUserInfoAsync()
    {
        //var paramToken = _httpContextAccessor.HttpContext?.Request.Headers[JwtToken.TokenName].ToString();
        //if (string.IsNullOrEmpty(paramToken))
        //{
        //    return new OperatorUserDvo();
        //}
        var token = _jwtContextHolder.GetToken();
        var userDao = await _SqlClient.Queryable<UserDao>().Where(a => a.id == token.user_id).FirstAsync();
        return new OperatorInfo()
        {
            Id = userDao.id.ToString(),
            UserId = userDao.id,
            UserName = userDao.namec,
            Avatar = userDao.avatar
        };
    }

    /// <summary>
    /// 工作台用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<OperatorUserWorkResponse> GetUserWorkAsync()
    {
        var token = _jwtContextHolder.GetToken();
        if (token.IsEmpty())
        {
            return new OperatorUserWorkResponse();
        }

        var userDao = await _SqlClient.Queryable<UserDao>().Where(a => a.id == token.user_id).FirstAsync();
        var userOrganizeList = await _SqlClient.Queryable<UserOrganizeDao>().Where(a => a.user_id == token.user_id).ToListAsync();
        var organizeIds = userOrganizeList.Select(a => a.organize_id).ToList();
        var organizeList = await _SqlClient.Queryable<OrganizeDao>().Where(a => organizeIds.Contains(a.id)).ToListAsync();

        var userPositionList = await _SqlClient.Queryable<UserPositionDao>().Where(a => a.user_id == token.user_id).ToListAsync();
        var positionIds = userPositionList.Select(a => a.position_id).ToList();
        var positionList = await _SqlClient.Queryable<PositionDao>().Where(a => positionIds.Contains(a.id)).ToListAsync();

        var userGroupList = await _SqlClient.Queryable<UserGroupDao>().Where(a => a.user_id == token.user_id).ToListAsync();
        var groupIds = userGroupList.Select(a => a.group_id).ToList();
        var groupList = await _SqlClient.Queryable<GroupDao>().Where(a => groupIds.Contains(a.id)).ToListAsync();

        var userRoleList = await _SqlClient.Queryable<UserRoleDao>().Where(a => a.user_id == token.user_id).ToListAsync();
        var roleIds = userRoleList.Select(a => a.role_id).ToList();
        var roleList = await _SqlClient.Queryable<RoleDao>().Where(a => roleIds.Contains(a.id)).ToListAsync();

        var messageCount = await _SqlClient.Queryable<MessageDao>().CountAsync(m => true);
        var agencyCount = await _SqlClient.Queryable<MessageDao>().CountAsync(m => !m.isread);
        return new OperatorUserWorkResponse()
        {
            id = userDao.id,
            codec = userDao.codec,
            names = userDao.names,
            namec = userDao.namec,
            sex = userDao.sex,
            cellphone = userDao.cellphone,
            email = userDao.email,
            avatar = userDao.avatar,
            organize_list = organizeList.Select(a => a.namec).ToList(),
            position_list = positionList.Select(a => a.namec).ToList(),
            group_list = groupList.Select(a => a.names).ToList(),
            role_list = roleList.Select(a => a.names).ToList(),
            remark = userDao.remark,
            lastTime = userDao.last_time.ToString(ScmEnv.FORMAT_DATETIME),
            loginSum = userDao.login_count,
            messageSum = messageCount,
            agencySum = agencyCount
        };
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 读取配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private async Task<ConfigDao> GetConfig(string key)
    {
        return await _SqlClient.Queryable<ConfigDao>().FirstAsync(a => a.key == key);
    }

    private async Task<LogOidcDao> GetLogOAuth(string code)
    {
        return await _SqlClient.Queryable<LogOidcDao>().FirstAsync(a => a.code == code);
    }
    #endregion

    #region 用户注册
    /// <summary>
    /// 用户注册
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    public async Task<SignonResponse> SignOnAsync(SignonRequest request)
    {
        var response = new SignonResponse();

        if (request.mode == ScmLoginModeEnum.ByPass)
        {
            await SignonByPwdAsync(request, response);
        }
        else if (request.mode == ScmLoginModeEnum.ByOauth)
        {
            await SignonByOauthAsync(request, response);
        }

        return response;
    }

    /// <summary>
    /// 口令注册
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    private async Task<bool> SignonByPwdAsync(SignonRequest request, SignonResponse response)
    {
        return await SignonByPwdAsUserAsync(request, response);
    }

    /// <summary>
    /// 个人注册
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task<bool> SignonByPwdAsUserAsync(SignonRequest request, SignonResponse response)
    {
        var user = request.user;
        var userDao = await _SqlClient.Queryable<UserDao>()
            .Where(a => a.codec == user)
            .FirstAsync();
        if (userDao != null)
        {
            LogUtils.Info($"个人注册：已存在相同的用户{request.user}！");
            response.SetFailure(SignonResponse.ERROR_13, $"已存在相同的用户{request.user}！");
            return false;
        }

        userDao = new UserDao();
        userDao.codec = request.user;
        userDao.names = request.user;
        userDao.namec = request.user_name;
        userDao.pass = request.pass;
        userDao.email = request.email;
        userDao.cellphone = request.phone;
        userDao.PrepareCreate(UserDto.SYS_ID);
        var result = await _SqlClient.Insertable(userDao).ExecuteCommandAsync();
        if (result < 1)
        {
            LogUtils.Info($"用户注册：用户{userDao.codec}数据库插入异常！");
            response.SetFailure(SignonResponse.ERROR_15, "用户信息注册失败，请修改名称后重试！");
            return false;
        }

        await SaveUserRoleFromUser(userDao, response);
        await SaveUserRoleFromRole(userDao, response);

        response.SetSuccess();
        return true;
    }

    #region OAuth注册
    /// <summary>
    /// 联合注册
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    private async Task<bool> SignonByOauthAsync(SignonRequest request, SignonResponse response)
    {
        var time = TimeUtils.GetUnixTime();

        var logOAuthDao = await GetLogOAuth(request.code);

        if (logOAuthDao == null)
        {
            response.SetFailure(SignonResponse.ERROR_41, "无效的联合登录信息！");
            return false;
        }

        if (logOAuthDao.expires_in <= time)
        {
            response.SetFailure(SignonResponse.ERROR_42, "联合登录授权已过期，请重新授权！");
            return false;
        }

        // 直接创建
        if (request.opt == SignonOptEnum.Create)
        {
            return await CreateUserWithOAuthAsync(request, response, logOAuthDao);
        }

        var user = request.user;
        if (!ScmUtils.IsValidUser(user))
        {
            response.SetFailure(SignonResponse.ERROR_14, "无效的登录用户，请重新输入！");
            return false;
        }
        var pass = request.pass;
        if (!ScmUtils.IsValidPass(request.pass))
        {
            response.SetFailure(SignonResponse.ERROR_16, "无效的登录密码，请重新输入！");
            return false;
        }

        var userDao = await _SqlClient.Queryable<UserDao>()
              .Where(a => a.codec == user && a.row_status == ScmRowStatusEnum.Enabled)
              .FirstAsync();
        // 检测用户信息
        if (userDao == null)
        {
            response.SetFailure(SignonResponse.ERROR_03, "账户或密码输入错误，请重试！");
            return false;
        }

        // 检测账户状态
        if (!userDao.IsEnabled())
        {
            response.SetFailure(SignonResponse.ERROR_04, "账号被冻结，请联系管理员！");
            return false;
        }

        // 检测是否限制登录
        if (userDao.next_time > time)
        {
            response.SetFailure(SignonResponse.ERROR_05, $"请于 {TimeUtils.FormatDataTime(userDao.next_time)} 后重试！");
            return false;
        }

        userDao.DecodePass();
        if (pass != userDao.pass)
        {
            await LoginPassError(userDao, time);
            response.SetFailure(SignonResponse.ERROR_03, "账户或密码输入错误，请重试！");
            return false;
        }

        var userOauthDaoList = await _SqlClient.Queryable<UserOAuthDao>()
            .Where(a => a.user_id == userDao.id)
            .ToListAsync();
        if (userOauthDaoList == null)
        {
            userOauthDaoList = new List<UserOAuthDao>();
        }

        var userOauthDao = userOauthDaoList.Find(a => a.oauth_id == logOAuthDao.code);
        if (userOauthDao == null)
        {
            await CreateUserOAuthAsync(userDao.id, logOAuthDao, userOauthDaoList.Count);
        }
        else
        {
            userOauthDao.qty += 1;
            await _SqlClient.Updateable(userOauthDao).ExecuteCommandAsync();
        }

        response.accessToken = GetToken(userDao, "");
        response.userInfo = GetUserInfo(userDao);
        response.userTheme = await GetTheme(userDao);

        response.SetSuccess();
        return true;
    }

    /// <summary>
    /// 根据OAuth信息创建用户
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <param name="logOAuthDao"></param>
    /// <returns></returns>
    private async Task<bool> CreateUserWithOAuthAsync(SignonRequest request, SignonResponse response, LogOidcDao logOAuthDao)
    {
        var userDao = new UserDao();
        userDao.codec = logOAuthDao.code;
        userDao.names = logOAuthDao.name;
        userDao.namec = logOAuthDao.name;
        userDao.pass = SecUtils.Sha256(_EnvConfig.GetPassword());
        userDao.email = "";
        userDao.cellphone = "";
        userDao.UseDefaultAvatar();
        userDao.PrepareCreate(UserDto.SYS_ID);
        userDao.EncodePass();
        await _SqlClient.Insertable(userDao).ExecuteCommandAsync();

        await CreateUserOAuthAsync(userDao.id, logOAuthDao);
        var result = await SaveUserRoleFromUser(userDao, response);
        result |= await SaveUserRoleFromRole(userDao, response);
        if (!result)
        {
            return false;
        }

        response.accessToken = GetToken(userDao, "");
        response.userInfo = GetUserInfo(userDao);
        response.userTheme = await GetTheme(userDao);

        response.SetSuccess();
        return true;
    }

    /// <summary>
    /// 记录联合授权信息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="logOAuthDao"></param>
    /// <param name="od"></param>
    /// <returns></returns>
    private async Task<UserOAuthDao> CreateUserOAuthAsync(long userId, LogOidcDao logOAuthDao, int od = 0)
    {
        var userOauthDao = new UserOAuthDao();
        userOauthDao.user_id = userId;
        userOauthDao.od = od;
        userOauthDao.provider = logOAuthDao.provider;
        userOauthDao.oauth_id = logOAuthDao.user;
        userOauthDao.user = logOAuthDao.user;
        userOauthDao.name = logOAuthDao.name;
        //userOauthDao.avatar = logOAuthDao.avatar;
        userOauthDao.qty += 1;
        userOauthDao.PrepareCreate(userId);
        await _SqlClient.Insertable(userOauthDao).ExecuteCommandAsync();

        return userOauthDao;
    }

    /// <summary>
    /// 给用户添加角色
    /// 以系统配置的用户为参照依据
    /// </summary>
    /// <param name="userDao"></param>
    /// <param name="oldUserId"></param>
    /// <returns></returns>
    private async Task<bool> SaveUserRoleFromUser(UserDao userDao, ScmApiResponse response)
    {
        var configDao = await GetConfig(CFG_TEMPLATE_USER);
        if (configDao == null)
        {
            LogUtils.Info($"用户注册：SaveUserRoleFromUser找不到对应的模板角色！");
            response.SetFailure(SignonResponse.ERROR_01, "用户注册失败，请联系网站管理员！");
            return false;
        }
        if (!ScmUtils.IsValidId(configDao.value))
        {
            LogUtils.Info($"用户注册：SaveUserRoleFromUser模板角色数值异常！");
            response.SetFailure(SignonResponse.ERROR_01, "用户注册失败，请联系网站管理员！");
            return false;
        }

        var templateUserId = long.Parse(configDao.value);

        // 更新数据权限
        var templateUserDao = await _SqlClient.Queryable<UserDao>().FirstAsync(a => a.id == templateUserId);
        if (templateUserDao == null)
        {
            LogUtils.Info($"用户注册：SaveUserRoleFromUser模板角色数值异常！");
            response.SetFailure(SignonResponse.ERROR_01, "用户注册失败，请联系网站管理员！");
            return false;
        }
        userDao.data = templateUserDao.data;
        await _SqlClient.UpdateAsync(userDao);

        var roleDaoList = await _SqlClient.Queryable<UserRoleDao>()
            .Where(a => a.user_id == templateUserId && a.row_status == ScmRowStatusEnum.Enabled)
            .OrderBy(a => a.id, OrderByType.Asc)
            .ToListAsync();

        if (roleDaoList.Count < 1)
        {
            LogUtils.Info($"用户注册：角色信息初始化为空！");
            response.SetFailure(SignonResponse.ERROR_01, "用户注册失败，请联系网站管理员！");
            return false;
        }

        var daoList = new List<UserRoleDao>();
        foreach (var roleDao in roleDaoList)
        {
            var userRole = new UserRoleDao();
            userRole.user_id = userDao.id;
            userRole.role_id = roleDao.role_id;
            daoList.Add(userRole);
        }
        await _SqlClient.Insertable(daoList).ExecuteCommandAsync();

        return true;
    }

    /// <summary>
    /// 给用户添加角色
    /// 直接以系统配置的角色为依据
    /// </summary>
    /// <param name="userDao"></param>
    /// <param name="oldUserId"></param>
    /// <returns></returns>
    private async Task<bool> SaveUserRoleFromRole(UserDao userDao, ScmApiResponse response)
    {
        var configDao = await GetConfig(CFG_TEMPLATE_USER_ROLE);
        if (configDao == null)
        {
            LogUtils.Info($"用户注册：SaveUserRoleFromRole找不到对应的模板角色！");
            response.SetFailure(SignonResponse.ERROR_01, "用户注册失败，请联系网站管理员！");
            return false;
        }
        if (string.IsNullOrEmpty(configDao.value))
        {
            LogUtils.Info($"用户注册：SaveUserRoleFromRole模板角色数值异常！");
            response.SetFailure(SignonResponse.ERROR_01, "用户注册失败，请联系网站管理员！");
            return false;
        }

        var roleIdList = configDao.value.Split(',');
        if (roleIdList.Length < 1)
        {
            LogUtils.Info($"用户注册：SaveUserRoleFromRole模板角色数值异常！");
            response.SetFailure(SignonResponse.ERROR_01, "用户注册失败，请联系网站管理员！");
            return false;
        }

        // 更新数据权限
        var data = ScmUserDataEnum.None;
        configDao = await GetConfig(CFG_TEMPLATE_USER_DATA);
        if (configDao != null)
        {
            if (TextUtils.IsInteger(configDao.value))
            {
                data = (ScmUserDataEnum)int.Parse(configDao.value);
            }
            await _SqlClient.UpdateAsync(userDao);
        }

        var daoList = new List<UserRoleDao>();
        foreach (var roleId in roleIdList)
        {
            var userRole = new UserRoleDao();
            userRole.user_id = userDao.id;
            userRole.role_id = long.Parse(roleId);
            daoList.Add(userRole);
        }
        await _SqlClient.Insertable(daoList).ExecuteCommandAsync();

        return true;
    }

    private async Task SaveUserRole(UserDao userDao, long roleId)
    {
        var userRole = new UserRoleDao();
        userRole.user_id = userDao.id;
        userRole.role_id = roleId;
        await _SqlClient.Insertable(userRole).ExecuteCommandAsync();
    }

    /// <summary>
    /// 密码错误
    /// </summary>
    /// <param name="userDao"></param>
    /// <param name="now"></param>
    /// <returns></returns>
    private async Task LoginPassError(UserDao userDao, long now)
    {
        userDao.error_count += 1;
        if (userDao.error_count > 3)
        {
            var nextTime = Math.Pow(2, userDao.error_count - 3) * 5;
            userDao.next_time = (now + (int)nextTime * 60 * 1000);
        }

        userDao.PrepareUpdate(UserDto.SYS_ID);
        await _SqlClient.Updateable(userDao)
            .UpdateColumns(a => new { a.error_count, a.next_time, a.update_time })
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheId">缓存ID</param>
    /// <param name="userDao"></param>
    /// <returns></returns>
    private string GetToken(UserDao userDao, string code)
    {
        var cacheId = TextUtils.GuidString();
        _CacheService.SetCache(cacheId, code);

        return JwtUtils.IssueJwt(new JwtToken()
        {
            id = cacheId,
            user_id = userDao.id,
            user_codes = userDao.codes,
            user_name = userDao.namec,
            time = DateTime.Now,
            data = userDao.data,
            //Role = "Admin",
            //RoleArray = "0"
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userDao"></param>
    /// <returns></returns>
    private OperatorInfo GetUserInfo(UserDao userDao)
    {
        var unitDao = GetUnitInfo();
        if (unitDao == null)
        {
            unitDao = new OperatorUnitDvo();
            unitDao.LoadDef();
        }

        return new OperatorInfo()
        {
            Id = userDao.id.ToString(),
            UserId = userDao.id,
            UserCode = userDao.codec,
            UserName = userDao.namec,
            UnitCode = unitDao.codec,
            UnitName = unitDao.namec,
            Avatar = userDao.avatar
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userDao"></param>
    /// <returns></returns>
    private async Task<UserThemeDto> GetTheme(UserDao userDao)
    {
        var themeDto = await _SqlClient.Queryable<UserThemeDao>()
            .Where(a => a.user_id == userDao.id)
            .Select<UserThemeDto>().FirstAsync();
        return themeDto;
    }
    #endregion
    #endregion

    #region 信息修改
    /// <summary>
    /// 修改个人基本信息
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task UpdateUserBasicAsync(UserBasicRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.names))
        {
            model.names = model.namec;
        }
        var dao = await _SqlClient.Queryable<UserDao>().Where(a => a.id == model.id).FirstAsync();
        if (dao == null)
        {
            return;
        }

        dao = CommonUtils.Adapt(model, dao);
        dao.PrepareUpdate(UserDto.SYS_ID);
        await _SqlClient.Updateable(dao).ExecuteCommandAsync();
    }

    /// <summary>
    /// 修改用户配置
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task UpdateUserThemeAsync(UserThemeRequest model)
    {
        var userId = _jwtContextHolder.GetToken().user_id;

        var dao = await _SqlClient.Queryable<UserThemeDao>().Where(a => a.user_id == userId).FirstAsync();
        if (dao == null)
        {
            dao = model.Adapt<UserThemeDao>();
            await _SqlClient.Insertable(dao).ExecuteCommandAsync();
        }

        dao = model.Adapt(dao);
        await _SqlClient.Updateable(dao).ExecuteCommandAsync();
    }

    /// <summary>
    /// 设置新密码
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task UpdateUserPassAsync(UserPassRequest model)
    {
        var oldPass = SecUtils.Sha256(model.OldPass);
        var userDao = await _SqlClient.Queryable<UserDao>().Where(a => a.id == model.id).FirstAsync();
        if (userDao == null)
        {
            throw new BusinessException("原密码输入错误~");
        }
        userDao.DecodePass();
        if (userDao.pass != oldPass)
        {
            throw new BusinessException("原密码输入错误~");
        }

        userDao.pass = SecUtils.Sha256(model.NewPass);
        userDao.EncodePass();
        userDao.PrepareUpdate(userDao.id);

        await _SqlClient.Updateable(userDao)
            .UpdateColumns(a => new { a.pass, a.update_time })
            .ExecuteCommandAsync();
    }
    #endregion

    #region 发送验证码
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    public async Task<SendSmsResponse> SendSmsAsync(SendSmsRequest request)
    {
        var types = SmsTypesEnum.Email;

        if (request.mode == ScmLoginModeEnum.ByPhone)
        {
            if (!TextUtils.IsCellphone(request.code))
            {
                throw new BusinessException("无效的手机号码！");
            }
            types = SmsTypesEnum.Phone;
        }
        else if (request.mode == ScmLoginModeEnum.ByEmail)
        {
            if (!TextUtils.IsEmail(request.code))
            {
                throw new BusinessException("无效的电子邮件！~");
            }
            types = SmsTypesEnum.Email;
        }
        else
        {
            throw new BusinessException("不支持的验证码方式！~");
        }

        var result = await _smsService.SendSmsAsync(types, request.code, request.req, "sms_login");
        if (result.HasError())
        {
            throw new BusinessException(result.Text);
        }

        var response = new SendSmsResponse();
        response.key = result.Dao.key;
        response.SetSuccess();
        return response;
    }
    #endregion

    #region 读取菜单
    /// <summary>
    /// 根据登录人ID查询权限菜单[SCUI]
    /// </summary>
    /// <param name="client">客户端</param>
    /// <param name="lang">语言：形如zh-CN、en等。</param>
    /// <returns></returns>
    public async Task<List<MenuDto>> GetAuthorityMenuAsync(ScmClientTypeEnum client, string lang)
    {
        var user = _jwtContextHolder.GetToken();

        if (string.IsNullOrWhiteSpace(lang))
        {
            lang = _EnvConfig.DefaultCulture;
        }

        //根据用户查询角色ID
        var userDao = await _SqlClient.Queryable<UserDao>()
            .FirstAsync(m => m.id == user.user_id && m.row_status == ScmRowStatusEnum.Enabled);

        var userRoleDao = await _SqlClient.Queryable<UserRoleDao>()
            .Where(a => a.user_id == user.user_id && a.row_status == ScmRowStatusEnum.Enabled)
            .ToListAsync();
        var userRoleIds = userRoleDao.Select(m => m.role_id).ToList();

        //根据角色查询授权的菜单Id集合
        var roleAuthList = await _SqlClient.Queryable<RoleAuthDao>()
            .Where(a => userRoleIds.Contains(a.role_id) && a.row_status == ScmRowStatusEnum.Enabled)
            .ToListAsync();
        roleAuthList = roleAuthList.DistinctBy(m => m.auth_id).ToList();

        #region 保存授权api
        //var apiList = new List<SysMenuApiUrl>();
        //foreach (var item in roleAuthList)
        //{
        //    apiList.AddRange(item.api);
        //}
        _CacheService.SetCache(KeyUtils.AUTHORIZZATIONAPI + ":" + userDao.id, "");
        #endregion

        //查询菜单集合
        var menuIds = roleAuthList.Select(m => m.auth_id).ToList();

        //根据菜单ID查询菜单详细
        var menuList = await _SqlClient.Queryable<Adm.Menu.AdmMenuDao>()
            .Where(a => menuIds.Contains(a.id) && a.row_status == ScmRowStatusEnum.Enabled)
            .WhereIF(client != ScmClientTypeEnum.None, a => a.client == client)
            .WhereIF(!string.IsNullOrWhiteSpace(lang), a => a.lang == lang)
            .OrderBy(a => a.od)
            .Select<MenuDto>()
            .ToListAsync();

        // 查询用户常用菜单
        await ListFavMenu(menuList, user.user_id);

        //return RecursiveModuleSc(menuList, 0);
        return menuList;
    }

    private async Task ListFavMenu(List<MenuDto> menuList, long userId)
    {
        var favMenuDao = menuList.Find(a => a.id == MenuDto.FAV_ID);
        if (favMenuDao == null)
        {
            return;
        }

        var favMenuList = await _SqlClient.Queryable<CfgMenuDao>()
            .Where(a => a.user_id == userId && a.row_status == ScmRowStatusEnum.Enabled)
            .ToListAsync();

        foreach (var favMenu in favMenuList)
        {
            var menuDto = menuList.Find(a => a.id == favMenu.menu_id);
            if (menuDto == null)
            {
                continue;
            }

            var favDao = menuDto.Adapt<MenuDto>();
            favDao.id = favMenu.id;
            favDao.pid = favMenuDao.id;
            menuList.Add(favDao);
        }
    }

    /// <summary>
    /// 递归模块列表
    /// </summary>
    /// <param name="menuList"></param>
    /// <param name="pId"></param>
    /// <returns></returns>
    private List<AuthorityDvo> RecursiveModuleSc(List<Adm.Menu.AdmMenuDao> menuList, long pId)
    {
        var result = new List<AuthorityDvo>();
        foreach (var item in menuList.Where(m => m.pid == pId).OrderBy(m => m.od))
        {
            var recursiveList = RecursiveModuleSc(menuList, item.id);
            result.Add(new AuthorityDvo()
            {
                id = item.id,
                path = item.uri,
                name = item.codec,
                component = item.view,
                meta = new AuthorityMeta()
                {
                    //id = item.id,
                    title = item.namec,
                    icon = item.icon,
                    type = item.types.ToKey().ToLower(),
                    hidden = !item.visible,
                    fullpage = item.fullpage ? true : null,
                    keepAlive = item.keepAlive,
                    affix = item.codec == "dashboard"
                },
                children = recursiveList
            });
        }

        return result;
    }
    #endregion
}