namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class DateThemeResponse
    {
        /// <summary>
        /// 主题名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 页面样式
        /// </summary>
        public PageTheme page { get; set; }

        /// <summary>
        /// 遮罩样式
        /// </summary>
        public MaskTheme mask { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PageTheme
    {
        /// <summary>
        /// 背景图片地址，格式：images/aaa.png
        /// </summary>
        public string backgroundImage { get; set; }
        /// <summary>
        /// 背景颜色，格式：#FFFFFF
        /// </summary>
        public string backgroundColor { get; set; }
        /// <summary>
        /// 背景大小
        /// </summary>
        public string backgroundSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string backgroundPosition { get; set; }
        /// <summary>
        /// 背景位置（X轴）
        /// </summary>
        public string backgroundPositionX { get; set; }
        /// <summary>
        /// 背景位置（Y轴）
        /// </summary>
        public string backgroundPositionY { get; set; }
        /// <summary>
        /// 背景重复方式
        /// </summary>
        public string backgroundRepeat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string backgroundOrigin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string backgroundClip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string backgroundAttachment { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MaskTheme
    {
        /// <summary>
        /// 前景遮罩颜色
        /// </summary>
        public string backgroundColor { get; set; }
    }
}
