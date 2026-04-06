namespace Com.Scm.Dao.User
{
    public class ScmUserDao : ScmDao, IUserDao
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public long user_id { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            user_id = userId;
        }

        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);
        }
    }
}
