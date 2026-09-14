using Com.Scm.Enums;

namespace Com.Scm.Dao.Terminal
{
    public class ScmTerminalDataDao : ScmTerminalDao, IStatusDao, ICreateDao, IUpdateDao, IDeleteDao
    {
        /// <summary>
        /// 删除标记
        /// </summary>
        public ScmRowDeleteEnum row_delete { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public ScmRowStatusEnum row_status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public long create_user { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long update_time { get; set; }

        /// <summary>
        /// 更新人员
        /// </summary>
        public long update_user { get; set; }
    }
}
