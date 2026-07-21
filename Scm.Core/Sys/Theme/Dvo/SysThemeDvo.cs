using Com.Scm.Dvo;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class SysThemeDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string theme { get; set; }
    }

    public class ThemeDvo
    {
        public string name { get; set; }
        public string backgroundType { get; set; }
        public string backgroundImage { get; set; }
        public string iconsPath { get; set; }
        public string startLogo { get; set; }
        public string gradientDirection { get; set; }
        public string taskbarColor { get; set; }
        public string taskbarTextColor { get; set; }
        public string menuColor { get; set; }
        public string menuTextColor { get; set; }
        public string iconColor { get; set; }

        public WindowTheme windowTheme { get; set; }
    }

    public class WindowTheme
    {
        public string bg { get; set; }
        public string headerBg { get; set; }
        public string titleColor { get; set; }
        public string titleColorInactive { get; set; }
        public string border { get; set; }
        public string borderRadius { get; set; }
        public string shadow { get; set; }
        public string closeBtnHover { get; set; }
        public string minBtnHover { get; set; }
        public string maxBtnHover { get; set; }
        public string titleHeight { get; set; }
        public string buttonWidth { get; set; }

        public WindowAnimations animations { get; set; }
    }

    public class WindowAnimations
    {
        public string open { get; set; }
        public string openEnd { get; set; }
        public string openDuration { get; set; }
        public string openTiming { get; set; }
        public string close { get; set; }
        public string closeEnd { get; set; }
        public string closeDuration { get; set; }
        public string closeTiming { get; set; }
        public string minimize { get; set; }
        public string minimizeEnd { get; set; }
        public string minimizeDuration { get; set; }
        public string minimizeTiming { get; set; }
        public string maximize { get; set; }
        public string maximizeEnd { get; set; }
        public string maximizeDuration { get; set; }
        public string maximizeTiming { get; set; }
        public string focus { get; set; }
        public string focusEnd { get; set; }
        public string focusDuration { get; set; }
        public string focusTiming { get; set; }
    }
}