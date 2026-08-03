using Com.Scm.Cache;
using Com.Scm.Filters;
using Com.Scm.Image.ImageSharp;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers
{
    /// <summary>
    /// Captcha
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "scm")]
    public class CaptchaController : ApiController
    {
        private ICacheService _CacheService;

        public CaptchaController(ICacheService cacheService)
        {
            _CacheService = cacheService;
        }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="identify">��ʶ��</param>
        /// <returns>ͼƬ����</returns>
        [HttpGet("cha/{identify}")]
        [NoJsonResult]
        [NoAuditLog]
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
        /// ����Key
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        [HttpGet("key/{identify}")]
        [NoJsonResult]
        [NoAuditLog]
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