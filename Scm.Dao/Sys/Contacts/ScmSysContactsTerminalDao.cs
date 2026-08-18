using Com.Scm.Dao;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;

namespace Com.Scm.Nas.App
{
    /// <summary>
    /// 设备通讯录表
    /// </summary>
    [SugarTable("scm_sys_contacts_terminal")]
    public class ScmSysContactsTerminalDao : ScmUserDataDao, IDeleteDao
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
        /// 其它附加参数
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public Dictionary<string, string> os_params { get; set; }

        public ScmRowDeleteEnum row_delete { get; set; }
    }
}
