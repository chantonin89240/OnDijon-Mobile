using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class SearchRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("QueryString")]
        public string QueryString { get; set; }

        [JsonProperty("PageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty("ResultSize")]
        public string ResultSize { get; set; }
    }
}
