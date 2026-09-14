using Com.Scm.Dao;
using Com.Scm.Dao.Sync;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using Com.Scm.Sys.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Gtd
{
    /// <summary>
    /// 待办（头档）
    /// </summary>
    [SugarTable("scm_gtd_header")]
    public class GtdHeaderDao : ScmUserDataDao, IDeleteDao, ISyncDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(256)]
        public string title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string remark { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public ScmGtdPriorityEnum priority { get; set; }

        /// <summary>
        /// 提醒标识
        /// </summary>
        public ScmGtdRemindEnum remind { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string cron { get; set; }

        /// <summary>
        /// 提示方式
        /// </summary>
        public ScmGtdNoticeEnum notice { get; set; }

        /// <summary>
        /// 上次提醒时间
        /// </summary>
        public long last_time { get; set; }

        /// <summary>
        /// 下次提醒时间
        /// </summary>
        public long next_time { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 数据修改时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public ScmGtdHandleEnum handle { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public ScmRowDeleteEnum row_delete { get; set; } = ScmRowDeleteEnum.No;

        /// <summary>
        /// 同步时间
        /// </summary>
        public long sync_time { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            if (!ScmUtils.IsNormalId(terminal_id))
            {
                terminal_id = ScmEnv.DEFAULT_ID;
            }

            sync_time = update_time;
        }

        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            sync_time = update_time;
        }
    }
}