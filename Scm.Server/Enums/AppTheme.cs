namespace Com.Scm.Enums
{
    /// <summary>
    /// 应用主题定义（类似 Android themes.xml）
    /// 包含主色调、辅色调、背景色等完整主题配置
    /// </summary>
    public class AppTheme
    {
        /// <summary>
        /// 主题名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主题标识（用于前端切换）
        /// </summary>
        public string Key { get; set; }

        #region 主色调
        
        /// <summary>
        /// 主色调（Primary Color）- 用于按钮、链接、高亮等
        /// </summary>
        public string Primary { get; set; } = AppColors.PrimaryBlue;
        
        /// <summary>
        /// 主色调浅色（用于 hover 背景）
        /// </summary>
        public string PrimaryLight { get; set; } = AppColors.PrimaryBlueLight;
        
        /// <summary>
        /// 主色调深色（用于 active 状态）
        /// </summary>
        public string PrimaryDark { get; set; } = AppColors.PrimaryBlueDark;
        
        #endregion

        #region 辅色调
        
        /// <summary>
        /// 辅色调（Secondary/Accent Color）- 用于次要按钮、标签等
        /// </summary>
        public string Secondary { get; set; } = AppColors.GrayDark;
        
        /// <summary>
        /// 辅色调浅色
        /// </summary>
        public string SecondaryLight { get; set; } = AppColors.GrayLight;
        
        /// <summary>
        /// 辅色调深色
        /// </summary>
        public string SecondaryDark { get; set; } = AppColors.GrayDarkest;
        
        #endregion

        #region 功能色
        
        /// <summary>
        /// 成功色
        /// </summary>
        public string Success { get; set; } = AppColors.Success;
        
        /// <summary>
        /// 成功色浅色
        /// </summary>
        public string SuccessLight { get; set; } = AppColors.SuccessLight;
        
        /// <summary>
        /// 警告色
        /// </summary>
        public string Warning { get; set; } = AppColors.Warning;
        
        /// <summary>
        /// 警告色浅色
        /// </summary>
        public string WarningLight { get; set; } = AppColors.WarningLight;
        
        /// <summary>
        /// 错误色
        /// </summary>
        public string Error { get; set; } = AppColors.Error;
        
        /// <summary>
        /// 错误色浅色
        /// </summary>
        public string ErrorLight { get; set; } = AppColors.ErrorLight;
        
        /// <summary>
        /// 信息色
        /// </summary>
        public string Info { get; set; } = AppColors.Info;
        
        /// <summary>
        /// 信息色浅色
        /// </summary>
        public string InfoLight { get; set; } = AppColors.InfoLight;
        
        #endregion

        #region 文字颜色
        
        /// <summary>
        /// 主文字颜色
        /// </summary>
        public string TextPrimary { get; set; } = AppColors.GrayDarkest;
        
        /// <summary>
        /// 普通文字颜色
        /// </summary>
        public string TextRegular { get; set; } = AppColors.GrayDarker;
        
        /// <summary>
        /// 次要文字颜色
        /// </summary>
        public string TextSecondary { get; set; } = AppColors.GrayDark;
        
        /// <summary>
        /// 占位文字颜色
        /// </summary>
        public string TextPlaceholder { get; set; } = AppColors.GrayMedium;
        
        /// <summary>
        /// 禁用文字颜色
        /// </summary>
        public string TextDisabled { get; set; } = AppColors.GrayMedium;
        
        #endregion

        #region 边框颜色
        
        /// <summary>
        /// 基础边框颜色
        /// </summary>
        public string BorderBase { get; set; } = AppColors.Gray;
        
        /// <summary>
        /// 浅边框颜色
        /// </summary>
        public string BorderLight { get; set; } = AppColors.GrayLight;
        
        /// <summary>
        /// 深边框颜色
        /// </summary>
        public string BorderDark { get; set; } = AppColors.GrayDark;
        
        /// <summary>
        /// 主色调边框（聚焦时）
        /// </summary>
        public string BorderPrimary { get; set; } = AppColors.PrimaryBlue;
        
        #endregion

        #region 背景颜色
        
        /// <summary>
        /// 页面背景色
        /// </summary>
        public string BackgroundPage { get; set; } = AppColors.GrayLight;
        
        /// <summary>
        /// 组件背景色
        /// </summary>
        public string BackgroundComponent { get; set; } = AppColors.White;
        
        /// <summary>
        /// 遮罩背景色
        /// </summary>
        public string BackgroundMask { get; set; } = AppColors.BlackTransparent;
        
        /// <summary>
        /// hover 背景色
        /// </summary>
        public string BackgroundHover { get; set; } = AppColors.GrayLight;
        
        #endregion

        #region 导航颜色
        
        /// <summary>
        /// 导航栏背景色
        /// </summary>
        public string NavbarBackground { get; set; } = AppColors.White;
        
        /// <summary>
        /// 导航栏文字色
        /// </summary>
        public string NavbarText { get; set; } = AppColors.GrayDarkest;
        
        /// <summary>
        /// 导航栏激活项背景色
        /// </summary>
        public string NavbarActiveBackground { get; set; } = AppColors.PrimaryBlueLight;
        
        /// <summary>
        /// 导航栏激活项文字色
        /// </summary>
        public string NavbarActiveText { get; set; } = AppColors.PrimaryBlue;
        
        #endregion

        #region 侧边栏颜色
        
        /// <summary>
        /// 侧边栏背景色
        /// </summary>
        public string SidebarBackground { get; set; } = AppColors.GrayDarkest;
        
        /// <summary>
        /// 侧边栏文字色
        /// </summary>
        public string SidebarText { get; set; } = AppColors.GrayLight;
        
        /// <summary>
        /// 侧边栏激活项背景色
        /// </summary>
        public string SidebarActiveBackground { get; set; } = AppColors.PrimaryBlue;
        
        /// <summary>
        /// 侧边栏激活项文字色
        /// </summary>
        public string SidebarActiveText { get; set; } = AppColors.White;
        
        #endregion

        /// <summary>
        /// 获取主题 CSS 变量（用于前端）
        /// </summary>
        public string ToCssVariables()
        {
            return $@"
--color-primary: {Primary};
--color-primary-light: {PrimaryLight};
--color-primary-dark: {PrimaryDark};
--color-secondary: {Secondary};
--color-secondary-light: {SecondaryLight};
--color-secondary-dark: {SecondaryDark};
--color-success: {Success};
--color-success-light: {SuccessLight};
--color-warning: {Warning};
--color-warning-light: {WarningLight};
--color-error: {Error};
--color-error-light: {ErrorLight};
--color-info: {Info};
--color-info-light: {InfoLight};
--text-primary: {TextPrimary};
--text-regular: {TextRegular};
--text-secondary: {TextSecondary};
--text-placeholder: {TextPlaceholder};
--text-disabled: {TextDisabled};
--border-base: {BorderBase};
--border-light: {BorderLight};
--border-dark: {BorderDark};
--border-primary: {BorderPrimary};
--bg-page: {BackgroundPage};
--bg-component: {BackgroundComponent};
--bg-mask: {BackgroundMask};
--bg-hover: {BackgroundHover};
--navbar-bg: {NavbarBackground};
--navbar-text: {NavbarText};
--navbar-active-bg: {NavbarActiveBackground};
--navbar-active-text: {NavbarActiveText};
--sidebar-bg: {SidebarBackground};
--sidebar-text: {SidebarText};
--sidebar-active-bg: {SidebarActiveBackground};
--sidebar-active-text: {SidebarActiveText};
";
        }
    }
}