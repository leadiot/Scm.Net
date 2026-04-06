namespace Com.Scm.Hubs
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class ClientUser
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 连接编号
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 应用编号
        /// </summary>
        public long AppId { get; set; }

        /// <summary>
        /// 登录人
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
