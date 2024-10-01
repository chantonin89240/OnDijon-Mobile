using Newtonsoft.Json;
using OnDijon.Common.Entities.Dto;
using OnDijon.Modules.Library.Entities.Dto.Model;

namespace OnDijon.Modules.Library.Entities.Dto
{
    public class CancelReservationListDto : WsDMDto
    {
        [JsonProperty("CancelReservation")]
        public CancelReservationDto CancelReservation { get; set; }
    }
}
