namespace Com.Scm.Config
{
    public class WebConfig
    {
        public const string NAME = "Web";

        /// <summary>
        /// 网站名称（冗余）
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 网站地址（冗余）
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// 网站标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 网站标语
        /// </summary>
        public string Slogan { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string MetaKeyWords { get; set; }
        /// <summary>
        /// 网站描述
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// 版权信息
        /// </summary>
        public string CopyRight { get; set; }
        /// <summary>
        /// 备案信息
        /// </summary>
        public string Beian { get; set; }

        /// <summary>
        /// 外部样式
        /// </summary>
        public string Styles { get; set; }
        /// <summary>
        /// 外部脚本
        /// </summary>
        public string Scripts { get; set; }

        /// <summary>
        /// 其它信息
        /// </summary>
        public string Custom { get; set; }

        public void Prepare(EnvConfig config)
        {
            if (string.IsNullOrWhiteSpace(SiteName))
            {
                SiteName = "Scm";
            }

            var copyright = CopyRight ?? "&copy; {year} - " + SiteName;
            CopyRight = copyright.Replace("{year}", DateTime.Now.Year.ToString());
        }
    }
}
