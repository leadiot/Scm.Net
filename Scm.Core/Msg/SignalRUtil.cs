using Com.Scm.Server;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Com.Scm.Msg
{
    /// <summary>
    /// 
    /// </summary>
    public class SignalRUtil
    {
        private static JsonSerializerOptions _Options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obt"></param>
        /// <returns></returns>
        public static string ToJson(object obt)
        {
            if (_Options == null)
            {
                _Options = new JsonSerializerOptions();
                _Options.AllowTrailingCommas = false;
                _Options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                _Options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                _Options.Converters.Add(new SystemDateTimeJsonConverter());
                _Options.Converters.Add(new SystemLongJsonConverter());
                _Options.Converters.Add(new SystemTypeJsonConverter());
            }

            return JsonSerializer.Serialize(obt, _Options);
        }
    }
}
