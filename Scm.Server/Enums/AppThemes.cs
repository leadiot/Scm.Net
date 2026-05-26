namespace Com.Scm.Enums
{
    /// <summary>
    /// 预设主题集合（类似 Android styles.xml）
    /// 提供多种主题供用户选择
    /// </summary>
    public static class AppThemes
    {
        /// <summary>
        /// 蓝色主题（Element UI 默认风格）
        /// </summary>
        public static AppTheme Blue => new AppTheme
        {
            Name = "科技蓝",
            Key = "blue",
            Primary = AppColors.PrimaryBlue,
            PrimaryLight = AppColors.PrimaryBlueLight,
            PrimaryDark = AppColors.PrimaryBlueDark,
            Secondary = "#909399",
            SecondaryLight = "#F4F4F5",
            SecondaryDark = "#606266",
            NavbarBackground = "#FFFFFF",
            NavbarText = "#303133",
            NavbarActiveBackground = "#ECF5FF",
            NavbarActiveText = "#409EFF",
            SidebarBackground = "#304156",
            SidebarText = "#BF CAD9",
            SidebarActiveBackground = "#409EFF",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 绿色主题（清新自然）
        /// </summary>
        public static AppTheme Green => new AppTheme
        {
            Name = "清新绿",
            Key = "green",
            Primary = AppColors.PrimaryGreen,
            PrimaryLight = AppColors.PrimaryGreenLight,
            PrimaryDark = AppColors.PrimaryGreenDark,
            Secondary = "#909399",
            SecondaryLight = "#F4F4F5",
            SecondaryDark = "#606266",
            NavbarBackground = "#FFFFFF",
            NavbarText = "#303133",
            NavbarActiveBackground = "#F0F9EB",
            NavbarActiveText = "#67C23A",
            SidebarBackground = "#2D3A4B",
            SidebarText = "#BF CAD9",
            SidebarActiveBackground = "#67C23A",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 紫色主题（优雅高贵）
        /// </summary>
        public static AppTheme Purple => new AppTheme
        {
            Name = "典雅紫",
            Key = "purple",
            Primary = AppColors.PrimaryPurple,
            PrimaryLight = AppColors.PrimaryPurpleLight,
            PrimaryDark = AppColors.PrimaryPurpleDark,
            Secondary = "#909399",
            SecondaryLight = "#F4F4F5",
            SecondaryDark = "#606266",
            NavbarBackground = "#FFFFFF",
            NavbarText = "#303133",
            NavbarActiveBackground = "#F3E5F5",
            NavbarActiveText = "#9B59B6",
            SidebarBackground = "#3A3A5C",
            SidebarText = "#C8C8DC",
            SidebarActiveBackground = "#9B59B6",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 橙色主题（活力温暖）
        /// </summary>
        public static AppTheme Orange => new AppTheme
        {
            Name = "活力橙",
            Key = "orange",
            Primary = AppColors.PrimaryOrange,
            PrimaryLight = AppColors.PrimaryOrangeLight,
            PrimaryDark = AppColors.PrimaryOrangeDark,
            Secondary = "#909399",
            SecondaryLight = "#F4F4F5",
            SecondaryDark = "#606266",
            NavbarBackground = "#FFFFFF",
            NavbarText = "#303133",
            NavbarActiveBackground = "#FDF6EC",
            NavbarActiveText = "#E6A23C",
            SidebarBackground = "#4A4A5A",
            SidebarText = "#C8C8D8",
            SidebarActiveBackground = "#E6A23C",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 红色主题（热情奔放）
        /// </summary>
        public static AppTheme Red => new AppTheme
        {
            Name = "热情红",
            Key = "red",
            Primary = AppColors.PrimaryRed,
            PrimaryLight = AppColors.PrimaryRedLight,
            PrimaryDark = AppColors.PrimaryRedDark,
            Secondary = "#909399",
            SecondaryLight = "#F4F4F5",
            SecondaryDark = "#606266",
            NavbarBackground = "#FFFFFF",
            NavbarText = "#303133",
            NavbarActiveBackground = "#FEF0F0",
            NavbarActiveText = "#F56C6C",
            SidebarBackground = "#4A3A4A",
            SidebarText = "#D8C8D8",
            SidebarActiveBackground = "#F56C6C",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 青色主题（冷静专业）
        /// </summary>
        public static AppTheme Cyan => new AppTheme
        {
            Name = "专业青",
            Key = "cyan",
            Primary = AppColors.PrimaryCyan,
            PrimaryLight = AppColors.PrimaryCyanLight,
            PrimaryDark = AppColors.PrimaryCyanDark,
            Secondary = "#909399",
            SecondaryLight = "#F4F4F5",
            SecondaryDark = "#606266",
            NavbarBackground = "#FFFFFF",
            NavbarText = "#303133",
            NavbarActiveBackground = "#E0F7FA",
            NavbarActiveText = "#00BCD4",
            SidebarBackground = "#2A3A4A",
            SidebarText = "#B8C8D8",
            SidebarActiveBackground = "#00BCD4",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 粉色主题（柔美浪漫）
        /// </summary>
        public static AppTheme Pink => new AppTheme
        {
            Name = "浪漫粉",
            Key = "pink",
            Primary = AppColors.PrimaryPink,
            PrimaryLight = AppColors.PrimaryPinkLight,
            PrimaryDark = AppColors.PrimaryPinkDark,
            Secondary = "#909399",
            SecondaryLight = "#F4F4F5",
            SecondaryDark = "#606266",
            NavbarBackground = "#FFFFFF",
            NavbarText = "#303133",
            NavbarActiveBackground = "#FCE4EC",
            NavbarActiveText = "#E91E63",
            SidebarBackground = "#4A3A5A",
            SidebarText = "#D8C8E8",
            SidebarActiveBackground = "#E91E63",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 深色主题（Dark Mode）
        /// </summary>
        public static AppTheme Dark => new AppTheme
        {
            Name = "暗黑模式",
            Key = "dark",
            Primary = "#409EFF",
            PrimaryLight = "#337ECC",
            PrimaryDark = "#66B1FF",
            Secondary = "#909399",
            SecondaryLight = "#606266",
            SecondaryDark = "#C0C4CC",
            Success = "#67C23A",
            SuccessLight = "#529B2E",
            Warning = "#E6A23C",
            WarningLight = "#CF9236",
            Error = "#F56C6C",
            ErrorLight = "#DD6161",
            Info = "#909399",
            InfoLight = "#606266",
            TextPrimary = "#E5EAF3",
            TextRegular = "#CFD3DC",
            TextSecondary = "#A3A6AD",
            TextPlaceholder = "#8D9095",
            TextDisabled = "#6C6E72",
            BorderBase = "#4C4D4F",
            BorderLight = "#414243",
            BorderDark = "#585859",
            BorderPrimary = "#409EFF",
            BackgroundPage = "#1D1E1F",
            BackgroundComponent = "#252627",
            BackgroundMask = "#00000080",
            BackgroundHover = "#3A3B3C",
            NavbarBackground = "#141414",
            NavbarText = "#E5EAF3",
            NavbarActiveBackground = "#337ECC",
            NavbarActiveText = "#66B1FF",
            SidebarBackground = "#1D1E1F",
            SidebarText = "#A3A6AD",
            SidebarActiveBackground = "#409EFF",
            SidebarActiveText = "#FFFFFF"
        };

        /// <summary>
        /// 所有预设主题列表
        /// </summary>
        public static List<AppTheme> All => new List<AppTheme>
        {
            Blue,
            Green,
            Purple,
            Orange,
            Red,
            Cyan,
            Pink,
            Dark
        };

        /// <summary>
        /// 根据标识获取主题
        /// </summary>
        public static AppTheme GetByKey(string key)
        {
            return All.FirstOrDefault(t => t.Key == key) ?? Blue;
        }

        /// <summary>
        /// 获取主题名称列表（用于前端下拉选择）
        /// </summary>
        public static List<ThemeOption> GetOptions()
        {
            return All.Select(t => new ThemeOption
            {
                Key = t.Key,
                Name = t.Name,
                Primary = t.Primary
            }).ToList();
        }
    }

    /// <summary>
    /// 主题选项（简化版，用于下拉列表）
    /// </summary>
    public class ThemeOption
    {
        /// <summary>
        /// 主题标识
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// 主题名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 主色调（用于预览）
        /// </summary>
        public string Primary { get; set; }
    }
}