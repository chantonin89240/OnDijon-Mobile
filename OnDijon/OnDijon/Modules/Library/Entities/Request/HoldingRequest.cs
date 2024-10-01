using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class HoldingRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("RecordId")]
        public string RecordId { get; set; }
    }
}
