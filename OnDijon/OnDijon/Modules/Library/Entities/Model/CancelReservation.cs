using Newtonsoft.Json;

namespace OnDijon.Modules.Library.Entities.Model
{
    public class CancelReservation
    {
        [JsonProperty("IdBorrower")]
        public string IdBorrower { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("ErrorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("IsCanceled")]
        public bool IsCanceled { get; set; }

        [JsonProperty("NotCanceledReason")]
        public string NotCanceledReason { get; set; }

        [JsonProperty("ReservationId")]
        public string ReservationId { get; set; }


    }
}
