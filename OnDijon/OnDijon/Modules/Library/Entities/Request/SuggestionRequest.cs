using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class SuggestionRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("Search")]
        public string Search { get; set; }
    }
}
