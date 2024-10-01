using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
    public class ReservationRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("EditId")]
        public string EditId { get; set; }
    }
}
