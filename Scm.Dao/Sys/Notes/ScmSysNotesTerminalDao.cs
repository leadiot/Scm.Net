using Com.Scm.Dao;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;

namespace Com.Scm.Nas.App
{
    /// <summary>
    /// 设备便签表
    /// </summary>
    [SugarTable("scm_sys_notes_terminal")]
    public class ScmSysNotesTerminalDao : ScmUserDataDao, IDeleteDao
    {
        /// <summary>
        /// 通讯录ID
        /// </summary>
        public long sys_id { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 其它附加参数
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public Dictionary<string, string> os_params { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScmRowDeleteEnum row_delete { get; set; }
    }
}
