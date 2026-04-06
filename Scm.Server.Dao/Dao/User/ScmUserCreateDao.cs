using Com.Scm.Utils;

namespace Com.Scm.Dao.User
{
    public class ScmUserCreateDao : ScmUserDao, ICreateDao
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public long create_user { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            create_user = userId;
            create_time = TimeUtils.GetUnixTime();
        }
    }
}