using Com.Scm.Config;
using Com.Scm.Enums;

namespace Com.Scm.Server.Config
{
    /// <summary>
    /// 主题配置（支持 appsettings.json 配置）
    /// </summary>
    public class ThemeConfig : EnvConfig
    {
        /// <summary>
                /// 主题配置节点名称
                /// </summary>
                public new const string NAME = "Theme";

        /// <summary>
        /// 默认主题标识（blue/green/purple/orange/red/cyan/pink/dark）
        /// </summary>
        public string DefaultTheme { get; set; } = "blue";

        /// <summary>
        /// 是否允许用户自定义主题
        /// </summary>
        public bool AllowUserCustom { get; set; } = true;

        /// <summary>
        /// 是否支持暗黑模式切换
        /// </summary>
        public bool SupportDarkMode { get; set; } = true;

        /// <summary>
        /// 自定义主题配置（覆盖预设主题）
        /// </summary>
        public AppTheme CustomTheme { get; set; }

        /// <summary>
        /// 准备配置
        /// </summary>
        public void Prepare(EnvConfig envConfig)
        {
            // 验证默认主题标识
            if (string.IsNullOrWhiteSpace(DefaultTheme))
            {
                DefaultTheme = "blue";
            }
        }

        /// <summary>
        /// 获取当前主题
        /// </summary>
        public AppTheme GetCurrentTheme()
        {
            // 如果有自定义主题，优先使用
            if (CustomTheme != null && !string.IsNullOrWhiteSpace(CustomTheme.Primary))
            {
                return CustomTheme;
            }

            // 使用预设主题
            return AppThemes.GetByKey(DefaultTheme);
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        public static ThemeConfig Default => new ThemeConfig
        {
            DefaultTheme = "blue",
            AllowUserCustom = true,
            SupportDarkMode = true
        };
    }
}