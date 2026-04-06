using Com.Scm.Dto;

namespace Com.Scm.Log
{
    public class LogHbDto : ScmDto
    {
        /// <summary>
        /// Device
        /// </summary>
        public const int TYPE_1 = 1;
        /// <summary>
        /// Service
        /// </summary>
        public const int TYPE_2 = 2;

        /// <summary>
        /// 终端类型
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 网络地址
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// 主机名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 心跳时间
        /// </summary>
        public long time { get; set; }
    }
}
