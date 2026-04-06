namespace Com.Scm.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitMQConfig
    {
        /// <summary>
        /// MQ安装的实际服务器IP地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 服务端口号
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Exchange名称
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// 是否启用持久化
        /// </summary>
        public bool Durable { get; set; }

        public static RabbitMQConfig Default
        {
            get
            {
                return new RabbitMQConfig { Host = "localhost", Port = 5672, UserName = "", Password = "", Durable = true };
            }
        }
    }
}
