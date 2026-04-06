using Com.Scm.Ur;
using Microsoft.AspNetCore.Hosting;

namespace Com.Scm.Config
{
    public class DataConfig
    {
        public const string NAME = "Data";

        /// <summary>
        /// 共享用户数据
        /// </summary>
        public long[] ShareUserIds { get; set; }

        public void Prepare(IWebHostEnvironment environment)
        {
            if (ShareUserIds == null || ShareUserIds.Length == 0)
            {
                ShareUserIds = new long[1] { UserDto.SYS_ID };
            }
        }
    }
}
