using Com.Scm.Cache;
using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Image.SkiaSharp;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Api.Controllers
{
    /// <summary>
    /// Captcha
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class CaptchaController : ApiController
    {
        private ICacheService _CacheService;

        public CaptchaController(ICacheService cacheService)
        {
            _CacheService = cacheService;
        }

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="identify">标识符</param>
        /// <returns>图片对象</returns>
        [HttpGet("cha/{identify}"), AllowAnonymous, NoJsonResult, NoAuditLog]
        public IActionResult Get(string identify)
        {
            if (string.IsNullOrEmpty(identify))
            {
                identify = ServerUtils.GetIp();
            }

            var captcha = new ImageEngine().GenCaptcha();
            _CacheService.SetCache(KeyUtils.CAPTCHACODE + identify, captcha.Value, 300);
            return File(captcha.Image, "image/png");
        }

        /// <summary>
        /// 生成Key
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        [HttpGet("key/{identify}"), AllowAnonymous, NoJsonResult, NoAuditLog]
        public IActionResult GetKey(string identify)
        {
            if (string.IsNullOrEmpty(identify))
            {
                identify = ServerUtils.GetIp();
            }

            _CacheService.SetCache(KeyUtils.CAPTCHACODE + identify, "", 300);
            return Content(identify);
        }
    }
}