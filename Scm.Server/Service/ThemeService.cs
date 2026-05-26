using Com.Scm.Enums;
using Com.Scm.Server.Config;
using Com.Scm.Service;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Server.Service
{
    /// <summary>
    /// 主题服务 - 提供主题配置 API
    /// </summary>
    public class ThemeService : AppService
    {
        private readonly ThemeConfig _themeConfig;

        public ThemeService(ThemeConfig themeConfig)
        {
            _themeConfig = themeConfig ?? ThemeConfig.Default;
        }

        /// <summary>
        /// 获取当前主题配置
        /// </summary>
        [HttpGet]
        public AppTheme GetCurrentTheme()
        {
            return _themeConfig.GetCurrentTheme();
        }

        /// <summary>
        /// 获取所有可用主题列表
        /// </summary>
        [HttpGet]
        public List<ThemeOption> GetThemeOptions()
        {
            return AppThemes.GetOptions();
        }

        /// <summary>
        /// 根据标识获取主题详情
        /// </summary>
        [HttpGet("{key}")]
        public AppTheme GetThemeByKey(string key)
        {
            return AppThemes.GetByKey(key);
        }

        /// <summary>
        /// 获取主题 CSS 变量（前端直接注入）
        /// </summary>
        [HttpGet("css-vars")]
        public string GetCssVariables()
        {
            return _themeConfig.GetCurrentTheme().ToCssVariables();
        }

        /// <summary>
        /// 获取主题配置信息
        /// </summary>
        [HttpGet("config")]
        public ThemeConfigInfo GetConfig()
        {
            return new ThemeConfigInfo
            {
                DefaultTheme = _themeConfig.DefaultTheme,
                AllowUserCustom = _themeConfig.AllowUserCustom,
                SupportDarkMode = _themeConfig.SupportDarkMode
            };
        }
    }

    /// <summary>
    /// 主题配置信息（简化版）
    /// </summary>
    public class ThemeConfigInfo
    {
        /// <summary>
        /// 默认主题标识
        /// </summary>
        public string DefaultTheme { get; set; }
        
        /// <summary>
        /// 是否允许用户自定义
        /// </summary>
        public bool AllowUserCustom { get; set; }
        
        /// <summary>
        /// 是否支持暗黑模式
        /// </summary>
        public bool SupportDarkMode { get; set; }
    }
}