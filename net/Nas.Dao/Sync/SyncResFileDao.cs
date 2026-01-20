using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Sync
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [SugarTable("nas_res_file")]
    public class SyncResFileDao : ScmDataDao
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public long user_id { get; set; }

        ///// <summary>
        ///// 终端ID
        ///// </summary>
        //[Required]
        //public long terminal_id { get; set; }

        ///// <summary>
        ///// 驱动ID
        ///// </summary>
        //[Required]
        //public long folder_id { get; set; }


        /// <summary>
        /// 文件类型
        /// </summary>
        public NasTypeEnums type { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        [Required]
        public long dir_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [StringLength(2048)]
        public string path { get; set; }

        /// <summary>
        /// 文档摘要
        /// </summary>
        [StringLength(64)]
        public string hash { get; set; }

        /// <summary>
        /// 文档大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long modify_time { get; set; }

        public ScmBoolEnum p_delete { get; set; }
        public ScmBoolEnum s_delete { get; set; }
        public ScmBoolEnum is_delete { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required]
        public long ver { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            if (string.IsNullOrEmpty(hash))
            {
                hash = "";
            }

            p_delete = ScmBoolEnum.False;
            s_delete = ScmBoolEnum.False;
            is_delete = ScmBoolEnum.False;

            ver = 1;
            row_status = ScmRowStatusEnum.Enabled;

            update_time = TimeUtils.GetUnixTime();
            create_time = update_time;
        }

        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);

            if (string.IsNullOrEmpty(hash))
            {
                hash = "";
            }

            ver += 1;
            update_time = TimeUtils.GetUnixTime();
        }
    }
}
