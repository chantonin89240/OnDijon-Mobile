using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Request
{
   public class CancelReservationRequest
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("EditId")]
        public string EditId { get; set; }

        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("IdReservations")]
        public string IdReservations { get; set; }
    }
}
