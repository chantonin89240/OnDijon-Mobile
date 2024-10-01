using Newtonsoft.Json;

namespace OnDijon.Common.Entities.Dto
{
    public class StatusMessage
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
