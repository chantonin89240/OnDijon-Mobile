using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class AutoConnectUrlRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("UidUser")]
        public string UidUser { get; set; }

        [JsonProperty("Host")]
        public string Host { get; set; }
    }
}
