using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class LoanRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("EditId")]
        public string EditId { get; set; }
    }
}
