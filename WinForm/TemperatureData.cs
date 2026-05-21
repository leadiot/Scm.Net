using Newtonsoft.Json;

namespace WinForm
{
    public class TemperatureData
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
