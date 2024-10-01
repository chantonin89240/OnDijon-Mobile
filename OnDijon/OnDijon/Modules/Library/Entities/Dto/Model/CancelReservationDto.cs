using Newtonsoft.Json;
using static OnDijon.Common.Entities.Utils;

namespace OnDijon.Modules.Library.Entities.Dto.Model
{
    public class CancelReservationDto
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
        [JsonConverter(typeof(CustomBoolConverter))]
        public bool IsCanceled { get; set; }

        [JsonProperty("NotCanceledReason")]
        public string NotCanceledReason { get; set; }

        [JsonProperty("ReservationId")]
        public string ReservationId { get; set; }
    }
}
