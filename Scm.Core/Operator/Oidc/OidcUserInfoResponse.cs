namespace Com.Scm.Operator.Oidc
{
    public class OidcUserInfoResponse : OidcResponse
    {
        /// <summary>
        /// 用户信息（基于全局的用户信息）
        /// </summary>
        public OidcUserInfo User { get; set; }
    }

    public class OidcUserInfo
    {
        /// <summary>
        /// 服务代码
        /// </summary>
        public string Osp { get; set; }
        /// <summary>
        /// 用户代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 展示姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 获取绝对头像路径
        /// </summary>
        /// <returns></returns>
        public string GetAvatarUrl()
        {
            return "http://www.oidc.org.cn/data/avatar/" + Avatar;
        }
    }
}
