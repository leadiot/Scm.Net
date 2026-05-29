using Com.Scm.Phone.Config;

namespace Com.Scm.Utils
{
    public class PhoneHelper
    {
        public static AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient(PhoneConfig config)
        {
            // 工程代码泄露可能会导致 AccessKey 泄露，并威胁账号下所有资源的安全性。以下代码示例仅供参考。
            // 建议使用更安全的 STS 方式，更多鉴权访问方式请参见：https://help.aliyun.com/document_detail/378671.html。
            AlibabaCloud.OpenApiClient.Models.Config aliyunConfig = new AlibabaCloud.OpenApiClient.Models.Config
            {
                // 必填，请确保代码运行环境设置了环境变量 ALIBABA_CLOUD_ACCESS_KEY_ID。
                AccessKeyId = config.Username,
                // 必填，请确保代码运行环境设置了环境变量 ALIBABA_CLOUD_ACCESS_KEY_SECRET。
                AccessKeySecret = config.Password,
            };
            // Endpoint 请参考 https://api.aliyun.com/product/Dysmsapi
            aliyunConfig.Endpoint = "dysmsapi.aliyuncs.com";
            return new AlibabaCloud.SDK.Dysmsapi20170525.Client(aliyunConfig);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="phone"></param>
        /// <param name="sms"></param>
        /// <returns></returns>
        public static bool SendPhone(PhoneConfig config, string phone, string sms)
        {
            AlibabaCloud.SDK.Dysmsapi20170525.Client client = CreateClient(config);
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                PhoneNumbers = phone,
                SignName = config.SignName,
                TemplateCode = config.TemplateCode,
                TemplateParam = config.TemplateData.Replace("$sms$", sms)
            };

            // 复制代码运行请自行打印 API 的返回值
            client.SendSmsWithOptions(sendSmsRequest, new AlibabaCloud.TeaUtil.Models.RuntimeOptions());

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="phone"></param>
        /// <param name="sms"></param>
        /// <returns></returns>
        public static async Task<bool> SendPhoneAsync(PhoneConfig config, string phone, string sms)
        {
            AlibabaCloud.SDK.Dysmsapi20170525.Client client = CreateClient(config);
            AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest sendSmsRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
            {
                PhoneNumbers = phone,
                SignName = config.SignName,
                TemplateCode = config.TemplateCode,
                TemplateParam = config.TemplateData.Replace("$sms$", sms)
            };

            // 复制代码运行请自行打印 API 的返回值
            await client.SendSmsWithOptionsAsync(sendSmsRequest, new AlibabaCloud.TeaUtil.Models.RuntimeOptions());

            return true;
        }
    }
}
