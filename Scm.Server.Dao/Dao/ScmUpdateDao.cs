using Com.Scm.Utils;

namespace Com.Scm.Dao
{
    public class ScmUpdateDao : ScmDao, IUpdateDao
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public long update_time { get; set; }

        /// <summary>
        /// 更新人员
        /// </summary>
        public long update_user { get; set; }

        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            update_user = userId;
            update_time = TimeUtils.GetUnixTime();
        }
    }
}
