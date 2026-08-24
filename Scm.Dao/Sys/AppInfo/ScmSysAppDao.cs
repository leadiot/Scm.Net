using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Sys.AppInfo
{
    /// <summary>
    /// 当前应用介绍信息
    /// </summary>
    [SugarTable("scm_sys_app")]
    public class ScmSysAppDao : ScmDataDao
    {
        /// <summary>
        /// 应用类型
        /// </summary>
        public int types { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        [SugarColumn(Length = 16, IsNullable = false)]
        public string code { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [SugarColumn(Length = 32, IsNullable = false)]
        public string name { get; set; }

        /// <summary>
        /// 精简介绍
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string slogan { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string project { get; set; }

        /// <summary>
        /// 网站地址
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string homepage { get; set; }

        /// <summary>
        /// 使用帮助
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string helppage { get; set; }

        /// <summary>
        /// 应用简介
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true)]
        public string content { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string email { get; set; }

        /// <summary>
        /// 群聊
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string qchat { get; set; }
    }
}
