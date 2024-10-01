using Newtonsoft.Json;

namespace OnDijon.Modules.Rating.Entities.Request
{
    public class GetSessionRatingRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("EditId")]
        public string EditId { get; set; }
    }
}
