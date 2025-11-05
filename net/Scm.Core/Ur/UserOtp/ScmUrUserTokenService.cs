using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Jwt;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Ur.UserOtp
{
    /// <summary>
    /// 三方登录服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Ur")]
    public class ScmUrUserTokenService : ApiService
    {
        private readonly JwtContextHolder _contextHolder;
        private readonly SugarRepository<UserDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextHolder"></param>
        /// <param name="userRepository"></param>
        /// <returns></returns>
        public ScmUrUserTokenService(JwtContextHolder contextHolder,
            SugarRepository<UserDao> userRepository)
        {
            _contextHolder = contextHolder;
            _thisRepository = userRepository;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<UserDto>()
                .FirstAsync();
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