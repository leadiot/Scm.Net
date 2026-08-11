using Com.Scm.Dvo;

namespace Com.Scm.Sys.I18n.Dvo
{
    public class ScmSysI18nDvo : ScmDvo
    {
        /// <summary>
        /// 所属模块：menu/dic/uom/error/field 等
        /// </summary>
        public string module { get; set; }

        /// <summary>
        /// 翻译键，命名规范 {module}.{business}.{field}
        /// 示例：menu.sys.user.name
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 语言代码：zh-cn / en / ja 等
        /// </summary>
        public string lang { get; set; }

        /// <summary>
        /// 翻译文本
        /// </summary>
        public string value { get; set; }
    }
}
