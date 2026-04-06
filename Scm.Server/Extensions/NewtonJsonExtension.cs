using Com.Scm.Utils;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Com.Scm.Server;

public static class NewtonJsonExtension
{
    public static void NewtonJsonSetup(this IMvcBuilder builder)
    {
        builder.AddNewtonsoftJson(options =>
        {
            //修改属性名称的序列化方式，首字母小写，即驼峰样式
            //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //日期类型默认格式化处理 方式1
            options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = ScmEnv.FORMAT_DATETIME });
            options.SerializerSettings.Converters.Add(new NewtownLongJsonConverter());
            //日期类型默认格式化处理 方式2
            //options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            //options.SerializerSettings.DateFormatString = ScmEnv.FORMAT_DATETIME;

            //忽略循环引用
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            //解决命名不一致问题 
            //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

            //空值处理
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        });
    }
}

public class NewtownLongJsonConverter : JsonConverter<long>
{
    public override long ReadJson(JsonReader reader, Type objectType, long existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var val = reader.Value?.ToString();
        if (val != null && TextUtils.IsNumberic(val))
        {
            return long.Parse(val);
        }
        return hasExistingValue ? existingValue : 0;
    }

    public override void WriteJson(JsonWriter writer, long value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}
