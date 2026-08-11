using Com.Scm.Enums;
using Com.Scm.Utils;

namespace Com.Scm.Dao.User
{
    public class ScmUserDataDao : ScmUserDao, IStatusDao, ICreateDao
    {
        /// <summary>
        /// 状态
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

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            row_status = ScmRowStatusEnum.Enabled;

            create_user = userId;
            create_time = TimeUtils.GetUnixTime();

            update_user = userId;
            update_time = create_time;
        }

        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            update_user = userId;
            update_time = TimeUtils.GetUnixTime();
        }

        public bool IsNormal()
        {
            return row_status == ScmRowStatusEnum.Normal;
        }

        public bool IsEnabled()
        {
            return row_status == ScmRowStatusEnum.Enabled;
        }
    }
}
