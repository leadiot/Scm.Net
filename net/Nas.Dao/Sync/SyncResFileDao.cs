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

        /// <summary>
        /// 文件类型
        /// </summary>
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public NasTypeEnums type { get; set; }

        /// <summary>
        /// 二级类型
        /// </summary>
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public NasSubTypeEnums sub { get; set; }

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
        [SugarColumn(Length = 256)]
        public string name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string path { get; set; }

        /// <summary>
        /// 文档摘要
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string hash { get; set; }

        /// <summary>
        /// 文档大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required]
        public long ver { get; set; }

        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmBoolEnum p_delete { get; set; }
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmBoolEnum s_delete { get; set; }
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmBoolEnum is_delete { get; set; }

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
