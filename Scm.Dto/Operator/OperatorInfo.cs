using Com.Scm.Dto;
using Com.Scm.Enums;

namespace Com.Scm.Operator
{
    /// <summary>
    /// 基础用户信息
    /// </summary>
    public class OperatorInfo : ScmDataDto
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 暂未使用
        /// </summary>
        public ScmUserTypesEnum Type { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 人员代码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 人员名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 权限（暂未使用）
        /// </summary>
        public List<string> Roles { get; set; } = new() { "admin" };

        public bool IsLogined()
        {
            return UserId > ScmEnv.DEFAULT_ID;
        }
    }
}
