using Newtonsoft.Json;

namespace OnDijon.Modules.CustomContent.Entities
{
    public class CustomContentRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("EditId")]
        public string EditId { get; set; }
    }
}
